using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BWYou.Base;

namespace BWYou.Control
{
    /// <summary>
    /// 메세지 처리용 리스트뷰 기본형
    /// </summary>
    public partial class ListViewBase : UserControl
    {
        /// <summary>
        /// 리스트뷰에 보여 지는 최대 행 수
        /// </summary>
        public int ItemsMaxCount { get; set; }
        /// <summary>
        /// 리스트뷰의 시간이 보여지는 포맷
        /// </summary>
        public string DateTimeFormat { get; set; }
        /// <summary>
        /// 하이라이트 될 때의 배경색
        /// </summary>
        public Color ProbBackColor { get; set; }
        /// <summary>
        /// 하이라이트 될 때의 전경색
        /// </summary>
        public Color ProbForeColor { get; set; }
        /// <summary>
        /// 하이라이트 되는 기준 Priority. 설정 값 이상이면 하이라이트
        /// </summary>
        public MessagePriority ProbPriority { get; set; }

        /// <summary>
        /// 리스트뷰 컨트롤
        /// </summary>
        public ListView listView { get; set; }

        /// <summary>
        /// 생성자
        /// </summary>
        public ListViewBase()
        {
            InitializeComponent();

            ItemsMaxCount = 100;
            DateTimeFormat = "HH:mm:ss";
            ProbBackColor = Color.GreenYellow;
            ProbForeColor = Color.Red;
            ProbPriority = MessagePriority.Debug;
            listView = lsvMessage;
        }


        #region ListView

        /// <summary>
        /// 리스트뷰 메세지 뿌리기용 델리게이트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected delegate void dWriteMessage(object sender, MessageEventArgs e);
        /// <summary>
        /// 리스트뷰에 메세지 뿌리기 위한 쓰레드용 처리(델리게이트, 인보크)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void WriteMessageInvoke(object sender, MessageEventArgs e)
        {
            if (lsvMessage.InvokeRequired)
            {
                dWriteMessage WM = new dWriteMessage(WriteMessage);
                lsvMessage.Invoke(WM, sender, e);
            }
            else
            {
                WriteMessage(sender, e);
            }
        }
        /// <summary>
        /// 리스트뷰에 메세지 뿌려라
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void InvokeWriteMessage(object sender, MessageEventArgs e)
        {
            WriteMessageInvoke(sender, e);
        }
        /// <summary>
        /// 리스트뷰에 메세지 뿌려라
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="message"></param>
        /// <param name="priority"></param>
        public void InvokeWriteMessage(object sender, string message, MessagePriority priority)
        {
            WriteMessageInvoke(sender, new MessageEventArgs(message, priority));
        }
        /// <summary>
        /// 리스트뷰에 메세지 뿌려라
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="message"></param>
        public void InvokeWriteMessage(object sender, string message)
        {
            WriteMessageInvoke(sender, new MessageEventArgs(message, MessagePriority.Info));
        }
        /// <summary>
        /// 리스트뷰에 메세지 뿌려라
        /// </summary>
        /// <param name="message"></param>
        /// <param name="priority"></param>
        public void InvokeWriteMessage(string message, MessagePriority priority)
        {
            WriteMessageInvoke(this, new MessageEventArgs(message, priority));
        }
        /// <summary>
        /// 리스트뷰에 메세지 뿌려라
        /// </summary>
        /// <param name="message"></param>
        public void InvokeWriteMessage(string message)
        {
            WriteMessageInvoke(this, new MessageEventArgs(message, MessagePriority.Info));
        }


        /// <summary>
        /// 리스트뷰에 메세지 뿌리기
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void WriteMessage(object sender, MessageEventArgs e)
        {
            if (lsvMessage.Items.Count > ItemsMaxCount)
            {
                lsvMessage.Items.RemoveAt(0);
            }

            ListViewItem lsvItem = new ListViewItem();
            lsvItem.Text = DateTime.Now.ToString(DateTimeFormat);

            if (e.priority >= ProbPriority)
            {
                lsvItem.BackColor = ProbBackColor;
                lsvItem.ForeColor = ProbForeColor;
                //lsvItem.Font = new Font("PMingLiU", 22, FontStyle.Bold);
            }

            lsvItem.SubItems.Add(e.message);
            lsvMessage.Items.Add(lsvItem);
            lsvMessage.EnsureVisible(lsvMessage.Items.Count - 1);    //최하단의 값이 항상 보이도록 설정
        }

        #endregion

    }
}
