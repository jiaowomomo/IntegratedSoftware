using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CommonLibrary.Layout
{
    public class LayoutSize
    {
        public static bool m_isInitialize = false;

        private static float m_fInitWidth;
        private static float m_fInitHeight;

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="ctrl">控件</param>
        public static void Initialize(Control ctrl)
        {
            try
            {
                m_fInitWidth = ctrl.Width;
                m_fInitHeight = ctrl.Height;
                SetTag(ctrl);
                m_isInitialize = true;
            }
            catch (Exception)
            {
                m_isInitialize = false;
            }
        }

        private static void SetControls(float widthScale, float heightScale, Control ctrl)
        {
            foreach (Control con in ctrl.Controls)
            {
                string[] mytag = con.Tag.ToString().Split(new char[] { ':' });
                float scalingValue = Convert.ToSingle(mytag[0]) * widthScale;
                con.Width = (int)scalingValue;
                scalingValue = Convert.ToSingle(mytag[1]) * heightScale;
                con.Height = (int)(scalingValue);
                scalingValue = Convert.ToSingle(mytag[2]) * widthScale;
                con.Left = (int)(scalingValue);
                scalingValue = Convert.ToSingle(mytag[3]) * heightScale;
                con.Top = (int)(scalingValue);
                Single currentSize = Convert.ToSingle(mytag[4]) * Math.Min(widthScale, heightScale);
                con.Font = new System.Drawing.Font(con.Font.Name, currentSize, con.Font.Style, con.Font.Unit);
                if (con.Controls.Count > 0)
                {
                    SetControls(widthScale, heightScale, con);
                }
            }
        }

        private static void SetTag(Control ctrl)
        {
            foreach (Control con in ctrl.Controls)
            {
                con.Tag = $"{con.Width}:{con.Height}:{con.Left}:{con.Top}:{con.Font.Size}";
                if (con.Controls.Count > 0)
                    SetTag(con);
            }
        }

        /// <summary>
        /// 自适应布局
        /// </summary>
        /// <param name="ctrl">控件</param>
        public static void AdaptiveLayout(Control ctrl)
        {
            if (m_isInitialize)
            {
                float widthScale = ctrl.Width / m_fInitWidth;
                float heightScale = ctrl.Height / m_fInitHeight;
                SetControls(widthScale, heightScale, ctrl);
            }
        }
    }
}
