using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Data.Sqlite;

namespace TransferManagementSystem
{
    public class AddTransferForm : Form
    {
        private DateTimePicker dtpDate;
        private TextBox txtBeneficiary;
        private TextBox txtAmount;
        private TextBox txtReference;
        private TextBox txtDescription;
        private ComboBox cmbCategory;
        private ComboBox cmbBank;
        private TextBox txtReceiptImage;

        private Button btnSelectReceipt;
        private Button btnExtractData;
        private Button btnSaveAll;
        private Button btnEditSelected;
        private Button btnDeleteSelected;

        private Label selectedFilesLabel;
        private Label progressLabel;
        private Label gridTitle;
        private Label receiptTitle;
        private Label receiptDescription;

        private DataGridView extractedGrid;
        private Panel receiptPanelForLayout;

        private readonly List<string> selectedReceiptFiles =
            new List<string>();

        private readonly List<ReceiptData> extractedReceipts =
            new List<ReceiptData>();

        private static readonly HttpClient GeminiHttpClient =
            CreateHttpClient();

        private static readonly string[] GeminiModels =
        {
            "gemini-3.8-flash",
            "gemini-3.7-flash",
            "gemini-3.6-flash",
            "gemini-3.5-flash-lite"
        };

        private static HttpClient CreateHttpClient()
        {
            HttpClient client = new HttpClient();

            client.Timeout =
                TimeSpan.FromSeconds(120);

            return client;
        }

        private class ReceiptData
        {
            public string FilePath { get; set; } = "";
            public string Language { get; set; } = "";
            public string Date { get; set; } = "";
            public string Beneficiary { get; set; } = "";
            public string Amount { get; set; } = "";
            public string Reference { get; set; } = "";
            public string Bank { get; set; } = "";
            public string Description { get; set; } = "";
            public string Category { get; set; } = "";
            public string Error { get; set; } = "";
        }

        public AddTransferForm()
        {
            CreateForm();
            LoadCategories();
        }

