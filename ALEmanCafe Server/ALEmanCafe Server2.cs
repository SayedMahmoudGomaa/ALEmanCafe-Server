using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Net;
using System.Globalization;
using System.Net.Sockets;
using System.Text;
using Microsoft.Win32;
using System.IO;
using System.Diagnostics;
using MySql.Data.MySqlClient;
using ALEmanCafe_Server.Products;

namespace ALEmanCafe_Server
{
    public partial class ALEmanCafeServer
    {
        //  protected override void OnLoad(EventArgs e)
        public bool LoadProducts()
        {
            #region Images

            if (this.ProductsList.InvokeRequired)
            {
                this.ProductsList.BeginInvoke(new MethodInvoker(
                    () => this.ProductsList.LargeImageList = new ImageList()));
            }
            else
            {
                this.ProductsList.LargeImageList = new ImageList();
            }

            if (this.ProductsList.InvokeRequired)
            {
                this.ProductsList.BeginInvoke(new MethodInvoker(
                    () => this.ProductsList.LargeImageList.ImageSize = new Size(100, 100)));
            }
            else
            {
                this.ProductsList.LargeImageList.ImageSize = new Size(100, 100);
            }


            if (this.ProductsList.InvokeRequired)
            {
                this.ProductsList.BeginInvoke(new MethodInvoker(
                    () => this.ProductsList.SmallImageList = new ImageList()));
            }
            else
            {
                this.ProductsList.SmallImageList = new ImageList();
            }

            if (this.ProductsList.InvokeRequired)
            {
                this.ProductsList.BeginInvoke(new MethodInvoker(
                    () => this.ProductsList.SmallImageList.ImageSize = new Size(22, 31)));
            }
            else
            {
                this.ProductsList.SmallImageList.ImageSize = new Size(22, 31);
            }


            if (this.ProductsList.InvokeRequired)
            {
                this.ProductsList.BeginInvoke(new MethodInvoker(
                    () => this.ProductsList.LargeImageList.Images.Add("LoginMonitor", Properties.Resources.LoginMonitor)));
            }
            else
            {
                this.ProductsList.LargeImageList.Images.Add("LoginMonitor", Properties.Resources.LoginMonitor);
            }

            if (this.ProductsList.InvokeRequired)
            {
                this.ProductsList.BeginInvoke(new MethodInvoker(
                    () => this.ProductsList.SmallImageList.Images.Add("LoginMonitor", Properties.Resources.LoginMonitor)));
            }
            else
            {
                this.ProductsList.SmallImageList.Images.Add("LoginMonitor", Properties.Resources.LoginMonitor);
            }

            //*********
            if (this.SalesLogView.InvokeRequired)
            {
                this.SalesLogView.BeginInvoke(new MethodInvoker(
                    () => this.SalesLogView.LargeImageList = new ImageList()));
            }
            else
            {
                this.SalesLogView.LargeImageList = new ImageList();
            }

            if (this.SalesLogView.InvokeRequired)
            {
                this.SalesLogView.BeginInvoke(new MethodInvoker(
                    () => this.SalesLogView.LargeImageList.ImageSize = new Size(100, 100)));
            }
            else
            {
                this.SalesLogView.LargeImageList.ImageSize = new Size(100, 100);
            }


            if (this.SalesLogView.InvokeRequired)
            {
                this.SalesLogView.BeginInvoke(new MethodInvoker(
                    () => this.SalesLogView.SmallImageList = new ImageList()));
            }
            else
            {
                this.SalesLogView.SmallImageList = new ImageList();
            }

            if (this.SalesLogView.InvokeRequired)
            {
                this.SalesLogView.BeginInvoke(new MethodInvoker(
                    () => this.SalesLogView.SmallImageList.ImageSize = new Size(22, 31)));
            }
            else
            {
                this.SalesLogView.SmallImageList.ImageSize = new Size(22, 31);
            }


            if (this.SalesLogView.InvokeRequired)
            {
                this.SalesLogView.BeginInvoke(new MethodInvoker(
                    () => this.SalesLogView.LargeImageList.Images.Add("LoginMonitor", Properties.Resources.LoginMonitor)));
            }
            else
            {
                this.SalesLogView.LargeImageList.Images.Add("LoginMonitor", Properties.Resources.LoginMonitor);
            }

            if (this.SalesLogView.InvokeRequired)
            {
                this.SalesLogView.BeginInvoke(new MethodInvoker(
                    () => this.SalesLogView.SmallImageList.Images.Add("LoginMonitor", Properties.Resources.LoginMonitor)));
            }
            else
            {
                this.SalesLogView.SmallImageList.Images.Add("LoginMonitor", Properties.Resources.LoginMonitor);
            }
            #endregion

            if (!Directory.Exists(ProductsInfos.ImagesURL))
                Directory.CreateDirectory(ProductsInfos.ImagesURL);
            MySqlConnection conn = new MySqlConnection(Program.connStr);
         
            conn = new MySqlConnection(Program.connStr);
            conn.Open();
            string s0 = "use `aleman_cafe_server`; SELECT * FROM `products`";
            MySqlCommand cmd = new MySqlCommand(s0, conn);
            try
            {
                var rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    ProductsInfos PI = new ProductsInfos(rdr);
                    ProductController(PI);
                    if (!ProductsInfos.PTypes.Contains(rdr.GetString(2)))
                    {
                        ProductsInfos.PTypes.Add(rdr.GetString(2));
                    }
                    int MID = rdr.GetInt32(0);
                    if (MID >= ProductsInfos.NextID)
                        ProductsInfos.NextID = (MID + 1);
                }
                rdr.Close();
                conn.Close();
            }
            catch (Exception ee)
            {
                conn.Close();
                //  MessageBox.Show("There are an error and database cannot be loaded." + Environment.NewLine + ee.ToString(), "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (MessageBox.Show("There are an error and database cannot be loaded, do you want to create new database?" + Environment.NewLine + ee.ToString(), "Database Error", MessageBoxButtons.YesNo, MessageBoxIcon.Error) == DialogResult.Yes)
                    MyDatabase.CreateDB();
                this.ReloadList();
            }

