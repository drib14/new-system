using System;
using System.Drawing;
using System.Windows.Forms;
using MySqlConnector;
using System.Data;
using System.Security.Cryptography;
using System.Text;

namespace EventManager
{
    public partial class AdminLoginForm : Form
    {
        private TextBox txtUsername;
        private TextBox txtPassword;
        private Button btnLogin;
        private Label lblUsername;
        private Label lblPassword;
        private Label lblTitle;

        public AdminLoginForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.txtUsername = new TextBox();
            this.txtPassword = new TextBox();
            this.btnLogin = new Button();
            this.lblUsername = new Label();
            this.lblPassword = new Label();
            this.lblTitle = new Label();
            this.SuspendLayout();

            // lblTitle
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new Font("Segoe UI", 16F, FontStyle.Bold, GraphicsUnit.Point);
            this.lblTitle.Location = new Point(110, 30);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new Size(181, 37);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Admin Login";

            // lblUsername
            this.lblUsername.AutoSize = true;
            this.lblUsername.Location = new Point(50, 100);
            this.lblUsername.Name = "lblUsername";
            this.lblUsername.Size = new Size(78, 20);
            this.lblUsername.TabIndex = 1;
            this.lblUsername.Text = "Username:";

            // txtUsername
            this.txtUsername.Location = new Point(140, 97);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new Size(200, 27);
            this.txtUsername.TabIndex = 2;

            // lblPassword
            this.lblPassword.AutoSize = true;
            this.lblPassword.Location = new Point(50, 150);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new Size(73, 20);
            this.lblPassword.TabIndex = 3;
            this.lblPassword.Text = "Password:";

            // txtPassword
            this.txtPassword.Location = new Point(140, 147);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new Size(200, 27);
            this.txtPassword.TabIndex = 4;

            // btnLogin
            this.btnLogin.Location = new Point(140, 200);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new Size(100, 35);
            this.btnLogin.TabIndex = 5;
            this.btnLogin.Text = "Login";
            this.btnLogin.UseVisualStyleBackColor = true;
            this.btnLogin.Click += new EventHandler(this.BtnLogin_Click);

            // AdminLoginForm
            this.ClientSize = new Size(400, 300);
            this.Controls.Add(this.btnLogin);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.lblPassword);
            this.Controls.Add(this.txtUsername);
            this.Controls.Add(this.lblUsername);
            this.Controls.Add(this.lblTitle);
            this.Name = "AdminLoginForm";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Admin Login";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private void BtnLogin_Click(object? sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim(); // Using plain text for prototype simplicity

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please enter both username and password.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string hash = password;
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                hash = builder.ToString();
            }

            try
            {
                string query = "SELECT COUNT(*) FROM admins WHERE username = @user AND (password_hash = @pass OR password_hash = @hash)";
                MySqlParameter[] parameters = {
                    new MySqlParameter("@user", username),
                    new MySqlParameter("@pass", password),
                    new MySqlParameter("@hash", hash)
                };

                int count = Convert.ToInt32(DbHelper.ExecuteScalar(query, parameters));

                if (count > 0)
                {
                    MessageBox.Show("Login successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    AdminDashboardForm dashboard = new AdminDashboardForm();
                    dashboard.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Invalid username or password.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Database error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
