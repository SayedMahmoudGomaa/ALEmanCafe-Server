using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace ALEmanCafe_Server.Products
{
    public class ProductsInfos
    {
        public static Dictionary<string, ProductsInfos> AllProducts = new Dictionary<string, ProductsInfos>();
        public static List<string> PTypes = new List<string>();
        public static string ImagesURL = Application.StartupPath + @"\Images\";
        public static int NextID = 1;
        public static int TotalProducts = 0;

        public int PID;
        public string PName;
        public string PType;
        public float Price;
        public float PPrice;
        public int Count;
        public int NoOfSales;
        public int NeededCount;
        public string Details;
        public string ImageURL;

        public int SoldToday = 0;
        public ProductsInfos()
        {

        }
        public ProductsInfos(MySqlDataReader MSDA)
        {
          this.PID = MSDA.GetInt32(0);
          this.PName = MSDA.GetString(1);
          this.PType = MSDA.GetString(2);
          this.Price = MSDA.GetFloat(3);
          this.PPrice = MSDA.GetFloat(4);
          this.Count = MSDA.GetInt32(5);
          this.NoOfSales = MSDA.GetInt32(6);
          this.NeededCount = MSDA.GetInt32(7);
          this.Details = MSDA.GetString(8);
          this.ImageURL = MSDA.GetString(9);
        }
    }
}