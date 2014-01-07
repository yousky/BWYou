using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using NLog;
using Amib.Threading;

namespace BWYou.Web
{
    /// <summary>
    /// This class is used to execute an operation in a separate thread.
    /// </summary>
    public class BackgroundWorker
    {
        /// <summary>
        /// This thread is used to run the operation in the background.
        /// </summary>
        private SmartThreadPool _theradPool = new SmartThreadPool();
        private BlockingQueue<System.Action> _actions = new BlockingQueue<System.Action>();

        private static Logger GetLogger() { return LogManager.GetCurrentClassLogger(); }

        public BackgroundWorker()
        {
            GetLogger().Info("BackgroundWorker is Started.");
            Thread innerThread = new Thread(this.DoWorks);
            innerThread.Start();
        }

        public void DoWorks()
        {
            GetLogger().Info("BackgroundWorker Thread is Started.");
            Thread.CurrentThread.Name = this.GetType().Name;

            while (true)
            {
                try
                {
                    System.Action action = _actions.Dequeue();
                    GetLogger().Info("Action Dequeued");
                    if (action != null)
                    {
                        _theradPool.QueueWorkItem(() =>
                        {
                            try
                            {
                                action();
                            }
                            catch (Exception ex)
                            {
                                GetLogger().Fatal(ex);
                            }
                        });
                    }
                }
                catch (ThreadAbortException ex)
                {
                    GetLogger().Info("ThreadAbortException: " + ex.Message);
                    break;
                }
                catch (Exception ex)
                {
                    GetLogger().Fatal(ex);
                }
            }

            GetLogger().Info("BackgroundWorker Thread is Stopped.");
        }

        public void AddWork(System.Action action)
        {
            _actions.Enqueue(action);
        }
    }
}
