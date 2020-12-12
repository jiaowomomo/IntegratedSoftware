using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;

namespace AutomationSystem.UI
{
    public partial class ImageBoxEx : UserControl
    {
        private bool IsMove = false;
        private Point initClickPoint;
        private int nBoxInitialWidth;//图片容器初始宽度
        private int nBoxInitialHeight;//图片容器初始高度
        private int nBlackLeftWidth;//Zoom模式下左偏移
        private int nBlackTopHeight;//Zoom模式下上偏移
        private Point boxInitialLocation;//图片容器初始左上角位置
        private double dbOriginalX;//原图上像素位置X
        private double dbOriginalY;//原图上像素位置Y
        private double dbScaleX;//原图与实际显示缩放比例X
        private double dbScaleY;//原图与实际显示缩放比例Y

        public ImageBoxEx()
        {
            InitializeComponent();
            Init();
        }

        private void pbCamera_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Middle)
            {
                if (!IsMove)
                {
                    IsMove = true;
                    initClickPoint = e.Location;
                }
            }
        }

        private void pbCamera_MouseMove(object sender, MouseEventArgs e)
        {
            if (pbCamera.Image != null)
            {
                //移动图片
                if (IsMove)
                {
                    if (pbCamera.Width > nBoxInitialWidth)
                    {
                        int offsetX = e.X - initClickPoint.X;
                        int offsetY = e.Y - initClickPoint.Y;
                        pbCamera.Location = new Point(pbCamera.Location.X + offsetX, pbCamera.Location.Y + offsetY);
                    }
                }
                if (pbCamera.SizeMode == PictureBoxSizeMode.StretchImage)
                {
                    dbOriginalX = (double)e.X * dbScaleX;
                    dbOriginalY = (double)e.Y * dbScaleY;
                }
                else if (pbCamera.SizeMode == PictureBoxSizeMode.Zoom)
                {
                    dbOriginalX = (double)(e.X - nBlackLeftWidth) * dbScaleX;
                    dbOriginalY = (double)(e.Y - nBlackTopHeight) * dbScaleY;
                }
            }
        }

        private void pbCamera_MouseUp(object sender, MouseEventArgs e)
        {
            if (IsMove)
            {
                IsMove = false;
            }
        }

        private void pbCamera_MouseWheel(object sender, MouseEventArgs e)
        {
            if (pbCamera.Image != null)
            {
                //放大图片
                if (e.Delta > 0)
                {
                    pbCamera.Size = new Size(Convert.ToInt32(pbCamera.Width * 1.1), Convert.ToInt32(pbCamera.Height * 1.1));
                    pbCamera.Location = new Point(pbCamera.Location.X - (int)(e.X * 1.1 - e.X), pbCamera.Location.Y - (int)(e.Y * 1.1 - e.Y));
                }
                //缩小图片
                else
                {
                    if (Convert.ToInt32(pbCamera.Width * 0.9) >= nBoxInitialWidth)
                    {
                        pbCamera.Size = new Size(Convert.ToInt32(pbCamera.Width * 0.9), Convert.ToInt32(pbCamera.Height * 0.9));
                        pbCamera.Location = new Point(pbCamera.Location.X - (int)(e.X * 0.9 - e.X), pbCamera.Location.Y - (int)(e.Y * 0.9 - e.Y));
                    }
                    else
                    {
                        pbCamera.Size = new Size(nBoxInitialWidth, nBoxInitialHeight);
                        pbCamera.Location = boxInitialLocation;
                    }
                }
                SetScale();
            }
        }

        private void SetScale()
        {
            if (pbCamera.SizeMode == PictureBoxSizeMode.StretchImage)
            {
                dbScaleX = (double)pbCamera.Image.Width / (double)pbCamera.Width;
                dbScaleY = (double)pbCamera.Image.Height / (double)pbCamera.Height;
            }
            else if (pbCamera.SizeMode == PictureBoxSizeMode.Zoom)
            {
                PropertyInfo rectangleProperty = this.pbCamera.GetType().GetProperty("ImageRectangle", BindingFlags.Instance | BindingFlags.NonPublic);
                Rectangle rectangle = (Rectangle)rectangleProperty.GetValue(this.pbCamera, null);

                int currentWidth = rectangle.Width;
                int currentHeight = rectangle.Height;

                dbScaleX = (double)pbCamera.Image.Height / (double)currentHeight;
                dbScaleY = dbScaleX;

                nBlackLeftWidth = (currentWidth == this.pbCamera.Width) ? 0 : (this.pbCamera.Width - currentWidth) / 2;
                nBlackTopHeight = (currentHeight == this.pbCamera.Height) ? 0 : (this.pbCamera.Height - currentHeight) / 2;
            }
        }

        private void ImageBoxEx_Resize(object sender, EventArgs e)
        {
            pbCamera.Size = this.Size;
            pbCamera.Location = new Point(0, 0);
        }

        private void Init()
        {
            this.pbCamera.MouseWheel += new MouseEventHandler(pbCamera_MouseWheel);
            nBoxInitialWidth = this.Width;
            nBoxInitialHeight = this.Height;
            boxInitialLocation = pbCamera.Location;
        }

        public void SetImage(Image image)
        {
            pbCamera.Image = image;
            nBoxInitialWidth = this.Width;
            nBoxInitialHeight = this.Height;
            boxInitialLocation = pbCamera.Location;
        }
    }
}
