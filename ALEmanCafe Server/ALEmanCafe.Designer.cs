namespace ALEmanCafe_Server
{
    partial class ALEmanCafe
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ALEmanCafe));
            this.label3 = new System.Windows.Forms.Label();
            this.PassTextBox = new System.Windows.Forms.MaskedTextBox();
            this.LoginButton = new System.Windows.Forms.Button();
            this.CancelsButton = new System.Windows.Forms.Button();
            this.UserTextBox = new System.Windows.Forms.MaskedTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // PassTextBox
            // 
            this.PassTextBox.Culture = new System.Globalization.CultureInfo("en-US");
            resources.ApplyResources(this.PassTextBox, "PassTextBox");
            this.PassTextBox.Name = "PassTextBox";
            // 
            // LoginButton
            // 
            this.LoginButton.BackColor = System.Drawing.SystemColors.ButtonFace;
            resources.ApplyResources(this.LoginButton, "LoginButton");
            this.LoginButton.Name = "LoginButton";
            this.LoginButton.UseVisualStyleBackColor = false;
            this.LoginButton.Click += new System.EventHandler(this.LoginButton_Click);
            // 
            // CancelsButton
            // 
            this.CancelsButton.BackColor = System.Drawing.SystemColors.ButtonFace;
            resources.ApplyResources(this.CancelsButton, "CancelsButton");
            this.CancelsButton.Name = "CancelsButton";
            this.CancelsButton.UseVisualStyleBackColor = false;
            this.CancelsButton.Click += new System.EventHandler(this.CancelsButton_Click);
            // 
            // UserTextBox
            // 
            this.UserTextBox.Culture = new System.Globalization.CultureInfo("en-US");
            resources.ApplyResources(this.UserTextBox, "UserTextBox");
            this.UserTextBox.Name = "UserTextBox";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // ALEmanCafe
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.Controls.Add(this.CancelsButton);
            this.Controls.Add(this.LoginButton);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.PassTextBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.UserTextBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ALEmanCafe";
            this.ShowInTaskbar = false;
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.MaskedTextBox PassTextBox;
        private System.Windows.Forms.Button LoginButton;
        private System.Windows.Forms.Button CancelsButton;
        private System.Windows.Forms.MaskedTextBox UserTextBox;
        private System.Windows.Forms.Label label2;
    }
}