        private void CreateForm()
        {
            Text = "إضافة تحويل جديد";

            Size = new Size(1150, 900);

            MinimumSize =
                new Size(1000, 720);

            StartPosition =
                FormStartPosition.CenterScreen;

            AutoScroll = true;

            BackColor =
                Color.FromArgb(245, 247, 250);

            RightToLeft =
                RightToLeft.Yes;

            RightToLeftLayout = true;

            DoubleBuffered = true;

            Label title =
                new Label();

            title.Text =
                "إضافة تحويل جديد";

            title.Font =
                new Font(
                    "Tahoma",
                    22,
                    FontStyle.Bold);

            title.ForeColor =
                Color.FromArgb(20, 45, 70);

            title.AutoSize = true;

            title.Location =
                new Point(55, 22);

            Controls.Add(title);

            receiptPanelForLayout =
                new Panel();

            Panel receiptPanel =
                receiptPanelForLayout;

            receiptPanel.BackColor =
                Color.White;

            receiptPanel.BorderStyle =
                BorderStyle.FixedSingle;

            receiptPanel.Size =
                new Size(1000, 170);

            receiptPanel.Location =
                new Point(55, 70);

            receiptPanel.Anchor =
                AnchorStyles.Top
                | AnchorStyles.Left
                | AnchorStyles.Right;

            Controls.Add(receiptPanel);

            receiptTitle =
                new Label();

            receiptTitle.Text =
                "إشعار التحويل";

            receiptTitle.Font =
                new Font(
                    "Tahoma",
                    12,
                    FontStyle.Bold);

            receiptTitle.ForeColor =
                Color.FromArgb(25, 50, 75);

            receiptTitle.AutoSize = true;

            receiptTitle.Location =
                new Point(835, 12);

            receiptPanel.Controls.Add(
                receiptTitle);

            receiptDescription =
                new Label();

            receiptDescription.Text =
                "ارفع إشعاراً واحداً أو عدة إشعارات. سيتم تحليل الصورة مباشرة واستخراج الحقول.";

            receiptDescription.Font =
                new Font(
                    "Tahoma",
                    9);

            receiptDescription.ForeColor =
                Color.Gray;

            receiptDescription.AutoSize = true;

            receiptDescription.Location =
                new Point(365, 42);

            receiptPanel.Controls.Add(
                receiptDescription);

            txtReceiptImage =
                new TextBox();

            txtReceiptImage.Font =
                new Font(
                    "Tahoma",
                    9);

            txtReceiptImage.Size =
                new Size(620, 32);

            txtReceiptImage.Location =
                new Point(20, 78);

            txtReceiptImage.ReadOnly =
                true;

            txtReceiptImage.RightToLeft =
                RightToLeft.Yes;

            txtReceiptImage.Anchor =
                AnchorStyles.Left
                | AnchorStyles.Right;

            receiptPanel.Controls.Add(
                txtReceiptImage);

            btnSelectReceipt =
                new Button();

            btnSelectReceipt.Text =
                "رفع إشعار / إشعارات";

            btnSelectReceipt.Font =
                new Font(
                    "Tahoma",
                    9,
                    FontStyle.Bold);

            btnSelectReceipt.BackColor =
                Color.FromArgb(
                    40,
                    100,
                    150);

            btnSelectReceipt.ForeColor =
                Color.White;

            btnSelectReceipt.FlatStyle =
                FlatStyle.Flat;

            btnSelectReceipt.Size =
                new Size(220, 32);

            btnSelectReceipt.Location =
                new Point(750, 78);

            btnSelectReceipt.Cursor =
                Cursors.Hand;

            btnSelectReceipt.FlatAppearance
                .BorderSize = 0;

            btnSelectReceipt.Click +=
                BtnImage_Click;

            btnSelectReceipt.Anchor =
                AnchorStyles.Top
                | AnchorStyles.Right;

            receiptPanel.Controls.Add(
                btnSelectReceipt);

            btnExtractData =
                new Button();

            btnExtractData.Text =
                "استخراج البيانات بالذكاء الاصطناعي";

            btnExtractData.Font =
                new Font(
                    "Tahoma",
                    9,
                    FontStyle.Bold);

            btnExtractData.BackColor =
                Color.FromArgb(
                    20,
                    125,
                    90);

            btnExtractData.ForeColor =
                Color.White;

            btnExtractData.FlatStyle =
                FlatStyle.Flat;

            btnExtractData.Size =
                new Size(300, 34);

            btnExtractData.Location =
                new Point(350, 122);

            btnExtractData.Cursor =
                Cursors.Hand;

            btnExtractData.FlatAppearance
                .BorderSize = 0;

            btnExtractData.Click +=
                BtnExtractData_Click;

            btnExtractData.Anchor =
                AnchorStyles.Top;

            receiptPanel.Controls.Add(
                btnExtractData);

            selectedFilesLabel =
                new Label();

            selectedFilesLabel.Text =
                "لم يتم اختيار إشعارات بعد.";

            selectedFilesLabel.Font =
                new Font(
                    "Tahoma",
                    9,
                    FontStyle.Bold);

            selectedFilesLabel.ForeColor =
                Color.Gray;

            selectedFilesLabel.AutoSize = true;

            selectedFilesLabel.Location =
                new Point(20, 126);

            receiptPanel.Controls.Add(
                selectedFilesLabel);

            progressLabel =
                new Label();

            progressLabel.Text = "";

            progressLabel.Font =
                new Font(
                    "Tahoma",
                    9,
                    FontStyle.Bold);

            progressLabel.ForeColor =
                Color.FromArgb(
                    20,
                    125,
                    90);

            progressLabel.AutoSize = true;

            progressLabel.Location =
                new Point(700, 126);

            progressLabel.Anchor =
                AnchorStyles.Top
                | AnchorStyles.Right;

            receiptPanel.Controls.Add(
                progressLabel);

            AddLabel(
                "التاريخ",
                55,
                260);

            dtpDate =
                new DateTimePicker();

            dtpDate.Format =
                DateTimePickerFormat.Short;

            dtpDate.Font =
                new Font(
                    "Tahoma",
                    11);

            dtpDate.Size =
                new Size(330, 35);

            dtpDate.Location =
                new Point(55, 290);

            dtpDate.Anchor =
                AnchorStyles.Top
                | AnchorStyles.Left;

            Controls.Add(dtpDate);

            AddLabel(
                "اسم المستفيد",
                450,
                260);

            txtBeneficiary =
                new TextBox();

            txtBeneficiary.Font =
                new Font(
                    "Tahoma",
                    11);

            txtBeneficiary.Size =
                new Size(480, 35);

            txtBeneficiary.Location =
                new Point(450, 290);

            txtBeneficiary.RightToLeft =
                RightToLeft.Yes;

            txtBeneficiary.Anchor =
                AnchorStyles.Top
                | AnchorStyles.Left
                | AnchorStyles.Right;

            Controls.Add(txtBeneficiary);

            AddLabel(
                "المبلغ",
                55,
                360);

            txtAmount =
                new TextBox();

            txtAmount.Font =
                new Font(
                    "Tahoma",
                    11);

            txtAmount.Size =
                new Size(330, 35);

            txtAmount.Location =
                new Point(55, 390);

            txtAmount.RightToLeft =
                RightToLeft.Yes;

            txtAmount.Anchor =
                AnchorStyles.Top
                | AnchorStyles.Left;

            Controls.Add(txtAmount);

            AddLabel(
                "رقم العملية / المرجع",
                450,
                360);

            txtReference =
                new TextBox();

            txtReference.Font =
                new Font(
                    "Tahoma",
                    11);

            txtReference.Size =
                new Size(480, 35);

            txtReference.Location =
                new Point(450, 390);

            txtReference.RightToLeft =
                RightToLeft.Yes;

            txtReference.Anchor =
                AnchorStyles.Top
                | AnchorStyles.Left
                | AnchorStyles.Right;

            Controls.Add(txtReference);

            AddLabel(
                "البنك",
                55,
                430);

            cmbBank =
                new ComboBox();

            cmbBank.Font =
                new Font(
                    "Tahoma",
                    11);

            cmbBank.Size =
                new Size(330, 35);

            cmbBank.Location =
                new Point(55, 460);

            cmbBank.DropDownStyle =
                ComboBoxStyle.DropDownList;

            cmbBank.RightToLeft =
                RightToLeft.Yes;

            cmbBank.Items.AddRange(
                new object[]
                {
                    "بنك الخرطوم",
                    "بنك فيصل",
                    "بنك أم درمان",
                    "بنك البركة",
                    "بنك النيلين",
                    "أخرى"
                });

            if (cmbBank.Items.Count > 0)
                cmbBank.SelectedIndex = 0;

            cmbBank.Anchor =
                AnchorStyles.Top
                | AnchorStyles.Left;

            Controls.Add(cmbBank);

            AddLabel(
                "التصنيف",
                450,
                430);

            cmbCategory =
                new ComboBox();

            cmbCategory.Font =
                new Font(
                    "Tahoma",
                    11);

            cmbCategory.Size =
                new Size(480, 35);

            cmbCategory.Location =
                new Point(450, 460);

            cmbCategory.DropDownStyle =
                ComboBoxStyle.DropDownList;

            cmbCategory.RightToLeft =
                RightToLeft.Yes;

            cmbCategory.Anchor =
                AnchorStyles.Top
                | AnchorStyles.Left
                | AnchorStyles.Right;

            Controls.Add(cmbCategory);

            AddLabel(
                "التعليق / الوصف",
                55,
                500);

            txtDescription =
                new TextBox();

            txtDescription.Font =
                new Font(
                    "Tahoma",
                    11);

            txtDescription.Multiline = true;

            txtDescription.Size =
                new Size(875, 70);

            txtDescription.Location =
                new Point(55, 530);

            txtDescription.RightToLeft =
                RightToLeft.Yes;

            txtDescription.ScrollBars =
                ScrollBars.Vertical;

            txtDescription.Anchor =
                AnchorStyles.Top
                | AnchorStyles.Left
                | AnchorStyles.Right;

            Controls.Add(txtDescription);

            Button btnSave =
                new Button();

            btnSave.Text =
                "حفظ التحويل الحالي / حفظ القائمة";

            btnSave.Font =
                new Font(
                    "Tahoma",
                    12,
                    FontStyle.Bold);

            btnSave.BackColor =
                Color.FromArgb(
                    20,
                    100,
                    75);

            btnSave.ForeColor =
                Color.White;

            btnSave.FlatStyle =
                FlatStyle.Flat;

            btnSave.Size =
                new Size(875, 48);

            btnSave.Location =
                new Point(55, 615);

            btnSave.Cursor =
                Cursors.Hand;

            btnSave.FlatAppearance
                .BorderSize = 0;

            btnSave.Click +=
                BtnSave_Click;

            btnSave.Anchor =
                AnchorStyles.Top
                | AnchorStyles.Left
                | AnchorStyles.Right;

            Controls.Add(btnSave);

            gridTitle =
                new Label();

            gridTitle.Text =
                "مراجعة الإشعارات المستخرجة — يمكنك تعديل أي خلية مباشرة";

            gridTitle.Font =
                new Font(
                    "Tahoma",
                    13,
                    FontStyle.Bold);

            gridTitle.ForeColor =
                Color.FromArgb(
                    20,
                    45,
                    70);

            gridTitle.AutoSize = true;

            gridTitle.Location =
                new Point(55, 690);

            gridTitle.Visible = false;

            Controls.Add(gridTitle);

            extractedGrid =
                new DataGridView();

            extractedGrid.Visible =
                false;

            extractedGrid.Location =
                new Point(55, 725);

            extractedGrid.Size =
                new Size(1000, 300);

            extractedGrid.BackgroundColor =
                Color.White;

            extractedGrid.BorderStyle =
                BorderStyle.FixedSingle;

            extractedGrid.AllowUserToAddRows =
                false;

            extractedGrid.AllowUserToDeleteRows =
                false;

            extractedGrid.ReadOnly =
                false;

            extractedGrid.EditMode =
                DataGridViewEditMode
                    .EditOnKeystrokeOrF2;

            extractedGrid.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;

            extractedGrid.RowHeadersVisible =
                false;

            extractedGrid.RightToLeft =
                RightToLeft.Yes;

            extractedGrid.SelectionMode =
                DataGridViewSelectionMode
                    .FullRowSelect;

            extractedGrid.MultiSelect =
                false;

            extractedGrid.Columns.Add(
                "File",
                "الإشعار");

            extractedGrid.Columns.Add(
                "Language",
                "اللغة");

            extractedGrid.Columns.Add(
                "Date",
                "التاريخ");

            extractedGrid.Columns.Add(
                "Beneficiary",
                "اسم المستفيد");

            extractedGrid.Columns.Add(
                "Amount",
                "المبلغ");

            extractedGrid.Columns.Add(
                "Reference",
                "رقم العملية");

            extractedGrid.Columns.Add(
                "Description",
                "التعليق");

            extractedGrid.Columns.Add(
                "Bank",
                "البنك");

            extractedGrid.Columns.Add(
                "Category",
                "التصنيف");

            extractedGrid.Columns["File"]
                .ReadOnly = true;

            extractedGrid.Columns["Language"]
                .ReadOnly = true;

            extractedGrid.Anchor =
                AnchorStyles.Top
                | AnchorStyles.Left
                | AnchorStyles.Right;

            Controls.Add(extractedGrid);

            btnEditSelected =
                new Button();

            btnEditSelected.Text =
                "تحميل السطر المحدد إلى الحقول";

            btnEditSelected.Font =
                new Font(
                    "Tahoma",
                    10,
                    FontStyle.Bold);

            btnEditSelected.BackColor =
                Color.FromArgb(
                    50,
                    90,
                    140);

            btnEditSelected.ForeColor =
                Color.White;

            btnEditSelected.FlatStyle =
                FlatStyle.Flat;

            btnEditSelected.Size =
                new Size(320, 45);

            btnEditSelected.Location =
                new Point(55, 1040);

            btnEditSelected.Visible =
                false;

            btnEditSelected.Cursor =
                Cursors.Hand;

            btnEditSelected.FlatAppearance
                .BorderSize = 0;

            btnEditSelected.Click +=
                BtnEditSelected_Click;

            btnEditSelected.Anchor =
                AnchorStyles.Top
                | AnchorStyles.Left;

            Controls.Add(btnEditSelected);

            btnDeleteSelected =
                new Button();

            btnDeleteSelected.Text =
                "حذف السطر المحدد";

            btnDeleteSelected.Font =
                new Font(
                    "Tahoma",
                    10,
                    FontStyle.Bold);

            btnDeleteSelected.BackColor =
                Color.FromArgb(
                    190,
                    65,
                    65);

            btnDeleteSelected.ForeColor =
                Color.White;

            btnDeleteSelected.FlatStyle =
                FlatStyle.Flat;

            btnDeleteSelected.Size =
                new Size(240, 45);

            btnDeleteSelected.Location =
                new Point(390, 1040);

            btnDeleteSelected.Visible =
                false;

            btnDeleteSelected.Cursor =
                Cursors.Hand;

            btnDeleteSelected.FlatAppearance
                .BorderSize = 0;

            btnDeleteSelected.Click +=
                BtnDeleteSelected_Click;

            btnDeleteSelected.Anchor =
                AnchorStyles.Top
                | AnchorStyles.Left;

            Controls.Add(btnDeleteSelected);

            btnSaveAll =
                new Button();

            btnSaveAll.Text =
                "حفظ كل الإشعارات من الجدول";

            btnSaveAll.Font =
                new Font(
                    "Tahoma",
                    11,
                    FontStyle.Bold);

            btnSaveAll.BackColor =
                Color.FromArgb(
                    20,
                    100,
                    75);

            btnSaveAll.ForeColor =
                Color.White;

            btnSaveAll.FlatStyle =
                FlatStyle.Flat;

            btnSaveAll.Size =
                new Size(640, 45);

            btnSaveAll.Location =
                new Point(400, 1040);

            btnSaveAll.Visible =
                false;

            btnSaveAll.Cursor =
                Cursors.Hand;

            btnSaveAll.FlatAppearance
                .BorderSize = 0;

            btnSaveAll.Click +=
                BtnSaveAll_Click;

            btnSaveAll.Anchor =
                AnchorStyles.Top
                | AnchorStyles.Left
                | AnchorStyles.Right;

            Controls.Add(btnSaveAll);

            Resize +=
                (sender, e) =>
                {
                    ArrangeResponsiveLayout();
                };

            ArrangeResponsiveLayout();
        }

