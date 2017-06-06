using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Text;
using MySql.Data.MySqlClient;
using ALEmanCafe_Server.Products;
using ALEmanCafe_Server.Other;

namespace ALEmanCafe_Server
{
    public class MyDatabase
    {
        public static ALEmanCafe ALEmancafe;
        public static string DatabaseDirection = Application.StartupPath + @"\\Config.db";
        public static bool LoadDatabaseConfig()
        {
            if (File.Exists(DatabaseDirection) == false)
            {
                SaveDatabaseConfig();
                //    MessageBox.Show("New");
            }
            else
            {
                byte[] bytes = File.ReadAllBytes(DatabaseDirection);
                Cryptor.Decrypt(ref bytes);
                string[] Value = System.Text.ASCIIEncoding.ASCII.GetString(bytes).Split(';');
                //   MessageBox.Show("bytes: " + bytes);
                //   MessageBox.Show("old: " + Program.DatabaseServer + ","+ Program.DatabaseUsername + "," + Program.DatabasePassword + Environment.NewLine + Value[0] + Environment.NewLine + Value[1] + Environment.NewLine + Value[2]);
                Program.DatabaseServer = Value[0];
                Program.DatabaseUsername = Value[1];
                Program.DatabasePassword = Value[2];
            }
            return true;
        }
        public static bool SaveDatabaseConfig()
        {
            string Value = Program.DatabaseServer + ";" + Program.DatabaseUsername + ";" + Program.DatabasePassword;
            // MessageBox.Show("Save: " + Value);
            byte[] bytes = Encoding.Default.GetBytes(Value);
            Cryptor.Encrypt(ref bytes);
            File.WriteAllBytes(DatabaseDirection, bytes);
            return true;
        }


        public static bool CreateDB(bool Test = false)
        {
            MySqlConnection conn = new MySqlConnection(Program.connStr);
            string s0 = "CREATE DATABASE IF NOT EXISTS aleman_cafe_server DEFAULT CHARACTER SET utf8 DEFAULT COLLATE utf8_general_ci; " +
            "use `aleman_cafe_server`; ";

            s0 += "CREATE TABLE IF NOT EXISTS `products` (" +
            "`id` INT AUTO_INCREMENT, " +
            "`name` VARCHAR(255) NOT NULL Unique," +
            "`type` VARCHAR(255)," +
            "`price` FLOAT(9,2) DEFAULT '0'," +
            "`purchasingprice` FLOAT(9,2) DEFAULT '0'," +
            "`count` Integer(9) DEFAULT '0', " +
            "`noofsales` Integer(9) DEFAULT '0', " +
            "`needed` Integer(5) DEFAULT '0', " +
            "`details` Text, " +
            "`picurl` Text, " +
            "PRIMARY KEY(id)); ";
            s0 += "CREATE TABLE IF NOT EXISTS `productslog` (" +
                "`id` INT(9)," +
                "`quantity` Integer(9)," +
                "`price` FLOAT(9,2)," +
                "`datetime` NUMERIC(21,0)," +
                "`type` Integer(1)); ";
            s0 += "CREATE TABLE IF NOT EXISTS `timers` (" +
        "`canuseinternet` Integer(1)," +
        "`paid` Integer(1)," +
        "`limitedtime` INT(9)," +
        "`usedtime` INT(9)," +
         "`remainingtime` INT(9)," +
         "`starttime` NUMERIC(21,0)," +
         "`stopedtime` NUMERIC(21,0)," +
         "`ip` varchar(50)," +
         "`macaddress` varchar(55)," +
         "`hostname` varchar(255) UNIQUE," +
          "`shownname` varchar(255) UNIQUE," +
            "`timestatus` Integer(3)," +
         "`pausedtime` NUMERIC(21,0));";
         ;

            s0 += "CREATE TABLE IF NOT EXISTS `systemlog` (" +
     "`hostname` VARCHAR(255)," +
          "`shownname` VARCHAR(255)," +
     "`price` FLOAT(9,2)," +
     "`starttime` NUMERIC(21,0)," +
     "`timestatu` INTEGER(1)," +
      "`usedtime` Integer(9), "+
         "PRIMARY KEY(starttime)); ";

            s0 += "CREATE TABLE IF NOT EXISTS `systeminfo` (" +
"`user` VARCHAR(255)," +
"`pass` VARCHAR(255)," +
"`view` VARCHAR(25)," +
"`smallicon` INTEGER(1)," +
"`grid` INTEGER(1)," +
"`statusbar` INTEGER(1)," +

"`menubar` INTEGER(1)," +
"`RunServerAtStartup` INTEGER(1)," +
"`Askpasswordonstartup` INTEGER(1)," +
"`EnableUSBPluginWarning` INTEGER(1)," +
"`EnableUSBPlugoutWarning` INTEGER(1)," +
"`HourPrice` DOUBLE(5,2) DEFAULT '1.50'," +
"`MinimumCost` DOUBLE(5,2) DEFAULT '0.50');";
     
            try
            {
                conn.Open();
                new MySqlCommand(s0, conn).ExecuteNonQuery();
            }
            catch (MySqlException me)
            {
                try { if (conn != null) conn.Close(); } catch { }
                if (!Test)
                {
                    if (Program.isAdmin)
                        MessageBox.Show(me.Message + Environment.NewLine + Program.connStr);
                    else
                        MessageBox.Show(me.Message);
                }
                else
                {
                 //   MessageBox.Show(me.Message + Environment.NewLine + Program.connStr);
                    DatabaseConfiguration DC = new DatabaseConfiguration(ALEmancafe);
                    DC.ShowDialog();
                    DC.Dispose();
                }
                return false;
            }
            conn.Close();
            return true;
            //CreateNewDB();
        }

