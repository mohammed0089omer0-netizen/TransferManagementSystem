using System;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Data.Sqlite;

namespace TransferManagementSystem
{
    public class CategoriesForm : Form
    {
        private DataGridView categoriesGrid;
        private TextBox categoryTextBox;
        private Button addButton;
        private Button editButton;
        private Button deleteButton;
        private Button clearButton;


        // =====================================================
        // تشغيل الصفحة
        // =====================================================

        public CategoriesForm()
        {
            InitializeForm();
            CreateInterface();
            LoadCategories();
        }


        // =====================================================
        // إعداد النافذة
        // =====================================================

        private void InitializeForm()
        {
            Text =
                "التصنيفات";

            StartPosition =
                FormStartPosition.CenterScreen;

            MinimumSize =
                new Size(
                    850,
                    600
                );

            Size =
                new Size(
                    1000,
                    650
                );

            BackColor =
                Color.FromArgb(
                    245,
                    247,
                    250
                );

            RightToLeft =
                RightToLeft.Yes;

            RightToLeftLayout =
                true;
        }


        // =====================================================
        // إنشاء الواجهة
        // =====================================================

        private void CreateInterface()
        {
            TableLayoutPanel mainLayout =
                new TableLayoutPanel();

            mainLayout.Dock =
                DockStyle.Fill;

            mainLayout.Padding =
                new Padding(
                    30
                );

            mainLayout.Margin =
                new Padding(0);

            mainLayout.ColumnCount =
                1;

            mainLayout.RowCount =
                3;

            mainLayout.ColumnStyles.Add(
                new ColumnStyle(
                    SizeType.Percent,
                    100
                )
            );


            // العنوان
            mainLayout.RowStyles.Add(
                new RowStyle(
                    SizeType.Absolute,
                    80
                )
            );


            // إضافة تصنيف
            mainLayout.RowStyles.Add(
                new RowStyle(
                    SizeType.Absolute,
                    100
                )
            );


            // الجدول
            mainLayout.RowStyles.Add(
                new RowStyle(
                    SizeType.Percent,
                    100
                )
            );


            Controls.Add(
                mainLayout
            );


            // =================================================
            // عنوان الصفحة
            // =================================================

            Label pageTitle =
                new Label();

            pageTitle.Text =
                "التصنيفات";

            pageTitle.Dock =
                DockStyle.Fill;

            pageTitle.Font =
                new Font(
                    "Tahoma",
                    23,
                    FontStyle.Bold
                );

            pageTitle.ForeColor =
                Color.FromArgb(
                    25,
                    45,
                    65
                );

            pageTitle.TextAlign =
                ContentAlignment.MiddleRight;

            mainLayout.Controls.Add(
                pageTitle,
                0,
                0
            );


            // =================================================
            // لوحة الإضافة
            // =================================================

            Panel addPanel =
                new Panel();

            addPanel.Dock =
                DockStyle.Fill;

            addPanel.BackColor =
                Color.White;

            addPanel.BorderStyle =
                BorderStyle.FixedSingle;

            mainLayout.Controls.Add(
                addPanel,
                0,
                1
            );


            // =================================================
            // عنوان الحقل
            // =================================================

            Label nameLabel =
                new Label();

            nameLabel.Text =
                "اسم التصنيف:";

            nameLabel.AutoSize =
                true;

            nameLabel.Font =
                new Font(
                    "Tahoma",
                    10,
                    FontStyle.Bold
                );

            nameLabel.ForeColor =
                Color.FromArgb(
                    40,
                    55,
                    70
                );

            nameLabel.Location =
                new Point(
                    650,
                    35
                );

            addPanel.Controls.Add(
                nameLabel
            );


            // =================================================
            // حقل التصنيف
            // =================================================

            categoryTextBox =
                new TextBox();

            categoryTextBox.Font =
                new Font(
                    "Tahoma",
                    11
                );

            categoryTextBox.Width =
                300;

            categoryTextBox.Height =
                35;

            categoryTextBox.Location =
                new Point(
                    330,
                    28
                );

            categoryTextBox.RightToLeft =
                RightToLeft.Yes;

            addPanel.Controls.Add(
                categoryTextBox
            );


            // =================================================
            // زر إضافة
            // =================================================

            addButton =
                CreateActionButton(
                    "إضافة",
                    Color.FromArgb(
                        45,
                        150,
                        95
                    )
                );

            addButton.Location =
                new Point(
                    215,
                    27
                );

            addButton.Click +=
                AddCategory;

            addPanel.Controls.Add(
                addButton
            );


            // =================================================
            // زر تعديل
            // =================================================

            editButton =
                CreateActionButton(
                    "تعديل",
                    Color.FromArgb(
                        55,
                        130,
                        220
                    )
                );

            editButton.Location =
                new Point(
                    110,
                    27
                );

            editButton.Click +=
                EditCategory;

            addPanel.Controls.Add(
                editButton
            );


            // =================================================
            // زر مسح
            // =================================================

            clearButton =
                CreateActionButton(
                    "مسح",
                    Color.FromArgb(
                        120,
                        125,
                        130
                    )
                );

            clearButton.Location =
                new Point(
                    5,
                    27
                );

            clearButton.Click +=
                (sender, e) =>
                {
                    categoryTextBox.Clear();
                    categoriesGrid.ClearSelection();
                };

            addPanel.Controls.Add(
                clearButton
            );


            // =================================================
            // الجدول
            // =================================================

            Panel gridPanel =
                new Panel();

            gridPanel.Dock =
                DockStyle.Fill;

            gridPanel.BackColor =
                Color.White;

            gridPanel.BorderStyle =
                BorderStyle.FixedSingle;

            mainLayout.Controls.Add(
                gridPanel,
                0,
                2
            );


            categoriesGrid =
                new DataGridView();

            categoriesGrid.Dock =
                DockStyle.Fill;

            categoriesGrid.RightToLeft =
                RightToLeft.Yes;

            categoriesGrid.BackgroundColor =
                Color.White;

            categoriesGrid.BorderStyle =
                BorderStyle.None;

            categoriesGrid.AllowUserToAddRows =
                false;

            categoriesGrid.AllowUserToDeleteRows =
                false;

            categoriesGrid.AllowUserToResizeRows =
                false;

            categoriesGrid.ReadOnly =
                true;

            categoriesGrid.RowHeadersVisible =
                false;

            categoriesGrid.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;

            categoriesGrid.MultiSelect =
                false;

            categoriesGrid.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;

            categoriesGrid.RowTemplate.Height =
                38;

            categoriesGrid.ColumnHeadersHeight =
                42;

            categoriesGrid.EnableHeadersVisualStyles =
                false;


            // =================================================
            // شكل رأس الجدول
            // =================================================

            categoriesGrid.ColumnHeadersDefaultCellStyle.BackColor =
                Color.FromArgb(
                    20,
                    60,
                    95
                );

            categoriesGrid.ColumnHeadersDefaultCellStyle.ForeColor =
                Color.White;

            categoriesGrid.ColumnHeadersDefaultCellStyle.Font =
                new Font(
                    "Tahoma",
                    10,
                    FontStyle.Bold
                );

            categoriesGrid.ColumnHeadersDefaultCellStyle.Alignment =
                DataGridViewContentAlignment.MiddleCenter;


            // =================================================
            // شكل الخلايا
            // =================================================

            categoriesGrid.DefaultCellStyle.Font =
                new Font(
                    "Tahoma",
                    10
                );

            categoriesGrid.DefaultCellStyle.Alignment =
                DataGridViewContentAlignment.MiddleCenter;

            categoriesGrid.DefaultCellStyle.SelectionBackColor =
                Color.FromArgb(
                    210,
                    225,
                    240
                );

            categoriesGrid.DefaultCellStyle.SelectionForeColor =
                Color.Black;


            // =================================================
            // الأعمدة
            // =================================================

            categoriesGrid.Columns.Add(
                "Id",
                "الرقم"
            );

            categoriesGrid.Columns.Add(
                "Name",
                "اسم التصنيف"
            );


            // =================================================
            // زر حذف داخل الجدول
            // =================================================

            DataGridViewButtonColumn deleteColumn =
                new DataGridViewButtonColumn();

            deleteColumn.Name =
                "Delete";

            deleteColumn.HeaderText =
                "الإجراء";

            deleteColumn.Text =
                "حذف";

            deleteColumn.UseColumnTextForButtonValue =
                true;

            deleteColumn.Width =
                100;

            categoriesGrid.Columns.Add(
                deleteColumn
            );


            categoriesGrid.CellClick +=
                CategoriesGrid_CellClick;


            gridPanel.Controls.Add(
                categoriesGrid
            );
        }


