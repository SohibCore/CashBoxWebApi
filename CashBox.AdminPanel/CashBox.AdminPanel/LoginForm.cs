using CashBox.AdminPanel.Services;

namespace CashBox.AdminPanel
{
    public partial class LoginForm : Form
    {
        private readonly AuthApiService _authService = new();

        public LoginForm()
        {
            InitializeComponent();
        }

        private async void btnLogin_Click(object sender, EventArgs e)
        {
            lblError.Text = "";

            if (string.IsNullOrWhiteSpace(UserName.Text) || string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                lblError.Text = "Login va parolni kiriting.";
                return;
            }

            btnLogin.Enabled = false;
            var (success, message) = await _authService.LoginAsync(UserName.Text.Trim(), txtPassword.Text);
            btnLogin.Enabled = true;

            if (!success)
            {
                lblError.Text = message;
                return;
            }

            var mainForm = new MainForm();
            mainForm.Show();
            this.Hide();
        }

        private void InitializeComponent()
        {
            UserName = new TextBox();
            btnLogin = new Button();
            txtPassword = new TextBox();
            lblError = new Label();
            label1 = new Label();
            label2 = new Label();
            checkBox1 = new CheckBox();
            SuspendLayout();
            // 
            // UserName
            // 
            UserName.Location = new Point(211, 253);
            UserName.Multiline = true;
            UserName.Name = "UserName";
            UserName.Size = new Size(203, 27);
            UserName.TabIndex = 0;
            UserName.Tag = "";
            UserName.TextAlign = HorizontalAlignment.Center;
            UserName.UseSystemPasswordChar = true;
            UserName.TextChanged += UserName_TextChanged;
            // 
            // btnLogin
            // 
            btnLogin.BackColor = Color.DarkTurquoise;
            btnLogin.ForeColor = SystemColors.ActiveCaptionText;
            btnLogin.Location = new Point(211, 401);
            btnLogin.Name = "btnLogin";
            btnLogin.Size = new Size(203, 26);
            btnLogin.TabIndex = 1;
            btnLogin.Text = "Kirish";
            btnLogin.UseVisualStyleBackColor = false;
            btnLogin.Click += btnLogin_Click_1;
            // 
            // txtPassword
            // 
            txtPassword.Location = new Point(211, 319);
            txtPassword.Multiline = true;
            txtPassword.Name = "txtPassword";
            txtPassword.Size = new Size(203, 27);
            txtPassword.TabIndex = 2;
            txtPassword.TextAlign = HorizontalAlignment.Center;
            // 
            // lblError
            // 
            lblError.AutoSize = true;
            lblError.ForeColor = Color.Red;
            lblError.Location = new Point(153, 33);
            lblError.Name = "lblError";
            lblError.Size = new Size(0, 20);
            lblError.TabIndex = 3;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = Color.Transparent;
            label1.ForeColor = SystemColors.ActiveCaptionText;
            label1.Location = new Point(211, 230);
            label1.Name = "label1";
            label1.Size = new Size(78, 20);
            label1.TabIndex = 4;
            label1.Text = "UserName";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.BackColor = Color.Transparent;
            label2.ForeColor = Color.Black;
            label2.Location = new Point(211, 296);
            label2.Name = "label2";
            label2.Size = new Size(70, 20);
            label2.TabIndex = 5;
            label2.Text = "Password";
            // 
            // checkBox1
            // 
            checkBox1.AutoSize = true;
            checkBox1.BackColor = Color.Transparent;
            checkBox1.Location = new Point(244, 352);
            checkBox1.Name = "checkBox1";
            checkBox1.Size = new Size(132, 24);
            checkBox1.TabIndex = 6;
            checkBox1.Text = "Show Password";
            checkBox1.UseVisualStyleBackColor = false;
            checkBox1.CheckedChanged += checkBox1_CheckedChanged;
            // 
            // LoginForm
            // 
            AcceptButton = btnLogin;
            BackColor = Color.CornflowerBlue;
            BackgroundImage = Properties.Resources.Screenshot_2026_06_12_122919;
            BackgroundImageLayout = ImageLayout.Stretch;
            ClientSize = new Size(997, 544);
            Controls.Add(checkBox1);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(lblError);
            Controls.Add(txtPassword);
            Controls.Add(btnLogin);
            Controls.Add(UserName);
            Cursor = Cursors.Help;
            DoubleBuffered = true;
            Name = "LoginForm";
            SizeGripStyle = SizeGripStyle.Show;
            TopMost = true;
            Load += txtUserName_Load;
            ResumeLayout(false);
            PerformLayout();

        }
        private TextBox UserName;
        private Button btnLogin;
        private TextBox txtPassword;
        private Label label1;
        private Label label2;
        private CheckBox checkBox1;
        private Label lblError;

        private void txtUserName_Load(object sender, EventArgs e)
        {

        }

        private async void btnLogin_Click_1(object sender, EventArgs e)
        {
            lblError.Text = "";

            if (string.IsNullOrWhiteSpace(UserName.Text) || string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                lblError.Text = "Login va parolni kiriting.";
                return;
            }

            btnLogin.Enabled = false;
            var (success, message) = await _authService.LoginAsync(UserName.Text.Trim(), txtPassword.Text);
            btnLogin.Enabled = true;

            if (!success)
            {
                lblError.Text = message;
                return;
            }

            var mainForm = new MainForm();
            mainForm.Show();
            this.Hide();
        }

        private void UserName_TextChanged(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
