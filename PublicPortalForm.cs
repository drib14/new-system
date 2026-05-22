using System;
using System.Drawing;
using System.Windows.Forms;
using System.Data;

namespace EventManager
{
    public partial class PublicPortalForm : Form
    {
        private DataGridView dgvEvents;
        private Label lblTitle;
        private Button btnRegister;
        private Button btnSubmitProof;

        public PublicPortalForm()
        {
            InitializeComponent();
            LoadEvents();
        }

        private void InitializeComponent()
        {
            this.dgvEvents = new DataGridView();
            this.lblTitle = new Label();
            this.btnRegister = new Button();
            this.btnSubmitProof = new Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvEvents)).BeginInit();
            this.SuspendLayout();

            // lblTitle
            this.lblTitle.AutoSize = true;
            this.lblTitle.Location = new Point(20, 20);
            this.lblTitle.Name = "lblTitleEvents";
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Upcoming Events";

            // dgvEvents
            this.dgvEvents.AllowUserToAddRows = false;
            this.dgvEvents.AllowUserToDeleteRows = false;
            this.dgvEvents.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvEvents.Location = new Point(20, 70);
            this.dgvEvents.Name = "dgvEvents";
            this.dgvEvents.ReadOnly = true;
            this.dgvEvents.RowHeadersWidth = 51;
            this.dgvEvents.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dgvEvents.Size = new Size(740, 250);
            this.dgvEvents.TabIndex = 1;

            // btnRegister
            this.btnRegister.Location = new Point(20, 340);
            this.btnRegister.Name = "btnRegister";
            this.btnRegister.Size = new Size(180, 40);
            this.btnRegister.TabIndex = 2;
            this.btnRegister.Text = "Register for Event";
            this.btnRegister.UseVisualStyleBackColor = true;
            this.btnRegister.Click += new EventHandler(this.BtnRegister_Click);

            // btnSubmitProof
            this.btnSubmitProof.Location = new Point(220, 340);
            this.btnSubmitProof.Name = "btnSubmitProof";
            this.btnSubmitProof.Size = new Size(200, 40);
            this.btnSubmitProof.TabIndex = 3;
            this.btnSubmitProof.Text = "Submit Proof";
            this.btnSubmitProof.UseVisualStyleBackColor = true;
            this.btnSubmitProof.Click += new EventHandler(this.BtnSubmitProof_Click);

            // PublicPortalForm
            this.ClientSize = new Size(782, 403);
            this.Controls.Add(this.btnSubmitProof);
            this.Controls.Add(this.btnRegister);
            this.Controls.Add(this.dgvEvents);
            this.Controls.Add(this.lblTitle);
            this.Name = "PublicPortalForm";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Public Portal";
            ((System.ComponentModel.ISupportInitialize)(this.dgvEvents)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

            Theme.ApplyToForm(this);
        }

        private void LoadEvents()
        {
            try
            {
                string query = "SELECT id, title, event_date, event_time, location, description FROM events WHERE event_date >= CURDATE() ORDER BY event_date ASC";
                DataTable dt = DbHelper.ExecuteQuery(query);
                dgvEvents.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading events: " + ex.Message, "Error");
            }
        }

        private void BtnRegister_Click(object? sender, EventArgs e)
        {
            if (dgvEvents.SelectedRows.Count > 0)
            {
                int eventId = Convert.ToInt32(dgvEvents.SelectedRows[0].Cells["id"].Value);
                string eventTitle = dgvEvents.SelectedRows[0].Cells["title"].Value?.ToString() ?? "";

                RegistrationForm regForm = new RegistrationForm(eventId, eventTitle);
                regForm.ShowDialog();
            }
            else
            {
                MessageBox.Show("Please select an event from the list to register.");
            }
        }

        private void BtnSubmitProof_Click(object? sender, EventArgs e)
        {
            SubmitProofForm proofForm = new SubmitProofForm();
            proofForm.ShowDialog();
        }
    }
}
