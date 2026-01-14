using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Data.OleDb;

namespace FitnessTrackerApp
{
    public partial class Uc_Register : UserControl
    {
      
        // အဟောင်းကို ဖျက်ပြီး ဒါနဲ့အစားထိုးပါ
        private string connString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=FitnessTracker.accdb;OLE DB Services=-1;";
        public Uc_Register()
        {
            InitializeComponent();
            SetPlaceholder(txtRegUser, "Enter Username");
            SetPlaceholder(txtRegPass, "Enter Password", true);
            SetPlaceholder(txtConfirmPass, "Confirm Password", true);
            chkShow.CheckedChanged += chkShow_CheckedChanged;
        }

        private void SetPlaceholder(TextBox textBox, string placeholder, bool isPassword = false)
        {
            textBox.TextAlign = HorizontalAlignment.Center;
            textBox.Text = placeholder;
            textBox.ForeColor = Color.Gray;
            if (isPassword) textBox.PasswordChar = '\0';

            textBox.Enter += (s, e) =>
            {
                if (textBox.Text == placeholder)
                {
                    textBox.Text = "";
                    textBox.ForeColor = Color.Black;
                    if (isPassword) textBox.PasswordChar = chkShow.Checked ? '\0' : '*';
                }
            };

            textBox.Leave += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(textBox.Text))
                {
                    textBox.Text = placeholder;
                    textBox.ForeColor = Color.Gray;
                    if (isPassword) textBox.PasswordChar = '\0';
                }
            };
        }

        private void chkShow_CheckedChanged(object sender, EventArgs e)
        {
            char passChar = chkShow.Checked ? '\0' : '*';
            if (txtRegPass.Text != "Enter Password" && !string.IsNullOrEmpty(txtRegPass.Text))
                txtRegPass.PasswordChar = passChar;
            if (txtConfirmPass.Text != "Confirm Password" && !string.IsNullOrEmpty(txtConfirmPass.Text))
                txtConfirmPass.PasswordChar = passChar;
        }

        private void btnSignUp_Click(object sender, EventArgs e)
        {
            string user = txtRegUser.Text.Trim();
            string pass = txtRegPass.Text;
            string confirmPass = txtConfirmPass.Text;

            if (user == "Enter Username" || pass == "Enter Password")
            {
                MessageBox.Show("Please fill in all fields.");
                return;
            }

            if (pass != confirmPass)
            {
                MessageBox.Show("Passwords do not match!");
                return;
            }

            if (pass.Length != 12 || !pass.Any(char.IsUpper) || !pass.Any(char.IsLower) || !pass.Any(char.IsDigit))
            {
                MessageBox.Show("Password must be 12 chars with Uppercase, Lowercase, and Number.");
                return;
            }

            try
            {
                using (OleDbConnection conn = new OleDbConnection(connString))
                {
                    conn.Open();
                    // Check if username exists
                    string checkSql = "SELECT COUNT(*) FROM Users WHERE Username = ?";
                    OleDbCommand checkCmd = new OleDbCommand(checkSql, conn);
                    checkCmd.Parameters.AddWithValue("?", user);
                    if (Convert.ToInt32(checkCmd.ExecuteScalar()) > 0)
                    {
                        MessageBox.Show("Username already taken!");
                        return;
                    }

                    // Insert User
                    string sql = "INSERT INTO Users (Username, [Password]) VALUES (?, ?)";
                    OleDbCommand cmd = new OleDbCommand(sql, conn);
                    cmd.Parameters.AddWithValue("?", user);
                    cmd.Parameters.AddWithValue("?", pass);
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Registration Successful! Please Login.");

                    // Return to Login screen
                    if (this.ParentForm is Auth loginForm)
                    {
                        loginForm.ShowLogin();
                    }
                    else
                    {
                        Application.Restart();
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show("Error: " + ex.Message); }
        }

        private void lnkLogin_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Application.Restart();
        }
    }
}