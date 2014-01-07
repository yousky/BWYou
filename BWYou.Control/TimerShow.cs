using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace BWYou.Control
{
    /// <summary>
    /// 지정 시간만큰 컨트롤 보이기
    /// </summary>
    public class TimerShow
    {
        /// <summary>
        /// 메세지를 지정한 시간만큼 지정 위치에 표시 한다.
        /// </summary>
        /// <param name="frmDisplay">표시 될 폼</param>
        /// <param name="strMessage">표시 할 메세지</param>
        public void Show(Form frmDisplay, string strMessage)
        {
            Show(frmDisplay, strMessage, 1500);
        }
        /// <summary>
        /// 메세지를 지정한 시간만큼 지정 위치에 표시 한다.
        /// </summary>
        /// <param name="frmDisplay">표시 될 폼</param>
        /// <param name="strMessage">표시 할 메세지</param>
        /// <param name="nShowTime">표시 하는 시간(ms)</param>
        public void Show(Form frmDisplay, string strMessage, int nShowTime)
        {
            Point pt = new Point(0, 0);
            Show(frmDisplay, strMessage, pt, nShowTime);
        }
        /// <summary>
        /// 메세지를 지정한 시간만큼 지정 위치에 표시 한다.
        /// </summary>
        /// <param name="frmDisplay">표시 될 폼</param>
        /// <param name="strMessage">표시 할 메세지</param>
        /// <param name="ptLocation">표시 될 폼에서의 상대적인 위치</param>
        /// <param name="nShowTime">표시 하는 시간(ms)</param>
        public void Show(System.Windows.Forms.Form frmDisplay, string strMessage, Point ptLocation, int nShowTime)
        {
            Label lbl = new Label();
            lbl.Location = ptLocation;
            lbl.Anchor = AnchorStyles.Right;
            lbl.AutoSize = true;
            lbl.BackColor = SystemColors.Info;
            lbl.BorderStyle = BorderStyle.None;
            lbl.Text = strMessage;

            Show(frmDisplay, lbl, true, nShowTime);

        }
        /// <summary>
        /// 메세지가 들어간 컨트롤을 지정한 시간만큼 표시 한다.
        /// </summary>
        /// <param name="frmDisplay">표시 될 폼</param>
        /// <param name="ctlMessage">표시 할 메세지 컨트롤</param>
        /// <param name="bStopRemoveOnFocus">마우스로 표시 할 메세지 컨트롤을 가리키고 있으면 사라지지 않게 하는지 여부</param>
        /// <param name="nShowTime">표시 하는 시간(ms)</param>
        public void Show(Form frmDisplay, System.Windows.Forms.Control ctlMessage, bool bStopRemoveOnFocus, int nShowTime)
        {
            frmDisplay.Controls.Add(ctlMessage);
            ctlMessage.BringToFront();

            System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
            timer.Interval = nShowTime;

            if (bStopRemoveOnFocus == true)
            {
                //컨트롤 내에서 마우스 나가면 일정 시간 후 메세지 사라지기
                ctlMessage.MouseLeave += new EventHandler(
                                                            delegate(object sender, EventArgs e)
                                                            {
                                                                timer.Start();
                                                            }
                                                        );

                //컨트롤 내에 마우스 존재시에는 메세지 계속 보이기
                ctlMessage.MouseEnter += new EventHandler(
                                                            delegate(object sender, EventArgs e)
                                                            {
                                                                timer.Stop();
                                                            }
                                                        );

            }


            //일정 시간 후 컨트롤 사라지게 하기
            timer.Tick += new EventHandler(
                                                delegate(object sender, EventArgs e)
                                                {
                                                    frmDisplay.Controls.Remove(ctlMessage);
                                                    timer.Stop();
                                                    timer.Dispose();
                                                }
                                            );
            timer.Start();

        }


    }

}
