using System;
using System.Drawing;
using System.Windows.Forms;
using System.Data.OleDb;

namespace FitnessTrackerApp
{
    public partial class Uc_Goal : UserControl
    {
        private TextBox txtGoal;
        private Label lblCurrentGoal;
        private Button btnSave;
       
        private string connString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=FitnessTracker.accdb;OLE DB Services=-1;";

        public Uc_Goal()
        {
            InitializeComponent();
            this.Size = new Size(600, 500);
            this.BackColor = Color.White;
            BuildUI();
            LoadCurrentGoal();
        }

        private void BuildUI()
        {
            Label lblTitle = new Label() { Text = "Set Daily Fitness Goal", Font = new Font("Segoe UI", 14, FontStyle.Bold), ForeColor = Color.FromArgb(64, 64, 64), Location = new Point(20, 20), AutoSize = true };

            Panel pnCard = new Panel() { Location = new Point(20, 60), Size = new Size(500, 300), BackColor = Color.FromArgb(245, 247, 250) };

            Label lblInfo = new Label() { Text = "Current Daily Goal:", Location = new Point(20, 30), AutoSize = true, Font = new Font("Segoe UI", 10), ForeColor = Color.Gray };
            lblCurrentGoal = new Label() { Text = "Not Set", Location = new Point(20, 55), AutoSize = true, Font = new Font("Segoe UI", 20, FontStyle.Bold), ForeColor = Color.FromArgb(0, 150, 136) };

            Label lblInput = new Label() { Text = "Enter New Calorie Target:", Location = new Point(20, 120), AutoSize = true, Font = new Font("Segoe UI", 10), ForeColor = Color.Gray };
            txtGoal = new TextBox() { Location = new Point(20, 145), Width = 460, Font = new Font("Segoe UI", 12), BorderStyle = BorderStyle.FixedSingle };

            btnSave = new Button() { Text = "UPDATE GOAL", Location = new Point(20, 200), Size = new Size(460, 45), BackColor = Color.FromArgb(0, 150, 136), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 10, FontStyle.Bold), Cursor = Cursors.Hand };
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.Click += BtnSave_Click;

            pnCard.Controls.AddRange(new Control[] { lblInfo, lblCurrentGoal, lblInput, txtGoal, btnSave });
            this.Controls.Add(lblTitle);
            this.Controls.Add(pnCard);
        }

        private void LoadCurrentGoal()
        {
            try
            {
                using (OleDbConnection conn = new OleDbConnection(connString))
                {
                    conn.Open();
                    string sql = "SELECT TOP 1 TargetCalories FROM Goals WHERE Username = ? ORDER BY ID DESC";
                    OleDbCommand cmd = new OleDbCommand(sql, conn);
                    cmd.Parameters.AddWithValue("?", SessionManager.CurrentUsername);
                    object result = cmd.ExecuteScalar();
                    if (result != null) lblCurrentGoal.Text = result.ToString() + " Calories";
                    else lblCurrentGoal.Text = "Not Set";
                }
            }
            catch { }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txtGoal.Text, out int goal) || goal <= 0)
            {
                MessageBox.Show("Please enter a valid calorie amount.");
                return;
            }

            try
            {
                using (OleDbConnection conn = new OleDbConnection(connString))
                {
                    conn.Open();
                    string sql = "INSERT INTO Goals (Username, TargetCalories, DateSet) VALUES (?, ?, ?)";
                    OleDbCommand cmd = new OleDbCommand(sql, conn);
                    cmd.Parameters.AddWithValue("?", SessionManager.CurrentUsername);
                    cmd.Parameters.AddWithValue("?", goal);
                    cmd.Parameters.AddWithValue("?", DateTime.Now.ToString());
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Goal updated successfully!");
                    lblCurrentGoal.Text = goal.ToString() + " Calories";
                    txtGoal.Clear();

                    // Switch to Activity Screen automatically
                    if (this.ParentForm is MainDashboard mainForm)
                    {
                        mainForm.SwitchToActivity();
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show("Error saving goal: " + ex.Message); }
        }
    }
}