using System;
using System.Drawing;
using System.Windows.Forms;

namespace TransferManagementSystem
{
    public partial class Form1 : Form
    {
        private TextBox txtUsername;
        private TextBox txtPassword;
        private Button btnLogin;
        private Button btnShowPassword;
        private LinkLabel lnkForgotPassword;
        private Label lblMessage;

        public Form1()
        {
            InitializeComponent();

            Database.Initialize();

            CreateLoginForm();
        }

        private void CreateLoginForm()
        {
            Text =
                "نظام إدارة التحويلات والمصروفات";

            StartPosition =
                FormStartPosition.CenterScreen;

            Size =
                new Size(1100, 650);

            MinimumSize =
                new Size(900, 550);

            BackColor =
                Color.FromArgb(18, 38, 60);

            RightToLeft =
                RightToLeft.No;

            RightToLeftLayout =
                false;

            Controls.Clear();

            TableLayoutPanel mainLayout =
                new TableLayoutPanel();

            mainLayout.Dock =
                DockStyle.Fill;

            mainLayout.ColumnCount =
                2;

            mainLayout.RowCount =
                1;

            mainLayout.ColumnStyles.Add(
                new ColumnStyle(
                    SizeType.Percent,
                    50
                )
            );

            mainLayout.ColumnStyles.Add(
                new ColumnStyle(
                    SizeType.Percent,
                    50
                )
            );

            Controls.Add(mainLayout);

            // =================================================
            // الجانب الأيسر
            // =================================================

            Panel infoPanel =
                new Panel();

            infoPanel.Dock =
                DockStyle.Fill;

            infoPanel.BackColor =
                Color.FromArgb(12, 29, 48);

            mainLayout.Controls.Add(
                infoPanel,
                0,
                0
            );

            TableLayoutPanel infoLayout =
                new TableLayoutPanel();

            infoLayout.Dock =
                DockStyle.Fill;

            infoLayout.ColumnCount =
                1;

            infoLayout.RowCount =
                5;

            infoLayout.RowStyles.Add(
                new RowStyle(
                    SizeType.Percent,
                    25
                )
            );

            infoLayout.RowStyles.Add(
                new RowStyle(
                    SizeType.Absolute,
                    110
                )
            );

            infoLayout.RowStyles.Add(
                new RowStyle(
                    SizeType.Absolute,
                    70
                )
            );

            infoLayout.RowStyles.Add(
                new RowStyle(
                    SizeType.Absolute,
                    100
                )
            );

            infoLayout.RowStyles.Add(
                new RowStyle(
                    SizeType.Percent,
                    75
                )
            );

            infoPanel.Controls.Add(
                infoLayout
            );

            Label lblTitle =
                new Label();

            lblTitle.Text =
                "نظام إدارة\r\nالتحويلات والمصروفات";

            lblTitle.ForeColor =
                Color.White;

            lblTitle.Font =
                new Font(
                    "Tahoma",
                    27,
                    FontStyle.Bold
                );

            lblTitle.Dock =
                DockStyle.Fill;

            lblTitle.TextAlign =
                ContentAlignment.MiddleCenter;

            lblTitle.RightToLeft =
                RightToLeft.Yes;

            infoLayout.Controls.Add(
                lblTitle,
                0,
                1
            );

            Label lblFeatures =
                new Label();

            lblFeatures.Text =
                "تنظيم • متابعة • تقارير";

            lblFeatures.ForeColor =
                Color.FromArgb(
                    180,
                    195,
                    210
                );

            lblFeatures.Font =
                new Font(
                    "Tahoma",
                    14
                );

            lblFeatures.Dock =
                DockStyle.Fill;

            lblFeatures.TextAlign =
                ContentAlignment.MiddleCenter;

            lblFeatures.RightToLeft =
                RightToLeft.Yes;

            infoLayout.Controls.Add(
                lblFeatures,
                0,
                2
            );

            Label lblInfo =
                new Label();

            lblInfo.Text =
                "إدارة التحويلات والمصروفات\r\nبسهولة وسرعة ودقة";

            lblInfo.ForeColor =
                Color.FromArgb(
                    145,
                    165,
                    185
                );

            lblInfo.Font =
                new Font(
                    "Tahoma",
                    10
                );

            lblInfo.Dock =
                DockStyle.Fill;

            lblInfo.TextAlign =
                ContentAlignment.MiddleCenter;

            lblInfo.RightToLeft =
                RightToLeft.Yes;

            infoLayout.Controls.Add(
                lblInfo,
                0,
                3
            );

            // =================================================
            // الجانب الأيمن - تسجيل الدخول
            // =================================================

            Panel loginArea =
                new Panel();

            loginArea.Dock =
                DockStyle.Fill;

            loginArea.BackColor =
                Color.FromArgb(
                    18,
                    38,
                    60
                );

            mainLayout.Controls.Add(
                loginArea,
                1,
                0
            );

            TableLayoutPanel loginCenter =
                new TableLayoutPanel();

            loginCenter.Dock =
                DockStyle.Fill;

            loginCenter.ColumnCount =
                3;

            loginCenter.RowCount =
                3;

            loginCenter.ColumnStyles.Add(
                new ColumnStyle(
                    SizeType.Percent,
                    50
                )
            );

            loginCenter.ColumnStyles.Add(
                new ColumnStyle(
                    SizeType.Absolute,
                    440
                )
            );

            loginCenter.ColumnStyles.Add(
                new ColumnStyle(
                    SizeType.Percent,
                    50
                )
            );

            loginCenter.RowStyles.Add(
                new RowStyle(
                    SizeType.Percent,
                    50
                )
            );

            loginCenter.RowStyles.Add(
                new RowStyle(
                    SizeType.Absolute,
                    500
                )
            );

            loginCenter.RowStyles.Add(
                new RowStyle(
                    SizeType.Percent,
                    50
                )
            );

            loginArea.Controls.Add(
                loginCenter
            );

            Panel loginPanel =
                new Panel();

            loginPanel.Dock =
                DockStyle.Fill;

            loginPanel.BackColor =
                Color.White;

            loginCenter.Controls.Add(
                loginPanel,
                1,
                1
            );

            // =================================================
            // عنوان تسجيل الدخول
            // =================================================

            Label lblLoginTitle =
                new Label();

            lblLoginTitle.Text =
                "تسجيل الدخول";

            lblLoginTitle.ForeColor =
                Color.FromArgb(
                    18,
                    38,
                    60
                );

            lblLoginTitle.Font =
                new Font(
                    "Tahoma",
                    24,
                    FontStyle.Bold
                );

            lblLoginTitle.AutoSize =
                true;

            lblLoginTitle.Location =
                new Point(
                    125,
                    42
                );

            lblLoginTitle.RightToLeft =
                RightToLeft.Yes;

            loginPanel.Controls.Add(
                lblLoginTitle
            );

            Label lblWelcome =
                new Label();

            lblWelcome.Text =
                "أدخل بيانات الدخول للمتابعة";

            lblWelcome.ForeColor =
                Color.Gray;

            lblWelcome.Font =
                new Font(
                    "Tahoma",
                    10
                );

            lblWelcome.AutoSize =
                true;

            lblWelcome.Location =
                new Point(
                    130,
                    88
                );

            lblWelcome.RightToLeft =
                RightToLeft.Yes;

            loginPanel.Controls.Add(
                lblWelcome
            );

            // =================================================
            // اسم المستخدم
            // =================================================

            Label lblUsername =
                new Label();

            lblUsername.Text =
                "اسم المستخدم";

            lblUsername.ForeColor =
                Color.FromArgb(
                    50,
                    50,
                    50
                );

            lblUsername.Font =
                new Font(
                    "Tahoma",
                    10,
                    FontStyle.Bold
                );

            lblUsername.AutoSize =
                true;

            lblUsername.Location =
                new Point(
                    45,
                    135
                );

            lblUsername.RightToLeft =
                RightToLeft.Yes;

            loginPanel.Controls.Add(
                lblUsername
            );

            txtUsername =
                new TextBox();

            txtUsername.Font =
                new Font(
                    "Tahoma",
                    11
                );

            txtUsername.Size =
                new Size(
                    350,
                    34
                );

            txtUsername.Location =
                new Point(
                    45,
                    163
                );

            txtUsername.RightToLeft =
                RightToLeft.Yes;

            loginPanel.Controls.Add(
                txtUsername
            );

            // =================================================
            // كلمة المرور
            // =================================================

            Label lblPassword =
                new Label();

            lblPassword.Text =
                "كلمة المرور";

            lblPassword.ForeColor =
                Color.FromArgb(
                    50,
                    50,
                    50
                );

            lblPassword.Font =
                new Font(
                    "Tahoma",
                    10,
                    FontStyle.Bold
                );

            lblPassword.AutoSize =
                true;

            lblPassword.Location =
                new Point(
                    45,
                    215
                );

            lblPassword.RightToLeft =
                RightToLeft.Yes;

            loginPanel.Controls.Add(
                lblPassword
            );

            txtPassword =
                new TextBox();

            txtPassword.Font =
                new Font(
                    "Tahoma",
                    11
                );

            txtPassword.Size =
                new Size(
                    305,
                    34
                );

            txtPassword.Location =
                new Point(
                    45,
                    243
                );

            txtPassword.PasswordChar =
                '●';

            txtPassword.RightToLeft =
                RightToLeft.Yes;

            loginPanel.Controls.Add(
                txtPassword
            );

            // =================================================
            // زر إظهار كلمة المرور
            // =================================================

            btnShowPassword =
                new Button();

            btnShowPassword.Text =
                "👁";

            btnShowPassword.Font =
                new Font(
                    "Segoe UI Emoji",
                    10
                );

            btnShowPassword.Size =
                new Size(
                    40,
                    34
                );

            btnShowPassword.Location =
                new Point(
                    355,
                    243
                );

            btnShowPassword.BackColor =
                Color.FromArgb(
                    225,
                    230,
                    235
                );

            btnShowPassword.FlatStyle =
                FlatStyle.Flat;

            btnShowPassword.FlatAppearance.BorderSize =
                0;

            btnShowPassword.Cursor =
                Cursors.Hand;

            btnShowPassword.Click +=
                BtnShowPassword_Click;

            loginPanel.Controls.Add(
                btnShowPassword
            );

            // =================================================
            // نسيت كلمة المرور
            // =================================================

            lnkForgotPassword =
                new LinkLabel();

            lnkForgotPassword.Text =
                "نسيت كلمة المرور؟";

            lnkForgotPassword.Font =
                new Font(
                    "Tahoma",
                    9,
                    FontStyle.Bold
                );

            lnkForgotPassword.LinkColor =
                Color.FromArgb(
                    18,
                    65,
                    105
                );

            lnkForgotPassword.ActiveLinkColor =
                Color.FromArgb(
                    30,
                    100,
                    150
                );

            lnkForgotPassword.AutoSize =
                true;

            lnkForgotPassword.Location =
                new Point(
                    45,
                    295
                );

            lnkForgotPassword.Cursor =
                Cursors.Hand;

            lnkForgotPassword.RightToLeft =
                RightToLeft.Yes;

            lnkForgotPassword.Click +=
                ForgotPassword_Click;

            loginPanel.Controls.Add(
                lnkForgotPassword
            );

            // =================================================
            // رسالة الخطأ
            // =================================================

            lblMessage =
                new Label();

            lblMessage.Text =
                "";

            lblMessage.ForeColor =
                Color.Red;

            lblMessage.Font =
                new Font(
                    "Tahoma",
                    9
                );

            lblMessage.AutoSize =
                true;

            lblMessage.Location =
                new Point(
                    45,
                    325
                );

            lblMessage.RightToLeft =
                RightToLeft.Yes;

            loginPanel.Controls.Add(
                lblMessage
            );

            // =================================================
            // زر الدخول
            // =================================================

            btnLogin =
                new Button();

            btnLogin.Text =
                "دخول";

            btnLogin.Font =
                new Font(
                    "Tahoma",
                    12,
                    FontStyle.Bold
                );

            btnLogin.ForeColor =
                Color.White;

            btnLogin.BackColor =
                Color.FromArgb(
                    18,
                    65,
                    105
                );

            btnLogin.FlatStyle =
                FlatStyle.Flat;

            btnLogin.FlatAppearance.BorderSize =
                0;

            btnLogin.Size =
                new Size(
                    300,
                    46
                );

            btnLogin.Location =
                new Point(
                    45,
                    370
                );

            btnLogin.Cursor =
                Cursors.Hand;

            btnLogin.Click +=
                BtnLogin_Click;

            loginPanel.Controls.Add(
                btnLogin
            );

            AcceptButton =
                btnLogin;

            txtUsername.Focus();
        }

