namespace ALEmanCafe_Server
{
    partial class RemoteApplications
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RemoteApplications));
            this.label3 = new System.Windows.Forms.Label();
            this.AppsView = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ColumnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.TerminateSelected = new System.Windows.Forms.Button();
            this.TerminateAll = new System.Windows.Forms.Button();
            this.CloseButton = new System.Windows.Forms.Button();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.terminateSelectedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.terminateAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.SystemColors.Highlight;
            this.label3.Dock = System.Windows.Forms.DockStyle.Top;
            this.label3.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.label3.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.label3.Location = new System.Drawing.Point(0, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(658, 34);
            this.label3.TabIndex = 13;
            this.label3.Text = "Application List";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // AppsView
            // 
            this.AppsView.BackColor = System.Drawing.SystemColors.Window;
            this.AppsView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.ColumnHeader2,
            this.columnHeader3,
            this.columnHeader4});
            this.AppsView.Font = new System.Drawing.Font("Arial", 10F);
            this.AppsView.FullRowSelect = true;
            this.AppsView.GridLines = true;
            this.AppsView.Location = new System.Drawing.Point(12, 34);
            this.AppsView.MultiSelect = false;
            this.AppsView.Name = "AppsView";
            this.AppsView.ShowItemToolTips = true;
            this.AppsView.Size = new System.Drawing.Size(634, 344);
            this.AppsView.TabIndex = 0;
            this.AppsView.UseCompatibleStateImageBehavior = false;
            this.AppsView.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Application";
            this.columnHeader1.Width = 150;
            // 
            // ColumnHeader2
            // 
            this.ColumnHeader2.Tag = "";
            this.ColumnHeader2.Text = "Title";
            this.ColumnHeader2.Width = 220;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Path";
            this.columnHeader3.Width = 280;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "id";
            this.columnHeader4.Width = 30;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Location = new System.Drawing.Point(0, 410);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(658, 22);
            this.statusStrip1.TabIndex = 15;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.AutoSize = false;
            this.toolStripStatusLabel1.BackColor = System.Drawing.Color.Transparent;
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(200, 17);
            this.toolStripStatusLabel1.Text = "Count : ";
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(11, 17);
            this.toolStripStatusLabel2.Text = "|";
            // 
            // TerminateSelected
            // 
            this.TerminateSelected.BackColor = System.Drawing.SystemColors.Window;
            this.TerminateSelected.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.TerminateSelected.Font = new System.Drawing.Font("Arial", 10F);
            this.TerminateSelected.Location = new System.Drawing.Point(12, 384);
            this.TerminateSelected.Name = "TerminateSelected";
            this.TerminateSelected.Size = new System.Drawing.Size(144, 23);
            this.TerminateSelected.TabIndex = 1;
            this.TerminateSelected.Text = "Terminate &Selected";
            this.TerminateSelected.UseVisualStyleBackColor = false;
            this.TerminateSelected.Click += new System.EventHandler(this.TerminateSelected_Click);
            // 
            // TerminateAll
            // 
            this.TerminateAll.BackColor = System.Drawing.SystemColors.Window;
            this.TerminateAll.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.TerminateAll.Font = new System.Drawing.Font("Arial", 10F);
            this.TerminateAll.Location = new System.Drawing.Point(162, 384);
            this.TerminateAll.Name = "TerminateAll";
            this.TerminateAll.Size = new System.Drawing.Size(144, 23);
            this.TerminateAll.TabIndex = 2;
            this.TerminateAll.Text = "Terminate &All";
            this.TerminateAll.UseVisualStyleBackColor = false;
            this.TerminateAll.Click += new System.EventHandler(this.TerminateAll_Click);
            // 
            // CloseButton
            // 
            this.CloseButton.BackColor = System.Drawing.SystemColors.Window;
            this.CloseButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.CloseButton.Font = new System.Drawing.Font("Arial", 10F);
            this.CloseButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.CloseButton.Location = new System.Drawing.Point(545, 384);
            this.CloseButton.Name = "CloseButton";
            this.CloseButton.Size = new System.Drawing.Size(101, 23);
            this.CloseButton.TabIndex = 3;
            this.CloseButton.Text = "&Close";
            this.CloseButton.UseVisualStyleBackColor = false;
            this.CloseButton.Click += new System.EventHandler(this.CloseButton_Click);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.terminateSelectedToolStripMenuItem,
            this.terminateAllToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.contextMenuStrip1.Size = new System.Drawing.Size(175, 48);
            // 
            // terminateSelectedToolStripMenuItem
            // 
            this.terminateSelectedToolStripMenuItem.Name = "terminateSelectedToolStripMenuItem";
            this.terminateSelectedToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.terminateSelectedToolStripMenuItem.Text = "Terminate &Selected";
            this.terminateSelectedToolStripMenuItem.Click += new System.EventHandler(this.terminateSelectedToolStripMenuItem_Click);
            // 
            // terminateAllToolStripMenuItem
            // 
            this.terminateAllToolStripMenuItem.Name = "terminateAllToolStripMenuItem";
            this.terminateAllToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.terminateAllToolStripMenuItem.Text = "Terminate &All";
            this.terminateAllToolStripMenuItem.Click += new System.EventHandler(this.terminateAllToolStripMenuItem_Click);
            // 
            // RemoteApplications
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(658, 432);
            this.Controls.Add(this.CloseButton);
            this.Controls.Add(this.TerminateAll);
            this.Controls.Add(this.TerminateSelected);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.AppsView);
            this.Controls.Add(this.label3);
            this.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "RemoteApplications";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Remote Applications : ";
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader ColumnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.Button TerminateSelected;
        private System.Windows.Forms.Button TerminateAll;
        private System.Windows.Forms.Button CloseButton;
        public System.Windows.Forms.ListView AppsView;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem terminateSelectedToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem terminateAllToolStripMenuItem;
    }
}