using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BWYou.Base
{
    /// <summary>
    /// 작업 진행 상태
    /// </summary>
    public enum WorkProgressState
    {
        /// <summary>
        /// 작업 대기 중
        /// </summary>
        Standby = 0,
        /// <summary>
        /// 작업 시작 안 됨.(못 함)
        /// </summary>
        Unstarted = 2,
        /// <summary>
        /// 작업 중
        /// </summary>
        Working = 4,
        /// <summary>
        /// 작업 일시 중단
        /// </summary>
        Suspended = 8,
        /// <summary>
        /// 작업 취소 요청 됨.(아직 취소 안 됨)
        /// </summary>
        AbortRequested = 16,
        /// <summary>
        /// 작업 취소 됨
        /// </summary>
        Aborted = 32,
        /// <summary>
        /// 작업 중지 됨.
        /// </summary>
        Stopped = 64
    }

    /// <summary>
    /// 작업 이벤트
    /// </summary>
    public class WorkEventArgs : EventArgs
    {
        /// <summary>
        /// 현재 작업 진행 상태
        /// </summary>
        public WorkProgressState workProgressState { get; private set; }
        /// <summary>
        /// 현재 작업 진행 정도
        /// </summary>
        public int workProgress { get; private set; }

        /// <summary>
        /// 작업 이벤트 생성자
        /// </summary>
        /// <param name="workProgressState"></param>
        /// <param name="workProgress"></param>
        public WorkEventArgs(WorkProgressState workProgressState, int workProgress)
        {
            this.workProgressState = workProgressState;
            this.workProgress = workProgress;
        }
    }
}