        private void BtnShowPassword_Click(
            object sender,
            EventArgs e)
        {
            if (txtPassword.PasswordChar == '\0')
            {
                txtPassword.PasswordChar =
                    '●';

                btnShowPassword.Text =
                    "👁";
            }
            else
            {
                txtPassword.PasswordChar =
                    '\0';

                btnShowPassword.Text =
                    "🙈";
            }

            txtPassword.Focus();
        }

        private void BtnLogin_Click(
            object sender,
            EventArgs e)
        {
            string username =
                txtUsername.Text.Trim();

            string password =
                txtPassword.Text;

            if (string.IsNullOrWhiteSpace(username))
            {
                lblMessage.ForeColor =
                    Color.Red;

                lblMessage.Text =
                    "أدخل اسم المستخدم";

                txtUsername.Focus();

                return;
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                lblMessage.ForeColor =
                    Color.Red;

                lblMessage.Text =
                    "أدخل كلمة المرور";

                txtPassword.Focus();

                return;
            }

            bool validLogin =
                Database.ValidateLogin(
                    username,
                    password
                );

            if (validLogin)
            {
                lblMessage.Text =
                    "";

                DashboardForm dashboard =
                    new DashboardForm();

                dashboard.FormClosed +=
                    (s, args) =>
                    {
                        Show();

                        txtPassword.Clear();

                        txtUsername.Focus();
                    };

                Hide();

                dashboard.Show();
            }
            else
            {
                lblMessage.ForeColor =
                    Color.Red;

                lblMessage.Text =
                    "اسم المستخدم أو كلمة المرور غير صحيحة";

                txtPassword.Clear();

                txtPassword.Focus();
            }
        }

