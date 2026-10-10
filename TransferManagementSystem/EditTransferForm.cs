using System;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Data.Sqlite;

namespace TransferManagementSystem
{
    public class EditTransferForm : Form
    {
        private int transferId;

        private DateTimePicker dtpDate;
        private TextBox txtBeneficiary;
        private TextBox txtAmount;
        private TextBox txtReference;
        private TextBox txtDescription;
        private ComboBox cmbCategory;
        private ComboBox cmbBank;
        private TextBox txtReceiptImage;

        public EditTransferForm(int id)
        {
            transferId = id;

            CreateForm();
            LoadCategories();
            LoadTransfer();
        }

        private void CreateForm()
        {
            Text = "تعديل التحويل";
            Size = new Size(850, 700);
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Color.FromArgb(245, 247, 250);

            RightToLeft = RightToLeft.Yes;
            RightToLeftLayout = true;

            Label title = new Label();

            title.Text = "تعديل بيانات التحويل";
            title.Font = new Font(
                "Tahoma",
                24,
                FontStyle.Bold
            );

            title.ForeColor = Color.FromArgb(
                20,
                45,
                70
            );

            title.AutoSize = true;
            title.Location = new Point(55, 30);

            Controls.Add(title);


            AddLabel(
                "التاريخ",
                55,
                100
            );

            dtpDate = new DateTimePicker();

            dtpDate.Format =
                DateTimePickerFormat.Short;

            dtpDate.Font =
                new Font(
                    "Tahoma",
                    11
                );

            dtpDate.Size =
                new Size(
                    300,
                    35
                );

            dtpDate.Location =
                new Point(
                    55,
                    130
                );

            Controls.Add(dtpDate);


            AddLabel(
                "اسم المستفيد",
                440,
                100
            );

            txtBeneficiary = new TextBox();

            txtBeneficiary.Font =
                new Font(
                    "Tahoma",
                    11
                );

            txtBeneficiary.Size =
                new Size(
                    300,
                    35
                );

            txtBeneficiary.Location =
                new Point(
                    440,
                    130
                );

            txtBeneficiary.RightToLeft =
                RightToLeft.Yes;

            Controls.Add(txtBeneficiary);


            AddLabel(
                "المبلغ",
                55,
                200
            );

            txtAmount = new TextBox();

            txtAmount.Font =
                new Font(
                    "Tahoma",
                    11
                );

            txtAmount.Size =
                new Size(
                    300,
                    35
                );

            txtAmount.Location =
                new Point(
                    55,
                    230
                );

            txtAmount.RightToLeft =
                RightToLeft.Yes;

            Controls.Add(txtAmount);


            AddLabel(
                "رقم الإشعار / المرجع",
                440,
                200
            );

            txtReference = new TextBox();

            txtReference.Font =
                new Font(
                    "Tahoma",
                    11
                );

            txtReference.Size =
                new Size(
                    300,
                    35
                );

            txtReference.Location =
                new Point(
                    440,
                    230
                );

            txtReference.RightToLeft =
                RightToLeft.Yes;

            Controls.Add(txtReference);


            AddLabel(
                "البنك",
                55,
                300
            );

            cmbBank = new ComboBox();

            cmbBank.Font =
                new Font(
                    "Tahoma",
                    11
                );

            cmbBank.Size =
                new Size(
                    300,
                    35
                );

            cmbBank.Location =
                new Point(
                    55,
                    330
                );

            cmbBank.DropDownStyle =
                ComboBoxStyle.DropDownList;

            cmbBank.RightToLeft =
                RightToLeft.Yes;

            cmbBank.Items.Add(
                "بنك الخرطوم"
            );

            cmbBank.Items.Add(
                "بنك فيصل"
            );

            cmbBank.Items.Add(
                "بنك أم درمان"
            );

            cmbBank.Items.Add(
                "بنك البركة"
            );

            cmbBank.Items.Add(
                "بنك النيلين"
            );

            cmbBank.Items.Add(
                "أخرى"
            );

            if (cmbBank.Items.Count > 0)
            {
                cmbBank.SelectedIndex = 0;
            }

            Controls.Add(cmbBank);


            AddLabel(
                "التصنيف",
                440,
                300
            );

            cmbCategory = new ComboBox();

            cmbCategory.Font =
                new Font(
                    "Tahoma",
                    11
                );

            cmbCategory.Size =
                new Size(
                    300,
                    35
                );

            cmbCategory.Location =
                new Point(
                    440,
                    330
                );

            cmbCategory.DropDownStyle =
                ComboBoxStyle.DropDownList;

            cmbCategory.RightToLeft =
                RightToLeft.Yes;

            Controls.Add(cmbCategory);


            AddLabel(
                "التعليق / الوصف",
                55,
                400
            );

            txtDescription = new TextBox();

            txtDescription.Font =
                new Font(
                    "Tahoma",
                    11
                );

            txtDescription.Multiline =
                true;

            txtDescription.Size =
                new Size(
                    685,
                    80
                );

            txtDescription.Location =
                new Point(
                    55,
                    430
                );

            txtDescription.RightToLeft =
                RightToLeft.Yes;

            Controls.Add(txtDescription);


            AddLabel(
                "صورة الإشعار",
                55,
                530
            );

            txtReceiptImage = new TextBox();

            txtReceiptImage.Font =
                new Font(
                    "Tahoma",
                    10
                );

            txtReceiptImage.Size =
                new Size(
                    520,
                    35
                );

            txtReceiptImage.Location =
                new Point(
                    55,
                    560
                );

            txtReceiptImage.ReadOnly =
                true;

            txtReceiptImage.RightToLeft =
                RightToLeft.Yes;

            Controls.Add(txtReceiptImage);


            Button btnImage =
                new Button();

            btnImage.Text =
                "اختيار صورة";

            btnImage.Font =
                new Font(
                    "Tahoma",
                    10,
                    FontStyle.Bold
                );

            btnImage.BackColor =
                Color.FromArgb(
                    40,
                    100,
                    150
                );

            btnImage.ForeColor =
                Color.White;

            btnImage.FlatStyle =
                FlatStyle.Flat;

            btnImage.FlatAppearance.BorderSize =
                0;

            btnImage.Size =
                new Size(
                    140,
                    35
                );

            btnImage.Location =
                new Point(
                    590,
                    560
                );

            btnImage.Cursor =
                Cursors.Hand;

            btnImage.Click +=
                BtnImage_Click;

            Controls.Add(btnImage);


            Button btnSave =
                new Button();

            btnSave.Text =
                "حفظ التعديلات";

            btnSave.Font =
                new Font(
                    "Tahoma",
                    12,
                    FontStyle.Bold
                );

            btnSave.BackColor =
                Color.FromArgb(
                    20,
                    100,
                    75
                );

            btnSave.ForeColor =
                Color.White;

            btnSave.FlatStyle =
                FlatStyle.Flat;

            btnSave.FlatAppearance.BorderSize =
                0;

            btnSave.Size =
                new Size(
                    685,
                    50
                );

            btnSave.Location =
                new Point(
                    55,
                    615
                );

            btnSave.Cursor =
                Cursors.Hand;

            btnSave.Click +=
                BtnSave_Click;

            Controls.Add(btnSave);
        }

