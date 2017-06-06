using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace ALEmanCafe_Server
{
    public partial class ALEmanCafe : Form
    {

        public ALEmanCafeServer Server;
        public ALEmanCafe()
        {
            InitializeComponent();
            this.PassTextBox.UseSystemPasswordChar = true;
            this.UserTextBox.KeyPress += new KeyPressEventHandler(UserTextBox_KeyPress);
            this.PassTextBox.KeyPress += new KeyPressEventHandler(PassTextBox_KeyPress);

            MyDatabase.ALEmancafe = this;
            if (MyDatabase.LoadDatabaseConfig()) { }
            if (MyDatabase.CreateDB(true)) { }
            if (MyDatabase.LoadSystemlogTimers()) { }

            this.Server = new ALEmanCafeServer();
            if (!this.Server.LoadALEmanCafeServer(this))
            { }
            if (MyDatabase.Load()) { }
            if (!Program.OrLanguage)
            TranslateAR();
        }

        private void LoginButton_Click(object sender, EventArgs e)
        {
            if (UserTextBox.Text == Program.MyUserName && PassTextBox.Text == Program.MyPassword)
            {
                this.Visible = false;
                this.Server.LoginNow(true);
                //   this.Visible = false;
                // this.Server.ShowDialog();
            }
            else
                MessageBox.Show("Invaled Username or Password");// + Environment.NewLine+"User : "+MyUserName + ", Pass : "+MyPassword);
        }

        public void RestartNow()
        {
            /*
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new MethodInvoker(
                    () => Application.Restart()));//.AsyncWaitHandle.WaitOne();
            }
            else
            {
                Application.Restart();
            }
    
            ExitNow();
            */
            try
            {
                MyDatabase.Save();
            }
            catch { }
            if (this.Server != null)
                if (this.Server.udpClient != null)
                {
                    this.Server.udpClient.Client.Close();
                    this.Server.udpClient.Close();
                }
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new MethodInvoker(
                    () => Application.Restart()));//.AsyncWaitHandle.WaitOne();
            }
            else
            {
                Application.Restart();
            }
        }

        public void ExitNow()
        {
            try
            {
                MyDatabase.Save();
            }
            catch { }
            Application.Exit();
            if (this.Server != null)
                if (this.Server.udpClient != null)
                {
                    this.Server.udpClient.Client.Close();
                    this.Server.udpClient.Close();
                }

            Environment.Exit(0);

        }

        private void CancelsButton_Click(object sender, EventArgs e)
        {
            ExitNow();
        }

        private void UserTextBox_KeyPress(object sender, KeyPressEventArgs K)
        {
            if (K.KeyChar == (char)Keys.Enter)
            {
                PassTextBox.Select();
            }
        }

        private void PassTextBox_KeyPress(object sender, KeyPressEventArgs K)
        {
            if (K.KeyChar == (char)Keys.Enter)
            {
                LoginButton_Click(sender, null);
            }
        }

        public void TranslateAR()
        {
            this.Server.RightToLeft = this.RightToLeft = RightToLeft.Yes;
            this.label2.Text = "إسم المستخدم: ";


        }
    }
}