        private void ForgotPassword_Click(
            object sender,
            EventArgs e)
        {
            ShowRecoveryForm();
        }

        private void ShowRecoveryForm()
        {
            Form recoveryForm =
                new Form();

            recoveryForm.Text =
                "استرداد كلمة المرور";

            recoveryForm.StartPosition =
                FormStartPosition.CenterParent;

            recoveryForm.Size =
                new Size(
                    500,
                    520
                );

            recoveryForm.MinimumSize =
                new Size(
                    500,
                    520
                );

            recoveryForm.MaximumSize =
                new Size(
                    500,
                    520
                );

            recoveryForm.BackColor =
                Color.White;

            recoveryForm.RightToLeft =
                RightToLeft.Yes;

            recoveryForm.RightToLeftLayout =
                true;

            Label title =
                new Label();

            title.Text =
                "استرداد كلمة المرور";

            title.Font =
                new Font(
                    "Tahoma",
                    21,
                    FontStyle.Bold
                );

            title.ForeColor =
                Color.FromArgb(
                    18,
                    38,
                    60
                );

            title.AutoSize =
                true;

            title.Location =
                new Point(
                    145,
                    35
                );

            recoveryForm.Controls.Add(
                title
            );

            Label info =
                new Label();

            info.Text =
                "أدخل بيانات الاسترداد لتعيين كلمة مرور جديدة";

            info.Font =
                new Font(
                    "Tahoma",
                    9
                );

            info.ForeColor =
                Color.Gray;

            info.AutoSize =
                true;

            info.Location =
                new Point(
                    105,
                    78
                );

            recoveryForm.Controls.Add(
                info
            );

            Label usernameLabel =
                new Label();

            usernameLabel.Text =
                "اسم المستخدم";

            usernameLabel.Font =
                new Font(
                    "Tahoma",
                    10,
                    FontStyle.Bold
                );

            usernameLabel.AutoSize =
                true;

            usernameLabel.Location =
                new Point(
                    55,
                    125
                );

            recoveryForm.Controls.Add(
                usernameLabel
            );

            TextBox usernameBox =
                new TextBox();

            usernameBox.Font =
                new Font(
                    "Tahoma",
                    11
                );

            usernameBox.Size =
                new Size(
                    380,
                    34
                );

            usernameBox.Location =
                new Point(
                    55,
                    152
                );

            usernameBox.RightToLeft =
                RightToLeft.Yes;

            recoveryForm.Controls.Add(
                usernameBox
            );

            Label codeLabel =
                new Label();

            codeLabel.Text =
                "كود الاسترداد";

            codeLabel.Font =
                new Font(
                    "Tahoma",
                    10,
                    FontStyle.Bold
                );

            codeLabel.AutoSize =
                true;

            codeLabel.Location =
                new Point(
                    55,
                    205
                );

            recoveryForm.Controls.Add(
                codeLabel
            );

            TextBox codeBox =
                new TextBox();

            codeBox.Font =
                new Font(
                    "Tahoma",
                    11
                );

            codeBox.Size =
                new Size(
                    380,
                    34
                );

            codeBox.Location =
                new Point(
                    55,
                    232
                );

            codeBox.RightToLeft =
                RightToLeft.Yes;

            recoveryForm.Controls.Add(
                codeBox
            );

            Label newPasswordLabel =
                new Label();

            newPasswordLabel.Text =
                "كلمة المرور الجديدة";

            newPasswordLabel.Font =
                new Font(
                    "Tahoma",
                    10,
                    FontStyle.Bold
                );

            newPasswordLabel.AutoSize =
                true;

            newPasswordLabel.Location =
                new Point(
                    55,
                    285
                );

            recoveryForm.Controls.Add(
                newPasswordLabel
            );

            TextBox newPasswordBox =
                new TextBox();

            newPasswordBox.Font =
                new Font(
                    "Tahoma",
                    11
                );

            newPasswordBox.Size =
                new Size(
                    380,
                    34
                );

            newPasswordBox.Location =
                new Point(
                    55,
                    312
                );

            newPasswordBox.PasswordChar =
                '●';

            newPasswordBox.RightToLeft =
                RightToLeft.Yes;

            recoveryForm.Controls.Add(
                newPasswordBox
            );

            Label confirmLabel =
                new Label();

            confirmLabel.Text =
                "تأكيد كلمة المرور";

            confirmLabel.Font =
                new Font(
                    "Tahoma",
                    10,
                    FontStyle.Bold
                );

            confirmLabel.AutoSize =
                true;

            confirmLabel.Location =
                new Point(
                    55,
                    365
                );

            recoveryForm.Controls.Add(
                confirmLabel
            );

            TextBox confirmBox =
                new TextBox();

            confirmBox.Font =
                new Font(
                    "Tahoma",
                    11
                );

            confirmBox.Size =
                new Size(
                    380,
                    34
                );

            confirmBox.Location =
                new Point(
                    55,
                    392
                );

            confirmBox.PasswordChar =
                '●';

            confirmBox.RightToLeft =
                RightToLeft.Yes;

            recoveryForm.Controls.Add(
                confirmBox
            );

            Button resetButton =
                new Button();

            resetButton.Text =
                "تعيين كلمة المرور";

            resetButton.Font =
                new Font(
                    "Tahoma",
                    11,
                    FontStyle.Bold
                );

            resetButton.ForeColor =
                Color.White;

            resetButton.BackColor =
                Color.FromArgb(
                    18,
                    65,
                    105
                );

            resetButton.FlatStyle =
                FlatStyle.Flat;

            resetButton.FlatAppearance.BorderSize =
                0;

            resetButton.Size =
                new Size(
                    180,
                    42
                );

            resetButton.Location =
                new Point(
                    160,
                    440
                );

            resetButton.Cursor =
                Cursors.Hand;

            recoveryForm.Controls.Add(
                resetButton
            );

            resetButton.Click +=
                (sender, e) =>
                {
                    string username =
                        usernameBox.Text.Trim();

                    string recoveryCode =
                        codeBox.Text.Trim();

                    string newPassword =
                        newPasswordBox.Text;

                    string confirmPassword =
                        confirmBox.Text;

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

                    if (string.IsNullOrWhiteSpace(recoveryCode))
                    {
                        MessageBox.Show(
                            "أدخل كود الاسترداد.",
                            "تنبيه",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning
                        );

                        codeBox.Focus();

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

                        newPasswordBox.Focus();

                        return;
                    }

                    if (newPassword.Length < 4)
                    {
                        MessageBox.Show(
                            "كلمة المرور يجب أن تكون 4 أحرف أو أرقام على الأقل.",
                            "تنبيه",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning
                        );

                        newPasswordBox.Focus();

                        return;
                    }

                    if (newPassword != confirmPassword)
                    {
                        MessageBox.Show(
                            "كلمتا المرور غير متطابقتين.",
                            "تنبيه",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning
                        );

                        confirmBox.Clear();

                        confirmBox.Focus();

                        return;
                    }

                    bool changed =
                        Database.ResetPasswordWithRecoveryCode(
                            username,
                            recoveryCode,
                            newPassword
                        );

                    if (changed)
                    {
                        MessageBox.Show(
                            "تم تغيير كلمة المرور بنجاح.\nيمكنك الآن تسجيل الدخول بكلمة المرور الجديدة.",
                            "تم بنجاح",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information
                        );

                        recoveryForm.Close();

                        txtPassword.Clear();

                        txtPassword.Focus();
                    }
                    else
                    {
                        MessageBox.Show(
                            "اسم المستخدم أو كود الاسترداد غير صحيح.",
                            "فشل الاسترداد",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error
                        );

                        codeBox.Clear();

                        codeBox.Focus();
                    }
                };

            recoveryForm.ShowDialog(this);
        }
    }
}