        private void ArrangeResponsiveLayout()
        {
            int width =
                Math.Max(
                    MinimumSize.Width,
                    ClientSize.Width);

            int horizontalMargin = 55;
            int availableWidth =
                Math.Max(
                    875,
                    width
                    - (horizontalMargin * 2));

            int leftColumnWidth =
                Math.Max(
                    300,
                    (availableWidth - 30) / 2);

            int rightColumnWidth =
                availableWidth
                - leftColumnWidth
                - 30;

            if (receiptPanelForLayout != null)
            {
                receiptPanelForLayout.Width =
                    availableWidth;

                txtReceiptImage.Width =
                    Math.Max(
                        350,
                        receiptPanelForLayout.ClientSize.Width
                        - btnSelectReceipt.Width
                        - 55);

                btnSelectReceipt.Left =
                    receiptPanelForLayout.ClientSize.Width
                    - btnSelectReceipt.Width
                    - 20;

                btnExtractData.Left =
                    (
                        receiptPanelForLayout.ClientSize.Width
                        - btnExtractData.Width
                    ) / 2;

                progressLabel.Left =
                    receiptPanelForLayout.ClientSize.Width
                    - progressLabel.Width
                    - 20;

                receiptTitle.Left =
                    receiptPanelForLayout.ClientSize.Width
                    - receiptTitle.Width
                    - 20;

                receiptDescription.Left =
                    Math.Max(
                        20,
                        (
                            receiptPanelForLayout.ClientSize.Width
                            - receiptDescription.Width
                        ) / 2);
            }

            dtpDate.Width =
                leftColumnWidth;

            txtAmount.Width =
                leftColumnWidth;

            cmbBank.Width =
                leftColumnWidth;

            txtBeneficiary.Left =
                horizontalMargin
                + leftColumnWidth
                + 30;

            txtBeneficiary.Width =
                rightColumnWidth;

            txtReference.Left =
                horizontalMargin
                + leftColumnWidth
                + 30;

            txtReference.Width =
                rightColumnWidth;

            cmbCategory.Left =
                horizontalMargin
                + leftColumnWidth
                + 30;

            cmbCategory.Width =
                rightColumnWidth;

            txtDescription.Width =
                availableWidth;

            extractedGrid.Width =
                availableWidth;

            int buttonsGap = 15;

            btnEditSelected.Left =
                horizontalMargin;

            btnDeleteSelected.Left =
                btnEditSelected.Left
                + btnEditSelected.Width
                + buttonsGap;

            btnSaveAll.Left =
                btnDeleteSelected.Left
                + btnDeleteSelected.Width
                + buttonsGap;

            btnSaveAll.Width =
                Math.Max(
                    300,
                    availableWidth
                    - btnEditSelected.Width
                    - btnDeleteSelected.Width
                    - (buttonsGap * 2));
        }

        private void AddLabel(
            string text,
            int x,
            int y)
        {
            Label label =
                new Label();

            label.Text = text;

            label.Font =
                new Font(
                    "Tahoma",
                    10,
                    FontStyle.Bold);

            label.ForeColor =
                Color.FromArgb(
                    50,
                    50,
                    50);

            label.AutoSize = true;

            label.Location =
                new Point(x, y);

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
                using (
                    SqliteCommand command =
                        new SqliteCommand(
                            "SELECT Name FROM Categories ORDER BY Id ASC;",
                            connection))
                using (
                    SqliteDataReader reader =
                        command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string name =
                            reader["Name"]?.ToString()
                            ?? "";

                        if (!string.IsNullOrWhiteSpace(
                            name))
                        {
                            cmbCategory.Items.Add(
                                name);
                        }
                    }
                }

                if (cmbCategory.Items.Count > 0)
                    cmbCategory.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "حدث خطأ أثناء تحميل التصنيفات:\n\n"
                    + ex.Message,
                    "خطأ",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
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
                    "اختيار إشعار أو عدة إشعارات";

                dialog.Filter =
                    "صور|*.jpg;*.jpeg;*.png;*.bmp;*.webp|كل الملفات|*.*";

                dialog.Multiselect = true;

                if (dialog.ShowDialog()
                    != DialogResult.OK)
                {
                    return;
                }

                selectedReceiptFiles.Clear();

                foreach (string file in dialog.FileNames)
                {
                    bool alreadySelected = false;

                    foreach (string existingFile
                        in selectedReceiptFiles)
                    {
                        if (
                            string.Equals(
                                existingFile,
                                file,
                                StringComparison.OrdinalIgnoreCase))
                        {
                            alreadySelected = true;
                            break;
                        }
                    }

                    if (!alreadySelected)
                    {
                        selectedReceiptFiles.Add(file);
                    }
                }

                if (selectedReceiptFiles.Count == 1)
                {
                    txtReceiptImage.Text =
                        selectedReceiptFiles[0];
                }
                else
                {
                    txtReceiptImage.Text =
                        "تم اختيار "
                        + selectedReceiptFiles.Count
                        + " إشعارات";
                }

                selectedFilesLabel.Text =
                    "تم اختيار "
                    + selectedReceiptFiles.Count
                    + " إشعار/إشعارات.";

                selectedFilesLabel.ForeColor =
                    Color.FromArgb(
                        20,
                        125,
                        90);

                progressLabel.Text = "";

                extractedReceipts.Clear();

                extractedGrid.Rows.Clear();

                extractedGrid.Visible =
                    false;

                gridTitle.Visible =
                    false;

                btnEditSelected.Visible =
                    false;

