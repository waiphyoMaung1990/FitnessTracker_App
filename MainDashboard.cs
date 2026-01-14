using System;
using System.Drawing;
using System.Windows.Forms;
using System.Data.OleDb;

namespace FitnessTrackerApp
{
    public partial class MainDashboard : Form
    {
        // UI Controls
        private Panel panelSideMenu;
        private Panel panelHeader;
        private Panel panelDesktop;
        private Label lblTitle;
        private Label lblWelcome;

        // Buttons
        private Button btnActivity;
        private Button btnGoal;
        private Button btnReport;
        private Button btnReset;
        private Button btnLogout;

        // Database Connection String
     //   private string connString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=FitnessTracker.accdb;";
     
        private string connString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=FitnessTracker.accdb;OLE DB Services=-1;";
        public MainDashboard()
        {
            InitializeComponent();
            SetupModernUI();

            // Ensures the application closes completely when the dashboard is closed
            this.FormClosed += (s, e) => Application.Exit();
        }

        private void SetupModernUI()
        {
            // 1. Form Properties
            this.Text = "Fitness Tracker Pro";
            this.Size = new Size(1000, 700);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.White;

            // 2. Sidebar Setup
            panelSideMenu = new Panel();
            panelSideMenu.Dock = DockStyle.Left;
            panelSideMenu.Width = 220;
            panelSideMenu.BackColor = Color.FromArgb(51, 51, 76);
            this.Controls.Add(panelSideMenu);

            // Sidebar Logo
            Panel panelLogo = new Panel();
            panelLogo.Dock = DockStyle.Top;
            panelLogo.Height = 80;
            panelLogo.BackColor = Color.FromArgb(39, 39, 58);

            Label lblLogo = new Label();
            lblLogo.Text = "FITNESS\nTRACKER";
            lblLogo.ForeColor = Color.White;
            lblLogo.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            lblLogo.TextAlign = ContentAlignment.MiddleCenter;
            lblLogo.Dock = DockStyle.Fill;
            panelLogo.Controls.Add(lblLogo);
            panelSideMenu.Controls.Add(panelLogo);

            // 3. Header Setup
            panelHeader = new Panel();
            panelHeader.Dock = DockStyle.Top;
            panelHeader.Height = 80;
            panelHeader.BackColor = Color.FromArgb(0, 150, 136); // Teal Color
            this.Controls.Add(panelHeader);

            lblTitle = new Label();
            lblTitle.Text = "DASHBOARD";
            lblTitle.ForeColor = Color.White;
            lblTitle.Font = new Font("Segoe UI", 15, FontStyle.Regular);
            lblTitle.AutoSize = true;
            lblTitle.Location = new Point(20, 25);
            panelHeader.Controls.Add(lblTitle);

            lblWelcome = new Label();
            lblWelcome.Text = "Welcome, " + (SessionManager.CurrentUsername ?? "User");
            lblWelcome.ForeColor = Color.White;
            lblWelcome.Font = new Font("Segoe UI", 10, FontStyle.Regular);
            lblWelcome.AutoSize = true;
            lblWelcome.Location = new Point(20, 55);
            panelHeader.Controls.Add(lblWelcome);

            // 4. Desktop Panel Setup
            panelDesktop = new Panel();
            panelDesktop.Dock = DockStyle.Fill;
            panelDesktop.BackColor = Color.WhiteSmoke;
            this.Controls.Add(panelDesktop);

            // Adjust Z-Order (Stacking order)
            this.Controls.SetChildIndex(panelDesktop, 0);
            this.Controls.SetChildIndex(panelHeader, 1);
            this.Controls.SetChildIndex(panelSideMenu, 2);

            // 5. Create Menu Buttons

            // (1) Set Goals Button
            btnGoal = CreateMenuButton("Set Goals");
            btnGoal.Click += (s, e) =>
            {
                // Prevent access if previous session data exists
                if (CheckHasData())
                {
                    MessageBox.Show("You have an active session.\nPlease click 'Start New Session' to reset and start over.");
                    return;
                }
                lblTitle.Text = "SET YOUR GOAL";
                ShowUserControl(new Uc_Goal());
            };
            panelSideMenu.Controls.Add(btnGoal);

            // (2) Record Activity Button
            btnActivity = CreateMenuButton("Record Activity");
            btnActivity.Click += (s, e) =>
            {
                // Prevent access if data exists (user should finish session or reset)
                // Note: Logic allows only flow from Goal -> Activity -> Report
                if (CheckHasData())
                {
                    // Optional: You can allow them to enter if they haven't finished all 6 activities, 
                    // but based on your request, we restrict to enforce 'New Session' flow.
                    // Or you can strictly enforce "Start New Session" clears everything.
                    MessageBox.Show("You have active records.\nPlease click 'Start New Session' to clear data and add new activities.");
                    return;
                }
                lblTitle.Text = "RECORD ACTIVITY";
                ShowUserControl(new Uc_Activity());
            };
            panelSideMenu.Controls.Add(btnActivity);

            // (3) Reports Button
            btnReport = CreateMenuButton("Reports");
            btnReport.Click += (s, e) =>
            {
                lblTitle.Text = "YOUR PROGRESS REPORT";
                ShowUserControl(new Uc_Report());
            };
            panelSideMenu.Controls.Add(btnReport);

            // (4) Start New Session Button
            btnReset = CreateMenuButton("Start New Session");
            btnReset.BackColor = Color.FromArgb(192, 0, 0); // Red color for attention
            btnReset.Click += BtnReset_Click;
            panelSideMenu.Controls.Add(btnReset);

            // (5) Logout Button
            btnLogout = CreateMenuButton("Logout");
            btnLogout.Click += (s, e) => {
                SessionManager.ClearSession();
                Application.Restart();
            };
            panelSideMenu.Controls.Add(btnLogout);
        }

