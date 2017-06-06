using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Globalization;
using System.Diagnostics;
using System.Collections;
using System.Windows.Forms;
using System.Net.NetworkInformation;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;
using System.Security;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Threading;

namespace ALEmanCafe_Server
{
    public class NetworkItems
    {
       // public ALEmanCafeServer ACS;
        public NetworkItems(/*ALEmanCafeServer ACS*/)
        {
          //  this.ACS = ACS;
        }
        public static Dictionary<string, NetworkItems> ALLNetworks;
        public static Dictionary<string, NetworkItems> NetworkInfo;// = new Dictionary<string, NetworkItems>();
        public static Dictionary<string, NetworkItems> OfflineNetworkInfo;

        public static Dictionary<string, NetworkItems> IdleNetworks = new Dictionary<string, NetworkItems>();
        public static Dictionary<string, NetworkItems> LoginNetworks = new Dictionary<string, NetworkItems>();
        public static Dictionary<string, NetworkItems> OldTimers = new Dictionary<string, NetworkItems>();
        public static Dictionary<long, PaymentInfo> PaymentInfos = new Dictionary<long, PaymentInfo>();
       // public static List<string> AllIP = new List<string>();
        public static string MyIp = "";
        public string IP = "", MacAddress = "", HostName = "", UserName = "", TotalMemory = "", FreeMemory = "", Harddisc = "", ShownName = "";
        public PCStatus PCStatu = PCStatus.Unknown;
        public enum PCStatus
        {
            Offline,
            Online,
            Unknown
        }


        public RemoteManagement RM = null;
        public string Title = "";
        public string MyOperatingSystem = "Unknown";//"Windows XP Professional SP3"
        public string RunningApplication = "";//NA "Explorer"
        public string UsageCost
        {
            get
            {
                if (this.StartTime == null || this.StartTime.Year.ToString() == "1" || UsedTime <= 0)
                    return "0000 جم";
                else
                {
                    uint TM = this.UsedTime;
                   return Program.GetUsageCost(TM);
                }
            }
        }
        public string TotalCost
        {
            get
            {
                if (this.StartTime == null || this.StartTime.Year.ToString() == "1" || UsedTime <= 0)
                    return "0000 جم";
                else
                {
                    uint TM = this.UsedTime;
                    uint TM2 = this.LimitedTime;
                    if (TimeStatu == TimeStatus.Login)
                        return Program.GetUsageCost(TM);
                    else
                        return Program.GetUsageCost(TM2);
                }
            }
        }
        public string MyVersion = "Unknown";

        public bool Login
        {
            get
            {
                if (LoginNetworks.ContainsKey(this.IP))
                    return true;
                else if (IdleNetworks.ContainsKey(this.IP))
                    return false;
                else
                {
                    IdleNetworks.Add(this.IP, this);
                    return false;
                }
            }
            set
            {
                if (value == true)
                {
                    if (LoginNetworks.ContainsKey(this.IP) == false)
                        LoginNetworks.Add(this.IP, this);
                    if (IdleNetworks.ContainsKey(this.IP))
                        IdleNetworks.Remove(this.IP);
                }
                else
                {
                    if (IdleNetworks.ContainsKey(this.IP) == false)
                        IdleNetworks.Add(this.IP, this);
                    if (LoginNetworks.ContainsKey(this.IP))
                        LoginNetworks.Remove(this.IP);
                }
            }

        }
        public bool Paused = false, CanUseInternet = true, Paid = false, Connected = false;//, Change = false;
        public uint LimitedTime, UsedTime, RemainingTime;
        public DateTime StartTime, StopedTime;
        public TimeSpan LastCheckConnected;
        public TimeStatus TimeStatu = TimeStatus.None;
        public long PausedTime = 0;
        public enum TimeStatus
        {
            Time,
            Login,
            None
        }


