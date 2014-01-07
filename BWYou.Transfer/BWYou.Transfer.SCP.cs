using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tamir.SharpSsh;
using Tamir.Streams;
using System.IO;

namespace BWYou.Transfer
{
    /// <summary>
    /// SCP를 통한 파일 전송 클래스
    /// </summary>
    public class SCP
    {
        Scp scpTranser = null;

        private string strRemoteIP = "";
        private string strRemoteID = "";
        private string strRemotePW = "";
        private int nRemotePort = 22;

        /// <summary>
        /// 생성자, 기본 연결 값을 설정하고, 연결 객체 생성 후 이벤트를 등록한다.
        /// </summary>
        /// <param name="RemoteIP"></param>
        /// <param name="id"></param>
        /// <param name="pw"></param>
        /// <param name="port"></param>
        public SCP(string RemoteIP, string id, string pw, int port)
        {
            strRemoteIP = RemoteIP;
            strRemoteID = id;
            strRemotePW = pw;
            nRemotePort = port;

            scpTranser = new Scp(strRemoteIP, strRemoteID, strRemotePW, nRemotePort);
            //scpTranser.OnTransferStart += new FileTransferEvent(Scp_OnTransferStart);
            scpTranser.OnTransferProgress += new FileTransferEvent(Scp_OnTransferProgress);
            //scpTranser.OnTransferEnd += new FileTransferEvent(Scp_OnTransferEnd);
        }
        /// <summary>
        /// 이벤트 제거 후 객체를 제거
        /// </summary>
        ~SCP()
        {
            if (scpTranser != null)
            {
                //scpTranser.OnTransferStart -= new FileTransferEvent(Scp_OnTransferStart);
                scpTranser.OnTransferProgress -= new FileTransferEvent(Scp_OnTransferProgress);
                //scpTranser.OnTransferEnd -= new FileTransferEvent(Scp_OnTransferEnd);

                scpTranser = null; 
            }
        }

