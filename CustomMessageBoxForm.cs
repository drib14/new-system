using System;
using System.Drawing;
using System.Windows.Forms;

namespace EventManager
{
    public partial class CustomMessageBoxForm : Form
    {
        private Label lblMessage;
        private Button btnOk;

        public CustomMessageBoxForm(string message, string title = "Notification", bool isError = false)
        {
            InitializeComponent();
            this.Text = title;
            lblMessage.Text = message;

            if (isError)
            {
                btnOk.BackColor = Theme.AccentDanger;
            }
        }

        private void InitializeComponent()
        {
            this.lblMessage = new Label();
            this.btnOk = new Button();
            this.SuspendLayout();

            //
            // lblMessage
            //
            this.lblMessage.AutoSize = false;
            this.lblMessage.Location = new Point(20, 20);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new Size(360, 80);
            this.lblMessage.TabIndex = 0;
            this.lblMessage.Text = "Message goes here.";
            this.lblMessage.TextAlign = ContentAlignment.MiddleCenter;

            //
            // btnOk
            //
            this.btnOk.Location = new Point(150, 110);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new Size(100, 40);
            this.btnOk.TabIndex = 1;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new EventHandler(this.BtnOk_Click);

            //
            // CustomMessageBoxForm
            //
            this.ClientSize = new Size(400, 170);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.lblMessage);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CustomMessageBoxForm";
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Notification";
            this.ResumeLayout(false);

            Theme.ApplyToForm(this);
            // Re-apply button styling in case Theme overridden it
            this.btnOk.BackColor = Theme.PrimaryColor;
            this.btnOk.ForeColor = Color.White;
        }

        private void BtnOk_Click(object? sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        public static DialogResult Show(string message, string title = "Notification", bool isError = false)
        {
            using (CustomMessageBoxForm form = new CustomMessageBoxForm(message, title, isError))
            {
                if (isError) form.btnOk.BackColor = Theme.AccentDanger;
                else form.btnOk.BackColor = Theme.PrimaryColor;

                return form.ShowDialog();
            }
        }
    }
}
