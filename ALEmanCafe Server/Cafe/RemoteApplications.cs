using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace ALEmanCafe_Server
{
    public partial class RemoteApplications : Form
    {
        public class AppInfo
        {
            public Icon AppICon = null;
            public uint APPID;
            public string AppTitle, AppPath, AppName;
        }
        public ALEmanCafeServer ACS;
        public NetworkItems NI;
        public List<AppInfo> AIL;
        public RemoteApplications(ALEmanCafeServer ACS, List<AppInfo> AIL, NetworkItems NI)
        {
            InitializeComponent();
            this.ACS = ACS;
            this.AIL = AIL;
            this.NI = NI;
            //listView1.SmallImageList = new ImageList();
            //  int tname = 1;
            foreach (ALEmanCafe_Server.RemoteApplications.AppInfo AI in AIL)
            {
                string[] info = new string[4];
                info[0] = AI.AppName;
                info[1] = AI.AppTitle;
                info[2] = AI.AppPath;
                info[3] = AI.APPID.ToString();
                ListViewItem Lvi = new ListViewItem(info);
                Lvi.Name = Lvi.Text = Lvi.ToolTipText = AI.AppName;
                /*
                if (AI.AppICon != null)
                    if (this.listView1.InvokeRequired)
                    {
                        this.listView1.BeginInvoke(new MethodInvoker(
                            () => this.listView1.SmallImageList.Images.Add(tname.ToString(), AI.AppICon)));
                    }
                    else
                    {
                        this.listView1.SmallImageList.Images.Add(tname.ToString(), AI.AppICon);
                    }

                Lvi.ImageKey = tname.ToString();
                tname++;
                */
                if (this.AppsView.InvokeRequired)
                {
                    this.AppsView.BeginInvoke(new MethodInvoker(
                        () => this.AppsView.Items.Add(Lvi)));
                }
                else
                {
                    this.AppsView.Items.Add(Lvi);
                }
            }
            if (AIL.Count <= 0)
                this.TerminateSelected.Enabled = false;
            AppsView.MouseClick += new MouseEventHandler(AppsView_MouseClick);
            AppsView.KeyDown += new KeyEventHandler(AppsView_KeyPress);
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
        }

        private void TerminateAll_Click(object sender, EventArgs e)
        {
            if (this.AIL.Count > 0)
                this.ACS.SendPCStatus(this.NI, ALEmanCafeServer.MessageStatus.terminateall);
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new MethodInvoker(
                    () =>   this.Close()));
            }
            else
            {
                this.Close();
            }
          
        }

        public uint GetSelectionID()
        {
            if (this.AppsView.SelectedItems.Count > 0)
            {
                string st = this.AppsView.SelectedItems[0].SubItems[3].Text;
                return uint.Parse(st);
                /*
                foreach (ALEmanCafe_Server.RemoteApplications.AppInfo AI in AIL)
                {
                    if (AI.
                }
                    */
            }
            return 0;
        }

        private void TerminateSelected_Click(object sender, EventArgs e)
        {
            uint selid = GetSelectionID();
            if (selid > 0)
            {
                this.ACS.SendPCStatus(this.NI, ALEmanCafeServer.MessageStatus.terminate, false, selid);
                ListViewItem Lvi = this.AppsView.SelectedItems[0];
                if (this.AppsView.InvokeRequired)
                {
                    this.AppsView.BeginInvoke(new MethodInvoker(
                        () => this.AppsView.Items.Remove(Lvi)));
                }
                else
                {
                    this.AppsView.Items.Remove(Lvi);
                }
            }
            if (this.AppsView.Items.Count <= 0)
                this.TerminateSelected.Enabled = false;
        }

        private void AppsView_MouseClick(object Sender, MouseEventArgs eeee)
        {
            if (eeee.Button == System.Windows.Forms.MouseButtons.Right)
            {
                ListView.SelectedListViewItemCollection SLV = this.AppsView.SelectedItems;
                if (SLV.Count > 0)
                    contextMenuStrip1.Show(MousePosition);
            }
        }
        private void AppsView_KeyPress(object Sender, KeyEventArgs eeee)
        {
            if (eeee.KeyCode == Keys.Apps || eeee.KeyData == Keys.Apps || eeee.Modifiers == Keys.Apps)
            {
                ListView.SelectedListViewItemCollection SLV = this.AppsView.SelectedItems;
                if (SLV.Count > 0)
                    contextMenuStrip1.Show(MousePosition);
            }
        }

        private void terminateSelectedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TerminateSelected_Click(sender, e);
        }

        private void terminateAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TerminateAll_Click(sender, e);
        }
    }
}
