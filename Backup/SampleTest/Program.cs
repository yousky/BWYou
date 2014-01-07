using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Threading;

namespace SampleTest
{
    static class Program
    {
        // 어플리케이션의 이름
        private static string strAppConstName = "BWYou_SampleTest";
        // 다중기동을 방지하는 뮤텍스인스턴스
        private static Mutex mutexObject;


        /// <summary>
        /// 해당 응용 프로그램의 주 진입점입니다.
        /// </summary>
        [STAThread]
        static void Main()
        {

            // Windows 2000(NT 5.0)이후만 글로벌 뮤텍스가 사용가능
            OperatingSystem os = Environment.OSVersion;
            if ((os.Platform == PlatformID.Win32NT) && (os.Version.Major >= 5))
            {
                strAppConstName = @"Global\" + strAppConstName;
            }

            try
            {
                // 뮤텍스를 생성
                mutexObject = new Mutex(false, strAppConstName);
            }
            catch (ApplicationException e)
            {
                // 글로벌 뮤텍스에 의한 다중실행 방지
                MessageBox.Show("이미 실행되고 있습니다." + Environment.NewLine + e.Message, "다중실행방지");
                return;
            }

            // 뮤텍스를 취득
            if (mutexObject.WaitOne(3000, false))
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new frmMain());

                //프로그램사용이 끝났으니 뮤텍스를 해방
                mutexObject.ReleaseMutex();
            }
            else
            {
                //이미 실행중이니 경고 메시지
                MessageBox.Show("이미 실행되고 있습니다.", "다중실행방지");
            }

            // 뮤텍스를 파기하고 완전종료
            mutexObject.Close();
        }
    }
}
