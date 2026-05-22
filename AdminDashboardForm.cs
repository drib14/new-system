using System;
using System.Drawing;
using System.Windows.Forms;
using System.Data;
using MySqlConnector;
using System.IO;
using System.Windows.Forms.DataVisualization.Charting;

namespace EventManager
{
    public partial class AdminDashboardForm : Form
    {
        private TabControl tabControl;
        private TabPage tabEvents;
        private TabPage tabRegistrations;
        private TabPage tabAttendance;
        private TabPage tabStaff;
        private DataGridView dgvStaff;
        private TextBox txtStaffUser, txtStaffPass;
        private Button btnAddStaff;

        // Events Tab
        private DataGridView dgvEventsAdmin;
        private TextBox txtTitle, txtLocation, txtDescription;
        private DateTimePicker dtpDate;
        private DateTimePicker dtpTime;
        private Button btnCreateEvent, btnUpdateEvent, btnDeleteEvent;
        private int _selectedEventId = -1;

        // Registrations Tab
        private DataGridView dgvRegistrations;
        private ComboBox cmbRegEvents;
        private TextBox txtRegFName, txtRegLName, txtRegEmail, txtRegPhone;
        private NumericUpDown numRegAge;
        private Button btnUpdateReg, btnDeleteReg;
        private int _selectedRegId = -1;

        // Staff Tab extras
        private Button btnUpdateStaff, btnDeleteStaff;
        private int _selectedStaffId = -1;

        // Attendance Tab
        private DataGridView dgvAttendance;
        private ComboBox cmbAttEvents;
        private PictureBox picProofViewer;
        private Button btnApprove;
        private Button btnReject;

        public AdminDashboardForm()
        {
            InitializeComponent();
            LoadEventsData();
            LoadStaff();
        }

