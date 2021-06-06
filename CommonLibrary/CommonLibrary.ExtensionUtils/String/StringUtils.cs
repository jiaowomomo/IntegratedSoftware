using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Runtime.CompilerServices;
using System.Reflection;
using System.IO;
using System.Linq;
using System;

namespace CommonLibrary.ExtensionUtils
{
    public enum PasswordFormat
    {
        SHA1,
        PlainText,
        MD5
    }

    /// <summary>
    /// String类型扩展静态类，对String类型进行扩展，提供额外的的函数、属性
    /// </summary>
    public static class StringUtils
    {
        /// <summary>
        /// 是否时间格式字符串
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static bool IsDate(this string text)
        {
            var currentFileDate = DateTime.MinValue;
            return DateTime.TryParse(text, out currentFileDate);
        }

        /// <summary>
        /// 计算密码哈希值，采用UTF8编码，以SHA1的方式计算
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string ToPasswordHash(this string text)
        {
            return ToPasswordHash(text, Encoding.UTF8, PasswordFormat.SHA1);
        }

        /// <summary>
        /// 计算密码哈希值，采用UTF8编码，以SHA1的方式计算
        /// </summary>
        /// <param name="text"></param>
        /// <param name="passwordFormat">一般用SHA1</param>
        /// <returns></returns>
        public static string ToPasswordHash(this string text, PasswordFormat passwordFormat)
        {
            return ToPasswordHash(text, Encoding.UTF8, passwordFormat);
        }

