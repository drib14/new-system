using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using MySqlConnector;
using System.Data;

namespace EventManager
{
    public partial class SubmitProofForm : Form
    {
        private Label lblEmail;
        private TextBox txtEmail;
        private Label lblEvent;
        private ComboBox cmbEvents;
        private Button btnSelectImage;
        private PictureBox picProof;
        private Button btnSubmit;
        private byte[] _imageBytes;

        public SubmitProofForm()
        {
            InitializeComponent();
            LoadEvents();
        }

        private void InitializeComponent()
        {
            this.lblEmail = new Label();
            this.txtEmail = new TextBox();
            this.lblEvent = new Label();
            this.cmbEvents = new ComboBox();
            this.btnSelectImage = new Button();
            this.picProof = new PictureBox();
            this.btnSubmit = new Button();
            ((System.ComponentModel.ISupportInitialize)(this.picProof)).BeginInit();
            this.SuspendLayout();

            this.lblEmail.Location = new Point(20, 20);
            this.lblEmail.Name = "lblEmail";
            this.lblEmail.Text = "Your Registered Email:";
            this.lblEmail.Width = 150;

            this.txtEmail.Location = new Point(180, 17);
            this.txtEmail.Width = 200;

            this.lblEvent.Location = new Point(20, 60);
            this.lblEvent.Name = "lblEvent";
            this.lblEvent.Text = "Select Event:";
            this.lblEvent.Width = 150;

            this.cmbEvents.Location = new Point(180, 57);
            this.cmbEvents.Width = 200;
            this.cmbEvents.DropDownStyle = ComboBoxStyle.DropDownList;

            this.btnSelectImage.Location = new Point(20, 100);
            this.btnSelectImage.Name = "btnSelectImage";
            this.btnSelectImage.Text = "Select Proof Image";
            this.btnSelectImage.Width = 150;
            this.btnSelectImage.Click += new EventHandler(this.BtnSelectImage_Click);

            this.picProof.Location = new Point(20, 140);
            this.picProof.Size = new Size(360, 200);
            this.picProof.SizeMode = PictureBoxSizeMode.Zoom;
            this.picProof.BorderStyle = BorderStyle.FixedSingle;

            this.btnSubmit.Location = new Point(180, 360);
            this.btnSubmit.Name = "btnSubmit";
            this.btnSubmit.Text = "Submit Attendance";
            this.btnSubmit.Width = 200;
            this.btnSubmit.Click += new EventHandler(this.BtnSubmit_Click);

            this.ClientSize = new Size(420, 420);
            this.Controls.Add(lblEmail);
            this.Controls.Add(txtEmail);
            this.Controls.Add(lblEvent);
            this.Controls.Add(cmbEvents);
            this.Controls.Add(btnSelectImage);
            this.Controls.Add(picProof);
            this.Controls.Add(btnSubmit);
            this.Name = "SubmitProofForm";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Submit Attendance Proof";

            ((System.ComponentModel.ISupportInitialize)(this.picProof)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private void LoadEvents()
        {
            try
            {
                DataTable dt = DbHelper.ExecuteQuery("SELECT id, title FROM events ORDER BY event_date DESC");
                cmbEvents.DataSource = dt;
                cmbEvents.DisplayMember = "title";
                cmbEvents.ValueMember = "id";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading events: " + ex.Message);
            }
        }

        private void BtnSelectImage_Click(object? sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    picProof.Image = Image.FromFile(ofd.FileName);
                    _imageBytes = File.ReadAllBytes(ofd.FileName);
                }
            }
        }

        private void BtnSubmit_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtEmail.Text) || cmbEvents.SelectedValue == null || _imageBytes == null)
            {
                MessageBox.Show("Please enter email, select an event, and upload an image.");
                return;
            }

            try
            {
                // First check if attendee exists
                string checkQuery = "SELECT id FROM attendees WHERE email = @email AND event_id = @eventId";
                object attendeeIdObj = DbHelper.ExecuteScalar(checkQuery,
                    new MySqlParameter("@email", txtEmail.Text.Trim()),
                    new MySqlParameter("@eventId", cmbEvents.SelectedValue));

                if (attendeeIdObj == null)
                {
                    MessageBox.Show("Could not find a registration with this email for the selected event.");
                    return;
                }

                int attendeeId = Convert.ToInt32(attendeeIdObj);

                // Insert proof
                string insertQuery = @"INSERT INTO attendance (attendee_id, event_id, proof_image, status)
                                       VALUES (@attId, @eventId, @img, 'Pending')";

                DbHelper.ExecuteNonQuery(insertQuery,
                    new MySqlParameter("@attId", attendeeId),
                    new MySqlParameter("@eventId", cmbEvents.SelectedValue),
                    new MySqlParameter("@img", _imageBytes));

                MessageBox.Show("Proof submitted successfully! It is pending admin approval.");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error submitting proof: " + ex.Message);
            }
        }
    }
}