        private void InitializeComponent()
        {
            this.tabControl = new TabControl();
            this.tabEvents = new TabPage();
            this.tabRegistrations = new TabPage();
            this.tabAttendance = new TabPage();
            this.tabStaff = new TabPage();

            this.tabControl.Controls.Add(this.tabEvents);
            this.tabControl.Controls.Add(this.tabRegistrations);
            this.tabControl.Controls.Add(this.tabAttendance);
            this.tabControl.Controls.Add(this.tabStaff);
            this.tabControl.Dock = DockStyle.Fill;
            this.tabControl.DrawMode = TabDrawMode.OwnerDrawFixed;
            this.tabControl.Padding = new Point(15, 10);
            this.tabControl.DrawItem += TabControl_DrawItem;

            // --- Events Tab Setup ---
            this.tabEvents.Text = "Manage Events";
            Label l1 = new Label() { Text = "Title:", Location = new Point(10, 20), Width = 100 };
            txtTitle = new TextBox() { Location = new Point(110, 17), Width = 200 };
            Label l2 = new Label() { Text = "Date:", Location = new Point(10, 60), Width = 100 };
            dtpDate = new DateTimePicker() { Location = new Point(110, 57), Width = 200, Format = DateTimePickerFormat.Short };
            Label l3 = new Label() { Text = "Time:", Location = new Point(10, 100), Width = 100 };
            dtpTime = new DateTimePicker() { Location = new Point(110, 97), Width = 200, Format = DateTimePickerFormat.Time, ShowUpDown = true };
            Label l4 = new Label() { Text = "Location:", Location = new Point(10, 140), Width = 100 };
            txtLocation = new TextBox() { Location = new Point(110, 137), Width = 200 };
            Label l5 = new Label() { Text = "Desc:", Location = new Point(10, 180), Width = 100 };
            txtDescription = new TextBox() { Location = new Point(110, 177), Width = 200, Multiline = true, Height = 60 };

            btnCreateEvent = new Button() { Text = "Create", Location = new Point(20, 250), Width = 90 };
            btnCreateEvent.Click += BtnCreateEvent_Click;
            btnUpdateEvent = new Button() { Text = "Update", Location = new Point(120, 250), Width = 90 };
            btnUpdateEvent.Click += BtnUpdateEvent_Click;
            btnDeleteEvent = new Button() { Text = "Delete", Location = new Point(220, 250), Width = 90 };
            btnDeleteEvent.Click += BtnDeleteEvent_Click;

            dgvEventsAdmin = new DataGridView() { Location = new Point(330, 20), Width = 600, Height = 450, ReadOnly = true, AllowUserToAddRows = false, SelectionMode = DataGridViewSelectionMode.FullRowSelect };
            dgvEventsAdmin.SelectionChanged += DgvEventsAdmin_SelectionChanged;

            tabEvents.Controls.Add(l1); tabEvents.Controls.Add(txtTitle);
            tabEvents.Controls.Add(l2); tabEvents.Controls.Add(dtpDate);
            tabEvents.Controls.Add(l3); tabEvents.Controls.Add(dtpTime);
            tabEvents.Controls.Add(l4); tabEvents.Controls.Add(txtLocation);
            tabEvents.Controls.Add(l5); tabEvents.Controls.Add(txtDescription);
            tabEvents.Controls.Add(btnCreateEvent); tabEvents.Controls.Add(btnUpdateEvent); tabEvents.Controls.Add(btnDeleteEvent);
            tabEvents.Controls.Add(dgvEventsAdmin);

            // --- Registrations Tab Setup ---
            tabRegistrations.Text = "Registrations";
            Label lr = new Label() { Text = "Select Event:", Location = new Point(10, 20), Width = 100 };
            cmbRegEvents = new ComboBox() { Location = new Point(110, 17), Width = 200, DropDownStyle = ComboBoxStyle.DropDownList };
            cmbRegEvents.SelectedIndexChanged += (s, e) => LoadRegistrations();

            Label rf = new Label() { Text = "First Name:", Location = new Point(330, 20), Width = 80 };
            txtRegFName = new TextBox() { Location = new Point(410, 17), Width = 120 };
            Label rl = new Label() { Text = "Last Name:", Location = new Point(540, 20), Width = 80 };
            txtRegLName = new TextBox() { Location = new Point(620, 17), Width = 120 };
            Label re = new Label() { Text = "Email:", Location = new Point(330, 60), Width = 80 };
            txtRegEmail = new TextBox() { Location = new Point(410, 57), Width = 120 };
            Label rp = new Label() { Text = "Phone:", Location = new Point(540, 60), Width = 80 };
            txtRegPhone = new TextBox() { Location = new Point(620, 57), Width = 120 };
            Label ra = new Label() { Text = "Age:", Location = new Point(760, 20), Width = 40 };
            numRegAge = new NumericUpDown() { Location = new Point(800, 17), Width = 60 };

            btnUpdateReg = new Button() { Text = "Update", Location = new Point(750, 55), Width = 80 };
            btnUpdateReg.Click += BtnUpdateReg_Click;
            btnDeleteReg = new Button() { Text = "Delete", Location = new Point(840, 55), Width = 80 };
            btnDeleteReg.Click += BtnDeleteReg_Click;

            dgvRegistrations = new DataGridView() { Location = new Point(10, 100), Width = 920, Height = 360, ReadOnly = true, AllowUserToAddRows = false, SelectionMode = DataGridViewSelectionMode.FullRowSelect };
            dgvRegistrations.SelectionChanged += DgvRegistrations_SelectionChanged;

            tabRegistrations.Controls.Add(lr); tabRegistrations.Controls.Add(cmbRegEvents);
            tabRegistrations.Controls.Add(rf); tabRegistrations.Controls.Add(txtRegFName);
            tabRegistrations.Controls.Add(rl); tabRegistrations.Controls.Add(txtRegLName);
            tabRegistrations.Controls.Add(re); tabRegistrations.Controls.Add(txtRegEmail);
            tabRegistrations.Controls.Add(rp); tabRegistrations.Controls.Add(txtRegPhone);
            tabRegistrations.Controls.Add(ra); tabRegistrations.Controls.Add(numRegAge);
            tabRegistrations.Controls.Add(btnUpdateReg); tabRegistrations.Controls.Add(btnDeleteReg);
            tabRegistrations.Controls.Add(dgvRegistrations);

            // --- Attendance Tab Setup ---
            tabAttendance.Text = "Approve Attendance";
            Label la = new Label() { Text = "Select Event:", Location = new Point(10, 20), Width = 100 };
            cmbAttEvents = new ComboBox() { Location = new Point(110, 17), Width = 200, DropDownStyle = ComboBoxStyle.DropDownList };
            cmbAttEvents.SelectedIndexChanged += (s, e) => LoadAttendance();
            dgvAttendance = new DataGridView() { Location = new Point(10, 60), Width = 500, Height = 400, ReadOnly = true, AllowUserToAddRows = false, SelectionMode = DataGridViewSelectionMode.FullRowSelect };
            dgvAttendance.SelectionChanged += DgvAttendance_SelectionChanged;

            picProofViewer = new PictureBox() { Location = new Point(530, 60), Width = 400, Height = 350, SizeMode = PictureBoxSizeMode.Zoom, BorderStyle = BorderStyle.FixedSingle };
            btnApprove = new Button() { Text = "Approve", Location = new Point(530, 420), Width = 100 };
            btnApprove.Click += (s, e) => UpdateAttendanceStatus("Approved");
            btnReject = new Button() { Text = "Reject", Location = new Point(650, 420), Width = 100 };
            btnReject.Click += (s, e) => UpdateAttendanceStatus("Rejected");

            tabAttendance.Controls.Add(la); tabAttendance.Controls.Add(cmbAttEvents); tabAttendance.Controls.Add(dgvAttendance);
            tabAttendance.Controls.Add(picProofViewer); tabAttendance.Controls.Add(btnApprove); tabAttendance.Controls.Add(btnReject);

            // --- Staff Tab Setup ---
            tabStaff.Text = "Manage Staff";
            Label ls1 = new Label() { Text = "Username:", Location = new Point(10, 20), Width = 100 };
            txtStaffUser = new TextBox() { Location = new Point(110, 17), Width = 150 };
            Label ls2 = new Label() { Text = "Password:", Location = new Point(270, 20), Width = 100 };
            txtStaffPass = new TextBox() { Location = new Point(370, 17), Width = 150, PasswordChar = '*' };

            btnAddStaff = new Button() { Text = "Add", Location = new Point(540, 15), Width = 90 };
            btnAddStaff.Click += BtnAddStaff_Click;
            btnUpdateStaff = new Button() { Text = "Update", Location = new Point(640, 15), Width = 90 };
            btnUpdateStaff.Click += BtnUpdateStaff_Click;
            btnDeleteStaff = new Button() { Text = "Delete", Location = new Point(740, 15), Width = 90 };
            btnDeleteStaff.Click += BtnDeleteStaff_Click;

            dgvStaff = new DataGridView() { Location = new Point(10, 80), Width = 920, Height = 380, ReadOnly = true, AllowUserToAddRows = false, SelectionMode = DataGridViewSelectionMode.FullRowSelect };
            dgvStaff.SelectionChanged += DgvStaff_SelectionChanged;

            tabStaff.Controls.Add(ls1); tabStaff.Controls.Add(txtStaffUser);
            tabStaff.Controls.Add(ls2); tabStaff.Controls.Add(txtStaffPass);
            tabStaff.Controls.Add(btnAddStaff); tabStaff.Controls.Add(btnUpdateStaff); tabStaff.Controls.Add(btnDeleteStaff);
            tabStaff.Controls.Add(dgvStaff);

            // Form
            this.ClientSize = new Size(960, 520);
            this.Controls.Add(this.tabControl);
            this.Name = "AdminDashboardForm";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Admin Dashboard";
            this.ResumeLayout(false);

            Theme.ApplyToForm(this);
        }

