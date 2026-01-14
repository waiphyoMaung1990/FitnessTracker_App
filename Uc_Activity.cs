using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Data.OleDb;

namespace FitnessTrackerApp
{
    public partial class Uc_Activity : UserControl
    {
        private class ActivityConfig
        {
            public string Name { get; set; }
            public string M1 { get; set; }
            public string M2 { get; set; }
            public string M3 { get; set; }
        }

        private ComboBox cmbActivity;
        private Label lblM1, lblM2, lblM3;
        private TextBox txtM1, txtM2, txtM3;
        private Button btnSave;
        private Panel pnContainer;
        private Dictionary<string, ActivityConfig> activities;
        
  
        private string connString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=FitnessTracker.accdb;OLE DB Services=-1;";
        public Uc_Activity()
        {
            InitializeComponent();
            this.Size = new Size(700, 500);
            this.BackColor = Color.White;
            InitializeActivities();
            BuildModernUI();
        }

        private void InitializeActivities()
        {
            activities = new Dictionary<string, ActivityConfig>();
            activities.Add("Walking", new ActivityConfig { Name = "Walking", M1 = "Steps", M2 = "Distance (km)", M3 = "Time (min)" });
            activities.Add("Swimming", new ActivityConfig { Name = "Swimming", M1 = "Laps", M2 = "Time (min)", M3 = "Heart Rate" });
            activities.Add("Running", new ActivityConfig { Name = "Running", M1 = "Distance (km)", M2 = "Time (min)", M3 = "Speed (km/h)" });
            activities.Add("Cycling", new ActivityConfig { Name = "Cycling", M1 = "Distance (km)", M2 = "Time (min)", M3 = "Elevation (m)" });
            activities.Add("Yoga", new ActivityConfig { Name = "Yoga", M1 = "Duration (min)", M2 = "Difficulty (1-10)", M3 = "Poses Count" });
            activities.Add("Jump Rope", new ActivityConfig { Name = "Jump Rope", M1 = "Jumps Count", M2 = "Duration (min)", M3 = "Heart Rate" });
        }

        private void BuildModernUI()
        {
            Label lblTitle = new Label() { Text = "New Activity Record", Font = new Font("Segoe UI", 14, FontStyle.Bold), ForeColor = Color.FromArgb(64, 64, 64), Location = new Point(20, 20), AutoSize = true };

            pnContainer = new Panel() { Location = new Point(20, 60), Size = new Size(500, 400), BackColor = Color.FromArgb(245, 247, 250) };

            Label lblSelect = new Label() { Text = "Select Activity", Location = new Point(20, 20), Font = new Font("Segoe UI", 10), ForeColor = Color.Gray };
            cmbActivity = new ComboBox() { Location = new Point(20, 45), Width = 400, DropDownStyle = ComboBoxStyle.DropDownList, Font = new Font("Segoe UI", 11), FlatStyle = FlatStyle.Flat };
            foreach (var key in activities.Keys) cmbActivity.Items.Add(key);
            cmbActivity.SelectedIndex = -1; // Start blank
            cmbActivity.SelectedIndexChanged += (s, e) => UpdateLabels();

            int startY = 100; int gap = 70;
            lblM1 = CreateLabel(20, startY); txtM1 = CreateTextBox(20, startY + 25);
            lblM2 = CreateLabel(20, startY + gap); txtM2 = CreateTextBox(20, startY + gap + 25);
            lblM3 = CreateLabel(20, startY + gap * 2); txtM3 = CreateTextBox(20, startY + gap * 2 + 25);

            btnSave = new Button() { Text = "SAVE RECORD", Location = new Point(20, 340), Size = new Size(460, 45), BackColor = Color.FromArgb(0, 122, 204), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 10, FontStyle.Bold), Cursor = Cursors.Hand };
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.Click += BtnSave_Click;

            pnContainer.Controls.AddRange(new Control[] { lblSelect, cmbActivity, lblM1, txtM1, lblM2, txtM2, lblM3, txtM3, btnSave });
            this.Controls.Add(lblTitle);
            this.Controls.Add(pnContainer);
        }

        private Label CreateLabel(int x, int y) => new Label() { Location = new Point(x, y), AutoSize = true, Font = new Font("Segoe UI", 9), ForeColor = Color.DimGray };
        private TextBox CreateTextBox(int x, int y) => new TextBox() { Location = new Point(x, y), Width = 460, Font = new Font("Segoe UI", 11), BorderStyle = BorderStyle.FixedSingle };

        private void UpdateLabels()
        {
            if (cmbActivity.SelectedItem == null) return;
            string selected = cmbActivity.SelectedItem.ToString();
            if (activities.ContainsKey(selected))
            {
                var config = activities[selected];
                lblM1.Text = config.M1; lblM2.Text = config.M2; lblM3.Text = config.M3;
                txtM1.Clear(); txtM2.Clear(); txtM3.Clear();
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (!double.TryParse(txtM1.Text, out double v1) || !double.TryParse(txtM2.Text, out double v2) || !double.TryParse(txtM3.Text, out double v3))
            {
                MessageBox.Show("Please enter valid numbers.");
                return;
            }

            if (cmbActivity.SelectedItem == null)
            {
                MessageBox.Show("Please select an activity first.");
                return;
            }

            string activity = cmbActivity.SelectedItem.ToString();
            int caloriesBurned = CalculateCalories(activity, v1, v2, v3);

            try
            {
                using (OleDbConnection conn = new OleDbConnection(connString))
                {
                    conn.Open();
                    string sql = "INSERT INTO Workouts (Username, WorkoutType, WorkoutValue, CaloriesBurned, EntryDate) VALUES (?, ?, ?, ?, ?)";
                    OleDbCommand cmd = new OleDbCommand(sql, conn);
                    string combinedValues = $"{v1},{v2},{v3}";
                    cmd.Parameters.AddWithValue("?", SessionManager.CurrentUsername);
                    cmd.Parameters.AddWithValue("?", activity);
                    cmd.Parameters.AddWithValue("?", combinedValues);
                    cmd.Parameters.AddWithValue("?", caloriesBurned);
                    cmd.Parameters.AddWithValue("?", DateTime.Now.ToString());
                    cmd.ExecuteNonQuery();

                    MessageBox.Show($"Success! You burned approx {caloriesBurned} calories.");

                    // Remove saved activity from list and reset UI
                    cmbActivity.Items.Remove(activity);
                    txtM1.Clear(); txtM2.Clear(); txtM3.Clear();
                    lblM1.Text = ""; lblM2.Text = ""; lblM3.Text = "";
                    cmbActivity.SelectedIndex = -1;
                    cmbActivity.Focus();
                }
            }
            catch (Exception ex) { MessageBox.Show("Error: " + ex.Message); }
        }

        private int CalculateCalories(string type, double m1, double m2, double m3)
        {
            double result = 0;
            switch (type)
            {
                case "Walking": result = m2 * 60; break; // m2=Distance
                case "Swimming": result = m2 * 8; break; // m2=Time
                case "Running": result = m1 * 65; break; // m1=Distance
                case "Cycling": result = m2 * 7; break; // m2=Time
                case "Yoga": result = m1 * 3; break; // m1=Duration
                case "Jump Rope": result = m2 * 12; break; // m2=Duration
                default: result = 50; break;
            }
            return (int)result;
        }
    }
}