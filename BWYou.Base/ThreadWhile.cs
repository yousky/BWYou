using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Data;

namespace BWYou.Base
{
    /// <summary>
    /// 일정 주기마다 반복되는 스레드 기본형, ThreadBase 상속
    /// </summary>
    public abstract class ThreadWhile : ThreadBase
    {
        /// <summary>
        /// 반복 실행 주기(nThreadSleepTime 밀리초 단위)
        /// </summary>
        public int nRepeatCycle { get; set; }

        /// <summary>
        /// 스레드 기본 생성자
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="nRepeatCycleSecond">반복 주기(초)</param>
        public ThreadWhile(string Name, int nRepeatCycleSecond)
            : base(Name)
        {
            this.nRepeatCycle = nRepeatCycleSecond * (1000 / nThreadSleepTime);
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
                    BeatHeart(this);    //ThreadMonitor 감시 처리 위해
                    ProgressWork(this, new WorkEventArgs(WorkProgressState.Standby, 0));

                    int nCycle = nRepeatCycle - 1;
                    while (bStopThread != true)
                    {
                        nCycle++;

                        if (nCycle >= nRepeatCycle)
                        {
                            nCycle = 0;

                            if (bPauseThread == false)
                            {
                                Do();
                            }
                            else
                            {
                                BeatHeart(this);
                                ProgressWork(this, new WorkEventArgs(WorkProgressState.Suspended, 0));
                            }
                        }

                        Thread.Sleep(nThreadSleepTime);
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
                    BeatHeart(this);
                    ProgressWork(this, new WorkEventArgs(WorkProgressState.Stopped, 0));
                }
            }
        }

    }
}
