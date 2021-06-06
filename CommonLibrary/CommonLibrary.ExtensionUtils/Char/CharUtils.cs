using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonLibrary.ExtensionUtils
{
    public static class CharUtils
    {
        /// <summary>
        /// 字符转整形
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static int ToInt32(this char instance)
        {
            return instance.ToString().ToInt32();
        }

        /// <summary>
        /// 字符转字节
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static byte ToByte(this char instance)
        {
            return Convert.ToByte(instance);
        }

        /// <summary>
        /// 判断是否十六进制
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static bool IsHex(this char instance)
        {
            var keyChar = (int)instance;
            if ((keyChar >= 48 && keyChar <= 57) || (keyChar >= 65 && keyChar <= 70) || (keyChar >= 97 && keyChar <= 102))
            {
                return true;
            }
            return false;
        }
    }
}
