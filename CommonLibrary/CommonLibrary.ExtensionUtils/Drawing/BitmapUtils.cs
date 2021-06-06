using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.IO;
using PixelFormat = System.Drawing.Imaging.PixelFormat;
using System.Drawing.Text;

namespace CommonLibrary.ExtensionUtils
{
    public static class BitmapUtils
    {
        private const string KERNEL32 = "Kernel32.dll";

        [DllImport(KERNEL32)]
        public extern static void CopyMemory(IntPtr dest, IntPtr src, uint length);

        /// <summary>
        /// 获取图像的文档尺寸，单位0.1mm
        /// </summary>
        /// <param name="bitmap"></param>
        /// <param name="decimals">小数点位数</param>
        /// <returns></returns>
        public static SizeF GetSizeF(this Bitmap bitmap)
        {
            var imgWidth = bitmap.Width / bitmap.HorizontalResolution * 254;
            var imgHeight = bitmap.Height / bitmap.VerticalResolution * 254;
            return new SizeF(imgWidth, imgHeight);
        }

        /// <summary>
        /// 图像旋转
        /// </summary>
        /// <param name="src">原始图像</param>
        /// <param name="degree">旋转角度</param>
        public static Bitmap Rotate(this Bitmap bitmap, float angle, Color backColor)
        {
            int width = bitmap.Width + 2;
            int height = bitmap.Height + 2;

            PixelFormat pf;

            if (backColor == Color.Transparent)
            {
                pf = PixelFormat.Format32bppArgb;
            }
            else
            {
                pf = bitmap.PixelFormat;
            }
            Bitmap temp = new Bitmap(width, height, pf);
            temp.SetResolution(bitmap.HorizontalResolution, bitmap.VerticalResolution);
            Graphics graphics = Graphics.FromImage(temp);
            graphics.Clear(backColor);
            graphics.DrawImage(bitmap, 1, 1);
            graphics.Dispose();

            GraphicsPath path = new GraphicsPath();
            path.AddRectangle(new RectangleF(0f, 0f, width, height));
            Matrix mtrx = new Matrix();
            mtrx.Rotate(angle);
            RectangleF rct = path.GetBounds(mtrx);

            Bitmap dst = new Bitmap((int)rct.Width, (int)rct.Height, pf);
            dst.SetResolution(bitmap.HorizontalResolution, bitmap.VerticalResolution);
            graphics = Graphics.FromImage(dst);
            graphics.Clear(backColor);
            graphics.TranslateTransform(width / 2, height / 2);
            graphics.RotateTransform(angle);
            graphics.TranslateTransform(-width / 2, -height / 2);
            graphics.InterpolationMode = InterpolationMode.HighQualityBilinear;
            graphics.DrawImage(temp, 0, 0);
            graphics.Dispose();

            temp.Dispose();
            return dst;
        }

        /// <summary>
        /// 获取旋转图像
        /// </summary>
        /// <param name="srcImage"></param>
        /// <param name="angle"></param>
        /// <param name="smoothingMode"></param>
        /// <param name="interpolationMode"></param>
        /// <returns></returns>
        public static Bitmap GetRotateImage(Bitmap srcImage, float angle,
            SmoothingMode smoothingMode = SmoothingMode.AntiAlias,
            InterpolationMode interpolationMode = InterpolationMode.NearestNeighbor)
        {
            angle = angle % 360;
            //弧度转换
            double radian = angle * Math.PI / 180.0;
            double cos = Math.Cos(radian);
            double sin = Math.Sin(radian);
            //原图的宽和高
            int w = srcImage.Width;
            int h = srcImage.Height;
            int W = (int)(Math.Max(Math.Abs(w * cos - h * sin), Math.Abs(w * cos + h * sin)));
            int H = (int)(Math.Max(Math.Abs(w * sin - h * cos), Math.Abs(w * sin + h * cos)));
            //目标位图
            var dsImage = new Bitmap(W, H);
            dsImage.SetResolution(srcImage.HorizontalResolution, srcImage.VerticalResolution);
            var g = Graphics.FromImage(dsImage);
            g.InterpolationMode = interpolationMode;
            g.SmoothingMode = smoothingMode;
            //计算偏移量
            Point Offset = new Point((W - w) / 2, (H - h) / 2);
            //构造图像显示区域：让图像的中心与窗口的中心点一致
            Rectangle rect = new Rectangle(Offset.X, Offset.Y, w, h);
            Point center = new Point(rect.X + rect.Width / 2, rect.Y + rect.Height / 2);
            g.TranslateTransform(center.X, center.Y);
            g.RotateTransform(angle);
            //恢复图像在水平和垂直方向的平移
            g.TranslateTransform(-center.X, -center.Y);
            var att = new ImageAttributes();

            g.DrawImage(srcImage, rect);
            //重至绘图的所有变换
            g.ResetTransform();
            g.Save();
            g.Dispose();
            return dsImage;
        }

