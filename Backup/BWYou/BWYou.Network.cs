/** \page BWYou.Network BWYou.Network
 *  \section developer 개발자
 *          - You
 *  \section info 개발목적
 *          - 네트워크 관련 함수, 클래스 모음
 *  \section advenced 추가정보
 *          - Version : 0.3.2.0
 *          - Last Updated : 2012.08.29
 *              -# 대상 프레임워크 닷넷 3.5로 변경
 *          - Error 발생시 : throw 발생.
 *          - 구현 사항
 *              -# MacAddress 추출
 *              -# Mail 전송(SMTP)
 *          - 미구현 사항
 *              -# 그 외 모두
 * 
 *  \section explain 설명
 *          - 네트워크 쪽 특화 클래스
 * 
 *  \section Network Network
 *          - 네트워크 쪽 특화 클래스
 * 
 *          - 사용예)
 *  \code
            
            try
            {
                string mac1 = clsNetwork.FindMacAddress();                  //"로컬 영역 연결"이나 가장 처음 찾은 랜카드의 MacAddress 반환
                string mac2 = clsNetwork.FindMacAddress("기본 네트워크");   //"기본 네트워크"라는 네트워크명의 랜카드의 MacAddress 반환
            }
            catch (Exception ex)
            {
                //에러 발생시 처리 방법
            }

            //메일 전송
            Mail mail = new Mail("smtp.gmail.com", 587, true, "sample", "samplepw", "보내는사람@sample.com", "받는사람@sample.com");    //생성자

            try
            {
                mail.SendMail("제목입니다", "보낼메시지를 넣으세요");               //메일 전송 실시
            }
            catch (Exception ex)
            {
                //에러 발생시 처리 방법
            }

 * 
 *  \endcode
 * 
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.Management;                //WMI 사용하여 MAC 주소를 얻기 위함
using System.Net.Mail;
using System.Net.Mime;
using System.Net;

namespace BWYou
{
    /// <summary>
    /// 네트워크 쪽 특화 클래스
    /// </summary>
    public class Network
    {

        /// <summary>
        /// 마지막 에러 메세지 저장
        /// </summary>
        private static string lastError = "";
        /// <summary>
        /// 마지막 에러 메세지 보여주기
        /// </summary>
        public static string LastError
        {
            get { return lastError; }
        }


        /// <summary>
        /// 맥주소 찾기(기본으로 네트워크망 이름이 "로컬 영역 연결"인 경우에 찾고, 없을 경우 임의의 맥주소 찾음)
        /// </summary>
        /// <returns></returns>
        public static string FindMacAddress()
        {
            string strMacAddress = "";

            try
            {
                ObjectQuery objectQuery = new ObjectQuery("SELECT * FROM Win32_NetworkAdapter");

                ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher(objectQuery);

                ManagementObjectCollection moc = managementObjectSearcher.Get();


                //int i = 0;
                foreach (ManagementObject managementObject in moc)
                {
                    //foreach (PropertyData pd in managementObject.Properties)
                    //{
                    //    Console.WriteLine(pd.Name + " : " + pd.Value);
                    //}

                    if ((managementObject["MACAddress"] != null) && (managementObject["NetConnectionID"]) != null)
                    {
                        //Console.WriteLine("===========================================" + Environment.NewLine);
                        //Console.WriteLine(i.ToString() + "\t" + managementObject["MACAddress"].ToString());
                        //Console.WriteLine(i.ToString() + "\t" + managementObject["AdapterTypeID"].ToString());
                        //Console.WriteLine(i.ToString() + "\t" + managementObject["NetConnectionID"].ToString());
                        //Console.WriteLine("===========================================" + Environment.NewLine);

                        //로컬 영역 연결의 맥주소 찾을 경우 찾기 중지
                        if (managementObject["AdapterTypeID"].ToString() == "0" &&
                            managementObject["NetConnectionID"].ToString() == "로컬 영역 연결")
                        {
                            strMacAddress = managementObject["MACAddress"].ToString();
                            break;
                        }
                        //AdapterTypeID = 0인 즉 처음 발견된 랜카드의 맥주소 찾기
                        else if (managementObject["AdapterTypeID"].ToString() == "0")
                        {
                            strMacAddress = managementObject["MACAddress"].ToString();
                        }

                    }
                    //i++;
                }
            }
            catch(Exception ex)
            {
                setErrorLog(ex);
                throw;
            }
            return strMacAddress;
        }
        /// <summary>
        /// 특정 네트워크망 이름의 맥주소 찾기
        /// </summary>
        /// <param name="strNetworkName"></param>
        /// <returns></returns>
        public static string FindMacAddress(string strNetworkName)
        {
            string strMacAddress = "";

            try
            {
                ObjectQuery objectQuery = new ObjectQuery("SELECT * FROM Win32_NetworkAdapter");

                ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher(objectQuery);

                ManagementObjectCollection moc = managementObjectSearcher.Get();


                //int i = 0;
                foreach (ManagementObject managementObject in moc)
                {
                    //foreach (PropertyData pd in managementObject.Properties)
                    //{
                    //    Console.WriteLine(pd.Name + " : " + pd.Value);
                    //}

                    if ((managementObject["MACAddress"] != null) && (managementObject["NetConnectionID"]) != null)
                    {
                        //Console.WriteLine("===========================================" + Environment.NewLine);
                        //Console.WriteLine(i.ToString() + "\t" + managementObject["MACAddress"].ToString());
                        //Console.WriteLine(i.ToString() + "\t" + managementObject["AdapterTypeID"].ToString());
                        //Console.WriteLine(i.ToString() + "\t" + managementObject["NetConnectionID"].ToString());
                        //Console.WriteLine("===========================================" + Environment.NewLine);

                        if (managementObject["AdapterTypeID"].ToString() == "0" &&
                            managementObject["NetConnectionID"].ToString() == strNetworkName)
                        {
                            strMacAddress = managementObject["MACAddress"].ToString();
                            break;
                        }

                    }
                    //i++;
                }
            }
            catch (Exception ex)
            {
                setErrorLog(ex);
                throw;
            }
            return strMacAddress;
        }

        /// <summary>
        /// 에러 객체를 따로 출력하거나 하는 등을 위하여 따로 둠.
        /// </summary>
        /// <param name="ex">Exception 객체</param>
        private static void setErrorLog(Exception ex)
        {
            lastError = ex.ToString() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace;
            //Console.WriteLine(e.Message);
        }


        /// <summary>
        /// 메일 관련 클래스
        /// </summary>
        public class Mail
        {
            string strSMTPSERVER = "";
            int nSMTPPORT = 0;
            bool bSMTPSSL = true;
            string strSMTPID = "";
            string strSMTPPW = "";
            string strSMTPFROM = "";
            string strSMTPTO = "";

            /// <summary>
            /// 마지막 에러 메세지 저장
            /// </summary>
            private string lastError = "";
            /// <summary>
            /// 마지막 에러 메세지 보여주기
            /// </summary>
            public string LastError
            {
                get { return lastError; }
            }

            /// <summary>
            /// 생성자, 기본 값 변경
            /// </summary>
            /// <param name="smtpServer"></param>
            /// <param name="smtpPort"></param>
            /// <param name="smtpSSL"></param>
            /// <param name="smtpID"></param>
            /// <param name="smtpPW"></param>
            /// <param name="smtpFrom"></param>
            /// <param name="smtpTo"></param>
            public Mail(string smtpServer, int smtpPort, bool smtpSSL, string smtpID, string smtpPW, string smtpFrom, string smtpTo)
            {
                strSMTPSERVER = smtpServer;
                nSMTPPORT = smtpPort;
                bSMTPSSL = smtpSSL;
                strSMTPID = smtpID;
                strSMTPPW = smtpPW;
                strSMTPFROM = smtpFrom;
                strSMTPTO = smtpTo;
            }

            /// <summary>
            /// 메일 발송
            /// </summary>
            /// <param name="smtpFrom"></param>
            /// <param name="smtpTo"></param>
            /// <param name="mailSubject"></param>
            /// <param name="mailMessage"></param>
            /// <returns></returns>
            public bool SendMail(string smtpFrom, string smtpTo, string mailSubject, string mailMessage)
            {
                MailAddress from = new MailAddress(smtpFrom, smtpFrom);
                MailAddress to = new MailAddress(smtpTo, smtpTo);
                MailMessage mMessage = new MailMessage(from, to);
                mMessage.Subject = mailSubject;
                mMessage.Body = mailMessage;


                SmtpClient smtp = new SmtpClient(strSMTPSERVER, nSMTPPORT);

                //Credentials 정보를 사용할지 여부 결정
                smtp.UseDefaultCredentials = true;
                smtp.Credentials = new NetworkCredential(strSMTPID, strSMTPPW);

                //SSL 보안 여부
                smtp.EnableSsl = bSMTPSSL;

                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Timeout = 300000;  //5분

                try
                {
                    smtp.Send(mMessage);

                }
                catch (Exception ex)
                {
                    setErrorLog(ex);
                    throw;
                }

                return true;

            }
            /// <summary>
            /// 메일 발송
            /// </summary>
            /// <param name="mailSubject"></param>
            /// <param name="mailMessage"></param>
            /// <returns></returns>
            public bool SendMail(string mailSubject, string mailMessage)
            {
                MailAddress from = new MailAddress(strSMTPFROM, strSMTPFROM);
                MailAddress to = new MailAddress(strSMTPTO, strSMTPTO);
                MailMessage mMessage = new MailMessage(from, to);
                mMessage.Subject = mailSubject;
                mMessage.Body = mailMessage;


                SmtpClient smtp = new SmtpClient(strSMTPSERVER, nSMTPPORT);

                //Credentials 정보를 사용할지 여부 결정
                smtp.UseDefaultCredentials = true;
                smtp.Credentials = new NetworkCredential(strSMTPID, strSMTPPW);

                //SSL 보안 여부
                smtp.EnableSsl = bSMTPSSL;

                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Timeout = 300000;  //5분

                try
                {
                    smtp.Send(mMessage);

                }
                catch (Exception ex)
                {
                    setErrorLog(ex);
                    throw;
                }

                return true;

            }

            /// <summary>
            /// 에러 객체를 따로 출력하거나 하는 등을 위하여 따로 둠.
            /// </summary>
            /// <param name="ex">Exception 객체</param>
            private void setErrorLog(Exception ex)
            {
                lastError = ex.ToString() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace;
                //Console.WriteLine(e.Message);
            }
        }

    }
}
