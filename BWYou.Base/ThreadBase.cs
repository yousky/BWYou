using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Data;

namespace BWYou.Base
{
    /// <summary>
    /// 스레드 기본 클래스, ClassWork 상속, Do() override하여 사용
    /// </summary>
    public abstract class ThreadBase : ClassWork
    {
        /// <summary>
        /// 스레드 내부 Sleep Time 기준. 밀리초
        /// </summary>
        protected readonly int nThreadSleepTime = 100;

        /// <summary>
        /// 작업 스레드
        /// </summary>
        protected Thread thr;
        /// <summary>
        /// 스레드 정지 신호
        /// </summary>
        protected bool bStopThread = true;
        /// <summary>
        /// 스레드 일시 정지 신호
        /// </summary>
        protected bool bPauseThread = false;
        /// <summary>
        /// 스레드 기본 생성자
        /// </summary>
        /// <param name="Name"></param>
        public ThreadBase(string Name)
            : base(Name)
        {

        }

        /// <summary>
        /// 스레드 실행
        /// </summary>
        public void Start()
        {
            bStopThread = false;
            if (thr == null || thr.ThreadState != System.Threading.ThreadState.Running)
            {
                thr = new Thread(new ThreadStart(DoDef));
                thr.Start();   //켬
            }

            Thread.Sleep(1);
        }

        /// <summary>
        /// 스레드 중지하도록 요청
        /// </summary>
        public void StopThread()
        {
            bStopThread = true;
            //thr = null;
        } 

        /// <summary>
        /// 강제로 스레드 중지
        /// </summary>
        public void ForceStopThread()
        {
            bStopThread = true;

            Thread.Sleep(1);

            if (thr != null)
            {
                if (thr.ThreadState != System.Threading.ThreadState.Aborted)
                {
                    try
                    {
                        thr.Abort();   //끔
                    }
                    catch (Exception)
                    {
                        //무시무시
                    }
                }
                thr = null;
            }

            Thread.Sleep(1);
        }
        

        /// <summary>
        /// 스레드 일시 정지하도록 요청
        /// </summary>
        public void PauseThread()
        {
            bPauseThread = true;
        }
        /// <summary>
        /// 스레드 일시 정지 된 것 다시 시작 하도록 요청
        /// </summary>
        public void ResumeThread()
        {
            bPauseThread = false;
        }

        /// <summary>
        /// 기본 스레드 하는 일
        /// </summary>
        protected virtual void DoDef()
        {
            lock(this)
            {
                try
                {
                    SayMessage(this, new MessageEventArgs(Name + " 스레드  처리 시작", MessagePriority.Info));
                    BeatHeart(this);
                    ProgressWork(this, new WorkEventArgs(WorkProgressState.Standby, 0));

                    while (bStopThread != true)
                    {
                        if (bPauseThread == false)
                        {
                            Do();
                        }
                        else
                        {
                            BeatHeart(this);
                            ProgressWork(this, new WorkEventArgs(WorkProgressState.Suspended, 0));
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

        /// <summary>
        /// 반복 할 일.
        /// ProgressWork(this, new WorkEventArgs(WorkProgressState.Working, 진행률값));
        /// BeatHeart(this); 잊지 말 것
        /// </summary>
        protected abstract void Do();

        /// <summary>
        /// 스레드 기본 클래스 소멸자
        /// </summary>
        ~ThreadBase()
        {
            Dispose(false);
        }
        /// <summary>
        /// 스레드 기본 클래스 Dispose
        /// </summary>
        public new void Dispose()
        {
            Dispose(true);
        }
        /// <summary>
        /// Dispose Pattern 구현
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (disposing == true)
            {
                //ForceStopThread();
                StopThread();
            }

            base.Dispose(disposing);
        }

    }
}
