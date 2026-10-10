using System;
using System.Drawing;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using Microsoft.Data.Sqlite;

namespace TransferManagementSystem
{
public class SettingsForm : Form
{
private Panel sidebar;
private Panel contentPanel;

    private Label databaseInfoLabel;
    private Label transferInfoLabel;
    private Label backupInfoLabel;

    public SettingsForm()
    {
        InitializeSettings();
    }

    private void InitializeSettings()
    {
        Text = "الإعدادات - نظام إدارة التحويلات والمصروفات";
        StartPosition = FormStartPosition.CenterScreen;
        MinimumSize = new Size(1000, 650);
        BackColor = Color.FromArgb(245, 247, 250);
        WindowState = FormWindowState.Maximized;
        RightToLeft = RightToLeft.Yes;
        RightToLeftLayout = true;

        TableLayoutPanel mainContainer = new TableLayoutPanel();

        mainContainer.Dock = DockStyle.Fill;
        mainContainer.Margin = new Padding(0);
        mainContainer.Padding = new Padding(0);
        mainContainer.ColumnCount = 2;
        mainContainer.RowCount = 1;

        mainContainer.ColumnStyles.Add(
            new ColumnStyle(SizeType.Absolute, 230)
        );

        mainContainer.ColumnStyles.Add(
            new ColumnStyle(SizeType.Percent, 100)
        );

        mainContainer.RowStyles.Add(
            new RowStyle(SizeType.Percent, 100)
        );

        Controls.Add(mainContainer);

        sidebar = new Panel();

        sidebar.Dock = DockStyle.Fill;
        sidebar.BackColor = Color.FromArgb(15, 38, 62);

        mainContainer.Controls.Add(
            sidebar,
            0,
            0
        );

        contentPanel = new Panel();

        contentPanel.Dock = DockStyle.Fill;
        contentPanel.BackColor = Color.FromArgb(245, 247, 250);
        contentPanel.Padding = new Padding(30);
        contentPanel.AutoScroll = true;
        contentPanel.RightToLeft = RightToLeft.Yes;

        mainContainer.Controls.Add(
            contentPanel,
            1,
            0
        );

        CreateSidebar();
        CreateSettingsPage();
    }

    private void CreateSidebar()
    {
        TableLayoutPanel layout = new TableLayoutPanel();

        layout.Dock = DockStyle.Fill;
        layout.Padding = new Padding(20);
        layout.BackColor = Color.FromArgb(15, 38, 62);
        layout.ColumnCount = 1;
        layout.RowCount = 3;

        layout.ColumnStyles.Add(
            new ColumnStyle(SizeType.Percent, 100)
        );

        layout.RowStyles.Add(
            new RowStyle(SizeType.Absolute, 105)
        );

        layout.RowStyles.Add(
            new RowStyle(SizeType.Percent, 100)
        );

        layout.RowStyles.Add(
            new RowStyle(SizeType.Absolute, 60)
        );

        sidebar.Controls.Add(layout);

        Label title = new Label();

        title.Text = "نظام التحويلات\r\nوالمصروفات";
        title.Dock = DockStyle.Fill;
        title.ForeColor = Color.White;
        title.Font = new Font("Tahoma", 17, FontStyle.Bold);
        title.TextAlign = ContentAlignment.MiddleCenter;

        layout.Controls.Add(title, 0, 0);

        FlowLayoutPanel buttons = new FlowLayoutPanel();

        buttons.Dock = DockStyle.Fill;
        buttons.FlowDirection = FlowDirection.TopDown;
        buttons.WrapContents = false;
        buttons.Padding = new Padding(0, 20, 0, 0);
        buttons.BackColor = Color.FromArgb(15, 38, 62);

        layout.Controls.Add(buttons, 0, 1);

        Button home = CreateSideButton("العودة للرئيسية");

        home.Click += (sender, e) =>
        {
            Close();
        };

        buttons.Controls.Add(home);

        Button logout = new Button();

        logout.Text = "تسجيل الخروج";
        logout.Width = 190;
        logout.Height = 50;
        logout.Margin = new Padding(0);
        logout.BackColor = Color.FromArgb(190, 55, 55);
        logout.ForeColor = Color.White;
        logout.FlatStyle = FlatStyle.Flat;
        logout.FlatAppearance.BorderSize = 0;
        logout.Font = new Font("Tahoma", 10, FontStyle.Bold);
        logout.Cursor = Cursors.Hand;

        logout.Click += (sender, e) =>
        {
            Application.Exit();
        };

        layout.Controls.Add(logout, 0, 2);
    }

