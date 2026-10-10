using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Microsoft.Data.Sqlite;

namespace TransferManagementSystem
{
public class DashboardForm : Form
{
private Panel sidebar;
private Panel contentPanel;

    private Color BackgroundColor =
        Color.FromArgb(245, 247, 250);

    private Color SidebarColor =
        Color.FromArgb(15, 38, 62);

    private Color PrimaryColor =
        Color.FromArgb(25, 75, 115);

    private Color TextColor =
        Color.FromArgb(30, 45, 60);

    public DashboardForm()
    {
        InitializeDashboard();
    }

    // =====================================================
    // إعداد البرنامج
    // =====================================================

    private void InitializeDashboard()
    {
        Text =
            "نظام إدارة التحويلات والمصروفات";

        StartPosition =
            FormStartPosition.CenterScreen;

        MinimumSize =
            new Size(1100, 700);

        BackColor =
            BackgroundColor;

        WindowState =
            FormWindowState.Maximized;

        AutoScaleMode =
            AutoScaleMode.Font;

        RightToLeft =
            RightToLeft.Yes;

        RightToLeftLayout =
            true;

        CreateMainLayout();
    }

    // =====================================================
    // التخطيط الرئيسي
    // =====================================================

    private void CreateMainLayout()
    {
        Controls.Clear();

        TableLayoutPanel mainContainer =
            new TableLayoutPanel();

        mainContainer.Dock =
            DockStyle.Fill;

        mainContainer.Margin =
            new Padding(0);

        mainContainer.Padding =
            new Padding(0);

        mainContainer.ColumnCount =
            2;

        mainContainer.RowCount =
            1;

        mainContainer.RightToLeft =
            RightToLeft.Yes;

        mainContainer.ColumnStyles.Add(
            new ColumnStyle(
                SizeType.Absolute,
                255
            )
        );

        mainContainer.ColumnStyles.Add(
            new ColumnStyle(
                SizeType.Percent,
                100
            )
        );

        mainContainer.RowStyles.Add(
            new RowStyle(
                SizeType.Percent,
                100
            )
        );

        Controls.Add(
            mainContainer
        );

        // =================================================
        // القائمة الجانبية
        // =================================================

        sidebar =
            new Panel();

        sidebar.Dock =
            DockStyle.Fill;

        sidebar.Margin =
            new Padding(0);

        sidebar.Padding =
            new Padding(0);

        sidebar.BackColor =
            SidebarColor;

        mainContainer.Controls.Add(
            sidebar,
            0,
            0
        );

        // =================================================
        // المحتوى
        // =================================================

        contentPanel =
            new Panel();

        contentPanel.Dock =
            DockStyle.Fill;

        contentPanel.Margin =
            new Padding(0);

        contentPanel.Padding =
            new Padding(
                35,
                30,
                35,
                30
            );

        contentPanel.BackColor =
            BackgroundColor;

        mainContainer.Controls.Add(
            contentPanel,
            1,
            0
        );

        CreateSidebar();

        CreateHomePage();
    }

    // =====================================================
    // القائمة الجانبية
    // =====================================================

