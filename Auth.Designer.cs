namespace FitnessTrackerApp
{
    partial class Auth
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            pn_home = new Panel();
            pn_auth = new Panel();
            btnClose = new Button();
            lnkSignUp = new LinkLabel();
            chkShow = new CheckBox();
            label2 = new Label();
            label1 = new Label();
            btnSignin = new Button();
            txtPassword = new TextBox();
            txtUsername = new TextBox();
            lb_login = new Label();
            pn_home.SuspendLayout();
            pn_auth.SuspendLayout();
            SuspendLayout();
            // 
            // pn_home
            // 
            pn_home.BackColor = Color.FromArgb(0, 133, 168);
            pn_home.Controls.Add(btnClose);
            pn_home.Controls.Add(pn_auth);
            pn_home.Dock = DockStyle.Fill;
            pn_home.Location = new Point(0, 0);
            pn_home.Name = "pn_home";
            pn_home.Size = new Size(618, 470);
            pn_home.TabIndex = 0;
            // 
            // pn_auth
            // 
            pn_auth.BackColor = Color.White;
            pn_auth.Controls.Add(lnkSignUp);
            pn_auth.Controls.Add(chkShow);
            pn_auth.Controls.Add(label2);
            pn_auth.Controls.Add(label1);
            pn_auth.Controls.Add(btnSignin);
            pn_auth.Controls.Add(txtPassword);
            pn_auth.Controls.Add(txtUsername);
            pn_auth.Controls.Add(lb_login);
            pn_auth.Location = new Point(137, 61);
            pn_auth.Name = "pn_auth";
            pn_auth.Size = new Size(346, 357);
            pn_auth.TabIndex = 0;
            // 
            // btnClose
            // 
            btnClose.BackColor = Color.White;
            btnClose.FlatStyle = FlatStyle.Flat;
            btnClose.ForeColor = Color.Blue;
            btnClose.Location = new Point(579, 3);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(36, 30);
            btnClose.TabIndex = 8;
            btnClose.Text = "x";
            btnClose.UseVisualStyleBackColor = false;
            btnClose.Click += btnClose_Click;
            // 
            // lnkSignUp
            // 
            lnkSignUp.AutoSize = true;
            lnkSignUp.Location = new Point(91, 311);
            lnkSignUp.Name = "lnkSignUp";
            lnkSignUp.Size = new Size(175, 15);
            lnkSignUp.TabIndex = 7;
            lnkSignUp.TabStop = true;
            lnkSignUp.Text = "Don't have an account? Sign Up";
            lnkSignUp.LinkClicked += lnkSignUp_LinkClicked;
            // 
            // chkShow
            // 
            chkShow.AutoSize = true;
            chkShow.Location = new Point(70, 218);
            chkShow.Name = "chkShow";
            chkShow.Size = new Size(108, 19);
            chkShow.TabIndex = 6;
            chkShow.Text = "Show Password";
            chkShow.UseVisualStyleBackColor = true;
            chkShow.CheckedChanged += chkShow_CheckedChanged;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.FlatStyle = FlatStyle.System;
            label2.Font = new Font("Times New Roman", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label2.Location = new Point(70, 148);
            label2.Name = "label2";
            label2.Size = new Size(54, 15);
            label2.TabIndex = 5;
            label2.Text = "Password";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.FlatStyle = FlatStyle.System;
            label1.Font = new Font("Times New Roman", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label1.Location = new Point(70, 79);
            label1.Name = "label1";
            label1.Size = new Size(61, 15);
            label1.TabIndex = 4;
            label1.Text = "User Name";
            // 
            // btnSignin
            // 
            btnSignin.BackColor = Color.FromArgb(0, 133, 168);
            btnSignin.Font = new Font("Times New Roman", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnSignin.ForeColor = Color.White;
            btnSignin.Location = new Point(70, 249);
            btnSignin.Name = "btnSignin";
            btnSignin.Size = new Size(216, 47);
            btnSignin.TabIndex = 3;
            btnSignin.Text = "SIGN IN";
            btnSignin.UseVisualStyleBackColor = false;
            btnSignin.Click += btnSignin_Click;
            // 
            // txtPassword
            // 
            txtPassword.Location = new Point(70, 174);
            txtPassword.Multiline = true;
            txtPassword.Name = "txtPassword";
            txtPassword.Size = new Size(216, 35);
            txtPassword.TabIndex = 2;
            // 
            // txtUsername
            // 
            txtUsername.Location = new Point(70, 104);
            txtUsername.Multiline = true;
            txtUsername.Name = "txtUsername";
            txtUsername.Size = new Size(216, 35);
            txtUsername.TabIndex = 1;
            // 
            // lb_login
            // 
            lb_login.AutoSize = true;
            lb_login.Font = new Font("Times New Roman", 16F);
            lb_login.ForeColor = Color.FromArgb(86, 87, 89);
            lb_login.Location = new Point(148, 30);
            lb_login.Name = "lb_login";
            lb_login.Size = new Size(61, 25);
            lb_login.TabIndex = 0;
            lb_login.Text = "Login";
            // 
            // Auth
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(618, 470);
            Controls.Add(pn_home);
            FormBorderStyle = FormBorderStyle.None;
            Name = "Auth";
            Text = "Form1";
            FormClosing += Auth_FormClosing;
            pn_home.ResumeLayout(false);
            pn_auth.ResumeLayout(false);
            pn_auth.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel pn_home;
        private Panel pn_auth;
        private Label lb_login;
        private TextBox txtPassword;
        private TextBox txtUsername;
        private Button btnSignin;
        private Label label2;
        private Label label1;
        private CheckBox chkShow;
        private LinkLabel lnkSignUp;
        private Button btnClose;
    }
}