        private void TabControl_DrawItem(object? sender, DrawItemEventArgs e)
        {
            TabControl tc = (TabControl)sender!;
            Graphics g = e.Graphics;
            Brush textBrush;
            Brush fillBrush;

            TabPage tabPage = tc.TabPages[e.Index];
            Rectangle tabBounds = tc.GetTabRect(e.Index);

            if (e.State == DrawItemState.Selected)
            {
                fillBrush = new SolidBrush(Theme.SecondaryColor);
                textBrush = new SolidBrush(Theme.PrimaryColor);
            }
            else
            {
                fillBrush = new SolidBrush(Theme.PrimaryColor);
                textBrush = new SolidBrush(Color.White);
            }

            g.FillRectangle(fillBrush, tabBounds);

            StringFormat stringFlags = new StringFormat();
            stringFlags.Alignment = StringAlignment.Center;
            stringFlags.LineAlignment = StringAlignment.Center;
            g.DrawString(tabPage.Text, tc.Font, textBrush, tabBounds, new StringFormat(stringFlags));
        }

        private void LoadStaff()
        {
            try { dgvStaff.DataSource = DbHelper.ExecuteQuery("SELECT id, username FROM admins"); }
            catch { }
        }

        private void DgvStaff_SelectionChanged(object? sender, EventArgs e)
        {
            if (dgvStaff.SelectedRows.Count > 0)
            {
                _selectedStaffId = Convert.ToInt32(dgvStaff.SelectedRows[0].Cells["id"].Value);
                txtStaffUser.Text = dgvStaff.SelectedRows[0].Cells["username"].Value?.ToString();
                txtStaffPass.Clear(); // don't show hash
            }
        }

