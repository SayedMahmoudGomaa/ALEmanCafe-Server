using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Security.Cryptography;

namespace ALEmanCafe_Server
{
    public partial class OptionsMenu : Form
    {
        public OptionsMenu()
        {
          //  MD5CryptoServiceProvider 
            InitializeComponent();
            this.VersionLabel.Text = Application.ProductVersion;
            this.ShowBox.CheckedChanged += new EventHandler(ShowBox_CheckedChanged);
            this.HourPrice.Text = Program.HourPrice + "";
            this.MinimumCost.Text = Program.MinimumCost + "";
            this.Askpasswordonstartup.Checked = Program.Askpasswordonstartup;
            this.Runserveratstartup.Checked = Program.RunServerAtStartup;
            this.UserTextBox.Text = Program.MyUserName;
            this.PassTextBox.Text = Program.MyPassword;
            this.EnableUSBPluginWarning.Checked = Program.EnableUSBPluginWarning;
            this.EnableUSBPlugoutWarning.Checked = Program.EnableUSBPlugoutWarning;
        }

        public void ShowBox_CheckedChanged(object Sender, EventArgs E)
        {
            if (ShowBox.Checked)
                PassTextBox.UseSystemPasswordChar = false;
            else
                PassTextBox.UseSystemPasswordChar = true;
        }

        private void CancelCloseButton_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            double HP = Program.HourPrice;
            double MC = Program.MinimumCost;
            try
            {
                HP = Convert.ToDouble(this.HourPrice.Text);
                MC = Convert.ToDouble(this.MinimumCost.Text);
            }
            catch { }

            Program.HourPrice = HP;
            Program.MinimumCost = MC;
            Program.MyUserName = this.UserTextBox.Text;
            Program.MyPassword = this.PassTextBox.Text;
            Program.Askpasswordonstartup = this.Askpasswordonstartup.Checked;
            Program.EnableUSBPluginWarning = this.EnableUSBPluginWarning.Checked;
            Program.EnableUSBPlugoutWarning = this.EnableUSBPlugoutWarning.Checked;
            MyDatabase.Save();
            CancelCloseButton.PerformClick();
        }
    }
}