        // =====================================================
        // إنشاء أزرار الإجراءات
        // =====================================================

        private Button CreateActionButton(
            string text,
            Color color)
        {
            Button button =
                new Button();

            button.Text =
                text;

            button.Width =
                95;

            button.Height =
                40;

            button.BackColor =
                color;

            button.ForeColor =
                Color.White;

            button.FlatStyle =
                FlatStyle.Flat;

            button.FlatAppearance.BorderSize =
                0;

            button.Font =
                new Font(
                    "Tahoma",
                    9,
                    FontStyle.Bold
                );

            button.Cursor =
                Cursors.Hand;

            return button;
        }


        // =====================================================
        // تحميل التصنيفات
        // =====================================================

        private void LoadCategories()
        {
            try
            {
                categoriesGrid.Rows.Clear();

                using (var connection =
                    Database.GetConnection())
                {
                    string sql = @"
                        SELECT Id, Name
                        FROM Categories
                        ORDER BY Id ASC;
                    ";

                    using (var command =
                        new SqliteCommand(
                            sql,
                            connection))
                    {
                        using (var reader =
                            command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                categoriesGrid.Rows.Add(
                                    reader["Id"].ToString(),
                                    reader["Name"].ToString()
                                );
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "حدث خطأ أثناء تحميل التصنيفات:\n\n"
                    + ex.Message,
                    "خطأ",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }


        // =====================================================
        // إضافة تصنيف
        // =====================================================

        private void AddCategory(
            object sender,
            EventArgs e)
        {
            string name =
                categoryTextBox.Text.Trim();

            if (string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show(
                    "اكتب اسم التصنيف أولاً.",
                    "تنبيه",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                categoryTextBox.Focus();

                return;
            }


            try
            {
                using (var connection =
                    Database.GetConnection())
                {
                    string sql = @"
                        INSERT INTO Categories (Name)
                        VALUES ($name);
                    ";

                    using (var command =
                        new SqliteCommand(
                            sql,
                            connection))
                    {
                        command.Parameters.AddWithValue(
                            "$name",
                            name
                        );

                        command.ExecuteNonQuery();
                    }
                }


                MessageBox.Show(
                    "تمت إضافة التصنيف بنجاح.",
                    "تم",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                categoryTextBox.Clear();

                LoadCategories();
            }
            catch (SqliteException)
            {
                MessageBox.Show(
                    "هذا التصنيف موجود بالفعل.",
                    "تنبيه",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "حدث خطأ أثناء إضافة التصنيف:\n\n"
                    + ex.Message,
                    "خطأ",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }


        // =====================================================
        // اختيار تصنيف من الجدول
        // =====================================================

        private void CategoriesGrid_CellClick(
            object sender,
            DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;


            // زر الحذف
            if (e.ColumnIndex ==
                categoriesGrid.Columns["Delete"].Index)
            {
                DeleteCategory(
                    e.RowIndex
                );

                return;
            }


            // تعبئة الحقل عند اختيار الصف
            string name =
                categoriesGrid
                .Rows[e.RowIndex]
                .Cells["Name"]
                .Value
                ?.ToString();

            categoryTextBox.Text =
                name ?? "";
        }


        // =====================================================
        // تعديل التصنيف
        // =====================================================

        private void EditCategory(
            object sender,
            EventArgs e)
        {
            if (categoriesGrid.SelectedRows.Count == 0)
            {
                MessageBox.Show(
                    "حدد التصنيف الذي تريد تعديله أولاً.",
                    "تنبيه",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                return;
            }


            string newName =
                categoryTextBox.Text.Trim();

            if (string.IsNullOrWhiteSpace(newName))
            {
                MessageBox.Show(
                    "اكتب اسم التصنيف الجديد.",
                    "تنبيه",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                return;
            }


            int id =
                Convert.ToInt32(
                    categoriesGrid
                    .SelectedRows[0]
                    .Cells["Id"]
                    .Value
                );


            try
            {
                using (var connection =
                    Database.GetConnection())
                {
                    string sql = @"
                        UPDATE Categories
                        SET Name = $name
                        WHERE Id = $id;
                    ";

                    using (var command =
                        new SqliteCommand(
                            sql,
                            connection))
                    {
                        command.Parameters.AddWithValue(
                            "$name",
                            newName
                        );

                        command.Parameters.AddWithValue(
                            "$id",
                            id
                        );

                        command.ExecuteNonQuery();
                    }
                }


                MessageBox.Show(
                    "تم تعديل التصنيف بنجاح.",
                    "تم",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                categoryTextBox.Clear();

                LoadCategories();
            }
            catch (SqliteException)
            {
                MessageBox.Show(
                    "هذا الاسم مستخدم لتصنيف آخر.",
                    "تنبيه",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "حدث خطأ أثناء تعديل التصنيف:\n\n"
                    + ex.Message,
                    "خطأ",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }


        // =====================================================
        // حذف التصنيف
        // =====================================================

        private void DeleteCategory(
            int rowIndex)
        {
            int id =
                Convert.ToInt32(
                    categoriesGrid
                    .Rows[rowIndex]
                    .Cells["Id"]
                    .Value
                );

            string name =
                categoriesGrid
                .Rows[rowIndex]
                .Cells["Name"]
                .Value
                ?.ToString() ?? "";


            DialogResult result =
                MessageBox.Show(
                    "هل أنت متأكد من حذف التصنيف:\n\n"
                    + name,
                    "تأكيد الحذف",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );


            if (result != DialogResult.Yes)
                return;


            try
            {
                using (var connection =
                    Database.GetConnection())
                {
                    // =====================================
                    // التأكد أن التصنيف غير مستخدم
                    // =====================================

                    string checkSql = @"
                        SELECT COUNT(*)
                        FROM Transfers
                        WHERE Category = $name;
                    ";

                    using (var checkCommand =
                        new SqliteCommand(
                            checkSql,
                            connection))
                    {
                        checkCommand.Parameters.AddWithValue(
                            "$name",
                            name
                        );

                        int count =
                            Convert.ToInt32(
                                checkCommand.ExecuteScalar()
                            );

                        if (count > 0)
                        {
                            MessageBox.Show(
                                "لا يمكن حذف هذا التصنيف لأنه مستخدم في "
                                + count
                                + " تحويل.",
                                "لا يمكن الحذف",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning
                            );

                            return;
                        }
                    }


                    // =====================================
                    // حذف التصنيف
                    // =====================================

                    string deleteSql = @"
                        DELETE FROM Categories
                        WHERE Id = $id;
                    ";

                    using (var deleteCommand =
                        new SqliteCommand(
                            deleteSql,
                            connection))
                    {
                        deleteCommand.Parameters.AddWithValue(
                            "$id",
                            id
                        );

                        deleteCommand.ExecuteNonQuery();
                    }
                }


                LoadCategories();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "حدث خطأ أثناء حذف التصنيف:\n\n"
                    + ex.Message,
                    "خطأ",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }
    }
}