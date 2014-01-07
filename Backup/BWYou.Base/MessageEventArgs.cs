using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BWYou.Base
{
    /// <summary>
    /// 중요도, 우선 순위
    /// </summary>
    public enum MessagePriority
    {
        /// <summary>
        /// Info Level - 0
        /// </summary>
        Info = 0,
        /// <summary>
        /// Warn Level - 1
        /// </summary>
        Warn = 1,
        /// <summary>
        /// Debug Level - 2
        /// </summary>
        Debug = 2,
        /// <summary>
        /// Error Level - 4
        /// </summary>
        Error = 4,
        /// <summary>
        /// Fatal Level - 8
        /// </summary>
        Fatal = 8
    }

    /// <summary>
    /// 메세지 이벤트
    /// </summary>
    public class MessageEventArgs : EventArgs
    {
        /// <summary>
        /// 메세지
        /// </summary>
        public string message { get; private set; }
        /// <summary>
        /// 메세지 중요도
        /// </summary>
        public MessagePriority priority { get; private set; }

        /// <summary>
        /// 메세지 이벤트 생성자
        /// </summary>
        /// <param name="message"></param>
        /// <param name="priority"></param>
        public MessageEventArgs(string message, MessagePriority priority)
        {
            this.message = message;
            this.priority = priority;
        }
    }
}
