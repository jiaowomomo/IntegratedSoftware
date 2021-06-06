using System;

namespace CommonLibrary.ExtensionUtils
{
    public static class Int16Utils
    {
        /// <summary>
        /// 转十六进制字符串
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static string ToHex(this Int16 instance)
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
        public static string ToHex(this Int16 instance, int paddingWidth)
        {
            return instance.ToHex().PadLeft(paddingWidth, '0');
        }

        /// <summary>
        /// 转二进制字符串
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static string ToBinary(this Int16 instance)
        {
            return Convert.ToString(instance, 2);
        }

        /// <summary>
        /// 转二进制字符串
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="paddingWidth"></param>
        /// <returns></returns>
        public static string ToBinary(this Int16 instance, int paddingWidth)
        {
            return Convert.ToString(instance, 2).PadLeft(paddingWidth, '0');
        }

        /// <summary>
        /// 转布尔
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static bool ToBoolean(this Int16 instance)
        {
            return instance != 0;
        }

        /// <summary>
        /// 转字节数组
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="isReverse"></param>
        /// <returns></returns>
        public static byte[] ToBytes(this Int16 instance, bool isReverse)
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
        public static T ToEnum<T>(this short instance)
        {
            return (T)Enum.Parse(typeof(T), instance.ToString());
        }
    }
}