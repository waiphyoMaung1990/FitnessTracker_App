using System;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Reflection; // Required for ADOX creation
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Data.OleDb;

namespace FitnessTrackerApp
{
    public partial class Auth : Form
    {
        private int failedAttempts = 0;
        private string dbPath = Path.Combine(Application.StartupPath, "FitnessTracker.accdb");
        private string connString;

        public Auth()
        {
            InitializeComponent();

            // FIXED: Added 'OLE DB Services=-1' to prevent AccessViolationException crashes
            connString = $@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={dbPath};OLE DB Services=-1;";

            SetPlaceholder(txtUsername, "Enter Username");
            SetPlaceholder(txtPassword, "Enter Password", true);

            this.pn_auth.Paint += new PaintEventHandler(pn_auth_Paint);
            this.BackColor = Color.FromArgb(30, 30, 30);

            // Initialize database structure on startup
            InitializeDatabase();
        }

        private void InitializeDatabase()
        {
            try
            {
                // 1. Auto-create database file if it doesn't exist
                if (!System.IO.File.Exists(dbPath))
                {
                    try
                    {
                        Type catType = Type.GetTypeFromProgID("ADOX.Catalog");
                        if (catType != null)
                        {
                            object cat = Activator.CreateInstance(catType);
                            catType.InvokeMember("Create", BindingFlags.InvokeMethod, null, cat, new object[] { connString });
                        }
                    }
                    catch
                    {
                        // Fallback mechanism
                        System.IO.File.Create(dbPath).Close();
                    }
                }

                // 2. Create Tables
                using (OleDbConnection conn = new OleDbConnection(connString))
                {
                    conn.Open();

                    // Create 'Users' table
                    try
                    {
                        string createTableSql = "CREATE TABLE Users (ID AUTOINCREMENT PRIMARY KEY, Username Text(50), [Password] Text(50))";
                        new OleDbCommand(createTableSql, conn).ExecuteNonQuery();
                    }
                    catch { /* Table already exists */ }

                    // Create 'Workouts' table with CaloriesBurned
                    try
                    {
                        string workoutTableSql = "CREATE TABLE Workouts (ID AUTOINCREMENT PRIMARY KEY, Username Text(50), WorkoutType Text(50), WorkoutValue Text(50), CaloriesBurned Int, EntryDate Text(50))";
                        new OleDbCommand(workoutTableSql, conn).ExecuteNonQuery();
                    }
                    catch { /* Table already exists */ }

                    // Create 'Goals' table
                    try
                    {
                        string goalTableSql = "CREATE TABLE Goals (ID AUTOINCREMENT PRIMARY KEY, Username Text(50), TargetCalories Int, DateSet Text(50))";
                        new OleDbCommand(goalTableSql, conn).ExecuteNonQuery();
                    }
                    catch { /* Table already exists */ }

                    // 3. Create Default Admin
                    string checkSql = "SELECT COUNT(*) FROM Users WHERE Username = 'admin'";
                    OleDbCommand cmd = new OleDbCommand(checkSql, conn);
                    int count = Convert.ToInt32(cmd.ExecuteScalar());

                    if (count == 0)
                    {
                        string insertSql = "INSERT INTO Users (Username, [Password]) VALUES ('admin', 'Admin1234567')";
                        new OleDbCommand(insertSql, conn).ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Setup Error: " + ex.Message);
            }
        }

        private bool CheckDatabaseLogin(string u, string p)
        {
            try
            {
                using (OleDbConnection conn = new OleDbConnection(connString))
                {
                    conn.Open();
                    string sql = "SELECT COUNT(*) FROM Users WHERE Username = ? AND [Password] = ?";
                    OleDbCommand cmd = new OleDbCommand(sql, conn);
                    cmd.Parameters.AddWithValue("?", u);
                    cmd.Parameters.AddWithValue("?", p);

                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    return count > 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Login Database Error: " + ex.Message);
                return false;
            }
        }

        // Placeholder Logic
        private void SetPlaceholder(TextBox textBox, string placeholder, bool isPassword = false)
        {
            textBox.TextAlign = HorizontalAlignment.Center;
            textBox.Text = placeholder;
            textBox.ForeColor = Color.Gray;

            if (isPassword)
            {
                textBox.UseSystemPasswordChar = false;
                textBox.PasswordChar = '\0';
            }

            textBox.Enter += (s, e) =>
            {
                if (textBox.Text == placeholder)
                {
                    textBox.Text = "";
                    textBox.ForeColor = Color.Black;
                    if (isPassword)
                    {
                        textBox.PasswordChar = chkShow.Checked ? '\0' : '*';
                    }
                }
            };

            textBox.Leave += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(textBox.Text))
                {
                    textBox.Text = placeholder;
                    textBox.ForeColor = Color.Gray;
                    if (isPassword)
                    {
                        textBox.PasswordChar = '\0';
                    }
                }
            };
        }

        private void btnSignin_Click(object sender, EventArgs e)
        {
            string user = txtUsername.Text.Trim();
            string pass = txtPassword.Text;

            // Basic Validation
            if (user == "Enter Username" || string.IsNullOrEmpty(user))
            {
                MessageBox.Show("Please enter a username.");
                return;
            }

            if (!Regex.IsMatch(user, "^[a-zA-Z0-9]+$"))
            {
                MessageBox.Show("Username must be alphanumeric.");
                return;
            }

            if (pass.Length != 12)
            {
                MessageBox.Show("Password must be exactly 12 characters long.");
                return;
            }

            if (!pass.Any(char.IsUpper) || !pass.Any(char.IsLower))
            {
                MessageBox.Show("Password must contain both uppercase and lowercase letters.");
                return;
            }

            // Database Login Check
            if (CheckDatabaseLogin(user, pass))
            {
                SessionManager.CurrentUsername = user;
                SessionManager.LoginTime = DateTime.Now;

                MessageBox.Show($"Welcome, {user}!");

                MainDashboard dashboard = new MainDashboard();
                dashboard.Show();
                this.Hide();
            }
            else
            {
                failedAttempts++;
                if (failedAttempts >= 3)
                {
                    btnSignin.Enabled = false;
                    MessageBox.Show("Account Locked! 3 failed attempts reached.");
                }
                else
                {
                    MessageBox.Show($"Invalid Login. Attempts left: {3 - failedAttempts}");
                }
            }
        }

        private void pn_auth_Paint(object sender, PaintEventArgs e)
        {
            // Painting rounded corners for the panel
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            int borderRadius = 30;
            Rectangle rect = pn_auth.ClientRectangle;

            using (GraphicsPath path = GetRoundedPath(rect, borderRadius))
            {
                pn_auth.Region = new Region(path);
                using (Pen pen = new Pen(Color.White, 1))
                {
                    e.Graphics.DrawPath(pen, path);
                }
            }
        }

        private GraphicsPath GetRoundedPath(Rectangle rect, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            float diameter = radius * 2f;
            SizeF size = new SizeF(diameter, diameter);
            RectangleF arc = new RectangleF(rect.Location, size);

            path.AddArc(arc, 180, 90);
            arc.X = rect.Right - diameter;
            path.AddArc(arc, 270, 90);
            arc.Y = rect.Bottom - diameter;
            path.AddArc(arc, 0, 90);
            arc.X = rect.Left;
            path.AddArc(arc, 90, 90);

            path.CloseFigure();
            return path;
        }

        private void chkShow_CheckedChanged(object sender, EventArgs e)
        {
            if (txtPassword.Text != "Enter Password" && !string.IsNullOrEmpty(txtPassword.Text))
            {
                txtPassword.PasswordChar = chkShow.Checked ? '\0' : '*';
            }
        }

        private void lnkSignUp_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Uc_Register uc = new Uc_Register();
            uc.Dock = DockStyle.Fill;
            pn_auth.Controls.Clear();
            pn_auth.Controls.Add(uc);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        // Method to restart the login form (called from Register UC)
        public void ShowLogin()
        {
            Application.Restart();
        }

        private void Auth_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }
    }
}