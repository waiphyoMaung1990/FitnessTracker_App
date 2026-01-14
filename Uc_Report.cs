using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Data.OleDb;

namespace FitnessTrackerApp
{
    public partial class Uc_Report : UserControl
    {
        private Label lblTotalCalories;
        private Label lblGoalStatus;
        private Label lblRemaining;
        private DataGridView dgvHistory;
        private Panel pnStatusCard;
        
        private string connString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=FitnessTracker.accdb;OLE DB Services=-1;";

        public Uc_Report()
        {
            InitializeComponent();
            this.Size = new Size(700, 500);
            this.BackColor = Color.White;
            BuildUI();
            LoadReportData();
        }

        private void BuildUI()
        {
            Label lblTitle = new Label() { Text = "Your Fitness Progress Report", Font = new Font("Segoe UI", 14, FontStyle.Bold), ForeColor = Color.FromArgb(64, 64, 64), Location = new Point(20, 20), AutoSize = true };

            pnStatusCard = new Panel() { Location = new Point(20, 60), Size = new Size(600, 150), BackColor = Color.FromArgb(240, 240, 245) };

            Label lblTotalTitle = new Label() { Text = "Total Calories Burned:", Location = new Point(20, 20), AutoSize = true, Font = new Font("Segoe UI", 10), ForeColor = Color.Gray };
            lblTotalCalories = new Label() { Text = "0 cal", Location = new Point(20, 45), AutoSize = true, Font = new Font("Segoe UI", 24, FontStyle.Bold), ForeColor = Color.FromArgb(0, 122, 204) };

            Label lblStatusTitle = new Label() { Text = "Goal Status:", Location = new Point(300, 20), AutoSize = true, Font = new Font("Segoe UI", 10), ForeColor = Color.Gray };
            lblGoalStatus = new Label() { Text = "Calculating...", Location = new Point(300, 45), AutoSize = true, Font = new Font("Segoe UI", 14, FontStyle.Bold), ForeColor = Color.DimGray };

            lblRemaining = new Label() { Text = "", Location = new Point(300, 80), AutoSize = true, Font = new Font("Segoe UI", 10, FontStyle.Italic), ForeColor = Color.Gray };

            pnStatusCard.Controls.AddRange(new Control[] { lblTotalTitle, lblTotalCalories, lblStatusTitle, lblGoalStatus, lblRemaining });

            Label lblHistory = new Label() { Text = "Activity History:", Location = new Point(20, 230), AutoSize = true, Font = new Font("Segoe UI", 12, FontStyle.Bold) };

            dgvHistory = new DataGridView() { Location = new Point(20, 260), Size = new Size(600, 200), BackgroundColor = Color.White, BorderStyle = BorderStyle.Fixed3D, ReadOnly = true, AllowUserToAddRows = false, RowHeadersVisible = false, SelectionMode = DataGridViewSelectionMode.FullRowSelect };

            this.Controls.Add(lblTitle);
            this.Controls.Add(pnStatusCard);
            this.Controls.Add(lblHistory);
            this.Controls.Add(dgvHistory);
        }

        public void LoadReportData()
        {
            try
            {
                using (OleDbConnection conn = new OleDbConnection(connString))
                {
                    conn.Open();
                    string username = SessionManager.CurrentUsername;

                    // 1. Get Goal
                    int targetGoal = 0;
                    string goalSql = "SELECT TOP 1 TargetCalories FROM Goals WHERE Username = ? ORDER BY ID DESC";
                    OleDbCommand cmdGoal = new OleDbCommand(goalSql, conn);
                    cmdGoal.Parameters.AddWithValue("?", username);
                    object goalResult = cmdGoal.ExecuteScalar();
                    if (goalResult != null) targetGoal = Convert.ToInt32(goalResult);

                    // 2. Get Activities
                    string workSql = "SELECT WorkoutType, CaloriesBurned, EntryDate FROM Workouts WHERE Username = ? ORDER BY ID DESC";
                    OleDbCommand cmdWork = new OleDbCommand(workSql, conn);
                    cmdWork.Parameters.AddWithValue("?", username);
                    OleDbDataAdapter adapter = new OleDbDataAdapter(cmdWork);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    dgvHistory.DataSource = dt;
                    if (dgvHistory.Columns.Count > 0) dgvHistory.Columns[0].Width = 150;
                    if (dgvHistory.Columns.Count > 1) dgvHistory.Columns[1].Width = 100;
                    if (dgvHistory.Columns.Count > 2) dgvHistory.Columns[2].Width = 200;

                    // 3. Calculate Total
                    int totalBurned = 0;
                    foreach (DataRow row in dt.Rows) totalBurned += Convert.ToInt32(row["CaloriesBurned"]);

                    // 4. Update UI
                    lblTotalCalories.Text = totalBurned.ToString() + " cal";
                    if (targetGoal > 0)
                    {
                        if (totalBurned >= targetGoal)
                        {
                            lblGoalStatus.Text = "GOAL ACHIEVED! ";
                            lblGoalStatus.ForeColor = Color.Green;
                            lblRemaining.Text = $"Exceeded by {totalBurned - targetGoal} cal.";
                            pnStatusCard.BackColor = Color.FromArgb(220, 255, 220);
                        }
                        else
                        {
                            lblGoalStatus.Text = "Keep Going!";
                            lblGoalStatus.ForeColor = Color.OrangeRed;
                            lblRemaining.Text = $"{targetGoal - totalBurned} cal remaining.";
                            pnStatusCard.BackColor = Color.FromArgb(255, 240, 230);
                        }
                    }
                    else
                    {
                        lblGoalStatus.Text = "No Goal Set";
                        lblGoalStatus.ForeColor = Color.Gray;
                        lblRemaining.Text = "Please set a goal.";
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show("Error: " + ex.Message); }
        }
    }
}