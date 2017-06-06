using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ALEmanCafe_Server.Other
{
    public partial class DatabaseConfiguration : Form
    {
        public ALEmanCafe ALEmancafe;
        public bool CorrectDBC = false;
        public DatabaseConfiguration(ALEmanCafe ALEmancafe)
        {
            InitializeComponent();
            this.ALEmancafe = ALEmancafe;
            this.FormClosing += DatabaseConfiguration_FormClosing;
        }

        private void DatabaseConfiguration_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!CorrectDBC)
                ALEmancafe.ExitNow();
        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            Program.DatabaseServer = ServerTextBox.Text;
            Program.DatabaseUsername = UserTextBox.Text;
            Program.DatabasePassword = PassTextBox.Text;
            if (MyDatabase.CreateDB())
            {
             //   MessageBox.Show(Program.DatabaseServer + Environment.NewLine + Program.DatabaseUsername + Environment.NewLine + Program.DatabasePassword);
                CorrectDBC = true;
                MyDatabase.SaveDatabaseConfig();
                this.Close();
            }
        }
    }
}
