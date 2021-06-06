using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.Drawing;
using System.IO;
using System;

namespace CommonLibrary.ExtensionUtils
{
    public static class ByteUtils
    {
        /// <summary>
        /// 计算Modbus校验位
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static ushort ToModbusCrcUInt16(this byte[] source)
        {
            ushort value;
            ushort newLoad = 0xffff, In_value;
            int count = 0;
            for (int i = 0; i < source.Length; i++)
            {
                value = (ushort)source[i];
                newLoad = (ushort)(Convert.ToInt32(value) ^ Convert.ToInt32(newLoad));
                In_value = 0xA001;
                while (count < 8)
                {
                    if (Convert.ToInt32(newLoad) % 2 == 1)//判断最低位是否为1
                    {
                        newLoad -= 0x00001;
                        newLoad = (ushort)(Convert.ToInt32(newLoad) / 2);//右移一位
                        count++;//计数器加一
                        newLoad = (ushort)(Convert.ToInt32(newLoad) ^ Convert.ToInt32(In_value));//异或操作
                    }
                    else
                    {
                        newLoad = (ushort)(Convert.ToInt32(newLoad) / 2);//右移一位
                        count++;//计数器加一
                    }
                }
                count = 0;
            }
            return newLoad;
        }

        /// <summary>
        /// 计算Modbus校验位字节数组
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static byte[] ToModbusCrc(this byte[] source)
        {
            return ToModbusCrcUInt16(source).ToBytes(false);
        }

        /// <summary>
        /// 字节数组生成位图
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static Bitmap ToBitmap(this byte[] source)
        {
            using(var ms = new MemoryStream(source))
            {
                return (Bitmap)Image.FromStream(ms);
            }
        }

        /// <summary>
        /// 截取字节数组
        /// </summary>
        /// <param name="source"></param>
        /// <param name="startIndex"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static byte[] SubBytes(this byte[] source, int startIndex, int length)
        {
            if (startIndex < 0 || startIndex > source.Length || length < 0)
            {
                throw new ArgumentOutOfRangeException();
            }

            byte[] Destination;
            if (startIndex + length <= source.Length)
            {
                Destination = new byte[length];
                Array.Copy(source, startIndex, Destination, 0, length);
            }
            else
            {
                Destination = new byte[source.Length - startIndex];
                Array.Copy(source, startIndex, Destination, 0, source.Length - startIndex);
            }

            return Destination;
        }

        /// <summary>
        /// 字节转布尔
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static bool ToBoolean(this Byte instance)
        {
            return instance != 0;
        }

        /// <summary>
        /// 字节转十六进制字符串
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static string ToHex(this Byte instance)
        {
            return Convert.ToString(instance, 16).ToUpper().PadLeft(2, '0');
        }

        /// <summary>
        /// 字节转十六进制字符串
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static string ToHex(this SByte instance)
        {
            return Convert.ToString(instance, 16).ToUpper().PadLeft(2, '0');
        }

        /// <summary>
        /// 字节转十六进制字符串
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="paddingWidth"></param>
        /// <returns></returns>
        public static string ToHex(this Byte instance, int paddingWidth)
        {
            return Convert.ToString(instance, 16).ToUpper().PadLeft(paddingWidth, '0');
        }

        /// <summary>
        /// 字节转十六进制字符串
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="paddingWidth"></param>
        /// <returns></returns>
        public static string ToHex(this SByte instance, int paddingWidth)
        {
            return Convert.ToString(instance, 16).ToUpper().PadLeft(paddingWidth, '0');
        }

        /// <summary>
        /// 字节数组转十六进制字符串
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string ToHex(this byte[] bytes)
        {
            string returnStr = "";
            if (bytes != null)
            {
                for (int i = 0; i < bytes.Length; i++)
                {
                    returnStr += bytes[i].ToString("X2");
                }
            }
            return returnStr;
        }

        /// <summary>
        /// 字节转整形
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static UInt16 ToUInt16(this Byte instance)
        {
            return Convert.ToUInt16(instance);
        }

        /// <summary>
        /// 字节转整形
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static int ToInt32(this Byte instance)
        {
            return Convert.ToInt32(instance);
        }

        public static int ToInt32(this Byte instance, bool isSignNumber)
        {
            var value = Convert.ToInt32(instance);
            if (isSignNumber && value > 128)
            {
                return value - 256;
            }
            else
            {
                return value;
            }
        }

        /// <summary>
        /// 字节数组转整形
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static int ToInt32(this byte[] source)
        {
            if (source.Length <= 4)
            {
                var ins = new byte[4];
                source.Reverse().ToArray().CopyTo(ins, 0);
                return  BitConverter.ToInt32(ins, 0);
            }
            throw new ArgumentOutOfRangeException();
        }

        /// <summary>
        /// 获取指定位值
        /// </summary>
        /// <param name="b"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static bool GetBit(this byte b, int index)
        {
            return (b & (1 << index)) > 0;
        }

        /// <summary>
        /// 设置指定位值为1
        /// </summary>
        /// <param name="b"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static byte SetBit(this byte b, int index)
        {
            b |= (byte)(1 << index);
            return b;
        }

        /// <summary>
        /// 设置指定位值为0
        /// </summary>
        /// <param name="b"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static byte ClearBit(this byte b, int index)
        {
            b &= (byte)((1 << 8) - 1 - (1 << index));
            return b;
        }

        /// <summary>
        /// 将指定位值取反
        /// </summary>
        /// <param name="b"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static byte ReverseBit(this byte b, int index)
        {
            b ^= (byte)(1 << index);
            return b;
        }

        /// <summary>
        /// 获取Hash值
        /// </summary>
        /// <param name="data"></param>
        /// <param name="hashName"></param>
        /// <returns></returns>
        public static byte[] Hash(this byte[] data, string hashName)
        {
            HashAlgorithm algorithm;
            if (string.IsNullOrEmpty(hashName)) algorithm = HashAlgorithm.Create();
            else algorithm = HashAlgorithm.Create(hashName);
            return algorithm.ComputeHash(data);
        }

        /// <summary>
        /// 获取Hash值
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static byte[] Hash(this byte[] data)
        {
            return Hash(data, null);
        }

        /// <summary>
        /// 字节数组转字符串
        /// </summary>
        /// <param name="data"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string Decode(this byte[] data, Encoding encoding)
        {
            return encoding.GetString(data);
        }

        /// <summary>
        /// 字节转枚举
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static T ToEnum<T>(this byte instance)
        {
            return (T)Enum.Parse(typeof(T), instance.ToString());
        }

        /// <summary>
        /// 字节数组转整形数组
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        public static int[] ToIntArray(this byte[] src)
        {
            var values = new int[src.Length];
            for (int i = 0; i < src.Length; i++)
            {
                values[i] = (int)src[i];
            }
            return values;
        }
    }
}

