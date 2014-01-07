/** \page BWYou.Base BWYou.Base
 *  \section developer 개발자
 *          - You
 *  \section info 개발목적
 *          - 즐겨 사용 할 클래스 모음
 *  \section advenced 추가정보
 *          - Version : 0.2.3.1
 *          - Last Updated : 2012.10.10
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
 * 
 * 
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace BWYou.Base
{
    class BWYou
    {
        public static object wlog = new object();
        public static void WriteLog(string LogFilePathName, string Message)
        {
            lock (wlog)
            {
                File.AppendAllText(LogFilePathName, "[" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff") + "]" + "\t" + Message + Environment.NewLine, Encoding.UTF8);
            }
        }
    }
}
