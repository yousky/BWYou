/** \page BWYou.Transfer BWYou.Transfer
 *  \section developer 개발자
 *          - You
 *  \section info 개발목적
 *          - 전송 관련 클래스 모음
 *  \section advenced 추가정보
 *          - Version : 0.2.0.1
 *          - Last Updated : 2013.04.22
 *              -# SCP mkdir 방법 변경.
 *          - Last Updated : 2013.04.22 Version : 0.2.0.0
 *              -# FTP 기능 추가
 *              -# namespace 정리
 *          - Updated : 2012.11.15 Version : 0.1.2.0
 *              -# VS2010 컨버팅
 *          - Updated : 2012.08.29 Version : 0.1.1.0
 *              -# 대상 프레임워크 닷넷 3.5로 변경
 *          - Error 발생시 : throw 발생.
 *          - 구현 사항
 *              -# SCP를 통한 SEND, RECEIVE
 *              -# FTP를 통한 SEND, RECEIVE
 *          - 미구현 사항
 *              -# SFTP
 *              -# FTP 이벤트, 비동기화
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
                BWYou.Transfer.SCP scp = new BWYou.Transfer.SCP("xxx.xxx.xxx.xxx", "id", "pass", 22); //연결에 필요한 값을 넣어 객체 생성
                scp.ftEvent += new BWYou.Transfer.SCP.FileTransferEventY(이벤트처리함수);
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
using System.Linq;
using System.Text;

namespace BWYou.Transfer
{

}
