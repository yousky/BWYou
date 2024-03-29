﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BWYou.Base
{
    /// <summary>
    /// 작업 클래스, ClassBase 상속, WorkProgress, HeartBeat 이벤트 구현
    /// </summary>
    public class ClassWork : ClassBase
    {

        #region 이벤트
        /// <summary>
        /// 작업 진행 이벤트 핸들러
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void WorkEventHandler(object sender, WorkEventArgs e);
        /// <summary>
        /// 작업 진행 이벤트
        /// </summary>
        public event WorkEventHandler WorkProgress;
        /// <summary>
        /// 작업 진행 발생
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ProgressWork(object sender, WorkEventArgs e)
        {
            if (WorkProgress != null)
            {
                WorkProgress(sender, e);
            }
        }
        /// <summary>
        /// 작업 진행 발생
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="workProgressState"></param>
        /// <param name="workProgress"></param>
        protected void ProgressWork(object sender, WorkProgressState workProgressState, int workProgress)
        {
            if (WorkProgress != null)
            {
                WorkProgress(sender, new WorkEventArgs(workProgressState, workProgress));
            }
        }
        /// <summary>
        /// 작업 진행 발생
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="workProgressState"></param>
        protected void ProgressWork(object sender, WorkProgressState workProgressState)
        {
            if (WorkProgress != null)
            {
                WorkProgress(sender, new WorkEventArgs(workProgressState, 0));
            }
        }


        /// <summary>
        /// Heart Beat 이벤트 핸들러
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void HeartBeatEventHandler(object sender, EventArgs e);
        /// <summary>
        /// Heart Beat 이벤트
        /// </summary>
        public event HeartBeatEventHandler HeartBeat;
        /// <summary>
        /// Heart Beat 발생
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void BeatHeart(object sender, EventArgs e)
        {
            if (HeartBeat != null)
            {
                HeartBeat(sender, e);
            }
        }
        /// <summary>
        /// Heart Beat 발생
        /// </summary>
        /// <param name="sender"></param>
        protected void BeatHeart(object sender)
        {
            if (HeartBeat != null)
            {
                HeartBeat(sender, new EventArgs());
            }
        }

        #endregion


        #region 생성 소멸

        /// <summary>
        /// 작업 클래스 생성자
        /// </summary>
        /// <param name="Name"></param>
        public ClassWork(string Name)
            : base(Name)
        {

        }
        /// <summary>
        /// 작업 클래스 생성자
        /// </summary>
        public ClassWork()
        {

        }
        /// <summary>
        /// 작업 클래스 소멸자
        /// </summary>
        ~ClassWork()
        {
            Dispose(false);
        }
        /// <summary>
        /// 작업 클래스 Dispose
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
                if (WorkProgress !=null)
                {
                    foreach (WorkEventHandler eventDelegate in WorkProgress.GetInvocationList())
                    {
                        WorkProgress -= new WorkEventHandler(eventDelegate);
                    } 
                }

                if (HeartBeat != null)
                {
                    foreach (HeartBeatEventHandler eventDelegate in HeartBeat.GetInvocationList())
                    {
                        HeartBeat -= new HeartBeatEventHandler(eventDelegate);
                    }
                }
            }
            base.Dispose(disposing);
        }

        #endregion

    }
}
