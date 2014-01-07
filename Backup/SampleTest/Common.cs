using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using log4net;
using log4net.Config;
using BWYou.Base;

namespace SampleTest
{
    class Common : ClassBase
    {
        Log4net cLog = new Log4net();

        public Common(string Name)
            : base(Name)
        {

        }


        public bool CreateDirectory(string strLocalPath)
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

            return true;
        }


        /// <summary>
        /// 해당 드라이브의 남아 있는 용량을 퍼센트값으로 나타낸다.
        /// </summary>
        /// <param name="strDriveName"></param>
        /// <returns></returns>
        public double ReadFreeSpacePercentDriveInfo(string strDriveName)
        {
            DriveInfo drvInfo = new DriveInfo(strDriveName);

            double nRet = drvInfo.TotalFreeSpace * 100.0 / drvInfo.TotalSize;

            return nRet;
        }
        /// <summary>
        /// 해당 드라이브의 남아 있는 용량을 메가바이트 값으로 나타낸다.
        /// </summary>
        /// <param name="strDriveName"></param>
        /// <returns></returns>
        public long ReadFreeSpaceDriveInfo(string strDriveName)
        {
            DriveInfo drvInfo = new DriveInfo(strDriveName);

            long nRet = drvInfo.TotalFreeSpace / (1024 * 1024);

            return nRet;
        }

        public bool WriteFile(string strFilePathName, bool bAppendExistFile, string strMessage)
        {
            using (StreamWriter sw = new StreamWriter(strFilePathName, bAppendExistFile, Encoding.Unicode))
            {
                try
                {
                    sw.WriteLine(strMessage);
                }
                catch
                {
                    throw;
                }
                finally
                {
                    sw.Close();
                }
            }
            return true;
        }


        public class Log4net
        {

            private ILog log;

            /// <summary>
            /// 로그를 만들기 위한 기본 인스턴스 부여 및 환경 설정 읽기
            /// </summary>
            public Log4net()
            {
                //처음 생성시 어디에 저장할지 환경파일을 개별적으로 선택 가능하게 하는게 좋은듯
                try
                {
                    log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
                    FileInfo fiConf = new FileInfo("log4net.xml");
                    XmlConfigurator.Configure(fiConf);
                    return;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            /// <summary>
            /// 디버그 정보 쓰기
            /// </summary>
            /// <param name="strLogMessage"></param>
            public void WriteDebugLog(string strLogMessage)
            {
                if (log.IsDebugEnabled) log.Debug(strLogMessage);
            }
            /// <summary>
            /// 에러 정보 쓰기
            /// </summary>
            /// <param name="strLogMessage"></param>
            public void WriteErrorLog(string strLogMessage)
            {
                if (log.IsErrorEnabled) log.Error(strLogMessage);
            }
            /// <summary>
            /// 치명적 에러 정보 쓰기
            /// </summary>
            /// <param name="strLogMessage"></param>
            public void WriteFatalLog(string strLogMessage)
            {
                if (log.IsFatalEnabled) log.Fatal(strLogMessage);
            }
            /// <summary>
            /// 인포 정보 쓰기
            /// </summary>
            /// <param name="strLogMessage"></param>
            public void WriteInfoLog(string strLogMessage)
            {
                if (log.IsInfoEnabled) log.Info(strLogMessage);
            }
            /// <summary>
            /// 경고 정보 쓰기
            /// </summary>
            /// <param name="strLogMessage"></param>
            public void WriteWarnLog(string strLogMessage)
            {
                if (log.IsWarnEnabled) log.Warn(strLogMessage);
            }

            //이걸 쓰면 꼬일런지도 몰라서 일단 뺌
            //~clsLog()
            //{
            //    LogManager.Shutdown();
            //}
        }
    }
}
