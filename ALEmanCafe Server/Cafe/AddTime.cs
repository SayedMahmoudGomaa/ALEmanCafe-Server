using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ALEmanCafe_Server
{
    public partial class AddTime : Form
    {
        public ALEmanCafeServer ACS;
        public NetworkItems NI;
        private uint RightsTime = 0;
        public TimeLemetionType TLT = TimeLemetionType.Unknown;
        public enum TimeLemetionType
        {
            AddTime,
            LimitTime,
            Unknown
        }

        public AddTime(ALEmanCafeServer ACS, NetworkItems NI)
        {
            InitializeComponent();
            this.TimeTextBox.KeyDown += new KeyEventHandler(TimeTextBox_KeyDown);
            this.ACS = ACS;
            this.NI = NI;
            TimeTextBox.TextChanged += new EventHandler(TimeTextBox_TextChanged);
        }

        private void TimeTextBox_TextChanged(object Sender, EventArgs EA)
        {
            if (TLT == TimeLemetionType.AddTime)
            {
                if (string.IsNullOrEmpty(TimeTextBox.Text) || TimeTextBox.Text == "-") { RightsTime = 0; return; }
                else if (TimeTextBox.Text.Contains("+")) TimeTextBox.Text = TimeTextBox.Text.Replace("+", "");
                if (TimeTextBox.Text.StartsWith("-"))
                {
                    if (TimeTextBox.Text.Split('-').Length > 2)
                    {
                        TimeTextBox.Text = RightsTime.ToString();
                        return;
                    }
                }

                else if (TimeTextBox.Text.Split('-').Length > 1 || TimeTextBox.Text.Contains("."))
                {
                    // MessageBox.Show(TimeTextBox.Text.Split('-').Length.ToString());
                    TimeTextBox.Text = RightsTime.ToString();

                    return;
                }

                uint NewValue = 0;
                string Negative = TimeTextBox.Text.StartsWith("-") ? "-" : "";
                try
                {
                    NewValue = Convert.ToUInt32(TimeTextBox.Text.Replace("-", ""));

                }
                catch { TimeTextBox.Text = Negative + RightsTime.ToString(); return; }
                RightsTime = NewValue;
            }
            else if (TLT == TimeLemetionType.LimitTime)
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

        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            if (TLT == TimeLemetionType.AddTime)
            {
                if (RightsTime > 0 && TimeTextBox.Text != "-" && TimeTextBox.Text != "-0")
                {
                    if (TimeTextBox.Text.Contains("-"))
                    {
                        ACS.SendPCStatus(NI, ALEmanCafeServer.MessageStatus.addtime, TimeTextBox.Text.Contains("-"), RightsTime, this.PaidCheckBox.Checked);
                    }
                    else
                    {
                        ACS.SendPCStatus(NI, ALEmanCafeServer.MessageStatus.addtime, TimeTextBox.Text.Contains("-"), RightsTime, this.PaidCheckBox.Checked);
                    }
                    this.Close();
                    this.Dispose();
                }
            }
            else if (TLT == TimeLemetionType.LimitTime)
            {
                if (RightsTime > 0)
                {
                    if (RightsTime <= NI.UsedTime)
                        return;
                    ACS.SendPCStatus(NI, ALEmanCafeServer.MessageStatus.limittime, false, RightsTime, this.PaidCheckBox.Checked);
                    this.Close();
                    this.Dispose();
                }
            }
        }
    }
}