        /// <summary>
        /// 计算密码哈希值
        /// </summary>
        /// <param name="text"></param>
        /// <param name="encoding">编码，一般采用UTF8</param>
        /// <param name="passwordFormat">一般用SHA1</param>
        /// <returns></returns>
        public static string ToPasswordHash(this string text, Encoding encoding, PasswordFormat passwordFormat)
        {
            if (passwordFormat == PasswordFormat.PlainText)
            {
                return text;
            }
            byte[] hash;
            var inputBytes = encoding.GetBytes(text.Trim());
            if (passwordFormat == PasswordFormat.MD5)
            {
                hash = MD5.Create().ComputeHash(inputBytes);
            }
            else
            {
                hash = SHA1.Create().ComputeHash(inputBytes);
            }
            var sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("x2"));
            }
            return sb.ToString().ToUpper();
        }

        /// <summary>
        /// 将String中的格式项替换为指定数组中相应Object实例值的文本等效项。
        /// </summary>
        /// <param name="formatString">带格式项的String</param>
        /// <param name="args">对象参数数组</param>
        /// <returns>返回完成格式化后的String。</returns>
        public static string FormatWith(this string formatString, params object[] args)
        {
            if (args == null)
            {
                throw new ArgumentNullException(string.Format("当调用{0}.FormatWith()方法时，args参数不能为空。", formatString));
            }
            return string.Format(formatString, args);
        }

        /// <summary>
        /// 将String中的格式项替换为指定数组中相应Object实例值的文本等效项。指定格式化对象机制。
        /// </summary>
        /// <param name="formatString">带格式项的String</param>
        /// <param name="provider">指定格式化对象机制</param>
        /// <param name="args">对象参数数组</param>
        /// <returns></returns>
        public static string FormatWith(this string formatString, IFormatProvider provider, params object[] args)
        {
            if (args == null)
            {
                throw new ArgumentNullException(string.Format("当调用{0}.FormatWith()方法时，args参数不能为空。", formatString));
            }
            return string.Format(provider, formatString, args);
        }

        /// <summary>
        /// 连接指定的String到String实例本身
        /// </summary>
        /// <param name="instance">String实例</param>
        /// <param name="text">指定的String</param>
        /// <returns>返回连接后的String实例</returns>
        public static string ConcatWith(this string instance, string text)
        {
            return string.Concat(instance, text);
        }

        /// <summary>
        /// 连接指定的对象数组实例的String值，并连接到String实例本身
        /// </summary>
        /// <param name="instance">String实例</param>
        /// <param name="args">对象数组</param>
        /// <returns>返回连接后的String实例</returns>
        public static string ConcatWith(this string instance, params object[] args)
        {
            if (args == null)
            {
                throw new ArgumentNullException(string.Format("当调用{0}.FormatWith()方法时，args参数不能为空。", instance));
            }

            return string.Concat(instance, string.Concat(args));
        }

        /// <summary>
        /// 判断字符串实例是否包含字符串数组参数的任意项
        /// </summary>
        /// <param name="instance">要判断的字符串实例</param>
        /// <param name="args">要判断的字符串数组参数</param>
        /// <returns>
        /// 如果包含则返回true，否则返回false
        /// </returns>
        public static bool Contains(this string instance, params string[] args)
        {
            foreach (string s in args)
            {
                if (instance.Contains(s))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 判断字符串实例是否为null或空值
        /// </summary>
        /// <param name="instance">字符串实例</param>
        /// <returns>
        ///   <c>true</c> 如果指定的字符串为空; 否则, <c>false</c>.
        /// </returns>
        public static bool IsEmpty(this string instance)
        {
            return string.IsNullOrEmpty(instance);
        }

        /// <summary>
        /// 判断字符串实例是否不为null或不为空值
        /// </summary>
        /// <param name="instance">字符串实例</param>
        /// <returns>
        ///   <c>true</c> 如果指定的字符串不为空; 否则, <c>false</c>.
        /// </returns>
        public static bool IsNotEmpty(this string instance)
        {
            return !instance.IsEmpty();
        }
        /// <summary>
        /// 确定指定的实例是否为数字。
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static bool IsNumeric(this string instance)
        {
            return Regex.IsMatch(instance, @"^[+-]?\d*[.]?\d*$");
        }

        /// <summary>
        /// 确定指定的实例是否为整数。
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static bool IsInteger(this string instance)
        {
            if (instance.IsNull())
                return false;
            else
                return Regex.IsMatch(instance, @"^[+-]?\d*$");
        }

        /// <summary>
        /// 从开始和结束位置之间返回一个字符串。
        /// </summary>
        /// <param name="text"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public static string Between(this string text, string start, string end)
        {
            if (text.IsEmpty())
            {
                return text;
            }

            return text.RightOf(start).LeftOfRightMostOf(end);
        }

        /// <summary>
        /// 返回此字符串数组的子数组。
        /// </summary>
        /// <param name="items"></param>
        /// <param name="start"></param>
        /// <returns></returns>
        public static string[] Subarray(this string[] items, int start)
        {
            return items.Subarray(start, items.Length - start);
        }

        /// <summary>
        /// 返回此字符串数组的子数组。
        /// </summary>
        /// <param name="items"></param>
        /// <param name="start"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string[] Subarray(this string[] items, int start, int length)
        {
            if (start > items.Length)
            {
                throw new ArgumentException(string.Format("The start index [{0}] is greater than the length [{1}] of the array.", start, items.Length));
            }

            if ((start + length) > items.Length)
            {
                throw new ArgumentException(string.Format("The length [{0}] to return is greater than the length [{1}] of the array.", length, items.Length));
            }

            string[] temp = new string[length];

            int count = 0;

            for (int i = start; i < start + length; i++)
            {
                temp[count] = items[i];
                count++;
            }

            return temp;
        }

        /// <summary>
        /// 串联字符串数组的所有元素
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        public static string Join(this IEnumerable items)
        {
            return items.Join(",", "{0}");
        }

        /// <summary>
        /// 串联字符串数组的所有元素
        /// </summary>
        /// <param name="items"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static string Join(this IEnumerable items, string separator)
        {
            return items.Join(separator, "{0}");
        }

        /// <summary>
        /// 串联字符串数组的所有元素
        /// </summary>
        /// <param name="items"></param>
        /// <param name="separator"></param>
        /// <param name="template"></param>
        /// <returns></returns>
        public static string Join(this IEnumerable items, string separator, string template)
        {
            StringBuilder sb = new StringBuilder();

            foreach (object item in items)
            {
                if (item != null)
                {
                    sb.Append(separator);
                    sb.Append(string.Format(template, item.ToString()));
                }
            }

            return sb.ToString().RightOf(separator);
        }

        /// <summary>
        /// 从字符串的每一端截取一个字符。
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string Chop(this string text)
        {
            return text.Chop(1);
        }

        /// <summary>
        /// 从字符串的每一端截取指定数量的字符。
        /// </summary>
        /// <param name="text"></param>
        /// <param name="characters"></param>
        /// <returns></returns>
        public static string Chop(this string text, int characters)
        {
            if (text.IsEmpty())
            {
                return text;
            }

            return text.Substring(characters, text.Length - characters - 1);
        }

        /// <summary>
        /// 从字符串的每一端截取下指定的字符串。 如果字符串的两端都不存在该字符，则不将其截取。
        /// </summary>
        /// <param name="text"></param>
        /// <param name="character"></param>
        /// <returns></returns>
        public static string Chop(this string text, string character)
        {
            if (text.IsEmpty())
            {
                return text;
            }

            if (text.StartsWith(character) && text.EndsWith(character))
            {
                int length = character.Length;
                return text.Substring(length, text.Length - (length + 1));
            }

            return text;
        }

        /// <summary>
        /// 将每个单词的第一个字符转换为大写
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string ToTitleCase(this string text)
        {
            if (text.IsEmpty())
            {
                return text;
            }

            return text.Split(' ').ToTitleCase();
        }

        /// <summary>
        /// 将每个单词的第一个字符转换为大写
        /// </summary>
        /// <param name="text"></param>
        /// <param name="ci"></param>
        /// <returns></returns>
        public static string ToTitleCase(this string text, CultureInfo ci)
        {
            if (text.IsEmpty())
            {
                return text;
            }

            return text.Split(' ').ToTitleCase(ci);
        }

        /// <summary>
        /// 将每个单词的第一个字符转换为大写
        /// </summary>
        /// <param name="words"></param>
        /// <returns></returns>
        public static string ToTitleCase(this string[] words)
        {
            return words.ToTitleCase(null);
        }

        /// <summary>
        /// 将每个单词的第一个字符转换为大写
        /// </summary>
        /// <param name="words"></param>
        /// <param name="ci"></param>
        /// <returns></returns>
        public static string ToTitleCase(this string[] words, CultureInfo ci)
        {
            if (words == null || words.Length == 0)
            {
                return "";
            }

            for (int i = 0; i < words.Length; i++)
            {
                words[i] = (ci != null ? char.ToUpper(words[i][0], ci) : char.ToUpper(words[i][0])) + words[i].Substring(1);
            }

            return string.Join(" ", words);
        }

        /// <summary>
        /// 是否布尔字符串
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        public static bool ToBoolean(this string instance)
        {
            if (instance == "1" || instance.ToLower() == "true" || instance.ToLower().Contains("enable"))
                return true;
            else
                return false;
        }

        /// <summary>
        /// 把16进制字符串转为10进制字节.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        public static byte ToByteFromHex(this string instance)
        {
            return Convert.ToByte(instance, 16);
        }

        /// <summary>
        /// 字符串转16进制字节数组
        /// </summary>
        /// <param name="hex"></param>
        /// <returns></returns>
        public static byte[] ToBytesFromHex(this string hex)
        {
            hex = hex.Replace(" ", "").Trim();
            if ((hex.Length % 2) != 0)
            {
                hex += " ";
            }
            var bytes = new byte[hex.Length / 2];
            for (int i = 0; i < bytes.Length; i++)
            {
                bytes[i] = Convert.ToByte(hex.Substring(i * 2, 2), 16);
            }
            return bytes;
        }

        /// <summary>
        /// 字符串转Ascii码
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static string ToASCII(this string instance)
        {
            var sb = new StringBuilder();
            instance.ToArray().ToList().ForEach(x =>
            {
                sb.Append(((int)x).ToHex());
            });
            return sb.ToString();
        }

        /// <summary>
        /// 字符串转字节数组
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static byte[] ToBytes(this string instance)
        {
            char[] values = instance.ToCharArray();
            var lstBytes = Encoding.UTF8.GetBytes(values);
            return lstBytes;
        }

        /// <summary>
        /// 转十六进制字符串
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static string ToHexString(this string instance)
        {
            var result = "";
            var values = instance.ToCharArray();
            foreach (char letter in values)
            {
                var value = Convert.ToInt32(letter);
                var hexOutput = String.Format("{0:X}", value);
                result += hexOutput;
            }
            return result;
        }

        /// <summary>
        /// 是否十六进制字符串
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static bool IsHexString(this string instance)
        {
            foreach (char letter in instance.ToCharArray())
            {
                if (!letter.IsHex())
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 转整形
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        public static int ToInt32(this string instance)
        {
            if (IsInteger(instance))
                return int.Parse(instance);
            else
                return 0;
        }

        /// <summary>
        /// 转整形
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static long ToInt64(this string instance)
        {
            if (IsInteger(instance))
                return long.Parse(instance);
            else
                return 0;
        }

        /// <summary>
        /// 转时间
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static DateTime ToDateTime(this string instance)
        {
            return DateTime.Parse(instance);
        }

        /// <summary>
        /// 把十六进制字符串转十进制数
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="isSignNumber"></param>
        /// <returns></returns>
        public static decimal ToDecimalFromHex(this string instance, bool isSignNumber)
        {
            var integerPart = instance.Substring(0, 2).ToIntFromHex(isSignNumber).ToString();
            var floatPart = instance.Substring(2, 2).ToIntFromHex().ToString();
            return decimal.Parse(integerPart + "." + floatPart);
        }

        /// <summary>
        /// 把16进制字符串转为10进制整型.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        public static int ToIntFromHex(this string instance)
        {
            try
            {
                return Convert.ToInt32(instance, 16);
            }
            catch (Exception)
            {
                return 0;
            }
        }

        /// <summary>
        /// 把十六进制字符串转整形
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static UInt16 ToUInt16FromHex(this string instance)
        {
            try
            {
                return Convert.ToUInt16(instance, 16);
            }
            catch (Exception)
            {
                return 0;
            }
        }

        /// <summary>
        /// 把16进制字符串转为10进制整型.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        public static int ToIntFromHex(this string instance, bool isSignNumber)
        {
            if (isSignNumber)
            {
                if (instance.ToIntFromHex().ToBinary().PadLeft(8, '0').Substring(0, 1) == "0")
                {
                    return Convert.ToInt32(instance, 16);
                }
                else
                {
                    return -(256 - Convert.ToInt32(instance, 16));
                }
            }
            else
            {
                return Convert.ToInt32(instance, 16);
            }
        }

        /// <summary>
        /// 把16进制字符串转为10进制整型.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        public static Int64 ToInt64FromHex(this string instance)
        {
            return Convert.ToInt64(instance, 16);
        }

        /// <summary>
        /// 把二进制转整形
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static int ToIntFromBinary(this string instance)
        {
            return Convert.ToInt32(instance, 2);
        }

        /// <summary>
        /// 把二进制转整形
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static short ToInt16FromBinary(this string instance)
        {
            return Convert.ToInt16(instance, 2);
        }

        /// <summary>
        /// 把二进制转整形
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static Int64 ToInt64FromBinary(this string instance)
        {
            return Convert.ToInt64(instance, 2);
        }

        /// <summary>
        /// 是否Pascal表达法字符串
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static bool IsPascalCase(this string text)
        {
            if (text.IsEmpty())
            {
                return false;
            }
            return text.Substring(0, 1).ToLowerInvariant().Equals(text.Substring(0, 1));
        }

        /// <summary>
        /// 转Pascal表达法字符串
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string ToPascalCase(this string text)
        {
            if (text.IsEmpty())
            {
                return text;
            }

            return text.Substring(0, 1).ToLower(CultureInfo.InvariantCulture) + text.Substring(1);
        }

        /// <summary>
        /// 转Pascal表达法字符串
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public static string ToPascalCase(this string[] values)
        {
            if (values == null || values.Length == 0)
            {
                return "";
            }
            return values.ToPascalCase();
        }

        /// <summary>
        /// 转Camel表达法字符串
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string ToCamelCase(this string text)
        {
            if (text.IsEmpty())
            {
                return text;
            }
            if (text.Contains("_"))
            {
                var value = "";
                text.Split('_').ToList().ForEach(x =>
                {
                    value += x.Substring(0, 1).ToUpper(CultureInfo.CurrentCulture) + x.Substring(1);
                });
                return value;
            }
            else
            {
                return text.Substring(0, 1).ToUpper(CultureInfo.CurrentCulture) + text.Substring(1);
            }
        }

        /// <summary>
        /// 转Camel表达法字符串
        /// </summary>
        /// <param name="values"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static string ToCamelCase(this string[] values, string separator)
        {
            string temp = "";
            foreach (string s in values)
            {
                temp += separator;
                temp += ToCamelCase(s);
            }
            return temp;
        }

        /// <summary>
        /// 转Camel表达法字符串
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public static string ToCamelCase(this string[] values)
        {
            return values.ToCamelCase("");
        }

        /// <summary>
        /// 用字符填充字符串的左侧以形成总长度
        /// </summary>
        /// <param name="text"></param>
        /// <param name="c"></param>
        /// <param name="totalLength"></param>
        /// <returns></returns>
        public static string PadLeft(this string text, char c, Int32 totalLength)
        {
            if (text.IsEmpty())
            {
                return text;
            }

            if (totalLength < text.Length)
            {
                return text;
            }

            return new String(c, totalLength - text.Length) + text;
        }

        /// <summary>
        /// 如果是单个字符，则在字符串的右侧用'0'填充
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string PadRight(this string text)
        {
            return PadRight(text, '0', 2);
        }

        /// <summary>
        /// 用字符填充字符串的右侧以形成总长度
        /// </summary>
        /// <param name="text"></param>
        /// <param name="c"></param>
        /// <param name="totalLength"></param>
        /// <returns></returns>
        public static string PadRight(this string text, char c, Int32 totalLength)
        {
            if (text.IsEmpty())
            {
                return text;
            }

            if (totalLength < text.Length)
            {
                return text;
            }

            return string.Concat(text, new String(c, totalLength - text.Length));
        }

        public static string PadLeftEx(this string str, char c, int totalByteCount)
        {
            Encoding coding = Encoding.GetEncoding("gb2312");
            int dcount = 0;
            foreach (char ch in str.ToCharArray())
            {
                if (coding.GetByteCount(ch.ToString()) == 2)
                    dcount++;
            }
            string w = str.PadLeft(totalByteCount - dcount, c);
            return w;
        }

        public static string PadRightEx(this string str, char c, int totalByteCount)
        {
            Encoding coding = Encoding.GetEncoding("gb2312");
            int dcount = 0;
            foreach (char ch in str.ToCharArray())
            {
                if (coding.GetByteCount(ch.ToString()) == 2)
                    dcount++;
            }
            string w = str.PadRight(totalByteCount - dcount, c);
            return w;
        }

        /// <summary>
        /// 第一次出现char的左侧
        /// </summary>
        /// <param name="text"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        public static string LeftOf(this string text, char c)
        {
            if (text.IsEmpty())
            {
                return text;
            }

            int i = text.IndexOf(c);

            if (i == -1)
            {
                return text;
            }

            return text.Substring(0, i);
        }

        /// <summary>
        /// 第一次出现的文字的左侧
        /// </summary>
        /// <param name="text"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string LeftOf(this string text, string value)
        {
            if (text.IsEmpty())
            {
                return text;
            }

            int i = text.IndexOf(value);

            if (i == -1)
            {
                return text;
            }

            return text.Substring(0, i);
        }

        /// <summary>
        /// 索引的左侧出现字符
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="c">The c.</param>
        /// <param name="n">The n.</param>
        /// <returns></returns>
        public static string LeftOf(this string text, char c, int index)
        {
            if (text.IsEmpty())
            {
                return text;
            }

            int i = -1;

            while (index != 0)
            {
                i = text.IndexOf(c, i + 1);
                if (i == -1)
                {
                    return text;
                }
                --index;
            }

            return text.Substring(0, i);
        }

        /// <summary>
        /// 首次出现字符的右侧
        /// </summary>
        /// <param name="text"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        public static string RightOf(this string text, char c)
        {
            if (text.IsEmpty())
            {
                return text;
            }

            int i = text.IndexOf(c);

            if (i == -1)
            {
                return "";
            }

            return text.Substring(i + 1);
        }

        /// <summary>
        /// 第一次出现文字的右侧
        /// </summary>
        /// <param name="text"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string RightOf(this string text, string value)
        {
            if (text.IsEmpty())
            {
                return text;
            }

            int i = text.IndexOf(value);

            if (i == -1)
            {
                return "";
            }

            return text.Substring(i + value.Length);
        }

        /// <summary>
        /// 字符的索引出现的右侧
        /// </summary>
        /// <param name="text"></param>
        /// <param name="c"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        public static string RightOf(this string text, char c, int n)
        {
            if (text.IsEmpty())
            {
                return text;
            }

            int i = -1;

            while (n != 0)
            {
                i = text.IndexOf(c, i + 1);
                if (i == -1)
                {
                    return "";
                }
                --n;
            }

            return text.Substring(i + 1);
        }

        /// <summary>
        /// 字符的索引出现的右侧
        /// </summary>
        /// <param name="text"></param>
        /// <param name="c"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        public static string RightOf(this string text, string c, int n)
        {
            if (text.IsEmpty())
            {
                return text;
            }

            int i = -1;

            while (n != 0)
            {
                i = text.IndexOf(c, i + 1);
                if (i == -1)
                {
                    return "";
                }
                --n;
            }

            return text.Substring(i + 1);
        }

        /// <summary>
        /// Lefts the of rightmost of.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="c">The c.</param>
        /// <returns></returns>
        public static string LeftOfRightMostOf(this string text, char c)
        {
            if (text.IsEmpty())
            {
                return text;
            }

            int i = text.LastIndexOf(c);

            if (i == -1)
            {
                return text;
            }

            return text.Substring(0, i);
        }

        /// <summary>
        /// Lefts the of rightmost of.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static string LeftOfRightMostOf(this string text, string value)
        {
            if (text.IsEmpty())
            {
                return text;
            }

            int i = text.LastIndexOf(value);

            if (i == -1)
            {
                return text;
            }

            return text.Substring(0, i);
        }

        public static string Substring(this string text, string start, string end)
        {
            var left = text.LeftOfRightMostOf(start);
            var right = text.RightOfRightMostOf(end);

            return text.Remove(0, left.Length + 1).RemoveRight(right.Length + 1);
        }

        /// <summary>
        /// Rights the of rightmost of.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="c">The c.</param>
        /// <returns></returns>
        public static string RightOfRightMostOf(this string text, char c)
        {
            if (text.IsEmpty())
            {
                return text;
            }

            int i = text.LastIndexOf(c);

            if (i == -1)
            {
                return text;
            }

            return text.Substring(i + 1);
        }

        /// <summary>
        /// Rights the of rightmost of.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static string RightOfRightMostOf(this string text, string value)
        {
            if (text.IsEmpty())
            {
                return text;
            }

            int i = text.LastIndexOf(value);

            if (i == -1)
            {
                return text;
            }

            return text.Substring(i + value.Length);
        }

        /// <summary>
        /// Replaces the last instance of.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        /// <returns></returns>
        public static string ReplaceLastInstanceOf(this string text, string oldValue, string newValue)
        {
            if (text.IsEmpty())
            {
                return text;
            }

            return string.Format("{0}{1}{2}", text.LeftOfRightMostOf(oldValue), newValue, text.RightOfRightMostOf(oldValue));
        }

        /// <summary>
        /// 替换字符串
        /// </summary>
        /// <param name="text"></param>
        /// <param name="index"></param>
        /// <param name="subValue"></param>
        /// <returns></returns>
        public static string Replace(this string text, int index, string subValue)
        {
            if (text.IsEmpty())
            {
                return text;
            }

            return text.Remove(index, 1).Insert(index, subValue);
        }

        /// <summary>
        /// 反转
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static string Reverse(this string instance)
        {
            if (string.IsNullOrEmpty(instance))
            {
                throw new ArgumentException("参数不合法");
            }

            StringBuilder sb = new StringBuilder(instance.Length);
            for (int index = instance.Length - 1; index >= 0; index--)
            {
                sb.Append(instance[index]);
            }
            return sb.ToString();
        }

        /// <summary>
        /// 接受类似“ ArrowRotateClockwise”的字符串，并返回“ arrow_rotate_clockwise.png”。
        /// </summary>
        /// <param name="name"></param>
        /// <param name="separator"></param>
        /// <param name="extension"></param>
        /// <returns></returns>
        public static string ToCharacterSeparatedFileName(this string name, char separator, string extension)
        {
            if (name.IsEmpty())
            {
                return name;
            }

            MatchCollection match = Regex.Matches(name, @"([A-Z]+)[a-z]*|\d{1,}[a-z]{0,}");

            string temp = "";

            for (int i = 0; i < match.Count; i++)
            {
                if (i != 0)
                {
                    temp += separator;
                }

                temp += match[i].ToString().ToLowerInvariant();
            }

            string format = (string.IsNullOrEmpty(extension)) ? "{0}{1}" : "{0}.{1}";

            return string.Format(format, temp, extension);
        }

        /// <summary>
        /// 引用指定的文本
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string Enquote(this string text)
        {
            if (text.IsEmpty())
            {
                return text;
            }

            if (string.IsNullOrEmpty(text))
            {
                return string.Empty;
            }

            int i;
            int len = text.Length;
            StringBuilder sb = new StringBuilder(len + 4);
            string t;

            for (i = 0; i < len; i += 1)
            {
                char c = text[i];
                if ((c == '\\') || (c == '"') || (c == '>'))
                {
                    sb.Append('\\');
                    sb.Append(c);
                }
                else if (c == '\b')
                    sb.Append("\\b");
                else if (c == '\t')
                    sb.Append("\\t");
                else if (c == '\n')
                    sb.Append("\\n");
                else if (c == '\f')
                    sb.Append("\\f");
                else if (c == '\r')
                    sb.Append("\\r");
                else
                {
                    if (c < ' ')
                    {
                        string tmp = new string(c, 1);
                        t = "000" + int.Parse(tmp, System.Globalization.NumberStyles.HexNumber);
                        sb.Append("\\u" + t.Substring(t.Length - 4));
                    }
                    else
                    {
                        sb.Append(c);
                    }
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// Ensures the semi colon.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns></returns>
        public static string EnsureSemiColon(this string text)
        {
            if (text.IsEmpty())
            {
                return text;
            }

            return (string.IsNullOrEmpty(text) || text.EndsWith(";")) ? text : string.Concat(text, ";");
        }

        /// <summary>
        /// Wraps the specified text.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="wrapByText">The wrap by text.</param>
        /// <returns></returns>
        public static string Wrap(this string text, string wrapByText)
        {
            if (text == null)
            {
                text = "";
            }

            return wrapByText.ConcatWith(text, wrapByText);
        }

        /// <summary>
        /// Wraps the specified text.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="wrapStart">The wrap start.</param>
        /// <param name="wrapEnd">The wrap end.</param>
        /// <returns></returns>
        public static string Wrap(this string text, string wrapStart, string wrapEnd)
        {
            if (text == null)
            {
                text = "";
            }

            return wrapStart.ConcatWith(text, wrapEnd);
        }

        /// <summary>
        /// Tests the specified text.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="pattern">The pattern.</param>
        /// <returns></returns>
        public static bool Test(this string text, string pattern)
        {
            return Regex.IsMatch(text, pattern);
        }

        /// <summary>
        /// Tests the specified text.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="pattern">The pattern.</param>
        /// <param name="options">The options.</param>
        /// <returns></returns>
        public static bool Test(this string text, string pattern, RegexOptions options)
        {
            return Regex.IsMatch(text, pattern, options);
        }

        /// <summary>
        /// Truncate a string and add an ellipsis ('...') to the end if it exceeds the specified length
        /// </summary>
        /// <param name="text">The string to truncate</param>
        /// <param name="length">The maximum length to allow before truncating</param>
        /// <returns>
        /// The converted text
        /// </returns>
        public static string Ellipsis(this string text, int length)
        {
            return Ellipsis(text, length, false);
        }

        /// <summary>
        /// Truncate a string and add an ellipsis ('...') to the end if it exceeds the specified length
        /// </summary>
        /// <param name="text">The string to truncate</param>
        /// <param name="length">The maximum length to allow before truncating</param>
        /// <param name="word">True to try to find a common work break</param>
        /// <returns>
        /// The converted text
        /// </returns>
        public static string Ellipsis(this string text, int length, bool word)
        {
            if (text != null && text.Length > length)
            {
                if (word)
                {
                    string vs = text.Substring(0, length - 2);
                    int index = Math.Max(vs.LastIndexOf(' '), Math.Max(vs.LastIndexOf('.'), Math.Max(vs.LastIndexOf('!'), vs.LastIndexOf('?'))));

                    if (index == -1 || index < (length - 15))
                    {
                        return text.Substring(0, length - 3) + "...";
                    }

                    return vs.Substring(0, index) + "...";
                }

                return text.Substring(0, length - 3) + "...";
            }
            return text;
        }

        public static string EncodeBase64(this string text, Encoding encode)
        {
            byte[] bytes = encode.GetBytes(text);
            try
            {
                return Convert.ToBase64String(bytes, Base64FormattingOptions.None);
            }
            catch
            {
                return text;
            }
        }

        public static string EncodeBase64(this string text)
        {
            return EncodeBase64(text, Encoding.UTF8);
        }

        public static string DecodeBase64(this string text, Encoding encode)
        {
            string decode = "";
            byte[] bytes = Convert.FromBase64String(text);
            try
            {
                decode = encode.GetString(bytes);
            }
            catch
            {
                decode = text;
            }
            return decode;
        }

        public static string DecodeBase64(this string text)
        {
            return DecodeBase64(text, Encoding.UTF8);
        }

        public static string EncryptToKey(this string value)
        {
            try
            {
                //数字代表Base64字符串第几位是小写，A3代表第3位是+，B代表/，C代表=
                value = value.EncodeBase64();
                var key = "";

                var i = 0;
                foreach (var item in value.ToArray())
                {
                    if (char.IsLower(item))
                    {
                        Char.ToUpper(item);
                        key += i.ToString() + "D";//分隔符
                    }
                    if (item == '+')
                    {
                        key += "A" + i.ToString() + "D";//分隔符
                    }
                    if (item == '-')
                    {
                        key += "B" + i.ToString() + "D";//分隔符
                    }
                    if (item == '=')
                    {
                        key += "C" + i.ToString() + "D";//分隔符
                    }
                    i++;
                }
                return value.ToUpper().Replace("+", "X").Replace("/", "Y").Replace("=", "Z") + "-" + key.TrimEnd('D');

            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        public static string DecrypFromKey(this string value)
        {
            try
            {
                if (value.Contains("-"))
                {
                    var code = value.Split('-')[0];
                    var key = value.Split('-')[1];
                    var lstCodeChar = code.ToArray();
                    var lstKey = key.Split('D');
                    for (int i = 0; i < lstKey.Length; i++)
                    {
                        if (lstKey[i].StartsWith("A"))
                        {
                            lstCodeChar[lstKey[i].Remove("A").ToInt32()] = '+';
                        }
                        else if (lstKey[i].StartsWith("B"))
                        {
                            lstCodeChar[lstKey[i].Remove("B").ToInt32()] = '/';
                        }
                        else if (lstKey[i].StartsWith("C"))
                        {
                            lstCodeChar[lstKey[i].Remove("C").ToInt32()] = '=';
                        }
                        else
                        {
                            lstCodeChar[lstKey[i].ToInt32()] = char.ToLower(lstCodeChar[lstKey[i].ToInt32()]);
                        }
                    }
                    value = new string(lstCodeChar);
                    return value.DecodeBase64();
                }
                else
                {
                    return string.Empty;
                }
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Strips the whitespace chars.
        /// </summary>
        /// <param name="html">The HTML.</param>
        /// <returns></returns>
        public static string StripWhitespaceChars(this string html)
        {
            return Regex.Replace(html, "[\n\r\t]", "");
        }

        /// <summary>
        /// Strips the extra spaces.
        /// </summary>
        /// <param name="html">The HTML.</param>
        /// <returns></returns>
        public static string StripExtraSpaces(this string html)
        {
            return Regex.Replace(html, @"\s+", " ");
        }

        /// <summary>
        /// 从右边开始取i个字符
        /// </summary>
        /// <param name="text"></param>
        /// <param name="i"></param>
        /// <returns></returns>
        public static string SubRightString(this string text, int i)
        {
            return text.Substring(text.Length - i); // or str=str.Remove(0,str.Length-i);
        }

        /// <summary>
        /// 从字符串中删除指定相同部分
        /// </summary>
        /// <param name="text">字符串实例</param>
        /// <param name="part">指定相同部分</param>
        /// <returns></returns>
        public static string Remove(this string text, string part)
        {
            return text.Replace(part, "");
        }

        /// <summary>
        /// 从右边移除i个字符
        /// </summary>
        /// <param name="text"></param>
        /// <param name="i"></param>
        /// <returns></returns>
        public static string RemoveRight(this string text, int i)
        {
            return text.Substring(0, text.Length - i); // or str=str.Remove(str.Length-i,i);
        }

        /// <summary>
        /// 按指定个数分割字符串
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static List<String> Split(this string instance, int length)
        {
            var ret = new List<String>();
            if (length >= instance.Length && instance.IsNotEmpty())
            {
                ret.Add(instance);
            }
            else if (instance.IsNotEmpty())
            {
                while (length < instance.Length)
                {
                    ret.Add(instance.Substring(0, length));
                    instance = instance.Remove(0, length);
                }
                ret.Add(instance);
            }
            return ret;
        }

        public static int GetSubStringCount(this string instance, string substring)
        {
            return instance.Length - instance.Replace(substring, String.Empty).Length;
        }

        public static int GetByteCount(this string instance)
        {
            return System.Text.Encoding.UTF8.GetByteCount(instance);
        }

        public static int GetByteCount(this string instance, Encoding encoding)
        {
            return encoding.GetByteCount(instance);
        }

        public static string SubStringByByteCount(this string instance, int startIndex, int count)
        {
            int byteCount = System.Text.Encoding.UTF8.GetByteCount(instance);
            int charCount = instance.Length;
            int newByteCount = 0;
            int startPos = 0;
            int endPos = 0;
            //超过字节总数
            if (startIndex > byteCount)
            {
                return string.Empty;
            }
            //跳过前面的部分
            for (int i = 0; i < charCount; i++)
            {
                if (startPos >= startIndex)
                {
                    startPos = i;
                    break;
                }
                if (instance[i] > 255)
                    startPos += 2;
                else
                    startPos += 1;
            }

            if (byteCount - startPos <= count)
            {
                if (startPos > charCount)
                {
                    return string.Empty;
                }
                else
                {
                    return instance.Substring(startPos);
                }
            }
            endPos = startPos;
            for (int i = startPos; i < charCount; i++)
            {
                if (instance[i] > 255)
                    newByteCount += 2;
                else
                    newByteCount += 1;
                if (newByteCount == count)
                {
                    endPos = i + 1;
                    break;
                }
                else if (newByteCount > count)
                {
                    endPos = i;
                    break;
                }
            }
            return instance.Substring(startPos, endPos - startPos);
        }

        public static string[] SplitByByteCount(this string instance, int byteCount)
        {
            int count = instance.GetByteCount() / byteCount;
            if (instance.GetByteCount() % byteCount > 0)
            {
                count += 1;
            }
            var lst = new string[count];
            for (int i = 0; i < count; i++)
            {
                lst[i] = instance.SubStringByByteCount(byteCount * i, byteCount);
            }
            return lst;
        }

        public static string[] SplitByString(this string instance, string splitstr)
        {
            return Regex.Split(instance, splitstr, RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// 判断输入的字符串只包含汉字
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsChinese(this string input)
        {
            return IsMatch(@"^[\u4e00-\u9fa5]+$", input);
        }

        /// <summary>
        /// 匹配3位或4位区号的电话号码，其中区号可以用小括号括起来，
        /// 也可以不用，区号与本地号间可以用连字号或空格间隔，
        /// 也可以没有间隔
        /// \(0\d{2}\)[- ]?\d{8}|0\d{2}[- ]?\d{8}|\(0\d{3}\)[- ]?\d{7}|0\d{3}[- ]?\d{7}
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsPhone(this string input)
        {
            string pattern = "^\\(0\\d{2}\\)[- ]?\\d{8}$|^0\\d{2}[- ]?\\d{8}$|^\\(0\\d{3}\\)[- ]?\\d{7}$|^0\\d{3}[- ]?\\d{7}$";
            return IsMatch(pattern, input);
        }

        /// <summary>
        /// 判断输入的字符串是否是一个合法的手机号
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsMobilePhone(this string input)
        {
            return IsMatch(@"^13\\d{9}$", input);
        }

        /// <summary>
        /// 判断输入的字符串字包含英文字母
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsEnglish(this string input)
        {
            return IsMatch(@"^[A-Za-z]+$", input);
        }

        /// <summary>
        /// 判断输入的字符串是否是一个合法的Email地址
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsEmail(this string input)
        {
            string pattern = @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
            return IsMatch(input, pattern);
        }

        /// <summary>
        /// 判断输入的字符串是否只包含数字和英文字母
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsNumberAndEnglish(this string input)
        {
            return IsMatch(@"^[A-Za-z0-9]+$", input);
        }

        /// <summary>
        /// 判断输入的字符串是否是表示一个IP地址
        /// </summary>
        /// <param name="input">被比较的字符串</param>
        /// <returns>是IP地址则为True</returns>
        public static bool IsIPv4(this string input)
        {
            string[] IPs = input.Split('.');

            for (int i = 0; i < IPs.Length; i++)
            {
                if (!IsMatch(IPs[i], @"^\d+$"))
                {
                    return false;
                }
                if (Convert.ToUInt16(IPs[i]) > 255)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 判断输入的字符串是否是合法的IPV6 地址
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsIPV6(this string input)
        {
            string pattern = "";
            string temp = input;
            string[] strs = temp.Split(':');
            if (strs.Length > 8)
            {
                return false;
            }
            int count = GetStringCount(input, "::");
            if (count > 1)
            {
                return false;
            }
            else if (count == 0)
            {
                pattern = @"^([\da-f]{1,4}:){7}[\da-f]{1,4}$";
                return IsMatch(pattern, input);
            }
            else
            {
                pattern = @"^([\da-f]{1,4}:){0,5}::([\da-f]{1,4}:){0,5}[\da-f]{1,4}$";
                return IsMatch(pattern, input);
            }
        }

        /// <summary>
        /// 判断字符串compare 在 input字符串中出现的次数
        /// </summary>
        /// <param name="input">源字符串</param>
        /// <param name="compare">用于比较的字符串</param>
        /// <returns>字符串compare 在 input字符串中出现的次数</returns>
        public static int GetStringCount(this string input, string compare)
        {
            int index = input.IndexOf(compare);
            if (index != -1)
            {
                return 1 + GetStringCount(input.Substring(index + compare.Length), compare);
            }
            else
            {
                return 0;
            }

        }

        /// <summary>
        /// 调用Regex中IsMatch函数实现一般的正则表达式匹配
        /// </summary>
        /// <param name="pattern">要匹配的正则表达式模式。</param>
        /// <param name="input">要搜索匹配项的字符串</param>
        /// <returns>如果正则表达式找到匹配项，则为 true；否则，为 false。</returns>
        public static bool IsMatch(this string input, string pattern)
        {
            if (input == null || input == "") return false;
            Regex regex = new Regex(pattern);
            return regex.IsMatch(input);
        }

        public static T ToEnum<T>(this string instance, bool isCode = false)
        {
            if (!isCode)
            {
                return (T)Enum.Parse(typeof(T), instance);
            }
            else
            {
                var value = EnumAttribute.ToList(typeof(T)).FirstOrDefault(x => x.Code == instance).Value;
                return (T)Enum.Parse(typeof(T), value.ToString());
            }
        }
    }
}