        public static bool Load()
        {
            MySqlConnection conn = new MySqlConnection(Program.connStr);
            conn = new MySqlConnection(Program.connStr);
            conn.Open();
            string s0 = "use `aleman_cafe_server`; SELECT * FROM `systeminfo`";
            MySqlCommand cmd = new MySqlCommand(s0, conn);
            try
            {
                bool loaded = false;
                var rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    loaded = true;
                    Program.MyUserName = rdr.GetString(0);
                    Program.MyPassword = rdr.GetString(1);
                    if (rdr.GetString(2) == "iconic")
                        ALEmancafe.Server.iconicToolStripMenuItem_Click(null, null);
                    else
                        ALEmancafe.Server.detailedToolStripMenuItem_Click(null, null);

                    if (rdr.GetInt16(3) > 0)
                        ALEmancafe.Server.useSmallIconsToolStripMenuItem_Click(null, null);

                    if (rdr.GetInt16(4) > 0)
                        ALEmancafe.Server.gridToolStripMenuItem_Click(null, null);

                    if (rdr.GetInt16(5) > 0)
                        ALEmancafe.Server.statusbarToolStripMenuItem_Click(null, null);
                
                    if (rdr.GetInt16(6) > 0)
                        ALEmancafe.Server.StatusMenuToolStripMenuItem_Click(null, null);

                    if (rdr.GetInt16(7) > 0)
                        Program.RunServerAtStartup = true;
                    else
                        Program.RunServerAtStartup = false;

                    if (rdr.GetInt16(8) > 0)
                        Program.Askpasswordonstartup = true;
                    else
                        Program.Askpasswordonstartup = false;

                    if (rdr.GetInt16(9) > 0)
                        Program.EnableUSBPluginWarning = true;
                    else
                        Program.EnableUSBPluginWarning = false;

                    if (rdr.GetInt16(10) > 0)
                        Program.EnableUSBPlugoutWarning = true;
                    else
                        Program.EnableUSBPlugoutWarning = false;

                    Program.HourPrice = rdr.GetDouble(11);
                    Program.MinimumCost = rdr.GetDouble(12);
           

                }
                rdr.Close();
                conn.Close();
                if (!loaded)
                {
                    CreateNewDB();
                    Load();
                }
            }
            catch (Exception ee)
            {
                conn.Close();
                //  MessageBox.Show("There are an error and database cannot be loaded." + Environment.NewLine + ee.ToString(), "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (MessageBox.Show("There are an error and systemlog database cannot be loaded, do you want to create new database?" + Environment.NewLine + (Program.isAdmin ? ee.ToString() : ee.Message), "Database Error", MessageBoxButtons.YesNo, MessageBoxIcon.Error) == DialogResult.Yes)
                    if (Program.isAdmin)
                    CreateDB();
                LoadSystemlog();
                return false;
            }
            LoadSystemlog();
            return true;
        }

