using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLibrary.ExtensionUtils
{
    public static class RectangleUtils
    {
        /// <summary>
        /// 获取旋转点数组
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="angle"></param>
        /// <returns></returns>
        public static PointF[] Rotate(this Rectangle rect, float angle)
        {
            using (var graph = new GraphicsPath())
            {
                Point Center = new Point(rect.X + rect.Width / 2, rect.Y + rect.Height / 2);
                graph.AddRectangle(rect);
                var a = -angle * (Math.PI / 180);
                var n1 = (float)Math.Cos(a);
                var n2 = (float)Math.Sin(a);
                var n3 = -(float)Math.Sin(a);
                var n4 = (float)Math.Cos(a);
                var n5 = (float)(Center.X * (1 - Math.Cos(a)) + Center.Y * Math.Sin(a));
                var n6 = (float)(Center.Y * (1 - Math.Cos(a)) - Center.X * Math.Sin(a));
                graph.Transform(new Matrix(n1, n2, n3, n4, n5, n6));
                return graph.PathPoints;
            }
        }

        /// <summary>
        /// 获取旋转点数组
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="angle"></param>
        /// <returns></returns>
        public static PointF[] Rotate(this RectangleF rect, float angle)
        {
            return new Rectangle((int)rect.X, (int)rect.Y, (int)rect.Width, (int)rect.Height).Rotate(angle);
        }

        /// <summary>
        /// 获取点数组
        /// </summary>
        /// <param name="rect"></param>
        /// <returns></returns>
        public static PointF[] ToPoints(this RectangleF rect)
        {
            var basePoint = rect.Location;
            var width = rect.Width;
            var height = rect.Height;
            return new PointF[] {
                basePoint,
                new PointF(basePoint.X + width, basePoint.Y),
                new PointF(basePoint.X, basePoint.Y + height),
            };
        }

        /// <summary>
        /// 获取旋转外边框
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="angle"></param>
        /// <returns></returns>
        public static Rectangle GetRotateOutter(this Rectangle rect, float angle)
        {
            var points = Rotate(rect, angle);
            var width = points.Max(x => x.X) - points.Min(x => x.X);
            var height = points.Max(x => x.Y) - points.Min(x => x.Y);
            return new Rectangle(rect.X, rect.Y, (int)width, (int)height);
        }

        /// <summary>
        /// 获取旋转外边框
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="angle"></param>
        /// <returns></returns>
        public static RectangleF GetRotateOutter(this RectangleF rect, float angle)
        {
            var points = Rotate(rect, angle);
            var width = points.Max(x => x.X) - points.Min(x => x.X);
            var height = points.Max(x => x.Y) - points.Min(x => x.Y);
            return new RectangleF(rect.X, rect.Y, width, height);
        }

        /// <summary>
        /// 转Point
        /// </summary>
        /// <param name="pointF"></param>
        /// <returns></returns>
        public static Point ToPoint(this PointF pointF)
        {
            return new Point((int)pointF.X, (int)pointF.Y);
        }

        /// <summary>
        /// 获取缩放尺寸
        /// </summary>
        /// <param name="sizeF"></param>
        /// <param name="widthScale"></param>
        /// <param name="heightScale"></param>
        /// <returns></returns>
        public static SizeF Resize(this SizeF sizeF, float widthScale, float heightScale)
        {
            return new SizeF(sizeF.Width * widthScale, sizeF.Height * heightScale);
        }

        /// <summary>
        /// 转Rectangle
        /// </summary>
        /// <param name="rectangleF"></param>
        /// <returns></returns>
        public static Rectangle ToRectangle(this RectangleF rectangleF)
        {
            return new Rectangle(rectangleF.Location.ToPoint(), rectangleF.Size.ToSize());
        }
    }
}