        public static void Loed()
        {
            NetworkInfo = new Dictionary<string, NetworkItems>();
            OfflineNetworkInfo = new Dictionary<string, NetworkItems>();
            ALLNetworks = new Dictionary<string, NetworkItems>();
            WOLClass.SetMyIP();
          //  List<string> notinsure = new List<string>();
            var gnc = WOLClass.getNetworkComputers();
            foreach (var pcinfo in gnc)
            {
                string[] pc = pcinfo.Split(',');
                /*
                if (pc[2].StartsWith("192.168"))
                {
                    notinsure.Add(pcinfo);
                    continue;
                }
                */
                NetworkItems NI = new NetworkItems();
                NI.HostName = NI.ShownName = pc[0];//2
                NI.IP = pc[0];
                NI.PCStatu = PCStatus.Online;
                NI.MacAddress = pc[1];
                //AllIP.Add(NI.IP);
                NetworkInfo.Add(NI.IP, NI);
                ALLNetworks.Add(NI.IP, NI);
            }
            /*
            foreach (var pcinfo in notinsure)
            {
                string[] pc = pcinfo.Split(',');
                if (AllIP.Contains(pc[2]))
                {
                    continue;
                }
                NetworkItems NI = new NetworkItems();
                NI.HostName = pc[2];
                NI.IP = pc[0];
                NI.PCStatu = PCStatus.Online;
                NI.MacAddress = pc[1];
                NetworkInfo.Add(NI.HostName, NI);
                ALLNetworks.Add(NI.HostName, NI);
            }
            */
        }

        public static void RefreshLoads()
        {
            foreach (var pcinfo in WOLClass.getNetworkComputers())
            {
                string[] pc = pcinfo.Split(',');
                if (NetworkInfo.ContainsKey(pc[0]) == false)//2
                {
                    if (OfflineNetworkInfo.ContainsKey(pc[0]) == false)//2
                    {
                        //New Online PC
                        /*
                        if (AllIP.Contains(pc[2]))
                        {
                            continue;
                        }
                        */
                        NetworkItems NI = new NetworkItems();
                        NI.HostName = NI.ShownName = pc[0];//2
                        NI.IP = pc[0];
                        NI.PCStatu = PCStatus.Online;
                        NI.MacAddress = pc[1];
                        NetworkInfo.Add(NI.IP, NI);
                        ALLNetworks.Add(NI.IP, NI);
                    }
                    else
                    {
                        //Offline but now Online
                        NetworkItems NI = OfflineNetworkInfo[pc[0]];// pc[2]];
                        NI.IP = pc[0];
                        NI.PCStatu = PCStatus.Online;
                        if (NI.MacAddress == "" || string.IsNullOrEmpty(NI.MacAddress))
                            NI.MacAddress = pc[1];
                        NetworkInfo.Add(NI.IP, NI);
                        OfflineNetworkInfo.Remove(NI.IP);
                    }
                }
                else
                {
                    if (OfflineNetworkInfo.ContainsKey(pc[0]))//2
                    {      //Offline but now Online
                        NetworkItems NI = OfflineNetworkInfo[pc[0]];//2
                        NI.IP = pc[0];
                        NI.PCStatu = PCStatus.Online;
                        if (NI.MacAddress == "" || string.IsNullOrEmpty(NI.MacAddress))
                            NI.MacAddress = pc[1];
                        OfflineNetworkInfo.Remove(NI.IP);
                    }
                    else
                    {
                        //Online but now Offline
                    }
                }
            }
        }

        public static void CreateVertualMachine(PCStatus State)
        {
            NetworkItems NI = new NetworkItems();
            NI.HostName = NI.ShownName = "ALEman" + new Random().Next(1, 14);
            while (NetworkInfo.ContainsKey(NI.IP) || OfflineNetworkInfo.ContainsKey(NI.IP))
            {
                NI.HostName = NI.ShownName = "ALEman" + new Random().Next(1, 14);
            }

            NI.IP = "192.168.1." + new Random().Next(2, 254);
            NI.PCStatu = State;
            NI.MacAddress = "255-255-255-" + new Random().Next(200, 255);
            if (State == PCStatus.Online)
                NetworkInfo.Add(NI.IP, NI);
            else
                OfflineNetworkInfo.Add(NI.IP, NI);
            ALLNetworks.Add(NI.IP, NI);
        }
    }