        /// <summary>
        /// 연결 상태 확인
        /// </summary>
        /// <returns></returns>
        public bool IsConnected()
        {
            if (scpTranser != null)
            {
                return scpTranser.Connected;
            }
            else
            {
                return false;   //객체 자체가 없어도 false로 리턴
            }
        }
        /// <summary>
        /// 기본 설정 값으로 연결. 연결 상태 유지
        /// </summary>
        public void Connect()
        {
            try
            {
                scpTranser.Connect();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 연결 종료
        /// </summary>
        public void Close()
        {
            try
            {
                if (scpTranser != null && scpTranser.Connected == true)
                {
                    scpTranser.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 로컬에 있는 파일을 리모트로 보내기
        /// </summary>
        /// <param name="strLocalPathFile"></param>
        /// <param name="strRemotePathFile"></param>
        public void Send(string strLocalPathFile, string strRemotePathFile)
        {
            try
            {
                Transfer(strLocalPathFile, strRemotePathFile, true);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 리모트의 파일을 로컬로 받아오기
        /// </summary>
        /// <param name="strRemotePathFile"></param>
        /// <param name="strLocalPathFile"></param>
        public void Receive(string strRemotePathFile, string strLocalPathFile)
        {
            try
            {
                Transfer(strLocalPathFile, strRemotePathFile, false);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 파일을 전송
        /// </summary>
        /// <param name="strLocalPathFile"></param>
        /// <param name="strRemotePathFile"></param>
        /// <param name="bIsSend">true : 보내기, false : 받기</param>
        private void Transfer(string strLocalPathFile, string strRemotePathFile, bool bIsSend)
        {
            bool bContinueConnect = false;

            try
            {
                if (scpTranser == null || scpTranser.Connected == false)
                {
                    bContinueConnect = false;
                    Connect();
                }
                else
                {
                    bContinueConnect = true;
                }

                if (bIsSend == true)
                {
                    scpTranser.To(strLocalPathFile, strRemotePathFile, true);
                }
                else
                {
                    scpTranser.From(strRemotePathFile, strLocalPathFile, true);
                }

                if (bContinueConnect == false)
                {
                    Close();
                }
            }
            catch (Exception ex)
            {
                if (bContinueConnect == false)
                {
                    Close();
                }
                throw ex;
            }
        }

        /// <summary>
        /// 리모트 디렉토리 만들기. 상위 디렉토리 생성 못함
        /// </summary>
        /// <param name="strRemotePath"></param>
        public void MkdirRemote(string strRemotePath)
        {
            //상위 없이도 한번에 만들어질까??

            bool bContinueConnect = false;

            try
            {
                if (scpTranser == null || scpTranser.Connected == false)
                {
                    bContinueConnect = false;
                    Connect();
                }
                else
                {
                    bContinueConnect = true;
                }

                scpTranser.Mkdir(strRemotePath);

                if (bContinueConnect == false)
                {
                    Close();
                }
            }
            catch (Exception ex)
            {
                if (bContinueConnect == false)
                {
                    Close();
                }
                throw ex;
            }
        }
        /// <summary>
        /// 리모트 디렉토리 만들기. 상위 디렉토리 자동 생성
        /// </summary>
        /// <param name="strRemotePath"></param>
        public void MkdirRemoteShell(string strRemotePath)
        {
            try
            {
                Tamir.SharpSsh.SshShell shell = new SshShell(strRemoteIP, strRemoteID, strRemotePW, nRemotePort);
                shell.Connect(nRemotePort);
                //SCPStream = new Scp(RemoteIP, id, pw, port);
                //SCPStream.Connect(port);

                while (!shell.ShellOpened)
                {
                    System.Threading.Thread.Sleep(100);
                }

                //shell.WriteLine("mkdir " + targetlocation + " -p");
                using (StreamWriter sw = new StreamWriter(shell.IO))
                {
                    using (StreamReader sr = new StreamReader(shell.IO))
                    {
                        sw.AutoFlush = true;
                        sw.WriteLine("cd /");
                        sw.WriteLine("mkdir " + strRemotePath + " -p");
                        sr.ReadLine();
                        sr.ReadLine();
                        System.Threading.Thread.Sleep(2000);
                    }
                    //sw.Close();
                }

                shell.RedirectToConsole();
                shell.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 로컬 디렉토리 만들기
        /// </summary>
        /// <param name="strLocalPath"></param>
        public void MkdirLocal(string strLocalPath)
        {
            try
            {
                //strLocalPath 폴더 없을 경우 만들기(폴더가 중간경로에도 없더라도 다 만들어줌)
                DirectoryInfo diDesc = new DirectoryInfo(strLocalPath);
                if (diDesc.Exists == false)
                {
                    Directory.CreateDirectory(strLocalPath);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region 이벤트

        /// <summary>
        /// Triggered on every interval with the transfer progress information.
        /// </summary>
        public event FileTransferEventY ftEvent;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="src"></param>
        /// <param name="dst"></param>
        /// <param name="transferredBytes"></param>
        /// <param name="totalBytes"></param>
        /// <param name="message"></param>
        public delegate void FileTransferEventY(string src, string dst, int transferredBytes, int totalBytes, string message);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="src"></param>
        /// <param name="dst"></param>
        /// <param name="transferredBytes"></param>
        /// <param name="totalBytes"></param>
        /// <param name="msg"></param>
        protected void OnTransferProgressEvent(string src, string dst, int transferredBytes, int totalBytes, string msg)
        {
            if (ftEvent != null)
            {
                ftEvent(src, dst, transferredBytes, totalBytes, msg);
            }
        }

        private void Scp_OnTransferStart(string src, string dst, int transferredBytes, int totalBytes, string message)
        {
            OnTransferProgressEvent(src, dst, transferredBytes, totalBytes, message);
        }
        private void Scp_OnTransferProgress(string src, string dst, int transferredBytes, int totalBytes, string message)
        {
            OnTransferProgressEvent(src, dst, transferredBytes, totalBytes, message);
        }
        private void Scp_OnTransferEnd(string src, string dst, int transferredBytes, int totalBytes, string message)
        {
            OnTransferProgressEvent(src, dst, transferredBytes, totalBytes, message);
        }

        #endregion
    }
}