    private void CreateSidebar()
    {
        sidebar.Controls.Clear();

        TableLayoutPanel sidebarLayout =
            new TableLayoutPanel();

        sidebarLayout.Dock =
            DockStyle.Fill;

        sidebarLayout.Margin =
            new Padding(0);

        sidebarLayout.Padding =
            new Padding(
                18,
                20,
                18,
                18
            );

        sidebarLayout.BackColor =
            SidebarColor;

        sidebarLayout.ColumnCount =
            1;

        sidebarLayout.RowCount =
            4;

        sidebarLayout.ColumnStyles.Add(
            new ColumnStyle(
                SizeType.Percent,
                100
            )
        );

        sidebarLayout.RowStyles.Add(
            new RowStyle(
                SizeType.Absolute,
                110
            )
        );

        sidebarLayout.RowStyles.Add(
            new RowStyle(
                SizeType.Absolute,
                1
            )
        );

        sidebarLayout.RowStyles.Add(
            new RowStyle(
                SizeType.Percent,
                100
            )
        );

        sidebarLayout.RowStyles.Add(
            new RowStyle(
                SizeType.Absolute,
                55
            )
        );

        sidebar.Controls.Add(
            sidebarLayout
        );

        // =================================================
        // رأس القائمة
        // =================================================

        Panel brandPanel =
            new Panel();

        brandPanel.Dock =
            DockStyle.Fill;

        brandPanel.Margin =
            new Padding(0);

        brandPanel.BackColor =
            SidebarColor;

        sidebarLayout.Controls.Add(
            brandPanel,
            0,
            0
        );

        Label brandTitle =
            new Label();

        brandTitle.Text =
            "نظام التحويلات";

        brandTitle.ForeColor =
            Color.White;

        brandTitle.Font =
            new Font(
                "Tahoma",
                18,
                FontStyle.Bold
            );

        brandTitle.AutoSize =
            true;

        brandTitle.Location =
            new Point(
                30,
                20
            );

        brandTitle.RightToLeft =
            RightToLeft.Yes;

        brandPanel.Controls.Add(
            brandTitle
        );

        Label brandSubtitle =
            new Label();

        brandSubtitle.Text =
            "والمصروفات";

        brandSubtitle.ForeColor =
            Color.FromArgb(
                155,
                180,
                200
            );

        brandSubtitle.Font =
            new Font(
                "Tahoma",
                11
            );

        brandSubtitle.AutoSize =
            true;

        brandSubtitle.Location =
            new Point(
                68,
                53
            );

        brandSubtitle.RightToLeft =
            RightToLeft.Yes;

        brandPanel.Controls.Add(
            brandSubtitle
        );

        Label brandLine =
            new Label();

        brandLine.Text =
            "إدارة • متابعة • تقارير";

        brandLine.ForeColor =
            Color.FromArgb(
                100,
                135,
                160
            );

        brandLine.Font =
            new Font(
                "Tahoma",
                8
            );

        brandLine.AutoSize =
            true;

        brandLine.Location =
            new Point(
                63,
                82
            );

        brandPanel.Controls.Add(
            brandLine
        );

        // =================================================
        // الخط الفاصل
        // =================================================

        Panel separator =
            new Panel();

        separator.Dock =
            DockStyle.Fill;

        separator.Margin =
            new Padding(0);

        separator.BackColor =
            Color.FromArgb(
                45,
                70,
                95
            );

        sidebarLayout.Controls.Add(
            separator,
            0,
            1
        );

        // =================================================
        // الأزرار
        // =================================================

        FlowLayoutPanel menuPanel =
            new FlowLayoutPanel();

        menuPanel.Dock =
            DockStyle.Fill;

        menuPanel.Margin =
            new Padding(0);

        menuPanel.Padding =
            new Padding(
                0,
                18,
                0,
                10
            );

        menuPanel.FlowDirection =
            FlowDirection.TopDown;

        menuPanel.WrapContents =
            false;

        menuPanel.AutoScroll =
            true;

        menuPanel.BackColor =
            SidebarColor;

        menuPanel.RightToLeft =
            RightToLeft.Yes;

        sidebarLayout.Controls.Add(
            menuPanel,
            0,
            2
        );

        // الرئيسية

        Button homeButton =
            CreateMenuButton(
                "الرئيسية",
                true
            );

        homeButton.Click +=
            (sender, e) =>
            {
                CreateHomePage();
            };

        menuPanel.Controls.Add(
            homeButton
        );

        // إضافة تحويل

        Button addTransferButton =
            CreateMenuButton(
                "إضافة تحويل جديد",
                false
            );

        addTransferButton.Click +=
            (sender, e) =>
            {
                using (
                    AddTransferForm form =
                    new AddTransferForm()
                )
                {
                    form.ShowDialog();
                }

                CreateHomePage();
            };

        menuPanel.Controls.Add(
            addTransferButton
        );

        // جميع التحويلات

        Button allTransfersButton =
            CreateMenuButton(
                "جميع التحويلات",
                false
            );

        allTransfersButton.Click +=
            (sender, e) =>
            {
                using (
                    AllTransfersForm form =
                    new AllTransfersForm()
                )
                {
                    form.ShowDialog();
                }

                CreateHomePage();
            };

        menuPanel.Controls.Add(
            allTransfersButton
        );

        // التصنيفات

        Button categoriesButton =
            CreateMenuButton(
                "التصنيفات",
                false
            );

        categoriesButton.Click +=
            (sender, e) =>
            {
                using (
                    CategoriesForm form =
                    new CategoriesForm()
                )
                {
                    form.ShowDialog();
                }

                CreateHomePage();
            };

        menuPanel.Controls.Add(
            categoriesButton
        );

        // التقارير

        Button reportsButton =
            CreateMenuButton(
                "التقارير",
                false
            );

        reportsButton.Click +=
            (sender, e) =>
            {
                using (
                    ReportsForm form =
                    new ReportsForm()
                )
                {
                    form.ShowDialog();
                }

                CreateHomePage();
            };

        menuPanel.Controls.Add(
            reportsButton
        );

        // الإعدادات

        Button settingsButton =
            CreateMenuButton(
                "الإعدادات",
                false
            );

        settingsButton.Click +=
            (sender, e) =>
            {
                using (
                    SettingsForm form =
                    new SettingsForm()
                )
                {
                    form.ShowDialog();
                }

                CreateHomePage();
            };

        menuPanel.Controls.Add(
            settingsButton
        );

        // =================================================
        // تسجيل الخروج
        // =================================================

        Button logoutButton =
            new Button();

        logoutButton.Text =
            "تسجيل الخروج";

        logoutButton.Dock =
            DockStyle.Fill;

        logoutButton.Margin =
            new Padding(
                0,
                5,
                0,
                0
            );

        logoutButton.BackColor =
            Color.FromArgb(
                185,
                60,
                60
            );

        logoutButton.ForeColor =
            Color.White;

        logoutButton.FlatStyle =
            FlatStyle.Flat;

        logoutButton.FlatAppearance.BorderSize =
            0;

        logoutButton.Font =
            new Font(
                "Tahoma",
                9,
                FontStyle.Bold
            );

        logoutButton.Cursor =
            Cursors.Hand;

        logoutButton.Click +=
            (sender, e) =>
            {
                Close();
            };

        sidebarLayout.Controls.Add(
            logoutButton,
            0,
            3
        );
    }

