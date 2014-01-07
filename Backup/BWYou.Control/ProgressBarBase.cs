using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BWYou.Control
{
    /// <summary>
    /// 진행률 보여주기용 프로그레스바 기본형
    /// </summary>
    public partial class ProgressBarBase : UserControl
    {
        /// <summary>
        /// 프로그레스바 컨트롤
        /// </summary>
        public ProgressBar progressBar { get; set; }

        /// <summary>
        /// 생성자
        /// </summary>
        public ProgressBarBase()
        {
            InitializeComponent();

            this.progressBar = progBar;
        }

        #region ProgressBar

        /// <summary>
        /// 델리게이트
        /// </summary>
        /// <param name="value"></param>
        protected delegate void dProgress(int value);
        /// <summary>
        /// 쓰레드용 처리(델리게이트, 인보크)
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public void ProgressInvoke(int value)
        {
            if (progBar.InvokeRequired)
            {
                dProgress dP = new dProgress(Progress);
                progBar.Invoke(dP, value);
            }
            else
            {
                Progress(value);
            }
        }
        /// <summary>
        /// 프로그레스바에 진행률 보여라
        /// </summary>
        /// <param name="value"></param>
        public void InvokeProgress(int value)
        {
            ProgressInvoke(value);
        }
        ///
        /// <summary>
        /// 실제 작업
        /// </summary>
        /// <param name="value"></param>
        protected void Progress(int value)
        {
            progBar.Value = value;
        }

        #endregion
    }
}
