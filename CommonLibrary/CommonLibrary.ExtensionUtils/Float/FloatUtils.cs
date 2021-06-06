using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonLibrary.ExtensionUtils
{
    public static class FloatUtils
    {
        /// <summary>
        /// 转十进制
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static decimal ToDecimal(this float instance)
        {
            return (decimal)instance;
        }

        /// <summary>
        /// 转字节数组
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="isReverse"></param>
        /// <returns></returns>
        public static byte[] ToBytes(this float instance, bool isReverse)
        {
            var result = BitConverter.GetBytes(instance);
            if (isReverse)
            {
                Array.Reverse(result);
            }
            return result;
        }
    }
}
