using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;

namespace BWYou.Transfer
{
    /// <summary>
    /// FTP 송수신 클래스
    /// </summary>
    public class FTP
    {
        /// <summary>
        /// FTP 다운로드 총 용량 체크용 이벤트 핸들러
        /// </summary>
        /// <param name="totalSize"></param>
        public delegate void FTPDownloadTotalSizeHandle(long totalSize);
        /// <summary>
        /// FTP 다운로드 받은 용량 체크용 이벤트 핸들러
        /// </summary>
        /// <param name="RcvSize"></param>
        public delegate void FTPDownloadReceivedSizeHandle(int RcvSize);

        /// <summary>
        /// FTP 다운로드 총 용량 체크용 이벤트
        /// </summary>
        public event FTPDownloadTotalSizeHandle ftpDNTotalSizeEvt;
        /// <summary>
        /// FTP 다운로드 받은 용량 체크용 이벤트
        /// </summary>
        public event FTPDownloadReceivedSizeHandle ftpDNRcvSizeEvt;

        string ftpServerAddress = String.Empty;
        string ftpUserID = String.Empty;
        string ftpPassword = String.Empty;
        bool usePassive = false;


        /// <summary>
        /// 생성자에 인자로 필요한 값들을 넣어준다.
        /// </summary>
        /// <param name="ip">FTP 서버주소</param>
        /// <param name="id">아이디</param>
        /// <param name="pw">패스워드</param>
        /// <param name="port">포트</param>
        /// <param name="usePassive">패시브 모드 여부</param>
        public FTP(string ip, string id, string pw, string port, bool usePassive)
        {
            ftpServerAddress = ip + ":" + port;
            ftpUserID = id;     //아이디
            ftpPassword = pw;  //패스워드
            this.usePassive = usePassive;   //패시브모드 사용여부
        }

