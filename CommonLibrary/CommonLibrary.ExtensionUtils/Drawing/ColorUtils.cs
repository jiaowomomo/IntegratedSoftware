using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLibrary.ExtensionUtils
{
    public static class ColorUtils
    {
        /// <summary>
        /// 比较颜色
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static bool IsEqual(this Color instance, Color target)
        {
            if (instance.R == target.R && instance.G == target.G && instance.B == target.B && instance.A >= 10)
            {
                return true;
            }
            else if (target.A < 10 && instance.A < 10)
            {
                return true;
            }
            return false;
        }
    }
}
