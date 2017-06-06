namespace ALEmanCafe_Server
{
    partial class SystemInofrmation
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
            this.listView1 = new System.Windows.Forms.ListView();
            this.Hostcolumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Usercolumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.IPcolumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Maccolumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.OScolumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.TotalMemorycolumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.FreeMemorycolumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Harddiskcolumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.OKButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // listView1
            // 
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Hostcolumn,
            this.Usercolumn,
            this.IPcolumn,
            this.Maccolumn,
            this.OScolumn,
            this.TotalMemorycolumn,
            this.FreeMemorycolumn,
            this.Harddiskcolumn});
            this.listView1.Dock = System.Windows.Forms.DockStyle.Top;
            this.listView1.FullRowSelect = true;
            this.listView1.GridLines = true;
            this.listView1.Location = new System.Drawing.Point(0, 0);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(737, 475);
            this.listView1.TabIndex = 0;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            // 
            // Hostcolumn
            // 
            this.Hostcolumn.Text = "Host";
            this.Hostcolumn.Width = 100;
            // 
            // Usercolumn
            // 
            this.Usercolumn.Text = "User";
            this.Usercolumn.Width = 100;
            // 
            // IPcolumn
            // 
            this.IPcolumn.Text = "IP";
            this.IPcolumn.Width = 120;
            // 
            // Maccolumn
            // 
            this.Maccolumn.Text = "Mac";
            this.Maccolumn.Width = 120;
            // 
            // OScolumn
            // 
            this.OScolumn.Text = "OS";
            this.OScolumn.Width = 250;
            // 
            // TotalMemorycolumn
            // 
            this.TotalMemorycolumn.Text = "Total Memory";
            this.TotalMemorycolumn.Width = 120;
            // 
            // FreeMemorycolumn
            // 
            this.FreeMemorycolumn.Text = "Free Memory";
            this.FreeMemorycolumn.Width = 120;
            // 
            // Harddiskcolumn
            // 
            this.Harddiskcolumn.Text = "Hard disk";
            this.Harddiskcolumn.Width = 120;
            // 
            // OKButton
            // 
            this.OKButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.OKButton.Image = global::ALEmanCafe_Server.Properties.Resources.TrueIcon;
            this.OKButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.OKButton.Location = new System.Drawing.Point(620, 481);
            this.OKButton.Name = "OKButton";
            this.OKButton.Size = new System.Drawing.Size(105, 23);
            this.OKButton.TabIndex = 1;
            this.OKButton.Text = "&OK";
            this.OKButton.UseVisualStyleBackColor = true;
            this.OKButton.Click += new System.EventHandler(this.OKButton_Click);
            // 
            // SystemInofrmation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(737, 516);
            this.Controls.Add(this.OKButton);
            this.Controls.Add(this.listView1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "SystemInofrmation";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "System Inofrmation";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader Hostcolumn;
        private System.Windows.Forms.ColumnHeader Usercolumn;
        private System.Windows.Forms.ColumnHeader IPcolumn;
        private System.Windows.Forms.ColumnHeader Maccolumn;
        private System.Windows.Forms.ColumnHeader OScolumn;
        private System.Windows.Forms.ColumnHeader TotalMemorycolumn;
        private System.Windows.Forms.ColumnHeader FreeMemorycolumn;
        private System.Windows.Forms.ColumnHeader Harddiskcolumn;
        private System.Windows.Forms.Button OKButton;
    }
}