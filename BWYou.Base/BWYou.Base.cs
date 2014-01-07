/** \page BWYou.Base BWYou.Base
 *  \section developer 개발자
 *          - You
 *  \section info 개발목적
 *          - 즐겨 사용 할 클래스 모음
 *  \section advenced 추가정보
 *          - Version : 0.3.0.0
 *          - Last Updated : 2013.07.22
 *              -# ClassWork HeartBeat Event 추가
 *              -# ThreadBase, ThreadWhile 재정리, 0.1초 간격으로 스레드 While문 처리 되도록 수정
 *              -# ThreadBase 일시 정지, 다시 시작 기능 추가
 *              -# ThreadMonitor 추가(작업이 죽었는지 여부를 감시 하는 모니터 스레드)
 *          - Last Updated : 2013.05.02 Version : 0.2.5.1
 *              -# Dispoing 버그 수정
 *          - Last Updated : 2013.04.25 Version : 0.2.5.0
 *              -# Config Class 추가
 *          - Updated : 2013.02.07 Version : 0.2.4.1
 *              -# ClassBase Event 버그 수정
 *          - Updated : 2012.11.15 Version : 0.2.4.0
 *              -# VS2010 컨버팅
 *          - Updated : 2012.10.10 Version : 0.2.3.1
 *              -# Thread Dispose 시 ForceStop 처리
 *              -# 간단 WriteLog 추가
 *          - Updated : 2012.09.19 Version : 0.2.3.0
 *              -# enum 값을 클래스 외부로 뺌.
 *              -# enum 이름 변경
 *              -# Dispose 관련 정리
 *              -# Thread Dispose 제대로 되도록 정리
 *          - Updated : 2012.08.29
 *              -# 대상 프레임워크 닷넷 3.5로 변경
 *              -# 이벤트 이름 정리, 이벤트 arg 정립
 *              -# 메세지 이벤트, 워크이벤트, 생성자 오버로딩
 *          - Error 발생시 : throw 발생.
 *          - 구현 사항
 *              -# 기본 클래스 및 스레드 클래스 생성
 *          - 미구현 사항
 *              -# 더 많은 클래스들
 *              -# 스레드쪽 일시중지
 *              -# 스레드 dispose 정확하게 구현 처리
 * 
 *  \section explain 설명
 *          - 기본 클래스
 * 
 *  \section Base Base
 *          - 기본 관련 클래스
 * 
 *          - 사용예) Class의 Base로 사용하거나 바로 사용
 *  \code
    //ClassBase 상속
    class clsSample : ClassBase
    {
        public clsSample()
            : base("TEST[" + Guid.NewGuid() + "]")
        {

        }

        private bool SampleMethod()
        {
            try
            {
                //작업 할 일~~

                return true;
            }
            catch (Exception ex)
            {
                //에러 발생 한 것 이벤트 발생.
                SayMessage(this, "Catch Error : "
                                    + Environment.NewLine + ex.Message
                                    + Environment.NewLine + ex.ToString(), MessagePriority.Debug);
                return false;
            }
        }
    }

    class clsWatcher
    {
        public clsWatcher()
        {
            clsSample cSampleBase = new clsSample();
            //ClassBase 이벤트 리스닝
            cSampleBase.MessageSay +=new ClassBase.MessageSayEventHandler(SayDo);
        }

        private void SayDo(object sender, MessageEventArgs e)
        {
            //이벤트 받으면 할 일.. 솰라 솰라
        }
    }



 *  \endcode
 * 
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace BWYou.Base
{
    /// <summary>
    /// 테스트용
    /// </summary>
    public class BWYou
    {
        /// <summary>
        /// 로그 파일 lock 체크용 object
        /// </summary>
        public static object wlog = new object();
        /// <summary>
        /// 로그 파일에 로그 쓰기
        /// </summary>
        /// <param name="LogFilePathName"></param>
        /// <param name="Message"></param>
        public static void WriteLog(string LogFilePathName, string Message)
        {
            lock (wlog)
            {
                File.AppendAllText(LogFilePathName, "[" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff") + "]" + "\t" + Message + Environment.NewLine, Encoding.UTF8);
            }
        }


    }
}
