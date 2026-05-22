using System;
using System.Drawing;
using System.Windows.Forms;

namespace EventManager
{
    public partial class MainForm : Form
    {
        private Button btnPublicPortal;
        private Button btnAdminLogin;
        private Label lblTitle;

        public MainForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.btnPublicPortal = new Button();
            this.btnAdminLogin = new Button();
            this.lblTitle = new Label();
            this.SuspendLayout();

            // lblTitle
            this.lblTitle.AutoSize = true;
            this.lblTitle.Location = new Point(50, 40);
            this.lblTitle.Name = "lblTitleMain";
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Event Management System";
            this.lblTitle.TextAlign = ContentAlignment.MiddleLeft;

            // btnPublicPortal
            this.btnPublicPortal.Location = new Point(50, 150);
            this.btnPublicPortal.Name = "btnPublicPortal";
            this.btnPublicPortal.Size = new Size(300, 60);
            this.btnPublicPortal.TabIndex = 1;
            this.btnPublicPortal.Text = "Public Portal (Attendees)";
            this.btnPublicPortal.UseVisualStyleBackColor = true;
            this.btnPublicPortal.Click += new EventHandler(this.BtnPublicPortal_Click);

            // btnAdminLogin
            this.btnAdminLogin.Location = new Point(370, 150);
            this.btnAdminLogin.Name = "btnAdminLogin";
            this.btnAdminLogin.Size = new Size(300, 60);
            this.btnAdminLogin.TabIndex = 2;
            this.btnAdminLogin.Text = "Admin Login (Organizers)";
            this.btnAdminLogin.UseVisualStyleBackColor = true;
            this.btnAdminLogin.Click += new EventHandler(this.BtnAdminLogin_Click);

            // MainForm
            this.ClientSize = new Size(800, 450);
            this.Controls.Add(this.btnAdminLogin);
            this.Controls.Add(this.btnPublicPortal);
            this.Controls.Add(this.lblTitle);
            this.Name = "MainForm";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Event Management System";
            this.Resize += new EventHandler(this.MainForm_Resize);
            this.ResumeLayout(false);
            this.PerformLayout();

            Theme.ApplyToForm(this);
        }

        private void MainForm_Resize(object? sender, EventArgs e)
        {
            int totalWidth = btnPublicPortal.Width + 20 + btnAdminLogin.Width;
            int startX = (this.ClientSize.Width - totalWidth) / 2;

            btnPublicPortal.Left = startX;
            btnAdminLogin.Left = startX + btnPublicPortal.Width + 20;

            lblTitle.Left = (this.ClientSize.Width - lblTitle.Width) / 2;
        }

        private void BtnPublicPortal_Click(object? sender, EventArgs e)
        {
            PublicPortalForm publicPortal = new PublicPortalForm();
            publicPortal.Show();
        }

        private void BtnAdminLogin_Click(object? sender, EventArgs e)
        {
            AdminLoginForm loginForm = new AdminLoginForm();
            loginForm.Show();
        }
    }
}
