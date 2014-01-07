﻿using System;
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
using System.Reflection;

namespace SampleTest
{
    public partial class frmServer : Form
    {
        public class StateObject
        {
            public Socket workSocket = null;
            public const int BufferSize = 1024;
            public byte[] buffer = new byte[BufferSize];
            public StringBuilder sb = new StringBuilder();
        }

        public class AsynchronousSocketListener
        {
            public static ManualResetEvent allDone = new ManualResetEvent(false);
            public AsynchronousSocketListener()
            {

            }

            public static void StartListening()
            {
                byte[] bytes = new byte[1024];

                IPHostEntry ipHostInfo = Dns.Resolve(Dns.GetHostName());
                IPAddress ipAddress = ipHostInfo.AddressList[0];
                IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 11000);

                Socket listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                try
                {
                    listener.Bind(localEndPoint);
                    listener.Listen(100);

                    while (true)
                    {
                        allDone.Reset();

                        Console.WriteLine("Waiting for a connection...");
                        listener.BeginAccept(new AsyncCallback(AcceptCallback), listener);

                        allDone.WaitOne();
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }

                Console.WriteLine("\nPress ENTER to continue...");
                Console.Read();
            }

            public static void AcceptCallback(IAsyncResult ar)
            {
                allDone.Set();

                Socket listener = (Socket)ar.AsyncState;
                Socket handler = listener.EndAccept(ar);

                StateObject state = new StateObject();
                state.workSocket = handler;
                handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReadCallback), state);

            }

            public static void ReadCallback(IAsyncResult ar)
            {
                String content = String.Empty;

                StateObject state = (StateObject)ar.AsyncState;
                Socket handler = state.workSocket;

                int bytesRead = handler.EndReceive(ar);

                if (bytesRead > 0)
                {
                    state.sb.Append(Encoding.ASCII.GetString(state.buffer, 0, bytesRead));

                    content = state.sb.ToString();
                    if (content.IndexOf("<EOF>") > -1)
                    {
                        Console.WriteLine("Read {0} bytes from socket. \n Data : {1}", content.Length, content);
                        Send(handler, content);
                    }
                    else
                    {
                        handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReadCallback), state);
                    }
                }
            }

            private static void Send(Socket handler, String data)
            {
                byte[] byteData = Encoding.ASCII.GetBytes(data);

                handler.BeginSend(byteData, 0, byteData.Length, 0, new AsyncCallback(SendCallback), handler);
            }

            private static void SendCallback(IAsyncResult ar)
            {
                try
                {
                    Socket handler = (Socket)ar.AsyncState;

                    int bytesSent = handler.EndSend(ar);
                    Console.WriteLine("Send {0} bytes to client.", bytesSent);

                    handler.Shutdown(SocketShutdown.Both);
                    handler.Close();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
            }

        }



        BWYou.Control.TimerShow ts = new BWYou.Control.TimerShow();


        Thread thr;

        public frmServer()
        {
            string str1 = @"lumbosacral spondylosis

Lt. symphysis pubis old fx.";

            string str2 = @"lumbosacral spondylosis

Lt. sup. pubic ramus old fx.";

            DiffMatchPatch.diff_match_patch dmp = new DiffMatchPatch.diff_match_patch();

            var d1 = dmp.diff_main(str2, str1);

            dmp.diff_cleanupSemantic(d1);

            dmp.diff_cleanupEfficiency(d1);

            InitializeComponent();
        }

        private void frmServer_Load(object sender, EventArgs e)
        {

        }

        public void StartServer()
        {
            try
            {
                AsynchronousSocketListener.StartListening();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        private void frmServer_FormClosing(object sender, FormClosingEventArgs e)
        {

        }
        


        private void btnTestStart_Click(object sender, EventArgs e)
        {
            thr = new Thread(new ThreadStart(StartServer));
            thr.Start();
        }

        private void btnTestEnd_Click(object sender, EventArgs e)
        {

        }

    }
}
