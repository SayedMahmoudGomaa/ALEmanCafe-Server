using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ALEmanCafe_Server
{
    public partial class ChangePC : Form
    {
        public ALEmanCafeServer ACS;
        public NetworkItems NI;
        public ChangePC(ALEmanCafeServer ACS, NetworkItems NI)
        {
            InitializeComponent();
            this.ACS = ACS;
            this.NI = NI;
            this.KeyDown += new KeyEventHandler(TimeTextBox_KeyDown);
            this.listView1.KeyDown += new KeyEventHandler(TimeTextBox_KeyDown);
            this.CancelCloseButton.KeyDown += new KeyEventHandler(TimeTextBox_KeyDown);
            this.OKButton.KeyDown += new KeyEventHandler(TimeTextBox_KeyDown);
            this.listView1.ItemActivate += new EventHandler(listView1_ItemActivate);
            if (this.listView1.InvokeRequired)
            {
                this.listView1.BeginInvoke(new MethodInvoker(
                    () =>
                    {
                        this.listView1.SmallImageList = new ImageList();
                        this.listView1.SmallImageList.ImageSize = new Size(31, 40);
                        this.listView1.SmallImageList.Images.Add("OnlineDisplay", Properties.Resources.OnlineDisplay);
                    }));
            }
            else
            {
                this.listView1.SmallImageList = new ImageList();
                this.listView1.SmallImageList.ImageSize = new Size(31, 40);
                this.listView1.SmallImageList.Images.Add("OnlineDisplay", Properties.Resources.OnlineDisplay);
            }

            foreach (NetworkItems NI2 in NetworkItems.IdleNetworks.Values)
            {
                if (NI2.Connected == false) continue;
                string[] st = new string[3];
                st[0] = NI2.ShownName;
                st[1] = NI2.IP;
                st[2] = NI2.UsedTime.ToString();
                ListViewItem LVI = new ListViewItem(st);
                LVI.Text = LVI.Name = NI2.ShownName;
                LVI.ToolTipText = "From " + NI.ShownName + " To " + NI2.ShownName;
                LVI.Tag = NI2;
                LVI.ImageKey = "OnlineDisplay";
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

            this.listView1.ItemSelectionChanged += ListView1_ItemSelectionChanged;
        }

        private void ListView1_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
                OKButton.Enabled = true;
            else
                OKButton.Enabled = false;
        }

        public void TimeTextBox_KeyDown(object Sender, KeyEventArgs K)
        {
            if (K.KeyCode == Keys.Escape || K.KeyData == Keys.Escape || K.Modifiers == Keys.Escape)
            {
                this.Close();
            }
        }

        private void listView1_ItemActivate(object sender, EventArgs e)
        {
            OKButton.PerformClick();
        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            if (this.listView1.SelectedItems.Count > 0)
            {
                string sip = this.listView1.SelectedItems[0].SubItems[1].Text;
                NetworkItems NI2 = NetworkItems.IdleNetworks[sip];
                if (NI2 != null)
                {
                    if (NI2.Paid || NI2.UsedTime <= 0 || NI2.TimeStatu == NetworkItems.TimeStatus.None)
                    {
                        ACS.ChangePC(NI, NI2);
                        this.Close();
                    }
                    else
                        MessageBox.Show(NI2.ShownName + " have old timer!", "PC cannot changed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            
            }
        }

        private void CancelCloseButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