    public class PaymentInfo
    {
        public DateTime StartTime;
        public uint UsedTime;
        public double Price;
        public string Showname;
        public string Hostname;
        public TimeStatus TimeStatu;
        public enum TimeStatus
        {
            Time,
            Login,
        }
    }
    /*
    public class NetworkBrowser
    {
        #region Dll Imports
        [DllImport("Netapi32", CharSet = CharSet.Auto, SetLastError = true), SuppressUnmanagedCodeSecurityAttribute]
        public static extern int NetServerEnum(
            string ServerNane, // must be null
            int dwLevel,
            ref IntPtr pBuf,
            int dwPrefMaxLen,
            out int dwEntriesRead,
            out int dwTotalEntries,
            int dwServerType,
            string domain, // null for login domain
            out int dwResumeHandle
            );
        [DllImport("Netapi32", SetLastError = true), SuppressUnmanagedCodeSecurityAttribute]
        public static extern int NetApiBufferFree(IntPtr pBuf);
        [StructLayout(LayoutKind.Sequential)]
        public struct _SERVER_INFO_100
        {
            internal int sv100_platform_id;
            [MarshalAs(UnmanagedType.LPWStr)]
            internal string sv100_name;
        }
        #endregion

        #region get Network Computers host name
        public ArrayList getNetworkComputers()
        {
            ArrayList networkComputers = new ArrayList();
            const int MAX_PREFERRED_LENGTH = -1;
            int SV_TYPE_WORKSTATION = 1;
            int SV_TYPE_SERVER = 2;
            IntPtr buffer = IntPtr.Zero;
            IntPtr tmpBuffer = IntPtr.Zero;
            int entriesRead = 0;
            int totalEntries = 0;
            int resHandle = 0;
            int sizeofINFO = Marshal.SizeOf(typeof(_SERVER_INFO_100));


            try
            {
                int ret = NetServerEnum(null, 100, ref buffer, MAX_PREFERRED_LENGTH,
                    out entriesRead,
                    out totalEntries, SV_TYPE_WORKSTATION | SV_TYPE_SERVER, null, out 
					resHandle);
                if (ret == 0)
                {
                    for (int i = 0; i < totalEntries; i++)
                    {
                        tmpBuffer = new IntPtr((long)buffer + (i * sizeofINFO));
                        _SERVER_INFO_100 svrInfo = (_SERVER_INFO_100)
                            Marshal.PtrToStructure(tmpBuffer, typeof(_SERVER_INFO_100));

                        networkComputers.Add(svrInfo.sv100_name);//sv100_name
                    }
                }
            }
            catch
            {
            }
            finally
            {
                //The NetApiBufferFree function frees 
                //the memory that the NetApiBufferAllocate function allocates
                NetApiBufferFree(buffer);
            }
            //return entries found
            return networkComputers;

        }
        #endregion

        #region getMacIP
        public static string getMacByIp(string ip)
        {
            var macIpPairs = GetAllMacAddressesAndIppairs();
            int index = macIpPairs.FindIndex(x => x.IpAddress == ip);
            if (index >= 0)
            {
                return macIpPairs[index].MacAddress.ToUpper();
            }
            else
            {
                return null;
            }
        }
        static List<MacIpPair> GetAllMacAddressesAndIppairs()
        {
            List<MacIpPair> mip = new List<MacIpPair>();
            System.Diagnostics.Process pProcess = new System.Diagnostics.Process();
            pProcess.StartInfo.FileName = "arp";
            pProcess.StartInfo.Arguments = "-a ";
            pProcess.StartInfo.UseShellExecute = false;
            pProcess.StartInfo.RedirectStandardOutput = true;
            pProcess.StartInfo.CreateNoWindow = true;
            pProcess.Start();
            string cmdOutput = pProcess.StandardOutput.ReadToEnd();
            string pattern = @"(?<ip>([0-9]{1,3}\.?){4})\s*(?<mac>([a-f0-9]{2}-?){6})";

            foreach (Match m in Regex.Matches(cmdOutput, pattern, RegexOptions.IgnoreCase))
            {
                mip.Add(new MacIpPair()
                {
                    MacAddress = m.Groups["mac"].Value,
                    IpAddress = m.Groups["ip"].Value
                });
            }
            return mip;
        }
        struct MacIpPair
        {
            public string MacAddress;
            public string IpAddress;
        }
        #endregion

        #region Get IP from hostname
        public static string DoGetHostAddresses(string hostname)
        {
            try
            {
                IPAddress[] ips = Dns.GetHostAddresses(hostname);

                return ips[0].ToString();
            }
            catch { return null; }
        }

        public static void SetMyIP()
        {
            IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                   // MessageBox.Show(ip.ToString());
                    NetworkItems.MyIp = ip.ToString(); return;
                }
            }
        }
        #endregion
    }
    */

