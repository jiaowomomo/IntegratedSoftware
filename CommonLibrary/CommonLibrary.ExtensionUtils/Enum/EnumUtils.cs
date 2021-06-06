using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace CommonLibrary.ExtensionUtils
{
    public static class EnumUtils
    {
        /// <summary>
        /// 转字节
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static byte ToByte(this Enum instance)
        {
            var type = instance.GetType();
            var name = type.GetEnumName(instance);
            return (byte)(int)Enum.Parse(type, name);
        }

        /// <summary>
        /// 转整形
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static int ToInt32(this Enum instance)
        {
            var type = instance.GetType();
            var name = type.GetEnumName(instance);
            return (int)Enum.Parse(type, name);
        }

        /// <summary>
        /// 获取描述名字
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static string GetDescription(this Enum instance)
        {
            var type = instance.GetType();
            var desAttr = EnumAttribute.ToList(type).FirstOrDefault(x => x.Name == type.GetEnumName(instance));
            if (desAttr != null)
            {
                return desAttr.Description;
            }
            else
            {
                var fi = type.GetField(instance.ToString());
                var attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (attributes != null && attributes.Length > 0)
                {
                    return attributes[0].Description;
                }
                else
                {
                    return instance.ToString();
                }
            }
        }

        public static EnumAttr GetEnumAttr(this Enum instance)
        {
            var type = instance.GetType();
            return EnumAttribute.ToList(type).FirstOrDefault(x => x.Name == type.GetEnumName(instance));
        }

        public static string GetEnumName(this Enum instance)
        {
            return Enum.GetName(instance.GetType(), instance);
        }
    }
}
