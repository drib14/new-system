using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace EventManager
{
    public static class Theme
    {
        public static readonly Color BgColor = Color.White;
        public static readonly Color PrimaryColor = Color.FromArgb(0, 120, 215); // Windows Blue
        public static readonly Color SecondaryColor = Color.FromArgb(240, 240, 240);
        public static readonly Color TextColor = Color.FromArgb(30, 30, 30);
        public static readonly Color SuccessColor = Color.FromArgb(40, 167, 69);
        public static readonly Color DangerColor = Color.FromArgb(220, 53, 69);
        public static readonly Font MainFont = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
        public static readonly Font TitleFont = new Font("Segoe UI Light", 20F, FontStyle.Regular, GraphicsUnit.Point);
        public static readonly Font SubTitleFont = new Font("Segoe UI Semibold", 12F, FontStyle.Regular, GraphicsUnit.Point);

        public static void ApplyToForm(Form form)
        {
            form.BackColor = BgColor;
            form.Font = MainFont;
            form.ForeColor = TextColor;

            foreach (Control ctrl in form.Controls)
            {
                ApplyToControl(ctrl);
            }
        }

        private static void ApplyToControl(Control ctrl)
        {
            if (ctrl is Button btn)
            {
                btn.FlatStyle = FlatStyle.Flat;
                btn.FlatAppearance.BorderSize = 0;
                btn.BackColor = PrimaryColor;
                btn.ForeColor = Color.White;
                btn.Cursor = Cursors.Hand;
                btn.Font = new Font("Segoe UI Semibold", 10F);

                // Keep specific colors if already set
                if (btn.Text.Contains("Reject") || btn.Text.Contains("Delete")) btn.BackColor = DangerColor;
                if (btn.Text.Contains("Approve") || btn.Text.Contains("Register") || btn.Text.Contains("Login")) btn.BackColor = SuccessColor;
            }
            else if (ctrl is Label lbl)
            {
                if (lbl.Name.StartsWith("lblTitle"))
                {
                    lbl.Font = TitleFont;
                    lbl.ForeColor = PrimaryColor;
                }
                else
                {
                    lbl.Font = MainFont;
                }
            }
            else if (ctrl is TextBox txt)
            {
                txt.BorderStyle = BorderStyle.FixedSingle;
                txt.Font = MainFont;
                txt.BackColor = Color.White;
            }
            else if (ctrl is ComboBox cmb)
            {
                cmb.FlatStyle = FlatStyle.Flat;
                cmb.Font = MainFont;
            }
            else if (ctrl is DataGridView dgv)
            {
                dgv.BackgroundColor = BgColor;
                dgv.BorderStyle = BorderStyle.None;
                dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
                dgv.GridColor = Color.FromArgb(230, 230, 230);
                dgv.EnableHeadersVisualStyles = false;
                dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
                dgv.ColumnHeadersDefaultCellStyle.BackColor = BgColor;
                dgv.ColumnHeadersDefaultCellStyle.ForeColor = PrimaryColor;
                dgv.ColumnHeadersDefaultCellStyle.Font = SubTitleFont;
                dgv.DefaultCellStyle.SelectionBackColor = SecondaryColor;
                dgv.DefaultCellStyle.SelectionForeColor = TextColor;
                dgv.RowHeadersVisible = false;
                dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }
            else if (ctrl is TabControl tc)
            {
                tc.Font = MainFont;
                foreach (TabPage tp in tc.TabPages)
                {
                    tp.BackColor = BgColor;
                    foreach (Control c in tp.Controls)
                    {
                        ApplyToControl(c);
                    }
                }
            }
            else if (ctrl is Panel pnl)
            {
                foreach (Control c in pnl.Controls)
                {
                    ApplyToControl(c);
                }
            }
            else if (ctrl is Chart chart)
            {
                chart.BackColor = BgColor;
                foreach (var area in chart.ChartAreas)
                {
                    area.BackColor = BgColor;
                    area.BorderColor = Color.Transparent;
                    area.AxisX.MajorGrid.LineColor = Color.FromArgb(240, 240, 240);
                    area.AxisY.MajorGrid.LineColor = Color.FromArgb(240, 240, 240);
                    area.AxisX.LabelStyle.Font = MainFont;
                    area.AxisY.LabelStyle.Font = MainFont;
                }
                foreach (var series in chart.Series)
                {
                    series.Font = MainFont;
                }
            }

            // Recursive
            if (!(ctrl is TabControl) && !(ctrl is Panel) && !(ctrl is TabPage))
            {
                foreach (Control child in ctrl.Controls)
                {
                    ApplyToControl(child);
                }
            }
        }
    }
}
