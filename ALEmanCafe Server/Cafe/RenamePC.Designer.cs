namespace ALEmanCafe_Server.Cafe
{
    partial class RenamePC
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RenamePC));
            this.OKButton = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.NewNameTextBox = new System.Windows.Forms.MaskedTextBox();
            this.HostNameButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // OKButton
            // 
            this.OKButton.Image = ((System.Drawing.Image)(resources.GetObject("OKButton.Image")));
            this.OKButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.OKButton.Location = new System.Drawing.Point(190, 46);
            this.OKButton.Name = "OKButton";
            this.OKButton.Size = new System.Drawing.Size(75, 25);
            this.OKButton.TabIndex = 14;
            this.OKButton.Text = "    &OK";
            this.OKButton.UseVisualStyleBackColor = true;
            this.OKButton.Click += new System.EventHandler(this.OKButton_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(68, 13);
            this.label3.TabIndex = 15;
            this.label3.Text = "New Name : ";
            // 
            // NewNameTextBox
            // 
            this.NewNameTextBox.Culture = new System.Globalization.CultureInfo("en-US");
            this.NewNameTextBox.Location = new System.Drawing.Point(86, 6);
            this.NewNameTextBox.Name = "NewNameTextBox";
            this.NewNameTextBox.Size = new System.Drawing.Size(179, 20);
            this.NewNameTextBox.TabIndex = 13;
            this.NewNameTextBox.Text = "ALEman0";
            // 
            // HostNameButton
            // 
            this.HostNameButton.Location = new System.Drawing.Point(15, 46);
            this.HostNameButton.Name = "HostNameButton";
            this.HostNameButton.Size = new System.Drawing.Size(93, 23);
            this.HostNameButton.TabIndex = 16;
            this.HostNameButton.Text = "Get HostName";
            this.HostNameButton.UseVisualStyleBackColor = true;
            this.HostNameButton.Click += new System.EventHandler(this.HostNameButton_Click);
            // 
            // RenamePC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 77);
            this.Controls.Add(this.HostNameButton);
            this.Controls.Add(this.OKButton);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.NewNameTextBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "RenamePC";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Rename PC";
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button OKButton;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.MaskedTextBox NewNameTextBox;
        private System.Windows.Forms.Button HostNameButton;
    }
}