using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Data;

namespace BWYou.Base
{
    /// <summary>
    /// 일정 주기마다 반복되는 스레드 기본형
    /// </summary>
    public abstract class ThreadWhile : ThreadBase
    {
        /// <summary>
        /// 반복 실행 주기
        /// </summary>
        public int nWhileMax { get; set; }

        /// <summary>
        /// 스레드 기본 생성자
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="nWhileMax">반복 주기(초)</param>
        public ThreadWhile(string Name, int nWhileMax)
            : base(Name)
        {
            this.nWhileMax = nWhileMax;
        }
        /// <summary>
        /// 기본 스레드 하는 일
        /// </summary>
        protected override void DoDef()
        {
            lock(this)
            {
                try
                {
                    SayMessage(this, new MessageEventArgs(Name + " 스레드  처리 시작", MessagePriority.Info));
                    ProgressWork(this, new WorkEventArgs(WorkProgressState.Working, 0));

                    int nWhile = nWhileMax - 1;
                    while (bStopThread != true)
                    {
                        nWhile++;
                        ProgressWork(this, new WorkEventArgs(WorkProgressState.Working, 0));

                        if (nWhile >= nWhileMax)
                        {
                            nWhile = 0;

                            Do();   //실제 반복 할 것.
                            ProgressWork(this, new WorkEventArgs(WorkProgressState.Working, nWhile));
                        }

                        Thread.Sleep(1000);
                    }

                    SayMessage(this, new MessageEventArgs(Name + " 스레드 처리 종료", MessagePriority.Info));
                    return;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    ProgressWork(this, new WorkEventArgs(WorkProgressState.Stopped, 0));
                }
            }
        }
        /// <summary>
        /// 반복 할 일.
        /// </summary>
        protected abstract void Do();

    }
}
