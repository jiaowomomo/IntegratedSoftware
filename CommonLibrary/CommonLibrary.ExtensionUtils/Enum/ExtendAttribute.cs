using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CommonLibrary.ExtensionUtils
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class ExtendAttribute : Attribute
    {
        /// <summary>
        /// 默认描述
        /// </summary>
        public string Description { get; set; }

        public bool IsTimeSpan { get; set; }

        public bool IsData { get; set; }

        /// <summary>
        /// 获取指定类的所有描述
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Dictionary<int, string> GetDescriptions(Type type)
        {
            if (type == null)
            {
                throw new Exception("type 必须是一个类型！");
            }
            var fields = type.GetProperties();

            var descs = new Dictionary<int, string>();
            for (int i = 0; i < fields.Length - 1; i++)
            {
                var attr = fields[i].GetCustomAttributes(true).FirstOrDefault<object>(x => { return typeof(ExtendAttribute).IsAssignableFrom(x.GetType()); });
                if (attr.IsNotNull())
                {
                    descs.Add(i, ((ExtendAttribute)attr).Description);
                }
            }

            return descs;
        }
    }
}