        /// <summary>
        /// 获取旋转矩形
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="angle"></param>
        /// <returns></returns>
        public static Rectangle GetRotateRectangle(int width, int height, float angle)
        {
            double radian = angle * Math.PI / 180; ;
            double cos = Math.Cos(radian);
            double sin = Math.Sin(radian);
            //只需要考虑到第四象限和第三象限的情况取大值(中间用绝对值就可以包括第一和第二象限)
            int newWidth = (int)(Math.Max(Math.Abs(width * cos - height * sin), Math.Abs(width * cos + height * sin)));
            int newHeight = (int)(Math.Max(Math.Abs(width * sin - height * cos), Math.Abs(width * sin + height * cos)));
            return new Rectangle(0, 0, newWidth, newHeight);
        }

        /// <summary>
        /// 转换为透明色
        /// </summary>
        /// <param name="bmp"></param>
        /// <param name="foreColor"></param>
        public static void ConvertToTransparent(this Bitmap bmp, Color foreColor)
        {
            var xDpi = bmp.HorizontalResolution;
            var yDpi = bmp.VerticalResolution;
            bmp.MakeTransparent(foreColor);
            bmp.SetResolution(xDpi, yDpi);
        }

        public static Bitmap RemoveOutterSpace(this Bitmap bitmap, Color backColor)
        {
            var bmpExt = new LockBitmap(bitmap);
            bmpExt.LockBits();
            //上左右下
            int top = 0, left = 0, right = bmpExt.Width, bottom = bmpExt.Height;

            //寻找最上面的标线,从左(0)到右，从上(0)到下
            for (int y = 0; y < bmpExt.Height; y++)//行
            {
                var found = false;
                for (int x = 0; x < bmpExt.Width; x++)//列
                {
                    var pix = bmpExt.GetPixel(x, y);
                    if (!pix.IsEqual(backColor))
                    {
                        top = y;
                        found = true;
                        break;
                    }
                }
                if (found)
                    break;
            }

            //寻找最左边的标线，从上（top位）到下，从左到右
            for (int x = 0; x < bmpExt.Width; x++)//列
            {
                bool find = false;
                for (int y = top; y < bmpExt.Height; y++)//行
                {
                    var pix = bmpExt.GetPixel(x, y);
                    if (!pix.IsEqual(backColor))
                    {
                        left = x;
                        find = true;
                        break;
                    }
                }
                if (find)
                    break;
            }

            //寻找最下边标线，从下到上，从左到右
            for (int y = bmpExt.Height - 1; y >= 0; y--)//行
            {
                bool find = false;
                for (int x = left; x < bmpExt.Width; x++)//列
                {
                    var pix = bmpExt.GetPixel(x, y);
                    if (!pix.IsEqual(backColor))
                    {
                        bottom = y;
                        find = true;
                        break;
                    }
                }
                if (find)
                    break;
            }

            //寻找最右边的标线，从上到下，从右往左
            for (int x = bmpExt.Width - 1; x >= 0; x--)//列
            {
                bool find = false;
                for (int y = 0; y <= bottom; y++)//行
                {
                    var pix = bmpExt.GetPixel(x, y);
                    if (!pix.IsEqual(backColor))
                    {
                        right = x;
                        find = true;
                        break;
                    }
                }
                if (find)
                    break;
            }

            //克隆位图对象的一部分。
            var cloneRect = new Rectangle(left, top, right - left + 2, bottom - top + 1);
            if (cloneRect.Width > bitmap.Width)
            {
                cloneRect.Width = bitmap.Width;
            }
            if (cloneRect.Height > bitmap.Height)
            {
                cloneRect.Height = bitmap.Height;
            }
            var cloneBitmap = bitmap.Clone(cloneRect, bitmap.PixelFormat);
            bmpExt.UnlockBits();
            bitmap.Dispose();
            return cloneBitmap;
        }

        /// <summary>
        /// 获取截取部分图像
        /// </summary>
        /// <param name="src"></param>
        /// <param name="dstRect"></param>
        /// <returns></returns>
        public static Bitmap GetSubPart(this Bitmap src, Rectangle dstRect)
        {

            if (dstRect.Width > src.Width)
            {
                dstRect.Width = src.Width;
            }
            if (dstRect.Height > src.Height)
            {
                dstRect.Height = src.Height;
            }
            return src.Clone(dstRect, src.PixelFormat);
        }

