using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ALEmanCafe_Server.Other
{
    public partial class AdminControlling : Form
    {
        public AdminControlling()
        {
            InitializeComponent();
            this.PassTextBox.KeyDown += PassTextBox_KeyDown;
        }

        private void PassTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                OKButton_Click(sender, null);
            else if (e.KeyCode == Keys.Escape)
                this.Close();
        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            if (PassTextBox.Text == Program.MyPassword)
            {
                Program.isAdmin = true;
                this.Close();
            }
            else
               if (MessageBox.Show(this, "Wrong Password!", "Wrong Password!", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error) == DialogResult.Cancel)
                this.Close();
        }
    }
}
