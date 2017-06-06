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
using ALEmanCafe_Server.Other;
using System.Threading;
using ALEmanCafe_Server.Properties;
using ALEmanCafe_Server.Cafe;

namespace ALEmanCafe_Server
{
    public partial class ALEmanCafeServer : Form
    {
        public bool IsRestart = false, privateDrag, inRecieveInfos = false;
        private ALEmanCafe LoginForm;
        public System.Timers.Timer RefreshTimer;
        public System.Timers.Timer ReciverTimer;
        public System.Timers.Timer CheckConnectTimer;
        public bool LoadALEmanCafeServer(ALEmanCafe LogForm)
        {
            InitializeComponent();
            this.LoginForm = LogForm;
            this.FormClosing += new FormClosingEventHandler(ALEmanCafeServer_FormClosing);
            ImageList imageList1 = new ImageList();
            imageList1.Images.Add("system_login", Properties.Resources.system_login);
            this.MyTabControl.ImageList = imageList1;
            this.MyTabControl.TabPages[0].ImageKey = "system_login";
            MonitorView.MouseClick += new MouseEventHandler(MonitorView_MouseClick);
            MonitorView.MouseDoubleClick += new MouseEventHandler(MonitorView_MouseClick);
            MonitorView.KeyDown += new KeyEventHandler(MonitorView_KeyPress);
            MonitorView.ItemSelectionChanged += new ListViewItemSelectionChangedEventHandler(MonitorView_ItemSelectionChanged);
            toolStripComboBox1.SelectedItem = "8";
            toolStripComboBox1.SelectedIndexChanged += new EventHandler(toolStripComboBox1_TextChanged);
            dateTimePicker1.Value = dateTimePicker2.Value = DateTime.Now;
            dateTimePicker1.ValueChanged += new EventHandler(dateTimePicker1_ValueChanged);
            dateTimePicker2.ValueChanged += new EventHandler(dateTimePicker1_ValueChanged);
            this.MonitorView.ItemDrag += new ItemDragEventHandler(MonitorView_ItemDrag);
            this.MonitorView.DragEnter += new DragEventHandler(MonitorView_DragEnter);
            this.MonitorView.DragOver += new DragEventHandler(MonitorView_DragOver);

            #region Timers
            RefreshTimer = new System.Timers.Timer(50);
            RefreshTimer.AutoReset = true;
            RefreshTimer.Elapsed += new System.Timers.ElapsedEventHandler(Timers);
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new MethodInvoker(
                    () => RefreshTimer.Start()));
            }
            else
            {
                RefreshTimer.Start();
            }

