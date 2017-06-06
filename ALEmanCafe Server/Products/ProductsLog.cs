using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace ALEmanCafe_Server.Products
{
    public class ProductsLog
    {
        public static List<ProductsLog> AllLogs = new List<ProductsLog>();

        public ProductsLog(){ }
        public ProductsLog(MySqlDataReader MSDA)
        {
            this.ID = MSDA.GetInt32(0);
            this.Quantity = MSDA.GetInt32(1);
            this.Price = MSDA.GetFloat(2);
            this.At = new DateTime(MSDA.GetInt64(3));
            if (MSDA.GetString(4) == "0")
                this.LogType = LogTypes.Sold;
            else
                this.LogType = LogTypes.Returned;
        }
        public int ID;
        public int Quantity;
        public float Price;
        public DateTime At;
        public LogTypes LogType;

        public enum LogTypes
        {
            Sold,//0
            Returned//1
        }
    }
}
