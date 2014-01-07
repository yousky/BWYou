using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BWYou.Base
{
    /// <summary>
    /// 기본 클래스, 클래스명 부여, IDisposable 인터페이스, SayMessage 이벤트
    /// </summary>
    public class ClassBase : IDisposable
    {
        /// <summary>
        /// 클래스 명
        /// </summary>
        public string Name { get; set; }

        #region 이벤트

        /// <summary>
        /// 메세지 처리용 이벤트 핸들러
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void MessageSayEventHandler(object sender, MessageEventArgs e);

        /// <summary>
        /// 메세지 처리 이벤트
        /// </summary>
        public event MessageSayEventHandler MessageSay;

        /// <summary>
        /// 메세지 이벤트 발생
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void SayMessage(object sender, MessageEventArgs e)
        {
            if (MessageSay != null)
            {
                MessageSay(sender, e);
            }
        }
        /// <summary>
        /// 메세지 이벤트 발생
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="message"></param>
        /// <param name="priority"></param>
        protected virtual void SayMessage(object sender, string message, MessagePriority priority)
        {
            if (MessageSay != null)
            {
                MessageSay(sender, new MessageEventArgs(message, priority));
            }
        }
        /// <summary>
        /// 메세지 이벤트 발생
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="message"></param>
        protected virtual void SayMessage(object sender, string message)
        {
            if (MessageSay != null)
            {
                MessageSay(sender, new MessageEventArgs(message, MessagePriority.Info));
            }
        }

        #endregion


        #region 생성 소멸

        /// <summary>
        /// 기본 클래스 생성자
        /// </summary>
        /// <param name="Name"></param>
        public ClassBase(string Name)
        {
            this.Name = Name;
        }
        /// <summary>
        /// 기본 클래스 생성자
        /// </summary>
        public ClassBase()
        {
            this.Name = "";
        }
        /// <summary>
        /// 기본 클래스 소멸자
        /// </summary>
        ~ClassBase()
        {
            Dispose(false);
        }
        /// <summary>
        /// 기본 클래스 Dispose
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }
        /// <summary>
        /// Dispose Pattern 구현
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing == true && MessageSay != null)
            {
                foreach (MessageSayEventHandler eventDelegate in MessageSay.GetInvocationList())
                {
                    MessageSay -= new MessageSayEventHandler(eventDelegate);
                }
            }
        }

        #endregion
    }
}
