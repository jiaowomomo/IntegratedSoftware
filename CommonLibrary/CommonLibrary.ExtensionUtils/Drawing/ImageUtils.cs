using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLibrary.ExtensionUtils
{
    public static class ImageUtils
    {
        /// <summary>
        /// 转灰度图
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static Image ToGrayImage(this Image source)
        {
            var c = new Color();
            var bmpSource = new Bitmap(source);
            var bmpGray = new Bitmap((Image)source.Clone());
            int rr, gg, bb, cc;
            for (int i = 0; i < bmpSource.Width; i++)
            {
                for (int j = 0; j < bmpSource.Height; j++)
                {
                    c = bmpSource.GetPixel(i, j);
                    rr = c.R;
                    gg = c.G;
                    bb = c.B;
                    cc = (int)((rr + gg + bb) / 3);
                    if (cc < 0)
                        cc = 0;
                    if (cc > 255)
                        cc = 255;
                    //用FromArgb把整形转换成颜色值
                    Color c1 = Color.FromArgb(cc, cc, cc);
                    bmpGray.SetPixel(i, j, c1);
                }
            }
            return bmpGray;
        }
    }
}