            ReciverTimer = new System.Timers.Timer(50);
            ReciverTimer.AutoReset = true;
            ReciverTimer.Elapsed += new System.Timers.ElapsedEventHandler(RecieveInfos);
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new MethodInvoker(
                    () => ReciverTimer.Start()));
            }
            else
            {
                ReciverTimer.Start();
            }
            #endregion
            #region Images

            if (this.MonitorView.InvokeRequired)
            {
                this.MonitorView.BeginInvoke(new MethodInvoker(
                    () =>
                    {
                        this.MonitorView.LargeImageList = new ImageList();
                        this.MonitorView.LargeImageList.ImageSize = new Size(100, 100);
                        this.MonitorView.SmallImageList = new ImageList();
                        this.MonitorView.SmallImageList.ImageSize = new Size(31, 40);
                        this.MonitorView.LargeImageList.Images.Add("LoginMonitor", Properties.Resources.LoginMonitor);
                        this.MonitorView.LargeImageList.Images.Add("OfflineDisplay", Properties.Resources.OfflineDisplay);
                        this.MonitorView.LargeImageList.Images.Add("OnlineDisplay", Properties.Resources.OnlineDisplay);
                        this.MonitorView.LargeImageList.Images.Add("TimeMonitor", Properties.Resources.TimeMonitor);
                        this.MonitorView.LargeImageList.Images.Add("PauseDisplay", Properties.Resources.PauseDisplay);
                        //**************
                        this.MonitorView.SmallImageList.Images.Add("LoginMonitor", Properties.Resources.LoginMonitor);
                        this.MonitorView.SmallImageList.Images.Add("OfflineDisplay", Properties.Resources.OfflineDisplay);
                        this.MonitorView.SmallImageList.Images.Add("OnlineDisplay", Properties.Resources.OnlineDisplay);
                        this.MonitorView.SmallImageList.Images.Add("TimeMonitor", Properties.Resources.TimeMonitor);
                        this.MonitorView.SmallImageList.Images.Add("PauseDisplay", Properties.Resources.PauseDisplay);
                    }
                        ));
            }
            else
            {
                this.MonitorView.LargeImageList = new ImageList();
                this.MonitorView.LargeImageList.ImageSize = new Size(100, 100);
                this.MonitorView.SmallImageList = new ImageList();
                this.MonitorView.SmallImageList.ImageSize = new Size(31, 40);
                this.MonitorView.LargeImageList.Images.Add("LoginMonitor", Properties.Resources.LoginMonitor);
                this.MonitorView.LargeImageList.Images.Add("OfflineDisplay", Properties.Resources.OfflineDisplay);
                this.MonitorView.LargeImageList.Images.Add("OnlineDisplay", Properties.Resources.OnlineDisplay);
                this.MonitorView.LargeImageList.Images.Add("TimeMonitor", Properties.Resources.TimeMonitor);
                this.MonitorView.LargeImageList.Images.Add("PauseDisplay", Properties.Resources.PauseDisplay);
                //**************
                this.MonitorView.SmallImageList.Images.Add("LoginMonitor", Properties.Resources.LoginMonitor);
                this.MonitorView.SmallImageList.Images.Add("OfflineDisplay", Properties.Resources.OfflineDisplay);
                this.MonitorView.SmallImageList.Images.Add("OnlineDisplay", Properties.Resources.OnlineDisplay);
                this.MonitorView.SmallImageList.Images.Add("TimeMonitor", Properties.Resources.TimeMonitor);
                this.MonitorView.SmallImageList.Images.Add("PauseDisplay", Properties.Resources.PauseDisplay);
            }
            #endregion
            LoadProducts();
            return true;
        }

        public void LoginNow(bool hideshow)
        {
            if (hideshow)
            this.LoginForm.Visible = false;

            #region Timers
            CheckConnectTimer = new System.Timers.Timer(1000);
            CheckConnectTimer.AutoReset = true;
            CheckConnectTimer.Elapsed += new System.Timers.ElapsedEventHandler(CheckConnectInfo);
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new MethodInvoker(
                    () => CheckConnectTimer.Start()));
            }
            else
            {
                CheckConnectTimer.Start();
            }
            #endregion

            if (hideshow)
            this.ShowDialog();
        }

        #region Drag&Drop
        private void MonitorView_ItemDrag(object sender, ItemDragEventArgs e)
        {
            privateDrag = true;
            DoDragDrop(e.Item, DragDropEffects.Move);
            privateDrag = false;
        }
        private void MonitorView_DragEnter(object sender, DragEventArgs e)
        {
            if (privateDrag) e.Effect = e.AllowedEffect;
        }
        private void MonitorView_DragOver(object sender, DragEventArgs e)
        {
            var pos = MonitorView.PointToClient(new Point(e.X, e.Y));
            var hit = MonitorView.HitTest(pos);
            if (hit.Item != null)
            {
                var dragItem = (ListViewItem)e.Data.GetData(typeof(ListViewItem));
                if (hit.Item.Text == dragItem.Text)
                {
                    e.Effect = DragDropEffects.None;
                    return;
                }
                NetworkItems From = NetworkItems.NetworkInfo[dragItem.Text];
                NetworkItems To = NetworkItems.NetworkInfo[hit.Item.Text];
                if (From == null || To == null)
                {
                    e.Effect = DragDropEffects.None;
                    return;
                }
                else if (From.PCStatu != NetworkItems.PCStatus.Online || From.TimeStatu == NetworkItems.TimeStatus.None || From.TimeStatu == NetworkItems.TimeStatus.Time && From.RemainingTime <= 0 || From.TimeStatu == NetworkItems.TimeStatus.Login && From.UsedTime <= 0)
                {
                    e.Effect = DragDropEffects.None;
                    return;
                }
                else if (To.PCStatu != NetworkItems.PCStatus.Online || To.Login || To.Connected == false)
                {
                    e.Effect = DragDropEffects.None;
                    return;
                }
                e.Effect = DragDropEffects.Move;

                ChangePC(From, To);
            }
            else
                e.Effect = DragDropEffects.None;
        }
        #endregion

        public void ChangePC(NetworkItems From, NetworkItems To)
        {
            if (From.PCStatu != NetworkItems.PCStatus.Online || From.Connected == false)
            {
                To.CanUseInternet = From.CanUseInternet;
                To.LimitedTime = From.LimitedTime;
                To.Paid = From.Paid;
                To.TimeStatu = From.TimeStatu;
                To.RemainingTime = From.RemainingTime;
                To.StartTime = From.StartTime;
                To.PausedTime = From.PausedTime;
                To.StopedTime = From.StopedTime;
                To.UsedTime = From.UsedTime;
                To.Login = true;
                JustRunMessage(null, To, true);

                From.LimitedTime = 0;
                From.Paid = false;
                From.TimeStatu = NetworkItems.TimeStatus.None;
                From.RemainingTime = 0;
                From.StartTime = new DateTime();
                From.PausedTime = 0;
                From.StopedTime = new DateTime();
                From.UsedTime = 0;
                RefreshPCStatusNow(From);
              //  SendPCStatus(From, MessageStatus.logout);
            }
            else
            {
                To.CanUseInternet = From.CanUseInternet;
                To.LimitedTime = From.LimitedTime;
                To.Paid = From.Paid;
                To.TimeStatu = From.TimeStatu;
                To.RemainingTime = From.RemainingTime;
                To.StartTime = From.StartTime;
                To.PausedTime = From.PausedTime;
                To.StopedTime = From.StopedTime;
                To.UsedTime = From.UsedTime;
                To.Login = true;
                JustRunMessage(null, To, true);

                From.LimitedTime = 0;
                From.Paid = false;
                From.TimeStatu = NetworkItems.TimeStatus.None;
                From.RemainingTime = 0;
                From.StartTime = new DateTime();
                From.PausedTime = 0;
                From.StopedTime = new DateTime();
                From.UsedTime = 0;
                SendPCStatus(From, MessageStatus.logout);
                //this.SendPCStatus(From, MessageStatus.changepc, false, 0, false, To);
            }
        }

        public void CheckConnectInfo(object sender, EventArgs eeee)
        {/*
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new MethodInvoker(
                    () => writeamess("inCheckConnectInfo : " +inCheckConnectInfo+ Environment.NewLine)));
            }
            else
            {
                writeamess("inCheckConnectInfo : " +inCheckConnectInfo+ Environment.NewLine);
            }
            */

            //   if (inCheckConnectInfo) return; inCheckConnectInfo = true;
            /*
            if (inRecieveInfos && DateTime.Now > new DateTime(LRT).AddMilliseconds(400))
            {
                inRecieveInfos = false;
              //  writeamess("inRecieveInfos");
            }
            */
            TimeSpan TS = new TimeSpan(DateTime.Now.Ticks);
            foreach (NetworkItems NI in NetworkItems.NetworkInfo.Values)
            {
                TimeSpan LCC = new TimeSpan(NI.LastCheckConnected.Ticks);
                TimeSpan Rem = TS - LCC;
                if (Rem.TotalSeconds >= 5)
                {
                    if (NI.Connected && NI.Login)
                    {
                        NI.Connected = false;
                        NI.Login = false;
                        NI.StopedTime = DateTime.Now;
                        RefreshPCStatus(NI);

                        if (NI.RM != null)
                        {
                            //    writeamess("DONE");
                            try
                            {
                                NI.RM.RemoteManagement_FormClosing(null, null);
                            }
                            catch { }
                        }
                    }
                    else
                        NI.Connected = false;
                }
                else
                {
                    bool connow = false;
                    if (!NI.Connected)
                        connow = true;

                    NI.Connected = true;
                    if (connow)
                    {
                        this.SendPCStatus(NI, MessageStatus.tellme);
                    }
                }
                this.SendPCStatus(NI, MessageStatus.checkconnected);
            }
            MyDatabase.SaveSystemlogTimers();
            //   inCheckConnectInfo = false;
        }


        public void RecieveInfos(object sender, EventArgs eeee)
        {
            if (inRecieveInfos) return;// LRT = DateTime.Now.Ticks;
            inRecieveInfos = true;
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new MethodInvoker(
                    () => ReceiveSock())).AsyncWaitHandle.WaitOne();
            }
            else
            {
                ReceiveSock();
            }
            /*
            if (NwStream!= null)
            {
                NwStream.Close();
                NwStream.Dispose();
                NwStream = null;
            }
            if (TcPclient != null)
            {
                TcPclient.Client.Close();
                TcPclient.Close();
                TcPclient = null;
            }
            if (listenerrrrrrr != null)
            {
                listenerrrrrrr.Server.Close();
                listenerrrrrrr.Stop();
                listenerrrrrrr = null;
            }
            */
            if (udpClient != null)
            {
                udpClient.Client.Close();
                udpClient.Close();
                udpClient = null;
            }
            inRecieveInfos = false;
        }
        /*
        public TcpListener listenerrrrrrr;
        public TcpClient TcPclient;
        public NetworkStream NwStream;
        */
        public UdpClient udpClient;
        public void ReceiveSock()
        {
            try
            {
                udpClient = new UdpClient(717);
                IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 717);
                while (true)
                {
                    try
                    {
                        Byte[] receiveBytes = udpClient.Receive(ref RemoteIpEndPoint);
                        string allrec = Encoding.ASCII.GetString(receiveBytes);
                        string[] Infos = allrec.Split(';');
                    //    udpClient.Close();

                        NetworkItems ThisPC = NetworkItems.ALLNetworks[Infos[1]];
                        RecieveTranslator(Infos, ThisPC);

                        System.Threading.Thread.Sleep(20);
                    }
                    catch (Exception ee) { writeamess(ee.ToString() + Environment.NewLine); udpClient.Close(); inRecieveInfos = false; }
                }
            }
            catch (Exception ee) { writeamess(ee.ToString() + Environment.NewLine); }
            /*
            try
            {
                //MessageBox.Show("OK: " + inRecieveInfos.ToString());
                listenerrrrrrr = new TcpListener(IPAddress.Any, 717);//Any
                                                                                 //writeamess("OK" + Environment.NewLine);

                listenerrrrrrr.Start();
                while (true)
                {
                    if (!listenerrrrrrr.Pending())
                    {
                        Thread.Sleep(50); //500
                        continue;
                    }
                    TcPclient = listenerrrrrrr.AcceptTcpClient();

                    NwStream = TcPclient.GetStream();
                    byte[] buffer = new byte[TcPclient.ReceiveBufferSize];

                    int bytesRead = NwStream.Read(buffer, 0, TcPclient.ReceiveBufferSize);
                    string dataReceived = Encoding.ASCII.GetString(buffer, 0, bytesRead);

                    if (TcPclient != null)
                    {
                        TcPclient.Client.Close();
                        TcPclient.Close();
                    }
                    if (NwStream != null)
                    {
                        NwStream.Close();
                        NwStream.Dispose();
                    }
                    string[] Infos = dataReceived.Split(';');
                    NetworkItems ThisPC = NetworkItems.ALLNetworks[Infos[1]];
                    RecieveTranslator(Infos, ThisPC);

                    System.Threading.Thread.Sleep(20);
                }
            }catch(Exception ee) { writeamess(ee.ToString()); }
            */
        }
        public void RecieveTranslator(string[] Infos, NetworkItems ThisPC)
        {
            if (ThisPC != null)
            {
                // if (Infos[0] != "checkconnectedok")
          //      writeamess("recieved from HostName : " + ThisPC.HostName + ", Infos : " + Infos[0] + ", Length : " + Infos.Length + Environment.NewLine);
                switch (Infos[0])
                {
                    case "USBPlugin":
                        {
                            MessageBox.Show(this, "USB Plugin at " + ThisPC.ShownName, "USB Plugin Warning", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            break;
                        }
                    case "USBPlugout":
                        {
                            MessageBox.Show(this, "USB Plugout at " + ThisPC.ShownName, "USB Plugout Warning", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            break;
                        }
                    case "sendfileok":
                        {
                            if (Infos.Length > 2)
                                MessageBox.Show(this, "You file has been sent to " + ThisPC.ShownName + " but found error to run the file, details : " + Environment.NewLine + Infos[2], "File not sent!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            else
                                MessageBox.Show(this, "You file has been sent to " + ThisPC.ShownName, "File sent", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            break;
                        }
                    case "checkconnectedok":
                        {
                            ThisPC.Connected = true;
                            ThisPC.LastCheckConnected = new TimeSpan(DateTime.Now.Ticks);
                            ThisPC.RunningApplication = Infos[2];
                            ThisPC.Title = Infos[3];
                            RefreshPCStatus(ThisPC);
                            break;
                        }
                    case "applistok":
                        {
                            List<RemoteApplications.AppInfo> AIL = new List<RemoteApplications.AppInfo>();
                            for (int C = 2; C < Infos.Length; C++)
                            {
                                //   writeamess("C : " + C);
                                string[] Infos2 = Infos[C].Split('|');
                                RemoteApplications.AppInfo AI = new RemoteApplications.AppInfo();
                                AI.AppName = Infos2[0];
                                AI.AppPath = Infos2[1];
                                AI.AppTitle = Infos2[2];
                                AI.APPID = uint.Parse(Infos2[3]);
                                AIL.Add(AI);
                            }
                            //  writeamess("done count : " + AIL.Count);
                            RemoteApplications RA = new RemoteApplications(this, AIL, ThisPC);
                            if (this.InvokeRequired)
                            {
                                this.BeginInvoke(new MethodInvoker(
                                    () => RA.Show())).AsyncWaitHandle.WaitOne();
                            }
                            else
                            {
                                RA.Show();
                            }
                            RefreshPCStatus(ThisPC);
                            break;
                        }
                    case "pauseok":
                        {
                            ThisPC.Paused = true;
                            ThisPC.StopedTime = DateTime.Now;
                            RefreshPCStatus(ThisPC);
                            break;
                        }
                    case "resumeok":
                        {
                            // TimeSpan ca = new TimeSpan(DateTime.Now.Ticks - ThisPC.StopedTime.Ticks);
                            //   ThisPC.StartTime = ThisPC.StartTime.AddMinutes(ca.TotalMinutes);

                            ThisPC.PausedTime += (DateTime.Now.Ticks - ThisPC.StopedTime.Ticks);
                            ThisPC.StopedTime = new DateTime();
                            ThisPC.Paused = false;
                            RefreshPCStatus(ThisPC);
                            break;
                        }
                    case "justrun":
                        {
                            JustRunMessage(Infos, ThisPC);
                            break;
                        }
                    case "unlimittimeok":
                        {
                            ThisPC.Paid = false;
                            ThisPC.LimitedTime = 0;
                            ThisPC.TimeStatu = NetworkItems.TimeStatus.Login;
                            RefreshPCStatus(ThisPC);
                            break;
                        }
                    case "limittimeok":
                        {
                            ThisPC.Paid = true;
                            uint RT = uint.Parse(Infos[2]);
                            ThisPC.LimitedTime = RT;
                            ThisPC.TimeStatu = NetworkItems.TimeStatus.Time;
                            if (Infos[3].ToLower() == "true")
                            {
                                ThisPC.Paid = true;
                                PaymentNow(ThisPC);
                            }
                            else
                                ThisPC.Paid = false;
                            RefreshPCStatus(ThisPC);
                            break;
                        }
                    case "addtimeok":
                        {
                            uint RT = uint.Parse(Infos[2]);
                            if (Infos[3].ToLower() == "true")
                                ThisPC.LimitedTime -= RT;
                            else
                                ThisPC.LimitedTime += RT;
                            if (Infos[4].ToLower() == "true")
                            {
                                ThisPC.Paid = true;
                                PaymentNow(ThisPC);
                            }
                            else
                                ThisPC.Paid = false;
                            RefreshPCStatus(ThisPC);
                            break;
                        }
                    case "continueloginok":
                        {
                            //  TimeSpan ca = new TimeSpan(DateTime.Now.Ticks - ThisPC.StopedTime.Ticks);
                            //ThisPC.StartTime = ThisPC.StartTime.AddMinutes(ca.TotalMinutes);
                            ThisPC.PausedTime += (DateTime.Now.Ticks - ThisPC.StopedTime.Ticks);
                            ThisPC.StopedTime = new DateTime();
                            ThisPC.Login = true;
                            ThisPC.Paused = false;
                            RefreshPCStatus(ThisPC);
                            break;
                        }
                    case "retell":
                        {
                            //  writeamess("LOG LEN : " + Infos.Length);
                            if (Infos.Length == 16)//Time
                            {
                                try
                                {
                                    ThisPC.StartTime = new DateTime((DateTime.Now.Ticks - long.Parse(Infos[2])));// ThisPC.StartTime = DateTime.Now.AddMinutes(-uint.Parse(Infos[2])); // ThisPC.StartTime = new DateTime(long.Parse(Infos[2]));
                                }
                                catch { ThisPC.StartTime = new DateTime((long.Parse(Infos[2]) - DateTime.Now.Ticks)); }

                                if (Infos[14] != "0")
                                    ThisPC.StopedTime = new DateTime(DateTime.Now.Ticks - long.Parse(Infos[14]));
                                else
                                    ThisPC.StopedTime = new DateTime();

                                ThisPC.LimitedTime = ThisPC.RemainingTime = uint.Parse(Infos[3]);
                                ThisPC.PausedTime = long.Parse(Infos[13]);
                                ThisPC.TimeStatu = NetworkItems.TimeStatus.Time;
                                if (Infos[4].ToLower() == "false")
                                    ThisPC.CanUseInternet = false;
                                else
                                    ThisPC.CanUseInternet = true;
                                ThisPC.UserName = Infos[5];
                                ThisPC.MyVersion = Infos[6];
                                ThisPC.MyOperatingSystem = Infos[7];
                                ThisPC.TotalMemory = (string)(Convert.ToUInt64(Infos[8]) / 1024 / 1024).ToString();
                                if (Infos[9].ToLower() == "true")
                                {
                                    ThisPC.Paid = true;
                                    PaymentNow(ThisPC);
                                }
                                else
                                    ThisPC.Paid = false;
                                ThisPC.FreeMemory = (string)(Convert.ToUInt64(Infos[10]) / 1024 / 1024).ToString();

                                if (Infos[11].ToLower() == "false")
                                    ThisPC.Paused = false;
                                else
                                    ThisPC.Paused = true;

                                ThisPC.HostName = Infos[12];

                                ThisPC.Login = true;
                            }
                            else if (Infos.Length == 14)//Login
                            {
                                ThisPC.StartTime = new DateTime((DateTime.Now.Ticks - long.Parse(Infos[2])));// ThisPC.StartTime = DateTime.Now.AddMinutes(-uint.Parse(Infos[2]));// ThisPC.StartTime = new DateTime(long.Parse(Infos[2]));

                                if (Infos[13] != "0")
                                    ThisPC.StopedTime = new DateTime((DateTime.Now.Ticks - long.Parse(Infos[13])));
                                else
                                    ThisPC.StopedTime = new DateTime();                                                            //  MessageBox.Show(uint.Parse(Infos[2]).ToString());
                                ThisPC.LimitedTime = 0;
                                ThisPC.PausedTime = long.Parse(Infos[12]);
                                ThisPC.TimeStatu = NetworkItems.TimeStatus.Login;
                                if (Infos[3].ToLower() == "false")
                                    ThisPC.CanUseInternet = false;
                                else
                                    ThisPC.CanUseInternet = true;
                                ThisPC.UserName = Infos[4];
                                ThisPC.MyVersion = Infos[5];
                                ThisPC.MyOperatingSystem = Infos[6];
                                ThisPC.TotalMemory = (string)(Convert.ToUInt64(Infos[7]) / 1024 / 1024).ToString();
                                if (Infos[8].ToLower() == "true")
                                {
                                    ThisPC.Paid = true;
                                    PaymentNow(ThisPC);
                                }
                                else
                                    ThisPC.Paid = false;
                                ThisPC.FreeMemory = (string)(Convert.ToUInt64(Infos[9]) / 1024 / 1024).ToString();

                                if (Infos[10].ToLower() == "false")
                                    ThisPC.Paused = false;
                                else
                                    ThisPC.Paused = true;

                                ThisPC.HostName = Infos[11];//13

                                ThisPC.Login = true;
                            }
                            else if (Infos.Length == 8)//Nothing  'Removed'
                            {
                                ThisPC.UserName = Infos[2];
                                ThisPC.MyVersion = Infos[3];
                                ThisPC.MyOperatingSystem = Infos[4];
                                ThisPC.TotalMemory = (string)(Convert.ToUInt64(Infos[5]) / 1024 / 1024).ToString();
                                ThisPC.FreeMemory = (string)(Convert.ToUInt64(Infos[7]) / 1024 / 1024).ToString();
                                if (Infos[6].ToLower() == "true")
                                {
                                    ThisPC.Paid = true;
                                    PaymentNow(ThisPC);
                                }
                                else
                                    ThisPC.Paid = false;
                                ThisPC.Login = false;
                                ThisPC.Paused = false;
                                this.JustRunMessage(null, ThisPC, false, true);
                            }
                            else if (Infos.Length == 15)//Offline but have old time
                            {
                                ThisPC.StartTime = new DateTime((DateTime.Now.Ticks - long.Parse(Infos[2])));//  ThisPC.StartTime = new DateTime(long.Parse(Infos[2]));
                                ThisPC.StopedTime = new DateTime((DateTime.Now.Ticks - long.Parse(Infos[3])));//   ThisPC.StopedTime = new DateTime(long.Parse(Infos[3]));
                                ThisPC.LimitedTime = ThisPC.RemainingTime = uint.Parse(Infos[4]);
                                ThisPC.PausedTime = long.Parse(Infos[14]);

                                if (ThisPC.LimitedTime > 0)
                                    ThisPC.TimeStatu = NetworkItems.TimeStatus.Time;
                                else
                                    ThisPC.TimeStatu = NetworkItems.TimeStatus.Login;

                                if (Infos[5].ToLower() == "false")
                                    ThisPC.CanUseInternet = false;
                                else
                                    ThisPC.CanUseInternet = true;

                                ThisPC.UserName = Infos[6];
                                ThisPC.MyVersion = Infos[7];
                                ThisPC.MyOperatingSystem = Infos[8];
                                ThisPC.TotalMemory = (string)(Convert.ToUInt64(Infos[9]) / 1024 / 1024).ToString();
                                if (Infos[10].ToLower() == "true")
                                {
                                    ThisPC.Paid = true;
                                    PaymentNow(ThisPC);
                                }
                                else
                                    ThisPC.Paid = false;
                                ThisPC.FreeMemory = (string)(Convert.ToUInt64(Infos[11]) / 1024 / 1024).ToString();

                                if (Infos[12].ToLower() == "false")
                                    ThisPC.Paused = false;
                                else
                                    ThisPC.Paused = true;

                                ThisPC.HostName = Infos[13];

                                ThisPC.Login = false;
                            }
                            else
                                MessageBox.Show("Unknown retell Length : " + Infos.Length);
                            ThisPC.LastCheckConnected = new TimeSpan(DateTime.Now.Ticks);
                            ThisPC.Connected = true;
                            RefreshPCStatus(ThisPC);
                            RefreshTimeStatus(ThisPC);
                            RefreshItemSelection();
                            break;
                        }
                    case "loginok":
                        {
                            if (Infos.Length == 3)
                            {
                                ThisPC.StartTime = DateTime.Now;
                                ThisPC.StopedTime = new DateTime();
                                ThisPC.CanUseInternet = true;
                                ThisPC.PausedTime = 0;

                                ThisPC.TimeStatu = NetworkItems.TimeStatus.Login;
                                if (Infos[2].ToLower() == "true")
                                {
                                    ThisPC.Paid = true;
                                    PaymentNow(ThisPC);
                                }
                                else
                                    ThisPC.Paid = false;
                                ThisPC.Login = true;
                                ThisPC.Paused = false;
                            }
                            else if (Infos.Length == 4)
                            {
                                uint LT = uint.Parse(Infos[2]);
                                ThisPC.StartTime = DateTime.Now;
                                ThisPC.StopedTime = new DateTime();
                                ThisPC.LimitedTime = LT;
                                ThisPC.UsedTime = 0;
                                ThisPC.PausedTime = 0;
                                ThisPC.CanUseInternet = true;
                                ThisPC.RemainingTime = 0;
                                if (ThisPC.LimitedTime > 0)
                                    ThisPC.TimeStatu = NetworkItems.TimeStatus.Time;
                                else
                                    ThisPC.TimeStatu = NetworkItems.TimeStatus.Login;
                                if (Infos[3].ToLower() == "true")
                                {
                                    ThisPC.Paid = true;
                                    PaymentNow(ThisPC);
                                }
                                else
                                    ThisPC.Paid = false;
                                ThisPC.Login = true;
                                ThisPC.Paused = false;
                            }
                            else if (Infos.Length == 5)
                            {
                                uint LT = uint.Parse(Infos[2]);
                                ThisPC.StartTime = DateTime.Now;
                                ThisPC.PausedTime = 0;
                                ThisPC.StopedTime = new DateTime();
                                ThisPC.LimitedTime = LT;
                                ThisPC.UsedTime = 0;
                                ThisPC.CanUseInternet = false;
                                ThisPC.RemainingTime = 0;
                                if (ThisPC.LimitedTime > 0)
                                    ThisPC.TimeStatu = NetworkItems.TimeStatus.Time;
                                else
                                    ThisPC.TimeStatu = NetworkItems.TimeStatus.Login;
                                if (Infos[4].ToLower() == "true")
                                {
                                    ThisPC.Paid = true;
                                    PaymentNow(ThisPC);
                                }
                                else
                                    ThisPC.Paid = false;
                                ThisPC.Login = true;
                                ThisPC.Paused = false;
                            }
                            RefreshPCStatus(ThisPC);
                            RefreshTimeStatus(ThisPC);
                            RefreshItemSelection();
                            break;
                        }
                    case "logoutok":
                        {
                            ThisPC.Login = false;
                            ThisPC.StopedTime = DateTime.Now;
                            ThisPC.Paused = false;
                            //ThisPC.StartTime = new DateTime();
                            // ThisPC.TimeStatu = NetworkItems.TimeStatus.None;
                            PaymentNow(ThisPC);
                            RefreshPCStatus(ThisPC);
                            RefreshTimeStatus(ThisPC);
                            RefreshItemSelection();
                            break;
                        }
                    default:
                        {
                            MessageBox.Show("Unknown : " + Infos[0] + ", 2nd : " + Infos[1]);
                            break;
                        }
                }
            }
            else if (Infos[0] == "justrun")
                JustRunMessage(Infos, ThisPC);
            else
                MessageBox.Show("Unknown logoutok Host : " + Infos[1]);
        }
        public void SendSock(string MyMessage, NetworkItems ThisPC)
        {
            if (string.IsNullOrEmpty(MyMessage) == false)
            {
                //  MessageBox.Show("send");
                UdpClient udpServer;
                try
                {
                    try
                    {
                        udpServer = new UdpClient(716);
                    }catch(SocketException se) { if (se.SocketErrorCode != SocketError.AddressAlreadyInUse) writeamess(se.SocketErrorCode.ToString()+ Environment.NewLine); return; }
                    try
                    {
                        udpServer.Connect(IPAddress.Parse(ThisPC.IP), 716);
                 
                    }
                    catch (SocketException se)
                    {
                        if (se.SocketErrorCode != SocketError.AddressNotAvailable)
                            writeamess(se.SocketErrorCode.ToString() + Environment.NewLine);
                        else
                        {
                            writeamess("AddressNotAvailable" + Environment.NewLine);
                        }
                        udpServer.Client.Close();
                        udpServer.Close();
                        return;
                    }
            
                    Byte[] sendBytes = Encoding.ASCII.GetBytes(MyMessage);
                    try
                    {
                        udpServer.Send(sendBytes, sendBytes.Length);
                    }
                    catch (SocketException se)
                    {
                        if (se.SocketErrorCode != SocketError.MessageSize)
                            writeamess(se.SocketErrorCode.ToString() + Environment.NewLine);
                        else if (MyMessage.StartsWith(""))
                            MessageBox.Show("File is to long, Not sent...");
                    }
                    udpServer.Client.Close();
                    udpServer.Close();
                }
                catch (Exception ee) { writeamess(ee.ToString() + Environment.NewLine); }
                /*
                try
                {
                    TcpClient Tclient = null;
                    NetworkStream nwStream = null;
                    try
                    {
                        Thread.Sleep(1);
                        Tclient = new TcpClient(ThisPC.IP, 716);//Error when client closed
                    }
                    catch { if (Tclient != null) { Tclient.Client.Close(); Tclient.Close(); } return; }// writeamess("Tclient Closed" + Environment.NewLine); return; }
                    nwStream = Tclient.GetStream();
                    byte[] bytesToSend = ASCIIEncoding.ASCII.GetBytes(MyMessage);
                    try
                    {
                        //---send the text---
                        nwStream.Write(bytesToSend, 0, bytesToSend.Length);
                    }
                    catch { if (Tclient != null) { Tclient.Client.Close(); Tclient.Close(); } if (nwStream != null) { nwStream.Close(); nwStream.Dispose(); } return; }

                    nwStream.Close();
                    nwStream.Dispose();
                    Tclient.Client.Close();
                    Tclient.Close();
                }
                catch { writeamess("Error"); }//(Exception ee) { writeamess(ee.ToString()); }
                */
            }

        }



        public bool InRefreshTimer = false;
        public bool Loaded = false;
     //   public long LRT;
        public void Timers(object sender, EventArgs eeee)
        {
            if (Loaded)
            {
                if (this.statusStrip1.InvokeRequired)
                {
                    this.statusStrip1.BeginInvoke(new MethodInvoker(
                        () => RefreshStatusBar())).AsyncWaitHandle.WaitOne();
                }
                else
                {
                    RefreshStatusBar();
                }
                NetworkItems.RefreshLoads();
            }
            if (InRefreshTimer) return; InRefreshTimer = true;
            Dictionary<string, NetworkItems> AllComputers = NetworkItems.ALLNetworks;
    
            if (!Loaded)
            {
                NetworkItems.Loed();
                if (this.statusStrip1.InvokeRequired)
                {
                    this.statusStrip1.BeginInvoke(new MethodInvoker(
                        () => toolStripProgressBar1.Visible = false)).AsyncWaitHandle.WaitOne();
                }
                else
                {
                    toolStripProgressBar1.Visible = false;
                }
                AllComputers = NetworkItems.ALLNetworks;
                foreach (NetworkItems OT in NetworkItems.OldTimers.Values)
                {
                    if (NetworkItems.ALLNetworks.ContainsKey(OT.IP) == false)
                    {
                        OT.PCStatu = NetworkItems.PCStatus.Offline;
                        NetworkItems.OfflineNetworkInfo.Add(OT.IP, OT);
                        NetworkItems.ALLNetworks.Add(OT.IP, OT);
                        //  this.AddNewPC(OT, this.MonitorView);
                      
                    }
                    /* //Loaded in database and added to NetworkItems
                    else//
                    {
                        var pc = NetworkItems.ALLNetworks[OT.HostName];
                        if (!pc.Login && pc.StartTime.Year.ToString() == "1")
                        {
                            pc.CanUseInternet = OT.CanUseInternet;
                            pc.HostName = OT.HostName;
                            pc.Paid = OT.Paid;
                            pc.LimitedTime = OT.LimitedTime;
                            pc.UsedTime = OT.UsedTime;
                            pc.RemainingTime = OT.RemainingTime;
                            pc.StartTime = OT.StartTime;
                            pc.StopedTime = OT.StopedTime;
                            pc.TimeStatu = OT.TimeStatu;
                            pc.PCStatu = NetworkItems.PCStatus.Online;
                        }
                    }
                    */
                }

                foreach (NetworkItems NI in AllComputers.Values)
                {
                    AddNewPC(NI, this.MonitorView);
                }
                // MessageBox.Show("HA");
                Loaded = true;
              //  RefreshTimer.Interval = 3000;
                InRefreshTimer = false;
                return;
            }

         

            foreach (NetworkItems NI in AllComputers.Values)
            {
                if (!NI.Paused && NI.PCStatu == NetworkItems.PCStatus.Online)
                {
                    if (this.MonitorView.InvokeRequired)
                    {
                        this.MonitorView.BeginInvoke(new MethodInvoker(
                            () => RefreshTimeStatus(NI))).AsyncWaitHandle.WaitOne();
                    }
                    else
                    {
                        RefreshTimeStatus(NI);
                    }
                }

                if (this.MonitorView.InvokeRequired)
                {
                    this.MonitorView.BeginInvoke(new MethodInvoker(
                        () => RefreshPCStatus(NI))).AsyncWaitHandle.WaitOne();
                }
                else
                {
                    RefreshPCStatus(NI);
                }
            }
            if (this.MonitorView.InvokeRequired)
            {
                this.MonitorView.BeginInvoke(new MethodInvoker(
                    () => RefreshItemSelection())).AsyncWaitHandle.WaitOne();
            }
            else
            {
                RefreshItemSelection();
            }

            InRefreshTimer = false;
        }

        #region MessageStatus
        public enum MessageStatus
        {
            login,
            logout,
            tellme,
            addtime,
            limittime,
            unlimittime,
            continuelogin,
            justruninfo,
            applist,
            pause,
            resume,
            terminate,
            terminateall,
            checkconnected,
            changepc,
            activeusbwrite,
            disableusbwrite,
            sendmessage,
            restart,
            shutdown,
            sendfile,
            startremote,
            endremote,
        }
        #endregion
        public void SendPCStatus(NetworkItems NI, MessageStatus MS, bool obj = false, uint obj2 = 0, bool Paid = false, NetworkItems NI2 = null, string Message = "", string Dir = "", int SPort = 0)
        {
            if (string.IsNullOrEmpty(NI.IP))
                return;
         //    if (MS != MessageStatus.checkconnected)
       //     writeamess("MESSAGE Send : " + MS.ToString() + ", To : " + NI.HostName + ", IP: " + NI.IP + Environment.NewLine);
            string MyMessage = "";
            switch (MS)
            {
                case MessageStatus.startremote:
                    {
                        MyMessage = "startremote;" + NetworkItems.MyIp + ";" + obj2 + ";" + SPort;
                        break;
                    }
                case MessageStatus.endremote:
                    {
                        MyMessage = "endremote;" + NetworkItems.MyIp;// +";" + obj2 + ";" + SPort;
                        break;
                    }
                case MessageStatus.sendfile:
                    {
                        MyMessage = "sendfile;" + NetworkItems.MyIp + ";" + Dir + ";" + Message;
                        break;
                    }
                case MessageStatus.restart:
                    {
                        MyMessage = "restart;" + NetworkItems.MyIp;
                        break;
                    }
                case MessageStatus.shutdown:
                    {
                        MyMessage = "shutdown;" + NetworkItems.MyIp;
                        break;
                    }
                case MessageStatus.sendmessage:
                    {
                        MyMessage = "sendmessage;" + NetworkItems.MyIp + ";" + Message;
                        break;
                    }
                case MessageStatus.changepc:
                    {
                        MyMessage = "changepc;" + NetworkItems.MyIp + ";" + Message;
                        break;
                    }
                case MessageStatus.activeusbwrite:
                    {
                        MyMessage = "activeusbwrite;" + NetworkItems.MyIp;
                        break;
                    }
                case MessageStatus.disableusbwrite:
                    {
                        MyMessage = "disableusbwrite;" + NetworkItems.MyIp;
                        break;
                    }
                case MessageStatus.checkconnected:
                    {
                        MyMessage = "checkconnected;" + NetworkItems.MyIp;
                        break;
                    }
                case MessageStatus.terminate:
                    {
                        MyMessage = "terminate;" + NetworkItems.MyIp + ";" + obj2.ToString();
                        break;
                    }
                case MessageStatus.terminateall:
                    {
                        MyMessage = "terminateall;" + NetworkItems.MyIp;
                        break;
                    }
                case MessageStatus.pause:
                    {
                        MyMessage = "pause;" + NetworkItems.MyIp;
                        break;
                    }
                case MessageStatus.resume:
                    {
                        MyMessage = "resume;" + NetworkItems.MyIp;
                        break;
                    }
                case MessageStatus.applist:
                    {
                        MyMessage = "applist;" + NetworkItems.MyIp;
                        break;
                    }
                case MessageStatus.unlimittime:
                    {
                        MyMessage = "unlimittime;" + NetworkItems.MyIp;
                        break;
                    }
                case MessageStatus.continuelogin:
                    {
                        MyMessage = "continuelogin;" + NetworkItems.MyIp + ";" + (DateTime.Now - NI.StartTime).Ticks + ";" + (DateTime.Now - NI.StopedTime).Ticks + ";" + NI.LimitedTime + ";" + NI.CanUseInternet.ToString() + ";" + NI.Paid.ToString() + ";" + NI.PausedTime;
                        break;
                    }
                case MessageStatus.tellme:
                    {
                        MyMessage = "tellme;" + NetworkItems.MyIp + ";" +
                        Program.EnableUSBPluginWarning + ";" +
                        Program.EnableUSBPlugoutWarning + ";" +
                        Program.HourPrice + ";" +
                        Program.MinimumCost + ";" +
                        Program.MyUserName + ";" +
                        Program.MyPassword + ";";
                        break;
                    }
                case MessageStatus.addtime:
                    {
                      //  writeamess("addtime : " + obj2);
                        //-              Time
                        MyMessage = "addtime;" + NetworkItems.MyIp + ";" + obj2 + ";" + obj.ToString() + ";" + Paid;
                        break;
                    }
                case MessageStatus.limittime:
                    {                                                    //  Time
                        MyMessage = "limittime;" + NetworkItems.MyIp + ";" + obj2.ToString() + ";" + Paid;
                        break;
                    }
                case MessageStatus.login:
                    {
                        if (obj2 <= 0) //Time Login
                        {
                            if (obj)  //Internet 
                                MyMessage = "login;" + NetworkItems.MyIp + ";nointernet;" + Paid;//4
                            else
                                MyMessage = "login;" + NetworkItems.MyIp + ";" + Paid;//3
                        }
                        else
                        {//Time
                            if (obj)  //Internet
                                MyMessage = "login;" + NetworkItems.MyIp + ";" + obj2 + ";nointernet;" + Paid;//5
                            else
                                MyMessage = "login;" + NetworkItems.MyIp + ";" + obj2 + ";" + Paid;//4
                        }
                        break;
                    }
                case MessageStatus.logout:
                    {
                        MyMessage = "logout;" + NetworkItems.MyIp;
                        break;
                    }
                default:
                    {
                        writeamess("Unknown MESSAGE : " + MS + Environment.NewLine);
                        break;
                    }
            }
            SendSock(MyMessage, NI);
            // writeamess("MESSAGE Sent : " + MyMessage + ", To : " + NI.HostName + Environment.NewLine);
        }

        public string oldm = "";
        public void writeamess(string mess)
        {
            if (oldm == mess)
                return;
            oldm = mess;
            if (this.richTextBox1.InvokeRequired)
            {
                this.richTextBox1.BeginInvoke(new MethodInvoker(
                    () => richTextBox1.Text += mess)).AsyncWaitHandle.WaitOne();
            }
            else
            {
                richTextBox1.Text += mess;
            }
        }

        public void JustRunMessage(string[] Infos, NetworkItems ThisPC, bool Change = false, bool havenothing = false)
        {
            if (ThisPC == null)
            {
                NetworkItems.RefreshLoads();
                ThisPC = NetworkItems.ALLNetworks[Infos[1]];
            }
            if (ThisPC != null)
            {
                if (Change == false && !havenothing)
                {
                    ThisPC.IP = Infos[1];
                    ThisPC.HostName = Infos[2];
                    ThisPC.UserName = Infos[3];
                    ThisPC.MyVersion = Infos[4];
                    ThisPC.MyOperatingSystem = Infos[5];
                    ThisPC.TotalMemory = (string)(Convert.ToUInt64(Infos[6]) / 1024 / 1024).ToString();
                    ThisPC.FreeMemory = (string)(Convert.ToUInt64(Infos[7]) / 1024 / 1024).ToString();
                    ThisPC.Connected = true;
                    ThisPC.LastCheckConnected = new TimeSpan(DateTime.Now.Ticks);
                    /*
                    if (string.IsNullOrEmpty(ThisPC.MacAddress))
                        ThisPC.MacAddress = NetworkBrowser.getMacByIp(ThisPC.IP);
                    */
                }
                string MyMessage = "justrunok;" +
                    NetworkItems.MyIp + ";" +
                    (DateTime.Now - ThisPC.StartTime).Ticks + ";" +
                    (DateTime.Now - ThisPC.StopedTime).Ticks + ";" +
                    ThisPC.LimitedTime + ";" +
                    ThisPC.CanUseInternet.ToString() + ";" +
                    ThisPC.Paid.ToString() + ";" +
                    ThisPC.Login.ToString() + ";" +

                    Program.EnableUSBPluginWarning + ";" +
                    Program.EnableUSBPlugoutWarning + ";" +
                    Program.HourPrice + ";" +
                    Program.MinimumCost + ";" +
                    Program.MyUserName + ";" +
                    Program.MyPassword + ";" +
                    ThisPC.Paused.ToString() + ";" +
                    ThisPC.PausedTime;

                SendSock(MyMessage, ThisPC);
            }
            else
            {
                NetworkItems.RefreshLoads();
            }
            RefreshPCStatus(ThisPC);
        }

        public void MonitorView_ItemSelectionChanged(object Sender, ListViewItemSelectionChangedEventArgs LVISC)
        {
            if (LVISC.IsSelected)
            {
                UsernameLabel.Text = LVISC.Item.SubItems[1].Text;
                StartTimelabel.Text = LVISC.Item.SubItems[4].Text;
                RemainingTimeLabel.Text = LVISC.Item.SubItems[5].Text;
                UsedTimeLabel.Text = LVISC.Item.SubItems[6].Text;
                UsageCostLabel.Text = LVISC.Item.SubItems[7].Text;
               
                NetworkItems NI = NetworkItems.ALLNetworks[LVISC.Item.Tag.ToString()];

                if (NI.TimeStatu == NetworkItems.TimeStatus.Time && NI.Connected)
                {
                    if (NI.Login)
                    {
                        PlayandPaycheck.Enabled = ConverttoUnlimitedButton.Enabled = Min5Button.Enabled = Min20Button.Enabled = Min30Button.Enabled = Min40Button.Enabled = Min50Button.Enabled = Min60Button.Enabled = Min70Button.Enabled = Min80Button.Enabled = Min90Button.Enabled = ConverttoUnlimitedButton.Enabled = true;
                        LimitTimeButton.Enabled = false;
                        try
                        {
                            progressBarEx1.Maximum = (int)NI.LimitedTime;
                            progressBarEx1.Value = (int)NI.UsedTime;
                        }
                        catch { }
                    }
                    else
                        ConverttoUnlimitedButton.Enabled = false;
                }
                else if (NI.TimeStatu == NetworkItems.TimeStatus.Login && NI.Connected)
                {
                    if (NI.Login)
                    {
                        PlayandPaycheck.Enabled = LimitTimeButton.Enabled = Min5Button.Enabled = Min20Button.Enabled = Min30Button.Enabled = Min40Button.Enabled = Min50Button.Enabled = Min60Button.Enabled = Min70Button.Enabled = Min80Button.Enabled = Min90Button.Enabled = LimitTimeButton.Enabled = true;
                    }
                    else
                        LimitTimeButton.Enabled = false;

                    ConverttoUnlimitedButton.Enabled = false;
                    progressBarEx1.Value = 0;
                }
                else
                    PlayandPaycheck.Enabled = ConverttoUnlimitedButton.Enabled = LimitTimeButton.Enabled = Min5Button.Enabled = Min20Button.Enabled = Min30Button.Enabled = Min40Button.Enabled = Min50Button.Enabled = Min60Button.Enabled = Min70Button.Enabled = Min80Button.Enabled = Min90Button.Enabled = LimitTimeButton.Enabled = false;
            }
            else
            {
                UsernameLabel.Text = "N/A";
                StartTimelabel.Text = "00:00";
                UsedTimeLabel.Text = "00:00";
                RemainingTimeLabel.Text = "00:00";
                UsageCostLabel.Text = "0000 جم";
                progressBarEx1.Value = 0;
                PlayandPaycheck.Enabled = ConverttoUnlimitedButton.Enabled = LimitTimeButton.Enabled = Min5Button.Enabled = Min20Button.Enabled = Min30Button.Enabled = Min40Button.Enabled = Min50Button.Enabled = Min60Button.Enabled = Min70Button.Enabled = Min80Button.Enabled = Min90Button.Enabled = false;

            }
        }

        public void RefreshItemSelection()
        {
            if (this.MonitorView.InvokeRequired)
            {
                this.MonitorView.BeginInvoke(new MethodInvoker(
                    () =>
                    {
                        if (this.MonitorView.SelectedItems.Count <= 0)
                            return;
                        ListViewItem SLV = this.MonitorView.SelectedItems[0];
                        if (SLV != null)
                        {
                            MonitorView_ItemSelectionChanged(null, new ListViewItemSelectionChangedEventArgs(SLV, SLV.Index, true));
                        }
                    }
                    ));
            }
            else
            {
                if (this.MonitorView.SelectedItems.Count <= 0)
                    return;
                ListViewItem SLV = this.MonitorView.SelectedItems[0];
                if (SLV != null)
                {
                    MonitorView_ItemSelectionChanged(null, new ListViewItemSelectionChangedEventArgs(SLV, SLV.Index, true));
                }
            }
         
        }

        public void RefreshStatusBar()
        {
            ClientsNum.Text = NetworkItems.ALLNetworks.Count.ToString();
            OnlineNum.Text = NetworkItems.LoginNetworks.Count.ToString();
            IdleNum.Text = NetworkItems.IdleNetworks.Count.ToString();
            UnreachableNum.Text = NetworkItems.OfflineNetworkInfo.Count.ToString();
            DateTimeStatus.Text = DateTime.Now.ToString().Replace(" ", " - ");
        }

        public NetworkItems CanLoginOrLogoutSelected()
        {
            ListViewItem LVI = MonitorView.SelectedItems[0];
            if (LVI != null)
            {
                NetworkItems NI = NetworkItems.NetworkInfo[LVI.Tag.ToString()];
                if (NI != null)
                {
                    if (NI.PCStatu == NetworkItems.PCStatus.Online && NI.Connected)
                    {
                        return NI;
                    }
                }
            }
            return null;
        }

        public void SelectedTimeButton(int RT)
        {
            NetworkItems NI = NetworkItems.ALLNetworks[MonitorView.SelectedItems[0].Tag.ToString()];
            if (NI != null)
            {
                if (NI.Login && NI.Connected)
                {
                    if (NI.TimeStatu == NetworkItems.TimeStatus.Login)
                    {
                        LimitTimeButton.PerformClick();
                    }
                    else if (NI.TimeStatu == NetworkItems.TimeStatus.Time)
                    {
                        addToolStripMenuItem.PerformClick();
                    }
                }
                else
                {
                    TimeLimited TL = new TimeLimited(this, NI);
                    TL.GameCheckBox.Visible = TL.PaidCheckBox.Visible = true;
                    TL.Text = "Time Limited : " + NI.ShownName;
                    TL.TimeTextBox.Text = RT.ToString();
                    TL.label3.Text = "Play";
                    TL.ShowDialog();
                    TL.Dispose();
                }
            }
        }

        public void PaymentNow(NetworkItems NI)
        {
            if (NI.StartTime == null)
                return;
            else if (NI.StartTime == new DateTime())
                return;
            else if (NI.UsedTime < 2)
                return;
            PaymentInfo PI = new PaymentInfo();
            PI.Hostname = NI.HostName;
            PI.Price = Convert.ToDouble(NI.TotalCost.Replace(" جم", ""));
            PI.StartTime = NI.StartTime;
            if (NI.TimeStatu == NetworkItems.TimeStatus.Login)
            {
                PI.TimeStatu = PaymentInfo.TimeStatus.Login;
            }
            else
            {
                PI.TimeStatu = PaymentInfo.TimeStatus.Time;
            }
            PI.UsedTime = NI.UsedTime;

            if (NetworkItems.PaymentInfos.ContainsKey(NI.StartTime.Ticks))
            {
                NetworkItems.PaymentInfos.Remove(NI.StartTime.Ticks);
                if (this.SystemLogListView.Items.ContainsKey(NI.StartTime.Ticks.ToString()))
                    this.SystemLogListView.Items.Remove(this.SystemLogListView.Items[NI.StartTime.Ticks.ToString()]);
               else if (this.SystemLogListView.Items.ContainsKey(PI.Hostname))
                    this.SystemLogListView.Items.Remove(this.SystemLogListView.Items[PI.Hostname]);
            }
            if (NetworkItems.PaymentInfos.ContainsKey(NI.StartTime.Ticks) == false)
            {
                NetworkItems.PaymentInfos.Add(NI.StartTime.Ticks, PI);

                /*
                if (NI.StartTime.Date >= dateTimePicker1.Value.Date && NI.StartTime.Date <= dateTimePicker2.Value.Date)
                {
                    string fdsfd;
                    string[] st = new string[4];
                    st[0] = PI.Hostname;
                    st[1] = PI.TimeStatu.ToString();
                    st[2] = PI.UsedTime.ToString();
                    st[3] = PI.Price.ToString();
                    ListViewItem LVI = new ListViewItem(st);
                    LVI.Text = PI.Hostname;
                    LVI.ToolTipText = LVI.Name = PI.StartTime.Ticks.ToString();
                    LVI.Tag = PI.StartTime.Ticks;
                    if (this.SystemLogListView.InvokeRequired)
                    {
                        this.SystemLogListView.BeginInvoke(new MethodInvoker(
                            () => this.SystemLogListView.Items.Add(LVI)));
                    }
                    else
                    {
                        this.SystemLogListView.Items.Add(LVI);
                    }
                    if (TotalShownCostLabel.Text == "0")
                        TotalShownCostLabel.Text = PI.UsedTime + "";
                    else
                    {
                        double OTC = Convert.ToDouble(TotalShownCostLabel.Text);
                        OTC += PI.UsedTime;
                        TotalShownCostLabel.Text = OTC + "";
                    }
                }
                */
            }
            RefreshSystemLogMenu();
            MyDatabase.SaveSystemlog(PI);
        }
        public void RefreshSystemLogMenu()
        {
         
            TotalShownCostLabel.Text = "0";
            if (SystemLogListView.InvokeRequired)
            {
                SystemLogListView.BeginInvoke(new MethodInvoker(
                    () => SystemLogListView.Items.Clear()));
            }
            else
            {
                SystemLogListView.Items.Clear();
            }
       
            double TP = 0;
            foreach (PaymentInfo PI in NetworkItems.PaymentInfos.Values)
            {
                if (PI.StartTime.Date >= dateTimePicker1.Value.Date &&  PI.StartTime.Date <= dateTimePicker2.Value.Date)// if (dateTimePicker1.Value >= PI.StartTime && dateTimePicker2.Value <= PI.StartTime)
                {
                    string[] st = new string[4];
                    st[0] = PI.Hostname;
                    st[1] = PI.TimeStatu.ToString();
                    st[2] = PI.UsedTime.ToString();
                    st[3] = PI.Price.ToString();
                    ListViewItem LVI = new ListViewItem(st);
                    LVI.Text = st[0];
                    LVI.ToolTipText = LVI.Name = PI.StartTime.Ticks.ToString();
                    LVI.Tag = PI.StartTime.Ticks;
                    if (SystemLogListView.InvokeRequired)
                    {
                        SystemLogListView.BeginInvoke(new MethodInvoker(
                            () => SystemLogListView.Items.Add(LVI)));
                    }
                    else
                    {
                        SystemLogListView.Items.Add(LVI);
                    }
                    TP += PI.Price;
                }
            }
            TotalShownCostLabel.Text = TP + "";
        }
        public void dateTimePicker1_ValueChanged(object sender, EventArgs eeee)
        {
            RefreshSystemLogMenu();
        }

        public void PrepareContextMenu(ListViewItem LVI)
        {
            NetworkItems NI = NetworkItems.ALLNetworks[LVI.Tag.ToString()];

            if (NI.PCStatu == NetworkItems.PCStatus.Online && NI.Connected)
            {
              controlToolStripMenuItem.Enabled = advancedToolStripMenuItem.Enabled = loginToolStripMenuItem.Enabled = prepaidToolStripMenuItem.Enabled = playAndPayToolStripMenuItem.Enabled = GameToolStripMenuItem.Enabled =
                                    loginToolStripMenuItem.Enabled = prepaidToolStripMenuItem.Enabled = playAndPayToolStripMenuItem.Enabled = GameToolStripMenuItem.Enabled =
                     timeToolStripMenuItem.Enabled = logoutToolStripMenuItem.Enabled = continueToolStripMenuItem.Enabled = continueToolStripMenuItem.Enabled = limitTimeToolStripMenuItem.Enabled = resumeTimeToolStripMenuItem.Enabled = pauseTimeToolStripMenuItem.Enabled = paymentToolStripMenuItem.Enabled = continueToolStripMenuItem.Enabled = addToolStripMenuItem.Enabled = convertToUnlimitedToolStripMenuItem.Enabled = true;

                wakeUPToolStripMenuItem.Visible = false;

                if (NI.Login)
                {
                    if (NI.TimeStatu == NetworkItems.TimeStatus.Login)
                    {
                        loginToolStripMenuItem.Enabled = prepaidToolStripMenuItem.Enabled = playAndPayToolStripMenuItem.Enabled = GameToolStripMenuItem.Enabled =
                           paymentToolStripMenuItem.Enabled = continueToolStripMenuItem.Enabled = addToolStripMenuItem.Enabled = convertToUnlimitedToolStripMenuItem.Enabled = false;
                        if (NI.Paused)
                            pauseTimeToolStripMenuItem.Enabled = false;
                        else
                            resumeTimeToolStripMenuItem.Enabled = false;
                    }
                    else//TIME
                    {
                        //   writeamess("1 Unlimit : " + ConverttoUnlimitedButton.Enabled + ", Limit : " + LimitTimeButton.Enabled + Environment.NewLine);
                        loginToolStripMenuItem.Enabled = prepaidToolStripMenuItem.Enabled = playAndPayToolStripMenuItem.Enabled = GameToolStripMenuItem.Enabled =
                        continueToolStripMenuItem.Enabled = limitTimeToolStripMenuItem.Enabled = false;
                        if (NI.Paused)
                            pauseTimeToolStripMenuItem.Enabled = false;
                        else
                            resumeTimeToolStripMenuItem.Enabled = false;

                        if (NI.Paid)
                            paymentToolStripMenuItem.Enabled = false;
                    }
                    // writeamess("1 Unlimit : " + ConverttoUnlimitedButton.Enabled + ", Limit : " + LimitTimeButton.Enabled + Environment.NewLine);
                }
                else
                {
                   // MessageBox.Show("kk");
                    timeToolStripMenuItem.Enabled = logoutToolStripMenuItem.Enabled = false;
                    if (NI.RemainingTime <= 0 && NI.TimeStatu != NetworkItems.TimeStatus.Time || NI.UsedTime <= 0 && NI.TimeStatu == NetworkItems.TimeStatus.Login || NI.TimeStatu == NetworkItems.TimeStatus.None)
                    {
                        changePCToolStripMenuItem.Enabled = false;
                    }
                    if (NI.Paid)
                        paymentToolStripMenuItem.Enabled = false;

                    if (NI.RemainingTime > 0 && NI.TimeStatu == NetworkItems.TimeStatus.Time || NI.UsedTime > 0 && NI.TimeStatu == NetworkItems.TimeStatus.Login)
                        continueToolStripMenuItem.Enabled = true;
                    else
                    {
                        //MessageBox.Show("kk: "+ NI.TimeStatu);
                        continueToolStripMenuItem.Enabled = false;
                    }
                }
                if (NI.RemainingTime > 0 && NI.TimeStatu == NetworkItems.TimeStatus.Time || NI.UsedTime > 0 && NI.TimeStatu == NetworkItems.TimeStatus.Login)
                {
                    changePCToolStripMenuItem.Enabled = true;
                }
            }
            else
            {
             controlToolStripMenuItem.Enabled = advancedToolStripMenuItem.Enabled =   loginToolStripMenuItem.Enabled = prepaidToolStripMenuItem.Enabled = playAndPayToolStripMenuItem.Enabled = GameToolStripMenuItem.Enabled =
                          loginToolStripMenuItem.Enabled = prepaidToolStripMenuItem.Enabled = playAndPayToolStripMenuItem.Enabled = GameToolStripMenuItem.Enabled =
           timeToolStripMenuItem.Enabled = logoutToolStripMenuItem.Enabled = continueToolStripMenuItem.Enabled = continueToolStripMenuItem.Enabled = limitTimeToolStripMenuItem.Enabled = resumeTimeToolStripMenuItem.Enabled = pauseTimeToolStripMenuItem.Enabled = paymentToolStripMenuItem.Enabled = continueToolStripMenuItem.Enabled = addToolStripMenuItem.Enabled = convertToUnlimitedToolStripMenuItem.Enabled = false;

                wakeUPToolStripMenuItem.Enabled = wakeUPToolStripMenuItem.Visible = true;

                if (NI.RemainingTime > 0 && NI.TimeStatu == NetworkItems.TimeStatus.Time || NI.UsedTime > 0 && NI.TimeStatu == NetworkItems.TimeStatus.Login)
                {
                     changePCToolStripMenuItem.Enabled = true;
                    if (NI.Connected)
                        continueToolStripMenuItem.Enabled = true;
                }
                else
                    changePCToolStripMenuItem.Enabled = false;
                if (!NI.Paid && NI.RemainingTime > 0 && NI.TimeStatu == NetworkItems.TimeStatus.Time || !NI.Paid && NI.UsedTime > 0 && NI.TimeStatu == NetworkItems.TimeStatus.Login)
                    paymentToolStripMenuItem.Enabled = true;
                else
                    paymentToolStripMenuItem.Enabled = false;
            }

            if (Program.isAdmin)
                deleteSelectedToolStripMenuItem.Visible = renameToolStripMenuItem.Visible = true;
            else
                deleteSelectedToolStripMenuItem.Visible = renameToolStripMenuItem.Visible = false;
        }

        public void RefreshTimeStatus(NetworkItems NI)
        {
            if (NI.TimeStatu == NetworkItems.TimeStatus.Login && NI.PCStatu == NetworkItems.PCStatus.Online)
            {
                NI.UsedTime = Program.GetUsedTime(NI.StartTime, NI.StopedTime, NI.PausedTime);
                NI.RemainingTime = 0;
            }
            else if (NI.TimeStatu == NetworkItems.TimeStatus.Time && NI.PCStatu == NetworkItems.PCStatus.Online)
            {
                NI.UsedTime = Program.GetUsedTime(NI.StartTime, NI.StopedTime, NI.PausedTime);
                NI.RemainingTime = (NI.LimitedTime - NI.UsedTime);
                if (NI.RemainingTime <= 0)
                {
                    if (NI.Login)
                    {
                        if (NI.TimeStatu != NetworkItems.TimeStatus.None)
                            SendPCStatus(NI, ALEmanCafeServer.MessageStatus.logout);
                    }
                }
            }
        }

        public void AddNewPC(NetworkItems NI, ListView MonitorView)
        {
            if (NetworkItems.OldTimers.ContainsKey(NI.IP))
            {
                NetworkItems NI2 = NetworkItems.OldTimers[NI.IP];
                NI.ShownName = NI2.ShownName;
                NI.HostName = NI2.HostName;
                NI.CanUseInternet = NI2.CanUseInternet;
             //   MessageBox.Show("IP : " + NI.IP + ", OIP : "+NI2.IP);
               // NI.IP = NI2.IP;
                NI.LimitedTime = NI2.LimitedTime;
              //  NI.MacAddress = NI2.MacAddress;
                NI.Paid = NI2.Paid;
                NI.TimeStatu = NI2.TimeStatu;
                NI.RemainingTime = NI2.RemainingTime;
                NI.StartTime = NI2.StartTime;
                NI.PausedTime = NI2.PausedTime;
                NI.StopedTime = NI2.StopedTime;
                NI.UsedTime = NI2.UsedTime;
              //  writeamess(NI.IP + ", " + NI.MacAddress + ", " + NI.TimeStatu + ", " + NI.Connected + ", " + NI.PCStatu);
            }

            string[] Items = new string[13];
            Items[0] = NI.ShownName;
            Items[1] = NI.ShownName;
            Items[2] = NI.PCStatu.ToString();
            Items[3] = NI.IP;
            Items[4] = NI.StartTime.Year.ToString() == "1" ? "00:00" : NI.StartTime.ToString("tt hh:mm"); //NI.StartTime.ToString();
            Items[5] = NI.RemainingTime.ToString();
            Items[6] = NI.UsedTime.ToString();
            Items[7] = NI.UsageCost.ToString();
            Items[8] = NI.RunningApplication;
            Items[9] = NI.Title;
            Items[10] = NI.MacAddress;
            Items[11] = NI.MyOperatingSystem;
            Items[12] = NI.MyVersion;
            ListViewItem LV = new ListViewItem(Items);
            LV.Name = LV.Text = NI.ShownName;
            LV.ToolTipText = "Host Name : " + NI.HostName + Environment.NewLine +
                "User : " + NI.PCStatu.ToString() + Environment.NewLine +
                "IP : " + NI.IP + Environment.NewLine +
                "Start Time : " + NI.StartTime.Year.ToString() == "1" ? "00:00" + Environment.NewLine : NI.StartTime.ToString("tt hh:mm") + Environment.NewLine +
                "Used Time : " + NI.UsedTime.ToString() + Environment.NewLine +
                "Remaining Time : " + NI.RemainingTime.ToString() + Environment.NewLine +
                "Usage Cost : " + NI.UsageCost + Environment.NewLine +
                "Running Application : " + NI.RunningApplication + Environment.NewLine +
                "Title : " + NI.Title + Environment.NewLine +
                "MacAddress : " + NI.MacAddress + Environment.NewLine +
                "OS : " + NI.MyOperatingSystem + Environment.NewLine +
            "Version : " + NI.MyVersion;
            LV.Tag = NI.IP;
            //  writeamess(NI.TimeStatu + ", " + NI.Connected + ", " + NI.PCStatu);

            if (NI.Connected == false || NI.PCStatu == NetworkItems.PCStatus.Offline)
                LV.ImageKey = "OfflineDisplay";
            else if (NI.Login)
            {
                if (NI.Paused)
                    LV.ImageKey = "PauseDisplay";
                else if (NI.TimeStatu == NetworkItems.TimeStatus.None)
                    LV.ImageKey = "OnlineDisplay";
                else if (NI.TimeStatu == NetworkItems.TimeStatus.Time)
                    LV.ImageKey = "TimeMonitor";
                else
                    LV.ImageKey = "LoginMonitor";
                NI.Connected = true;
            }
            else
            {
                NI.Connected = true;
                LV.ImageKey = "OnlineDisplay";
            }



            if (MonitorView.InvokeRequired)
            {
                MonitorView.BeginInvoke(new MethodInvoker(
                    () => MonitorView.Items.Add(LV)));
            }
            else
            {
                MonitorView.Items.Add(LV);
            }

            if (NI.PCStatu == NetworkItems.PCStatus.Online)//&& NI.Connected
            {
               
                this.SendPCStatus(NI, MessageStatus.tellme);
            }

            /*
            if (MonitorView.InvokeRequired)
            {
                MonitorView.BeginInvoke(new MethodInvoker(
                    () => MonitorView.Refresh()));
            }
            else
            {
                MonitorView.Refresh();
            }
            */
            Application.DoEvents();
        }

        public void RefreshPCStatusNow(NetworkItems NI)
        {
            var It = MonitorView.Items.Find(NI.ShownName, false);
            if (It.Length > 0)
            {
                var Item = It[0];
                if (Item != null)
                {
                    //added before
                    Item.SubItems[0].Text = NI.ShownName;
                    Item.SubItems[1].Text = NI.ShownName;
                    Item.SubItems[2].Text = NI.PCStatu.ToString();
                    Item.SubItems[3].Text = NI.IP;
                    Item.SubItems[4].Text = NI.StartTime.Year.ToString() == "1" ? "00:00" : NI.StartTime.ToString("tt hh:mm");
                    Item.SubItems[5].Text = Program.GetUsedAndRemTime(NI.RemainingTime);
                    Item.SubItems[6].Text = Program.GetUsedAndRemTime(NI.UsedTime);
                    Item.SubItems[7].Text = NI.UsageCost.ToString();
                    Item.SubItems[8].Text = NI.RunningApplication;
                    Item.SubItems[9].Text = NI.Title;
                    Item.SubItems[10].Text = NI.MacAddress;
                    Item.SubItems[11].Text = NI.MyOperatingSystem;
                    Item.SubItems[12].Text = NI.MyVersion;

                    Item.ToolTipText = "Host Name : " + NI.HostName + Environment.NewLine +
                    "User : " + NI.PCStatu.ToString() + Environment.NewLine +
                    "IP : " + NI.IP + Environment.NewLine +
                    "Start Time : " + NI.StartTime.Year.ToString() == "1" ? "00:00" + Environment.NewLine : NI.StartTime.ToString("tt hh:mm") + Environment.NewLine +
                    "Used Time : " + NI.UsedTime.ToString() + Environment.NewLine +
                    "Remaining Time : " + NI.RemainingTime.ToString() + Environment.NewLine +
                    "Usage Cost : " + NI.UsageCost + Environment.NewLine +
                    "Running Application : " + NI.RunningApplication + Environment.NewLine +
                    "Title : " + NI.Title + Environment.NewLine +
                    "MacAddress : " + NI.MacAddress + Environment.NewLine +
                    "OS : " + NI.MyOperatingSystem + Environment.NewLine +
                    "Version : " + NI.MyVersion;

                    if (NI.PCStatu == NetworkItems.PCStatus.Offline || NI.Connected == false)
                    {
                        NI.Connected = false;
                        Item.ImageKey = "OfflineDisplay";
                    }
                    else if (NI.Login)
                    {
                        if (NI.Paused)
                            Item.ImageKey = "PauseDisplay";
                        else if (NI.TimeStatu == NetworkItems.TimeStatus.None)
                            Item.ImageKey = "OnlineDisplay";
                        else if (NI.TimeStatu == NetworkItems.TimeStatus.Time)
                            Item.ImageKey = "TimeMonitor";
                        else
                            Item.ImageKey = "LoginMonitor";
                        NI.Connected = true;
                    }
                    else
                    {
                        NI.Connected = true;
                        Item.ImageKey = "OnlineDisplay";
                    }
                }
                else
                {
                 //   MessageBox.Show("ER? " + NI.IP);
                    AddNewPC(NI, MonitorView);
                }
            }
            else AddNewPC(NI, MonitorView);
        }

        public void RefreshPCStatus(NetworkItems NI)
        {
            if (this.MonitorView.InvokeRequired)
            {
                this.MonitorView.BeginInvoke((MethodInvoker)delegate()
                {
                     RefreshPCStatusNow(NI);
                }).AsyncWaitHandle.WaitOne();
            }
            else
            {
                RefreshPCStatusNow(NI);
            }
        }

        public void toolStripComboBox1_TextChanged(object sender, EventArgs eeee)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new MethodInvoker(
                    () => this.Font = new System.Drawing.Font("Tahoma", float.Parse(toolStripComboBox1.Text))));
            }
            else
            {
                this.Font = new System.Drawing.Font("Tahoma", float.Parse(toolStripComboBox1.Text));//, FontStyle.Bold
            }
        }

        private void MonitorView_MouseClick(object Sender, MouseEventArgs eeee)
        {
            if (eeee.Button == System.Windows.Forms.MouseButtons.Right)
            {
                ListView.SelectedListViewItemCollection SLV = this.MonitorView.SelectedItems;
                if (SLV.Count > 0)
                {
                    PrepareContextMenu(SLV[0]);
                    contextMenuStrip1.Show(MousePosition);
                }
            }
            else if (eeee.Button == System.Windows.Forms.MouseButtons.Left && eeee.Clicks > 1)
            {
                NetworkItems NI = CanLoginOrLogoutSelected();
                if (NI != null)
                {
                    if (!NI.Login)
                        SendPCStatus(NI, ALEmanCafeServer.MessageStatus.login);
                    else
                        SendPCStatus(NI, ALEmanCafeServer.MessageStatus.logout);
                }
                /*
               ListViewItem LVI = MonitorView.SelectedItems[0];
               if (LVI != null)
               {
                   NetworkItems NI = NetworkItems.NetworkInfo[LVI.Tag.ToString()];
                   if (NI != null)
                   {
                       if (NI.PCStatu == NetworkItems.PCStatus.Online)
                       {

                       }
                   }
               }
                */
            }
        }

        private void MonitorView_KeyPress(object Sender, KeyEventArgs eeee)
        {
            if (eeee.KeyCode == Keys.Apps || eeee.KeyData == Keys.Apps || eeee.Modifiers == Keys.Apps)
            {
                ListView.SelectedListViewItemCollection SLV = this.MonitorView.SelectedItems;
                if (SLV.Count > 0)
                {
                    PrepareContextMenu(SLV[0]);
                    contextMenuStrip1.Show(MousePosition);
                }
            }
        }

        private void restartToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(this, "Are you sure? You want to restart?", "Restart Warning!", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
            {
                IsRestart = true;
                this.LoginForm.RestartNow();
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.LoginForm.ExitNow();
        }

        public void detailedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.MonitorView.InvokeRequired)
            {
                this.MonitorView.BeginInvoke(new MethodInvoker(
                    () =>          MonitorView.View = View.Details));
            }
            else
            {
                MonitorView.View = View.Details;
            }
   
            detailedToolStripMenuItem.Checked = true;
            iconicToolStripMenuItem.Checked = false;
            Application.DoEvents();
            if (this.MonitorView.InvokeRequired)
            {
                this.MonitorView.BeginInvoke(new MethodInvoker(
                    () => MonitorView.Refresh()));
            }
            else
            {
                MonitorView.Refresh();
            }
            Application.DoEvents();
        }

        public void iconicToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.MonitorView.InvokeRequired)
            {
                this.MonitorView.BeginInvoke(new MethodInvoker(
                    () =>     MonitorView.View = View.LargeIcon));
            }
            else
            {
                MonitorView.View = View.LargeIcon;
            }
        
            detailedToolStripMenuItem.Checked = false;
            iconicToolStripMenuItem.Checked = true;
            Application.DoEvents();
            if (this.MonitorView.InvokeRequired)
            {
                this.MonitorView.BeginInvoke(new MethodInvoker(
                    () => MonitorView.Refresh()));
            }
            else
            {
                MonitorView.Refresh();
            }
            Application.DoEvents();
        }

        public void statusbarToolStripMenuItem_Click(object sender, EventArgs e)
        {
         
            if (statusbarToolStripMenuItem.Checked)
                statusbarToolStripMenuItem.Checked = statusStrip1.Visible = false;
                /*
            else if (this.statusStrip1.InvokeRequired)
            {

            }
            else if (this.InvokeRequired)
            {

            }*/
            else
                statusbarToolStripMenuItem.Checked = statusStrip1.Visible = true;
        }

        public void StatusMenuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (StatusMenuToolStripMenuItem.Checked)
                StatusMenuToolStripMenuItem.Checked = StatusMenu1.Visible = false;
            else
                StatusMenuToolStripMenuItem.Checked = StatusMenu1.Visible = true;
        }

        public void ALEmanCafeServer_FormClosing(object Sender, FormClosingEventArgs fc)
        {
            if (IsRestart == false)
            {
                fc.Cancel = true;
                this.LoginForm.ExitNow();
            }
        }

        public void gridToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridToolStripMenuItem.Checked)
                gridToolStripMenuItem.Checked = MonitorView.GridLines = false;
            else
                gridToolStripMenuItem.Checked = MonitorView.GridLines = true;
        }

        private void wakeUPToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ListViewItem SLV = this.MonitorView.SelectedItems[0];
            if (SLV != null)
            {
                try
                {
                    bool Stat = new WOLClass().WakeFunction(SLV.SubItems[10].Text);
                    if (!Stat)
                    {
                        var ff = NetworkItems.ALLNetworks[SLV.SubItems[10].Text];
                        WOLClass.WakeUp(ff.MacAddress, ff.IP, ff.HostName);
                        MessageBox.Show("this PC cannot wakeup!");
                    }
                     
                }
                catch { MessageBox.Show("Error! this PC cannot wakeup!"); }
            }
            else
                MessageBox.Show("Please select the PC first");
        }

        private void loginToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NetworkItems NI = CanLoginOrLogoutSelected();
            if (NI != null)
            {
                if (NI.Login == false)
                    SendPCStatus(NI, ALEmanCafeServer.MessageStatus.login, false);
                else////////////
                    SendPCStatus(NI, ALEmanCafeServer.MessageStatus.logout);
            }
            else
                MessageBox.Show("Unknown loginToolStripMenuItem!");
        }

        private void logoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NetworkItems NI = CanLoginOrLogoutSelected();
            if (NI != null)
            {
                if (NI.TimeStatu != NetworkItems.TimeStatus.None)
                    SendPCStatus(NI, ALEmanCafeServer.MessageStatus.logout);
            }
            else
                MessageBox.Show("Unknown PC!");
        }

        private void addToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddTime AT = new AddTime(this, NetworkItems.ALLNetworks[MonitorView.SelectedItems[0].Tag.ToString()]);
            AT.TLT = ALEmanCafe_Server.AddTime.TimeLemetionType.AddTime;
            AT.Text = "Time Limited : " + MonitorView.SelectedItems[0].Text;
            AT.label3.Text = "Add Time";
            AT.ShowDialog();
            AT.Dispose();
        }

        public void prepaidToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TimeLimited TL = new TimeLimited(this, NetworkItems.ALLNetworks[MonitorView.SelectedItems[0].Tag.ToString()]);
            TL.TLT = TimeLimited.TimeLemetionType.Prepaid;
            TL.Text = "Time Limited : " + MonitorView.SelectedItems[0].Text;
            TL.label3.Text = "Prepaid";
            TL.ShowDialog();
        }

        private void playAndPayToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TimeLimited TL = new TimeLimited(this, NetworkItems.ALLNetworks[MonitorView.SelectedItems[0].Tag.ToString()]);
            TL.TLT = TimeLimited.TimeLemetionType.PlayAndPay;
            TL.Text = "Time Limited : " + MonitorView.SelectedItems[0].Text;
            TL.label3.Text = "Play and Pay";
            TL.ShowDialog();
            TL.Dispose();
        }

        private void applicationListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.SendPCStatus(NetworkItems.ALLNetworks[MonitorView.SelectedItems[0].Tag.ToString()], MessageStatus.applist);
            /*
            RemoteApplications RA = new RemoteApplications(this, GetRunningAppsInfo());
            RA.Show();
            */
        }
        
        private void GameLogintoolStripMenuItem5_Click(object sender, EventArgs e)
        {
            NetworkItems NI = CanLoginOrLogoutSelected();
            if (NI != null)
            {
                if (NI.Login == false)
                    SendPCStatus(NI, ALEmanCafeServer.MessageStatus.login, true);
                else////////////
                    SendPCStatus(NI, ALEmanCafeServer.MessageStatus.logout);
            }
        }

        private void GamePlayAndPaytoolStripMenuItem5_Click(object sender, EventArgs e)
        {
            TimeLimited TL = new TimeLimited(this, NetworkItems.ALLNetworks[MonitorView.SelectedItems[0].Tag.ToString()]);
            TL.TLT = TimeLimited.TimeLemetionType.GamePlayAndPay;
            TL.Text = "Time Limited : " + MonitorView.SelectedItems[0].Text;
            TL.label3.Text = "Play and Pay";
            TL.ShowDialog();
            TL.Dispose();
        }

        private void GamePrepaidtoolStripMenuItem5_Click(object sender, EventArgs e)
        {
            TimeLimited TL = new TimeLimited(this, NetworkItems.ALLNetworks[MonitorView.SelectedItems[0].Tag.ToString()]);
            TL.TLT = TimeLimited.TimeLemetionType.GamePrepaid;
            TL.Text = "Time Limited : " + MonitorView.SelectedItems[0].Text;
            TL.label3.Text = "Prepaid";
            TL.ShowDialog();
            TL.Dispose();
        }
     
        private void continueToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.SendPCStatus(NetworkItems.ALLNetworks[MonitorView.SelectedItems[0].Tag.ToString()], MessageStatus.continuelogin);
        }

        private void LimitTimeButton_Click(object sender, EventArgs e)
        {
            limitTimeToolStripMenuItem_Click(sender, e);
        }
        private void limitTimeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddTime AT = new AddTime(this, NetworkItems.ALLNetworks[MonitorView.SelectedItems[0].Tag.ToString()]);
            AT.TLT = ALEmanCafe_Server.AddTime.TimeLemetionType.LimitTime;
            AT.Text = "Time Limitetion : " + MonitorView.SelectedItems[0].Text;
            AT.label3.Text = "Limit Time";
            AT.ShowDialog();
            AT.Dispose();
        }

        private void convertToUnlimitedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(this, "Are you sure to convert session to Unlimited?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                this.SendPCStatus(NetworkItems.ALLNetworks[MonitorView.SelectedItems[0].Tag.ToString()], MessageStatus.unlimittime);
        }
        private void ConverttoUnlimitedButton_Click(object sender, EventArgs e)
        {
            convertToUnlimitedToolStripMenuItem_Click(sender, e);
        }

        private void pauseTimeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SendPCStatus(NetworkItems.ALLNetworks[MonitorView.SelectedItems[0].Tag.ToString()], MessageStatus.pause);
        }

        private void resumeTimeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SendPCStatus(NetworkItems.ALLNetworks[MonitorView.SelectedItems[0].Tag.ToString()], MessageStatus.resume);
        }

        private void Min5Button_Click(object sender, EventArgs e)
        {
            SelectedTimeButton(5);
        }

        private void Min20Button_Click(object sender, EventArgs e)
        {
            SelectedTimeButton(20);
        }

        private void Min30Button_Click(object sender, EventArgs e)
        {
            SelectedTimeButton(30);
        }

        private void Min40Button_Click(object sender, EventArgs e)
        {
            SelectedTimeButton(40);
        }

        private void Min50Button_Click(object sender, EventArgs e)
        {
            SelectedTimeButton(50);
        }

        private void Min60Button_Click(object sender, EventArgs e)
        {
            SelectedTimeButton(60);
        }

        private void Min70Button_Click(object sender, EventArgs e)
        {
            SelectedTimeButton(70);
        }

        private void Min80Button_Click(object sender, EventArgs e)
        {
            SelectedTimeButton(80);
        }

        private void Min90Button_Click(object sender, EventArgs e)
        {
            SelectedTimeButton(90);
        }

        private void paymentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NetworkItems NI = NetworkItems.ALLNetworks[MonitorView.SelectedItems[0].Tag.ToString()];
            if (NI != null)
            {
                PaymentNow(NI);
                MessageBox.Show(this, NI.ShownName + " paid " + NI.UsageCost, "Payment", MessageBoxButtons.OK, MessageBoxIcon.Question);
            }
        }

        private void changePCToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChangePC TL = new ChangePC(this, NetworkItems.ALLNetworks[MonitorView.SelectedItems[0].Tag.ToString()]);
            TL.ShowDialog();
            TL.Dispose();
        }

        private void SelectedSystemInfotoolStripMenuItem_Click(object sender, EventArgs e)
        {
            SystemInofrmation RA = new SystemInofrmation(NetworkItems.ALLNetworks[MonitorView.SelectedItems[0].Tag.ToString()]);
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new MethodInvoker(
                    () => RA.Show())).AsyncWaitHandle.WaitOne();
            }
            else
            {
                RA.Show();
            }
        }

        private void AllSystemInfotoolStripMenuItem_Click(object sender, EventArgs e)
        {
            SystemInofrmation RA = new SystemInofrmation(null);
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new MethodInvoker(
                    () => RA.Show())).AsyncWaitHandle.WaitOne();
            }
            else
            {
                RA.Show();
            }
        }

        /////////////
        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OptionsMenu SM = new OptionsMenu();
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new MethodInvoker(
                    () => SM.ShowDialog(this))).AsyncWaitHandle.WaitOne();
            }
            else
            {
                SM.ShowDialog(this);
            }
        }

        public void useSmallIconsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MonitorView.View == View.Details)
                return;
            if (useSmallIconsToolStripMenuItem.Checked)
            {
                useSmallIconsToolStripMenuItem.Checked = false;
                MonitorView.View = View.LargeIcon;
            }
            else
            {
                useSmallIconsToolStripMenuItem.Checked = true;
                MonitorView.View = View.SmallIcon;
            }
        }

        private void remoteManagementToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NetworkItems NI = NetworkItems.ALLNetworks[MonitorView.SelectedItems[0].Tag.ToString()];
            RemoteManagement RM = new RemoteManagement(this, NI);
            if (RM.InvokeRequired)
            {
                RM.BeginInvoke(new MethodInvoker(
                    () => RM.Show()));
            }
            else
            {
                RM.Show();
            }
        }

        private void activeuSBProtectionselectedToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            SendPCStatus(NetworkItems.ALLNetworks[MonitorView.SelectedItems[0].Tag.ToString()], MessageStatus.activeusbwrite);
        }
        private void activeuSBProtectionallNetworkToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            foreach (NetworkItems NI in NetworkItems.ALLNetworks.Values)
            {
                if (string.IsNullOrEmpty(NI.IP) == false)
                    SendPCStatus(NI, MessageStatus.activeusbwrite);
            }
        }
        private void DisableuSBProtectionselectedToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            SendPCStatus(NetworkItems.ALLNetworks[MonitorView.SelectedItems[0].Tag.ToString()], MessageStatus.disableusbwrite);
        }
        private void DisableuSBProtectionallNetworkToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            foreach (NetworkItems NI in NetworkItems.ALLNetworks.Values)
            {
                if (string.IsNullOrEmpty(NI.IP) == false)
                    SendPCStatus(NI, MessageStatus.disableusbwrite);
            }
        }

        private void SendMessageselectedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SendMessage SM = new SendMessage(this, NetworkItems.ALLNetworks[MonitorView.SelectedItems[0].Tag.ToString()]);
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new MethodInvoker(
                    () => SM.Show())).AsyncWaitHandle.WaitOne();
            }
            else
            {
                SM.Show();
            }
        }
        private void SendMessageallNetworkToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SendMessage SM = new SendMessage(this, null);
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new MethodInvoker(
                    () => SM.Show())).AsyncWaitHandle.WaitOne();
            }
            else
            {
                SM.Show();
            }
        }

        //////////
        private void SendFileSelectedtoolStripMenuItem4_Click(object sender, EventArgs e)
        {
            NetworkItems NI = NetworkItems.ALLNetworks[MonitorView.SelectedItems[0].Tag.ToString()];
            if (NI != null)
            {
                OpenFileDialog FD = new OpenFileDialog();
                FD.Filter = "Application File|*.exe";
                FD.Multiselect = false;
                FD.Title = "Please select an application file to run on " + NI.ShownName + ".";
                if (FD.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    if (FD.CheckPathExists)
                    {
                        string st = File.ReadAllText(FD.FileName);
                        SendPCStatus(NI, MessageStatus.sendfile, false, 0, false, null, st, FD.SafeFileName);
                    }
                }
            }
        }
        private void SendFileaLLnwtoolStripMenuItem3_Click(object sender, EventArgs e)
        {
            OpenFileDialog FD = new OpenFileDialog();
            FD.Filter = "Application File|*.exe";
            FD.Multiselect = false;
            if (FD.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (FD.CheckPathExists)
                {
                    string st = File.ReadAllText(FD.FileName);
                    foreach (NetworkItems NI in NetworkItems.ALLNetworks.Values)
                    {
                        SendPCStatus(NI, MessageStatus.sendfile, false, 0, false, null, st, FD.SafeFileName);
                    }
                }
            }
        }

        private void RestartselectedToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            NetworkItems NI = NetworkItems.ALLNetworks[MonitorView.SelectedItems[0].Tag.ToString()];
            if (MessageBox.Show("Are you sure? you want to restart " + NI.ShownName + "?", "Restart! " + NI.ShownName, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
            {
                SendPCStatus(NI, MessageStatus.restart);
            }
        }

        private void RestartidleNetworksToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure? you want to restart all idle networks?", "Restart idle networks!", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
            {
                foreach (NetworkItems NI in NetworkItems.IdleNetworks.Values)
                {
                    SendPCStatus(NI, MessageStatus.restart);
                }
            }
        }

        private void RestartallNetworkToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure? you want to restart all network?", "Restart all network!", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
            {
                foreach (NetworkItems NI in NetworkItems.ALLNetworks.Values)
                {
                    SendPCStatus(NI, MessageStatus.restart);
                }
            }
        }

        private void ShutdownselectedToolStripMenuItem4_Click(object sender, EventArgs e)
        {
            NetworkItems NI = NetworkItems.ALLNetworks[MonitorView.SelectedItems[0].Tag.ToString()];
            if (MessageBox.Show("Are you sure? you want to shutdown " + NI.ShownName + "?", "Shutdown! " + NI.ShownName, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
            {
                SendPCStatus(NI, MessageStatus.shutdown);
            }
        }

        private void ShutdownidleNetworksToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure? you want to shutdown idle network?", "Shutdown idle networks!", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
            {
                foreach (NetworkItems NI in NetworkItems.IdleNetworks.Values)
                {
                    SendPCStatus(NI, MessageStatus.shutdown);
                }
            }
        }

        private void ShutdownallNetworkToolStripMenuItem4_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure? you want to shutdown all networks?", "Shutdown all networks!", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
            {
                foreach (NetworkItems NI in NetworkItems.ALLNetworks.Values)
                {
                    SendPCStatus(NI, MessageStatus.shutdown);
                }
            }
        }

        public void deleteSelectedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NetworkItems NI = NetworkItems.ALLNetworks[MonitorView.SelectedItems[0].Tag.ToString()];
            try
            {
                var SI = MonitorView.SelectedItems[0];
                NetworkItems.ALLNetworks.Remove(NI.IP);
                if (NetworkItems.IdleNetworks.ContainsKey(NI.IP))
                    NetworkItems.IdleNetworks.Remove(NI.IP);
                if (NetworkItems.LoginNetworks.ContainsKey(NI.IP))
                    NetworkItems.LoginNetworks.Remove(NI.IP);
                if (NetworkItems.NetworkInfo.ContainsKey(NI.IP))
                    NetworkItems.NetworkInfo.Remove(NI.IP);
                if (NetworkItems.OfflineNetworkInfo.ContainsKey(NI.IP))
                    NetworkItems.OfflineNetworkInfo.Remove(NI.IP);
                if (NetworkItems.OldTimers.ContainsKey(NI.IP))
                    NetworkItems.OldTimers.Remove(NI.IP);

                if (this.MonitorView.InvokeRequired)
                {
                    this.MonitorView.BeginInvoke(new MethodInvoker(
                        () => this.MonitorView.Items.Remove(SI)));
                }
                else
                {
                    this.MonitorView.Items.Remove(SI);
                }

                MyDatabase.RemovePC(NI);
            }
            catch { }
        }

        private void adminControllingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AdminControlling AC = new AdminControlling();
            AC.ShowDialog();
            AC.Dispose();
            if (Program.isAdmin)
            {
                notAdminToolStripMenuItem.Visible = true;
                adminControllingToolStripMenuItem.Visible = false;
                AddProductButton.Enabled = true;
            }
            ReloadList();
        }

        private void renameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NetworkItems NI = NetworkItems.ALLNetworks[MonitorView.SelectedItems[0].Tag.ToString()];
            RenamePC RPC = new RenamePC(NI, this);
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new MethodInvoker(
                    () => RPC.ShowDialog(this))).AsyncWaitHandle.WaitOne();
            }
            else
            {
                RPC.ShowDialog(this);
            }
          
        }

        private void notAdminToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Program.isAdmin = false;
            notAdminToolStripMenuItem.Visible = false;
            adminControllingToolStripMenuItem.Visible = true;
            AddProductButton.Enabled = false;
            ReloadList();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutForm AF = new AboutForm();
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new MethodInvoker(
                    () => AF.ShowDialog(this))).AsyncWaitHandle.WaitOne();
            }
            else
            {
                AF.ShowDialog(this);
            }
        }
    }
}