using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ALEmanCafe_Server
{
    public partial class SendMessage : Form
    {
        public ALEmanCafeServer ACS;
        public NetworkItems NI;
        public SendMessage(ALEmanCafeServer ACS, NetworkItems NI)
        {
            InitializeComponent();
            this.ACS = ACS;
            this.NI = NI;
        }

        private void CancelCloseButton_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            if (NI != null)
            {
                ACS.SendPCStatus(NI, ALEmanCafeServer.MessageStatus.sendmessage, false, 0, false, null, richTextBox1.Text);
            }
            else
            {
                foreach (NetworkItems NI2 in NetworkItems.ALLNetworks.Values)
                {
                    if (string.IsNullOrEmpty(NI2.IP) == false)
                        ACS.SendPCStatus(NI2, ALEmanCafeServer.MessageStatus.sendmessage, false, 0, false, null, richTextBox1.Text);
                }
            }
            CancelCloseButton.PerformClick();
        }
    }
}
