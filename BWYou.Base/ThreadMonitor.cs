using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BWYou.Base
{
    /// <summary>
    /// 작업 중인 것 감시하는 스레드
    /// 
    /// 메인(폼 또는 콘솔) 스레드
    /// 	실 작업 & 모니터 함수
    /// 		실 작업 스레드 생성
    /// 		모니터 클래스 생성(실 작업 스레드 전달)
    /// 	모니터 클래스 이벤트 리스닝 함수
    /// 		실 작업 스레드 죽이지 않았으면서 죽었다는 이벤트 발생시 기존 실 작업 & 모니터 없애고 새로 실 작업 & 모니터 함수 다시 처리. 반복.. 
    /// </summary>
    public class ThreadMonitor : ThreadWhile
    {
        #region 이벤트
        /// <summary>
        /// 감시 작업 죽었다는 걸 알리는 이벤트 핸들러
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void DeadClassWork4MonitorNotifyEventHandler(object sender, EventArgs e);
        /// <summary>
        /// 감시 작업 죽었다는 걸 알리는 이벤트
        /// </summary>
        public event DeadClassWork4MonitorNotifyEventHandler DeadClassWork4MonitorNotify;
        /// <summary>
        /// 감시 작업 죽었다는 걸 알림
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void NotifyDeadClassWork4Monitor(object sender, EventArgs e)
        {
            if (DeadClassWork4MonitorNotify != null)
            {
                DeadClassWork4MonitorNotify(sender, e);
            }
        }
        /// <summary>
        /// 감시 작업 죽었다는 걸 알림
        /// </summary>
        /// <param name="sender"></param>
        protected void NotifyDeadClassWork4Monitor(object sender)
        {
            if (DeadClassWork4MonitorNotify != null)
            {
                DeadClassWork4MonitorNotify(sender, new EventArgs());
            }
        }

        #endregion

        /// <summary>
        /// 시간 초과로 죽었다고 판단 할 시간(초단위)
        /// </summary>
        public int WorkTimeoutSecond { get; set; }
        /// <summary>
        /// 마지막 HeartBeat 발생한 일시
        /// </summary>
        public DateTime LastHeartBeatDateTime { get; protected set; }
        /// <summary>
        /// 마지막 작업 상태
        /// </summary>
        public WorkProgressState LastWorkProgressState { get; protected set; }

        /// <summary>
        /// 살아 있는지 감시 할 스레드
        /// </summary>
        public ClassWork classWork4Monitor { get; protected set; }

        /// <summary>
        /// 생성자
        /// DeadClassWork4MonitorNotify 이벤트를 통해 감시 할 작업이 죽었을 때 알림 발생 시킴
        /// </summary>
        /// <param name="classWork4Monitor">살아 있는지 감시 할 작업</param>
        /// <param name="RepeatCycleSecond">감시 주기(초단위)</param>
        /// <param name="ThreadTimeoutSecond"></param>
        public ThreadMonitor(ClassWork classWork4Monitor, int RepeatCycleSecond, int ThreadTimeoutSecond)
            : base("ThreadMonitor[" + Guid.NewGuid() + "]", RepeatCycleSecond)
        {
            this.WorkTimeoutSecond = ThreadTimeoutSecond;
            ChangeClassWork4Monitor(classWork4Monitor);
        }
        public void ChangeClassWork4Monitor(ClassWork classWork4Monitor)
        {
            this.classWork4Monitor = classWork4Monitor;
            if (classWork4Monitor != null)
            {
                classWork4Monitor.HeartBeat += new HeartBeatEventHandler(WriteClassWork4Monitor_HeartBeatDateTime);
                classWork4Monitor.WorkProgress += new WorkEventHandler(WriteClassWork4Monitor_WorkProgress);
            }
        }
        /// <summary>
        /// 감시 클래스의 작업 상태 기록
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void  WriteClassWork4Monitor_WorkProgress(object sender, WorkEventArgs e)
        {
            this.LastWorkProgressState = e.workProgressState;
        }
        /// <summary>
        /// 감시 클래스의 HeartBeat 이벤트 시간 기록
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WriteClassWork4Monitor_HeartBeatDateTime(object sender, EventArgs e)
        {
            this.LastHeartBeatDateTime = DateTime.Now;
        }
        /// <summary>
        /// 감시 하고 있는 작업 살아 있는지 여부 확인
        /// </summary>
        protected override void Do()
        {
            BeatHeart(this);    //감시 스레드 살아 있는지 여부 알림

            bool bDeadedThread = false;
            if (classWork4Monitor != null)
            {
                if (LastWorkProgressState == WorkProgressState.Working || LastWorkProgressState == WorkProgressState.Standby || LastWorkProgressState == WorkProgressState.Suspended)
                {
                    if (LastHeartBeatDateTime.AddSeconds(WorkTimeoutSecond).CompareTo(DateTime.Now) < 0)
                    {
                        bDeadedThread = true;
                        SayMessage(this, "ClassWork4Monitor Dead[Timeout(" + WorkTimeoutSecond.ToString() + "s)] : " + classWork4Monitor.Name, MessagePriority.Warn);
                    }
                }
                else
                {
                    bDeadedThread = true;
                    SayMessage(this, "ClassWork4Monitor Dead[Not Work(" + LastWorkProgressState.ToString() + ")] : " + classWork4Monitor.Name, MessagePriority.Warn);
                }
            }
            else
            {
                bDeadedThread = true;
                SayMessage(this, "ClassWork4Monitor Dead[(null)]", MessagePriority.Warn);
            }

            if (bDeadedThread == true)
            {
                //모니터 중인 작업 죽었다는 것을 알림
                NotifyDeadClassWork4Monitor(this);
            }
        }

        /// <summary>
        /// 모니터 스레드 소멸자
        /// </summary>
        ~ThreadMonitor()
        {
            Dispose(false);
        }
        /// <summary>
        /// 모니터 스레드 Dispose
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
                if (DeadClassWork4MonitorNotify != null)
                {
                    foreach (DeadClassWork4MonitorNotifyEventHandler eventDelegate in DeadClassWork4MonitorNotify.GetInvocationList())
                    {
                        DeadClassWork4MonitorNotify -= new DeadClassWork4MonitorNotifyEventHandler(eventDelegate);
                    }
                }
            }

            base.Dispose(disposing);
        }
    }
}