    public class WOLClass : UdpClient
    {
        public WOLClass()
            : base()
        { }
        //this is needed to send broadcast packet
        public void SetClientToBrodcastMode()
        {
            if (this.Active)
                this.Client.SetSocketOption(SocketOptionLevel.Socket,
                                          SocketOptionName.Broadcast, 0);
        }

        public static void SetMyIP()
        {
            IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    // MessageBox.Show(ip.ToString());
                    NetworkItems.MyIp = ip.ToString(); return;
                }
            }
        }

        public bool WakeFunction(string MAC_ADDRESS)
        {
            try
            {
                MAC_ADDRESS = MAC_ADDRESS.Replace("-", "");
                WOLClass client = new WOLClass();
                client.Connect(new
                   IPAddress(0xffffffff),  //255.255.255.255  i.e broadcast
                   0x2fff); // port=12287 let's use this one 
                client.SetClientToBrodcastMode();
                //set sending bites
                long counter = 0;
                //buffer to be send
                byte[] bytes = new byte[1024];   // more than enough :-)
                //first 6 bytes should be 0xFF
                for (int y = 0; y < 6; y++)
                    bytes[counter++] = 0xFF;
                //now repeate MAC 16 times
                for (int y = 0; y < 16; y++)
                {
                    int i = 0;
                    for (int z = 0; z < 6; z++)
                    {
                        bytes[counter++] =
                            byte.Parse(MAC_ADDRESS.Substring(i, 2),
                            NumberStyles.HexNumber);
                        i += 2;
                    }
                }

                //now send wake up packet
                int reterned_value = client.Send(bytes, 1024);
                if (reterned_value <= 0)
                    return false;
                else
                    return true;
            }
            catch { return false; }
        }

        public static void WakeUp(string macAddress, string ipAddress, string subnetMask)
        {
            //     UdpClient client = new UdpClient(); moved
            Byte[] datagram = new byte[102];

            for (int i = 0; i <= 5; i++)
            {
                datagram[i] = 0xff;
            }

            string[] macDigits = null;
            if (macAddress.Contains("-"))
            {
                macDigits = macAddress.Split('-');
            }
            else
            {
                macDigits = macAddress.Split(':');
            }

            if (macDigits.Length != 6)
            {
                throw new ArgumentException("Incorrect MAC address supplied!");
            }

            int start = 6;
            for (int i = 0; i < 16; i++)
            {
                for (int x = 0; x < 6; x++)
                {
                    datagram[start + i * 6 + x] = (byte)Convert.ToInt32(macDigits[x], 16);
                }
            }

            IPAddress address = IPAddress.Parse(ipAddress);
            IPAddress mask = IPAddress.Parse(subnetMask);
            IPAddress broadcastAddress = IPAddressExtensions.GetBroadcastAddress(address, mask);//GetBroadcastAddress
            UdpClient client = new UdpClient();// moved here
            client.Send(datagram, datagram.Length, broadcastAddress.ToString(), 3);
            client.Close();
        }

        public static List<string> getNetworkComputers()
        {
        //    Stopwatch watch = Stopwatch.StartNew();
            List<string> _ret = new List<string>();

            Process netUtility = new Process();
            netUtility.StartInfo.FileName = "arp.exe";
            netUtility.StartInfo.CreateNoWindow = true;
            netUtility.StartInfo.Arguments = "-a";
            netUtility.StartInfo.RedirectStandardOutput = true;
            netUtility.StartInfo.UseShellExecute = false;
            netUtility.StartInfo.RedirectStandardError = true;
            netUtility.Start();

            StreamReader streamReader = new StreamReader(netUtility.StandardOutput.BaseStream, netUtility.StandardOutput.CurrentEncoding);
            string line = "";
            while ((line = streamReader.ReadLine()) != null)
            {
               if (line.StartsWith("  "))// if (line.Contains("dynamic")/*StartsWith("  ")*/ && line.Contains("192.168"))
                {
                    var Itms = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                    if (Itms.Length >= 3 && Itms[0].StartsWith("192.168") && Itms[0] != "192.168.1.1" && Itms[0] != "192.168.1.255")
                    {
                        /*
                        string hn = "";
                        try
                        {
                            hn = Dns.GetHostEntry(Itms[0]).HostName.ToUpper();
                          //  MessageBox.Show(hn);
                        }
                        catch { continue; }
                        */
                        _ret.Add(Itms[0] + "," + Itms[1]);// + ","+ hn);
                    }
                }
            }
            streamReader.Close();
           // watch.Stop();
         //   MessageBox.Show("Finished in " + watch.Elapsed.ToString());
            return _ret;
        }

        #region get Mac by IP
        /*
        public static string getMacByIp(string ip)
        {
            var macIpPairs = GetAllMacAddressesAndIppairs();
            int index = macIpPairs.FindIndex(x => x.IpAddress == ip);
            if (index >= 0)
            {
                return macIpPairs[index].MacAddress.ToUpper();
            }
            else
            {
                return null;
            }
        }
        public static List<MacIpPair> GetAllMacAddressesAndIppairs()
        {
            List<MacIpPair> mip = new List<MacIpPair>();
            System.Diagnostics.Process pProcess = new System.Diagnostics.Process();
            pProcess.StartInfo.FileName = "arp";
            pProcess.StartInfo.Arguments = "-a ";
            pProcess.StartInfo.UseShellExecute = false;
            pProcess.StartInfo.RedirectStandardOutput = true;
            pProcess.StartInfo.CreateNoWindow = true;
            pProcess.Start();
            string cmdOutput = pProcess.StandardOutput.ReadToEnd();
            string pattern = @"(?<ip>([0-9]{1,3}\.?){4})\s*(?<mac>([a-f0-9]{2}-?){6})";

            foreach (Match m in Regex.Matches(cmdOutput, pattern, RegexOptions.IgnoreCase))
            {
                mip.Add(new MacIpPair()
                {
                    MacAddress = m.Groups["mac"].Value,
                    IpAddress = m.Groups["ip"].Value,
                    HostName = Dns.GetHostEntry(m.Groups["ip"].Value).HostName
                });
            }
            return mip;
        }
        public struct MacIpPair
        {
            public string MacAddress;
            public string IpAddress;
            public string HostName;
        }
        */
        #endregion
    }

    public static class IPAddressExtensions
    {
        public static IPAddress GetBroadcastAddress(IPAddress address, IPAddress subnetMask)
        {
            byte[] ipAdressBytes = address.GetAddressBytes();
            byte[] subnetMaskBytes = subnetMask.GetAddressBytes();

            if (ipAdressBytes.Length != subnetMaskBytes.Length)
                throw new ArgumentException("Lengths of IP address and subnet mask do not match.");

            byte[] broadcastAddress = new byte[ipAdressBytes.Length];
            for (int i = 0; i < broadcastAddress.Length; i++)
            {
                broadcastAddress[i] = (byte)(ipAdressBytes[i] | (subnetMaskBytes[i] ^ 255));
            }
            return new IPAddress(broadcastAddress);
        }
    }
}

