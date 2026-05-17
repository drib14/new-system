using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace EventManager
{
    public static class Theme
    {
        // Unique Minimalist Palette
        public static readonly Color BgColor = Color.FromArgb(250, 250, 250); // Off-white
        public static readonly Color PrimaryColor = Color.FromArgb(24, 24, 27); // Zinc 900 (almost black)
        public static readonly Color SecondaryColor = Color.FromArgb(244, 244, 245); // Zinc 100
        public static readonly Color TextColor = Color.FromArgb(39, 39, 42); // Zinc 800
        public static readonly Color TextMuted = Color.FromArgb(113, 113, 122); // Zinc 500
        public static readonly Color AccentSuccess = Color.FromArgb(16, 185, 129); // Emerald 500
        public static readonly Color AccentDanger = Color.FromArgb(239, 68, 68); // Red 500

        public static readonly Font MainFont = new Font("Segoe UI", 10.5F, FontStyle.Regular, GraphicsUnit.Point);
        public static readonly Font ButtonFont = new Font("Segoe UI Semibold", 10F, FontStyle.Regular, GraphicsUnit.Point);
        public static readonly Font TitleFont = new Font("Segoe UI Semilight", 22F, FontStyle.Regular, GraphicsUnit.Point);
        public static readonly Font SubTitleFont = new Font("Segoe UI Semibold", 11F, FontStyle.Regular, GraphicsUnit.Point);

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
                btn.Font = ButtonFont;
                btn.ForeColor = Color.White;
                btn.Cursor = Cursors.Hand;
                btn.Height = Math.Max(btn.Height, 45); // Taller buttons
                btn.Padding = new Padding(10, 5, 10, 5); // Readability

                // Color mapping based on context
                if (btn.Text.Contains("Reject") || btn.Text.Contains("Delete"))
                    btn.BackColor = AccentDanger;
                else if (btn.Text.Contains("Approve") || btn.Text.Contains("Register") || btn.Text.Contains("Login") || btn.Text.Contains("Submit"))
                    btn.BackColor = AccentSuccess;
                else
                    btn.BackColor = PrimaryColor;
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
                    lbl.ForeColor = TextColor;
                }
            }
            else if (ctrl is TextBox txt)
            {
                txt.BorderStyle = BorderStyle.FixedSingle;
                txt.Font = MainFont;
                txt.BackColor = Color.White;
                txt.ForeColor = TextColor;
                txt.Height = Math.Max(txt.Height, 35);
                txt.Margin = new Padding(0, 5, 0, 5);
            }
            else if (ctrl is ComboBox cmb)
            {
                cmb.FlatStyle = FlatStyle.Flat;
                cmb.Font = MainFont;
                cmb.BackColor = SecondaryColor;
            }
            else if (ctrl is DataGridView dgv)
            {
                dgv.BackgroundColor = Color.White;
                dgv.BorderStyle = BorderStyle.None;
                dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
                dgv.GridColor = SecondaryColor;
                dgv.EnableHeadersVisualStyles = false;

                dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
                dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.White;
                dgv.ColumnHeadersDefaultCellStyle.ForeColor = TextMuted;
                dgv.ColumnHeadersDefaultCellStyle.Font = SubTitleFont;
                dgv.ColumnHeadersDefaultCellStyle.Padding = new Padding(0, 10, 0, 10);

                dgv.DefaultCellStyle.SelectionBackColor = SecondaryColor;
                dgv.DefaultCellStyle.SelectionForeColor = TextColor;
                dgv.DefaultCellStyle.Padding = new Padding(5); // Spacious rows
                dgv.RowTemplate.Height = 45; // Tall rows
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
                    area.AxisX.MajorGrid.LineColor = SecondaryColor;
                    area.AxisY.MajorGrid.LineColor = SecondaryColor;
                    area.AxisX.LabelStyle.Font = MainFont;
                    area.AxisX.LabelStyle.ForeColor = TextMuted;
                    area.AxisY.LabelStyle.Font = MainFont;
                    area.AxisY.LabelStyle.ForeColor = TextMuted;
                }
                foreach (var series in chart.Series)
                {
                    series.Font = MainFont;
                    if (series.ChartType == SeriesChartType.Column)
                    {
                        series.Color = PrimaryColor;
                    }
                }
            }

            // Recursive application
            if (!(ctrl is TabControl) && !(ctrl is Panel) && !(ctrl is TabPage) && !(ctrl is Chart))
            {
                foreach (Control child in ctrl.Controls)
                {
                    ApplyToControl(child);
                }
            }
        }
    }
}