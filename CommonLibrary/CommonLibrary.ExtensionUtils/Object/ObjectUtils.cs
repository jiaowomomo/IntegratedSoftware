using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Xml.Serialization;

namespace CommonLibrary.ExtensionUtils
{
    public static class ObjectUtils
    {
        /// <summary>
        /// object转指定类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static T As<T>(this object source) where T : class
        {
            return source as T;
        }

        /// <summary>
        /// 判断Object实例是否为null。
        /// </summary>
        /// <param name="instance">Object实例</param>
        /// <returns>
        ///   <c>true</c> 如果Object实例为null; 否则, <c>false</c>.
        /// </returns>
        public static bool IsNull(this object instance)
        {
            if (instance != null && instance.GetType() == typeof(string) && string.IsNullOrEmpty(instance.ToString()))
            {
                return true;
            }
            return instance == null;
        }

        /// <summary>
        /// 判断Object实例是否为null
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static bool IsNotNull(this object instance)
        {
            return !IsNull(instance);
        }

        /// <summary>
        /// 转字节
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static byte ToByte(this object instance)
        {
            if (instance.ToString().IsNumeric())
            {
                if (instance.ToString().ToInt32() >= 0 && instance.ToString().ToInt32() < 256)
                    return byte.Parse(instance.ToString());
                else
                    return 0;
            }
            else
                return 0;
        }

        /// <summary> 
        /// 将一个object对象序列化，返回一个byte[]         
        /// </summary> 
        /// <param name="source">能序列化的对象</param>         
        /// <returns></returns> 
        public static byte[] SerializeToBytes(this object source)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                IFormatter formatter = new BinaryFormatter();
                formatter.Serialize(ms, source);
                return ms.GetBuffer();
            }
        }

        /// <summary> 
        /// 将一个序列化后的byte[]数组还原         
        /// </summary>
        /// <param name="source"></param>         
        /// <returns></returns> 
        public static T DeserializeToObject<T>(this byte[] source)
        {
            using (MemoryStream ms = new MemoryStream(source))
            {
                IFormatter formatter = new BinaryFormatter();
                return (T)formatter.Deserialize(ms);
            }
        }

        /// <summary>
        /// 把整数型的对象值转换为整形
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static int AsInt32(this object instance)
        {
            if (instance.IsNotNull())
            {
                if ((instance.GetType() == typeof(string) || instance.GetType() == typeof(byte)) && instance.ToString().IsInteger())
                {
                    return instance.ToString().ToInt32();
                }
                else
                {
                    return Convert.ToInt32(instance);
                }
            }
            return 0;
        }

        /// <summary>
        /// 转十进制
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static decimal ToDecimal(this object instance)
        {
            if (instance.IsNotNull() && instance != DBNull.Value)
            {
                return Convert.ToDecimal(instance);
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// 转整形
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static Int64 ToInt64(this object instance)
        {
            if (instance.IsNotNull() && instance.ToString().IsInteger())
            {
                return instance.ToString().ToInt64();
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// 返回对象属性名字
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="propertyValue"></param>
        /// <returns></returns>
        public static string GetPropertyName(this object obj, object propertyValue)
        {
            Type t = obj.GetType();
            foreach (PropertyInfo pi in t.GetProperties())
            {
                object value = pi.GetValue(obj, null);
                string name = pi.Name;
                if (value == propertyValue)
                {
                    return pi.Name;
                }
            }
            return "";
        }

        public static object Clone(this object source)
        {
            Object copyObj = null;
            if (source.IsNull())
            {
                return null;
            }
            if (source.GetType().IsValueType == true)//值类型
            {
                copyObj = source;
            }
            else//引用类型
            {
                copyObj = System.Activator.CreateInstance(source.GetType()); //创建引用对象
                var memberCollection = source.GetType().GetMembers();
                foreach (System.Reflection.MemberInfo member in memberCollection)
                {
                    if (member.MemberType == System.Reflection.MemberTypes.Field)
                    {
                        var field = (System.Reflection.FieldInfo)member;
                        var fieldValue = field.GetValue(source);
                        if (fieldValue is ICloneable)
                        {
                            field.SetValue(copyObj, (fieldValue as ICloneable).Clone());
                        }
                        else
                        {
                            field.SetValue(copyObj, Clone(fieldValue));
                        }
                    }
                    else if (member.MemberType == System.Reflection.MemberTypes.Property)
                    {
                        var prop = (System.Reflection.PropertyInfo)member;
                        var info = prop.GetSetMethod(false);
                        if (info != null)
                        {
                            if (prop.GetIndexParameters().Length == 0)
                            {
                                var propertyValue = prop.GetValue(source, null);
                                if (propertyValue is ICloneable)
                                {
                                    prop.SetValue(copyObj, (propertyValue as ICloneable).Clone(), null);
                                }
                                else
                                {
                                    prop.SetValue(copyObj, Clone(propertyValue), null);
                                }
                            }
                        }

                    }
                }
            }
            return copyObj;
        }

        /// <summary>
        /// 对复杂类型的相同字段赋值到另一个类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static T Clone<T>(this object source)
        {
            using (Stream stream = new MemoryStream()) // 初始化一个 流对象
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T)); //将要序列化的对象序列化到xml文档（Formatter）
                serializer.Serialize(stream, source); //将序列后的对象写入到流中
                stream.Seek(0, SeekOrigin.Begin);
                return (T)serializer.Deserialize(stream);// 反序列化得到新的对象
            }
        }
    }
}