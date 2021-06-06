using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CommonLibrary.ExtensionUtils
{
    public class DoubleClickTimer : Timer
    {
        private int m_milliseconds = 0;
        private bool m_bIsFirstClick = true;
        private bool m_bIsDoubleClick = false;
        private Rectangle m_doubleClickRectangle = new Rectangle();
        private MouseEventArgs m_currentMouseEventArgs = null;

        public event MouseEventHandler SingleClick = null;
        public event MouseEventHandler DoubleClick = null;

        public DoubleClickTimer()
        {
            InitTimer();
        }

        private void InitTimer()
        {
            this.Interval = 100;
            this.Tick += DoubleClickTimer_Tick;
        }

        private void DoubleClickTimer_Tick(object sender, EventArgs e)
        {
            m_milliseconds += 100;
            if (m_milliseconds >= SystemInformation.DoubleClickTime)
            {
                this.Stop();

                if (m_bIsDoubleClick)
                {
                    DoubleClick?.Invoke(this, m_currentMouseEventArgs);
                }
                else
                {
                    SingleClick?.Invoke(this, m_currentMouseEventArgs);
                }

                m_bIsFirstClick = true;
                m_bIsDoubleClick = false;
                m_milliseconds = 0;
            }
        }

        public void MouseDown(object sender, MouseEventArgs e)
        {
            m_currentMouseEventArgs = e;
            if (m_bIsFirstClick)
            {
                m_bIsFirstClick = false;
                m_doubleClickRectangle = new Rectangle(
                    e.X - (SystemInformation.DoubleClickSize.Width / 2),
                    e.Y - (SystemInformation.DoubleClickSize.Height / 2),
                    SystemInformation.DoubleClickSize.Width,
                    SystemInformation.DoubleClickSize.Height);
                this.Start();
            }
            else
            {
                if (m_doubleClickRectangle.Contains(e.Location) && m_milliseconds < SystemInformation.DoubleClickTime)
                {
                    m_bIsDoubleClick = true;
                }
            }
        }
    }
}