    // =====================================================
    // زر القائمة
    // =====================================================

    private Button CreateMenuButton(
        string text,
        bool active)
    {
        Button button =
            new Button();

        button.Text =
            text;

        button.Width =
            219;

        button.Height =
            48;

        button.Margin =
            new Padding(
                0,
                0,
                0,
                7
            );

        button.BackColor =
            active
            ? Color.FromArgb(
                35,
                82,
                120
            )
            : SidebarColor;

        button.ForeColor =
            Color.White;

        button.FlatStyle =
            FlatStyle.Flat;

        button.FlatAppearance.BorderSize =
            0;

        button.Font =
            new Font(
                "Tahoma",
                9.5f,
                FontStyle.Bold
            );

        button.Cursor =
            Cursors.Hand;

        button.TextAlign =
            ContentAlignment.MiddleCenter;

        button.RightToLeft =
            RightToLeft.Yes;

        return button;
    }

    // =====================================================
    // الصفحة الرئيسية
    // =====================================================

    private void CreateHomePage()
    {
        contentPanel.Controls.Clear();

        TableLayoutPanel mainLayout =
            new TableLayoutPanel();

        mainLayout.Dock =
            DockStyle.Fill;

        mainLayout.Margin =
            new Padding(0);

        mainLayout.Padding =
            new Padding(0);

        mainLayout.ColumnCount =
            1;

        mainLayout.RowCount =
            4;

        mainLayout.ColumnStyles.Add(
            new ColumnStyle(
                SizeType.Percent,
                100
            )
        );

        mainLayout.RowStyles.Add(
            new RowStyle(
                SizeType.Absolute,
                75
            )
        );

        mainLayout.RowStyles.Add(
            new RowStyle(
                SizeType.Absolute,
                135
            )
        );

        mainLayout.RowStyles.Add(
            new RowStyle(
                SizeType.Absolute,
                20
            )
        );

        mainLayout.RowStyles.Add(
            new RowStyle(
                SizeType.Percent,
                100
            )
        );

        contentPanel.Controls.Add(
            mainLayout
        );

        CreateHeader(
            mainLayout
        );

        CreateCards(
            mainLayout
        );

        CreateLatestTransfers(
            mainLayout
        );
    }

    // =====================================================
    // رأس الصفحة
    // =====================================================

