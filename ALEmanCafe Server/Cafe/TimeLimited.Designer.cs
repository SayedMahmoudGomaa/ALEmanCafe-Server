namespace ALEmanCafe_Server
{
    partial class TimeLimited
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
            this.CancelCloseButton = new System.Windows.Forms.Button();
            this.OKButton = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.TimeTextBox = new System.Windows.Forms.MaskedTextBox();
            this.maskedTextBox1 = new System.Windows.Forms.MaskedTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.PaidCheckBox = new System.Windows.Forms.CheckBox();
            this.GameCheckBox = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // CancelCloseButton
            // 
            this.CancelCloseButton.Image = global::ALEmanCafe_Server.Properties.Resources.FalseIcon;
            this.CancelCloseButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.CancelCloseButton.Location = new System.Drawing.Point(228, 174);
            this.CancelCloseButton.Name = "CancelCloseButton";
            this.CancelCloseButton.Size = new System.Drawing.Size(75, 25);
            this.CancelCloseButton.TabIndex = 2;
            this.CancelCloseButton.Text = "    &Cancel";
            this.CancelCloseButton.UseVisualStyleBackColor = true;
            this.CancelCloseButton.Click += new System.EventHandler(this.CancelCloseButton_Click);
            // 
            // OKButton
            // 
            this.OKButton.Image = global::ALEmanCafe_Server.Properties.Resources.TrueIcon;
            this.OKButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.OKButton.Location = new System.Drawing.Point(137, 174);
            this.OKButton.Name = "OKButton";
            this.OKButton.Size = new System.Drawing.Size(75, 25);
            this.OKButton.TabIndex = 1;
            this.OKButton.Text = "    &OK";
            this.OKButton.UseVisualStyleBackColor = true;
            this.OKButton.Click += new System.EventHandler(this.OKButton_Click);
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.SystemColors.Highlight;
            this.label3.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.label3.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.label3.Location = new System.Drawing.Point(-2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(318, 46);
            this.label3.TabIndex = 12;
            this.label3.Text = "Play and Pay";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // TimeTextBox
            // 
            this.TimeTextBox.BackColor = System.Drawing.SystemColors.Control;
            this.TimeTextBox.Culture = new System.Globalization.CultureInfo("");
            this.TimeTextBox.CutCopyMaskFormat = System.Windows.Forms.MaskFormat.ExcludePromptAndLiterals;
            this.TimeTextBox.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.TimeTextBox.ForeColor = System.Drawing.SystemColors.ControlText;
            this.TimeTextBox.Location = new System.Drawing.Point(137, 74);
            this.TimeTextBox.Name = "TimeTextBox";
            this.TimeTextBox.Size = new System.Drawing.Size(166, 24);
            this.TimeTextBox.TabIndex = 0;
            // 
            // maskedTextBox1
            // 
            this.maskedTextBox1.BackColor = System.Drawing.SystemColors.HotTrack;
            this.maskedTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.maskedTextBox1.Enabled = false;
            this.maskedTextBox1.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.maskedTextBox1.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.maskedTextBox1.Location = new System.Drawing.Point(137, 120);
            this.maskedTextBox1.Name = "maskedTextBox1";
            this.maskedTextBox1.ReadOnly = true;
            this.maskedTextBox1.Size = new System.Drawing.Size(166, 17);
            this.maskedTextBox1.TabIndex = 11;
            this.maskedTextBox1.Text = "00:00 جم";
            this.maskedTextBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label2
            // 
            this.label2.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.label2.Location = new System.Drawing.Point(30, 122);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(101, 13);
            this.label2.TabIndex = 10;
            this.label2.Text = "Cost : ";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(30, 79);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(101, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Enter Time Rights : ";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // PaidCheckBox
            // 
            this.PaidCheckBox.AutoSize = true;
            this.PaidCheckBox.Checked = true;
            this.PaidCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.PaidCheckBox.Location = new System.Drawing.Point(137, 151);
            this.PaidCheckBox.Name = "PaidCheckBox";
            this.PaidCheckBox.Size = new System.Drawing.Size(46, 17);
            this.PaidCheckBox.TabIndex = 13;
            this.PaidCheckBox.Text = "&Paid";
            this.PaidCheckBox.UseVisualStyleBackColor = true;
            this.PaidCheckBox.Visible = false;
            // 
            // GameCheckBox
            // 
            this.GameCheckBox.AutoSize = true;
            this.GameCheckBox.Location = new System.Drawing.Point(228, 151);
            this.GameCheckBox.Name = "GameCheckBox";
            this.GameCheckBox.Size = new System.Drawing.Size(53, 17);
            this.GameCheckBox.TabIndex = 14;
            this.GameCheckBox.Text = "&Game";
            this.GameCheckBox.UseVisualStyleBackColor = true;
            this.GameCheckBox.Visible = false;
            // 
            // TimeLimited
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(314, 212);
            this.Controls.Add(this.GameCheckBox);
            this.Controls.Add(this.PaidCheckBox);
            this.Controls.Add(this.CancelCloseButton);
            this.Controls.Add(this.OKButton);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.TimeTextBox);
            this.Controls.Add(this.maskedTextBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TimeLimited";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Time Limited : ";
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button CancelCloseButton;
        private System.Windows.Forms.Button OKButton;
        private System.Windows.Forms.MaskedTextBox maskedTextBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.Label label3;
        public System.Windows.Forms.CheckBox PaidCheckBox;
        public System.Windows.Forms.CheckBox GameCheckBox;
        public System.Windows.Forms.MaskedTextBox TimeTextBox;
    }
}