        // =============================================
        // Helper Functions
        // =============================================

        // Checks if the user has existing data in the database
        private bool CheckHasData()
        {
            try
            {
                using (OleDbConnection conn = new OleDbConnection(connString))
                {
                    conn.Open();
                    string sql = "SELECT COUNT(*) FROM Workouts WHERE Username = ?";
                    OleDbCommand cmd = new OleDbCommand(sql, conn);
                    cmd.Parameters.AddWithValue("?", SessionManager.CurrentUsername);
                    int count = Convert.ToInt32(cmd.ExecuteScalar());

                    // Check goals too
                    string sqlGoal = "SELECT COUNT(*) FROM Goals WHERE Username = ?";
                    OleDbCommand cmdGoal = new OleDbCommand(sqlGoal, conn);
                    cmdGoal.Parameters.AddWithValue("?", SessionManager.CurrentUsername);
                    int countGoal = Convert.ToInt32(cmdGoal.ExecuteScalar());

                    return (count > 0 || countGoal > 0);
                }
            }
            catch { return false; }
        }

        // Logic to reset the session
        private void BtnReset_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Are you sure you want to start a new session?\nThis will DELETE all your current progress.", "Confirm Reset", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (dr == DialogResult.Yes)
            {
                try
                {
                    using (OleDbConnection conn = new OleDbConnection(connString))
                    {
                        conn.Open();
                        string user = SessionManager.CurrentUsername;

                        // Delete Workouts
                        new OleDbCommand($"DELETE FROM Workouts WHERE Username = '{user}'", conn).ExecuteNonQuery();

                        // Delete Goals
                        new OleDbCommand($"DELETE FROM Goals WHERE Username = '{user}'", conn).ExecuteNonQuery();
                    }

                    MessageBox.Show("Session Reset Successful! You can now set a new goal.");

                    // Automatically go to Goal setting page
                    lblTitle.Text = "SET YOUR GOAL";
                    ShowUserControl(new Uc_Goal());
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error resetting data: " + ex.Message);
                }
            }
        }

        // Function to switch to Activity screen (Called from Uc_Goal)
        public void SwitchToActivity()
        {
            lblTitle.Text = "RECORD ACTIVITY";
            ShowUserControl(new Uc_Activity());
        }

        private Button CreateMenuButton(string text)
        {
            Button btn = new Button();
            btn.Dock = DockStyle.Top;
            btn.Height = 60;
            btn.Text = "   " + text;
            btn.TextAlign = ContentAlignment.MiddleLeft;
            btn.FlatStyle = FlatStyle.Flat;
            btn.ForeColor = Color.Gainsboro;
            btn.BackColor = Color.FromArgb(51, 51, 76);
            btn.Font = new Font("Segoe UI", 10, FontStyle.Regular);
            btn.FlatAppearance.BorderSize = 0;
            btn.TextImageRelation = TextImageRelation.ImageBeforeText;
            btn.Cursor = Cursors.Hand;

            btn.MouseEnter += (s, e) => btn.BackColor = Color.FromArgb(39, 39, 58);
            btn.MouseLeave += (s, e) => {
                if (text == "Start New Session") btn.BackColor = Color.FromArgb(192, 0, 0); // Keep red if reset button
                else btn.BackColor = Color.FromArgb(51, 51, 76);
            };

            // Fix initial color for reset button in MouseLeave logic above or set explicitly
            if (text == "Start New Session") btn.BackColor = Color.FromArgb(192, 0, 0);

            return btn;
        }

        private void ShowUserControl(UserControl uc)
        {
            panelDesktop.Controls.Clear();
            uc.Dock = DockStyle.Fill;
            panelDesktop.Controls.Add(uc);
            uc.BringToFront();
        }
    }
}