    private void CreateHeader(
        TableLayoutPanel parent)
    {
        Panel header =
            new Panel();

        header.Dock =
            DockStyle.Fill;

        header.Margin =
            new Padding(0);

        header.BackColor =
            BackgroundColor;

        parent.Controls.Add(
            header,
            0,
            0
        );

        Label title =
            new Label();

        title.Text =
            "الرئيسية";

        title.ForeColor =
            TextColor;

        title.Font =
            new Font(
                "Tahoma",
                24,
                FontStyle.Bold
            );

        title.AutoSize =
            true;

        title.Location =
            new Point(
                0,
                2
            );

        title.RightToLeft =
            RightToLeft.Yes;

        header.Controls.Add(
            title
        );

        Label subtitle =
            new Label();

        subtitle.Text =
            "مرحباً بك في نظام إدارة التحويلات والمصروفات";

        subtitle.ForeColor =
            Color.FromArgb(
                120,
                130,
                140
            );

        subtitle.Font =
            new Font(
                "Tahoma",
                9.5f
            );

        subtitle.AutoSize =
            true;

        subtitle.Location =
            new Point(
                2,
                43
            );

        subtitle.RightToLeft =
            RightToLeft.Yes;

        header.Controls.Add(
            subtitle
        );

        Label dateLabel =
            new Label();

        dateLabel.Text =
            DateTime.Now.ToString(
                "yyyy/MM/dd"
            );

        dateLabel.ForeColor =
            Color.FromArgb(
                100,
                115,
                130
            );

        dateLabel.Font =
            new Font(
                "Tahoma",
                9
            );

        dateLabel.AutoSize =
            true;

        dateLabel.Anchor =
            AnchorStyles.Top |
            AnchorStyles.Left;

        dateLabel.Location =
            new Point(
                5,
                10
            );

        header.Controls.Add(
            dateLabel
        );
    }

    // =====================================================
    // البطاقات
    // =====================================================

    private void CreateCards(
        TableLayoutPanel parent)
    {
        TableLayoutPanel cards =
            new TableLayoutPanel();

        cards.Dock =
            DockStyle.Fill;

        cards.Margin =
            new Padding(0);

        cards.Padding =
            new Padding(0);

        cards.ColumnCount =
            4;

        cards.RowCount =
            1;

        cards.RightToLeft =
            RightToLeft.Yes;

        for (int i = 0; i < 4; i++)
        {
            cards.ColumnStyles.Add(
                new ColumnStyle(
                    SizeType.Percent,
                    25
                )
            );
        }

        cards.RowStyles.Add(
            new RowStyle(
                SizeType.Percent,
                100
            )
        );

        parent.Controls.Add(
            cards,
            0,
            1
        );

        decimal total =
            GetTotalExpenses();

        int count =
            GetTransferCount();

        decimal materials =
            GetCategoryTotal(
                "المواد"
            );

        decimal workers =
            GetCategoryTotal(
                "العمال"
            );

        CreateCard(
            cards,
            0,
            "إجمالي المصروفات",
            FormatNumber(total),
            "جنيه",
            Color.FromArgb(
                45,
                180,
                140
            )
        );

        CreateCard(
            cards,
            1,
            "عدد التحويلات",
            count.ToString("N0"),
            "تحويل",
            Color.FromArgb(
                55,
                125,
                215
            )
        );

        CreateCard(
            cards,
            2,
            "المواد",
            FormatNumber(materials),
            "جنيه",
            Color.FromArgb(
                235,
                165,
                55
            )
        );

        CreateCard(
            cards,
            3,
            "العمال",
            FormatNumber(workers),
            "جنيه",
            Color.FromArgb(
                135,
                85,
                185
            )
        );
    }

    // =====================================================
    // البطاقة
    // =====================================================