        private static bool LoadSystemlog()
        {

            MySqlConnection conn = new MySqlConnection(Program.connStr);
            conn = new MySqlConnection(Program.connStr);
            conn.Open();
            string s0 = "use `aleman_cafe_server`; SELECT * FROM `systemlog`";
            MySqlCommand cmd = new MySqlCommand(s0, conn);
            try
            {
                var rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    PaymentInfo PI = new PaymentInfo();
                    PI.Hostname = rdr.GetString(0);
                    PI.Showname = rdr.GetString(1);
                    PI.Price = rdr.GetDouble(2);
                    PI.StartTime = new DateTime(rdr.GetInt64(3));
                    PI.TimeStatu = rdr.GetString(4) == "0" ? PaymentInfo.TimeStatus.Login : PaymentInfo.TimeStatus.Time;
                    PI.UsedTime = rdr.GetUInt32(5);
                    NetworkItems.PaymentInfos.Add(PI.StartTime.Ticks, PI);
                }
                rdr.Close();
                conn.Close();
            }
            catch (Exception ee)
            {
                conn.Close();
                //  MessageBox.Show("There are an error and database cannot be loaded." + Environment.NewLine + ee.ToString(), "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (MessageBox.Show("load systemlog, There are an error and systemlog database cannot be loaded, do you want to create new database?" + Environment.NewLine + (Program.isAdmin ? ee.ToString() : ee.Message), "Database Error", MessageBoxButtons.YesNo, MessageBoxIcon.Error) == DialogResult.Yes)
                    if (Program.isAdmin)
                    CreateDB();
                return false;
            }

