using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Microsoft.Data.Sqlite;
using ClosedXML.Excel;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

using DrawingColor = System.Drawing.Color;
using DrawingSize = System.Drawing.Size;

namespace TransferManagementSystem
{
    public class ReportsForm : Form
    {
        private DateTimePicker fromDatePicker;
        private DateTimePicker toDatePicker;

        private Button searchButton;
        private Button clearButton;
        private Button pdfButton;
        private Button excelButton;

        private Label totalAmountLabel;
        private Label transferCountLabel;

        private DataGridView reportGrid;
        private DataGridView categoryGrid;
        private DataGridView bankGrid;

        public ReportsForm()
        {
            InitializeForm();
            CreateInterface();

            fromDatePicker.Value = DateTime.Today;
            toDatePicker.Value = DateTime.Today;

            LoadReport();
        }

        private void InitializeForm()
        {
            Text = "التقارير";
            StartPosition = FormStartPosition.CenterScreen;
            MinimumSize = new DrawingSize(1100, 700);
            Size = new DrawingSize(1250, 780);
            BackColor = DrawingColor.FromArgb(245, 247, 250);
            RightToLeft = RightToLeft.Yes;
            RightToLeftLayout = true;
        }

        private void CreateInterface()
        {
            TableLayoutPanel mainLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(25),
                ColumnCount = 1,
                RowCount = 4
            };

            mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 65));
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 90));
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 105));
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

            Controls.Add(mainLayout);

            Label pageTitle = new Label
            {
                Text = "التقارير",
                Dock = DockStyle.Fill,
                Font = new Font("Tahoma", 23, FontStyle.Bold),
                ForeColor = DrawingColor.FromArgb(25, 45, 65),
                TextAlign = ContentAlignment.MiddleRight
            };

            mainLayout.Controls.Add(pageTitle, 0, 0);

            Panel filterPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = DrawingColor.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            mainLayout.Controls.Add(filterPanel, 0, 1);

            Label fromLabel = CreateLabel("من تاريخ:");
            fromLabel.Location = new Point(1030, 30);
            filterPanel.Controls.Add(fromLabel);

            fromDatePicker = new DateTimePicker
            {
                Format = DateTimePickerFormat.Short,
                Width = 160,
                Font = new Font("Tahoma", 10),
                Location = new Point(850, 25)
            };
            filterPanel.Controls.Add(fromDatePicker);

            Label toLabel = CreateLabel("إلى تاريخ:");
            toLabel.Location = new Point(760, 30);
            filterPanel.Controls.Add(toLabel);

            toDatePicker = new DateTimePicker
            {
                Format = DateTimePickerFormat.Short,
                Width = 160,
                Font = new Font("Tahoma", 10),
                Location = new Point(580, 25)
            };
            filterPanel.Controls.Add(toDatePicker);

            searchButton = CreateButton(
                "عرض التقرير",
                DrawingColor.FromArgb(45, 130, 200));
            searchButton.Location = new Point(455, 22);
            searchButton.Click += SearchButton_Click;
            filterPanel.Controls.Add(searchButton);

            clearButton = CreateButton(
                "مسح",
                DrawingColor.FromArgb(120, 125, 130));
            clearButton.Location = new Point(345, 22);
            clearButton.Click += ClearButton_Click;
            filterPanel.Controls.Add(clearButton);

            pdfButton = CreateButton(
                "تصدير PDF",
                DrawingColor.FromArgb(190, 65, 65));
            pdfButton.Width = 120;
            pdfButton.Location = new Point(210, 22);
            pdfButton.Click += PdfButton_Click;
            filterPanel.Controls.Add(pdfButton);

            excelButton = CreateButton(
                "تصدير Excel",
                DrawingColor.FromArgb(35, 125, 75));
            excelButton.Width = 120;
            excelButton.Location = new Point(75, 22);
            excelButton.Click += ExcelButton_Click;
            filterPanel.Controls.Add(excelButton);

            TableLayoutPanel statsLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 1,
                RightToLeft = RightToLeft.Yes
            };

            statsLayout.ColumnStyles.Add(
                new ColumnStyle(SizeType.Percent, 50));
            statsLayout.ColumnStyles.Add(
                new ColumnStyle(SizeType.Percent, 50));

            mainLayout.Controls.Add(statsLayout, 0, 2);

            Panel totalCard = CreateStatCard(
                "إجمالي المبالغ",
                "0 جنيه",
                DrawingColor.FromArgb(45, 190, 145),
                out totalAmountLabel);

            statsLayout.Controls.Add(totalCard, 0, 0);

            Panel countCard = CreateStatCard(
                "عدد التحويلات",
                "0 تحويل",
                DrawingColor.FromArgb(55, 130, 220),
                out transferCountLabel);

            statsLayout.Controls.Add(countCard, 1, 0);

            TabControl tabControl = new TabControl
            {
                Dock = DockStyle.Fill,
                Font = new Font("Tahoma", 10, FontStyle.Bold),
                RightToLeft = RightToLeft.Yes,
                RightToLeftLayout = true
            };

            TabPage detailsTab = new TabPage("تفاصيل التحويلات")
            {
                BackColor = DrawingColor.White
            };

            reportGrid = CreateGrid();

            reportGrid.Columns.Add("Date", "التاريخ");
            reportGrid.Columns.Add("Beneficiary", "المستفيد");
            reportGrid.Columns.Add("Amount", "المبلغ");
            reportGrid.Columns.Add("Category", "التصنيف");
            reportGrid.Columns.Add("Bank", "البنك");
            reportGrid.Columns.Add("Reference", "رقم الإشعار");
            reportGrid.Columns.Add("Description", "التعليق");

            detailsTab.Controls.Add(reportGrid);

            TabPage categoryTab = new TabPage("حسب التصنيف")
            {
                BackColor = DrawingColor.White
            };

            categoryGrid = CreateGrid();
            categoryGrid.Columns.Add("Category", "التصنيف");
            categoryGrid.Columns.Add("Count", "عدد التحويلات");
            categoryGrid.Columns.Add("Amount", "إجمالي المبلغ");
            categoryTab.Controls.Add(categoryGrid);

            TabPage bankTab = new TabPage("حسب البنك")
            {
                BackColor = DrawingColor.White
            };

            bankGrid = CreateGrid();
            bankGrid.Columns.Add("Bank", "البنك");
            bankGrid.Columns.Add("Count", "عدد التحويلات");
            bankGrid.Columns.Add("Amount", "إجمالي المبلغ");
            bankTab.Controls.Add(bankGrid);

            tabControl.TabPages.Add(detailsTab);
            tabControl.TabPages.Add(categoryTab);
            tabControl.TabPages.Add(bankTab);

            mainLayout.Controls.Add(tabControl, 0, 3);
        }

        private Label CreateLabel(string text)
        {
            return new Label
            {
                Text = text,
                AutoSize = true,
                Font = new Font("Tahoma", 10, FontStyle.Bold),
                ForeColor = DrawingColor.FromArgb(40, 55, 70)
            };
        }

        private Button CreateButton(string text, DrawingColor color)
        {
            return new Button
            {
                Text = text,
                Width = 105,
                Height = 42,
                BackColor = color,
                ForeColor = DrawingColor.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Tahoma", 9, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
        }

        private DataGridView CreateGrid()
        {
            DataGridView grid = new DataGridView
            {
                Dock = DockStyle.Fill,
                RightToLeft = RightToLeft.Yes,
                BackgroundColor = DrawingColor.White,
                BorderStyle = BorderStyle.None,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AllowUserToResizeRows = false,
                ReadOnly = true,
                RowHeadersVisible = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                RowTemplate = { Height = 36 },
                ColumnHeadersHeight = 42,
                EnableHeadersVisualStyles = false
            };

            grid.ColumnHeadersDefaultCellStyle.BackColor =
                DrawingColor.FromArgb(20, 60, 95);
            grid.ColumnHeadersDefaultCellStyle.ForeColor =
                DrawingColor.White;
            grid.ColumnHeadersDefaultCellStyle.Font =
                new Font("Tahoma", 9, FontStyle.Bold);
            grid.ColumnHeadersDefaultCellStyle.Alignment =
                DataGridViewContentAlignment.MiddleCenter;

            grid.DefaultCellStyle.Font = new Font("Tahoma", 9);
            grid.DefaultCellStyle.Alignment =
                DataGridViewContentAlignment.MiddleCenter;

            grid.DefaultCellStyle.SelectionBackColor =
                DrawingColor.FromArgb(210, 225, 240);
            grid.DefaultCellStyle.SelectionForeColor =
                DrawingColor.Black;

            return grid;
        }

        private Panel CreateStatCard(
            string title,
            string value,
            DrawingColor color,
            out Label valueLabel)
        {
            Panel card = new Panel
            {
                Dock = DockStyle.Fill,
                Margin = new Padding(5),
                BackColor = DrawingColor.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            Panel colorBar = new Panel
            {
                Dock = DockStyle.Left,
                Width = 7,
                BackColor = color
            };

            card.Controls.Add(colorBar);

            Label titleLabel = new Label
            {
                Text = title,
                Dock = DockStyle.Top,
                Height = 35,
                Padding = new Padding(10, 5, 10, 0),
                Font = new Font("Tahoma", 9),
                ForeColor = DrawingColor.Gray,
                TextAlign = ContentAlignment.MiddleRight
            };

            card.Controls.Add(titleLabel);

            valueLabel = new Label
            {
                Text = value,
                Dock = DockStyle.Fill,
                Padding = new Padding(10, 0, 10, 5),
                Font = new Font("Tahoma", 20, FontStyle.Bold),
                ForeColor = color,
                TextAlign = ContentAlignment.MiddleRight
            };

            card.Controls.Add(valueLabel);

            return card;
        }

        private void SearchButton_Click(object sender, EventArgs e)
        {
            if (!ValidateDates())
                return;

            LoadReport();
        }

        private bool ValidateDates()
        {
            if (fromDatePicker.Value.Date > toDatePicker.Value.Date)
            {
                MessageBox.Show(
                    "تاريخ البداية يجب أن يكون قبل تاريخ النهاية.",
                    "تنبيه",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return false;
            }

            return true;
        }

        private void LoadReport()
        {
            try
            {
                reportGrid.Rows.Clear();
                categoryGrid.Rows.Clear();
                bankGrid.Rows.Clear();

                DateTime fromDate = fromDatePicker.Value.Date;
                DateTime toDate = toDatePicker.Value.Date;

                decimal totalAmount = 0;
                int transferCount = 0;

                using (var connection = Database.GetConnection())
                {
                    string detailsSql = @"
                        SELECT
                            TransferDate,
                            Beneficiary,
                            Amount,
                            Category,
                            Bank,
                            ReferenceNumber,
                            Description
                        FROM Transfers
                        WHERE date(TransferDate)
                        BETWEEN date($fromDate)
                        AND date($toDate)
                        ORDER BY Id DESC;";

                    using (var command = new SqliteCommand(detailsSql, connection))
                    {
                        command.Parameters.AddWithValue(
                            "$fromDate",
                            fromDate.ToString("yyyy-MM-dd"));

                        command.Parameters.AddWithValue(
                            "$toDate",
                            toDate.ToString("yyyy-MM-dd"));

                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string date = reader["TransferDate"]?.ToString() ?? "";
                                string beneficiary = reader["Beneficiary"]?.ToString() ?? "";

                                decimal amount = 0;

                                if (reader["Amount"] != DBNull.Value)
                                {
                                    decimal.TryParse(
                                        reader["Amount"].ToString(),
                                        out amount);
                                }

                                string category = reader["Category"]?.ToString() ?? "";
                                string bank = reader["Bank"]?.ToString() ?? "";
                                string reference = reader["ReferenceNumber"]?.ToString() ?? "";
                                string description = reader["Description"]?.ToString() ?? "";

                                reportGrid.Rows.Add(
                                    date.Replace("-", "/"),
                                    beneficiary,
                                    FormatNumber(amount),
                                    category,
                                    bank,
                                    reference,
                                    description);

                                totalAmount += amount;
                                transferCount++;
                            }
                        }
                    }

                    string categorySql = @"
                        SELECT
                            Category,
                            COUNT(*) AS TransferCount,
                            SUM(Amount) AS TotalAmount
                        FROM Transfers
                        WHERE date(TransferDate)
                        BETWEEN date($fromDate)
                        AND date($toDate)
                        GROUP BY Category
                        ORDER BY TotalAmount DESC;";

                    using (var command = new SqliteCommand(categorySql, connection))
                    {
                        command.Parameters.AddWithValue(
                            "$fromDate",
                            fromDate.ToString("yyyy-MM-dd"));

                        command.Parameters.AddWithValue(
                            "$toDate",
                            toDate.ToString("yyyy-MM-dd"));

                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string category = reader["Category"]?.ToString() ?? "";

                                int count = Convert.ToInt32(reader["TransferCount"]);

                                decimal amount =
                                    reader["TotalAmount"] == DBNull.Value
                                        ? 0
                                        : Convert.ToDecimal(reader["TotalAmount"]);

                                categoryGrid.Rows.Add(
                                    category,
                                    count.ToString("N0"),
                                    FormatNumber(amount));
                            }
                        }
                    }

                    string bankSql = @"
                        SELECT
                            Bank,
                            COUNT(*) AS TransferCount,
                            SUM(Amount) AS TotalAmount
                        FROM Transfers
                        WHERE date(TransferDate)
                        BETWEEN date($fromDate)
                        AND date($toDate)
                        GROUP BY Bank
                        ORDER BY TotalAmount DESC;";

                    using (var command = new SqliteCommand(bankSql, connection))
                    {
                        command.Parameters.AddWithValue(
                            "$fromDate",
                            fromDate.ToString("yyyy-MM-dd"));

                        command.Parameters.AddWithValue(
                            "$toDate",
                            toDate.ToString("yyyy-MM-dd"));

                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string bank = reader["Bank"]?.ToString() ?? "";

                                int count = Convert.ToInt32(reader["TransferCount"]);

                                decimal amount =
                                    reader["TotalAmount"] == DBNull.Value
                                        ? 0
                                        : Convert.ToDecimal(reader["TotalAmount"]);

                                bankGrid.Rows.Add(
                                    bank,
                                    count.ToString("N0"),
                                    FormatNumber(amount));
                            }
                        }
                    }
                }

                totalAmountLabel.Text = FormatNumber(totalAmount) + " جنيه";
                transferCountLabel.Text = transferCount.ToString("N0") + " تحويل";
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "حدث خطأ أثناء إنشاء التقرير:\n\n" + ex.Message,
                    "خطأ",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void ClearButton_Click(object sender, EventArgs e)
        {
            fromDatePicker.Value = DateTime.Today;
            toDatePicker.Value = DateTime.Today;

            reportGrid.Rows.Clear();
            categoryGrid.Rows.Clear();
            bankGrid.Rows.Clear();

            totalAmountLabel.Text = "0 جنيه";
            transferCountLabel.Text = "0 تحويل";
        }

        private void PdfButton_Click(object sender, EventArgs e)
        {
            if (!ValidateDates())
                return;

            if (reportGrid.Rows.Count == 0)
            {
                MessageBox.Show(
                    "لا توجد بيانات لتصديرها.",
                    "تنبيه",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            try
            {
                using (SaveFileDialog dialog = new SaveFileDialog())
                {
                    dialog.Title = "حفظ التقرير بصيغة PDF";
                    dialog.Filter = "PDF Files|*.pdf";
                    dialog.FileName =
                        "تقرير التحويلات_" +
                        DateTime.Now.ToString("yyyyMMdd_HHmm") +
                        ".pdf";

                    if (dialog.ShowDialog() != DialogResult.OK)
                        return;

                    CreatePdf(dialog.FileName);

                    MessageBox.Show(
                        "تم تصدير التقرير إلى PDF بنجاح.",
                        "تم التصدير",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    OpenCreatedFile(dialog.FileName);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "حدث خطأ أثناء تصدير التقرير:\n\n" + ex.Message,
                    "خطأ",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void CreatePdf(string filePath)
        {
            DateTime fromDate = fromDatePicker.Value.Date;
            DateTime toDate = toDatePicker.Value.Date;

            QuestPDF.Settings.License = LicenseType.Community;

            RegisterPdfFonts();

            Document.Create(document =>
            {
                document.Page(page =>
                {
                    page.Size(PageSizes.A4.Landscape());
                    page.Margin(25);
                    page.ContentFromRightToLeft();

                    page.DefaultTextStyle(style =>
                        style.FontFamily("Tahoma").FontSize(8));

                    page.Header()
                        .Column(column =>
                        {
                            column.Item()
                                .AlignCenter()
                                .Text("تقرير التحويلات")
                                .FontFamily("Tahoma")
                                .FontSize(20)
                                .Bold();

                            column.Item()
                                .PaddingTop(5)
                                .AlignCenter()
                                .Text(
                                    "من " +
                                    fromDate.ToString("yyyy/MM/dd") +
                                    " إلى " +
                                    toDate.ToString("yyyy/MM/dd"))
                                .FontFamily("Tahoma")
                                .FontSize(10);

                            column.Item().PaddingBottom(10);
                        });

                    page.Content()
                        .Column(column =>
                        {
                            column.Spacing(10);

                            column.Item()
                                .Row(row =>
                                {
                                    row.RelativeItem()
                                        .Border(1)
                                        .Padding(8)
                                        .AlignCenter()
                                        .Column(c =>
                                        {
                                            c.Item()
                                                .AlignCenter()
                                                .Text("إجمالي المبالغ")
                                                .FontFamily("Tahoma")
                                                .Bold();

                                            c.Item()
                                                .PaddingTop(3)
                                                .AlignCenter()
                                                .Text(totalAmountLabel.Text)
                                                .FontFamily("Tahoma")
                                                .FontSize(14)
                                                .Bold();
                                        });

                                    row.RelativeItem()
                                        .Border(1)
                                        .Padding(8)
                                        .AlignCenter()
                                        .Column(c =>
                                        {
                                            c.Item()
                                                .AlignCenter()
                                                .Text("عدد التحويلات")
                                                .FontFamily("Tahoma")
                                                .Bold();

                                            c.Item()
                                                .PaddingTop(3)
                                                .AlignCenter()
                                                .Text(transferCountLabel.Text)
                                                .FontFamily("Tahoma")
                                                .FontSize(14)
                                                .Bold();
                                        });
                                });

                            column.Item()
                                .PaddingTop(5)
                                .Text("تفاصيل التحويلات")
                                .FontFamily("Tahoma")
                                .FontSize(13)
                                .Bold();

                            column.Item()
                                .Table(table =>
                                {
                                    table.ColumnsDefinition(columns =>
                                    {
                                        columns.RelativeColumn(1.2f);
                                        columns.RelativeColumn(2.2f);
                                        columns.RelativeColumn(1.4f);
                                        columns.RelativeColumn(1.6f);
                                        columns.RelativeColumn(1.5f);
                                        columns.RelativeColumn(1.8f);
                                        columns.RelativeColumn(2.8f);
                                    });

                                    table.Header(header =>
                                    {
                                        header.Cell().Element(PdfHeaderCell).Text("التاريخ");
                                        header.Cell().Element(PdfHeaderCell).Text("المستفيد");
                                        header.Cell().Element(PdfHeaderCell).Text("المبلغ");
                                        header.Cell().Element(PdfHeaderCell).Text("التصنيف");
                                        header.Cell().Element(PdfHeaderCell).Text("البنك");
                                        header.Cell().Element(PdfHeaderCell).Text("رقم الإشعار");
                                        header.Cell().Element(PdfHeaderCell).Text("التعليق");
                                    });

                                    foreach (DataGridViewRow row in reportGrid.Rows)
                                    {
                                        if (row.IsNewRow)
                                            continue;

                                        table.Cell().Element(PdfBodyCell)
                                            .Text(GetCellValue(row, "Date"));

                                        table.Cell().Element(PdfBodyCell)
                                            .Text(GetCellValue(row, "Beneficiary"));

                                        table.Cell().Element(PdfBodyCell)
                                            .Text(GetCellValue(row, "Amount"));

                                        table.Cell().Element(PdfBodyCell)
                                            .Text(GetCellValue(row, "Category"));

                                        table.Cell().Element(PdfBodyCell)
                                            .Text(GetCellValue(row, "Bank"));

                                        table.Cell().Element(PdfBodyCell)
                                            .Text(GetCellValue(row, "Reference"));

                                        table.Cell().Element(PdfBodyCell)
                                            .DefaultTextStyle(style =>
                                                style.FontFamily("Tahoma", "Segoe UI Emoji")
                                                    .FontSize(8))
                                            .Text(GetCellValue(row, "Description"));
                                    }
                                });

                            column.Item()
                                .PaddingTop(8)
                                .Text("ملخص حسب التصنيف")
                                .FontFamily("Tahoma")
                                .FontSize(13)
                                .Bold();

                            column.Item()
                                .Table(table =>
                                {
                                    table.ColumnsDefinition(columns =>
                                    {
                                        columns.RelativeColumn(2.5f);
                                        columns.RelativeColumn(1f);
                                        columns.RelativeColumn(2f);
                                    });

                                    table.Header(header =>
                                    {
                                        header.Cell().Element(PdfHeaderCell).Text("التصنيف");
                                        header.Cell().Element(PdfHeaderCell).Text("العدد");
                                        header.Cell().Element(PdfHeaderCell).Text("الإجمالي");
                                    });

                                    foreach (DataGridViewRow row in categoryGrid.Rows)
                                    {
                                        if (row.IsNewRow)
                                            continue;

                                        table.Cell().Element(PdfBodyCell)
                                            .Text(GetCellValue(row, "Category"));

                                        table.Cell().Element(PdfBodyCell)
                                            .Text(GetCellValue(row, "Count"));

                                        table.Cell().Element(PdfBodyCell)
                                            .Text(GetCellValue(row, "Amount"));
                                    }
                                });

                            column.Item()
                                .PaddingTop(8)
                                .Text("ملخص حسب البنك")
                                .FontFamily("Tahoma")
                                .FontSize(13)
                                .Bold();

                            column.Item()
                                .Table(table =>
                                {
                                    table.ColumnsDefinition(columns =>
                                    {
                                        columns.RelativeColumn(2.5f);
                                        columns.RelativeColumn(1f);
                                        columns.RelativeColumn(2f);
                                    });

                                    table.Header(header =>
                                    {
                                        header.Cell().Element(PdfHeaderCell).Text("البنك");
                                        header.Cell().Element(PdfHeaderCell).Text("العدد");
                                        header.Cell().Element(PdfHeaderCell).Text("الإجمالي");
                                    });

                                    foreach (DataGridViewRow row in bankGrid.Rows)
                                    {
                                        if (row.IsNewRow)
                                            continue;

                                        table.Cell().Element(PdfBodyCell)
                                            .Text(GetCellValue(row, "Bank"));

                                        table.Cell().Element(PdfBodyCell)
                                            .Text(GetCellValue(row, "Count"));

                                        table.Cell().Element(PdfBodyCell)
                                            .Text(GetCellValue(row, "Amount"));
                                    }
                                });
                        });

                    page.Footer()
                        .AlignCenter()
                        .Text(text =>
                        {
                            text.DefaultTextStyle(x => x
                                .FontFamily("Tahoma")
                                .FontSize(8));

                            text.Span("تم إنشاء التقرير بتاريخ ");
                            text.Span(
                                DateTime.Now.ToString("yyyy/MM/dd HH:mm"));
                            text.Span("  |  صفحة ");
                            text.CurrentPageNumber();
                            text.Span(" من ");
                            text.TotalPages();
                        });
                });
            })
            .GeneratePdf(filePath);
        }

        private void RegisterPdfFonts()
        {
            try
            {
                string fontsFolder =
                    Environment.GetFolderPath(
                        Environment.SpecialFolder.Fonts);

                string regularFont =
                    Path.Combine(fontsFolder, "tahoma.ttf");

                string boldFont =
                    Path.Combine(fontsFolder, "tahomabd.ttf");

                string emojiFont =
                    Path.Combine(fontsFolder, "seguiemj.ttf");

                if (File.Exists(regularFont))
                {
                    QuestPDF.Drawing.FontManager.RegisterFont(
                        File.OpenRead(regularFont));
                }

                if (File.Exists(boldFont))
                {
                    QuestPDF.Drawing.FontManager.RegisterFont(
                        File.OpenRead(boldFont));
                }

                if (File.Exists(emojiFont))
                {
                    QuestPDF.Drawing.FontManager.RegisterFont(
                        File.OpenRead(emojiFont));
                }
            }
            catch
            {
                // إذا تعذر تسجيل Tahoma يستخدم QuestPDF الخط المتاح.
            }
        }

        private IContainer PdfHeaderCell(IContainer container)
        {
            return container
                .Background(Colors.Blue.Darken3)
                .Border(0.5f)
                .BorderColor(Colors.White)
                .Padding(5)
                .AlignCenter()
                .AlignMiddle()
                .DefaultTextStyle(style =>
                    style.FontFamily("Tahoma")
                        .FontSize(8)
                        .FontColor(Colors.White)
                        .Bold());
        }

        private IContainer PdfBodyCell(IContainer container)
        {
            return container
                .Border(0.5f)
                .BorderColor(Colors.Grey.Lighten1)
                .Padding(4)
                .AlignCenter()
                .AlignMiddle()
                .DefaultTextStyle(style =>
                    style.FontFamily("Tahoma")
                        .FontSize(8));
        }

        private void ExcelButton_Click(object sender, EventArgs e)
        {
            if (!ValidateDates())
                return;

            if (reportGrid.Rows.Count == 0)
            {
                MessageBox.Show(
                    "لا توجد بيانات لتصديرها.",
                    "تنبيه",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            try
            {
                using (SaveFileDialog dialog = new SaveFileDialog())
                {
                    dialog.Title = "حفظ التقرير بصيغة Excel";
                    dialog.Filter = "Excel Files|*.xlsx";
                    dialog.FileName =
                        "تقرير التحويلات_" +
                        DateTime.Now.ToString("yyyyMMdd_HHmm") +
                        ".xlsx";

                    if (dialog.ShowDialog() != DialogResult.OK)
                        return;

                    CreateExcel(dialog.FileName);

                    MessageBox.Show(
                        "تم تصدير التقرير إلى Excel بنجاح.",
                        "تم التصدير",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    OpenCreatedFile(dialog.FileName);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "حدث خطأ أثناء تصدير Excel:\n\n" + ex.Message,
                    "خطأ",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void CreateExcel(string filePath)
        {
            using (XLWorkbook workbook = new XLWorkbook())
            {
                IXLWorksheet details =
                    workbook.Worksheets.Add("تفاصيل التحويلات");

                details.RightToLeft = true;

                details.Cell(1, 1).Value = "تقرير التحويلات";
                details.Range(1, 1, 1, 7).Merge();

                details.Cell(1, 1).Style.Font.Bold = true;
                details.Cell(1, 1).Style.Font.FontSize = 18;
                details.Cell(1, 1).Style.Alignment.Horizontal =
                    XLAlignmentHorizontalValues.Center;

                details.Cell(2, 1).Value = "من تاريخ";
                details.Cell(2, 2).Value = fromDatePicker.Value.Date;
                details.Cell(2, 3).Value = "إلى تاريخ";
                details.Cell(2, 4).Value = toDatePicker.Value.Date;

                details.Cell(3, 1).Value = "إجمالي المبالغ";
                details.Cell(3, 2).Value = totalAmountLabel.Text;
                details.Cell(3, 3).Value = "عدد التحويلات";
                details.Cell(3, 4).Value = transferCountLabel.Text;

                string[] headers =
                {
                    "التاريخ",
                    "المستفيد",
                    "المبلغ",
                    "التصنيف",
                    "البنك",
                    "رقم الإشعار",
                    "التعليق"
                };

                for (int i = 0; i < headers.Length; i++)
                {
                    details.Cell(5, i + 1).Value = headers[i];
                }

                int detailRow = 6;

                foreach (DataGridViewRow row in reportGrid.Rows)
                {
                    if (row.IsNewRow)
                        continue;

                    details.Cell(detailRow, 1).Value =
                        GetCellValue(row, "Date");

                    details.Cell(detailRow, 2).Value =
                        GetCellValue(row, "Beneficiary");

                    details.Cell(detailRow, 3).Value =
                        GetCellValue(row, "Amount");

                    details.Cell(detailRow, 4).Value =
                        GetCellValue(row, "Category");

                    details.Cell(detailRow, 5).Value =
                        GetCellValue(row, "Bank");

                    details.Cell(detailRow, 6).Value =
                        GetCellValue(row, "Reference");

                    details.Cell(detailRow, 7).Value =
                        GetCellValue(row, "Description");

                    detailRow++;
                }

                IXLRange detailHeader =
                    details.Range(5, 1, 5, 7);

                StyleExcelHeader(detailHeader);

                details.Columns().AdjustToContents();

                IXLWorksheet categories =
                    workbook.Worksheets.Add("حسب التصنيف");

                categories.RightToLeft = true;

                categories.Cell(1, 1).Value =
                    "ملخص التحويلات حسب التصنيف";

                categories.Range(1, 1, 1, 3).Merge();

                categories.Cell(1, 1).Style.Font.Bold = true;
                categories.Cell(1, 1).Style.Font.FontSize = 16;
                categories.Cell(1, 1).Style.Alignment.Horizontal =
                    XLAlignmentHorizontalValues.Center;

                categories.Cell(3, 1).Value = "التصنيف";
                categories.Cell(3, 2).Value = "عدد التحويلات";
                categories.Cell(3, 3).Value = "إجمالي المبلغ";

                int categoryRow = 4;

                foreach (DataGridViewRow row in categoryGrid.Rows)
                {
                    if (row.IsNewRow)
                        continue;

                    categories.Cell(categoryRow, 1).Value =
                        GetCellValue(row, "Category");

                    categories.Cell(categoryRow, 2).Value =
                        GetCellValue(row, "Count");

                    categories.Cell(categoryRow, 3).Value =
                        GetCellValue(row, "Amount");

                    categoryRow++;
                }

                IXLRange categoryHeader =
                    categories.Range(3, 1, 3, 3);

                StyleExcelHeader(categoryHeader);

                categories.Columns().AdjustToContents();

                IXLWorksheet banks =
                    workbook.Worksheets.Add("حسب البنك");

                banks.RightToLeft = true;

                banks.Cell(1, 1).Value =
                    "ملخص التحويلات حسب البنك";

                banks.Range(1, 1, 1, 3).Merge();

                banks.Cell(1, 1).Style.Font.Bold = true;
                banks.Cell(1, 1).Style.Font.FontSize = 16;
                banks.Cell(1, 1).Style.Alignment.Horizontal =
                    XLAlignmentHorizontalValues.Center;

                banks.Cell(3, 1).Value = "البنك";
                banks.Cell(3, 2).Value = "عدد التحويلات";
                banks.Cell(3, 3).Value = "إجمالي المبلغ";

                int bankRow = 4;

                foreach (DataGridViewRow row in bankGrid.Rows)
                {
                    if (row.IsNewRow)
                        continue;

                    banks.Cell(bankRow, 1).Value =
                        GetCellValue(row, "Bank");

                    banks.Cell(bankRow, 2).Value =
                        GetCellValue(row, "Count");

                    banks.Cell(bankRow, 3).Value =
                        GetCellValue(row, "Amount");

                    bankRow++;
                }

                IXLRange bankHeader =
                    banks.Range(3, 1, 3, 3);

                StyleExcelHeader(bankHeader);

                banks.Columns().AdjustToContents();

                workbook.SaveAs(filePath);
            }
        }

        private void StyleExcelHeader(IXLRange range)
        {
            range.Style.Font.Bold = true;
            range.Style.Fill.BackgroundColor =
                XLColor.FromHtml("#143C5F");
            range.Style.Font.FontColor = XLColor.White;
            range.Style.Alignment.Horizontal =
                XLAlignmentHorizontalValues.Center;
            range.Style.Alignment.Vertical =
                XLAlignmentVerticalValues.Center;
        }

        private void OpenCreatedFile(string filePath)
        {
            try
            {
                if (!File.Exists(filePath))
                    return;

                System.Diagnostics.Process.Start(
                    new System.Diagnostics.ProcessStartInfo
                    {
                        FileName = filePath,
                        UseShellExecute = true
                    });
            }
            catch
            {
                // الملف محفوظ حتى لو تعذر فتحه تلقائياً.
            }
        }

        private string GetCellValue(
            DataGridViewRow row,
            string columnName)
        {
            return row.Cells[columnName].Value?.ToString() ?? "";
        }

        private string FormatNumber(decimal number)
        {
            return number.ToString("N0");
        }
    }
}
