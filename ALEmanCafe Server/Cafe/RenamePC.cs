using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace ALEmanCafe_Server.Cafe
{
    public partial class RenamePC : Form
    {
        public NetworkItems NI;
        public ALEmanCafeServer ACS;
        public RenamePC(NetworkItems NI, ALEmanCafeServer ACS)
        {
            InitializeComponent();
            this.NI = NI;
            this.ACS = ACS;
            this.NewNameTextBox.KeyDown += NewNameTextBox_KeyDown;
        }

        private void NewNameTextBox_KeyDown(object sender, KeyEventArgs K)
        {
            if (K.KeyCode == Keys.Enter)
                OKButton.PerformClick();
            else if (K.KeyCode == Keys.Escape)
            {
                this.Close();
                this.Dispose();
            }
        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(NewNameTextBox.Text) == false)
            {
           //    bool already = false;
          
                if (ACS.MonitorView.Items.ContainsKey(NewNameTextBox.Text))
                {
                    MessageBox.Show("this name is Already in use", "Duplicate name", MessageBoxButtons.OK, MessageBoxIcon.Error); return;
                }

                var mv = ACS.MonitorView.Items.Find(NI.ShownName, false);
                if (mv.Length < 0)
                {
                    MessageBox.Show("this name not in the list!", "old name not exist!", MessageBoxButtons.OK, MessageBoxIcon.Stop); return;
                }
                mv[0].Name = mv[0].Text = NI.ShownName= NewNameTextBox.Text;
              
                ACS.RefreshPCStatusNow(NI);
                ACS.MonitorView.Sort();
                /*
                ACS.deleteSelectedToolStripMenuItem_Click(sender, e);

                this.NI.ShownName = NewNameTextBox.Text;
                NetworkItems.ALLNetworks.Add(this.NI.IP, this.NI);
                ACS.AddNewPC(NI, ACS.MonitorView);
                */
                this.Close();
                this.Dispose();
            }
        }

        private void HostNameButton_Click(object sender, EventArgs e)
        {
            NewNameTextBox.Text = Dns.GetHostEntry(NI.IP).HostName;
        }
    }
}
