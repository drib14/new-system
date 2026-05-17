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
        private TabPage tabAnalytics;
        private TabPage tabStaff;
        private DataGridView dgvStaff;
        private TextBox txtStaffUser, txtStaffPass;
        private Button btnAddStaff;

        // Events Tab
        private DataGridView dgvEventsAdmin;
        private TextBox txtTitle, txtLocation, txtDescription;
        private DateTimePicker dtpDate;
        private DateTimePicker dtpTime;
        private Button btnCreateEvent;

        // Registrations Tab
        private DataGridView dgvRegistrations;
        private ComboBox cmbRegEvents;

        // Attendance Tab
        private DataGridView dgvAttendance;
        private ComboBox cmbAttEvents;
        private PictureBox picProofViewer;
        private Button btnApprove;
        private Button btnReject;

        // Analytics Tab
        private ComboBox cmbAnaEvents;
        private Label lblTotalReg;
        private Label lblTotalAtt;
        private Chart chartAges;
        private Chart chartAttendance;

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
            this.tabAnalytics = new TabPage();
            this.tabStaff = new TabPage();

            this.tabControl.Controls.Add(this.tabEvents);
            this.tabControl.Controls.Add(this.tabRegistrations);
            this.tabControl.Controls.Add(this.tabAttendance);
            this.tabControl.Controls.Add(this.tabAnalytics);
            this.tabControl.Controls.Add(this.tabStaff);
            this.tabControl.Dock = DockStyle.Fill;

            // --- Events Tab Setup ---
            this.tabEvents.Text = "Manage Events";
            Label l1 = new Label() { Text = "Title:", Location = new Point(10, 20) };
            txtTitle = new TextBox() { Location = new Point(100, 17), Width = 200 };
            Label l2 = new Label() { Text = "Date:", Location = new Point(10, 60) };
            dtpDate = new DateTimePicker() { Location = new Point(100, 57), Width = 200, Format = DateTimePickerFormat.Short };
            Label l3 = new Label() { Text = "Time:", Location = new Point(10, 100) };
            dtpTime = new DateTimePicker() { Location = new Point(100, 97), Width = 200, Format = DateTimePickerFormat.Time, ShowUpDown = true };
            Label l4 = new Label() { Text = "Location:", Location = new Point(10, 140) };
            txtLocation = new TextBox() { Location = new Point(100, 137), Width = 200 };
            Label l5 = new Label() { Text = "Desc:", Location = new Point(10, 180) };
            txtDescription = new TextBox() { Location = new Point(100, 177), Width = 200, Multiline = true, Height = 60 };
            btnCreateEvent = new Button() { Text = "Create Event", Location = new Point(100, 250), Width = 120 };
            btnCreateEvent.Click += BtnCreateEvent_Click;

            dgvEventsAdmin = new DataGridView() { Location = new Point(330, 20), Width = 600, Height = 450, ReadOnly = true, AllowUserToAddRows = false, SelectionMode = DataGridViewSelectionMode.FullRowSelect };

            tabEvents.Controls.Add(l1); tabEvents.Controls.Add(txtTitle);
            tabEvents.Controls.Add(l2); tabEvents.Controls.Add(dtpDate);
            tabEvents.Controls.Add(l3); tabEvents.Controls.Add(dtpTime);
            tabEvents.Controls.Add(l4); tabEvents.Controls.Add(txtLocation);
            tabEvents.Controls.Add(l5); tabEvents.Controls.Add(txtDescription);
            tabEvents.Controls.Add(btnCreateEvent);
            tabEvents.Controls.Add(dgvEventsAdmin);

            // --- Registrations Tab Setup ---
            tabRegistrations.Text = "Registrations";
            Label lr = new Label() { Text = "Select Event:", Location = new Point(10, 20) };
            cmbRegEvents = new ComboBox() { Location = new Point(100, 17), Width = 200, DropDownStyle = ComboBoxStyle.DropDownList };
            cmbRegEvents.SelectedIndexChanged += (s, e) => LoadRegistrations();
            dgvRegistrations = new DataGridView() { Location = new Point(10, 60), Width = 920, Height = 400, ReadOnly = true, AllowUserToAddRows = false, SelectionMode = DataGridViewSelectionMode.FullRowSelect };
            tabRegistrations.Controls.Add(lr); tabRegistrations.Controls.Add(cmbRegEvents); tabRegistrations.Controls.Add(dgvRegistrations);

            // --- Attendance Tab Setup ---
            tabAttendance.Text = "Approve Attendance";
            Label la = new Label() { Text = "Select Event:", Location = new Point(10, 20) };
            cmbAttEvents = new ComboBox() { Location = new Point(100, 17), Width = 200, DropDownStyle = ComboBoxStyle.DropDownList };
            cmbAttEvents.SelectedIndexChanged += (s, e) => LoadAttendance();
            dgvAttendance = new DataGridView() { Location = new Point(10, 60), Width = 500, Height = 400, ReadOnly = true, AllowUserToAddRows = false, SelectionMode = DataGridViewSelectionMode.FullRowSelect };
            dgvAttendance.SelectionChanged += DgvAttendance_SelectionChanged;

            picProofViewer = new PictureBox() { Location = new Point(530, 60), Width = 400, Height = 350, SizeMode = PictureBoxSizeMode.Zoom, BorderStyle = BorderStyle.FixedSingle };
            btnApprove = new Button() { Text = "Approve", Location = new Point(530, 420), Width = 100, BackColor = Color.LightGreen };
            btnApprove.Click += (s, e) => UpdateAttendanceStatus("Approved");
            btnReject = new Button() { Text = "Reject", Location = new Point(650, 420), Width = 100, BackColor = Color.LightCoral };
            btnReject.Click += (s, e) => UpdateAttendanceStatus("Rejected");

            tabAttendance.Controls.Add(la); tabAttendance.Controls.Add(cmbAttEvents); tabAttendance.Controls.Add(dgvAttendance);
            tabAttendance.Controls.Add(picProofViewer); tabAttendance.Controls.Add(btnApprove); tabAttendance.Controls.Add(btnReject);

            // --- Analytics Tab Setup ---
            tabAnalytics.Text = "Analytics";
            Label lan = new Label() { Text = "Select Event:", Location = new Point(10, 20) };
            cmbAnaEvents = new ComboBox() { Location = new Point(100, 17), Width = 200, DropDownStyle = ComboBoxStyle.DropDownList };
            cmbAnaEvents.SelectedIndexChanged += (s, e) => LoadAnalytics();

            lblTotalReg = new Label() { Location = new Point(10, 60), Width = 300, Font = new Font("Segoe UI Light", 16) };
            lblTotalAtt = new Label() { Location = new Point(10, 100), Width = 300, Font = new Font("Segoe UI Light", 16) };

            // Attendance Bar Chart
            chartAttendance = new Chart() { Location = new Point(10, 150), Width = 400, Height = 300 };
            ChartArea caAtt = new ChartArea("MainArea");
            chartAttendance.ChartAreas.Add(caAtt);
            Series sAtt = new Series("Attendance") { ChartType = SeriesChartType.Column };
            sAtt.IsValueShownAsLabel = true;
            chartAttendance.Series.Add(sAtt);

            // Demographics Pie Chart
            chartAges = new Chart() { Location = new Point(450, 60), Width = 450, Height = 390 };
            chartAges.Titles.Add("Attendee Demographics (Ages)");
            ChartArea caAges = new ChartArea("MainArea");
            chartAges.ChartAreas.Add(caAges);
            Series sAges = new Series("Ages") { ChartType = SeriesChartType.Doughnut };
            sAges.IsValueShownAsLabel = true;
            chartAges.Series.Add(sAges);

            tabAnalytics.Controls.Add(lan); tabAnalytics.Controls.Add(cmbAnaEvents);
            tabAnalytics.Controls.Add(lblTotalReg); tabAnalytics.Controls.Add(lblTotalAtt);
            tabAnalytics.Controls.Add(chartAttendance);
            tabAnalytics.Controls.Add(chartAges);

            // --- Staff Tab Setup ---
            tabStaff.Text = "Manage Staff";
            Label ls1 = new Label() { Text = "Username:", Location = new Point(10, 20) };
            txtStaffUser = new TextBox() { Location = new Point(100, 17), Width = 150 };
            Label ls2 = new Label() { Text = "Password:", Location = new Point(270, 20) };
            txtStaffPass = new TextBox() { Location = new Point(350, 17), Width = 150, PasswordChar = '*' };
            btnAddStaff = new Button() { Text = "Add Staff", Location = new Point(520, 15), Width = 100 };
            btnAddStaff.Click += BtnAddStaff_Click;

            dgvStaff = new DataGridView() { Location = new Point(10, 60), Width = 920, Height = 400, ReadOnly = true, AllowUserToAddRows = false, SelectionMode = DataGridViewSelectionMode.FullRowSelect };

            tabStaff.Controls.Add(ls1); tabStaff.Controls.Add(txtStaffUser);
            tabStaff.Controls.Add(ls2); tabStaff.Controls.Add(txtStaffPass);
            tabStaff.Controls.Add(btnAddStaff); tabStaff.Controls.Add(dgvStaff);

            // Form
            this.ClientSize = new Size(960, 520);
            this.Controls.Add(this.tabControl);
            this.Name = "AdminDashboardForm";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Admin Dashboard";
            this.ResumeLayout(false);

            Theme.ApplyToForm(this);
        }


        private void LoadStaff()
        {
            try { dgvStaff.DataSource = DbHelper.ExecuteQuery("SELECT id, username FROM admins"); }
            catch { }
        }

        private void BtnAddStaff_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtStaffUser.Text) || string.IsNullOrWhiteSpace(txtStaffPass.Text))
            {
                MessageBox.Show("Username and password required.");
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
                MessageBox.Show("Staff added.");
                txtStaffUser.Clear(); txtStaffPass.Clear();
                LoadStaff();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error adding staff (username might exist): " + ex.Message);
            }
        }

        private void LoadEventsData()
        {
            try
            {
                DataTable dt = DbHelper.ExecuteQuery("SELECT * FROM events ORDER BY event_date DESC");
                dgvEventsAdmin.DataSource = dt;

                // Detach event handlers first to avoid null binding issues
                cmbRegEvents.SelectedIndexChanged -= (s, e) => LoadRegistrations();
                cmbAttEvents.SelectedIndexChanged -= (s, e) => LoadAttendance();
                cmbAnaEvents.SelectedIndexChanged -= (s, e) => LoadAnalytics();

                // Bind dropdowns if there are events
                if (dt.Rows.Count > 0)
                {
                    DataTable dt2 = dt.Copy();
                    DataTable dt3 = dt.Copy();
                    DataTable dt4 = dt.Copy();

                    cmbRegEvents.DisplayMember = "title"; cmbRegEvents.ValueMember = "id"; cmbRegEvents.DataSource = dt2;
                    cmbAttEvents.DisplayMember = "title"; cmbAttEvents.ValueMember = "id"; cmbAttEvents.DataSource = dt3;
                    cmbAnaEvents.DisplayMember = "title"; cmbAnaEvents.ValueMember = "id"; cmbAnaEvents.DataSource = dt4;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading events: " + ex.Message);
            }
        }

        private void BtnCreateEvent_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTitle.Text) || string.IsNullOrWhiteSpace(txtLocation.Text))
            {
                MessageBox.Show("Title and Location are required.");
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
                MessageBox.Show("Event created!");
                txtTitle.Clear(); txtLocation.Clear(); txtDescription.Clear();
                LoadEventsData();
            LoadStaff();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error creating event: " + ex.Message);
            }
        }

        private void LoadRegistrations()
        {
            if (cmbRegEvents.SelectedValue == null) return;
            string query = "SELECT id, first_name, last_name, email, age, phone FROM attendees WHERE event_id = @e";
            dgvRegistrations.DataSource = DbHelper.ExecuteQuery(query, new MySqlParameter("@e", cmbRegEvents.SelectedValue));
        }

        private void LoadAttendance()
        {
            if (cmbAttEvents.SelectedValue == null) return;
            string query = @"SELECT a.id as AttendanceID, att.first_name, att.last_name, att.email, a.status
                             FROM attendance a
                             JOIN attendees att ON a.attendee_id = att.id
                             WHERE a.event_id = @e";
            dgvAttendance.DataSource = DbHelper.ExecuteQuery(query, new MySqlParameter("@e", cmbAttEvents.SelectedValue));
            picProofViewer.Image = null;
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
                    MemoryStream ms = new MemoryStream(imgBytes);
                    picProofViewer.Image = Image.FromStream(ms);
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
            MessageBox.Show($"Marked as {status}");
            LoadAttendance();
        }

        private void LoadAnalytics()
        {
            if (cmbAnaEvents.SelectedValue == null) return;
            int eId = Convert.ToInt32(cmbAnaEvents.SelectedValue);

            int totalReg = Convert.ToInt32(DbHelper.ExecuteScalar("SELECT COUNT(*) FROM attendees WHERE event_id = @e", new MySqlParameter("@e", eId)));
            int totalAtt = Convert.ToInt32(DbHelper.ExecuteScalar("SELECT COUNT(*) FROM attendance WHERE event_id = @e AND status = 'Approved'", new MySqlParameter("@e", eId)));

            lblTotalReg.Text = $"Total Registered: {totalReg}";
            lblTotalAtt.Text = $"Total Attended: {totalAtt}";

            // Update Bar Chart
            chartAttendance.Series["Attendance"].Points.Clear();
            chartAttendance.Series["Attendance"].Points.AddXY("Registered", totalReg);
            chartAttendance.Series["Attendance"].Points.AddXY("Attended", totalAtt);

            // Pie chart for ages (Doughnut)
            DataTable dtAges = DbHelper.ExecuteQuery("SELECT age, COUNT(*) as cnt FROM attendees WHERE event_id = @e GROUP BY age", new MySqlParameter("@e", eId));
            chartAges.Series["Ages"].Points.Clear();
            foreach (DataRow row in dtAges.Rows)
            {
                chartAges.Series["Ages"].Points.AddXY($"{row["age"]} yrs", row["cnt"]);
            }
        }
    }
}
