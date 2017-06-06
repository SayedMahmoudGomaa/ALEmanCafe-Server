using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Diagnostics;
using System.Drawing;
using System.Net.Sockets;
using MouseKeyboardLibrary;
using System.Runtime.InteropServices;
using System.Collections;
using Microsoft.Win32;
using System.Threading;
using System.Net;
using NetFwTypeLib;
using System.Net.NetworkInformation;
using System.Security;

namespace ALEmanCafe_Server
{
    static class Program
    {
        [DllImport("user32.dll")]
        static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool SetForegroundWindow(IntPtr hWnd);
        [DllImport("user32.dll")]
        private static extern Boolean ShowWindow(IntPtr hWnd, Int32 nCmdShow);

        [STAThread]
        static void Main()
        {
            #region CloseOtherHC
            Process CP = Process.GetCurrentProcess();
            foreach (Process p in Process.GetProcessesByName(CP.ProcessName))
            {
                if (p.Id != CP.Id)
                {
                    SetForegroundWindow(p.MainWindowHandle);
                    ShowWindow(p.MainWindowHandle, 3);//3
                    Application.Exit();
                    Environment.Exit(0);
                    return;
                }
            }
            #endregion
            #region WFW
            /*
            UdpClient udpClient = new UdpClient(716);
            udpClient.Close();
            bool NotDone = true;
            byte nn = 0;
            while (NotDone || nn < 11)
            {
                nn++;
                byte count = 0;
                foreach (Process p in Process.GetProcesses())
                {
                    try
                    {
                        if (p == null) continue;
                        else if (p.MainModule == null) continue;
                        if (p.ProcessName == "rundll32" || p.MainWindowTitle == "Windows Security Alert" || Path.GetFileName(p.MainModule.FileName) == "rundll32")
                        {
                            count++;
                            SetWindowPos(p.MainWindowHandle, new IntPtr(-1), 0, 0, 0, 0, 0x0002 | 0x0001 | 0x0040);
                            KeyboardSimulator.KeyPress(Keys.U);
                            if (Process.GetProcessById(p.Id) == null)
                                NotDone = false;
                            continue;
                        }
                    }
                    catch { continue; }
                }
                if (count < 1)
                    NotDone = false;
            }
             */
            #endregion

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            AuthorizeApplication("ALEmanCafe", Application.ExecutablePath, NET_FW_SCOPE_.NET_FW_SCOPE_ALL, NET_FW_IP_VERSION_.NET_FW_IP_VERSION_ANY);
            AuthorizeApplication("ALEmanCafe Server", Application.ExecutablePath, NET_FW_SCOPE_.NET_FW_SCOPE_ALL, NET_FW_IP_VERSION_.NET_FW_IP_VERSION_ANY);
            ALEmanCafe AC = new ALEmanCafe();


            while (!AC.Server.Loaded)
            {
                Thread.Sleep(1);
            }

            if (Askpasswordonstartup)
                Application.Run(AC);
            else
            {
                AC.Server.LoginNow(false);
                Application.Run(AC.Server);
            }
        }

        public static bool OrLanguage = true;
        public static string DatabaseServer = "localhost", DatabaseUsername = "root", DatabasePassword = "";
        public static RegistryKey key = Registry.LocalMachine.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Run", true);
        public static double HourPrice = 1.50;
        public static double MinimumCost = 0.50;
        public static string connStr
        {
            get
            {
                return "server='" + DatabaseServer + "';user='" + DatabaseUsername + "';password='" + DatabasePassword + "'; CharSet=utf8;";//;port=3306//password=32145678;
            }
        }
        public static bool RunServerAtStartup
        {
            get
            {
                if (key.GetValue(Application.ProductName.Replace(" ", "-")) == null)
                    return false;
                else
                    return true;
            }
            set
            {
                if (value == true)
                {
                    if (key.GetValue(Application.ProductName.Replace(" ", "-")) == null)
                        key.SetValue(Application.ProductName.Replace(" ", "-"), Application.ExecutablePath.ToString());
                }
                else if (key.GetValue(Application.ProductName.Replace(" ", "-")) != null)
                    key.DeleteValue(Application.ProductName.Replace(" ", "-"));
            }
        }
        public static bool Askpasswordonstartup = true, EnableUSBPluginWarning = false, EnableUSBPlugoutWarning = false, isAdmin = false;
        public static string MyUserName = "";
        public static string MyPassword = "";

        public static int PPort = 720;


