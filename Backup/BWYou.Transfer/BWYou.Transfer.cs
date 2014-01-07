/** \page BWYou.Transfer BWYou.Transfer
 *  \section developer 개발자
 *          - You
 *  \section info 개발목적
 *          - 전송 관련 클래스 모음
 *  \section advenced 추가정보
 *          - Version : 0.1.1.0
 *          - Last Updated : 2012.08.29
 *              -# 대상 프레임워크 닷넷 3.5로 변경
 *          - Error 발생시 : throw 발생.
 *          - 구현 사항
 *              -# SCP를 통한 SEND, RECEIVE
 *          - 미구현 사항
 *              -# SFTP, FTP
 * 
 *  \section explain 설명
 *          - 전송 관련 클래스
 * 
 *  \section Transfer Transfer
 *          - 전송 관련 클래스
 * 
 *          - 사용예)
 *  \code
            
            try
            {
                BWYou.Transfer.Transfer.SCP scp = new BWYou.Transfer.Transfer.SCP("xxx.xxx.xxx.xxx", "id", "pass", 22); //연결에 필요한 값을 넣어 객체 생성
                scp.ftEvent += new BWYou.Transfer.Transfer.SCP.FileTransferEventY(이벤트처리함수);
                scp.Connect();                                              //연결된 상태로 유지(안하면 매번 명령 처리할 때마다 연결하므로 좀 느려짐
                scp.MkdirLocal(@"c:\test\");
                scp.Receive(@"/usr/local/test.zip", @"c:\test\test.zip");   //받기
                scp.MkdirRemote(@"/home/test/");
                scp.Send(@"D:\test\xxx.zip", @"/home/test/xxx.zip");        //보내기
                scp.Close();                                                //연결된 상태 종료
                scp.ftEvent -= new BWYou.Transfer.Transfer.SCP.FileTransferEventY(이벤트처리함수);
            }
            catch (Exception ex)
            {
                //에러 발생시 처리 방법
            }


 * 
 *  \endcode
 * 
 *  \section 주의사항 파일 전송 주의 사항
 *      - SCP를 통해 리눅스로 파일을 전송할 경우에는 패스가 아닌 패스+파일네임까지 넣어야 전송이 된다.
 * 
 */

using System;
using System.Collections.Generic;
using System.Text;
using Tamir.SharpSsh;
using Tamir.Streams;
using System.IO;

namespace BWYou
{
    /// <summary>
    /// 전송 관련 클래스
    /// </summary>
    public class Transfer
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
                //scpTranser.OnTransferStart -= new FileTransferEvent(Scp_OnTransferStart);
                scpTranser.OnTransferProgress -= new FileTransferEvent(Scp_OnTransferProgress);
                //scpTranser.OnTransferEnd -= new FileTransferEvent(Scp_OnTransferEnd);

                scpTranser = null;
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
            /// 리모트 디렉토리 만들기
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
}
