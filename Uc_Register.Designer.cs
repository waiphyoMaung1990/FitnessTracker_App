namespace FitnessTrackerApp
{
    partial class Uc_Register
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            label2 = new Label();
            label1 = new Label();
            btnSignUp = new Button();
            txtRegPass = new TextBox();
            txtRegUser = new TextBox();
            lb_Register = new Label();
            txtConfirmPass = new TextBox();
            label3 = new Label();
            chkShow = new CheckBox();
            lnkLogin = new LinkLabel();
            SuspendLayout();
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.FlatStyle = FlatStyle.System;
            label2.Font = new Font("Times New Roman", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label2.Location = new Point(65, 112);
            label2.Name = "label2";
            label2.Size = new Size(54, 15);
            label2.TabIndex = 11;
            label2.Text = "Password";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.FlatStyle = FlatStyle.System;
            label1.Font = new Font("Times New Roman", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label1.Location = new Point(65, 53);
            label1.Name = "label1";
            label1.Size = new Size(61, 15);
            label1.TabIndex = 10;
            label1.Text = "User Name";
            // 
            // btnSignUp
            // 
            btnSignUp.BackColor = Color.FromArgb(0, 133, 168);
            btnSignUp.Font = new Font("Times New Roman", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnSignUp.ForeColor = Color.White;
            btnSignUp.Location = new Point(65, 261);
            btnSignUp.Name = "btnSignUp";
            btnSignUp.Size = new Size(216, 47);
            btnSignUp.TabIndex = 9;
            btnSignUp.Text = "SIGN UP";
            btnSignUp.UseVisualStyleBackColor = false;
            btnSignUp.Click += btnSignUp_Click;
            // 
            // txtRegPass
            // 
            txtRegPass.Location = new Point(65, 133);
            txtRegPass.Multiline = true;
            txtRegPass.Name = "txtRegPass";
            txtRegPass.Size = new Size(216, 35);
            txtRegPass.TabIndex = 8;
            // 
            // txtRegUser
            // 
            txtRegUser.Location = new Point(65, 74);
            txtRegUser.Multiline = true;
            txtRegUser.Name = "txtRegUser";
            txtRegUser.Size = new Size(216, 35);
            txtRegUser.TabIndex = 7;
            // 
            // lb_Register
            // 
            lb_Register.AutoSize = true;
            lb_Register.Font = new Font("Times New Roman", 16F);
            lb_Register.ForeColor = Color.FromArgb(86, 87, 89);
            lb_Register.Location = new Point(143, 29);
            lb_Register.Name = "lb_Register";
            lb_Register.Size = new Size(84, 25);
            lb_Register.TabIndex = 6;
            lb_Register.Text = "Register";
            // 
            // txtConfirmPass
            // 
            txtConfirmPass.Location = new Point(65, 195);
            txtConfirmPass.Multiline = true;
            txtConfirmPass.Name = "txtConfirmPass";
            txtConfirmPass.Size = new Size(216, 35);
            txtConfirmPass.TabIndex = 12;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.FlatStyle = FlatStyle.System;
            label3.Font = new Font("Times New Roman", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label3.Location = new Point(65, 174);
            label3.Name = "label3";
            label3.Size = new Size(97, 15);
            label3.TabIndex = 13;
            label3.Text = "Confirm Password";
            // 
            // chkShow
            // 
            chkShow.AutoSize = true;
            chkShow.Location = new Point(65, 235);
            chkShow.Name = "chkShow";
            chkShow.Size = new Size(108, 19);
            chkShow.TabIndex = 14;
            chkShow.Text = "Show Password";
            chkShow.UseVisualStyleBackColor = true;
            // 
            // lnkLogin
            // 
            lnkLogin.AutoSize = true;
            lnkLogin.Location = new Point(86, 322);
            lnkLogin.Name = "lnkLogin";
            lnkLogin.Size = new Size(175, 15);
            lnkLogin.TabIndex = 15;
            lnkLogin.TabStop = true;
            lnkLogin.Text = "Already have an account? Login";
            lnkLogin.LinkClicked += lnkLogin_LinkClicked;
            // 
            // Uc_Register
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(lnkLogin);
            Controls.Add(chkShow);
            Controls.Add(label3);
            Controls.Add(txtConfirmPass);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(btnSignUp);
            Controls.Add(txtRegPass);
            Controls.Add(txtRegUser);
            Controls.Add(lb_Register);
            Name = "Uc_Register";
            Size = new Size(346, 357);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label2;
        private Label label1;
        private Button btnSignUp;
        private TextBox txtRegPass;
        private TextBox txtRegUser;
        private Label lb_Register;
        private TextBox txtConfirmPass;
        private Label label3;
        private CheckBox chkShow;
        private LinkLabel lnkLogin;
    }
}
