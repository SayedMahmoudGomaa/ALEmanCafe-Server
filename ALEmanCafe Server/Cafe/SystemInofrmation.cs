using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ALEmanCafe_Server
{
    public partial class SystemInofrmation : Form
    {
        public SystemInofrmation(NetworkItems OM)
        {
            InitializeComponent();
            if (OM == null)
            {
                foreach (NetworkItems NI in NetworkItems.ALLNetworks.Values)
                {
                    string[] st = new string[8];
                    st[0] = NI.HostName;
                    st[1] = NI.UserName;
                    st[2] = NI.IP;
                    st[3] = NI.MacAddress;
                    st[4] = NI.MyOperatingSystem;
                    st[5] = NI.TotalMemory;
                    st[6] = NI.FreeMemory;
                    st[7] = NI.Harddisc;

                    ListViewItem LVI = new ListViewItem(st);
                    LVI.Text = LVI.ToolTipText = LVI.Name = st[0];

                    if (this.listView1.InvokeRequired)
                    {
                        this.listView1.BeginInvoke(new MethodInvoker(
                            () => this.listView1.Items.Add(LVI)));
                    }
                    else
                    {
                        this.listView1.Items.Add(LVI);
                    }
                }
            }
            else
            {
                string[] st = new string[8];
                st[0] = OM.HostName;
                st[1] = OM.UserName;
                st[2] = OM.IP;
                st[3] = OM.MacAddress;
                st[4] = OM.MyOperatingSystem;
                st[5] = OM.TotalMemory;
                st[6] = OM.FreeMemory;
                st[7] = OM.Harddisc;

                ListViewItem LVI = new ListViewItem(st);
                LVI.Text = LVI.ToolTipText = LVI.Name = st[0];

                if (this.listView1.InvokeRequired)
                {
                    this.listView1.BeginInvoke(new MethodInvoker(
                        () => this.listView1.Items.Add(LVI)));
                }
                else
                {
                    this.listView1.Items.Add(LVI);
                }
            }
        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
        }
    }
}