        public static uint GetUsedTime(DateTime StartTime, DateTime StoppedTime, long pausedtime)
        {
            TimeSpan ST = new TimeSpan(((StoppedTime.Year.ToString() == "1" ? DateTime.Now.Ticks : StoppedTime.Ticks) - (StartTime.Ticks + pausedtime)));//TimeSpan ST = new TimeSpan(((StoppedTime.Year.ToString() == "1" ? DateTime.Now.Ticks : StoppedTime.Ticks) - StartTime.Ticks));
            if (ST.TotalMinutes > 0)
                return (uint)ST.TotalMinutes;
            else
                return 0;
        }
        /*
        public static uint GetRemainingTime(DateTime StartTime, uint TotalTime)
        {
            TimeSpan ST = new TimeSpan(StartTime.AddMinutes(TotalTime).Ticks - DateTime.Now.Ticks);
            if (ST.TotalMinutes > 0)
                return (uint)ST.TotalMinutes;
            else
                return 0;
        }
        */
        public static string GetUsedAndRemTime(uint Minutes)
        {
            string Res = "00:00";
            int Hours = 0;
            if (Minutes > 0)
            {
                while (Minutes >= 60)
                {
                    Minutes -= 60;
                    Hours++;
                }
                if (Hours < 1)
                    Res = "00:";
                else if (Hours < 10)
                    Res = "0" + Hours + ":";
                else
                    Res = Hours + ":";

                if (Minutes < 1)
                    Res += "00";
                else if (Minutes < 10)
                    Res += "0" + Minutes;
                else
                    Res += Minutes;
            }
            return Res;
        }

        public static string GetUsageCost(uint Minutes)
        {
            uint TM = Minutes;
            if (TM <= 0)
                return "0000 جم";

            double Price = 0;
            double ss = HourPrice * 10 / 60;
            while (TM >= 10)
            {
                Price += ss;
                TM -= 10;
            }
            if (TM > 2)
                Price += ss;

            if (Price < Program.MinimumCost)
                Price = Program.MinimumCost;
            string lastp = Price + "";
            if (!lastp.Contains("."))
                lastp = lastp + ".00";
            else if (lastp.Split('.')[1].Length < 2)
                lastp = lastp + "0";

            return lastp + " جم";
        }

        /*
        public static void CopyListsToList(Dictionary<string, NetworkItems> From, Dictionary<string, NetworkItems> To)
        {
            foreach (NetworkItems NI in From.Values)
            {
                To.Add(NI.IP, NI);
            }
        }
        */
        public static List<Process> GetRunningApps()
        {
            List<Process> Pc = new List<Process>();
            Process[] processes = Process.GetProcesses();
            foreach (Process p in processes)
            {
                if (p.ProcessName.ToLower() == "explorer" ||
                   p.ProcessName.ToLower() == "devenv" ||
                      p.ProcessName == Application.ProductName
                   ) continue;

                if (!string.IsNullOrEmpty(p.MainWindowTitle))
                {
                    Pc.Add(p);
                }
            }
            return Pc;
        }
        public static List<ALEmanCafe_Server.RemoteApplications.AppInfo> GetRunningAppsInfo()
        {
            List<RemoteApplications.AppInfo> AppsInfo = new List<RemoteApplications.AppInfo>();
            foreach (Process p in GetRunningApps())
            {
                RemoteApplications.AppInfo AI = new RemoteApplications.AppInfo();
                AI.AppICon = Icon.ExtractAssociatedIcon(p.MainModule.FileName);
                AI.AppName = p.ProcessName;
                AI.AppPath = p.MainModule.FileName;
                AI.AppTitle = p.MainWindowTitle;
                AppsInfo.Add(AI);
            }
            return AppsInfo;
        }

        public static string GerRanChars(int Len)
        {
            string val = "";
            Random r = new Random();
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789abcdefghijklmnopqrstuvwxyz";
            for (int i = 0; i < Len; i++)
            {
                val += chars[r.Next(chars.Length)];
            }
            /*   new string(Enumerable.Repeat(chars, 10)
                   .Select(s => s[r.Next(s.Length)]).ToArray());*/
            return val;
        }

