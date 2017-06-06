using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ALEmanCafe_Server.Products
{
    public partial class SellingOperate : Form
    {
        public bool Sold = true;
        private ProductsInfos PI = null;
        private int RightsQuantity = 1;
        public ALEmanCafeServer ACS2;
        public SellingOperate(ALEmanCafeServer ACS2, ProductsInfos PI)
        {
            InitializeComponent();
            this.PI = PI;
            this.ACS2 = ACS2;
            this.TimeTextBox.KeyDown += new KeyEventHandler(TimeTextBox_KeyDown);
            TimeTextBox.TextChanged += new EventHandler(TimeTextBox_TextChanged);
            TimeTextBox.SelectAll();
            maskedTextBox1.Text = PI.Price + (Program.OrLanguage == true ? " EGP" : " جنيه");
            if (Program.OrLanguage == false)
            { this.Text = "عملية بيع/إسترجاع"; this.RightToLeft = RightToLeft.Yes; label3.Text = "البيع والإسترجاع"; label1.Text = "الكمية "; label2.Text = "السعر"; CancelCloseButton.Text = "خروج"; OKButton.Text = "حسناً"; }
        }

        private void TimeTextBox_TextChanged(object Sender, EventArgs EA)
        {
            if (string.IsNullOrEmpty(TimeTextBox.Text)) { RightsQuantity = 0; return; }
            else if (TimeTextBox.Text.Contains("+") || TimeTextBox.Text.Contains("-")) TimeTextBox.Text = TimeTextBox.Text.Replace("+", "").Replace("-", "");
            if (TimeTextBox.Text.Contains("-")) TimeTextBox.Text = TimeTextBox.Text.Replace("-", "");

            int NewValue = 0;
            try
            {
                NewValue = Convert.ToInt32(TimeTextBox.Text.Replace("-", ""));

            }
            catch { TimeTextBox.Text = RightsQuantity.ToString(); return; }
            RightsQuantity = NewValue;
            maskedTextBox1.Text = (this.PI.Price *  RightsQuantity).ToString() + (Program.OrLanguage == true ? " EGP" : " جنيه");
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
                if (RightsQuantity > 0)
                {
                    ProductsLog PL = new ProductsLog();
                    PL.ID = PI.PID;
                    PL.Quantity = RightsQuantity;
                    PL.Price = this.PI.Price * RightsQuantity;
                    PL.At = DateTime.Now;
                    if (Sold)
                    {
                        PI.Count -= RightsQuantity;
                        PI.NoOfSales += RightsQuantity;
                        PI.SoldToday += RightsQuantity;
                        PL.LogType = ProductsLog.LogTypes.Sold;
                    }
                    else
                    {
                        PL.LogType = ProductsLog.LogTypes.Returned;
                        PI.NoOfSales -= RightsQuantity;
                        PI.SoldToday -= RightsQuantity;
                        PI.Count += RightsQuantity;
                    }
                    MySqlConnection conn = new MySqlConnection(Program.connStr);
                    string s0 = "use `aleman_cafe_server`; UPDATE `products` SET count='" + PI.Count.ToString() + "', noofsales='" + PI.NoOfSales + "' WHERE id='" + PI.PID + "';";
                    try
                    {
                        conn.Open();
                        MySqlCommand cmd = new MySqlCommand(s0, conn);
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception ee) { conn.Close(); MessageBox.Show("Error: " + ee.Message); return; }
                    conn.Close();

                    conn = new MySqlConnection(Program.connStr);
                    s0 = "use `aleman_cafe_server`; INSERT INTO `productslog` VALUES('" +
                        PL.ID.ToString() + "', '" +
                        PL.Quantity.ToString() + "', '" +
                           PL.Price.ToString() + "', '" +
                              PL.At.Ticks.ToString() + "', '";
                    if (PL.LogType == ProductsLog.LogTypes.Sold)
                        s0 += "0');";
                    else
                        s0 += "1');";
                    try
                    {
                        conn.Open();
                        MySqlCommand cmd = new MySqlCommand(s0, conn);
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception ee) { conn.Close(); MessageBox.Show("Error: " + ee.Message); return; }
                    conn.Close();
                    this.ACS2.ProductController(PI, true);
                    this.ACS2.ProductLogController(PL);
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