        /// <summary>
        /// Method to upload the specified file to the specified FTP Server
        /// </summary>
        /// <param name="folder">파일이 위치 할 폴더. 루트 일 경우 . 으로 표시 필요. 빈칸 쓰면 안 됨.</param>
        /// <param name="filename">file full name to be uploaded</param>
        public void Upload(string folder, string filename)
        {
            try
            {

                FileInfo fileInf = new FileInfo(filename);

                //FtpWebRequest reqFTP = GetFtpWebRequest(new Uri("ftp://" + ftpServerAddress + "/" + fileInf.Name), WebRequestMethods.Ftp.UploadFile);
                FtpWebRequest reqFTP = GetFtpWebRequest(new Uri("ftp://" + ftpServerAddress + "/" + folder + "/" + fileInf.Name), WebRequestMethods.Ftp.UploadFile);

                // Notify the server about the size of the uploaded file
                reqFTP.ContentLength = fileInf.Length;

                // The buffer size is set to 2kb
                int buffLength = 2048;
                byte[] buff = new byte[buffLength];
                int contentLen;


                // Opens a file stream (System.IO.FileStream) to read the file to be uploaded

                using (FileStream fs = fileInf.OpenRead())
                {
                    // Stream to which the file to be upload is written
                    using (Stream strm = reqFTP.GetRequestStream())
                    {
                        // Read from the file stream 2kb at a time
                        contentLen = fs.Read(buff, 0, buffLength);

                        // Till Stream content ends
                        while (contentLen != 0)
                        {
                            // Write Content from the file stream to the FTP Upload Stream
                            strm.Write(buff, 0, contentLen);
                            contentLen = fs.Read(buff, 0, buffLength);
                        }

                        // Close the file stream and the Request Stream
                        //strm.Close(); 
                    }
                    //fs.Close(); 
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// 파일 삭제
        /// </summary>
        /// <param name="fileName"></param>
        public void DeleteFTP(string fileName)
        {
            try
            {
                FtpWebRequest reqFTP = GetFtpWebRequest(new Uri("ftp://" + ftpServerAddress + "/" + fileName), WebRequestMethods.Ftp.DeleteFile);

                string result = String.Empty;
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                long size = response.ContentLength;
                Stream datastream = response.GetResponseStream();
                StreamReader sr = new StreamReader(datastream);
                result = sr.ReadToEnd();
                sr.Close();
                datastream.Close();
                response.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 마지막 수정 일시 정보 얻기
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="dt"></param>
        /// <returns></returns>
        public bool GetFileLastModifiedInfo(string filename, ref DateTime dt)
        {
            try
            {
                FtpWebRequest reqFTP = GetFtpWebRequest(new Uri("ftp://" + ftpServerAddress + "/" + filename), WebRequestMethods.Ftp.GetDateTimestamp);
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                dt = response.LastModified;

                response.Close();
                return true;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 디렉토리 내의 상세 파일 리스트 얻기
        /// </summary>
        /// <param name="subFolder"></param>
        /// <returns></returns>
        public List<string> GetFilesDetailList(string subFolder)
        {
            List<string> files = new List<string>();
            string line = null;


            try
            {
                //StringBuilder result = new StringBuilder();

                FtpWebRequest reqFTP;
                reqFTP = GetFtpWebRequest(new Uri("ftp://" + ftpServerAddress + "/" + subFolder), WebRequestMethods.Ftp.ListDirectoryDetails);

                WebResponse response = reqFTP.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream(), System.Text.Encoding.Default);



                while ((line = reader.ReadLine()) != null)
                {
                    files.Add(line);
                }

                reader.Close();
                response.Close();
                return files;
                //MessageBox.Show(result.ToString().Split('\n'));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 디렉토리 내의 파일 리스트 얻기
        /// </summary>
        /// <param name="subFolder"></param>
        /// <returns></returns>
        public string[] GetFileList(string subFolder)
        {
            try
            {
                FtpWebRequest reqFTP = GetFtpWebRequest(new Uri("ftp://" + ftpServerAddress + "/" + subFolder), WebRequestMethods.Ftp.ListDirectory);
                WebResponse response = reqFTP.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream());
                //MessageBox.Show(reader.ReadToEnd());
                string line = reader.ReadLine();

                StringBuilder result = new StringBuilder();

                while (line != null)
                {
                    result.Append(line);
                    result.Append("\n");
                    line = reader.ReadLine();
                }
                result.Remove(result.ToString().LastIndexOf('\n'), 1);
                reader.Close();
                response.Close();
                //MessageBox.Show(response.StatusDescription);
                return result.ToString().Split('\n');
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 파일이 존재 해야 할 디렉토리가 존재 하지 않으면 디렉토리 생성.
        /// </summary>
        /// <param name="localFullPathFile"></param>
        /// <returns></returns>
        public bool checkDir(string localFullPathFile)
        {
            FileInfo fInfo = new FileInfo(localFullPathFile);

            if (!fInfo.Exists)
            {
                DirectoryInfo dInfo = new DirectoryInfo(fInfo.DirectoryName);
                if (!dInfo.Exists)
                {
                    dInfo.Create();
                }
                //dInfo.Delete();
            }

            //fInfo.Delete();
            return true;

        }

        /// <summary>
        /// 파일 다운 로드
        /// </summary>
        /// <param name="localFullPathFile">다운 로드 받을 위치(패스+파일명)</param>
        /// <param name="serverFullPathFile">다운 로드 파일의 소스 위치(패스+파일명)</param>
        public void Download(string localFullPathFile, string serverFullPathFile)
        {
            try
            {
                //filePath = <<The full path where the file is to be created.>>, 
                //fileName = <<Name of the file to be created(Need not be the name of the file on FTP server).>>
                checkDir(localFullPathFile);
                using (FileStream outputStream = new FileStream(localFullPathFile, FileMode.Create))
                {
                    FtpWebRequest reqFTP = GetFtpWebRequest(new Uri("ftp://" + ftpServerAddress + "/" + serverFullPathFile), WebRequestMethods.Ftp.DownloadFile);
                    using (FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse())
                    {
                        using (Stream ftpStream = response.GetResponseStream())
                        {
                            long cl = response.ContentLength;

                            if (ftpDNTotalSizeEvt != null) ftpDNTotalSizeEvt(cl);

                            int bufferSize = 2048;
                            int readCount;
                            byte[] buffer = new byte[bufferSize];

                            readCount = ftpStream.Read(buffer, 0, bufferSize);
                            while (readCount > 0)
                            {
                                if (ftpDNRcvSizeEvt != null) ftpDNRcvSizeEvt(readCount);

                                outputStream.Write(buffer, 0, readCount);
                                readCount = ftpStream.Read(buffer, 0, bufferSize);
                            }

                            //ftpStream.Close(); 
                        }
                        //response.Close(); 
                    }
                    //outputStream.Close(); 
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private long GetFileSize(string filename)
        {
            FtpWebRequest reqFTP;
            long fileSize = 0;
            try
            {
                reqFTP = GetFtpWebRequest(new Uri("ftp://" + ftpServerAddress + "/" + filename), WebRequestMethods.Ftp.GetFileSize);
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                Stream ftpStream = response.GetResponseStream();

                fileSize = response.ContentLength;

                ftpStream.Close();
                response.Close();

                return fileSize;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 이름 변경
        /// </summary>
        /// <param name="currentFilename"></param>
        /// <param name="newFilename"></param>
        public void Rename(string currentFilename, string newFilename)
        {
            FtpWebRequest reqFTP;
            try
            {
                reqFTP = GetFtpWebRequest(new Uri("ftp://" + ftpServerAddress + "/" + currentFilename), WebRequestMethods.Ftp.Rename);
                reqFTP.RenameTo = newFilename;
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                Stream ftpStream = response.GetResponseStream();

                ftpStream.Close();
                response.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 디렉토리 생성
        /// </summary>
        /// <param name="dirName"></param>
        public void MakeDir(string dirName)
        {
            FtpWebRequest reqFTP;
            try
            {
                // dirName = name of the directory to create.
                reqFTP = GetFtpWebRequest(new Uri("ftp://" + ftpServerAddress + "/" + dirName), WebRequestMethods.Ftp.MakeDirectory);
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                Stream ftpStream = response.GetResponseStream();

                ftpStream.Close();
                response.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// FtpWebRequest 받아서 사용하기
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="ftpWebRequestMethod"></param>
        /// <returns></returns>
        private FtpWebRequest GetFtpWebRequest(Uri uri, string ftpWebRequestMethod)
        {
            FtpWebRequest reqFTP;
            try
            {
                // dirName = name of the directory to create.
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(uri);
                reqFTP.Method = ftpWebRequestMethod;
                reqFTP.UseBinary = true;
                reqFTP.UsePassive = usePassive;
                reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);

                // By default KeepAlive is true, where the control connection is not closed
                // after a command is executed.
                //reqFTP.KeepAlive = false;

                return reqFTP;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }
}