                btnDeleteSelected.Visible =
                    false;

                btnSaveAll.Visible =
                    false;
            }
        }

        private async void BtnExtractData_Click(
            object sender,
            EventArgs e)
        {
            if (selectedReceiptFiles.Count == 0)
            {
                MessageBox.Show(
                    "يرجى رفع صورة إشعار التحويل أولاً.",
                    "تنبيه",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            string apiKey =
                GetGeminiApiKey();

            if (string.IsNullOrWhiteSpace(
                apiKey))
            {
                MessageBox.Show(
                    "مفتاح Gemini غير موجود.\n\n"
                    + "ضع المفتاح في متغير Windows باسم:\n"
                    + "GEMINI_API_KEY\n\n"
                    + "أو أنشئ ملف باسم:\n"
                    + "gemini_api_key.txt\n\n"
                    + "بجانب ملف تشغيل البرنامج وضع المفتاح بداخله.",
                    "Gemini API",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return;
            }

            try
            {
                btnExtractData.Enabled =
                    false;

                btnSelectReceipt.Enabled =
                    false;

                btnSaveAll.Enabled =
                    false;

                extractedReceipts.Clear();

                extractedGrid.Rows.Clear();

                List<ReceiptData> allResults =
                    new List<ReceiptData>();

                int batchSize = 3;

                for (
                    int start = 0;
                    start < selectedReceiptFiles.Count;
                    start += batchSize)
                {
                    List<Task<ReceiptData>> batch =
                        new List<Task<ReceiptData>>();

                    int end =
                        Math.Min(
                            start + batchSize,
                            selectedReceiptFiles.Count);

                    for (
                        int i = start;
                        i < end;
                        i++)
                    {
                        batch.Add(
                            AnalyzeReceiptWithGeminiAsync(
                                selectedReceiptFiles[i],
                                apiKey));
                    }

                    ReceiptData[] batchResults =
                        await Task.WhenAll(batch);

                    allResults.AddRange(
                        batchResults);

                    progressLabel.Text =
                        "تم تحليل "
                        + allResults.Count
                        + " من "
                        + selectedReceiptFiles.Count
                        + "...";
                }

                int failedCount = 0;

                HashSet<string> seenReferences =
                    new HashSet<string>(
                        StringComparer.OrdinalIgnoreCase);

                foreach (ReceiptData data in allResults)
                {
                    string reference =
                        NormalizeReference(
                            data.Reference);

                    bool complete =
                        !string.IsNullOrWhiteSpace(
                            data.Beneficiary)
                        &&
                        !string.IsNullOrWhiteSpace(
                            data.Amount)
                        &&
                        !string.IsNullOrWhiteSpace(
                            reference);

                    if (!complete)
                    {
                        failedCount++;
                        continue;
                    }

                    data.Reference =
                        reference;

                    if (!seenReferences.Add(
                        reference))
                    {
                        continue;
                    }

                    extractedReceipts.Add(data);
                }

                ShowExtractedReceipts();

                if (extractedReceipts.Count == 1)
                {
                    FillFieldsFromReceipt(
                        extractedReceipts[0]);

                    if (!string.IsNullOrWhiteSpace(
                        extractedReceipts[0].Error))
                    {
                        MessageBox.Show(
                            "تم الاتصال بـ Gemini لكن النتيجة ناقصة:\n\n"
                            + extractedReceipts[0].Error
                            + "\n\n"
                            + "راجع السطر في الجدول وعدله مباشرة.",
                            "نتيجة التحليل",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);
                    }
                    else
                    {
                        MessageBox.Show(
                            "تم استخراج بيانات الإشعار ووضعها في الحقول والجدول.\n\n"
                            + "راجع الاسم والمبلغ والتاريخ ورقم العملية والتعليق قبل الحفظ.",
                            "تم الاستخراج",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                    }
                }
                else
                {
                    int duplicateCount =
                        selectedReceiptFiles.Count
                        - extractedReceipts.Count
                        - failedCount;

                    if (duplicateCount < 0)
                    {
                        duplicateCount = 0;
                    }

                    MessageBox.Show(
                        "تمت معالجة "
                        + selectedReceiptFiles.Count
                        + " إشعار.\n\n"
                        + "تم استخراج "
                        + extractedReceipts.Count
                        + " إشعار فريد.\n"
                        + "تم تجاهل "
                        + duplicateCount
                        + " إشعار مكرر حسب رقم العملية."
                        + (
                            failedCount > 0
                                ? "\n"
                                  + failedCount
                                  + " إشعار لم تكتمل قراءته وتم استبعاده من الجدول حتى لا تظهر حقول فارغة."
                                : ""
                          )
                        + "\n\nراجع الجدول وعدل أي خلية مباشرة قبل الحفظ.",
                        "اكتمل الاستخراج",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }

                progressLabel.Text =
                    "اكتمل التحليل.";
            }
            catch (Exception ex)
            {
                progressLabel.Text =
                    "فشل التحليل.";

                MessageBox.Show(
                    "حدث خطأ أثناء الاتصال بـ Gemini:\n\n"
                    + ex.Message,
                    "Gemini API",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                btnExtractData.Enabled =
                    true;

                btnSelectReceipt.Enabled =
                    true;

                btnSaveAll.Enabled =
                    true;
            }
        }

        private async Task<ReceiptData>
            AnalyzeReceiptWithGeminiAsync(
                string filePath,
                string apiKey)
        {
            ReceiptData data =
                new ReceiptData();

            data.FilePath =
                filePath;

            if (cmbCategory.Items.Count > 0)
            {
                data.Category =
                    cmbCategory.Items[0]
                    ?.ToString()
                    ?? "";
            }

            byte[] imageBytes =
                await File.ReadAllBytesAsync(
                    filePath);

            string base64 =
                Convert.ToBase64String(
                    imageBytes);

            string mimeType =
                GetMimeType(filePath);

            string prompt = @"
You are an expert document-data extraction system.

Analyze the attached bank transfer receipt image directly.

The receipt is from the same bank template and can be either Arabic or English.

IMPORTANT RULES:

1. Detect whether the receipt is Arabic or English.
2. If Arabic, use the Arabic labels and extract the Arabic values.
3. If English, use the English labels and extract the English values.
4. Do NOT translate the beneficiary name.
5. Do NOT invent missing values.
6. Read the actual image.
7. Do not guess a field from an unrelated line.
8. The transaction/reference number is the value next to:
   Arabic: رقم العملية
   English: Trx. ID
9. The date is the value next to:
   Arabic: التاريخ و الزمن
   English: Date & Time
10. The beneficiary is the value next to:
   Arabic: اسم المرسل إليه
   Arabic without hamza: اسم المرسل اليه
   English: To Account Title
11. The comment is the value next to:
   Arabic: التعليق
   English: Comment
12. The amount is the value next to:
   Arabic: المبلغ
   English: Amount
13. The beneficiary can occupy two or more lines. Join those lines with a single space.
14. Preserve the beneficiary spelling exactly as visible.
15. Preserve the comment exactly as visible.
16. If the comment is N/A, return N/A.
17. Do not use an account number as the transaction ID.
18. Do not use a phone number as the transaction ID.
19. Do not use an account number as the amount.
20. Return the date as yyyy-MM-dd.
21. Return the amount as digits with exactly two decimal places and without commas.
22. Return JSON only.

Known English example:

Trx. ID:
20315483266

Date & Time:
29-Sep-2026 09:37:10

To Account Title:
Asya Mohamed Zaki
Mahgoub

Comment:
N/A

Amount:
20,000.00

Expected result for this example:

reference = 20315483266
beneficiary = Asya Mohamed Zaki Mahgoub
amount = 20000.00
description = N/A
date = 2026-09-29

Known Arabic example:

رقم العملية:
20533061209

التاريخ و الزمن:
28-Sep-2026 13:04:28

اسم المرسل إليه:
آسيا محمد زكي محجوب

التعليق:
N/A

المبلغ:
40,000.00

Expected result for this example:

reference = 20533061209
beneficiary = آسيا محمد زكي محجوب
amount = 40000.00
description = N/A
date = 2026-09-28

Return exactly this JSON structure:

{
  ""language"": ""ar"",
  ""reference"": """",
  ""date"": """",
  ""beneficiary"": """",
  ""amount"": """",
  ""description"": """",
  ""bank"": ""بنك الخرطوم"",
  ""confidence_note"": """"
}

language must be either ar or en.
";

            List<string> errors =
                new List<string>();

            foreach (
                string model in GeminiModels)
            {
                try
                {
                    progressLabel.Text =
                        "جاري استخدام "
                        + model
                        + "...";

                    Application.DoEvents();

                    string requestJson =
                        BuildGeminiRequest(
                            prompt,
                            base64,
                            mimeType);

                    using (
                        HttpRequestMessage request =
                            new HttpRequestMessage(
                                HttpMethod.Post,
                                "https://generativelanguage.googleapis.com/v1beta/models/"
                                + model
                                + ":generateContent"))
                    {
                        request.Headers.Add(
                            "x-goog-api-key",
                            apiKey);

                        request.Content =
                            new StringContent(
                                requestJson,
                                Encoding.UTF8,
                                "application/json");

                        using (
                            HttpResponseMessage response =
                                await GeminiHttpClient.SendAsync(
                                    request))
                        {
                            string responseText =
                                await response.Content
                                    .ReadAsStringAsync();

                            if (response.IsSuccessStatusCode)
                            {
                                ReceiptData result =
                                    ParseGeminiResponse(
                                        responseText,
                                        data);

                                if (string.IsNullOrWhiteSpace(
                                    result.Error))
                                {
                                    progressLabel.Text =
                                        "تم التحليل باستخدام "
                                        + model;

                                    return result;
                                }

                                data = result;

                                if (!string.IsNullOrWhiteSpace(
                                    result.Error))
                                {
                                    errors.Add(
                                        model
                                        + ": "
                                        + result.Error);
                                }
                            }
                            else
                            {
                                errors.Add(
                                    model
                                    + " HTTP "
                                    + (int)response.StatusCode
                                    + ": "
                                    + ExtractApiErrorMessage(
                                        responseText));

                                if (
                                    response.StatusCode
                                    == System.Net.HttpStatusCode
                                        .Unauthorized
                                    ||
                                    response.StatusCode
                                    == System.Net.HttpStatusCode
                                        .Forbidden)
                                {
                                    break;
                                }
                            }
                        }
                    }
                }
                catch (TaskCanceledException)
                {
                    errors.Add(
                        model
                        + ": انتهت مهلة الاتصال.");
                }
                catch (Exception ex)
                {
                    errors.Add(
                        model
                        + ": "
                        + ex.Message);
                }
            }

            data.Error =
                "لم ينجح أي موديل Gemini في استخراج البيانات.\n\n"
                + string.Join(
                    "\n",
                    errors);

            return data;
        }

        private string BuildGeminiRequest(
            string prompt,
            string base64,
            string mimeType)
        {
            var requestObject =
                new
                {
                    contents = new[]
                    {
                        new
                        {
                            parts = new object[]
                            {
                                new
                                {
                                    text = prompt
                                },

                                new
                                {
                                    inline_data = new
                                    {
                                        mime_type =
                                            mimeType,
                                        data =
                                            base64
                                    }
                                }
                            }
                        }
                    },

                    generationConfig =
                        new
                        {
                            responseMimeType =
                                "application/json",

                            responseSchema =
                                new
                                {
                                    type = "OBJECT",

                                    properties =
                                        new
                                        {
                                            language =
                                                new
                                                {
                                                    type =
                                                        "STRING"
                                                },

                                            reference =
                                                new
                                                {
                                                    type =
                                                        "STRING"
                                                },

                                            date =
                                                new
                                                {
                                                    type =
                                                        "STRING"
                                                },

                                            beneficiary =
                                                new
                                                {
                                                    type =
                                                        "STRING"
                                                },

                                            amount =
                                                new
                                                {
                                                    type =
                                                        "STRING"
                                                },

                                            description =
                                                new
                                                {
                                                    type =
                                                        "STRING"
                                                },

                                            bank =
                                                new
                                                {
                                                    type =
                                                        "STRING"
                                                },

                                            confidence_note =
                                                new
                                                {
                                                    type =
                                                        "STRING"
                                                }
                                        },

                                    required =
                                        new[]
                                        {
                                            "language",
                                            "reference",
                                            "date",
                                            "beneficiary",
                                            "amount",
                                            "description",
                                            "bank",
                                            "confidence_note"
                                        }
                                },

                            maxOutputTokens =
                                700
                        }
                };

            return JsonSerializer.Serialize(
                requestObject);
        }

        private ReceiptData ParseGeminiResponse(
            string responseText,
            ReceiptData data)
        {
            try
            {
                using (
                    JsonDocument document =
                        JsonDocument.Parse(
                            responseText))
                {
                    JsonElement root =
                        document.RootElement;

                    if (
                        !root.TryGetProperty(
                            "candidates",
                            out JsonElement candidates)
                        ||
                        candidates.GetArrayLength()
                        == 0)
                    {
                        data.Error =
                            "Gemini لم يرجع أي نتيجة.";

                        return data;
                    }

                    JsonElement candidate =
                        candidates[0];

                    if (
                        !candidate.TryGetProperty(
                            "content",
                            out JsonElement content))
                    {
                        data.Error =
                            "Gemini رجع نتيجة بدون content.";

                        return data;
                    }

                    if (
                        !content.TryGetProperty(
                            "parts",
                            out JsonElement parts))
                    {
                        data.Error =
                            "Gemini رجع content بدون parts.";

                        return data;
                    }

                    StringBuilder output =
                        new StringBuilder();

                    foreach (
                        JsonElement part
                        in parts.EnumerateArray())
                    {
                        if (
                            part.TryGetProperty(
                                "text",
                                out JsonElement textElement))
                        {
                            output.Append(
                                textElement
                                    .GetString());
                        }
                    }

                    string extractedJson =
                        output
                            .ToString()
                            .Trim();

                    if (
                        string.IsNullOrWhiteSpace(
                            extractedJson))
                    {
                        data.Error =
                            "Gemini رجع استجابة فارغة.";

                        return data;
                    }

                    extractedJson =
                        CleanJsonResponse(
                            extractedJson);

                    using (
                        JsonDocument extracted =
                            JsonDocument.Parse(
                                extractedJson))
                    {
                        JsonElement obj =
                            extracted.RootElement;

                        data.Language =
                            GetJsonString(
                                obj,
                                "language");

                        data.Reference =
                            NormalizeReference(
                                GetJsonString(
                                    obj,
                                    "reference"));

                        data.Date =
                            NormalizeReturnedDate(
                                GetJsonString(
                                    obj,
                                    "date"));

                        data.Beneficiary =
                            CleanBeneficiary(
                                GetJsonString(
                                    obj,
                                    "beneficiary"));

                        data.Amount =
                            NormalizeAmount(
                                GetJsonString(
                                    obj,
                                    "amount"));

                        data.Description =
                            CleanDescription(
                                GetJsonString(
                                    obj,
                                    "description"));

                        data.Bank =
                            GetJsonString(
                                obj,
                                "bank");

                        if (
                            string.IsNullOrWhiteSpace(
                                data.Bank))
                        {
                            data.Bank =
                                "بنك الخرطوم";
                        }

                        bool missingRequired =
                            string.IsNullOrWhiteSpace(
                                data.Beneficiary)
                            ||
                            string.IsNullOrWhiteSpace(
                                data.Reference)
                            ||
                            string.IsNullOrWhiteSpace(
                                data.Amount);

                        if (missingRequired)
                        {
                            StringBuilder missing =
                                new StringBuilder();

                            if (
                                string.IsNullOrWhiteSpace(
                                    data.Beneficiary))
                            {
                                missing.Append(
                                    "اسم المستفيد، ");
                            }

                            if (
                                string.IsNullOrWhiteSpace(
                                    data.Reference))
                            {
                                missing.Append(
                                    "رقم العملية، ");
                            }

                            if (
                                string.IsNullOrWhiteSpace(
                                    data.Amount))
                            {
                                missing.Append(
                                    "المبلغ، ");
                            }

                            data.Error =
                                "النتيجة ناقصة. الحقول غير المقروءة: "
                                + missing.ToString()
                                    .TrimEnd(
                                        ' ',
                                        '،');
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                data.Error =
                    "تعذر قراءة نتيجة Gemini:\n"
                    + ex.Message;
            }

            return data;
        }

        private string CleanJsonResponse(
            string text)
        {
            text =
                text.Trim();

            if (
                text.StartsWith(
                    "```"))
            {
                int firstNewLine =
                    text.IndexOf(
                        '\n');

                if (firstNewLine >= 0)
                {
                    text =
                        text.Substring(
                            firstNewLine + 1);
                }

                if (
                    text.EndsWith(
                        "```"))
                {
                    text =
                        text.Substring(
                            0,
                            text.Length - 3);
                }
            }

            return text.Trim();
        }

        private string ExtractApiErrorMessage(
            string responseText)
        {
            try
            {
                using (
                    JsonDocument document =
                        JsonDocument.Parse(
                            responseText))
                {
                    JsonElement root =
                        document.RootElement;

                    if (
                        root.TryGetProperty(
                            "error",
                            out JsonElement error))
                    {
                        if (
                            error.TryGetProperty(
                                "message",
                                out JsonElement message))
                        {
                            return
                                message
                                    .GetString()
                                ?? responseText;
                        }
                    }
                }
            }
            catch
            {
            }

            return responseText;
        }

        private string GetJsonString(
            JsonElement obj,
            string property)
        {
            if (
                obj.TryGetProperty(
                    property,
                    out JsonElement element)
                &&
                element.ValueKind
                == JsonValueKind.String)
            {
                return
                    element
                        .GetString()
                    ?.Trim()
                    ?? "";
            }

            return "";
        }

        private string NormalizeReturnedDate(
            string value)
        {
            if (
                string.IsNullOrWhiteSpace(
                    value))
            {
                return "";
            }

            value =
                NormalizeDigits(
                    value.Trim());

            DateTime parsed;

            string[] formats =
            {
                "yyyy-MM-dd",
                "dd-MMM-yyyy",
                "d-MMM-yyyy",
                "dd-MMM-yyyy HH:mm:ss",
                "d-MMM-yyyy HH:mm:ss",
                "dd/MM/yyyy",
                "d/MM/yyyy",
                "dd-MM-yyyy",
                "d-MM-yyyy"
            };

            if (
                DateTime.TryParseExact(
                    value,
                    formats,
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out parsed))
            {
                return
                    parsed.ToString(
                        "yyyy-MM-dd");
            }

            if (
                DateTime.TryParse(
                    value,
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out parsed))
            {
                return
                    parsed.ToString(
                        "yyyy-MM-dd");
            }

            return value;
        }

        private string NormalizeAmount(
            string value)
        {
            if (
                string.IsNullOrWhiteSpace(
                    value))
            {
                return "";
            }

            string cleaned =
                NormalizeDigits(
                    value)
                .Replace(
                    ",",
                    "")
                .Replace(
                    " ",
                    "")
                .Trim();

            decimal amount;

            if (
                decimal.TryParse(
                    cleaned,
                    NumberStyles.Any,
                    CultureInfo.InvariantCulture,
                    out amount))
            {
                return
                    amount.ToString(
                        "0.00",
                        CultureInfo.InvariantCulture);
            }

            return cleaned;
        }

        private string NormalizeReference(
            string value)
        {
            string normalized =
                NormalizeDigits(
                    value ?? "");

            StringBuilder digits =
                new StringBuilder();

            foreach (char c in normalized)
            {
                if (char.IsDigit(c))
                {
                    digits.Append(c);
                }
            }

            return digits.ToString();
        }

        private string NormalizeDigits(
            string text)
        {
            if (
                string.IsNullOrEmpty(
                    text))
            {
                return "";
            }

            return text
                .Replace('٠', '0')
                .Replace('١', '1')
                .Replace('٢', '2')
                .Replace('٣', '3')
                .Replace('٤', '4')
                .Replace('٥', '5')
                .Replace('٦', '6')
                .Replace('٧', '7')
                .Replace('٨', '8')
                .Replace('٩', '9')
                .Replace('۰', '0')
                .Replace('۱', '1')
                .Replace('۲', '2')
                .Replace('۳', '3')
                .Replace('۴', '4')
                .Replace('۵', '5')
                .Replace('۶', '6')
                .Replace('۷', '7')
                .Replace('۸', '8')
                .Replace('۹', '9');
        }

        private string CleanBeneficiary(
            string value)
        {
            if (
                string.IsNullOrWhiteSpace(
                    value))
            {
                return "";
            }

            value =
                value.Replace(
                    "\r",
                    " ")
                .Replace(
                    "\n",
                    " ");

            value =
                value.Replace(
                    "  ",
                    " ");

            return value.Trim();
        }

        private string CleanDescription(
            string value)
        {
            if (
                string.IsNullOrWhiteSpace(
                    value))
            {
                return "";
            }

            return
                value
                    .Replace(
                        "\r",
                        " ")
                    .Replace(
                        "\n",
                        " ")
                    .Trim();
        }

        private string GetMimeType(
            string filePath)
        {
            string extension =
                Path.GetExtension(
                    filePath)
                .ToLowerInvariant();

            switch (extension)
            {
                case ".png":
                    return "image/png";

                case ".webp":
                    return "image/webp";

                case ".bmp":
                    return "image/bmp";

                case ".gif":
                    return "image/gif";

                case ".jpg":
                case ".jpeg":
                default:
                    return "image/jpeg";
            }
        }

        private string GetGeminiApiKey()
        {
            string key =
                Environment.GetEnvironmentVariable(
                    "GEMINI_API_KEY");

            if (
                !string.IsNullOrWhiteSpace(
                    key))
            {
                return key.Trim();
            }

            string[] possibleFiles =
            {
                Path.Combine(
                    Application.StartupPath,
                    "gemini_api_key.txt"),

                Path.Combine(
                    AppDomain.CurrentDomain.BaseDirectory,
                    "gemini_api_key.txt")
            };

            foreach (
                string file
                in possibleFiles)
            {
                if (
                    File.Exists(
                        file))
                {
                    string fileKey =
                        File.ReadAllText(
                            file)
                        .Trim();

                    if (
                        !string.IsNullOrWhiteSpace(
                            fileKey))
                    {
                        return fileKey;
                    }
                }
            }

            return "";
        }

        private void FillFieldsFromReceipt(
            ReceiptData data)
        {
            txtBeneficiary.Text =
                data.Beneficiary;

            txtAmount.Text =
                data.Amount;

            txtReference.Text =
                data.Reference;

            txtDescription.Text =
                data.Description;

            txtReceiptImage.Text =
                data.FilePath;

            DateTime date;

            if (
                DateTime.TryParseExact(
                    data.Date,
                    "yyyy-MM-dd",
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out date))
            {
                dtpDate.Value =
                    date;
            }

            SelectBank(
                data.Bank);

            if (
                !string.IsNullOrWhiteSpace(
                    data.Category))
            {
                cmbCategory.SelectedItem =
                    data.Category;
            }
        }

        private void SelectBank(
            string bank)
        {
            if (
                string.IsNullOrWhiteSpace(
                    bank))
            {
                return;
            }

            for (
                int i = 0;
                i < cmbBank.Items.Count;
                i++)
            {
                string item =
                    cmbBank.Items[i]
                    ?.ToString()
                    ?? "";

                if (
                    item.Contains(
                        "الخرطوم",
                        StringComparison
                            .OrdinalIgnoreCase)
                    &&
                    bank.Contains(
                        "الخرطوم",
                        StringComparison
                            .OrdinalIgnoreCase))
                {
                    cmbBank.SelectedIndex =
                        i;

                    return;
                }

                if (
                    string.Equals(
                        item,
                        bank,
                        StringComparison
                            .OrdinalIgnoreCase))
                {
                    cmbBank.SelectedIndex =
                        i;

                    return;
                }
            }
        }

        private void ShowExtractedReceipts()
        {
            extractedGrid.Rows.Clear();

            foreach (
                ReceiptData data
                in extractedReceipts)
            {
                int row =
                    extractedGrid.Rows.Add();

                extractedGrid.Rows[row]
                    .Cells["File"]
                    .Value =
                    Path.GetFileName(
                        data.FilePath);

                extractedGrid.Rows[row]
                    .Cells["Language"]
                    .Value =
                    data.Language == "ar"
                        ? "عربي"
                        : data.Language == "en"
                            ? "English"
                            : data.Language;

                extractedGrid.Rows[row]
                    .Cells["Date"]
                    .Value =
                    data.Date;

                extractedGrid.Rows[row]
                    .Cells["Beneficiary"]
                    .Value =
                    data.Beneficiary;

                extractedGrid.Rows[row]
                    .Cells["Amount"]
                    .Value =
                    data.Amount;

                extractedGrid.Rows[row]
                    .Cells["Reference"]
                    .Value =
                    data.Reference;

                extractedGrid.Rows[row]
                    .Cells["Description"]
                    .Value =
                    data.Description;

                extractedGrid.Rows[row]
                    .Cells["Bank"]
                    .Value =
                    data.Bank;

                extractedGrid.Rows[row]
                    .Cells["Category"]
                    .Value =
                    data.Category;
            }

            extractedGrid.Visible =
                true;

            gridTitle.Visible =
                true;

            btnEditSelected.Visible =
                true;

            btnDeleteSelected.Visible =
                true;

            btnSaveAll.Visible =
                true;
        }

        private void BtnDeleteSelected_Click(
            object sender,
            EventArgs e)
        {
            if (
                extractedGrid.SelectedRows.Count
                == 0)
            {
                MessageBox.Show(
                    "حدد إشعاراً من الجدول أولاً.",
                    "تنبيه",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            DataGridViewRow row =
                extractedGrid.SelectedRows[0];

            string fileName =
                row.Cells["File"]
                    .Value
                    ?.ToString()
                    ?? "";

            string reference =
                row.Cells["Reference"]
                    .Value
                    ?.ToString()
                    ?? "";

            DialogResult result =
                MessageBox.Show(
                    "هل تريد حذف هذا السطر من جدول الإشعارات؟\n\n"
                    + "الإشعار: "
                    + fileName
                    + (
                        string.IsNullOrWhiteSpace(reference)
                            ? ""
                            : "\nرقم العملية: "
                              + reference
                    )
                    + "\n\nلن يتم حفظ هذا السطر في قاعدة البيانات.",
                    "حذف إشعار من الجدول",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

            if (result != DialogResult.Yes)
            {
                return;
            }

            extractedGrid.Rows.Remove(row);

            if (extractedGrid.Rows.Count == 0)
            {
                extractedGrid.Visible =
                    false;

                gridTitle.Visible =
                    false;

                btnEditSelected.Visible =
                    false;

                btnDeleteSelected.Visible =
                    false;

                btnSaveAll.Visible =
                    false;

                return;
            }

            int remainingIndex =
                Math.Min(
                    row.Index,
                    extractedGrid.Rows.Count - 1);

            if (remainingIndex >= 0)
            {
                extractedGrid.ClearSelection();

                extractedGrid.Rows[
                    remainingIndex]
                    .Selected = true;
            }
        }

        private void BtnEditSelected_Click(
            object sender,
            EventArgs e)
        {
            if (
                extractedGrid.SelectedRows.Count
                == 0)
            {
                MessageBox.Show(
                    "حدد إشعاراً من الجدول أولاً.",
                    "تنبيه",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            DataGridViewRow row =
                extractedGrid.SelectedRows[0];

            txtBeneficiary.Text =
                row.Cells["Beneficiary"]
                    .Value
                    ?.ToString()
                    ?? "";

            txtAmount.Text =
                row.Cells["Amount"]
                    .Value
                    ?.ToString()
                    ?? "";

            txtReference.Text =
                row.Cells["Reference"]
                    .Value
                    ?.ToString()
                    ?? "";

            txtDescription.Text =
                row.Cells["Description"]
                    .Value
                    ?.ToString()
                    ?? "";

            string date =
                row.Cells["Date"]
                    .Value
                    ?.ToString()
                    ?? "";

            DateTime parsedDate;

            if (
                DateTime.TryParseExact(
                    date,
                    "yyyy-MM-dd",
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out parsedDate))
            {
                dtpDate.Value =
                    parsedDate;
            }

            SelectBank(
                row.Cells["Bank"]
                    .Value
                    ?.ToString()
                    ?? "");

            string category =
                row.Cells["Category"]
                    .Value
                    ?.ToString()
                    ?? "";

            if (
                !string.IsNullOrWhiteSpace(
                    category))
            {
                cmbCategory.SelectedItem =
                    category;
            }

            txtBeneficiary.Focus();
        }

        private void BtnSaveAll_Click(
            object sender,
            EventArgs e)
        {
            SaveExtractedGrid();
        }

        private void SaveExtractedGrid()
        {
            if (
                extractedGrid.Rows.Count
                == 0)
            {
                MessageBox.Show(
                    "لا توجد إشعارات في الجدول للحفظ.",
                    "تنبيه",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            int savedCount = 0;
            int skippedCount = 0;

            try
            {
                extractedGrid.EndEdit();

                using (
                    SqliteConnection connection =
                        Database.GetConnection())
                {
                    using (
                        SqliteTransaction transaction =
                            connection.BeginTransaction())
                    {
                        foreach (
                            DataGridViewRow row
                            in extractedGrid.Rows)
                        {
                            string date =
                                row.Cells["Date"]
                                    .Value
                                    ?.ToString()
                                    ?.Trim()
                                    ?? "";

                            string beneficiary =
                                row.Cells["Beneficiary"]
                                    .Value
                                    ?.ToString()
                                    ?.Trim()
                                    ?? "";

                            string amountText =
                                row.Cells["Amount"]
                                    .Value
                                    ?.ToString()
                                    ?.Trim()
                                    ?? "";

                            string reference =
                                NormalizeReference(
                                    row.Cells["Reference"]
                                        .Value
                                        ?.ToString()
                                        ?? "");

                            string description =
                                row.Cells["Description"]
                                    .Value
                                    ?.ToString()
                                    ?.Trim()
                                    ?? "";

                            string bank =
                                row.Cells["Bank"]
                                    .Value
                                    ?.ToString()
                                    ?.Trim()
                                    ?? "";

                            string category =
                                row.Cells["Category"]
                                    .Value
                                    ?.ToString()
                                    ?.Trim()
                                    ?? "";

                            decimal amount;

                            string normalizedAmount =
                                NormalizeDigits(
                                    amountText)
                                .Replace(
                                    ",",
                                    "");

                            if (
                                string.IsNullOrWhiteSpace(
                                    beneficiary)
                                ||
                                !decimal.TryParse(
                                    normalizedAmount,
                                    NumberStyles.Any,
                                    CultureInfo.InvariantCulture,
                                    out amount))
                            {
                                skippedCount++;

                                continue;
                            }

                            if (
                                string.IsNullOrWhiteSpace(
                                    date))
                            {
                                date =
                                    DateTime.Now
                                        .ToString(
                                            "yyyy-MM-dd");
                            }

                            if (
                                !string.IsNullOrWhiteSpace(
                                    reference))
                            {
                                string duplicateSql = @"
                                    SELECT COUNT(*)
                                    FROM Transfers
                                    WHERE TRIM(ReferenceNumber) = TRIM($reference);";

                                using (
                                    SqliteCommand duplicateCommand =
                                        new SqliteCommand(
                                            duplicateSql,
                                            connection,
                                            transaction))
                                {
                                    duplicateCommand.Parameters
                                        .AddWithValue(
                                            "$reference",
                                            reference);

                                    int existingCount =
                                        Convert.ToInt32(
                                            duplicateCommand.ExecuteScalar());

                                    if (existingCount > 0)
                                    {
                                        skippedCount++;
                                        continue;
                                    }
                                }
                            }

                            string fileName =
                                row.Cells["File"]
                                    .Value
                                    ?.ToString()
                                    ?? "";

                            string imagePath =
                                FindSelectedFile(
                                    fileName);

                            string sql = @"
                                INSERT INTO Transfers
                                (
                                    TransferDate,
                                    Beneficiary,
                                    Amount,
                                    ReferenceNumber,
                                    Description,
                                    Category,
                                    Bank,
                                    ReceiptImage
                                )
                                VALUES
                                (
                                    $date,
                                    $beneficiary,
                                    $amount,
                                    $reference,
                                    $description,
                                    $category,
                                    $bank,
                                    $image
                                );";

                            using (
                                SqliteCommand command =
                                    new SqliteCommand(
                                        sql,
                                        connection,
                                        transaction))
                            {
                                command.Parameters
                                    .AddWithValue(
                                        "$date",
                                        date);

                                command.Parameters
                                    .AddWithValue(
                                        "$beneficiary",
                                        beneficiary);

                                command.Parameters
                                    .AddWithValue(
                                        "$amount",
                                        amount);

                                command.Parameters
                                    .AddWithValue(
                                        "$reference",
                                        reference);

                                command.Parameters
                                    .AddWithValue(
                                        "$description",
                                        description);

                                command.Parameters
                                    .AddWithValue(
                                        "$category",
                                        category);

                                command.Parameters
                                    .AddWithValue(
                                        "$bank",
                                        bank);

                                command.Parameters
                                    .AddWithValue(
                                        "$image",
                                        imagePath);

                                command.ExecuteNonQuery();
                            }

                            savedCount++;
                        }

                        transaction.Commit();
                    }
                }

                MessageBox.Show(
                    "تم حفظ "
                    + savedCount
                    + " تحويلات بنجاح."
                    +
                    (
                        skippedCount > 0
                            ? "\nتم تجاوز "
                              + skippedCount
                              + " صفوف ناقصة أو مكررة في قاعدة البيانات."
                            : ""
                    ),
                    "تم الحفظ",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                ClearFields();

                extractedReceipts.Clear();

                extractedGrid.Rows.Clear();

                extractedGrid.Visible =
                    false;

                gridTitle.Visible =
                    false;

                btnEditSelected.Visible =
                    false;

                btnDeleteSelected.Visible =
                    false;

                btnSaveAll.Visible =
                    false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "حدث خطأ أثناء حفظ الإشعارات:\n\n"
                    + ex.Message,
                    "خطأ",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private string FindSelectedFile(
            string fileName)
        {
            foreach (
                string file
                in selectedReceiptFiles)
            {
                if (
                    Path.GetFileName(
                        file)
                    .Equals(
                        fileName,
                        StringComparison
                            .OrdinalIgnoreCase))
                {
                    return file;
                }
            }

            return "";
        }

        private void BtnSave_Click(
            object sender,
            EventArgs e)
        {
            // =====================================================
            // مهم جداً:
            // إذا كانت هناك إشعارات في الجدول، احفظ الجدول كله.
            // لا تفحص txtBeneficiary ولا txtAmount في هذه الحالة.
            // =====================================================

            if (
                extractedGrid != null
                &&
                extractedGrid.Rows.Count > 0
                &&
                extractedGrid.Visible)
            {
                SaveExtractedGrid();

                return;
            }

            // =====================================================
            // الحفظ الفردي العادي
            // =====================================================

            if (
                string.IsNullOrWhiteSpace(
                    txtBeneficiary.Text))
            {
                MessageBox.Show(
                    "يرجى إدخال اسم المستفيد.",
                    "تنبيه",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                txtBeneficiary.Focus();

                return;
            }

            decimal amount;

            if (
                !decimal.TryParse(
                    NormalizeDigits(
                        txtAmount.Text)
                    .Replace(
                        ",",
                        ""),
                    NumberStyles.Any,
                    CultureInfo.InvariantCulture,
                    out amount))
            {
                MessageBox.Show(
                    "يرجى إدخال مبلغ صحيح.",
                    "تنبيه",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                txtAmount.Focus();

                return;
            }

            if (
                cmbCategory.SelectedIndex < 0
                ||
                string.IsNullOrWhiteSpace(
                    cmbCategory.Text))
            {
                MessageBox.Show(
                    "يرجى اختيار التصنيف.",
                    "تنبيه",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                cmbCategory.Focus();

                return;
            }

            try
            {
                string singleReference =
                    NormalizeReference(
                        txtReference.Text);

                if (
                    !string.IsNullOrWhiteSpace(
                        singleReference))
                {
                    using (
                        SqliteConnection checkConnection =
                            Database.GetConnection())
                    using (
                        SqliteCommand checkCommand =
                            new SqliteCommand(
                                @"
                                SELECT COUNT(*)
                                FROM Transfers
                                WHERE TRIM(ReferenceNumber) = TRIM($reference);",
                                checkConnection))
                    {
                        checkCommand.Parameters.AddWithValue(
                            "$reference",
                            singleReference);

                        int existingCount =
                            Convert.ToInt32(
                                checkCommand.ExecuteScalar());

                        if (existingCount > 0)
                        {
                            MessageBox.Show(
                                "هذا الإشعار موجود مسبقاً.\n\n"
                                + "رقم العملية: "
                                + singleReference
                                + "\n\nتم تجاهله ولن تتم إضافته مرة أخرى.",
                                "إشعار مكرر",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);

                            return;
                        }
                    }
                }

                using (
                    SqliteConnection connection =
                        Database.GetConnection())
                using (
                    SqliteCommand command =
                        new SqliteCommand(
                            @"
                            INSERT INTO Transfers
                            (
                                TransferDate,
                                Beneficiary,
                                Amount,
                                ReferenceNumber,
                                Description,
                                Category,
                                Bank,
                                ReceiptImage
                            )
                            VALUES
                            (
                                $date,
                                $beneficiary,
                                $amount,
                                $reference,
                                $description,
                                $category,
                                $bank,
                                $image
                            );",
                            connection))
                {
                    command.Parameters
                        .AddWithValue(
                            "$date",
                            dtpDate.Value.ToString(
                                "yyyy-MM-dd"));

                    command.Parameters
                        .AddWithValue(
                            "$beneficiary",
                            txtBeneficiary.Text.Trim());

                    command.Parameters
                        .AddWithValue(
                            "$amount",
                            amount);

                    command.Parameters
                        .AddWithValue(
                            "$reference",
                            singleReference);

                    command.Parameters
                        .AddWithValue(
                            "$description",
                            txtDescription.Text.Trim());

                    command.Parameters
                        .AddWithValue(
                            "$category",
                            cmbCategory.Text.Trim());

                    command.Parameters
                        .AddWithValue(
                            "$bank",
                            cmbBank.Text.Trim());

                    command.Parameters
                        .AddWithValue(
                            "$image",
                            txtReceiptImage.Text.Trim());

                    command.ExecuteNonQuery();
                }

                MessageBox.Show(
                    "تم حفظ التحويل بنجاح.",
                    "نجاح",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                ClearFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "حدث خطأ أثناء حفظ التحويل:\n\n"
                    + ex.Message,
                    "خطأ",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void ClearFields()
        {
            txtBeneficiary.Clear();

            txtAmount.Clear();

            txtReference.Clear();

            txtDescription.Clear();

            txtReceiptImage.Clear();

            dtpDate.Value =
                DateTime.Now;

            if (
                cmbCategory.Items.Count > 0)
            {
                cmbCategory.SelectedIndex =
                    0;
            }

            if (
                cmbBank.Items.Count > 0)
            {
                cmbBank.SelectedIndex =
                    0;
            }

            selectedReceiptFiles.Clear();

            selectedFilesLabel.Text =
                "لم يتم اختيار إشعارات بعد.";

            selectedFilesLabel.ForeColor =
                Color.Gray;

            progressLabel.Text = "";
        }
    }
}