    private Button CreateSideButton(string text)
    {
        Button button = new Button();

        button.Text = text;
        button.Width = 190;
        button.Height = 50;
        button.Margin = new Padding(0, 0, 0, 8);
        button.BackColor = Color.FromArgb(15, 38, 62);
        button.ForeColor = Color.White;
        button.FlatStyle = FlatStyle.Flat;
        button.FlatAppearance.BorderSize = 0;
        button.Font = new Font("Tahoma", 10, FontStyle.Bold);
        button.Cursor = Cursors.Hand;

        return button;
    }

    private void CreateSettingsPage()
    {
        contentPanel.Controls.Clear();

        TableLayoutPanel mainLayout = new TableLayoutPanel();

        mainLayout.Dock = DockStyle.Top;
        mainLayout.AutoSize = true;
        mainLayout.ColumnCount = 1;
        mainLayout.RowCount = 7;
        mainLayout.RightToLeft = RightToLeft.Yes;

        mainLayout.ColumnStyles.Add(
            new ColumnStyle(SizeType.Percent, 100)
        );

        Label pageTitle = new Label();

        pageTitle.Text = "الإعدادات";
        pageTitle.Height = 55;
        pageTitle.Dock = DockStyle.Fill;
        pageTitle.Font = new Font("Tahoma", 23, FontStyle.Bold);
        pageTitle.ForeColor = Color.FromArgb(25, 45, 65);
        pageTitle.TextAlign = ContentAlignment.MiddleRight;

        mainLayout.Controls.Add(pageTitle, 0, 0);

        Label description = new Label();

        description.Text =
            "من هنا يمكنك إدارة إعدادات النظام والحساب والبيانات والنسخ الاحتياطية.";

        description.Height = 40;
        description.Dock = DockStyle.Fill;
        description.Font = new Font("Tahoma", 10);
        description.ForeColor = Color.Gray;
        description.TextAlign = ContentAlignment.MiddleRight;

        mainLayout.Controls.Add(description, 0, 1);

        Panel accountCard = CreateSettingsCard(
            "إعدادات الحساب",
            "إدارة اسم المستخدم وكلمة المرور."
        );

        Button changeCredentials = CreateActionButton(
            "تغيير بيانات الدخول"
        );

        changeCredentials.Click += ChangeCredentials_Click;

        accountCard.Controls.Add(changeCredentials);

        mainLayout.Controls.Add(accountCard, 0, 2);

        Panel securityCard = CreateSettingsCard(
            "أمان الحساب",
            "إدارة كود الاسترداد المستخدم عند نسيان كلمة المرور."
        );

        Button changeRecoveryCode = CreateActionButton(
            "تغيير كود الاسترداد"
        );

        changeRecoveryCode.Click += ChangeRecoveryCode_Click;

        securityCard.Controls.Add(changeRecoveryCode);

        mainLayout.Controls.Add(securityCard, 0, 3);

        Panel dataCard = CreateSettingsCard(
            "إدارة البيانات",
            "إنشاء نسخة احتياطية أو استعادة نسخة محفوظة من قاعدة البيانات."
        );

        Button backup = CreateActionButton("نسخ احتياطي");

        backup.Click += Backup_Click;

        dataCard.Controls.Add(backup);

        Button restore = CreateActionButton("استعادة البيانات");

        restore.Click += Restore_Click;

        dataCard.Controls.Add(restore);

        Button openFolder = CreateActionButton("فتح مجلد البيانات");

        openFolder.Click += OpenDataFolder_Click;

        dataCard.Controls.Add(openFolder);

        mainLayout.Controls.Add(dataCard, 0, 4);

        Panel databaseCard = CreateSettingsCard(
            "معلومات قاعدة البيانات",
            "معلومات مباشرة عن قاعدة البيانات وعدد التحويلات الموجودة."
        );

        databaseInfoLabel = CreateInfoLabel();
        transferInfoLabel = CreateInfoLabel();
        backupInfoLabel = CreateInfoLabel();

        databaseCard.Controls.Add(databaseInfoLabel);
        databaseCard.Controls.Add(transferInfoLabel);
        databaseCard.Controls.Add(backupInfoLabel);

        Button refreshInfo = CreateActionButton(
            "تحديث المعلومات"
        );

        refreshInfo.Click += (sender, e) =>
        {
            LoadDatabaseInformation();

            MessageBox.Show(
                "تم تحديث معلومات قاعدة البيانات.",
                "تم",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
        };

        databaseCard.Controls.Add(refreshInfo);

        mainLayout.Controls.Add(databaseCard, 0, 5);

        Panel aboutCard = CreateSettingsCard(
            "معلومات البرنامج",
            "نظام إدارة التحويلات والمصروفات"
        );

        Label version = CreateAboutLabel(
            "الإصدار: 1.0.0"
        );

        aboutCard.Controls.Add(version);

        Label databasePath = CreateAboutLabel(
            "ملف قاعدة البيانات: transfers.db"
        );

        aboutCard.Controls.Add(databasePath);

        mainLayout.Controls.Add(aboutCard, 0, 6);

        contentPanel.Controls.Add(mainLayout);

        LoadDatabaseInformation();
    }

    private Panel CreateSettingsCard(
        string title,
        string description)
    {
        Panel card = new Panel();

        card.Height = 145;
        card.Dock = DockStyle.Top;
        card.Margin = new Padding(0, 10, 0, 10);
        card.BackColor = Color.White;
        card.BorderStyle = BorderStyle.FixedSingle;

        Label titleLabel = new Label();

        titleLabel.Text = title;
        titleLabel.Dock = DockStyle.Top;
        titleLabel.Height = 42;
        titleLabel.Padding = new Padding(15, 5, 15, 0);
        titleLabel.Font = new Font("Tahoma", 13, FontStyle.Bold);
        titleLabel.ForeColor = Color.FromArgb(30, 50, 70);
        titleLabel.TextAlign = ContentAlignment.MiddleRight;

        card.Controls.Add(titleLabel);

        Label descriptionLabel = new Label();

        descriptionLabel.Text = description;
        descriptionLabel.Dock = DockStyle.Top;
        descriptionLabel.Height = 35;
        descriptionLabel.Padding = new Padding(15, 0, 15, 0);
        descriptionLabel.Font = new Font("Tahoma", 9);
        descriptionLabel.ForeColor = Color.Gray;
        descriptionLabel.TextAlign = ContentAlignment.MiddleRight;

        card.Controls.Add(descriptionLabel);

        return card;
    }

    private Label CreateInfoLabel()
    {
        Label label = new Label();

        label.Dock = DockStyle.Top;
        label.Height = 25;
        label.Padding = new Padding(15, 2, 15, 0);
        label.Font = new Font("Tahoma", 9);
        label.ForeColor = Color.FromArgb(80, 90, 100);
        label.TextAlign = ContentAlignment.MiddleRight;

        return label;
    }

    private Label CreateAboutLabel(string text)
    {
        Label label = new Label();

        label.Text = text;
        label.Dock = DockStyle.Top;
        label.Height = 35;
        label.Padding = new Padding(15, 5, 15, 0);
        label.Font = new Font("Tahoma", 9);
        label.ForeColor = Color.Gray;
        label.TextAlign = ContentAlignment.MiddleRight;

        return label;
    }

    private Button CreateActionButton(string text)
    {
        Button button = new Button();

        button.Text = text;
        button.Width = 160;
        button.Height = 38;
        button.Dock = DockStyle.Right;
        button.Margin = new Padding(10);
        button.BackColor = Color.FromArgb(20, 60, 95);
        button.ForeColor = Color.White;
        button.FlatStyle = FlatStyle.Flat;
        button.FlatAppearance.BorderSize = 0;
        button.Font = new Font("Tahoma", 9, FontStyle.Bold);
        button.Cursor = Cursors.Hand;

        return button;
    }

    private void ChangeRecoveryCode_Click(
        object sender,
        EventArgs e)
    {
        Form form = new Form();

        form.Text = "تغيير كود الاسترداد";
        form.StartPosition = FormStartPosition.CenterParent;
        form.Size = new Size(520, 480);
        form.MinimumSize = new Size(520, 480);
        form.BackColor = Color.FromArgb(245, 247, 250);
        form.RightToLeft = RightToLeft.Yes;
        form.RightToLeftLayout = true;

        Label title = new Label();

        title.Text = "تغيير كود الاسترداد";
        title.Font = new Font("Tahoma", 19, FontStyle.Bold);
        title.ForeColor = Color.FromArgb(25, 45, 65);
        title.AutoSize = true;
        title.Location = new Point(145, 25);

        form.Controls.Add(title);

        Label info = new Label();

        info.Text =
            "أدخل بيانات الدخول الحالية ثم اختر كود استرداد جديد.";

        info.Font = new Font("Tahoma", 9);
        info.ForeColor = Color.Gray;
        info.AutoSize = true;
        info.Location = new Point(85, 65);

        form.Controls.Add(info);

        Label usernameLabel = new Label();

        usernameLabel.Text = "اسم المستخدم";
        usernameLabel.Font = new Font("Tahoma", 10, FontStyle.Bold);
        usernameLabel.AutoSize = true;
        usernameLabel.Location = new Point(45, 105);

        form.Controls.Add(usernameLabel);

        TextBox usernameBox = new TextBox();

        usernameBox.Font = new Font("Tahoma", 11);
        usernameBox.Size = new Size(410, 32);
        usernameBox.Location = new Point(45, 130);

        form.Controls.Add(usernameBox);

        Label passwordLabel = new Label();

        passwordLabel.Text = "كلمة المرور الحالية";
        passwordLabel.Font = new Font("Tahoma", 10, FontStyle.Bold);
        passwordLabel.AutoSize = true;
        passwordLabel.Location = new Point(45, 175);

        form.Controls.Add(passwordLabel);

        TextBox passwordBox = new TextBox();

        passwordBox.Font = new Font("Tahoma", 11);
        passwordBox.Size = new Size(350, 32);
        passwordBox.Location = new Point(45, 200);
        passwordBox.PasswordChar = '●';
        passwordBox.RightToLeft = System.Windows.Forms.RightToLeft.No;

        form.Controls.Add(passwordBox);

        Button passwordEye = CreatePasswordEyeButton();

        passwordEye.Location = new Point(400, 200);

        passwordEye.Click += (s, args) =>
        {
            TogglePassword(
                passwordBox,
                passwordEye
            );
        };

        form.Controls.Add(passwordEye);

        Label newLabel = new Label();

        newLabel.Text = "كود الاسترداد الجديد";
        newLabel.Font = new Font("Tahoma", 10, FontStyle.Bold);
        newLabel.AutoSize = true;
        newLabel.Location = new Point(45, 245);

        form.Controls.Add(newLabel);

        TextBox newCodeBox = new TextBox();

        newCodeBox.Font = new Font("Tahoma", 11);
        newCodeBox.Size = new Size(410, 32);
        newCodeBox.Location = new Point(45, 270);
        newCodeBox.PasswordChar = '●';
        newCodeBox.RightToLeft = System.Windows.Forms.RightToLeft.No;

        form.Controls.Add(newCodeBox);

        Label confirmLabel = new Label();

        confirmLabel.Text = "تأكيد كود الاسترداد الجديد";
        confirmLabel.Font = new Font("Tahoma", 10, FontStyle.Bold);
        confirmLabel.AutoSize = true;
        confirmLabel.Location = new Point(45, 315);

        form.Controls.Add(confirmLabel);

        TextBox confirmBox = new TextBox();

        confirmBox.Font = new Font("Tahoma", 11);
        confirmBox.Size = new Size(410, 32);
        confirmBox.Location = new Point(45, 340);
        confirmBox.PasswordChar = '●';
        confirmBox.RightToLeft = System.Windows.Forms.RightToLeft.No;

        form.Controls.Add(confirmBox);

        Button saveButton = new Button();

        saveButton.Text = "حفظ كود الاسترداد";
        saveButton.Font = new Font("Tahoma", 10, FontStyle.Bold);
        saveButton.BackColor = Color.FromArgb(20, 60, 95);
        saveButton.ForeColor = Color.White;
        saveButton.FlatStyle = FlatStyle.Flat;
        saveButton.FlatAppearance.BorderSize = 0;
        saveButton.Size = new Size(190, 42);
        saveButton.Location = new Point(155, 400);
        saveButton.Cursor = Cursors.Hand;

        saveButton.Click += (s, args) =>
        {
            string username =
                usernameBox.Text.Trim();

            string password =
                passwordBox.Text;

            string newCode =
                newCodeBox.Text.Trim();

            string confirmCode =
                confirmBox.Text.Trim();

            if (string.IsNullOrWhiteSpace(username))
            {
                MessageBox.Show(
                    "أدخل اسم المستخدم.",
                    "تنبيه",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                usernameBox.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show(
                    "أدخل كلمة المرور الحالية.",
                    "تنبيه",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                passwordBox.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(newCode))
            {
                MessageBox.Show(
                    "أدخل كود الاسترداد الجديد.",
                    "تنبيه",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                newCodeBox.Focus();
                return;
            }

            if (newCode.Length < 4)
            {
                MessageBox.Show(
                    "كود الاسترداد يجب أن يكون 4 أحرف أو أرقام على الأقل.",
                    "تنبيه",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                newCodeBox.Focus();
                return;
            }

            if (newCode != confirmCode)
            {
                MessageBox.Show(
                    "كود الاسترداد الجديد وتأكيده غير متطابقين.",
                    "تنبيه",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                confirmBox.Focus();
                return;
            }

            try
            {
                bool changed =
                    Database.ChangeRecoveryCode(
                        username,
                        password,
                        newCode
                    );

                if (!changed)
                {
                    MessageBox.Show(
                        "اسم المستخدم أو كلمة المرور الحالية غير صحيحة.",
                        "فشل التحقق",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );

                    return;
                }

                MessageBox.Show(
                    "تم تغيير كود الاسترداد بنجاح.",
                    "تم الحفظ",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                form.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "حدث خطأ أثناء تغيير كود الاسترداد:\n\n"
                    + ex.Message,
                    "خطأ",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        };

        form.Controls.Add(saveButton);

        form.AcceptButton = saveButton;

        form.ShowDialog(this);
    }

    private void ChangeCredentials_Click(
        object sender,
        EventArgs e)
    {
        Form form = new Form();

        form.Text = "تغيير بيانات الدخول";
        form.StartPosition = FormStartPosition.CenterParent;
        form.Size = new Size(520, 520);
        form.MinimumSize = new Size(520, 520);
        form.BackColor = Color.FromArgb(245, 247, 250);
        form.RightToLeft = RightToLeft.Yes;
        form.RightToLeftLayout = true;

        Label title = new Label();

        title.Text = "تغيير بيانات الدخول";
        title.Font = new Font("Tahoma", 19, FontStyle.Bold);
        title.ForeColor = Color.FromArgb(25, 45, 65);
        title.AutoSize = true;
        title.Location = new Point(150, 25);

        form.Controls.Add(title);

        Label usernameLabel = new Label();

        usernameLabel.Text = "اسم المستخدم الحالي";
        usernameLabel.Font = new Font("Tahoma", 10, FontStyle.Bold);
        usernameLabel.AutoSize = true;
        usernameLabel.Location = new Point(45, 80);

        form.Controls.Add(usernameLabel);

        TextBox currentUsernameBox = new TextBox();

        currentUsernameBox.Font = new Font("Tahoma", 11);
        currentUsernameBox.Size = new Size(410, 32);
        currentUsernameBox.Location = new Point(45, 105);

        form.Controls.Add(currentUsernameBox);

        Label currentPasswordLabel = new Label();

        currentPasswordLabel.Text = "كلمة المرور الحالية";
        currentPasswordLabel.Font = new Font("Tahoma", 10, FontStyle.Bold);
        currentPasswordLabel.AutoSize = true;
        currentPasswordLabel.Location = new Point(45, 150);

        form.Controls.Add(currentPasswordLabel);

        TextBox currentPasswordBox = new TextBox();

        currentPasswordBox.Font = new Font("Tahoma", 11);
        currentPasswordBox.Size = new Size(350, 32);
        currentPasswordBox.Location = new Point(45, 175);
        currentPasswordBox.PasswordChar = '●';

        form.Controls.Add(currentPasswordBox);

        Button currentPasswordEye = CreatePasswordEyeButton();

        currentPasswordEye.Location = new Point(400, 175);

        currentPasswordEye.Click += (s, args) =>
        {
            TogglePassword(
                currentPasswordBox,
                currentPasswordEye
            );
        };

        form.Controls.Add(currentPasswordEye);

        Label newUsernameLabel = new Label();

        newUsernameLabel.Text = "اسم المستخدم الجديد";
        newUsernameLabel.Font = new Font("Tahoma", 10, FontStyle.Bold);
        newUsernameLabel.AutoSize = true;
        newUsernameLabel.Location = new Point(45, 220);

        form.Controls.Add(newUsernameLabel);

        TextBox newUsernameBox = new TextBox();

        newUsernameBox.Font = new Font("Tahoma", 11);
        newUsernameBox.Size = new Size(410, 32);
        newUsernameBox.Location = new Point(45, 245);

        form.Controls.Add(newUsernameBox);

        Label newPasswordLabel = new Label();

        newPasswordLabel.Text = "كلمة المرور الجديدة";
        newPasswordLabel.Font = new Font("Tahoma", 10, FontStyle.Bold);
        newPasswordLabel.AutoSize = true;
        newPasswordLabel.Location = new Point(45, 290);

        form.Controls.Add(newPasswordLabel);

        TextBox newPasswordBox = new TextBox();

        newPasswordBox.Font = new Font("Tahoma", 11);
        newPasswordBox.Size = new Size(350, 32);
        newPasswordBox.Location = new Point(45, 315);
        newPasswordBox.PasswordChar = '●';

        form.Controls.Add(newPasswordBox);

        Button newPasswordEye = CreatePasswordEyeButton();

        newPasswordEye.Location = new Point(400, 315);

        newPasswordEye.Click += (s, args) =>
        {
            TogglePassword(
                newPasswordBox,
                newPasswordEye
            );
        };

        form.Controls.Add(newPasswordEye);

        Label confirmLabel = new Label();

        confirmLabel.Text = "تأكيد كلمة المرور الجديدة";
        confirmLabel.Font = new Font("Tahoma", 10, FontStyle.Bold);
        confirmLabel.AutoSize = true;
        confirmLabel.Location = new Point(45, 360);

        form.Controls.Add(confirmLabel);

        TextBox confirmBox = new TextBox();

        confirmBox.Font = new Font("Tahoma", 11);
        confirmBox.Size = new Size(350, 32);
        confirmBox.Location = new Point(45, 385);
        confirmBox.PasswordChar = '●';

        form.Controls.Add(confirmBox);

        Button confirmEye = CreatePasswordEyeButton();

        confirmEye.Location = new Point(400, 385);

        confirmEye.Click += (s, args) =>
        {
            TogglePassword(
                confirmBox,
                confirmEye
            );
        };

        form.Controls.Add(confirmEye);

        Button saveButton = new Button();

        saveButton.Text = "حفظ التغييرات";
        saveButton.Font = new Font("Tahoma", 10, FontStyle.Bold);
        saveButton.BackColor = Color.FromArgb(20, 60, 95);
        saveButton.ForeColor = Color.White;
        saveButton.FlatStyle = FlatStyle.Flat;
        saveButton.FlatAppearance.BorderSize = 0;
        saveButton.Size = new Size(180, 40);
        saveButton.Location = new Point(165, 430);
        saveButton.Cursor = Cursors.Hand;

        saveButton.Click += (s, args) =>
        {
            string currentUsername =
                currentUsernameBox.Text.Trim();

            string currentPassword =
                currentPasswordBox.Text;

            string newUsername =
                newUsernameBox.Text.Trim();

            string newPassword =
                newPasswordBox.Text;

            string confirmPassword =
                confirmBox.Text;

            if (string.IsNullOrWhiteSpace(currentUsername))
            {
                MessageBox.Show(
                    "أدخل اسم المستخدم الحالي.",
                    "تنبيه",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                return;
            }

            if (string.IsNullOrWhiteSpace(currentPassword))
            {
                MessageBox.Show(
                    "أدخل كلمة المرور الحالية.",
                    "تنبيه",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                return;
            }

            if (string.IsNullOrWhiteSpace(newUsername))
            {
                MessageBox.Show(
                    "أدخل اسم المستخدم الجديد.",
                    "تنبيه",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                return;
            }

            if (string.IsNullOrWhiteSpace(newPassword))
            {
                MessageBox.Show(
                    "أدخل كلمة المرور الجديدة.",
                    "تنبيه",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                return;
            }

            if (newPassword.Length < 4)
            {
                MessageBox.Show(
                    "كلمة المرور الجديدة يجب أن تكون 4 أحرف أو أكثر.",
                    "تنبيه",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                return;
            }

            if (newPassword != confirmPassword)
            {
                MessageBox.Show(
                    "كلمتا المرور الجديدتان غير متطابقتين.",
                    "تنبيه",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                return;
            }

            bool changed =
                Database.ChangeCredentials(
                    currentUsername,
                    currentPassword,
                    newUsername,
                    newPassword
                );

            if (!changed)
            {
                MessageBox.Show(
                    "اسم المستخدم الحالي أو كلمة المرور الحالية غير صحيحة، أو اسم المستخدم الجديد مستخدم بالفعل.",
                    "فشل التغيير",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );

                return;
            }

            MessageBox.Show(
                "تم تغيير اسم المستخدم وكلمة المرور بنجاح.",
                "تم",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );

            form.Close();
        };

        form.Controls.Add(saveButton);

        form.AcceptButton = saveButton;

        form.ShowDialog(this);
    }

    private Button CreatePasswordEyeButton()
    {
        Button button = new Button();

        button.Text = "👁";
        button.Font = new Font("Segoe UI Emoji", 10);
        button.Size = new Size(45, 32);
        button.FlatStyle = FlatStyle.Flat;
        button.FlatAppearance.BorderSize = 0;
        button.BackColor = Color.FromArgb(225, 230, 235);
        button.Cursor = Cursors.Hand;

        return button;
    }

    private void TogglePassword(
        TextBox box,
        Button eyeButton)
    {
        if (box.PasswordChar == '\0')
        {
            box.PasswordChar = '●';
            eyeButton.Text = "👁";
        }
        else
        {
            box.PasswordChar = '\0';
            eyeButton.Text = "🙈";
        }
    }

    private void Backup_Click(
        object sender,
        EventArgs e)
    {
        try
        {
            string databasePath =
                Path.Combine(
                    Application.StartupPath,
                    "transfers.db"
                );

            if (!File.Exists(databasePath))
            {
                MessageBox.Show(
                    "لم يتم العثور على قاعدة البيانات.",
                    "خطأ",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );

                return;
            }

            using (SaveFileDialog dialog =
                new SaveFileDialog())
            {
                dialog.Title = "حفظ نسخة احتياطية";
                dialog.Filter = "Database Backup|*.db";
                dialog.FileName =
                    "transfers_backup_"
                    + DateTime.Now.ToString("yyyyMMdd_HHmmss")
                    + ".db";

                if (dialog.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                File.Copy(
                    databasePath,
                    dialog.FileName,
                    true
                );

                MessageBox.Show(
                    "تم إنشاء النسخة الاحتياطية بنجاح.",
                    "تم الحفظ",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                LoadDatabaseInformation();
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                "حدث خطأ أثناء إنشاء النسخة الاحتياطية:\n\n"
                + ex.Message,
                "خطأ",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            );
        }
    }

    private void Restore_Click(
        object sender,
        EventArgs e)
    {
        try
        {
            using (OpenFileDialog dialog =
                new OpenFileDialog())
            {
                dialog.Title = "اختيار النسخة الاحتياطية";
                dialog.Filter = "Database Backup|*.db";

                if (dialog.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                DialogResult confirm =
                    MessageBox.Show(
                        "تحذير:\n\n"
                        + "استعادة النسخة الاحتياطية ستستبدل قاعدة البيانات الحالية.\n\n"
                        + "هل تريد المتابعة؟",
                        "تأكيد الاستعادة",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning
                    );

                if (confirm != DialogResult.Yes)
                {
                    return;
                }

                string databasePath =
                    Path.Combine(
                        Application.StartupPath,
                        "transfers.db"
                    );

                string safetyBackup =
                    Path.Combine(
                        Application.StartupPath,
                        "transfers_before_restore_"
                        + DateTime.Now.ToString("yyyyMMdd_HHmmss")
                        + ".db"
                    );

                if (File.Exists(databasePath))
                {
                    File.Copy(
                        databasePath,
                        safetyBackup,
                        true
                    );
                }

                File.Copy(
                    dialog.FileName,
                    databasePath,
                    true
                );

                MessageBox.Show(
                    "تمت استعادة البيانات بنجاح.\n\n"
                    + "يفضل إعادة تشغيل البرنامج حتى تظهر جميع البيانات بشكل صحيح.",
                    "تمت الاستعادة",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                LoadDatabaseInformation();
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                "حدث خطأ أثناء استعادة البيانات:\n\n"
                + ex.Message,
                "خطأ",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            );
        }
    }

    private void OpenDataFolder_Click(
        object sender,
        EventArgs e)
    {
        try
        {
            string databasePath =
                Path.Combine(
                    Application.StartupPath,
                    "transfers.db"
                );

            string folderPath =
                Path.GetDirectoryName(databasePath);

            if (string.IsNullOrEmpty(folderPath))
            {
                return;
            }

            Process.Start(
                new ProcessStartInfo
                {
                    FileName = folderPath,
                    UseShellExecute = true
                }
            );
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                "تعذر فتح مجلد البيانات:\n\n"
                + ex.Message,
                "خطأ",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            );
        }
    }

    private void LoadDatabaseInformation()
    {
        try
        {
            string databasePath =
                Path.Combine(
                    Application.StartupPath,
                    "transfers.db"
                );

            if (!File.Exists(databasePath))
            {
                databaseInfoLabel.Text =
                    "حالة قاعدة البيانات: غير موجودة";

                transferInfoLabel.Text =
                    "عدد التحويلات: 0";

                backupInfoLabel.Text =
                    "حجم قاعدة البيانات: 0 KB";

                return;
            }

            FileInfo fileInfo =
                new FileInfo(databasePath);

            double sizeKB =
                fileInfo.Length / 1024.0;

            int transferCount = 0;

            using (var connection =
                Database.GetConnection())
            {
                string sql =
                    "SELECT COUNT(*) FROM Transfers;";

                using (var command =
                    new SqliteCommand(
                        sql,
                        connection))
                {
                    object result =
                        command.ExecuteScalar();

                    if (result != null &&
                        result != DBNull.Value)
                    {
                        transferCount =
                            Convert.ToInt32(result);
                    }
                }
            }

            databaseInfoLabel.Text =
                "حالة قاعدة البيانات: تعمل بشكل طبيعي";

            transferInfoLabel.Text =
                "عدد التحويلات: "
                + transferCount.ToString("N0");

            backupInfoLabel.Text =
                "حجم قاعدة البيانات: "
                + sizeKB.ToString("N2")
                + " KB";
        }
        catch
        {
            databaseInfoLabel.Text =
                "حالة قاعدة البيانات: تعذر القراءة";

            transferInfoLabel.Text =
                "عدد التحويلات: غير متاح";

            backupInfoLabel.Text =
                "الحجم: غير متاح";
        }
    }
}


}