    private void CreateCard(
        TableLayoutPanel parent,
        int column,
        string title,
        string number,
        string unit,
        Color color)
    {
        Panel card =
            new Panel();

        card.Dock =
            DockStyle.Fill;

        card.Margin =
            new Padding(
                5,
                0,
                5,
                0
            );

        card.BackColor =
            Color.White;

        card.Padding =
            new Padding(
                18,
                12,
                18,
                10
            );

        parent.Controls.Add(
            card,
            column,
            0
        );

        Panel accent =
            new Panel();

        accent.Dock =
            DockStyle.Right;

        accent.Width =
            5;

        accent.BackColor =
            color;

        card.Controls.Add(
            accent
        );

        Label titleLabel =
            new Label();

        titleLabel.Text =
            title;

        titleLabel.ForeColor =
            Color.FromArgb(
                115,
                125,
                135
            );

        titleLabel.Font =
            new Font(
                "Tahoma",
                9
            );

        titleLabel.AutoSize =
            true;

        titleLabel.Location =
            new Point(
                18,
                15
            );

        titleLabel.RightToLeft =
            RightToLeft.Yes;

        card.Controls.Add(
            titleLabel
        );

        Label numberLabel =
            new Label();

        numberLabel.Text =
            number;

        numberLabel.ForeColor =
            color;

        numberLabel.Font =
            new Font(
                "Tahoma",
                20,
                FontStyle.Bold
            );

        numberLabel.AutoSize =
            true;

        numberLabel.Location =
            new Point(
                18,
                42
            );

        numberLabel.RightToLeft =
            RightToLeft.Yes;

        card.Controls.Add(
            numberLabel
        );

        Label unitLabel =
            new Label();

        unitLabel.Text =
            unit;

        unitLabel.ForeColor =
            Color.FromArgb(
                140,
                145,
                150
            );

        unitLabel.Font =
            new Font(
                "Tahoma",
                8.5f
            );

        unitLabel.AutoSize =
            true;

        unitLabel.Location =
            new Point(
                20,
                88
            );

        unitLabel.RightToLeft =
            RightToLeft.Yes;

        card.Controls.Add(
            unitLabel
        );
    }

    // =====================================================
    // آخر التحويلات
    // =====================================================

    private void CreateLatestTransfers(
        TableLayoutPanel parent)
    {
        Panel tablePanel =
            new Panel();

        tablePanel.Dock =
            DockStyle.Fill;

        tablePanel.Margin =
            new Padding(0);

        tablePanel.BackColor =
            Color.White;

        parent.Controls.Add(
            tablePanel,
            0,
            3
        );

        Label title =
            new Label();

        title.Text =
            "آخر التحويلات";

        title.ForeColor =
            TextColor;

        title.Font =
            new Font(
                "Tahoma",
                14,
                FontStyle.Bold
            );

        title.AutoSize =
            true;

        title.Location =
            new Point(
                20,
                15
            );

        title.RightToLeft =
            RightToLeft.Yes;

        tablePanel.Controls.Add(
            title
        );

        DataGridView grid =
            new DataGridView();

        grid.Dock =
            DockStyle.Fill;

        grid.Margin =
            new Padding(
                15,
                55,
                15,
                15
            );

        grid.BackgroundColor =
            Color.White;

        grid.BorderStyle =
            BorderStyle.None;

        grid.AllowUserToAddRows =
            false;

        grid.AllowUserToDeleteRows =
            false;

        grid.AllowUserToResizeRows =
            false;

        grid.ReadOnly =
            true;

        grid.RowHeadersVisible =
            false;

        grid.SelectionMode =
            DataGridViewSelectionMode.FullRowSelect;

        grid.MultiSelect =
            false;

        grid.AutoSizeColumnsMode =
            DataGridViewAutoSizeColumnsMode.Fill;

        grid.RightToLeft =
            RightToLeft.Yes;

        grid.ColumnHeadersHeight =
            42;

        grid.RowTemplate.Height =
            36;

        grid.EnableHeadersVisualStyles =
            false;

        grid.ScrollBars =
            ScrollBars.Both;

        grid.ColumnHeadersDefaultCellStyle.BackColor =
            Color.FromArgb(
                25,
                75,
                115
            );

        grid.ColumnHeadersDefaultCellStyle.ForeColor =
            Color.White;

        grid.ColumnHeadersDefaultCellStyle.Font =
            new Font(
                "Tahoma",
                9,
                FontStyle.Bold
            );

        grid.ColumnHeadersDefaultCellStyle.Alignment =
            DataGridViewContentAlignment.MiddleCenter;

        grid.DefaultCellStyle.Font =
            new Font(
                "Tahoma",
                9
            );

        grid.DefaultCellStyle.Alignment =
            DataGridViewContentAlignment.MiddleCenter;

        grid.DefaultCellStyle.ForeColor =
            Color.FromArgb(
                50,
                60,
                70
            );

        grid.DefaultCellStyle.SelectionBackColor =
            Color.FromArgb(
                220,
                232,
                242
            );

        grid.DefaultCellStyle.SelectionForeColor =
            Color.Black;

        grid.Columns.Add(
            "Date",
            "التاريخ"
        );

        grid.Columns.Add(
            "Beneficiary",
            "المستفيد"
        );

        grid.Columns.Add(
            "Amount",
            "المبلغ"
        );

        grid.Columns.Add(
            "Category",
            "التصنيف"
        );

        grid.Columns.Add(
            "Reference",
            "رقم الإشعار"
        );

        LoadLatestTransfers(
            grid
        );

        tablePanel.Controls.Add(
            grid
        );

        grid.BringToFront();
    }

