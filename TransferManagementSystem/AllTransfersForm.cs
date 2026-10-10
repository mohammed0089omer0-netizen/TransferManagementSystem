using System;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Data.Sqlite;

namespace TransferManagementSystem
{
public class AllTransfersForm : Form
{
private DataGridView transfersGrid;
private TextBox searchBox;

    public AllTransfersForm()
    {
        InitializeForm();
        LoadTransfers();
    }

    private void InitializeForm()
    {
        Text = "جميع التحويلات";
        StartPosition = FormStartPosition.CenterScreen;
        Size = new Size(1250, 720);
        MinimumSize = new Size(1000, 600);
        BackColor = Color.FromArgb(245, 247, 250);
        RightToLeft = RightToLeft.Yes;
        RightToLeftLayout = true;

        TableLayoutPanel mainLayout = new TableLayoutPanel();
        mainLayout.Dock = DockStyle.Fill;
        mainLayout.Margin = new Padding(0);
        mainLayout.Padding = new Padding(20);
        mainLayout.ColumnCount = 1;
        mainLayout.RowCount = 3;

        mainLayout.ColumnStyles.Add(
            new ColumnStyle(SizeType.Percent, 100));

        mainLayout.RowStyles.Add(
            new RowStyle(SizeType.Absolute, 65));

        mainLayout.RowStyles.Add(
            new RowStyle(SizeType.Absolute, 55));

        mainLayout.RowStyles.Add(
            new RowStyle(SizeType.Percent, 100));

        Controls.Add(mainLayout);

        Label title = new Label();
        title.Text = "جميع التحويلات";
        title.Dock = DockStyle.Fill;
        title.ForeColor = Color.FromArgb(25, 45, 65);
        title.Font = new Font("Tahoma", 22, FontStyle.Bold);
        title.TextAlign = ContentAlignment.MiddleRight;

        mainLayout.Controls.Add(title, 0, 0);

        Panel searchPanel = new Panel();
        searchPanel.Dock = DockStyle.Fill;

        mainLayout.Controls.Add(searchPanel, 0, 1);

        Label searchLabel = new Label();
        searchLabel.Text = "بحث:";
        searchLabel.AutoSize = true;
        searchLabel.Font = new Font("Tahoma", 10, FontStyle.Bold);
        searchLabel.ForeColor = Color.FromArgb(40, 40, 40);
        searchLabel.Location = new Point(10, 13);

        searchPanel.Controls.Add(searchLabel);

        searchBox = new TextBox();
        searchBox.Width = 300;
        searchBox.Height = 30;
        searchBox.Font = new Font("Tahoma", 10);
        searchBox.Location = new Point(65, 8);
        searchBox.RightToLeft = RightToLeft.Yes;

        searchPanel.Controls.Add(searchBox);

        Button searchButton = new Button();
        searchButton.Text = "بحث";
        searchButton.Width = 90;
        searchButton.Height = 30;
        searchButton.Location = new Point(375, 8);
        searchButton.BackColor = Color.FromArgb(20, 60, 95);
        searchButton.ForeColor = Color.White;
        searchButton.FlatStyle = FlatStyle.Flat;
        searchButton.FlatAppearance.BorderSize = 0;
        searchButton.Font = new Font("Tahoma", 9, FontStyle.Bold);
        searchButton.Cursor = Cursors.Hand;
        searchButton.Click += SearchButton_Click;

        searchPanel.Controls.Add(searchButton);

        Button clearButton = new Button();
        clearButton.Text = "مسح";
        clearButton.Width = 80;
        clearButton.Height = 30;
        clearButton.Location = new Point(475, 8);
        clearButton.BackColor = Color.FromArgb(110, 110, 110);
        clearButton.ForeColor = Color.White;
        clearButton.FlatStyle = FlatStyle.Flat;
        clearButton.FlatAppearance.BorderSize = 0;
        clearButton.Font = new Font("Tahoma", 9, FontStyle.Bold);
        clearButton.Cursor = Cursors.Hand;
        clearButton.Click += ClearButton_Click;

        searchPanel.Controls.Add(clearButton);

        Panel gridPanel = new Panel();
        gridPanel.Dock = DockStyle.Fill;
        gridPanel.BackColor = Color.White;
        gridPanel.BorderStyle = BorderStyle.FixedSingle;

        mainLayout.Controls.Add(gridPanel, 0, 2);

        transfersGrid = new DataGridView();
        transfersGrid.Dock = DockStyle.Fill;
        transfersGrid.BackgroundColor = Color.White;
        transfersGrid.BorderStyle = BorderStyle.None;
        transfersGrid.ReadOnly = true;
        transfersGrid.AllowUserToAddRows = false;
        transfersGrid.AllowUserToDeleteRows = false;
        transfersGrid.AllowUserToResizeRows = false;
        transfersGrid.RowHeadersVisible = false;
        transfersGrid.SelectionMode =
            DataGridViewSelectionMode.FullRowSelect;
        transfersGrid.MultiSelect = false;
        transfersGrid.AutoSizeColumnsMode =
            DataGridViewAutoSizeColumnsMode.Fill;
        transfersGrid.RightToLeft = RightToLeft.Yes;
        transfersGrid.ColumnHeadersHeight = 42;
        transfersGrid.RowTemplate.Height = 35;
        transfersGrid.EnableHeadersVisualStyles = false;

        transfersGrid.ColumnHeadersDefaultCellStyle.BackColor =
            Color.FromArgb(20, 60, 95);

        transfersGrid.ColumnHeadersDefaultCellStyle.ForeColor =
            Color.White;

        transfersGrid.ColumnHeadersDefaultCellStyle.Font =
            new Font("Tahoma", 9, FontStyle.Bold);

        transfersGrid.ColumnHeadersDefaultCellStyle.Alignment =
            DataGridViewContentAlignment.MiddleCenter;

        transfersGrid.DefaultCellStyle.Font =
            new Font("Tahoma", 9);

        transfersGrid.DefaultCellStyle.Alignment =
            DataGridViewContentAlignment.MiddleCenter;

        transfersGrid.DefaultCellStyle.SelectionBackColor =
            Color.FromArgb(210, 225, 240);

        transfersGrid.DefaultCellStyle.SelectionForeColor =
            Color.Black;

        transfersGrid.Columns.Add("Id", "الرقم");
        transfersGrid.Columns.Add("Date", "التاريخ");
        transfersGrid.Columns.Add("Beneficiary", "المستفيد");
        transfersGrid.Columns.Add("Amount", "المبلغ");
        transfersGrid.Columns.Add("Reference", "رقم الإشعار");
        transfersGrid.Columns.Add("Description", "الوصف");
        transfersGrid.Columns.Add("Category", "التصنيف");
        transfersGrid.Columns.Add("Bank", "البنك");

        DataGridViewButtonColumn editColumn =
            new DataGridViewButtonColumn();

        editColumn.Name = "Edit";
        editColumn.HeaderText = "تعديل";
        editColumn.Text = "تعديل";
        editColumn.UseColumnTextForButtonValue = true;
        editColumn.Width = 80;
        editColumn.FlatStyle = FlatStyle.Flat;

        transfersGrid.Columns.Add(editColumn);

        DataGridViewButtonColumn deleteColumn =
            new DataGridViewButtonColumn();

        deleteColumn.Name = "Delete";
        deleteColumn.HeaderText = "حذف";
        deleteColumn.Text = "حذف";
        deleteColumn.UseColumnTextForButtonValue = true;
        deleteColumn.Width = 80;
        deleteColumn.FlatStyle = FlatStyle.Flat;

        transfersGrid.Columns.Add(deleteColumn);

        DataGridViewButtonColumn imageColumn =
            new DataGridViewButtonColumn();

        imageColumn.Name = "Image";
        imageColumn.HeaderText = "الإشعار";
        imageColumn.Text = "عرض";
        imageColumn.UseColumnTextForButtonValue = true;
        imageColumn.Width = 80;
        imageColumn.FlatStyle = FlatStyle.Flat;

        transfersGrid.Columns.Add(imageColumn);

        transfersGrid.CellContentClick +=
            TransfersGrid_CellContentClick;

        gridPanel.Controls.Add(transfersGrid);
    }

