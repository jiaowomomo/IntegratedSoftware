using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonLibrary.ExtensionUtils
{
    public static class Int64Utils
    {
        /// <summary>
        /// 转字节数组
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="isReverse"></param>
        /// <returns></returns>
        public static byte[] ToBytes(this Int64 instance, bool isReverse)
        {
            var result = BitConverter.GetBytes(instance);
            if (isReverse)
            {
                Array.Reverse(result);
            }
            return result;
        }

        /// <summary>
        /// 转十六进制字符串
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static string ToHex(this long instance)
        {
            if (instance < 0)
            {
                return Convert.ToString(256 + instance, 16).ToUpper();
            }
            else
            {
                return Convert.ToString(instance, 16).ToUpper();
            }
        }

        /// <summary>
        /// 转十六进制字符串
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="paddingWidth"></param>
        /// <returns></returns>
        public static string ToHex(this long instance, int paddingWidth)
        {
            return instance.ToHex().PadLeft(paddingWidth, '0');
        }

        /// <summary>
        /// 计算文件大小函数(保留两位小数),Size为字节大小
        /// </summary>
        /// <param name="size">初始文件大小</param>
        /// <returns></returns>
        public static string ToFileSizeString(this long size)
        {
            var num = 1024.00; //byte
            if (size < num)
                return size + "字节";
            if (size < Math.Pow(num, 2))
                return (size / num).ToString("f0") + "KB"; //kb
            if (size < Math.Pow(num, 3))
                return (size / Math.Pow(num, 2)).ToString("f2") + "MB"; //M
            if (size < Math.Pow(num, 4))
                return (size / Math.Pow(num, 3)).ToString("f2") + "GB"; //G

            return (size / Math.Pow(num, 4)).ToString("f2") + "TB"; //T
        }
    }
}