            ALEmancafe.Server.RefreshSystemLogMenu();
            // LoadSystemlogTimers();
            return true;
        }

        public static bool LoadSystemlogTimers()
        {
            MySqlConnection conn = new MySqlConnection(Program.connStr);
            conn = new MySqlConnection(Program.connStr);
            conn.Open();
            string s0 = "use `aleman_cafe_server`; SELECT * FROM `timers`";
            MySqlCommand cmd = new MySqlCommand(s0, conn);
            try
            {
                var rdr = cmd.ExecuteReader();
                bool CanUseInternet = true, Paid = false;
                uint LimitedTime, UsedTime, RemainingTime;
                DateTime StartTime, StopedTime;
                long PausedTime;
                string IP = "", MacAddress = "", HostName, ShownName;

                while (rdr.Read())
                {
                    if (rdr.GetInt16(0) == 0)
                        CanUseInternet = false;
                    else
                        CanUseInternet = true;
                    if (rdr.GetInt16(1) == 0)
                        Paid = false;
                    else
                        Paid = true;
                    LimitedTime = rdr.GetUInt32(2);
                    UsedTime = rdr.GetUInt32(3);
                    RemainingTime = rdr.GetUInt32(4);
                    StartTime = new DateTime(rdr.GetInt64(5));
                    StopedTime = new DateTime(rdr.GetInt64(6));
                    IP = rdr.GetString(7);
                    MacAddress = rdr.GetString(8);
                    HostName = rdr.GetString(9);
                    ShownName = rdr.GetString(10);
                    PausedTime = rdr.GetInt64(11);
                    if (StopedTime.Year.ToString() == "1" && StartTime.Year.ToString() != "1")
                    {
                        StopedTime = new DateTime(StartTime.AddMinutes(UsedTime).Ticks + PausedTime);
                    }
                    NetworkItems NI = new NetworkItems();
                    NI.IP = IP;
                    NI.MacAddress = MacAddress;
                    NI.CanUseInternet = CanUseInternet;
                    NI.HostName = HostName;
                    NI.ShownName = ShownName;
                    NI.Paid = Paid;
                    NI.LimitedTime = LimitedTime;
                    NI.UsedTime = UsedTime;
                    NI.RemainingTime = RemainingTime;
                    NI.PausedTime = PausedTime;
                    NI.StartTime = StartTime;
                    NI.StopedTime = StopedTime;
                    if (rdr.GetString(11) == "0")
                        NI.TimeStatu = NetworkItems.TimeStatus.None;
                    else if (rdr.GetString(11) == "1")
                        NI.TimeStatu = NetworkItems.TimeStatus.Login;
                    else if (rdr.GetString(11) == "2")
                        NI.TimeStatu = NetworkItems.TimeStatus.Time;
                    else
                        NI.TimeStatu = NetworkItems.TimeStatus.None;
                 //   if (NI.TimeStatu != NetworkItems.TimeStatus.None && NI.StopedTime.Year.ToString() != "1")
                   //     NI.Paused = true;
          //          MessageBox.Show(IP + ", " + MacAddress + ", " + NI.TimeStatu);
                    NetworkItems.OldTimers.Add(NI.IP, NI);
                }
                rdr.Close();
                conn.Close();
            }
            catch (Exception ee)
            {
                conn.Close();
                //  MessageBox.Show("There are an error and database cannot be loaded." + Environment.NewLine + ee.ToString(), "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (MessageBox.Show("There are an error and database cannot be loaded, do you want to create new database?" + Environment.NewLine + (Program.isAdmin ? ee.ToString() : ee.Message), "Database Error", MessageBoxButtons.YesNo, MessageBoxIcon.Error) == DialogResult.Yes)
                    if (Program.isAdmin)
                    CreateDB();
                return false;
            }
            return true;
        }



        public static void Save()
        {
            MySqlConnection conn = new MySqlConnection(Program.connStr);
            string s0 = "use `aleman_cafe_server`; UPDATE `systeminfo` SET" +
                " user='" + Program.MyUserName +
                "', pass='" + Program.MyPassword +
                "', view='" + (ALEmancafe.Server.iconicToolStripMenuItem.Checked ? "iconic', " : "detailed', ") +
                "smallicon='" + (ALEmancafe.Server.useSmallIconsToolStripMenuItem.Checked ? "1', " : "0', ") +
                 "grid='" + (ALEmancafe.Server.gridToolStripMenuItem.Checked ? "1', " : "0', ") +
                  "statusbar='" + (ALEmancafe.Server.statusbarToolStripMenuItem.Checked ? "1', " : "0', ") +

                       "menubar='" + (ALEmancafe.Server.statusbarToolStripMenuItem.Checked ? "1', " : "0', ") +
                            "RunServerAtStartup='" + (Program.RunServerAtStartup ? "1', " : "0', ") +
                                 "Askpasswordonstartup='" + (Program.Askpasswordonstartup ? "1', " : "0', ") +
                                      "EnableUSBPluginWarning='" + (Program.EnableUSBPluginWarning ? "1', " : "0', ") +
                        "EnableUSBPlugoutWarning='" + (Program.EnableUSBPlugoutWarning ? "1', " : "0', ") +

                              "HourPrice='" + Program.HourPrice + "', " +
                                      "MinimumCost='" + Program.MinimumCost + "';";
            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(s0, conn);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ee) { conn.Close(); Save(); if (Program.isAdmin) MessageBox.Show("Error: " + ee.ToString()); else MessageBox.Show("Error: " + ee.Message); return; }
            conn.Close();
        }

        public static void SaveSystemlog(PaymentInfo PI)
        {
            MySqlConnection conn = new MySqlConnection(Program.connStr);
            byte ts = 0;
            if (PI.TimeStatu == PaymentInfo.TimeStatus.Time)
                ts = 1;
            string s0 = "use `aleman_cafe_server`; INSERT INTO `systemlog` (hostname, shownname, price, starttime, timestatu, usedtime) VALUES";

                         s0 += "('" + PI.Hostname + "', '" +
                       PI.Showname + "', '" +
   PI.Price + "', '" +
 PI.StartTime.Ticks + "', '" +
 ts + "', '" +
   PI.UsedTime + "') ON DUPLICATE KEY UPDATE hostname=VALUES(hostname), shownname=VALUES(shownname), price=VALUES(price), starttime=VALUES(starttime), timestatu=VALUES(timestatu), usedtime=VALUES(usedtime);";
           
            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(s0, conn);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ee) { conn.Close(); if (Program.isAdmin) MessageBox.Show("systemlog - Error: " + ee.ToString()); else MessageBox.Show("SysLog Error: " + ee.Message); return; }
            conn.Close();
        }

        public static void OLD_SaveSystemlog()
        {
            MySqlConnection conn = new MySqlConnection(Program.connStr);
            string s0 = "use `aleman_cafe_server`; INSERT INTO `systemlog` (hostname, shownname, price, starttime, timestatu, usedtime) VALUES";
            foreach (PaymentInfo PI in NetworkItems.PaymentInfos.Values)
            {
                byte ts = 0;
                if (PI.TimeStatu == PaymentInfo.TimeStatus.Time)
                    ts = 1;
                s0 += "('" + PI.Hostname + "', '" +
                       PI.Showname + "', '" +
   PI.Price + "', '" +
 PI.StartTime.Ticks + "', '" +
 ts + "', '" +
   PI.UsedTime + "'),";
            }
            s0 = s0.Remove(s0.Length - 1);
            s0 += ";";
            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(s0, conn);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ee) { conn.Close(); if (Program.isAdmin) MessageBox.Show("systemlog - Error: " + ee.ToString()); else MessageBox.Show("SysLog Error: " + ee.Message); return; }
            conn.Close();
        }

        public static void SaveSystemlogTimers()
        {
            foreach (NetworkItems NI in NetworkItems.ALLNetworks.Values)
            {
                MySqlConnection conn = new MySqlConnection(Program.connStr);
             
                /*
                INSERT INTO AggregatedData(datenum, Timestamp)
VALUES("734152.979166667", "2010-01-14 23:30:00.000")
ON DUPLICATE KEY UPDATE
  Timestamp = VALUES(Timestamp)
                    */
                string CanUseInternet, Paid;
                if (NI.CanUseInternet)
                    CanUseInternet = "1";
                else
                    CanUseInternet = "0";
                if (NI.Paid)
                    Paid = "1";
                else
                    Paid = "0";

                byte TimeStatu = 0;
                if (NI.TimeStatu == NetworkItems.TimeStatus.Login)
                    TimeStatu = 1;
                else if (NI.TimeStatu == NetworkItems.TimeStatus.Time)
                    TimeStatu = 2;

                string s0 = "use `aleman_cafe_server`; INSERT INTO `timers` (canuseinternet, paid, limitedtime, usedtime, remainingtime, starttime, stopedtime, ip, macaddress, hostname, shownname, timestatus, pausedtime) " +
   "VALUES('" + CanUseInternet + "', '" +
   Paid + "', '" +
   NI.LimitedTime + "', '" +
    NI.UsedTime + "', '" +
   NI.RemainingTime + "', '" +
   NI.StartTime.Ticks + "', '" +
   NI.StopedTime.Ticks + "', '" +
   NI.IP + "', '" +
    NI.MacAddress + "', '" +
     NI.HostName + "', '" +
          NI.ShownName + "', '" +
      TimeStatu + "', '" +
      NI.PausedTime + "') ON DUPLICATE KEY UPDATE canuseinternet=VALUES(canuseinternet), paid=VALUES(paid), limitedtime=VALUES(limitedtime), usedtime=VALUES(usedtime), remainingtime=VALUES(remainingtime), starttime=VALUES(starttime), stopedtime=VALUES(stopedtime), ip=VALUES(ip), macaddress=VALUES(macaddress), hostname=VALUES(hostname), shownname=VALUES(shownname), timestatus=VALUES(timestatus), pausedtime=VALUES(pausedtime);";
                try
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(s0, conn);
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ee) { conn.Close(); if (Program.isAdmin) MessageBox.Show("timers Error: " + ee.ToString()); else MessageBox.Show("Timer Error: " + ee.Message); return; }
                conn.Close();
            }
        }

        public static void RemovePC(NetworkItems NI)
        {
            MySqlConnection conn = new MySqlConnection(Program.connStr);


            string s0 = "use `aleman_cafe_server`;  DELETE FROM `timers` WHERE hostname='" + NI.HostName + "';"; ;
            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(s0, conn);
                cmd.ExecuteNonQuery();
            }
            catch { conn.Close(); return; }
            conn.Close();
        }

        public static void CreateNewDB()
        {
            MySqlConnection conn = new MySqlConnection(Program.connStr);
            string s0 = "use `aleman_cafe_server`; INSERT INTO `systeminfo` (user, pass, view, smallicon, grid, statusbar, menubar) VALUES";
            s0 += "('" + Program.MyUserName + "', '" +
Program.MyPassword + "', '";
            /*
            if (ALEmancafe.Server.iconicToolStripMenuItem.Checked)
                s0 += "iconic', '";
            else
                s0 += "detailed', '";
            */
            s0 += "iconic', '";

            s0 += "0', '1', '1', '1');";
        //  s0 += (ALEmancafe.Server.useSmallIconsToolStripMenuItem.Checked ? "1" : "0") + "', '" +
        //(ALEmancafe.Server.gridToolStripMenuItem.Checked ? "1" : "0") + "', '" +
           //  (ALEmancafe.Server.statusbarToolStripMenuItem.Checked ? "1" : "0") + "', '" +
            //   (ALEmancafe.Server.StatusMenuToolStripMenuItem.Checked ? "1" : "0") + "');";
            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(s0, conn);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ee) { conn.Close(); if (Program.isAdmin) MessageBox.Show("SYSInfo Error: " + ee.ToString()); else MessageBox.Show("SYSInfo Error: " + ee.Message); return; }
            conn.Close();

            /*
            string Value = "User=" + Environment.NewLine;

            Value += "Pass=" + Environment.NewLine;

            Value += "View=detailed" + Environment.NewLine;

            Value += "SmallIcon=false" + Environment.NewLine;

            Value += "grid=true" + Environment.NewLine;

    //        Value += "menu=false" + Environment.NewLine;

            Value += "statusbar=true" + Environment.NewLine;

            Value += "menubar=true";// +Environment.NewLine;
          //  SetLoads(Value.Split(Environment.NewLine.ToCharArray()));
            byte[] bytes = Encoding.Default.GetBytes(Value);
            Cryptor.Encrypt(ref bytes);
            File.WriteAllBytes(DatabaseDirection, bytes);
            */
        }
    }
}