using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonLibrary.ExtensionUtils
{
    public static class DoubleUtils
    {
        /// <summary>
        /// 转整形
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static int ToInt32(this double instance)
        {
            return (int)instance;
        }
    }
}
