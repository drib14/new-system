using System;
using System.Drawing;
using System.Windows.Forms;
using MySqlConnector;
using QRCoder;

namespace EventManager
{
    public partial class RegistrationForm : Form
    {
        private int _eventId;
        private string _eventTitle;

        private Label lblTitle;
        private Label lblFirstName;
        private TextBox txtFirstName;
        private Label lblLastName;
        private TextBox txtLastName;
        private Label lblEmail;
        private TextBox txtEmail;
        private Label lblAge;
        private NumericUpDown numAge;
        private Label lblPhone;
        private TextBox txtPhone;
        private Button btnRegister;
        private PictureBox picQRCode;
        private Label lblQRInstruction;

        public RegistrationForm(int eventId, string eventTitle)
        {
            _eventId = eventId;
            _eventTitle = eventTitle;
            InitializeComponent();
            lblTitle.Text = $"Register for: {_eventTitle}";
        }

        private void InitializeComponent()
        {
            this.lblTitle = new Label();
            this.lblFirstName = new Label();
            this.txtFirstName = new TextBox();
            this.lblLastName = new Label();
            this.txtLastName = new TextBox();
            this.lblEmail = new Label();
            this.txtEmail = new TextBox();
            this.lblAge = new Label();
            this.numAge = new NumericUpDown();
            this.lblPhone = new Label();
            this.txtPhone = new TextBox();
            this.btnRegister = new Button();
            this.picQRCode = new PictureBox();
            this.lblQRInstruction = new Label();
            ((System.ComponentModel.ISupportInitialize)(this.numAge)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picQRCode)).BeginInit();
            this.SuspendLayout();

            int startY = 60;
            int gap = 40;

            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
            this.lblTitle.Location = new Point(20, 15);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new Size(130, 28);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Register for Event";

            this.lblFirstName.Location = new Point(20, startY);
            this.lblFirstName.Name = "lblFirstName";
            this.lblFirstName.Text = "First Name:";
            this.txtFirstName.Location = new Point(120, startY - 3);
            this.txtFirstName.Width = 200;

            this.lblLastName.Location = new Point(20, startY + gap);
            this.lblLastName.Name = "lblLastName";
            this.lblLastName.Text = "Last Name:";
            this.txtLastName.Location = new Point(120, startY + gap - 3);
            this.txtLastName.Width = 200;

            this.lblEmail.Location = new Point(20, startY + 2 * gap);
            this.lblEmail.Name = "lblEmail";
            this.lblEmail.Text = "Email:";
            this.txtEmail.Location = new Point(120, startY + 2 * gap - 3);
            this.txtEmail.Width = 200;

            this.lblAge.Location = new Point(20, startY + 3 * gap);
            this.lblAge.Name = "lblAge";
            this.lblAge.Text = "Age:";
            this.numAge.Location = new Point(120, startY + 3 * gap - 3);
            this.numAge.Width = 80;

            this.lblPhone.Location = new Point(20, startY + 4 * gap);
            this.lblPhone.Name = "lblPhone";
            this.lblPhone.Text = "Phone:";
            this.txtPhone.Location = new Point(120, startY + 4 * gap - 3);
            this.txtPhone.Width = 200;

            this.btnRegister.Location = new Point(120, startY + 5 * gap);
            this.btnRegister.Name = "btnRegister";
            this.btnRegister.Text = "Register";
            this.btnRegister.Width = 100;
            this.btnRegister.Click += new EventHandler(this.BtnRegister_Click);

            this.picQRCode.Location = new Point(360, startY);
            this.picQRCode.Size = new Size(200, 200);
            this.picQRCode.SizeMode = PictureBoxSizeMode.Zoom;
            this.picQRCode.BorderStyle = BorderStyle.FixedSingle;

            this.lblQRInstruction.Location = new Point(360, startY + 210);
            this.lblQRInstruction.Size = new Size(200, 50);
            this.lblQRInstruction.Text = "Your QR Ticket will appear here. Please save it.";
            this.lblQRInstruction.TextAlign = ContentAlignment.TopCenter;

            this.ClientSize = new Size(600, 350);
            this.Controls.Add(lblTitle);
            this.Controls.Add(lblFirstName);
            this.Controls.Add(txtFirstName);
            this.Controls.Add(lblLastName);
            this.Controls.Add(txtLastName);
            this.Controls.Add(lblEmail);
            this.Controls.Add(txtEmail);
            this.Controls.Add(lblAge);
            this.Controls.Add(numAge);
            this.Controls.Add(lblPhone);
            this.Controls.Add(txtPhone);
            this.Controls.Add(btnRegister);
            this.Controls.Add(picQRCode);
            this.Controls.Add(lblQRInstruction);
            this.Name = "RegistrationForm";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Event Registration";

            ((System.ComponentModel.ISupportInitialize)(this.numAge)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picQRCode)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

            Theme.ApplyToForm(this);
        }

        private void BtnRegister_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtFirstName.Text) || string.IsNullOrWhiteSpace(txtLastName.Text) || string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                MessageBox.Show("Please fill in all required fields (Name, Email).");
                return;
            }

            string qrText = $"EVENT:{_eventId}|EMAIL:{txtEmail.Text.Trim()}|TS:{DateTime.Now.Ticks}";

            try
            {
                string query = @"INSERT INTO attendees (event_id, first_name, last_name, email, age, phone, qr_code_text)
                                 VALUES (@eventId, @fName, @lName, @email, @age, @phone, @qrText)";

                MySqlParameter[] parameters = {
                    new MySqlParameter("@eventId", _eventId),
                    new MySqlParameter("@fName", txtFirstName.Text.Trim()),
                    new MySqlParameter("@lName", txtLastName.Text.Trim()),
                    new MySqlParameter("@email", txtEmail.Text.Trim()),
                    new MySqlParameter("@age", numAge.Value),
                    new MySqlParameter("@phone", txtPhone.Text.Trim()),
                    new MySqlParameter("@qrText", qrText)
                };

                DbHelper.ExecuteNonQuery(query, parameters);

                // Generate QR Code
                QRCodeGenerator qrGenerator = new QRCodeGenerator();
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(qrText, QRCodeGenerator.ECCLevel.Q);
                QRCode qrCode = new QRCode(qrCodeData);
                Bitmap qrCodeImage = qrCode.GetGraphic(20);

                picQRCode.Image = qrCodeImage;
                btnRegister.Enabled = false;

                MessageBox.Show("Registration successful! Please save your QR Code.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error registering: " + ex.Message, "Error");
            }
        }
    }
}
