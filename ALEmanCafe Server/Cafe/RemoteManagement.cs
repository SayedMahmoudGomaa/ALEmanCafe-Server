using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net.Sockets;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Threading;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Drawing.Imaging;
using System.Collections;

namespace ALEmanCafe_Server
{
    public partial class RemoteManagement : Form
    {
        public ALEmanCafeServer ACS = null;
        public NetworkItems NI;
        public System.Timers.Timer ReciverTimer, SenderTimer;
        public int RPort = 0;
        public int SPort = 0;
        private Bitmap _buffer;
        public RemoteManagement(ALEmanCafeServer ACS, NetworkItems NI)
        {
            InitializeComponent();
            this.ACS = ACS;
            this.NI = NI;
            this.NI.RM = this;
            this.RPort = Program.PPort;
            Program.PPort++;
            this.SPort = Program.PPort;
            Program.PPort++;
          //  this.ACS.writeamess("IP : " + NI.IP);
            this.ACS.SendPCStatus(NI, ALEmanCafeServer.MessageStatus.startremote, false, (uint)this.RPort, false, null, null, null, this.SPort);
            this.FormClosing += new FormClosingEventHandler(RemoteManagement_FormClosing);
            pictureBox1.Paint += PictureBox1Paint;

            ReciverTimer = new System.Timers.Timer(20);
            ReciverTimer.AutoReset = true;
            ReciverTimer.Elapsed += new System.Timers.ElapsedEventHandler(RecieveInfos);
            /*
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new MethodInvoker(
                    () => ReciverTimer.Start()));
            }
            else
            {
                ReciverTimer.Start();
            }
            */
            SenderTimer = new System.Timers.Timer(50);
            SenderTimer.AutoReset = true;
            SenderTimer.Elapsed += new System.Timers.ElapsedEventHandler(SenderInfos);
            
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new MethodInvoker(
                    () =>
                    {
                        ReciverTimer.Start();
                        SenderTimer.Start();
                        this.MouseDown += new MouseEventHandler(RemoteManagement_MouseClick);
                        this.pictureBox1.MouseDown += new MouseEventHandler(RemoteManagement_MouseClick);
                        this.panel1.MouseDown += new MouseEventHandler(RemoteManagement_MouseClick);
                        this.KeyDown += new KeyEventHandler(RemoteManagement_KeyDown);
                    }));
            }
            else
            {
                ReciverTimer.Start();
                SenderTimer.Start();
                this.MouseDown += new MouseEventHandler(RemoteManagement_MouseClick);
                this.pictureBox1.MouseDown += new MouseEventHandler(RemoteManagement_MouseClick);
                this.panel1.MouseDown += new MouseEventHandler(RemoteManagement_MouseClick);
                this.KeyDown += new KeyEventHandler(RemoteManagement_KeyDown);
            }
            /*
            this.MouseDown += new MouseEventHandler(RemoteManagement_MouseClick);
            this.pictureBox1.MouseDown += new MouseEventHandler(RemoteManagement_MouseClick);
            this.panel1.MouseDown += new MouseEventHandler(RemoteManagement_MouseClick);

            this.KeyDown += new KeyEventHandler(RemoteManagement_KeyDown);
            */
        }

        public void RemoteManagement_MouseClick(object sender, MouseEventArgs eeee)
        {
            SendStatus(null, eeee);
            /*
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new MethodInvoker(
                    () => SendStatus(null, eeee)));//.AsyncWaitHandle.WaitOne();
            }
            else
            {
                SendStatus(null, eeee);
            }*/
        }
        public void RemoteManagement_KeyDown(object sender, KeyEventArgs eeee)
        {
            SendStatus(eeee);
          /*
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new MethodInvoker(
                    () => SendStatus(eeee)));//.AsyncWaitHandle.WaitOne();
            }
            else
            {
                SendStatus(eeee);
            }*/
        }


        public bool inSenderInfos = false;
        public void SenderInfos(object sender, EventArgs eeee)
        {
            if (inSenderInfos) return; inSenderInfos = true;
          //  SendStatus();
            if (this.pictureBox1.InvokeRequired)
            {
                this.pictureBox1.BeginInvoke(new MethodInvoker(
                    () => SendStatus()));
            }
            else
            {
                SendStatus();
            }
            inSenderInfos = false;
        }

        public TcpClient Tclient;
        public NetworkStream nwStream;
        public void SendStatus(KeyEventArgs K = null, MouseEventArgs M = null)//2.25  Sala7 100 - 20G
        {
            if (Stopped) return;
            try
            {
            //    this.ACS.writeamess("Sent");

                string MyMessage = "";
                try
                {
                    if (this.InvokeRequired)
                    {
                        this.BeginInvoke(new MethodInvoker(
                            () =>
                            {
                                if (this.pictureBox1.ClientRectangle.Contains(this.pictureBox1.PointToClient(Cursor.Position)) && this.WindowState != FormWindowState.Minimized && Form.ActiveForm == this)
                                {
                                    var location = this.pictureBox1.PointToClient(Cursor.Position);//this
                                    MyMessage = location.X + ";" + location.Y + ";";
                                }
                                else if (K != null || M != null)
                                    MyMessage += "0;0;";
                                else
                                    return;
                            }));
                    }
                    else
                    {
                        if (this.pictureBox1.ClientRectangle.Contains(this.pictureBox1.PointToClient(Cursor.Position)) && this.WindowState != FormWindowState.Minimized && Form.ActiveForm == this)
                        {
                            var location = this.pictureBox1.PointToClient(Cursor.Position);
                            MyMessage = location.X + ";" + location.Y + ";";
                        }
                        else if (K != null || M != null)
                            MyMessage += "0;0;";
                        else
                            return;
                    }
                  
                }
                catch { ACS.writeamess("MyMessageError" + Environment.NewLine); return; }
                if (K != null)
                {
                    MyMessage += "Keyboard;" + K.KeyCode.ToString() + ";" + K.Control.ToString() + ";" + K.Shift.ToString() + ";" + K.Alt.ToString() + ";" + K.SuppressKeyPress.ToString();
                }
                else if (M != null)
                {
                    MyMessage += "mouse;" + M.Clicks + ";" + M.Button.ToString();
                }

                //---data to send to the server---

                if (Stopped) return;
                //---create a TCPClient object at the IP and port no.---
                try
                {
                    Tclient = new TcpClient(NI.IP, this.SPort);//Error when client closed
                }
                catch { RemoteManagement_FormClosing(null, null); return; }
                nwStream = Tclient.GetStream();
                byte[] bytesToSend = ASCIIEncoding.ASCII.GetBytes(MyMessage);

                //---send the text---
          //      ACS.writeamess("Sending : " + MyMessage);
                nwStream.Write(bytesToSend, 0, bytesToSend.Length);

                try
                {
                    Tclient.Client.Close();
                    Tclient.Close();
                }catch { }
                nwStream.Close();
                nwStream.Dispose();
                /*
                try
                {
                    var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    try
                    {
                        var encoding = Encoding.UTF8;
                        socket.Connect(new IPEndPoint(ip, this.SPort));//Connectio Closed
                        socket.Send(BitConverter.GetBytes(encoding.GetByteCount(MyMessage)));
                      //  socket.Send(encoding.GetBytes(MyMessage));
                     //   socket.Shutdown(SocketShutdown.Both);
                       // socket.Close();
                    }
                    catch//(Exception ee)
                    {
                        try
                        {
                            socket.Close();
                      //      socket.Shutdown(SocketShutdown.Both);
                        }catch { }
                    //    ACS.writeamess("Error: " + ee.ToString());
                    }
                }
                catch { SendStatus(K, M); }
                */
            }
            catch(Exception ee) { ACS.writeamess(ee.ToString() + Environment.NewLine); SendStatus(K, M); }
        }

        public bool Stopped = false;


        public void RemoteManagement_FormClosing(object sender, FormClosingEventArgs eeee)
        {
            try
            {
                Stopped = true;
                if (this.InvokeRequired)
                {
                    this.BeginInvoke(new MethodInvoker(
                        () => this.Text += " Closed"));//.AsyncWaitHandle.WaitOne();
                }
                else
                {
                    this.Text += " Closed";
                }

                this.ACS.SendPCStatus(NI, ALEmanCafeServer.MessageStatus.endremote);

                if (Tclient != null)
                {
                    Tclient.Client.Close();
                    Tclient.Close();
                }

                if (nwStream != null)
                {
                    nwStream.Close();
                    nwStream.Dispose();
                }
                if (stream != null)
                {
                    stream.Close();
                    stream.Dispose();
                }
                if (socket != null)
                    socket.Close();

                this.MouseDown -= new MouseEventHandler(RemoteManagement_MouseClick);
                this.pictureBox1.MouseDown -= new MouseEventHandler(RemoteManagement_MouseClick);
                this.panel1.MouseDown -= new MouseEventHandler(RemoteManagement_MouseClick);
                this.KeyDown -= new KeyEventHandler(RemoteManagement_KeyDown);


                try
                {
                    this.ReciverTimer.Stop();
                    this.ReciverTimer.Dispose();
                }
                catch { }

                try
                {
                    this.SenderTimer.Stop();
                    this.SenderTimer.Dispose();
                    this.NI.RM = null;
                }
                catch { }
            }
            catch { }
        }



        public bool inRecieveInfos = false;
        public void RecieveInfos(object sender, EventArgs eeee)
        {
            if (inRecieveInfos) return; inRecieveInfos = true;

            try
            {
                GetSnapshots();
                // Application.DoEvents();
                /*
                 if (this.InvokeRequired)
                 {
                     this.BeginInvoke(new MethodInvoker(
                         () => GetSnapshots()));
                 }
                 else
                 {
                     GetSnapshots();
                 }
                 */
                // Application.DoEvents();
            }
            catch
            {
          
                this.inRecieveInfos = false;
            }
            inRecieveInfos = false;
        }

        public Socket socket;
        public MemoryStream stream;
        private void GetSnapshots()
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Connect(new IPEndPoint(IPAddress.Parse(NI.IP), this.RPort));//IPAddress.Loopback
            while (true)
            {
                try
                {
                    if (!NI.Connected || Stopped)
                    {
                        socket.Close();
                        return;
                    }
                    
                    byte[] lengthData = new byte[4];
                    int lengthBytesRead = 0;
                    while (lengthBytesRead < lengthData.Length)
                    {
                        var read = socket.Receive(lengthData, lengthBytesRead, lengthData.Length - lengthBytesRead, SocketFlags.None);
                        if (read == 0) return;
                        lengthBytesRead += read;
                        System.Threading.Thread.Sleep(1);
                    }
                    if (!NI.Connected || Stopped)
                    {
                        socket.Close();
                        return;
                    }
                    var length = BitConverter.ToInt32(lengthData, 0);
                    var imageData = new byte[length];
                    var imageBytesRead = 0;
                    while (imageBytesRead < imageData.Length)
                    {
                        var read = socket.Receive(imageData, imageBytesRead, imageData.Length - imageBytesRead, SocketFlags.None);
                        if (read == 0) return;
                        imageBytesRead += read;
                        System.Threading.Thread.Sleep(1);
                    }
                    if (!NI.Connected || Stopped)
                    {
                        socket.Close();
                        return;
                    }
                    using (stream = new MemoryStream(imageData))
                    {
                        var bitmap = new Bitmap(stream);
                        if (!NI.Connected || Stopped)
                        {
                            stream.Close();
                            stream.Dispose();
                            socket.Close();
                            return;
                        }
                        try
                        {
                            Invoke(new ImageCompleteDelegate(ImageComplete), new object[] { bitmap });//Error after exit
                        }catch { }
                    }
                }
                catch// (Exception ee)
                {
                    //  this.ACS.writeamess("inRecieveInfos: " + ee.ToString());
                    try
                    {
                        //   socket.Shutdown(SocketShutdown.Both);
                        socket.Close();

                    }
                    catch (Exception ee2) { this.ACS.writeamess("inRecieveInfos: " + ee2.ToString()); }
                    inRecieveInfos = false;
                    break;
                }
                System.Threading.Thread.Sleep(1);
            }
        }
        private void PictureBox1Paint(object sender, PaintEventArgs e)
        {
            if (_buffer == null) return;
            e.Graphics.DrawImage(_buffer, 0, 0);
        }
        private delegate void ImageCompleteDelegate(Bitmap bitmap);
        private void ImageComplete(Bitmap bitmap)
        {
            if (_buffer != null)
            {
                _buffer.Dispose();
            }
            _buffer = new Bitmap(bitmap);
            /*
            Application.DoEvents();
            pictureBox1.Size = _buffer.Size;
            pictureBox1.Invalidate();
            */

          //  Application.DoEvents();
            if (this.pictureBox1.InvokeRequired)
            {
                this.pictureBox1.BeginInvoke(new MethodInvoker(
                    () =>
                    {
                        pictureBox1.Size = _buffer.Size;
                        pictureBox1.Invalidate();
                    }));
            }
            else
            {
                pictureBox1.Size = _buffer.Size;
                pictureBox1.Invalidate();
            }
         //   Application.DoEvents();
        }
    }
}