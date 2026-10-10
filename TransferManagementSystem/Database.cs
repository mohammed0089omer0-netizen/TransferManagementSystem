using Microsoft.Data.Sqlite;
using System;
using System.IO;

namespace TransferManagementSystem
{
    public static class Database
    {
        private static readonly string DatabasePath =
            Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "transfers.db"
            );

        private static readonly string ConnectionString =
            $"Data Source={DatabasePath}";

        public static void Initialize()
        {
            using (var connection =
                new SqliteConnection(ConnectionString))
            {
                connection.Open();

                string transfersSql = @"
                    CREATE TABLE IF NOT EXISTS Transfers
                    (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        TransferDate TEXT NOT NULL,
                        Beneficiary TEXT NOT NULL,
                        Amount REAL NOT NULL,
                        ReferenceNumber TEXT,
                        Description TEXT,
                        Category TEXT,
                        Bank TEXT,
                        ReceiptImage TEXT
                    );
                ";

                using (var command =
                    new SqliteCommand(
                        transfersSql,
                        connection))
                {
                    command.ExecuteNonQuery();
                }

                string categoriesSql = @"
                    CREATE TABLE IF NOT EXISTS Categories
                    (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Name TEXT NOT NULL UNIQUE
                    );
                ";

                using (var command =
                    new SqliteCommand(
                        categoriesSql,
                        connection))
                {
                    command.ExecuteNonQuery();
                }

                string insertDefaultsSql = @"
                    INSERT OR IGNORE INTO Categories (Name)
                    VALUES
                    ('المواد'),
                    ('العمال'),
                    ('أخرى');
                ";

                using (var command =
                    new SqliteCommand(
                        insertDefaultsSql,
                        connection))
                {
                    command.ExecuteNonQuery();
                }

                string usersSql = @"
                    CREATE TABLE IF NOT EXISTS Users
                    (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Username TEXT NOT NULL UNIQUE,
                        Password TEXT NOT NULL,
                        RecoveryCode TEXT NOT NULL DEFAULT '2468'
                    );
                ";

                using (var command =
                    new SqliteCommand(
                        usersSql,
                        connection))
                {
                    command.ExecuteNonQuery();
                }

                EnsureRecoveryCodeColumn(connection);

                string checkUserSql =
                    "SELECT COUNT(*) FROM Users;";

                int userCount = 0;

                using (var command =
                    new SqliteCommand(
                        checkUserSql,
                        connection))
                {
                    userCount =
                        Convert.ToInt32(
                            command.ExecuteScalar()
                        );
                }

                if (userCount == 0)
                {
                    string insertUserSql = @"
                        INSERT INTO Users
                        (
                            Username,
                            Password,
                            RecoveryCode
                        )
                        VALUES
                        (
                            'admin',
                            '1234',
                            '2468'
                        );
                    ";

                    using (var command =
                        new SqliteCommand(
                            insertUserSql,
                            connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }
                else
                {
                    string fixRecoveryCodeSql = @"
                        UPDATE Users
                        SET RecoveryCode = '2468'
                        WHERE RecoveryCode IS NULL
                           OR TRIM(RecoveryCode) = '';
                    ";

                    using (var command =
                        new SqliteCommand(
                            fixRecoveryCodeSql,
                            connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }
            }
        }

        private static void EnsureRecoveryCodeColumn(
            SqliteConnection connection)
        {
            bool exists = false;

            string checkColumnSql = @"
                PRAGMA table_info(Users);
            ";

            using (var command =
                new SqliteCommand(
                    checkColumnSql,
                    connection))
            {
                using (var reader =
                    command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string columnName =
                            reader["name"]?.ToString() ?? "";

                        if (columnName.Equals(
                            "RecoveryCode",
                            StringComparison.OrdinalIgnoreCase))
                        {
                            exists = true;
                            break;
                        }
                    }
                }
            }

            if (!exists)
            {
                string addColumnSql = @"
                    ALTER TABLE Users
                    ADD COLUMN RecoveryCode TEXT;
                ";

                using (var command =
                    new SqliteCommand(
                        addColumnSql,
                        connection))
                {
                    command.ExecuteNonQuery();
                }

                string updateColumnSql = @"
                    UPDATE Users
                    SET RecoveryCode = '2468'
                    WHERE RecoveryCode IS NULL
                       OR TRIM(RecoveryCode) = '';
                ";

                using (var command =
                    new SqliteCommand(
                        updateColumnSql,
                        connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        public static SqliteConnection GetConnection()
        {
            var connection =
                new SqliteConnection(
                    ConnectionString
                );

            connection.Open();

            return connection;
        }

        public static bool ValidateLogin(
            string username,
            string password)
        {
            using (var connection =
                GetConnection())
            {
                string sql = @"
                    SELECT COUNT(*)
                    FROM Users
                    WHERE Username = $username
                    AND Password = $password;
                ";

                using (var command =
                    new SqliteCommand(
                        sql,
                        connection))
                {
                    command.Parameters.AddWithValue(
                        "$username",
                        username
                    );

                    command.Parameters.AddWithValue(
                        "$password",
                        password
                    );

                    int count =
                        Convert.ToInt32(
                            command.ExecuteScalar()
                        );

                    return count > 0;
                }
            }
        }

        public static bool ChangeCredentials(
            string currentUsername,
            string currentPassword,
            string newUsername,
            string newPassword)
        {
            using (var connection =
                GetConnection())
            {
                string checkSql = @"
                    SELECT COUNT(*)
                    FROM Users
                    WHERE Username = $username
                    AND Password = $password;
                ";

                using (var checkCommand =
                    new SqliteCommand(
                        checkSql,
                        connection))
                {
                    checkCommand.Parameters.AddWithValue(
                        "$username",
                        currentUsername
                    );

                    checkCommand.Parameters.AddWithValue(
                        "$password",
                        currentPassword
                    );

                    int count =
                        Convert.ToInt32(
                            checkCommand.ExecuteScalar()
                        );

                    if (count == 0)
                    {
                        return false;
                    }
                }

                string duplicateSql = @"
                    SELECT COUNT(*)
                    FROM Users
                    WHERE Username = $newUsername
                    AND Username != $currentUsername;
                ";

                using (var duplicateCommand =
                    new SqliteCommand(
                        duplicateSql,
                        connection))
                {
                    duplicateCommand.Parameters.AddWithValue(
                        "$newUsername",
                        newUsername
                    );

                    duplicateCommand.Parameters.AddWithValue(
                        "$currentUsername",
                        currentUsername
                    );

                    int duplicateCount =
                        Convert.ToInt32(
                            duplicateCommand.ExecuteScalar()
                        );

                    if (duplicateCount > 0)
                    {
                        return false;
                    }
                }

                string updateSql = @"
                    UPDATE Users
                    SET Username = $newUsername,
                        Password = $newPassword
                    WHERE Username = $currentUsername
                    AND Password = $currentPassword;
                ";

                using (var updateCommand =
                    new SqliteCommand(
                        updateSql,
                        connection))
                {
                    updateCommand.Parameters.AddWithValue(
                        "$newUsername",
                        newUsername
                    );

                    updateCommand.Parameters.AddWithValue(
                        "$newPassword",
                        newPassword
                    );

                    updateCommand.Parameters.AddWithValue(
                        "$currentUsername",
                        currentUsername
                    );

                    updateCommand.Parameters.AddWithValue(
                        "$currentPassword",
                        currentPassword
                    );

                    int affected =
                        updateCommand.ExecuteNonQuery();

                    return affected > 0;
                }
            }
        }

        public static bool ResetPasswordWithRecoveryCode(
            string username,
            string recoveryCode,
            string newPassword)
        {
            using (var connection =
                GetConnection())
            {
                string sql = @"
                    UPDATE Users
                    SET Password = $newPassword
                    WHERE Username = $username
                    AND RecoveryCode = $recoveryCode;
                ";

                using (var command =
                    new SqliteCommand(
                        sql,
                        connection))
                {
                    command.Parameters.AddWithValue(
                        "$username",
                        username
                    );

                    command.Parameters.AddWithValue(
                        "$recoveryCode",
                        recoveryCode
                    );

                    command.Parameters.AddWithValue(
                        "$newPassword",
                        newPassword
                    );

                    int affected =
                        command.ExecuteNonQuery();

                    return affected > 0;
                }
            }
        }

        public static bool ChangeRecoveryCode(
            string username,
            string password,
            string newRecoveryCode)
        {
            using (var connection =
                GetConnection())
            {
                string sql = @"
                    UPDATE Users
                    SET RecoveryCode = $newRecoveryCode
                    WHERE Username = $username
                    AND Password = $password;
                ";

                using (var command =
                    new SqliteCommand(
                        sql,
                        connection))
                {
                    command.Parameters.AddWithValue(
                        "$username",
                        username
                    );

                    command.Parameters.AddWithValue(
                        "$password",
                        password
                    );

                    command.Parameters.AddWithValue(
                        "$newRecoveryCode",
                        newRecoveryCode
                    );

                    int affected =
                        command.ExecuteNonQuery();

                    return affected > 0;
                }
            }
        }
    }
}