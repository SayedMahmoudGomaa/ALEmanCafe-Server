using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ALEmanCafe_Server
{
    public partial class TimeLimited : Form
    {
        public ALEmanCafeServer ACS;
        public TimeLemetionType TLT = TimeLemetionType.Unknown;
        public NetworkItems NI;
        private uint RightsTime = 0;
        public enum TimeLemetionType
        {
            Prepaid,
            GamePrepaid,
            PlayAndPay,
            GamePlayAndPay,
            Unknown
        }

        public TimeLimited(ALEmanCafeServer ACS,NetworkItems NI)
        {
            InitializeComponent();
            this.ACS = ACS;
            this.NI = NI;
            this.TimeTextBox.KeyDown += new KeyEventHandler(TimeTextBox_KeyDown);
            TimeTextBox.TextChanged += new EventHandler(TimeTextBox_TextChanged);
            TimeTextBox.SelectAll();
        }

        private void TimeTextBox_TextChanged(object Sender, EventArgs EA)
        {
            if (string.IsNullOrEmpty(TimeTextBox.Text)) { RightsTime = 0; return; }
            else if (TimeTextBox.Text.Contains("+")) TimeTextBox.Text = TimeTextBox.Text.Replace("+", "");
            if (TimeTextBox.Text.Contains("-")) TimeTextBox.Text = TimeTextBox.Text.Replace("-", "");

            uint NewValue = 0;
            try
            {
                NewValue = Convert.ToUInt32(TimeTextBox.Text.Replace("-", ""));

            }
            catch { TimeTextBox.Text = RightsTime.ToString(); return; }
            RightsTime = NewValue;
            maskedTextBox1.Text = Program.GetUsageCost(RightsTime);
        }

        public void TimeTextBox_KeyDown(object Sender, KeyEventArgs K)
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
            if (string.IsNullOrEmpty(TimeTextBox.Text) == false)
            {
                if (RightsTime > 0)
                {
                    if (GameCheckBox.Visible && TLT == TimeLemetionType.Unknown)
                    {
                        if (GameCheckBox.Checked && PaidCheckBox.Checked)
                            TLT = TimeLemetionType.GamePlayAndPay;
                        else if (GameCheckBox.Checked)
                            TLT = TimeLemetionType.GamePrepaid;
                        else if (PaidCheckBox.Checked)
                            TLT = TimeLemetionType.PlayAndPay;
                        else
                            TLT = TimeLemetionType.Prepaid;
                    }

                    if (TLT == TimeLemetionType.GamePlayAndPay)
                        ACS.SendPCStatus(NI, ALEmanCafeServer.MessageStatus.login, true, RightsTime, true);
                    else if (TLT == TimeLemetionType.GamePrepaid)
                        ACS.SendPCStatus(NI, ALEmanCafeServer.MessageStatus.login, true, RightsTime, false);
                    else if (TLT == TimeLemetionType.PlayAndPay)
                        ACS.SendPCStatus(NI, ALEmanCafeServer.MessageStatus.login, false, RightsTime, true);
                    else if (TLT == TimeLemetionType.Prepaid)
                        ACS.SendPCStatus(NI, ALEmanCafeServer.MessageStatus.login, false, RightsTime, false);

                    this.Close();
                    this.Dispose();
                }
            }
        }

        private void CancelCloseButton_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
        }
    }
}