        private void BtnUpdateStaff_Click(object? sender, EventArgs e)
        {
            if (_selectedStaffId == -1) return;
            if (string.IsNullOrWhiteSpace(txtStaffUser.Text))
            {
                CustomMessageBoxForm.Show("Username required.");
                return;
            }

            try
            {
                if (!string.IsNullOrWhiteSpace(txtStaffPass.Text))
                {
                    string hash;
                    using (System.Security.Cryptography.SHA256 sha256 = System.Security.Cryptography.SHA256.Create())
                    {
                        byte[] bytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(txtStaffPass.Text));
                        System.Text.StringBuilder builder = new System.Text.StringBuilder();
                        for (int i = 0; i < bytes.Length; i++) builder.Append(bytes[i].ToString("x2"));
                        hash = builder.ToString();
                    }
                    DbHelper.ExecuteNonQuery("UPDATE admins SET username=@u, password_hash=@p WHERE id=@id",
                        new MySqlParameter("@u", txtStaffUser.Text.Trim()),
                        new MySqlParameter("@p", hash),
                        new MySqlParameter("@id", _selectedStaffId));
                }
                else
                {
                    DbHelper.ExecuteNonQuery("UPDATE admins SET username=@u WHERE id=@id",
                        new MySqlParameter("@u", txtStaffUser.Text.Trim()),
                        new MySqlParameter("@id", _selectedStaffId));
                }
                CustomMessageBoxForm.Show("Staff updated.", "Success");
                txtStaffUser.Clear(); txtStaffPass.Clear();
                LoadStaff();
            }
            catch (Exception ex)
            {
                CustomMessageBoxForm.Show("Error updating staff: " + ex.Message, "Error", true);
            }
        }

        private void BtnDeleteStaff_Click(object? sender, EventArgs e)
        {
            if (_selectedStaffId == -1) return;
            try
            {
                DbHelper.ExecuteNonQuery("DELETE FROM admins WHERE id=@id", new MySqlParameter("@id", _selectedStaffId));
                CustomMessageBoxForm.Show("Staff deleted.", "Success");
                txtStaffUser.Clear(); txtStaffPass.Clear();
                LoadStaff();
            }
            catch (Exception ex)
            {
                CustomMessageBoxForm.Show("Error deleting staff: " + ex.Message, "Error", true);
            }
        }

