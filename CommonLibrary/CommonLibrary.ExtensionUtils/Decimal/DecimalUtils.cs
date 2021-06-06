
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonLibrary.ExtensionUtils
{
    public static class DecimalUtils
    {
        /// <summary>
        /// 把decimal类型的实例值转为Int32类型的值
        /// </summary>
        /// <param name="instance">decimal类型的实例值</param>
        /// <returns>
        ///   返回Int32类型的值
        /// </returns>
        public static int ToInt32(this decimal instance)
        {
            return (int)instance;
        }

        public static decimal ToKeepFloat(this decimal instance, byte floatNumber)
        {
            if (floatNumber == 0 || floatNumber > 9)
            {
                throw new ArgumentException("参数错误，小数数字只允许1到9！");
            }
            else
            {
                var s = instance.ToString().TrimEnd('0');
                if (s.SubRightString(1) == floatNumber.ToString())
                {
                    return instance;
                }
                else
                {
                    return Math.Round(instance, MidpointRounding.AwayFromZero);
                }
            }
        }
        
        /// <summary>
        /// 转整形
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static short ToInt16(this decimal instance)
        {
            return (short)instance;
        }

        /// <summary>
        /// 转浮点
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static float ToFloat(this decimal instance)
        {
            return (float)instance;
        }

        /// <summary>
        /// 转双精度
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static double ToDouble(this decimal instance)
        {
            return (double)instance;
        }

        /// <summary>
        /// 将小数值按指定的小数位数舍入
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="decimals"></param>
        /// <returns></returns>
        public static decimal ToRound(this decimal instance, int decimals)
        {
            return Math.Round(instance, decimals);
        }

        /// <summary>
        /// 转十六进制
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static string ToHex(this decimal instance)
        {
            if (instance.ToString().Contains("."))
            {
                var s = instance.ToString().Split('.');
                if (int.Parse(s[1]) > 0)
                {
                    return int.Parse(s[0]).ToHex() + "." + int.Parse(s[1]).ToHex();
                }
                else
                {
                    return instance.ToInt32().ToHex();
                }
            }
            else
            {
                return instance.ToInt32().ToHex();
            }
        }

        /// <summary>
        /// 转十六进制
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="paddingIntWidth"></param>
        /// <param name="paddingDecimalWidth"></param>
        /// <returns></returns>
        public static string ToHex(this decimal instance, int paddingIntWidth, int paddingDecimalWidth)
        {
            return instance.ToHex(paddingIntWidth, paddingDecimalWidth, false);
        }

        /// <summary>
        /// 转十六进制
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="paddingIntWidth"></param>
        /// <param name="paddingDecimalWidth"></param>
        /// <param name="isSignNumber"></param>
        /// <returns></returns>
        public static string ToHex(this decimal instance, int paddingIntWidth, int paddingDecimalWidth, bool isSignNumber)
        {
            var hex = instance.ToHex();
            var bin = "0";
            if (hex.Contains("."))
            {
                var s = hex.Split('.');
                //hex = s[0].PadLeft(paddingIntWidth, '0') + s[1].PadRight(paddingDecimalWidth, '0');
                //修改负数小数位转16进制错误，例如-10.1，原来的代码转为F610，正确的应该是F601
                hex = s[0].PadLeft(paddingIntWidth, '0') + s[1].PadLeft(paddingDecimalWidth, '0');
                bin = hex.ToIntFromHex().ToBinary().PadLeft(paddingIntWidth * 4, '0');
            }
            else
            {
                hex = hex.PadLeft(paddingIntWidth, '0').PadRight(paddingIntWidth + paddingDecimalWidth, '0');
                bin = hex.ToIntFromHex().ToBinary().PadLeft(paddingIntWidth * 4, '0');
            }
            if (instance >= 0)
            {
                return hex;
            }
            else
            {
                hex = instance.ToHex();
                if (!isSignNumber)
                {
                    return hex;
                }
                var s = hex.Split('.');
                var leftHex = s[0].PadLeft(paddingIntWidth, '0');
                var rightHex = "".PadLeft(paddingDecimalWidth, '0');
                if (s.Length > 1)
                {
                    //rightHex = s[1].PadRight(paddingDecimalWidth, '0');
                    //修改负数小数位转16进制错误，例如-10.1，原来的代码转为F610，正确的应该是F601
                    rightHex = s[1].PadLeft(paddingDecimalWidth, '0');
                }

                var leftBin = leftHex.ToIntFromHex().ToBinary().PadLeft(paddingIntWidth * 4, '0');
                leftBin = "{0}{1}".FormatWith("1", leftBin.Remove(0, 1));
                return "{0}{1}".FormatWith(leftBin.ToIntFromBinary().ToHex(), rightHex);
            }
        }

        /// <summary>
        /// 转字节
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static byte ToByte(this decimal instance)
        {
            return Convert.ToByte(instance);
        }

        /// <summary>
        /// 转字节
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static sbyte ToSByte(this decimal instance)
        {
            return Convert.ToSByte(instance);
        }
    }
}