        /// <summary>
        /// 获取每行字节数
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static int GetPerLineByteCount(this Bitmap source)
        {
            var name = source.PixelFormat.GetEnumName();
            if (name.StartsWith("Format"))
            {
                var end = name.IndexOf("bpp");
                var biBitCount = name.Substring(6, end - 6).ToInt32();
                if (biBitCount < 8)
                {
                    return (source.Width * biBitCount + 31) / 32 * 4;
                }
                else
                {
                    return (((source.Width * biBitCount) / 8 + 3) / 4) * 4;
                }
            }
            return 0;
        }

        /// <summary>
        /// 转换为1位索引位图
        /// </summary>
        /// <param name="pimage"></param>
        /// <param name="HeightScale"></param>
        /// <returns></returns>
        public static Bitmap ConvertTo1BppIndexed(this Bitmap pimage, float HeightScale)
        {
            Bitmap source = null;

            // If original bitmap is not already in 32 BPP, ARGB format, then convert
            if (pimage.PixelFormat != PixelFormat.Format32bppArgb)
            {
                source = new Bitmap(pimage.Width, pimage.Height, PixelFormat.Format32bppArgb);
                source.SetResolution(600, 600 * HeightScale);
                using (Graphics g = Graphics.FromImage(source))
                {
                    g.DrawImageUnscaled(pimage, 0, 0);
                }
            }
            else
            {
                source = pimage;
            }
            //source.Save($"D:/SiDaTest/Format32bppArgbaaa.bmp");
            // Lock source bitmap in memory
            BitmapData sourceData = source.LockBits(new Rectangle(0, 0, source.Width, source.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

            // Copy image data to binary array
            int imageSize = sourceData.Stride * sourceData.Height;
            byte[] sourceBuffer = new byte[imageSize];
            Marshal.Copy(sourceData.Scan0, sourceBuffer, 0, imageSize);

            // Unlock source bitmap
            source.UnlockBits(sourceData);

            // Create destination bitmap
            Bitmap destination = new Bitmap(source.Width, source.Height, PixelFormat.Format1bppIndexed);
            destination.SetResolution(source.HorizontalResolution, source.VerticalResolution * HeightScale);
            //destination.SetResolution(source.HorizontalResolution, HorizontalDpi);
            // Lock destination bitmap in memory
            BitmapData destinationData = destination.LockBits(new Rectangle(0, 0, destination.Width, destination.Height), ImageLockMode.WriteOnly, PixelFormat.Format1bppIndexed);

            // Create destination buffer
            imageSize = destinationData.Stride * destinationData.Height;
            byte[] destinationBuffer = new byte[imageSize];

            int sourceIndex = 0;
            int destinationIndex = 0;
            int pixelTotal = 0;
            byte destinationValue = 0;
            int pixelValue = 128;
            int height = source.Height;
            int width = source.Width;
            int threshold = 500;

            // Iterate lines
            for (int y = 0; y < height; y++)
            {
                sourceIndex = y * sourceData.Stride;
                destinationIndex = y * destinationData.Stride;
                destinationValue = 0;
                pixelValue = 128;

                // Iterate pixels
                for (int x = 0; x < width; x++)
                {
                    // Compute pixel brightness (i.e. total of Red, Green, and Blue values)
                    pixelTotal = sourceBuffer[sourceIndex + 1] + sourceBuffer[sourceIndex + 2] + sourceBuffer[sourceIndex + 3];
                    if (pixelTotal > threshold)
                    {
                        destinationValue += (byte)pixelValue;
                    }
                    if (pixelValue == 1)
                    {
                        destinationBuffer[destinationIndex] = destinationValue;
                        destinationIndex++;
                        destinationValue = 0;
                        pixelValue = 128;
                    }
                    else
                    {
                        pixelValue >>= 1;
                    }
                    sourceIndex += 4;
                }
                if (pixelValue != 128)
                {
                    destinationBuffer[destinationIndex] = destinationValue;
                }
            }

            // Copy binary image data to destination bitmap
            Marshal.Copy(destinationBuffer, 0, destinationData.Scan0, imageSize);

            // Unlock destination bitmap
            destination.UnlockBits(destinationData);

            // Dispose of source if not originally supplied bitmap
            if (source != pimage)
            {
                source.Dispose();
            }

            // Return
            return destination;
        }

        /// <summary>
        /// 转换为1位索引位图
        /// </summary>
        /// <param name="src">原图</param>
        /// <param name="grayThreshold">灰度阈值，值越大，黑度扩散越大</param>
        /// <returns></returns>
        public static Bitmap ConvertTo1BppIndexed(this Bitmap src, int grayThreshold = 0)
        {
            Bitmap source = null;

            source = new Bitmap(src.Width, src.Height, PixelFormat.Format32bppArgb);
            source.SetResolution(src.HorizontalResolution, src.VerticalResolution);
            using (Graphics g = Graphics.FromImage(source))
            {
                g.Clear(Color.White);
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.TextRenderingHint = TextRenderingHint.SingleBitPerPixelGridFit;
                g.DrawImageUnscaled(src, 0, 0);
            }

            var sourceRect = new Rectangle(0, 0, source.Width, source.Height);
            var sourceData = source.LockBits(sourceRect, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            var imageSize = sourceData.Stride * sourceData.Height;
            var srcBuffer = new byte[imageSize];
            Marshal.Copy(sourceData.Scan0, srcBuffer, 0, imageSize);
            source.UnlockBits(sourceData);
            var dest = new Bitmap(source.Width, source.Height, PixelFormat.Format1bppIndexed);
            dest.SetResolution(src.HorizontalResolution, src.VerticalResolution);
            var destRect = new Rectangle(0, 0, dest.Width, dest.Height);
            var destData = dest.LockBits(destRect, ImageLockMode.WriteOnly, PixelFormat.Format1bppIndexed);
            imageSize = destData.Stride * destData.Height;
            var destBuffer = new byte[imageSize];
            int srcIndex, destIndex, pixelTotal;
            byte destValue = 0;
            int pixelValue = 128;
            var height = source.Height;
            var width = source.Width;
            var threshold = 500 + grayThreshold;
            for (int y = 0; y < height; y++)
            {
                srcIndex = y * sourceData.Stride;
                destIndex = y * destData.Stride;
                destValue = 0;
                pixelValue = 128;

                for (int x = 0; x < width; x++)
                {
                    pixelTotal = srcBuffer[srcIndex + 1] + srcBuffer[srcIndex + 2] + srcBuffer[srcIndex + 3];
                    if (pixelTotal > threshold)
                    {
                        destValue += (byte)pixelValue;
                    }
                    if (pixelValue == 1)
                    {
                        destBuffer[destIndex] = destValue;
                        destIndex++;
                        destValue = 0;
                        pixelValue = 128;
                    }
                    else
                    {
                        pixelValue >>= 1;
                    }
                    srcIndex += 4;
                }
                if (pixelValue != 128)
                {
                    destBuffer[destIndex] = destValue;
                }
            }
            Marshal.Copy(destBuffer, 0, destData.Scan0, imageSize);
            dest.UnlockBits(destData);
            if (source != src)
            {
                source.Dispose();
            }
            return dest;
        }

        /// <summary>
        /// 获取Rgb格式字节数组
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static byte[] GetRgbBuffer(this Bitmap source)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                source.Save(stream, ImageFormat.Bmp);
                byte[] data = new byte[stream.Length - 62];
                stream.Position = 62;
                stream.Read(data, 0, data.Length);
                return data;
            }
        }

        public static Bitmap SetMargin(this Bitmap source, int margin, Color marginColor)
        {
            var bmp = new Bitmap(source.Width + margin, source.Height + margin);
            var g = Graphics.FromImage(bmp);
            g.Clear(marginColor);
            var centerPoint = new Point(bmp.Width / 2 - source.Width / 2, bmp.Height / 2 - source.Height / 2);
            g.DrawImage(source, centerPoint);
            g.Save();
            source.Dispose();
            return bmp;
        }

        /// <summary>
        /// 缩放图片
        /// </summary>
        /// <param name="bitmap">原图片</param>
        /// <param name="width">新图片宽度</param>
        /// <param name="height">新图片高度</param>
        /// <returns>新图片</returns>
        public static Bitmap ScaleToSize(this Bitmap bitmap, int width, int height)
        {
            if (bitmap.Width == width && bitmap.Height == height)
            {
                return bitmap;
            }

            var scaledBitmap = new Bitmap(width, height);

            using (var g = Graphics.FromImage(scaledBitmap))
            {
                g.InterpolationMode = InterpolationMode.NearestNeighbor;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.TextRenderingHint = TextRenderingHint.SingleBitPerPixelGridFit;
                g.DrawImage(bitmap, 0, 0, width, height);
            }
            scaledBitmap.SetResolution(bitmap.HorizontalResolution, bitmap.VerticalResolution);
            return scaledBitmap;
        }

        /// <summary>
        /// 缩放图片
        /// </summary>
        /// <param name="bitmap">原图片</param>
        /// <param name="size">新图片大小</param>
        /// <returns>新图片</returns>
        public static Bitmap ScaleToSize(this Bitmap bitmap, Size size)
        {
            return bitmap.ScaleToSize(size.Width, size.Height);
        }

        /// <summary>
        /// 按比例来缩放
        /// </summary>
        /// <param name="bitmap">原图</param>
        /// <param name="ratio">ratio大于1,则放大;否则缩小</param>
        /// <returns>新图片</returns>
        public static Bitmap ScaleToSize(this Bitmap bitmap, float ratio)
        {
            return bitmap.ScaleToSize((int)(bitmap.Width * ratio), (int)(bitmap.Height * ratio));
        }

        /// <summary>
        /// 按给定长度/宽度等比例缩放
        /// </summary>
        /// <param name="bitmap">原图</param>
        /// <param name="width">新图片宽度</param>
        /// <param name="height">新图片高度</param>
        /// <returns>新图片</returns>
        public static Bitmap ScaleProportional(this Bitmap bitmap, int width, int height)
        {
            float proportionalWidth, proportionalHeight;

            if (width.Equals(0))
            {
                proportionalWidth = ((float)height) / bitmap.Size.Height * bitmap.Width;
                proportionalHeight = height;
            }
            else if (height.Equals(0))
            {
                proportionalWidth = width;
                proportionalHeight = ((float)width) / bitmap.Size.Width * bitmap.Height;
            }
            else if (((float)width) / bitmap.Size.Width * bitmap.Size.Height <= height)
            {
                proportionalWidth = width;
                proportionalHeight = ((float)width) / bitmap.Size.Width * bitmap.Height;
            }
            else
            {
                proportionalWidth = ((float)height) / bitmap.Size.Height * bitmap.Width;
                proportionalHeight = height;
            }

            return bitmap.ScaleToSize((int)proportionalWidth, (int)proportionalHeight);
        }

        /// <summary>
        /// 按给定长度/宽度缩放,同时可以设置背景色
        /// </summary>
        /// <param name="bitmap">原图</param>
        /// <param name="backgroundColor">背景色</param>
        /// <param name="width">新图片宽度</param>
        /// <param name="height">新图片高度</param>
        /// <returns></returns>
        public static Bitmap ScaleToSize(this Bitmap bitmap, Color backgroundColor, int width, int height)
        {
            var scaledBitmap = new Bitmap(width, height);
            using (var g = Graphics.FromImage(scaledBitmap))
            {
                g.Clear(backgroundColor);

                var proportionalBitmap = bitmap.ScaleProportional(width, height);

                var imagePosition = new Point((int)((width - proportionalBitmap.Width) / 2m), (int)((height - proportionalBitmap.Height) / 2m));
                g.DrawImage(proportionalBitmap, imagePosition);
            }

            return scaledBitmap;
        }

        /// <summary>
        /// 更改像素格式
        /// </summary>
        /// <param name="bitmap"></param>
        /// <param name="HorizontalDpi"></param>
        /// <returns></returns>
        public static Bitmap ChangePixelFormat(this Bitmap bitmap, float HorizontalDpi)
        {
            Bitmap source = null;

            // If original bitmap is not already in 32 BPP, ARGB format, then convert
            if (bitmap.PixelFormat != PixelFormat.Format32bppArgb)
            {
                source = new Bitmap(bitmap.Width, bitmap.Height, PixelFormat.Format32bppArgb);
                source.SetResolution(HorizontalDpi, bitmap.VerticalResolution);
                using (Graphics g = Graphics.FromImage(source))
                {
                    g.DrawImageUnscaled(bitmap, 0, 0);
                }
                bitmap = source;
            }

            var pixelFormat = PixelFormat.Format32bppArgb;
            Rectangle rect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);

            BitmapData bitmapData = bitmap.LockBits(rect, ImageLockMode.ReadOnly, pixelFormat);
            try
            {
                Bitmap convertedBitmap = new Bitmap(bitmap.Width, bitmap.Height, pixelFormat);
                BitmapData convertedBitmapData = convertedBitmap.LockBits(rect, ImageLockMode.WriteOnly, pixelFormat);
                try
                {
                    CopyMemory(convertedBitmapData.Scan0, bitmapData.Scan0, (uint)bitmapData.Stride * (uint)bitmapData.Height);
                }
                finally
                {
                    convertedBitmap.UnlockBits(convertedBitmapData);
                }

                return convertedBitmap;
            }
            finally
            {
                bitmap.UnlockBits(bitmapData);
            }
        }
    }
}