    private void SearchButton_Click(
        object sender,
        EventArgs e)
    {
        LoadTransfers(searchBox.Text.Trim());
    }

    private void ClearButton_Click(
        object sender,
        EventArgs e)
    {
        searchBox.Clear();
        LoadTransfers();
    }

    private void LoadTransfers(string search = "")
    {
        transfersGrid.Rows.Clear();

        try
        {
            using (var connection =
                Database.GetConnection())
            {
                string sql = @"
                    SELECT
                        Id,
                        TransferDate,
                        Beneficiary,
                        Amount,
                        ReferenceNumber,
                        Description,
                        Category,
                        Bank,
                        ReceiptImage
                    FROM Transfers
                ";

                if (!string.IsNullOrWhiteSpace(search))
                {
                    sql += @"
                        WHERE
                            Beneficiary LIKE $search
                            OR ReferenceNumber LIKE $search
                            OR Description LIKE $search
                            OR Category LIKE $search
                            OR Bank LIKE $search
                    ";
                }

                sql += " ORDER BY Id DESC;";

                using (var command =
                    new SqliteCommand(sql, connection))
                {
                    if (!string.IsNullOrWhiteSpace(search))
                    {
                        command.Parameters.AddWithValue(
                            "$search",
                            "%" + search + "%");
                    }

                    using (var reader =
                        command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string id =
                                reader["Id"]?.ToString() ?? "";

                            string date =
                                reader["TransferDate"]?.ToString() ?? "";

                            string beneficiary =
                                reader["Beneficiary"]?.ToString() ?? "";

                            decimal amount = 0;

                            if (reader["Amount"] != DBNull.Value)
                            {
                                decimal.TryParse(
                                    reader["Amount"].ToString(),
                                    out amount);
                            }

                            string reference =
                                reader["ReferenceNumber"]?.ToString() ?? "";

                            string description =
                                reader["Description"]?.ToString() ?? "";

                            string category =
                                reader["Category"]?.ToString() ?? "";

                            string bank =
                                reader["Bank"]?.ToString() ?? "";

                            string image =
                                reader["ReceiptImage"]?.ToString() ?? "";

                            int rowIndex =
                                transfersGrid.Rows.Add(
                                    id,
                                    date.Replace("-", "/"),
                                    beneficiary,
                                    amount.ToString("N0"),
                                    reference,
                                    description,
                                    category,
                                    bank,
                                    "تعديل",
                                    "حذف",
                                    string.IsNullOrWhiteSpace(image)
                                        ? "لا توجد"
                                        : "عرض"
                                );

                            transfersGrid.Rows[rowIndex].Tag =
                                image;
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                "حدث خطأ أثناء تحميل التحويلات:\n\n"
                + ex.Message,
                "خطأ",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }

    private void TransfersGrid_CellContentClick(
        object sender,
        DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0 ||
            e.ColumnIndex < 0)
        {
            return;
        }

        DataGridViewRow row =
            transfersGrid.Rows[e.RowIndex];

        string idText =
            row.Cells["Id"].Value?.ToString() ?? "";

        if (!int.TryParse(idText, out int transferId))
        {
            return;
        }

        string columnName =
            transfersGrid.Columns[e.ColumnIndex].Name;

        if (columnName == "Delete")
        {
            DeleteTransfer(transferId);
        }
       
       else if (columnName == "Edit")
{
    using (EditTransferForm form =
           new EditTransferForm(transferId))
    {
        form.ShowDialog();
    }

    LoadTransfers(searchBox.Text.Trim());
}
        else if (columnName == "Image")
        {
            ShowReceiptImage(row);
        }
    }

    private void DeleteTransfer(int transferId)
    {
        DialogResult result =
            MessageBox.Show(
                "هل أنت متأكد من حذف هذا التحويل؟\n\n"
                + "لا يمكن التراجع عن عملية الحذف.",
                "تأكيد الحذف",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button2);

        if (result != DialogResult.Yes)
        {
            return;
        }

        try
        {
            using (var connection =
                Database.GetConnection())
            {
                string sql = @"
                    DELETE FROM Transfers
                    WHERE Id = $id;
                ";

                using (var command =
                    new SqliteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue(
                        "$id",
                        transferId);

                    int affectedRows =
                        command.ExecuteNonQuery();

                    if (affectedRows > 0)
                    {
                        MessageBox.Show(
                            "تم حذف التحويل بنجاح.",
                            "تم الحذف",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);

                        LoadTransfers(
                            searchBox.Text.Trim());
                    }
                    else
                    {
                        MessageBox.Show(
                            "لم يتم العثور على التحويل.",
                            "تنبيه",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                "حدث خطأ أثناء حذف التحويل:\n\n"
                + ex.Message,
                "خطأ",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }

    private void ShowReceiptImage(
        DataGridViewRow row)
    {
        string imagePath =
            row.Tag?.ToString() ?? "";

        if (string.IsNullOrWhiteSpace(imagePath))
        {
            MessageBox.Show(
                "لا توجد صورة إشعار مرتبطة بهذا التحويل.",
                "صورة الإشعار",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);

            return;
        }

        if (!System.IO.File.Exists(imagePath))
        {
            MessageBox.Show(
                "صورة الإشعار غير موجودة في المسار المحفوظ:\n\n"
                + imagePath,
                "صورة الإشعار",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);

            return;
        }

        try
        {
            Form imageForm = new Form();

            imageForm.Text = "صورة إشعار التحويل";
            imageForm.StartPosition =
                FormStartPosition.CenterScreen;

            imageForm.Size =
                new Size(900, 700);

            imageForm.MinimumSize =
                new Size(600, 500);

            imageForm.BackColor =
                Color.FromArgb(30, 30, 30);

            PictureBox pictureBox =
                new PictureBox();

            pictureBox.Dock =
                DockStyle.Fill;

            pictureBox.BackColor =
                Color.FromArgb(30, 30, 30);

            pictureBox.SizeMode =
                PictureBoxSizeMode.Zoom;

            using (var stream =
                new System.IO.FileStream(
                    imagePath,
                    System.IO.FileMode.Open,
                    System.IO.FileAccess.Read))
            {
                pictureBox.Image =
                    Image.FromStream(stream);
            }

            imageForm.Controls.Add(
                pictureBox);

            imageForm.ShowDialog();
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                "حدث خطأ أثناء عرض الصورة:\n\n"
                + ex.Message,
                "خطأ",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }
}


}
