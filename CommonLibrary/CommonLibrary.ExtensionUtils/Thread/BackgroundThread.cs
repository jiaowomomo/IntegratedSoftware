using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CommonLibrary.ExtensionUtils
{
    public abstract class BackgroundThread
    {
        protected Thread m_thread = null;
        protected volatile bool m_bIsRunning = false;

        public bool IsRunning { get => m_thread != null && m_bIsRunning; }

        public bool Start(object parameter)
        {
            if (IsRunning || m_thread == null)
            {
                return false;
            }

            StartBefore();
            m_thread.Start(parameter);
            m_bIsRunning = true;
            return true;
        }

        public void Resume()
        {
            if (m_thread != null)
            {
                m_thread.Resume();
            }
        }

        public void Suspend()
        {
            if (m_thread != null)
            {
                m_thread.Suspend();
            }
        }

        public Thread Exit(bool isWait = true, Action waitBeforeAction = null)
        {
            if (m_thread != null)
            {
                Thread thread = m_thread;
                if (IsRunning)
                {
                    try
                    {
                        m_bIsRunning = false;
                        if (isWait)
                        {
                            waitBeforeAction?.Invoke();
                            m_thread.Join();
                        }
                    }
                    catch (Exception ex)
                    { }
                    ExitAfter();
                }

                m_thread = null;
                return thread;
            }
            return null;
        }

        protected void SetThreadFunc(ParameterizedThreadStart parameterizedThreadStart, string threadName = "")
        {
            m_thread = new Thread(parameterizedThreadStart);
            if (string.IsNullOrEmpty(threadName))
                m_thread.Name = GetType().Name;
            else
                m_thread.Name = threadName;
            m_thread.IsBackground = true;
        }

        protected virtual void StartBefore()
        {
        }

        protected virtual void ExitAfter()
        {
        }
    }
}
