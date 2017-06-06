using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Security.Cryptography;
using MySql.Data.MySqlClient;
using ALEmanCafe_Server.Properties;
using System.IO;

namespace ALEmanCafe_Server.Products
{
    public partial class AddProduct : Form
    {
        public ProductsInfos EPI = null;
        public ALEmanCafeServer ACS2;
        public AddProduct(ALEmanCafeServer ACS2, ProductsInfos PI = null)
        {
            InitializeComponent();
            this.ACS2 = ACS2;
            foreach (string PT in ProductsInfos.PTypes)
                this.TypeComboBox.Items.Add(PT);
            if (PI != null)
            {
                this.NameTextBox.Text = PI.PName;
                if (string.IsNullOrEmpty(PI.ImageURL))
                {
                    try
                    {
                        this.pictureBox1.Image = Image.FromFile(ProductsInfos.ImagesURL + PI.ImageURL);
                        this.pictureBox1.Visible = true;
                    }
                    catch { }
                }
            }
        }

        public void Edit(ProductsInfos EPI)
        {
            this.EPI = EPI;
            this.NameTextBox.Text = EPI.PName.ToString();
            this.CountTextBox.Text = EPI.Count.ToString();
            this.DetailsRichTextBox.Text = EPI.Details.ToString();
            this.URLTextBox.Text = EPI.ImageURL.ToString();
            this.NCountTextBox.Text = EPI.NeededCount.ToString();
            this.NOSTextBox.Text = EPI.NoOfSales.ToString();
            this.PPriceTextBox.Text = EPI.PPrice.ToString();
            this.PriceTextBox.Text = EPI.Price.ToString();
            this.TypeComboBox.Text = EPI.PType.ToString();
        }
        private void OKButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(CountTextBox.Text)) CountTextBox.Text = "0";
            bool copied = false;
            if (!string.IsNullOrEmpty(TypeComboBox.Text))
                if (!ProductsInfos.PTypes.Contains(TypeComboBox.Text))
                    ProductsInfos.PTypes.Add(TypeComboBox.Text);
            string imname = this.URLTextBox.Text;
            if (!string.IsNullOrEmpty(this.URLTextBox.Text))
                if (File.Exists(this.URLTextBox.Text))
                    if (Path.GetFullPath(this.URLTextBox.Text) != Path.GetFullPath(ProductsInfos.ImagesURL + Path.GetFileName(this.URLTextBox.Text)))
                    {
                        string EX = Path.GetExtension(this.URLTextBox.Text);
                        bool loop = true;

                        do
                        {
                     
                            string res = Program.GerRanChars(10);
                            string lr = ProductsInfos.ImagesURL + res + EX;
                            if (!File.Exists(lr))
                            {
                                imname = lr;
                                loop = false;
                            }
                        }
                        while (loop);
                        File.Copy(this.URLTextBox.Text, imname);
                        copied = true;
                    }
            MySqlConnection conn = new MySqlConnection(Program.connStr);
            string s0 = "";
            if (this.EPI == null)
            {
                s0 = "use `aleman_cafe_server`; INSERT INTO `products` (name, type, price, purchasingprice, count, noofsales, needed, details, picurl) " +
       "VALUES('" + NameTextBox.Text + "', '" +
       TypeComboBox.Text + "', '" +
       PriceTextBox.Text + "', '" +
       PPriceTextBox.Text + "', '" +
       CountTextBox.Text + "', '" +
       NOSTextBox.Text + "', '" +
       NCountTextBox.Text + "', '" +
       DetailsRichTextBox.Text;
                if (!string.IsNullOrEmpty(imname))
                    s0 += "', '" + Path.GetFileName(imname) + "'); ";
                else
                    s0 += "', ''); ";
            }
            else
            {
                s0 = "use `aleman_cafe_server`; UPDATE `products` SET " +
                 //SET(name, type, price, purchasingprice, count, noofsales, needed, details, picurl) " +
                 "name='" + NameTextBox.Text + "', " +
                "type='" + TypeComboBox.Text + "', " +
                "price='" + PriceTextBox.Text + "', " +
                "purchasingprice='" + PPriceTextBox.Text + "', " +
                "count='" + CountTextBox.Text + "', " +
                "noofsales='" + NOSTextBox.Text + "', " +
               "needed='" + NCountTextBox.Text + "', " +
              "details='" + DetailsRichTextBox.Text + "', ";
                if (!string.IsNullOrEmpty(imname))
                    s0 += "picurl='" + Path.GetFileName(imname) + "' WHERE id='"+this.EPI.PID + "';";
                else
                    s0 += "picurl='' WHERE id='" + this.EPI.PID + "';";
            }
            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(s0, conn);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ee) { conn.Close(); if (copied) File.Delete(imname); MessageBox.Show("Error: " + ee.Message); return; }
            conn.Close();
            ProductsInfos PI = new ProductsInfos();
            if (EPI == null)
            {
                PI.PID = ProductsInfos.NextID;
                ProductsInfos.NextID++;
            }
            else
                PI = this.EPI;
            PI.PName = NameTextBox.Text;
            PI.PType = TypeComboBox.Text;
            PI.Price = float.Parse(PriceTextBox.Text);
            PI.PPrice = float.Parse(PPriceTextBox.Text);
            PI.Count = int.Parse(CountTextBox.Text);
            PI.NoOfSales = int.Parse(NOSTextBox.Text);
            PI.NeededCount = int.Parse(NCountTextBox.Text);
            PI.Details = DetailsRichTextBox.Text;
            PI.ImageURL = string.IsNullOrEmpty(imname) ? "" : Path.GetFileName(imname);
            if (this.EPI != null)
            {
                this.ACS2.ProductController(PI, true, false);
                this.Close();
            }
            else
            {
                this.ACS2.ProductController(PI);
                this.Close();
            }
        }
       
        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog OFD = new OpenFileDialog();
            OFD.Multiselect = false;
            OFD.Filter = "JPG (*.jpg) |*.jpg|PNG (*.png) |*.png|BMP (*.bmp) |*.bmp|GIF (*.gif) |*.gif";
            if (OFD.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.URLTextBox.Text = OFD.FileName;
                try
                {
                    this.pictureBox1.Image = Image.FromFile(OFD.FileName);
                    this.pictureBox1.Visible = true;
                }
                catch { }
            }

        }
    }
}