        private void AddLabel(
            string text,
            int x,
            int y)
        {
            Label label =
                new Label();

            label.Text =
                text;

            label.Font =
                new Font(
                    "Tahoma",
                    10,
                    FontStyle.Bold
                );

            label.ForeColor =
                Color.FromArgb(
                    50,
                    50,
                    50
                );

            label.AutoSize =
                true;

            label.Location =
                new Point(
                    x,
                    y
                );

            Controls.Add(label);
        }

        private void LoadCategories()
        {
            try
            {
                cmbCategory.Items.Clear();

                using (
                    SqliteConnection connection =
                    Database.GetConnection())
                {
                    string sql = @"
                        SELECT Name
                        FROM Categories
                        ORDER BY Id ASC;
                    ";

                    using (
                        SqliteCommand command =
                        new SqliteCommand(
                            sql,
                            connection))
                    {
                        using (
                            SqliteDataReader reader =
                            command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string name =
                                    reader["Name"]
                                    ?.ToString() ?? "";

                                if (
                                    !string.IsNullOrWhiteSpace(
                                        name))
                                {
                                    cmbCategory.Items.Add(
                                        name
                                    );
                                }
                            }
                        }
                    }
                }

                if (cmbCategory.Items.Count > 0)
                {
                    cmbCategory.SelectedIndex = 0;
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

        private void LoadTransfer()
        {
            try
            {
                using (
                    SqliteConnection connection =
                    Database.GetConnection())
                {
                    string sql = @"
                        SELECT
                            TransferDate,
                            Beneficiary,
                            Amount,
                            ReferenceNumber,
                            Description,
                            Category,
                            Bank,
                            ReceiptImage
                        FROM Transfers
                        WHERE Id = $id;
                    ";

                    using (
                        SqliteCommand command =
                        new SqliteCommand(
                            sql,
                            connection))
                    {
                        command.Parameters.AddWithValue(
                            "$id",
                            transferId
                        );

                        using (
                            SqliteDataReader reader =
                            command.ExecuteReader())
                        {
                            if (!reader.Read())
                            {
                                MessageBox.Show(
                                    "لم يتم العثور على بيانات التحويل.",
                                    "تنبيه",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Warning
                                );

                                Close();
                                return;
                            }

                            string date =
                                reader["TransferDate"]
                                ?.ToString() ?? "";

                            DateTime parsedDate;

                            if (
                                DateTime.TryParse(
                                    date,
                                    out parsedDate))
                            {
                                dtpDate.Value =
                                    parsedDate;
                            }

                            txtBeneficiary.Text =
                                reader["Beneficiary"]
                                ?.ToString() ?? "";

                            txtAmount.Text =
                                reader["Amount"]
                                ?.ToString() ?? "";

                            txtReference.Text =
                                reader["ReferenceNumber"]
                                ?.ToString() ?? "";

                            txtDescription.Text =
                                reader["Description"]
                                ?.ToString() ?? "";

                            string category =
                                reader["Category"]
                                ?.ToString() ?? "";

                            if (
                                !string.IsNullOrWhiteSpace(
                                    category))
                            {
                                int index =
                                    cmbCategory.Items.IndexOf(
                                        category
                                    );

                                if (index >= 0)
                                {
                                    cmbCategory.SelectedIndex =
                                        index;
                                }
                            }

                            string bank =
                                reader["Bank"]
                                ?.ToString() ?? "";

                            if (
                                !string.IsNullOrWhiteSpace(
                                    bank))
                            {
                                int index =
                                    cmbBank.Items.IndexOf(
                                        bank
                                    );

                                if (index >= 0)
                                {
                                    cmbBank.SelectedIndex =
                                        index;
                                }
                            }

                            txtReceiptImage.Text =
                                reader["ReceiptImage"]
                                ?.ToString() ?? "";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "حدث خطأ أثناء تحميل بيانات التحويل:\n\n"
                    + ex.Message,
                    "خطأ",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void BtnImage_Click(
            object sender,
            EventArgs e)
        {
            using (
                OpenFileDialog dialog =
                new OpenFileDialog())
            {
                dialog.Title =
                    "اختيار صورة إشعار التحويل";

                dialog.Filter =
                    "صور|*.jpg;*.jpeg;*.png;*.bmp|كل الملفات|*.*";

                if (
                    dialog.ShowDialog() ==
                    DialogResult.OK)
                {
                    txtReceiptImage.Text =
                        dialog.FileName;
                }
            }
        }

        private void BtnSave_Click(
            object sender,
            EventArgs e)
        {
            if (
                string.IsNullOrWhiteSpace(
                    txtBeneficiary.Text))
            {
                MessageBox.Show(
                    "يرجى إدخال اسم المستفيد",
                    "تنبيه",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                txtBeneficiary.Focus();
                return;
            }

            decimal amount;

            if (
                !decimal.TryParse(
                    txtAmount.Text.Trim(),
                    out amount))
            {
                MessageBox.Show(
                    "يرجى إدخال مبلغ صحيح",
                    "تنبيه",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                txtAmount.Focus();
                return;
            }

            if (
                cmbCategory.SelectedIndex < 0 ||
                string.IsNullOrWhiteSpace(
                    cmbCategory.Text))
            {
                MessageBox.Show(
                    "يرجى اختيار التصنيف",
                    "تنبيه",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                cmbCategory.Focus();
                return;
            }

            try
            {
                using (
                    SqliteConnection connection =
                    Database.GetConnection())
                {
                    string sql = @"
                        UPDATE Transfers
                        SET
                            TransferDate = $date,
                            Beneficiary = $beneficiary,
                            Amount = $amount,
                            ReferenceNumber = $reference,
                            Description = $description,
                            Category = $category,
                            Bank = $bank,
                            ReceiptImage = $image
                        WHERE Id = $id;
                    ";

                    using (
                        SqliteCommand command =
                        new SqliteCommand(
                            sql,
                            connection))
                    {
                        command.Parameters.AddWithValue(
                            "$id",
                            transferId
                        );

                        command.Parameters.AddWithValue(
                            "$date",
                            dtpDate.Value.ToString(
                                "yyyy-MM-dd")
                        );

                        command.Parameters.AddWithValue(
                            "$beneficiary",
                            txtBeneficiary.Text.Trim()
                        );

                        command.Parameters.AddWithValue(
                            "$amount",
                            amount
                        );

                        command.Parameters.AddWithValue(
                            "$reference",
                            txtReference.Text.Trim()
                        );

                        command.Parameters.AddWithValue(
                            "$description",
                            txtDescription.Text.Trim()
                        );

                        command.Parameters.AddWithValue(
                            "$category",
                            cmbCategory.Text.Trim()
                        );

                        command.Parameters.AddWithValue(
                            "$bank",
                            cmbBank.Text.Trim()
                        );

                        command.Parameters.AddWithValue(
                            "$image",
                            txtReceiptImage.Text.Trim()
                        );

                        int affectedRows =
                            command.ExecuteNonQuery();

                        if (affectedRows == 0)
                        {
                            MessageBox.Show(
                                "لم يتم العثور على التحويل.",
                                "تنبيه",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning
                            );

                            return;
                        }
                    }
                }

                MessageBox.Show(
                    "تم تعديل التحويل بنجاح.",
                    "نجاح",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                DialogResult =
                    DialogResult.OK;

                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "حدث خطأ أثناء حفظ التعديلات:\n\n"
                    + ex.Message,
                    "خطأ",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }
    }
}