        #region Firewall
        /* Com refrences
        using NATUPNPLib;
using NETCONLib;
using NetFwTypeLib;
        */
        public static bool AuthorizeApplication(string title, string applicationPath, NET_FW_SCOPE_ scope, NET_FW_IP_VERSION_ ipVersion)
        {
            Type type = Type.GetTypeFromProgID("HNetCfg.FwAuthorizedApplication");
            INetFwAuthorizedApplication auth = Activator.CreateInstance(type)
                as INetFwAuthorizedApplication;
            auth.Name = title;
            auth.ProcessImageFileName = applicationPath;
            auth.Scope = scope;
            auth.IpVersion = ipVersion;
            auth.Enabled = true;

            INetFwMgr manager = GetFirewallManager();

            if (manager == null) return false;

            if (!manager.LocalPolicy.CurrentProfile.FirewallEnabled)
                manager.LocalPolicy.CurrentProfile.FirewallEnabled = true;

            try
            {
                manager.LocalPolicy.CurrentProfile.AuthorizedApplications.Add(auth);
            }
            catch// (Exception ex)
            {
                //   MessageBox.Show(ex.ToString());
                return false;
            }
            return true;
        }
        private static NetFwTypeLib.INetFwMgr GetFirewallManager()
        {
            Type objectType = Type.GetTypeFromCLSID(new Guid("{304CE942-6E39-40D8-943A-B913C40C9CD4}"));
            try
            {
                return Activator.CreateInstance(objectType) as NetFwTypeLib.INetFwMgr;
            }
            catch { return null; }
        }
        /*
        public static bool GloballyOpenPort(string title, int portNo,
    NET_FW_SCOPE_ scope, NET_FW_IP_PROTOCOL_ protocol,
    NET_FW_IP_VERSION_ ipVersion)
        {
            Type type = Type.GetTypeFromProgID("HNetCfg.FWOpenPort");
            INetFwOpenPort port = Activator.CreateInstance(type)
                as INetFwOpenPort;
            port.Name = title;
            port.Port = portNo;
            port.Scope = scope;
            port.Protocol = protocol;
            port.IpVersion = ipVersion;
            INetFwMgr manager = GetFirewallManagerCached();
            try
            {
                manager.LocalPolicy.CurrentProfile.GloballyOpenPorts.Add(port);
            }
            catch //(Exception ex)
            {
                return false;
            }
            return true;
        }
        */
        #endregion
    }


    public static class Cryptor
    {
        private static byte[] key = new byte[] {
            0xad, 0x6b, 0x4f, 0xfb, 0xdd, 0xb8, 14, 9, 0x13, 0x33, 0x8f, 0xf5, 0x43, 9, 0x15, 0x88,
            0x5d, 0x80, 0xa3, 0x45, 0x2d, 0x42, 8, 0x56, 0x80, 0xf8, 0x19, 0xc5, 0x88, 0x1b, 0x3e, 0xef,
            0x81, 7, 0x30, 0x36, 0x95, 0x52, 0, 0xf7, 0xfd, 0x5b, 0x5c, 0xbc, 0x6a, 0x26, 14, 0xb2,
            0xa3, 0x67, 0xc5, 0x5d, 0x6f, 220, 0x18, 0x8a, 0xb5, 0xe0, 200, 0x85, 0xe2, 0x3e, 0x45, 0x8d,
            0x8b, 0x43, 0x74, 0x85, 0x54, 0x17, 0xb0, 0xec, 0x10, 0x4d, 15, 15, 0x29, 0xb8, 230, 0x7d,
            0x42, 0x80, 0x8f, 0xbc, 0x1c, 0x76, 0x69, 0x3a, 0xb6, 0xa5, 0x21, 0x86, 0xb9, 0x29, 0x30, 0xc0,
            0x12, 0x45, 0xa5, 0x4f, 0xe1, 0xaf, 0x25, 0xd1, 0x92, 0x2e, 0x30, 0x58, 0x49, 0x67, 0xa5, 0xd3,
            0x84, 0xf4, 0x89, 0xca, 0xfc, 0xb7, 4, 0x4f, 0xcc, 110, 0xac, 0x31, 0xd4, 0x87, 7, 0x72
         };

        public static void Decrypt(ref byte[] bytes)
        {
            if (bytes.Length >= 0)
            {
                int index = 0;
                byte num2 = 0;
                while (index < bytes.Length)
                {
                    num2 = (byte)(bytes[index] ^ key[((byte)index) % 0x80]);
                    int num3 = index % 8;
                    bytes[index] = (byte)((num2 << (8 - num3)) + (num2 >> num3));
                    index++;
                }
            }
        }

        public static void Encrypt(ref byte[] bytes)
        {
            if (bytes.Length >= 0)
            {
                int index = 0;
                byte num2 = 0;
                while (index < bytes.Length)
                {
                    int num3 = index % 8;
                    num2 = (byte)((bytes[index] >> (8 - num3)) + (bytes[index] << num3));
                    bytes[index] = (byte)(num2 ^ key[((byte)index) % 0x80]);
                    index++;
                }
            }
        }
    }

    class ListViewItemComparer : IComparer
    {
        private int col;
        private SortOrder order;
        public ListViewItemComparer()
        {
            col = 0;
            order = SortOrder.Ascending;
        }
        public ListViewItemComparer(int column, SortOrder order)
        {
            col = column;
            this.order = order;
        }
        public int Compare(object x, object y)
        {
            int returnVal = -1;
            returnVal = String.Compare(((ListViewItem)x).SubItems[col].Text,
                                    ((ListViewItem)y).SubItems[col].Text);
            // Determine whether the sort order is descending.
            if (order == SortOrder.Descending)
                // Invert the value returned by String.Compare.
                returnVal *= -1;
            return returnVal;
        }
    }

}