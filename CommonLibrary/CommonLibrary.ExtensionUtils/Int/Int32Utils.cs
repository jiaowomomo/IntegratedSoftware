using System;

namespace CommonLibrary.ExtensionUtils
{
    public static class Int32Utils
    {
        /// <summary>
        /// 转十六进制字符串
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static string ToHex(this int instance)
        {
            var text = string.Empty;
            if (instance < 0)
            {
                text = Convert.ToString(256 + instance, 16).ToUpper();

            }
            else
            {
                text = Convert.ToString(instance, 16).ToUpper();
            }
            if ((text.Length % 2) > 0)
                return "0" + text;
            else
                return text;
        }

        /// <summary>
        /// 转十六进制字符串
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="paddingWidth"></param>
        /// <returns></returns>
        public static string ToHex(this int instance, int paddingWidth)
        {
            return instance.ToHex().PadLeft(paddingWidth, '0');
        }

        /// <summary>
        /// 转二进制字符串
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static string ToBinary(this int instance)
        {
            return Convert.ToString(instance, 2);
        }

        /// <summary>
        /// 转二进制字符串
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="paddingWidth"></param>
        /// <returns></returns>
        public static string ToBinary(this int instance, int paddingWidth)
        {
            return Convert.ToString(instance, 2).PadLeft(paddingWidth, '0');
        }

        /// <summary>
        /// 转布尔
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static bool ToBoolean(this int instance)
        {
            return instance != 0;
        }

        /// <summary>
        /// 转字节数组
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="isReverse"></param>
        /// <returns></returns>
        public static byte[] ToBytes(this int instance, bool isReverse)
        {
            var result = BitConverter.GetBytes(instance);
            if (isReverse)
            {
                Array.Reverse(result);
            }
            return result;
        }

        /// <summary>
        /// 转枚举
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static T ToEnum<T>(this int instance)
        {
            return (T)Enum.Parse(typeof(T), instance.ToString());
        }
    }
}
