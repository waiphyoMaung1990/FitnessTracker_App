namespace FitnessTrackerApp
{
    partial class MainDashboard
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            pn_Header = new Panel();
            pn_main = new Panel();
            SuspendLayout();
            // 
            // pn_Header
            // 
            pn_Header.BackColor = Color.CornflowerBlue;
            pn_Header.Dock = DockStyle.Top;
            pn_Header.Location = new Point(0, 0);
            pn_Header.Name = "pn_Header";
            pn_Header.Size = new Size(862, 40);
            pn_Header.TabIndex = 2;
            // 
            // pn_main
            // 
            pn_main.Dock = DockStyle.Fill;
            pn_main.Location = new Point(0, 40);
            pn_main.Name = "pn_main";
            pn_main.Size = new Size(862, 436);
            pn_main.TabIndex = 3;
            // 
            // MainDashboard
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(862, 476);
            Controls.Add(pn_main);
            Controls.Add(pn_Header);
            Name = "MainDashboard";
            Text = "MainDashboard";
            ResumeLayout(false);
        }

        #endregion

        private Panel pn_SideMenu;
        private Panel pn_Header;
        private Panel pn_main;
    }
}