            s0 = "use `aleman_cafe_server`; SELECT * FROM `productslog`";
            conn.Open();
            var TodayLogs = new List<ProductsLog>();
             cmd = new MySqlCommand(s0, conn);
            try
            {
                var rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    ProductsLog PL = new ProductsLog(rdr);
                    if (PL.At.Date == DateTime.Now.Date && !TodayLogs.Contains(PL))
                        TodayLogs.Add(PL);
                    ProductLogController(PL);
                }
                rdr.Close();
                conn.Close();
            }
            catch (Exception ee)
            {
                conn.Close();
                //  MessageBox.Show("There are an error and database cannot be loaded." + Environment.NewLine + ee.ToString(), "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (MessageBox.Show("There are an error and database 'productslog' cannot be loaded, do you want to create new database?" + Environment.NewLine + ee.ToString(), "Database Error", MessageBoxButtons.YesNo, MessageBoxIcon.Error) == DialogResult.Yes)
                    MyDatabase.CreateDB();
                this.ReloadList();
            }
            var WU = new List<int>();
            foreach (var N in TodayLogs)
            {
                var PI = ProductsInfos.AllProducts[N.ID.ToString()];
                if (N.LogType == ProductsLog.LogTypes.Sold)
                    PI.SoldToday += N.Quantity;
                else
                    PI.SoldToday -= N.Quantity;
                if (!WU.Contains(PI.PID))
                    WU.Add(PI.PID);
            }

            foreach(int i in WU)
            {
                var PI = ProductsInfos.AllProducts[i.ToString()];
                ProductController(PI, true);
            }
            /*
            this.thumbnailsToolStripMenuItem.Click += new System.EventHandler(this.thumbnailsToolStripMenuItem_Click);
            this.tilesToolStripMenuItem.Click += new System.EventHandler(this.tilesToolStripMenuItem_Click);
            this.iconsToolStripMenuItem.Click += new System.EventHandler(this.iconsToolStripMenuItem_Click);
            this.listToolStripMenuItem.Click += new System.EventHandler(this.listToolStripMenuItem_Click);
            this.detailsToolStripMenuItem.Click += new System.EventHandler(this.detailsToolStripMenuItem_Click);
            this.AddProductButton.Click += new System.EventHandler(this.AddProductButton_Click);
            */
            ProductsList.MouseClick += new MouseEventHandler(ProductsList_MouseClick);
            ProductsList.MouseDoubleClick += new MouseEventHandler(ProductsList_MouseClick);
            ProductsList.KeyDown += new KeyEventHandler(ProductsList_KeyPress);
            ProductsList.ItemSelectionChanged += new ListViewItemSelectionChangedEventHandler(ProductsList_ItemSelectionChanged);
            ProductsList.ColumnClick += ProductsList_ColumnClick;

