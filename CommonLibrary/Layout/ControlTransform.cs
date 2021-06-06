using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CommonLibrary.Layout
{
    public enum EnumMousePointPosition
    {
        MouseSizeNone = 0, //无    
        MouseSizeRight = 1, //拉伸右边框    
        MouseSizeLeft = 2, //拉伸左边框    
        MouseSizeBottom = 3, //拉伸下边框    
        MouseSizeTop = 4, //拉伸上边框    
        MouseSizeTopLeft = 5, //拉伸左上角    
        MouseSizeTopRight = 6, //拉伸右上角    
        MouseSizeBottomLeft = 7, //拉伸左下角    
        MouseSizeBottomRight = 8, //拉伸右下角    
        MouseDrag = 9   //鼠标拖动    
    }

    public class ControlTransform
    {
        private const int PIXEL_OFFSET = 5;
        private const int MINWIDTH = 10;
        private const int MINHEIGHT = 10;

        private EnumMousePointPosition m_mousePointPosition;
        private Point m_recordPoint = new Point();
        private Point m_currentPoint = new Point();

        public ControlTransform()
        {
        }

        public void MyMouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            m_recordPoint.X = e.X;
            m_recordPoint.Y = e.Y;
            m_currentPoint.X = e.X;
            m_currentPoint.Y = e.Y;
        }

        public void MyMouseLeave(object sender, System.EventArgs e)
        {
            Control ctrl = sender as Control;
            m_mousePointPosition = EnumMousePointPosition.MouseSizeNone;
            ctrl.Cursor = Cursors.Arrow;
        }

        /// <summary>
        /// 根据鼠标位置，判断光标类型
        /// </summary>
        /// <param name="size"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        private EnumMousePointPosition MousePointPosition(Size size, System.Windows.Forms.MouseEventArgs e)
        {

            if ((e.X >= -1 * PIXEL_OFFSET) | (e.X <= size.Width) | (e.Y >= -1 * PIXEL_OFFSET) | (e.Y <= size.Height))
            {
                if (e.X < PIXEL_OFFSET)
                {
                    if (e.Y < PIXEL_OFFSET)
                    {
                        return EnumMousePointPosition.MouseSizeTopLeft;
                    }
                    else
                    {
                        if (e.Y > -1 * PIXEL_OFFSET + size.Height)
                        {
                            return EnumMousePointPosition.MouseSizeBottomLeft;
                        }
                        else
                        {
                            return EnumMousePointPosition.MouseSizeLeft;
                        }
                    }
                }
                else
                {
                    if (e.X > -1 * PIXEL_OFFSET + size.Width)
                    {
                        if (e.Y < PIXEL_OFFSET)
                        {
                            return EnumMousePointPosition.MouseSizeTopRight;
                        }
                        else
                        {
                            if (e.Y > -1 * PIXEL_OFFSET + size.Height)
                            {
                                return EnumMousePointPosition.MouseSizeBottomRight;
                            }
                            else
                            {
                                return EnumMousePointPosition.MouseSizeRight;
                            }
                        }
                    }
                    else
                    {
                        if (e.Y < PIXEL_OFFSET)
                        {
                            return EnumMousePointPosition.MouseSizeTop;
                        }
                        else
                        {
                            if (e.Y > -1 * PIXEL_OFFSET + size.Height)
                            {
                                return EnumMousePointPosition.MouseSizeBottom;
                            }
                            else
                            {
                                return EnumMousePointPosition.MouseDrag;
                            }
                        }
                    }
                }
            }
            else
            {
                return EnumMousePointPosition.MouseSizeNone;
            }
        }

        public void MyMouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            Control ctrl = (sender as Control);

            if (e.Button == MouseButtons.Left)
            {
                switch (m_mousePointPosition)
                {
                    case EnumMousePointPosition.MouseDrag:
                        ctrl.Left = ctrl.Left + e.X - m_recordPoint.X;
                        ctrl.Top = ctrl.Top + e.Y - m_recordPoint.Y;
                        break;
                    case EnumMousePointPosition.MouseSizeBottom:
                        ctrl.Height = ctrl.Height + e.Y - m_currentPoint.Y;
                        m_currentPoint.X = e.X;
                        m_currentPoint.Y = e.Y; //记录光标拖动的当前点    
                        break;
                    case EnumMousePointPosition.MouseSizeBottomRight:
                        ctrl.Width = ctrl.Width + e.X - m_currentPoint.X;
                        ctrl.Height = ctrl.Height + e.Y - m_currentPoint.Y;
                        m_currentPoint.X = e.X;
                        m_currentPoint.Y = e.Y; //记录光标拖动的当前点    
                        break;
                    case EnumMousePointPosition.MouseSizeRight:
                        ctrl.Width = ctrl.Width + e.X - m_currentPoint.X;
                        //       lCtrl.Height = lCtrl.Height + e.Y - p1.Y;    
                        m_currentPoint.X = e.X;
                        m_currentPoint.Y = e.Y; //记录光标拖动的当前点    
                        break;
                    case EnumMousePointPosition.MouseSizeTop:
                        ctrl.Top = ctrl.Top + (e.Y - m_recordPoint.Y);
                        ctrl.Height = ctrl.Height - (e.Y - m_recordPoint.Y);
                        break;
                    case EnumMousePointPosition.MouseSizeLeft:
                        ctrl.Left = ctrl.Left + e.X - m_recordPoint.X;
                        ctrl.Width = ctrl.Width - (e.X - m_recordPoint.X);
                        break;
                    case EnumMousePointPosition.MouseSizeBottomLeft:
                        ctrl.Left = ctrl.Left + e.X - m_recordPoint.X;
                        ctrl.Width = ctrl.Width - (e.X - m_recordPoint.X);
                        ctrl.Height = ctrl.Height + e.Y - m_currentPoint.Y;
                        m_currentPoint.X = e.X;
                        m_currentPoint.Y = e.Y; //记录光标拖动的当前点    
                        break;
                    case EnumMousePointPosition.MouseSizeTopRight:
                        ctrl.Top = ctrl.Top + (e.Y - m_recordPoint.Y);
                        ctrl.Width = ctrl.Width + (e.X - m_currentPoint.X);
                        ctrl.Height = ctrl.Height - (e.Y - m_recordPoint.Y);
                        m_currentPoint.X = e.X;
                        m_currentPoint.Y = e.Y; //记录光标拖动的当前点    
                        break;
                    case EnumMousePointPosition.MouseSizeTopLeft:
                        ctrl.Left = ctrl.Left + e.X - m_recordPoint.X;
                        ctrl.Top = ctrl.Top + (e.Y - m_recordPoint.Y);
                        ctrl.Width = ctrl.Width - (e.X - m_recordPoint.X);
                        ctrl.Height = ctrl.Height - (e.Y - m_recordPoint.Y);
                        break;
                    default:
                        break;
                }
                if (ctrl.Width < MINWIDTH) ctrl.Width = MINWIDTH;
                if (ctrl.Height < MINHEIGHT) ctrl.Height = MINHEIGHT;
            }
            else
            {
                m_mousePointPosition = MousePointPosition(ctrl.Size, e);   //判断光标的位置状态    
                switch (m_mousePointPosition)   //改变光标    
                {
                    case EnumMousePointPosition.MouseSizeNone:
                        ctrl.Cursor = Cursors.Arrow;        //箭头    
                        break;
                    case EnumMousePointPosition.MouseDrag:
                        ctrl.Cursor = Cursors.SizeAll;      //四方向    
                        break;
                    case EnumMousePointPosition.MouseSizeBottom:
                        ctrl.Cursor = Cursors.SizeNS;       //南北    
                        break;
                    case EnumMousePointPosition.MouseSizeTop:
                        ctrl.Cursor = Cursors.SizeNS;       //南北    
                        break;
                    case EnumMousePointPosition.MouseSizeLeft:
                        ctrl.Cursor = Cursors.SizeWE;       //东西    
                        break;
                    case EnumMousePointPosition.MouseSizeRight:
                        ctrl.Cursor = Cursors.SizeWE;       //东西    
                        break;
                    case EnumMousePointPosition.MouseSizeBottomLeft:
                        ctrl.Cursor = Cursors.SizeNESW;     //东北到南西    
                        break;
                    case EnumMousePointPosition.MouseSizeBottomRight:
                        ctrl.Cursor = Cursors.SizeNWSE;     //东南到西北    
                        break;
                    case EnumMousePointPosition.MouseSizeTopLeft:
                        ctrl.Cursor = Cursors.SizeNWSE;     //东南到西北    
                        break;
                    case EnumMousePointPosition.MouseSizeTopRight:
                        ctrl.Cursor = Cursors.SizeNESW;     //东北到南西    
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