        private void BtnAddStaff_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtStaffUser.Text) || string.IsNullOrWhiteSpace(txtStaffPass.Text))
            {
                CustomMessageBoxForm.Show("Username and password required.");
                return;
            }

            string hash = txtStaffPass.Text;
            using (System.Security.Cryptography.SHA256 sha256 = System.Security.Cryptography.SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(txtStaffPass.Text));
                System.Text.StringBuilder builder = new System.Text.StringBuilder();
                for (int i = 0; i < bytes.Length; i++) builder.Append(bytes[i].ToString("x2"));
                hash = builder.ToString();
            }

            try
            {
                DbHelper.ExecuteNonQuery("INSERT INTO admins (username, password_hash) VALUES (@u, @p)",
                    new MySqlParameter("@u", txtStaffUser.Text.Trim()),
                    new MySqlParameter("@p", hash));
                CustomMessageBoxForm.Show("Staff added.");
                txtStaffUser.Clear(); txtStaffPass.Clear();
                LoadStaff();
            }
            catch (Exception ex)
            {
                CustomMessageBoxForm.Show("Error adding staff (username might exist): " + ex.Message);
            }
        }

        private void LoadEventsData()
        {
            try
            {
                DataTable dt = DbHelper.ExecuteQuery("SELECT * FROM events ORDER BY event_date DESC");
                dgvEventsAdmin.DataSource = dt;

                // Bind dropdowns if there are events
                if (dt.Rows.Count > 0)
                {
                    DataTable dt2 = dt.Copy();
                    DataTable dt3 = dt.Copy();

                    cmbRegEvents.DisplayMember = "title"; cmbRegEvents.ValueMember = "id"; cmbRegEvents.DataSource = dt2;
                    cmbAttEvents.DisplayMember = "title"; cmbAttEvents.ValueMember = "id"; cmbAttEvents.DataSource = dt3;
                }
            }
            catch (Exception ex)
            {
                CustomMessageBoxForm.Show("Error loading events: " + ex.Message, "Error", true);
            }
        }

        private void DgvEventsAdmin_SelectionChanged(object? sender, EventArgs e)
        {
            if (dgvEventsAdmin.SelectedRows.Count > 0)
            {
                _selectedEventId = Convert.ToInt32(dgvEventsAdmin.SelectedRows[0].Cells["id"].Value);
                txtTitle.Text = dgvEventsAdmin.SelectedRows[0].Cells["title"].Value?.ToString();
                txtLocation.Text = dgvEventsAdmin.SelectedRows[0].Cells["location"].Value?.ToString();
                txtDescription.Text = dgvEventsAdmin.SelectedRows[0].Cells["description"].Value?.ToString();

                if (DateTime.TryParse(dgvEventsAdmin.SelectedRows[0].Cells["event_date"].Value?.ToString(), out DateTime d))
                    dtpDate.Value = d;

                if (TimeSpan.TryParse(dgvEventsAdmin.SelectedRows[0].Cells["event_time"].Value?.ToString(), out TimeSpan t))
                    dtpTime.Value = DateTime.Today.Add(t);
            }
        }

        private void BtnCreateEvent_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTitle.Text) || string.IsNullOrWhiteSpace(txtLocation.Text))
            {
                CustomMessageBoxForm.Show("Title and Location are required.");
                return;
            }

            try
            {
                string query = @"INSERT INTO events (title, event_date, event_time, location, description)
                                 VALUES (@t, @d, @ti, @loc, @desc)";
                DbHelper.ExecuteNonQuery(query,
                    new MySqlParameter("@t", txtTitle.Text.Trim()),
                    new MySqlParameter("@d", dtpDate.Value.Date),
                    new MySqlParameter("@ti", dtpTime.Value.TimeOfDay),
                    new MySqlParameter("@loc", txtLocation.Text.Trim()),
                    new MySqlParameter("@desc", txtDescription.Text.Trim())
                );
                CustomMessageBoxForm.Show("Event created!", "Success");
                txtTitle.Clear(); txtLocation.Clear(); txtDescription.Clear();
                LoadEventsData();
            }
            catch (Exception ex)
            {
                CustomMessageBoxForm.Show("Error creating event: " + ex.Message, "Error", true);
            }
        }

        private void BtnUpdateEvent_Click(object? sender, EventArgs e)
        {
            if (_selectedEventId == -1) return;
            if (string.IsNullOrWhiteSpace(txtTitle.Text) || string.IsNullOrWhiteSpace(txtLocation.Text))
            {
                CustomMessageBoxForm.Show("Title and Location are required.");
                return;
            }

            try
            {
                string query = @"UPDATE events SET title=@t, event_date=@d, event_time=@ti, location=@loc, description=@desc WHERE id=@id";
                DbHelper.ExecuteNonQuery(query,
                    new MySqlParameter("@t", txtTitle.Text.Trim()),
                    new MySqlParameter("@d", dtpDate.Value.Date),
                    new MySqlParameter("@ti", dtpTime.Value.TimeOfDay),
                    new MySqlParameter("@loc", txtLocation.Text.Trim()),
                    new MySqlParameter("@desc", txtDescription.Text.Trim()),
                    new MySqlParameter("@id", _selectedEventId)
                );
                CustomMessageBoxForm.Show("Event updated!", "Success");
                LoadEventsData();
            }
            catch (Exception ex)
            {
                CustomMessageBoxForm.Show("Error updating event: " + ex.Message, "Error", true);
            }
        }

        private void BtnDeleteEvent_Click(object? sender, EventArgs e)
        {
            if (_selectedEventId == -1) return;
            try
            {
                DbHelper.ExecuteNonQuery("DELETE FROM events WHERE id=@id", new MySqlParameter("@id", _selectedEventId));
                CustomMessageBoxForm.Show("Event deleted!", "Success");
                txtTitle.Clear(); txtLocation.Clear(); txtDescription.Clear();
                LoadEventsData();
            }
            catch (Exception ex)
            {
                CustomMessageBoxForm.Show("Error deleting event: " + ex.Message, "Error", true);
            }
        }

        private void LoadRegistrations()
        {
            if (cmbRegEvents.SelectedValue == null) return;
            if (int.TryParse(cmbRegEvents.SelectedValue.ToString(), out int eventId))
            {
                string query = "SELECT id, first_name, last_name, email, age, phone FROM attendees WHERE event_id = @e";
                dgvRegistrations.DataSource = DbHelper.ExecuteQuery(query, new MySqlParameter("@e", eventId));
            }
        }

        private void DgvRegistrations_SelectionChanged(object? sender, EventArgs e)
        {
            if (dgvRegistrations.SelectedRows.Count > 0)
            {
                _selectedRegId = Convert.ToInt32(dgvRegistrations.SelectedRows[0].Cells["id"].Value);
                txtRegFName.Text = dgvRegistrations.SelectedRows[0].Cells["first_name"].Value?.ToString();
                txtRegLName.Text = dgvRegistrations.SelectedRows[0].Cells["last_name"].Value?.ToString();
                txtRegEmail.Text = dgvRegistrations.SelectedRows[0].Cells["email"].Value?.ToString();
                txtRegPhone.Text = dgvRegistrations.SelectedRows[0].Cells["phone"].Value?.ToString();
                if (int.TryParse(dgvRegistrations.SelectedRows[0].Cells["age"].Value?.ToString(), out int a))
                    numRegAge.Value = a;
            }
        }

        private void BtnUpdateReg_Click(object? sender, EventArgs e)
        {
            if (_selectedRegId == -1) return;
            if (string.IsNullOrWhiteSpace(txtRegFName.Text) || string.IsNullOrWhiteSpace(txtRegLName.Text))
            {
                CustomMessageBoxForm.Show("Name required.");
                return;
            }
            try
            {
                DbHelper.ExecuteNonQuery(@"UPDATE attendees SET first_name=@f, last_name=@l, email=@e, phone=@p, age=@a WHERE id=@id",
                    new MySqlParameter("@f", txtRegFName.Text.Trim()),
                    new MySqlParameter("@l", txtRegLName.Text.Trim()),
                    new MySqlParameter("@e", txtRegEmail.Text.Trim()),
                    new MySqlParameter("@p", txtRegPhone.Text.Trim()),
                    new MySqlParameter("@a", numRegAge.Value),
                    new MySqlParameter("@id", _selectedRegId));
                CustomMessageBoxForm.Show("Registration updated.", "Success");
                LoadRegistrations();
            }
            catch (Exception ex)
            {
                CustomMessageBoxForm.Show("Error updating registration: " + ex.Message, "Error", true);
            }
        }

        private void BtnDeleteReg_Click(object? sender, EventArgs e)
        {
            if (_selectedRegId == -1) return;
            try
            {
                DbHelper.ExecuteNonQuery("DELETE FROM attendees WHERE id=@id", new MySqlParameter("@id", _selectedRegId));
                CustomMessageBoxForm.Show("Registration deleted.", "Success");
                txtRegFName.Clear(); txtRegLName.Clear(); txtRegEmail.Clear(); txtRegPhone.Clear();
                LoadRegistrations();
            }
            catch (Exception ex)
            {
                CustomMessageBoxForm.Show("Error deleting registration: " + ex.Message, "Error", true);
            }
        }

        private void LoadAttendance()
        {
            if (cmbAttEvents.SelectedValue == null) return;
            if (int.TryParse(cmbAttEvents.SelectedValue.ToString(), out int eventId))
            {
                // Only fetch pending attendance to avoid re-approving
                string query = @"SELECT a.id as AttendanceID, att.first_name, att.last_name, att.email, a.status
                                 FROM attendance a
                                 JOIN attendees att ON a.attendee_id = att.id
                                 WHERE a.event_id = @e AND a.status = 'Pending'";
                picProofViewer.Image = null;
                dgvAttendance.DataSource = DbHelper.ExecuteQuery(query, new MySqlParameter("@e", eventId));
            }
        }

        private void DgvAttendance_SelectionChanged(object? sender, EventArgs e)
        {
            if (dgvAttendance.SelectedRows.Count > 0)
            {
                int attId = Convert.ToInt32(dgvAttendance.SelectedRows[0].Cells["AttendanceID"].Value);
                DataTable dt = DbHelper.ExecuteQuery("SELECT proof_image FROM attendance WHERE id = @id", new MySqlParameter("@id", attId));
                if (dt.Rows.Count > 0 && dt.Rows[0]["proof_image"] != DBNull.Value)
                {
                    byte[] imgBytes = (byte[])dt.Rows[0]["proof_image"];
                    using (MemoryStream ms = new MemoryStream(imgBytes))
                    {
                        Image originalImage = Image.FromStream(ms);
                        picProofViewer.Image = new Bitmap(originalImage);
                    }
                }
                else
                {
                    picProofViewer.Image = null;
                }
            }
        }

        private void UpdateAttendanceStatus(string status)
        {
            if (dgvAttendance.SelectedRows.Count == 0) return;
            int attId = Convert.ToInt32(dgvAttendance.SelectedRows[0].Cells["AttendanceID"].Value);
            DbHelper.ExecuteNonQuery("UPDATE attendance SET status = @s WHERE id = @id",
                new MySqlParameter("@s", status), new MySqlParameter("@id", attId));
            CustomMessageBoxForm.Show($"Marked as {status}");
            LoadAttendance();
        }

    }
}
