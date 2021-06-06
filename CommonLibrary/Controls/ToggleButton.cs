using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CommonLibrary.Controls
{
    [Description("按钮样式")]
    public enum CheckStyle
    {
        style1 = 0,
        style2 = 1,
        style3 = 2,
        style4 = 3,
        style5 = 4,
        style6 = 5
    };

    public partial class ToggleButton : UserControl
    {
        private bool m_bIsCheck = false;
        private CheckStyle m_checkStyle = CheckStyle.style1;

        public ToggleButton()
        {
            InitializeComponent();

            //设置Style支持透明背景色并且双缓冲
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.SetStyle(ControlStyles.Selectable, true);
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.BackColor = Color.Transparent;

            this.Cursor = Cursors.Hand;
            this.Size = new Size(87, 27);
        }

        [Description("指示控件是否处于选择状态")]
        public bool Checked
        {
            set { m_bIsCheck = value; this.Invalidate(); }
            get { return m_bIsCheck; }
        }

        [Description("指示控件显示样式")]
        public CheckStyle CheckStyle
        {
            set { m_checkStyle = value; this.Invalidate(); }
            get { return m_checkStyle; }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Bitmap bitMapOn = null;
            Bitmap bitMapOff = null;

            switch (m_checkStyle)
            {
                case CheckStyle.style1:
                    bitMapOn = Properties.Resources.btncheckon1;
                    bitMapOff = Properties.Resources.btncheckoff1;
                    break;
                case CheckStyle.style2:
                    bitMapOn = Properties.Resources.btncheckon2;
                    bitMapOff = Properties.Resources.btncheckoff2;
                    break;
                case CheckStyle.style3:
                    bitMapOn = Properties.Resources.btncheckon3;
                    bitMapOff = Properties.Resources.btncheckoff3;
                    break;
                case CheckStyle.style4:
                    bitMapOn = Properties.Resources.btncheckon4;
                    bitMapOff = Properties.Resources.btncheckoff4;
                    break;
                case CheckStyle.style5:
                    bitMapOn = Properties.Resources.btncheckon5;
                    bitMapOff = Properties.Resources.btncheckoff5;
                    break;
                case CheckStyle.style6:
                    bitMapOn = Properties.Resources.btncheckon6;
                    bitMapOff = Properties.Resources.btncheckoff6;
                    break;
                default:
                    break;
            }

            Graphics g = e.Graphics;
            Rectangle rec = new Rectangle(0, 0, this.Size.Width, this.Size.Height);

            g.DrawImage(m_bIsCheck ? bitMapOn : bitMapOff, rec);
        }

        private void ToggleButton_Click(object sender, EventArgs e)
        {
            m_bIsCheck = !m_bIsCheck;
            this.Invalidate();
        }
    }
}
