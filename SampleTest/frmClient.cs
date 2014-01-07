using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace SampleTest
{
    public partial class frmClient : Form
    {
        public class StateObject
        {
            public Socket workSocket = null;
            public const int BufferSize = 256;
            public byte[] buffer = new byte[BufferSize];
            public StringBuilder sb = new StringBuilder();
        }

        public class AsynchronousClient
        {
            private const int port = 11000;

            private static ManualResetEvent connectDone = new ManualResetEvent(false);
            private static ManualResetEvent sendDone = new ManualResetEvent(false);
            private static ManualResetEvent receiveDone = new ManualResetEvent(false);

            private static String response = String.Empty;

            public static void StartClient()
            {
                try
                {
                    IPHostEntry ipHostInfo = Dns.Resolve(Dns.GetHostName());
                    IPAddress ipAddress = ipHostInfo.AddressList[0];    //접속할 IP
                    IPEndPoint remoteEP = new IPEndPoint(ipAddress, port);

                    Socket client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                    client.BeginConnect(remoteEP, new AsyncCallback(ConnectCallback), client);
                    connectDone.WaitOne();

                    Send(client, "This is a test<EOF>");
                    sendDone.WaitOne();

                    Receive(client);
                    receiveDone.WaitOne();

                    Console.WriteLine("Response received : {0}", response);

                    client.Shutdown(SocketShutdown.Both);
                    client.Close();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
            }

            private static void ConnectCallback(IAsyncResult ar)
            {
                try
                {
                    Socket client = (Socket)ar.AsyncState;

                    client.EndConnect(ar);

                    Console.WriteLine("Socket connected to {0}", client.RemoteEndPoint.ToString());

                    connectDone.Set();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
            }

            private static void Receive(Socket client)
            {
                try
                {
                    StateObject state = new StateObject();
                    state.workSocket = client;

                    client.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReceiveCallback), state);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
            }

            private static void ReceiveCallback(IAsyncResult ar)
            {
                try
                {
                    StateObject state = (StateObject)ar.AsyncState;
                    Socket client = state.workSocket;

                    int bytesRead = client.EndReceive(ar);

                    if (bytesRead > 0)
                    {
                        state.sb.Append(Encoding.ASCII.GetString(state.buffer, 0, bytesRead));

                        client.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReceiveCallback), state);
                    }
                    else
                    {
                        if (state.sb.Length > 1)
                        {
                            response = state.sb.ToString();
                        }
                        receiveDone.Set();
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
            }

            private static void Send(Socket client, String data)
            {
                byte[] byteData = Encoding.ASCII.GetBytes(data);

                client.BeginSend(byteData, 0, byteData.Length, 0, new AsyncCallback(SendCallback), client);
            }

            private static void SendCallback(IAsyncResult ar)
            {
                try
                {
                    Socket client = (Socket)ar.AsyncState;

                    int bytesSent = client.EndSend(ar);
                    Console.WriteLine("Sent {0} bytes to server", bytesSent);

                    sendDone.Set();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
            }



        }








        BWYou.Control.TimerShow ts = new BWYou.Control.TimerShow();

        private Socket ClientSocket = null;

        Thread thr;

        public frmClient()
        {
            InitializeComponent();
        }

        private void frmClient_Load(object sender, EventArgs e)
        {

        }

        private void btnTestStart_Click(object sender, EventArgs e)
        {
            thr = new Thread(new ThreadStart(StartClient));
            thr.Start();

        }


        private void btnTestEnd_Click(object sender, EventArgs e)
        {

        }

        private void btnSendStart_Click(object sender, EventArgs e)
        {

        }

        public void StartClient()
        {
            try
            {
                AsynchronousClient.StartClient();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }












    }
}
