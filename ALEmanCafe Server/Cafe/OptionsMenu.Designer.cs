namespace ALEmanCafe_Server
{
    partial class OptionsMenu
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
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("General");
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("General", new System.Windows.Forms.TreeNode[] {
            treeNode1});
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OptionsMenu));
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.MinimumCost = new System.Windows.Forms.MaskedTextBox();
            this.HourPrice = new System.Windows.Forms.MaskedTextBox();
            this.VersionLabel = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.Askpasswordonstartup = new System.Windows.Forms.CheckBox();
            this.Runserveratstartup = new System.Windows.Forms.CheckBox();
            this.ShowBox = new System.Windows.Forms.CheckBox();
            this.PassTextBox = new System.Windows.Forms.MaskedTextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.UserTextBox = new System.Windows.Forms.MaskedTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.EnableUSBPlugoutWarning = new System.Windows.Forms.CheckBox();
            this.EnableUSBPluginWarning = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.CancelCloseButton = new System.Windows.Forms.Button();
            this.OKButton = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // treeView1
            // 
            this.treeView1.Dock = System.Windows.Forms.DockStyle.Left;
            this.treeView1.FullRowSelect = true;
            this.treeView1.HotTracking = true;
            this.treeView1.Location = new System.Drawing.Point(0, 0);
            this.treeView1.Name = "treeView1";
            treeNode1.Name = "Options";
            treeNode1.Text = "General";
            treeNode2.Name = "General";
            treeNode2.Text = "General";
            this.treeView1.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode2});
            this.treeView1.ShowNodeToolTips = true;
            this.treeView1.Size = new System.Drawing.Size(134, 383);
            this.treeView1.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.CancelCloseButton);
            this.panel1.Controls.Add(this.OKButton);
            this.panel1.Controls.Add(this.groupBox3);
            this.panel1.Controls.Add(this.VersionLabel);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.groupBox2);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(134, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(364, 383);
            this.panel1.TabIndex = 1;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.MinimumCost);
            this.groupBox3.Controls.Add(this.HourPrice);
            this.groupBox3.Location = new System.Drawing.Point(9, 205);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(291, 63);
            this.groupBox3.TabIndex = 11;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Pricing";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Enabled = false;
            this.label6.Location = new System.Drawing.Point(152, 21);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(72, 13);
            this.label6.TabIndex = 12;
            this.label6.Text = "Minimum Cost";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Enabled = false;
            this.label5.Location = new System.Drawing.Point(5, 21);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(56, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = "Hour Price";
            // 
            // MinimumCost
            // 
            this.MinimumCost.Location = new System.Drawing.Point(155, 37);
            this.MinimumCost.Name = "MinimumCost";
            this.MinimumCost.Size = new System.Drawing.Size(130, 20);
            this.MinimumCost.TabIndex = 10;
            this.MinimumCost.Text = "0.50";
            // 
            // HourPrice
            // 
            this.HourPrice.Location = new System.Drawing.Point(8, 37);
            this.HourPrice.Name = "HourPrice";
            this.HourPrice.Size = new System.Drawing.Size(130, 20);
            this.HourPrice.TabIndex = 9;
            this.HourPrice.Text = "1.50";
            // 
            // VersionLabel
            // 
            this.VersionLabel.AutoSize = true;
            this.VersionLabel.Enabled = false;
            this.VersionLabel.Location = new System.Drawing.Point(61, 361);
            this.VersionLabel.Name = "VersionLabel";
            this.VersionLabel.Size = new System.Drawing.Size(0, 13);
            this.VersionLabel.TabIndex = 10;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Enabled = false;
            this.label4.Location = new System.Drawing.Point(3, 361);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(52, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Version : ";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.Askpasswordonstartup);
            this.groupBox2.Controls.Add(this.Runserveratstartup);
            this.groupBox2.Controls.Add(this.ShowBox);
            this.groupBox2.Controls.Add(this.PassTextBox);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.UserTextBox);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Location = new System.Drawing.Point(9, 45);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(343, 154);
            this.groupBox2.TabIndex = 8;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "General";
            // 
            // Askpasswordonstartup
            // 
            this.Askpasswordonstartup.AutoSize = true;
            this.Askpasswordonstartup.Checked = true;
            this.Askpasswordonstartup.CheckState = System.Windows.Forms.CheckState.Checked;
            this.Askpasswordonstartup.Location = new System.Drawing.Point(8, 131);
            this.Askpasswordonstartup.Name = "Askpasswordonstartup";
            this.Askpasswordonstartup.Size = new System.Drawing.Size(145, 17);
            this.Askpasswordonstartup.TabIndex = 13;
            this.Askpasswordonstartup.Text = "Ask password on startup";
            this.Askpasswordonstartup.UseVisualStyleBackColor = true;
            // 
            // Runserveratstartup
            // 
            this.Runserveratstartup.AutoSize = true;
            this.Runserveratstartup.Location = new System.Drawing.Point(8, 108);
            this.Runserveratstartup.Name = "Runserveratstartup";
            this.Runserveratstartup.Size = new System.Drawing.Size(130, 17);
            this.Runserveratstartup.TabIndex = 12;
            this.Runserveratstartup.Text = "Run server at startup";
            this.Runserveratstartup.UseVisualStyleBackColor = true;
            // 
            // ShowBox
            // 
            this.ShowBox.AutoSize = true;
            this.ShowBox.Location = new System.Drawing.Point(214, 84);
            this.ShowBox.Name = "ShowBox";
            this.ShowBox.Size = new System.Drawing.Size(52, 17);
            this.ShowBox.TabIndex = 11;
            this.ShowBox.Text = "Show";
            this.ShowBox.UseVisualStyleBackColor = true;
            // 
            // PassTextBox
            // 
            this.PassTextBox.Location = new System.Drawing.Point(8, 82);
            this.PassTextBox.Name = "PassTextBox";
            this.PassTextBox.Size = new System.Drawing.Size(200, 20);
            this.PassTextBox.TabIndex = 10;
            this.PassTextBox.UseSystemPasswordChar = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(5, 66);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(85, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "Admin Password";
            // 
            // UserTextBox
            // 
            this.UserTextBox.Location = new System.Drawing.Point(8, 37);
            this.UserTextBox.Name = "UserTextBox";
            this.UserTextBox.Size = new System.Drawing.Size(200, 20);
            this.UserTextBox.TabIndex = 8;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(5, 21);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(87, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Admin Username";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.EnableUSBPlugoutWarning);
            this.groupBox1.Controls.Add(this.EnableUSBPluginWarning);
            this.groupBox1.Location = new System.Drawing.Point(9, 274);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(200, 63);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Security";
            // 
            // EnableUSBPlugoutWarning
            // 
            this.EnableUSBPlugoutWarning.AutoSize = true;
            this.EnableUSBPlugoutWarning.Location = new System.Drawing.Point(5, 39);
            this.EnableUSBPlugoutWarning.Name = "EnableUSBPlugoutWarning";
            this.EnableUSBPlugoutWarning.Size = new System.Drawing.Size(166, 17);
            this.EnableUSBPlugoutWarning.TabIndex = 1;
            this.EnableUSBPlugoutWarning.Text = "Enable USB Plug-out Warning";
            this.EnableUSBPlugoutWarning.UseVisualStyleBackColor = true;
            // 
            // EnableUSBPluginWarning
            // 
            this.EnableUSBPluginWarning.AutoSize = true;
            this.EnableUSBPluginWarning.Location = new System.Drawing.Point(5, 16);
            this.EnableUSBPluginWarning.Name = "EnableUSBPluginWarning";
            this.EnableUSBPluginWarning.Size = new System.Drawing.Size(158, 17);
            this.EnableUSBPluginWarning.TabIndex = 0;
            this.EnableUSBPluginWarning.Text = "Enable USB Plug-in Warning";
            this.EnableUSBPluginWarning.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label1.Location = new System.Drawing.Point(6, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(346, 33);
            this.label1.TabIndex = 0;
            this.label1.Text = "Options";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // CancelCloseButton
            // 
            this.CancelCloseButton.Image = global::ALEmanCafe_Server.Properties.Resources.FalseIcon;
            this.CancelCloseButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.CancelCloseButton.Location = new System.Drawing.Point(277, 349);
            this.CancelCloseButton.Name = "CancelCloseButton";
            this.CancelCloseButton.Size = new System.Drawing.Size(75, 25);
            this.CancelCloseButton.TabIndex = 13;
            this.CancelCloseButton.Text = "    &Cancel";
            this.CancelCloseButton.UseVisualStyleBackColor = true;
            this.CancelCloseButton.Click += new System.EventHandler(this.CancelCloseButton_Click);
            // 
            // OKButton
            // 
            this.OKButton.Image = ((System.Drawing.Image)(resources.GetObject("OKButton.Image")));
            this.OKButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.OKButton.Location = new System.Drawing.Point(186, 349);
            this.OKButton.Name = "OKButton";
            this.OKButton.Size = new System.Drawing.Size(75, 25);
            this.OKButton.TabIndex = 12;
            this.OKButton.Text = "    &OK";
            this.OKButton.UseVisualStyleBackColor = true;
            this.OKButton.Click += new System.EventHandler(this.OKButton_Click);
            // 
            // OptionsMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(498, 383);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.treeView1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OptionsMenu";
            this.ShowInTaskbar = false;
            this.Text = "Options";
            this.TopMost = true;
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox EnableUSBPlugoutWarning;
        private System.Windows.Forms.CheckBox EnableUSBPluginWarning;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox ShowBox;
        private System.Windows.Forms.MaskedTextBox PassTextBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.MaskedTextBox UserTextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label VersionLabel;
        private System.Windows.Forms.CheckBox Askpasswordonstartup;
        private System.Windows.Forms.CheckBox Runserveratstartup;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.MaskedTextBox MinimumCost;
        private System.Windows.Forms.MaskedTextBox HourPrice;
        private System.Windows.Forms.Button CancelCloseButton;
        private System.Windows.Forms.Button OKButton;
    }
}