    // =====================================================
    // إجمالي المصروفات
    // =====================================================

    private decimal GetTotalExpenses()
    {
        try
        {
            using (
                var connection =
                Database.GetConnection()
            )
            {
                string sql =
                    "SELECT COALESCE(SUM(Amount), 0) FROM Transfers";

                using (
                    var command =
                    new SqliteCommand(
                        sql,
                        connection
                    )
                )
                {
                    object result =
                        command.ExecuteScalar();

                    if (result == null ||
                        result == DBNull.Value)
                    {
                        return 0;
                    }

                    return Convert.ToDecimal(
                        result
                    );
                }
            }
        }
        catch
        {
            return 0;
        }
    }

    // =====================================================
    // عدد التحويلات
    // =====================================================

    private int GetTransferCount()
    {
        try
        {
            using (
                var connection =
                Database.GetConnection()
            )
            {
                string sql =
                    "SELECT COUNT(*) FROM Transfers";

                using (
                    var command =
                    new SqliteCommand(
                        sql,
                        connection
                    )
                )
                {
                    object result =
                        command.ExecuteScalar();

                    if (result == null ||
                        result == DBNull.Value)
                    {
                        return 0;
                    }

                    return Convert.ToInt32(
                        result
                    );
                }
            }
        }
        catch
        {
            return 0;
        }
    }

    // =====================================================
    // إجمالي التصنيف
    // =====================================================

    private decimal GetCategoryTotal(
        string category)
    {
        try
        {
            using (
                var connection =
                Database.GetConnection()
            )
            {
                string sql = @"
                    SELECT COALESCE(SUM(Amount), 0)
                    FROM Transfers
                    WHERE Category = $category;
                ";

                using (
                    var command =
                    new SqliteCommand(
                        sql,
                        connection
                    )
                )
                {
                    command.Parameters.AddWithValue(
                        "$category",
                        category
                    );

                    object result =
                        command.ExecuteScalar();

                    if (result == null ||
                        result == DBNull.Value)
                    {
                        return 0;
                    }

                    return Convert.ToDecimal(
                        result
                    );
                }
            }
        }
        catch
        {
            return 0;
        }
    }

    // =====================================================
    // آخر التحويلات
    // =====================================================

    private void LoadLatestTransfers(
        DataGridView grid)
    {
        try
        {
            using (
                var connection =
                Database.GetConnection()
            )
            {
                string sql = @"
                    SELECT
                        TransferDate,
                        Beneficiary,
                        Amount,
                        Category,
                        ReferenceNumber
                    FROM Transfers
                    ORDER BY Id DESC
                    LIMIT 10;
                ";

                using (
                    var command =
                    new SqliteCommand(
                        sql,
                        connection
                    )
                )
                {
                    using (
                        var reader =
                        command.ExecuteReader()
                    )
                    {
                        while (reader.Read())
                        {
                            string date =
                                reader["TransferDate"]
                                ?.ToString() ?? "";

                            string beneficiary =
                                reader["Beneficiary"]
                                ?.ToString() ?? "";

                            decimal amount = 0;

                            if (reader["Amount"] !=
                                DBNull.Value)
                            {
                                decimal.TryParse(
                                    reader["Amount"]
                                    .ToString(),
                                    out amount
                                );
                            }

                            string category =
                                reader["Category"]
                                ?.ToString() ?? "";

                            string reference =
                                reader["ReferenceNumber"]
                                ?.ToString() ?? "";

                            grid.Rows.Add(
                                date.Replace(
                                    "-",
                                    "/"
                                ),
                                beneficiary,
                                FormatNumber(
                                    amount
                                ),
                                category,
                                reference
                            );
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
                MessageBoxIcon.Error
            );
        }
    }

    // =====================================================
    // تنسيق الأرقام
    // =====================================================

    private string FormatNumber(
        decimal number)
    {
        return number.ToString(
            "N0"
        );
    }
}

}