            SearchText.TextChanged += SearchText_TextChanged;

            FromdatePicker.ValueChanged += FromdatePicker_ValueChanged;
            TodatePicker.ValueChanged += FromdatePicker_ValueChanged;
            return true;
        }

        private void FromdatePicker_ValueChanged(object sender, EventArgs e)
        {
            if (this.SalesLogView.InvokeRequired)
            {
                this.SalesLogView.BeginInvoke(new MethodInvoker(
                    () => this.SalesLogView.Items.Clear()));
            }
            else
            {
                this.SalesLogView.Items.Clear();
            }

            foreach (ProductsLog PL in ProductsLog.AllLogs)
            {
                if (PL.At.Date >= FromdatePicker.Value.Date && PL.At.Date <= TodatePicker.Value.Date)
                    ProductLogController(PL);
            }
        }

        private void SearchText_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(SearchText.Text))
                ReloadList();
            else
            {
                if (this.ProductsList.InvokeRequired)
                {
                    this.ProductsList.BeginInvoke(new MethodInvoker(
                        () => this.ProductsList.Items.Clear()));
                }
                else
                {
                    this.ProductsList.Items.Clear();
                }

                foreach (ProductsInfos PI in ProductsInfos.AllProducts.Values)
                {
                    if (PI.PName.ToLower().Contains(SearchText.Text.ToLower()) || PI.PType.ToLower().Contains(SearchText.Text.ToLower()) || PI.Details.ToLower().Contains(SearchText.Text.ToLower()))
                        ProductController(PI);
                }
            }
        }

        private void soldToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ListView.SelectedListViewItemCollection SLV = this.ProductsList.SelectedItems;
            if (SLV.Count > 0)
            {
                var PInfo = ProductsInfos.AllProducts[SLV[0].Text];
                SellingOperate SO = new SellingOperate(this, PInfo);
                SO.ShowDialog();
            }
        }

        private void returnedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ListView.SelectedListViewItemCollection SLV = this.ProductsList.SelectedItems;
            if (SLV.Count > 0)
            {
                var PInfo = ProductsInfos.AllProducts[SLV[0].Text];
                SellingOperate SO = new SellingOperate(this,PInfo);
                SO.Sold = false;
                SO.ShowDialog();
            }
        }

        private int sortColumn = -1;
        private void ProductsList_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            if (e.Column != sortColumn)
            {
                // Set the sort column to the new column.
                sortColumn = e.Column;
                // Set the sort order to ascending by default.
                SalesLogView.Sorting = SortOrder.Ascending;
            }
            else
            {
                // Determine what the last sort order was and change it.
                if (ProductsList.Sorting == SortOrder.Ascending)
                    ProductsList.Sorting = SortOrder.Descending;
                else
                    ProductsList.Sorting = SortOrder.Ascending;
            }

            // Call the sort method to manually sort.
            ProductsList.Sort();
            // Set the ListViewItemSorter property to a new ListViewItemComparer
            // object.
            this.ProductsList.ListViewItemSorter = new ListViewItemComparer(e.Column,
                                                              ProductsList.Sorting);
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ListView.SelectedListViewItemCollection SLV = this.ProductsList.SelectedItems;
            if (SLV.Count > 0)
            {
                AddProduct AP = new AddProduct(this);
                AP.Edit(ProductsInfos.AllProducts[SLV[0].Text]);
                AP.ShowDialog();
            }
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure? do you want to delete this product?", "Deleting product", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                ListView.SelectedListViewItemCollection SLV = this.ProductsList.SelectedItems;
                if (SLV.Count > 0)
                {
                    try
                    {
                        var PInfo = ProductsInfos.AllProducts[SLV[0].Text];
                       for(int p = 0; p <= ProductsLog.AllLogs.Count; p++)// foreach (ProductsLog PL2 in ProductsLog.AllLogs)
                        {
                            ProductsLog PL2 = ProductsLog.AllLogs[p];
                            if (PL2.ID == PInfo.PID)
                                ProductLogController(PL2, true);
                        }
                        MySqlConnection conn = new MySqlConnection(Program.connStr);
                        string s0 = "use aleman_cafe_server; DELETE FROM products WHERE id='" + PInfo.PID + "';";
                        conn.Open();
                        MySqlCommand cmd = new MySqlCommand(s0, conn);
                        cmd.ExecuteNonQuery();
                        ProductController(PInfo, false, true);
                        conn.Close();
                        conn = new MySqlConnection(Program.connStr);
                        conn.Open();
                        s0 = "use aleman_cafe_server; DELETE FROM productslog WHERE id='" + PInfo.PID + "';";
                        cmd = new MySqlCommand(s0, conn);
                        cmd.ExecuteNonQuery();
                        conn.Close();
                    }catch(Exception ee) { MessageBox.Show("Error : " + ee.ToString());}
                }
            }
        }

        private void ProductsList_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs LVISC)
        {
            if (LVISC.IsSelected)
            {
                PSName.Text = LVISC.Item.SubItems[1].Text;
                PSType.Text = LVISC.Item.SubItems[2].Text;
                PSCount.Text = LVISC.Item.SubItems[5].Text;
                PSPrice.Text = LVISC.Item.SubItems[3].Text;
                PSSTC.Text = ProductsInfos.AllProducts[LVISC.Item.Text].SoldToday.ToString();
                PSNC.Text = LVISC.Item.SubItems[7].Text;
            }
        }

        private void ProductsList_KeyPress(object sender, KeyEventArgs eeee)
        {
            if (eeee.KeyCode == Keys.Apps || eeee.KeyData == Keys.Apps || eeee.Modifiers == Keys.Apps)
            {
                ListView.SelectedListViewItemCollection SLV = this.ProductsList.SelectedItems;
                if (SLV.Count > 0)
                {
                    PrepareContextMenu2(SLV[0]);
                    contextMenuStrip2.Show(MousePosition);
                }
            }
        }

        private void ProductsList_MouseClick(object sender, MouseEventArgs eeee)
        {
            if (eeee.Button == System.Windows.Forms.MouseButtons.Right)
            {
                ListView.SelectedListViewItemCollection SLV = this.ProductsList.SelectedItems;
                if (SLV.Count > 0)
                {
                    PrepareContextMenu2(SLV[0]);
                    contextMenuStrip2.Show(MousePosition);
                }
            }
        }

        public void PrepareContextMenu2(ListViewItem LVI)
        {
            //   ProductsInfos PI = ProductsInfos.AllProducts[LVI.Text];
            deleteToolStripMenuItem.Enabled = editToolStripMenuItem.Enabled = Program.isAdmin;

        }

        public void ReloadList()
        {
            if (this.ProductsList.InvokeRequired)
            {
                this.ProductsList.BeginInvoke(new MethodInvoker(
                    () => this.ProductsList.Items.Clear()));
            }
            else
            {
                this.ProductsList.Items.Clear();
            }

            foreach (ProductsInfos PI in ProductsInfos.AllProducts.Values)
            {
                ProductController(PI);
            }
        }
        public void ProductController(ProductsInfos PI, bool Update = false, bool Delete = false)
        {
            if (Delete)
            {
                var elv = ProductsList.Items.Find(PI.PID.ToString(), false);
                if (elv.Length > 0)
                    this.ProductsList.Items.Remove(elv[0]);
                ProductsInfos.AllProducts.Remove(PI.PID.ToString());
                ProductsInfos.TotalProducts--;
                ItemCountL.Text = ProductsInfos.TotalProducts.ToString();
                return;
            }
            string[] Items = new string[9];
            Items[0] = PI.PID.ToString();
            Items[1] = PI.PName;
            Items[2] = PI.PType;
            Items[3] = PI.Price.ToString();
            if (Program.isAdmin)
                Items[4] = PI.PPrice.ToString();
            Items[5] = PI.Count.ToString();
            Items[6] = PI.NoOfSales.ToString();
            Items[7] = PI.NeededCount.ToString();
            Items[8] = PI.Details;
            //name, type, price, purchasingprice, count, noofsales, needed, details, picurl) "+
            ListViewItem LV = new ListViewItem(Items);
            LV.Name = LV.Text = PI.PID.ToString();
            LV.ToolTipText = "Name : " + PI.PName + Environment.NewLine +
            "Type : " + PI.PType + Environment.NewLine +
            "Price : " + PI.Price + Environment.NewLine +
            "Purchasing Price : " + PI.PPrice + Environment.NewLine +
            "Count : " + PI.Count + Environment.NewLine +
            "Number of sales : " + PI.NoOfSales + Environment.NewLine +
            "Needed count : " + PI.NeededCount + Environment.NewLine +
            "Details : " + PI.Details;

            if (string.IsNullOrEmpty(PI.ImageURL))

                LV.ImageKey = "LoginMonitor";
            else
            {
                try
                {
                    if (!ProductsList.LargeImageList.Images.ContainsKey(PI.ImageURL))
                    {
                        if (this.ProductsList.InvokeRequired)
                        {
                            this.ProductsList.BeginInvoke(new MethodInvoker(
                                () => this.ProductsList.LargeImageList.Images.Add(PI.ImageURL, Image.FromFile(ProductsInfos.ImagesURL + PI.ImageURL))));
                        }
                        else
                        {
                            this.ProductsList.LargeImageList.Images.Add(PI.ImageURL, Image.FromFile(ProductsInfos.ImagesURL + PI.ImageURL));
                        }
                    }
                    if (!ProductsList.SmallImageList.Images.ContainsKey(PI.ImageURL))
                    {
                        if (this.ProductsList.InvokeRequired)
                        {
                            this.ProductsList.BeginInvoke(new MethodInvoker(
                                () => this.ProductsList.SmallImageList.Images.Add(PI.ImageURL, Image.FromFile(ProductsInfos.ImagesURL + PI.ImageURL))));
                        }
                        else
                        {
                            this.ProductsList.SmallImageList.Images.Add(PI.ImageURL, Image.FromFile(ProductsInfos.ImagesURL + PI.ImageURL));
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ProductsInfos.ImagesURL + PI.ImageURL + Environment.NewLine + ex.ToString());
                }
                LV.ImageKey = PI.ImageURL;
            }

            if (Update)
            {
                var elv = ProductsList.Items.Find(PI.PID.ToString(), false);
                if (elv.Length > 0)
                    this.ProductsList.Items.Remove(elv[0]);
                ProductsInfos.AllProducts.Remove(PI.PID.ToString());
                ProductsInfos.AllProducts.Add(PI.PID.ToString(), PI);
            }
            else
            {
                if (!ProductsInfos.AllProducts.ContainsKey(PI.PID.ToString()))
                {
                    ProductsInfos.AllProducts.Add(PI.PID.ToString(), PI);
                    ProductsInfos.TotalProducts++;
                    ItemCountL.Text = ProductsInfos.TotalProducts.ToString();
                }
            }
            if (this.ProductsList.InvokeRequired)
            {
                this.ProductsList.BeginInvoke(new MethodInvoker(
                    () => this.ProductsList.Items.Add(LV)));
            }
            else
            {
                this.ProductsList.Items.Add(LV);
            }

            /*
            string[] Items = new string[10];
            Items[0] = MDR.GetString(0);
            Items[1] = MDR.GetString(1);
            Items[2] = MDR.GetString(2);
            Items[3] = MDR.GetString(3);
            Items[4] = MDR.GetString(4);
            Items[5] = MDR.GetString(5);
            Items[6] = MDR.GetString(6);
            Items[7] = MDR.GetString(7);
            Items[8] = MDR.GetString(8);
            Items[9] = MDR.GetString(9);
            //name, type, price, purchasingprice, count, noofsales, needed, details, picurl) "+
            ListViewItem LV = new ListViewItem(Items);
            LV.Name = LV.Text = MDR.GetString(0);
            LV.ToolTipText = "Name : " + MDR.GetString(1) + Environment.NewLine +
            "Type : " + MDR.GetString(2) + Environment.NewLine +
            "Price : " + MDR.GetFloat(3) + Environment.NewLine +
            "Purchasing Price : " + MDR.GetFloat(4) + Environment.NewLine +
            "Count : " + MDR.GetInt32(5) + Environment.NewLine +
            "Number of sales : " + MDR.GetInt32(6) + Environment.NewLine +
            "Needed count : " + MDR.GetInt32(7) + Environment.NewLine +
            "Details : " + MDR.GetString(8);
            LV.ImageKey = "OnlineDisplay";
            if (this.ProductsList.InvokeRequired)
            {
                this.ProductsList.BeginInvoke(new MethodInvoker(
                    () => this.ProductsList.Items.Add(LV)));
            }
            else
            {
                this.ProductsList.Items.Add(LV);
            }*/
            Application.DoEvents();
        }
        public void ProductLogController(ProductsLog PL, bool Delete = false)
        {
            if (Delete)
            {
                for(int sSs = 0; sSs < SalesLogView.Items.Count; sSs++)//foreach(ListView.ListViewItemCollection ss in SalesLogView.Items)
                {
                    ListViewItem ss = SalesLogView.Items[sSs];
                        if (ss.SubItems[7].Text == PL.At.ToString())
                        SalesLogView.Items.Remove(ss);
                }
                /*
                var elv = SalesLogView.Items.Find(PL.ID.ToString(), false);
                if (elv.Length > 0)
                    this.SalesLogView.Items.Remove(elv[0]);
                */
                ProductsLog.AllLogs.Remove(PL);
                TotalLogs.Text = ProductsLog.AllLogs.Count.ToString();
                return;
            }
            var PI = ProductsInfos.AllProducts[PL.ID.ToString()];
            string[] Items = new string[8];
            Items[0] = PL.ID.ToString();
            Items[1] = PI.PName;
            Items[2] = PI.PType;
            Items[3] = PL.Price.ToString();
            Items[4] = PI.Price.ToString();
            Items[5] = PL.Quantity.ToString();
            Items[6] = PL.LogType.ToString();
            Items[7] = PL.At.ToString();
            //name, type, price, purchasingprice, count, noofsales, needed, details, picurl) "+
            ListViewItem LV = new ListViewItem(Items);
            LV.Name = LV.Text = PI.PID.ToString();
            LV.ToolTipText = "Name : " + PI.PName + Environment.NewLine +
            "Type : " + PI.PType + Environment.NewLine +
            "Price : " + PI.Price + Environment.NewLine +
            "Purchasing Price : " + PI.PPrice + Environment.NewLine +
            "Count : " + PI.Count + Environment.NewLine +
            "Number of sales : " + PI.NoOfSales + Environment.NewLine +
            "Needed count : " + PI.NeededCount + Environment.NewLine +
            "Details : " + PI.Details + Environment.NewLine +
            "Action : " + PL.LogType.ToString() + Environment.NewLine +
            "Quantity : " + PL.Quantity + Environment.NewLine +
            "DateTime : " + PL.At.ToString();

            if (string.IsNullOrEmpty(PI.ImageURL))

                LV.ImageKey = "LoginMonitor";
            else
            {
                try
                {
                    if (!SalesLogView.LargeImageList.Images.ContainsKey(PI.ImageURL))
                    {
                        if (this.SalesLogView.InvokeRequired)
                        {
                            this.SalesLogView.BeginInvoke(new MethodInvoker(
                                () => this.SalesLogView.LargeImageList.Images.Add(PI.ImageURL, Image.FromFile(ProductsInfos.ImagesURL + PI.ImageURL))));
                        }
                        else
                        {
                            this.SalesLogView.LargeImageList.Images.Add(PI.ImageURL, Image.FromFile(ProductsInfos.ImagesURL + PI.ImageURL));
                        }
                    }
                    if (!SalesLogView.SmallImageList.Images.ContainsKey(PI.ImageURL))
                    {
                        if (this.SalesLogView.InvokeRequired)
                        {
                            this.SalesLogView.BeginInvoke(new MethodInvoker(
                                () => this.SalesLogView.SmallImageList.Images.Add(PI.ImageURL, Image.FromFile(ProductsInfos.ImagesURL + PI.ImageURL))));
                        }
                        else
                        {
                            this.SalesLogView.SmallImageList.Images.Add(PI.ImageURL, Image.FromFile(ProductsInfos.ImagesURL + PI.ImageURL));
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ProductsInfos.ImagesURL + PI.ImageURL + Environment.NewLine + ex.ToString());
                }
                LV.ImageKey = PI.ImageURL;
            }


            if (!ProductsLog.AllLogs.Contains(PL))
            {
                ProductsLog.AllLogs.Add(PL);
                TotalLogs.Text = ProductsLog.AllLogs.Count.ToString();
            }

            if (this.SalesLogView.InvokeRequired)
            {
                this.SalesLogView.BeginInvoke(new MethodInvoker(
                    () => this.SalesLogView.Items.Add(LV)));
            }
            else
            {
                this.SalesLogView.Items.Add(LV);
            }
            Application.DoEvents();
        }

        public void AddProductButton_Click(object sender, EventArgs e)
        {
            AddProduct AP = new AddProduct(this);
            AP.ShowDialog();
        }

        public void thumbnailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.thumbnailsToolStripMenuItem.Checked = true;
            this.tilesToolStripMenuItem.Checked = false;
            this.iconsToolStripMenuItem.Checked = false;
            this.listToolStripMenuItem.Checked = false;
            this.detailsToolStripMenuItem.Checked = false;


            if (this.ProductsList.InvokeRequired)
            {
                this.ProductsList.BeginInvoke(new MethodInvoker(
                    () =>
                                  this.ProductsList.View = View.LargeIcon
                        )
                        );
            }
            else
            {
                this.ProductsList.View = View.LargeIcon;
            }
        }

        public void tilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.thumbnailsToolStripMenuItem.Checked = false;
            this.tilesToolStripMenuItem.Checked = true;
            this.iconsToolStripMenuItem.Checked = false;
            this.listToolStripMenuItem.Checked = false;
            this.detailsToolStripMenuItem.Checked = false;

            if (this.ProductsList.InvokeRequired)
            {
                this.ProductsList.BeginInvoke(new MethodInvoker(
                    () =>
                                           this.ProductsList.View = View.Tile
                        )
                        );
            }
            else
            {
                this.ProductsList.View = View.Tile;
            }
        }

        public void iconsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.thumbnailsToolStripMenuItem.Checked = false;
            this.tilesToolStripMenuItem.Checked = false;
            this.iconsToolStripMenuItem.Checked = true;
            this.listToolStripMenuItem.Checked = false;
            this.detailsToolStripMenuItem.Checked = false;

            if (this.ProductsList.InvokeRequired)
            {
                this.ProductsList.BeginInvoke(new MethodInvoker(
                    () =>
                                          this.ProductsList.View = View.SmallIcon
                        )
                        );
            }
            else
            {
                this.ProductsList.View = View.SmallIcon;
            }
        }

        public void listToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.thumbnailsToolStripMenuItem.Checked = false;
            this.tilesToolStripMenuItem.Checked = false;
            this.iconsToolStripMenuItem.Checked = false;
            this.listToolStripMenuItem.Checked = true;
            this.detailsToolStripMenuItem.Checked = false;

            if (this.ProductsList.InvokeRequired)
            {
                this.ProductsList.BeginInvoke(new MethodInvoker(
                    () =>
                                                     this.ProductsList.View = View.List
                        )
                        );
            }
            else
            {
                this.ProductsList.View = View.List;
            }
        }

        public void detailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.thumbnailsToolStripMenuItem.Checked = false;
            this.tilesToolStripMenuItem.Checked = false;
            this.iconsToolStripMenuItem.Checked = false;
            this.listToolStripMenuItem.Checked = false;
            this.detailsToolStripMenuItem.Checked = true;

            if (this.ProductsList.InvokeRequired)
            {
                this.ProductsList.BeginInvoke(new MethodInvoker(
                    () =>
                                                              this.ProductsList.View = View.Details
                        )
                        );
            }
            else
            {
                this.ProductsList.View = View.Details;
            }
        }
        /*
        private void Backup()
        {
            string constring = "server=localhost;user=root;pwd=qwerty;database=test;";
            string file = "C:\\backup.sql";
            using (MySqlConnection conn = new MySqlConnection(constring))
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    using (MySqlBackup mb = new MySqlBackup(cmd))
                    {
                        cmd.Connection = conn;
                        conn.Open();
                        mb.ExportToFile(file);
                        conn.Close();
                        mb.Dispose();
                    }
                }
            }
        }

        private void Restore()
        {
            string constring = "server=localhost;user=root;pwd=qwerty;database=test;";
            string file = "C:\\backup.sql";
            using (MySqlConnection conn = new MySqlConnection(constring))
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    using (MySqlBackup mb = new MySqlBackup(cmd))
                    {
                        cmd.Connection = conn;
                        conn.Open();
                        mb.ImportFromFile(file);
                        conn.Close();
                    }
                }
            }
        }
        */
    }
}