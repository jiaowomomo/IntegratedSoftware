using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLibrary.ExtensionUtils
{
    /// <summary>
    /// 枚举扩展类，用于为枚举添加描述修饰符
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class EnumAttribute : Attribute
    {
        /// <summary>
        /// 默认描述
        /// </summary>
        public string Description { get; set; }
        public string Code { get; set; }

        public bool IsRequireInputParameter { get; set; }

        /// <summary>
        /// 获取指定枚举值的描述
        /// </summary>
        /// <param name="enumValue"></param>
        /// <returns></returns>
        public virtual string GetDescription(object enumValue)
        {
            if (enumValue == null)
            {
                throw new Exception("枚举值不能为空！");
            }
            return Description ?? enumValue.ToString();
        }

        /// <summary>
        /// 获取指定枚举值的特征类
        /// </summary>
        /// <param name="enumValue"></param>
        /// <returns></returns>
        public virtual EnumAttribute GetAttribute(object enumValue)
        {
            if (enumValue == null)
            {
                throw new Exception("枚举值不能为空！");
            }
            return new EnumAttribute
            {
                Description = Description,
                IsRequireInputParameter = IsRequireInputParameter,
                Code = Code
            };
        }

        /// <summary>
        /// 根据枚举类型和指定的枚举整形值获取枚举描述
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="enumIntValue"></param>
        /// <returns></returns>
        public static string GetDescription<T>(int enumIntValue)
        {
            return EnumAttribute.ToList<T>().FirstOrDefault(x => { return x.Value == enumIntValue; }).Description;
        }

        /// <summary>
        /// 根据枚举类型和指定的枚举整形值获取枚举描述
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="enumIntValue"></param>
        /// <returns></returns>
        public static string GetDescription(Type enumType, int enumIntValue)
        {
            return EnumAttribute.ToList(enumType.GetType()).FirstOrDefault(x => { return x.Value == enumIntValue; }).Description;
        }

        /// <summary>
        /// 根据枚举名称获取枚举描述
        /// </summary>
        /// <param name="Name">枚举名称</param>
        /// <returns></returns>
        public static string GetDescription<T>(string Name)
        {
            try
            {
                return GetDescription<T>(int.Parse(string.Format("{0:D}", Enum.Parse(typeof(T), Name))));
            }
            catch (Exception)
            {
                return "";
            }
        }

        public static List<EnumAttr> ToList<T>(bool isAddDefaultItem = false)
        {
            return ToList(typeof(T), isAddDefaultItem);
        }

        public static List<EnumAttr> ToList(Type enumType, bool isAddDefaultItem = false)
        {
            if (enumType == null)
            {
                throw new Exception("enumType必须是类型");
            }
            var lst = new List<EnumAttr>();
            if (isAddDefaultItem)
            {
                lst.Add(new EnumAttr()
                {
                    Value = -1,
                    Description = "--请选择--"
                });
            }

            enumType.GetFields().Where(x => x.IsStatic).ToList().ForEach(field =>
            {
                var fieldValue = Enum.Parse(enumType, field.Name);
                var attrs = field.GetCustomAttributes(true);
                var lstAttrs = attrs.Where(attr => { return typeof(EnumAttribute).IsAssignableFrom(attr.GetType()); }).ToList();
                if (lstAttrs.Count > 0)
                {
                    lstAttrs.ForEach(attr =>
                    {
                        var ins = (EnumAttribute)attr;
                        lst.Add(new EnumAttr
                        {
                            Value = (int)fieldValue,
                            Name = field.Name,
                            Description = ins.Description,
                            Code = ins.Code,
                            IsRequireInputParameter = ins.IsRequireInputParameter
                        });

                    });
                }
                else
                {
                    lst.Add(new EnumAttr
                    {
                        Value = (int)fieldValue,
                        Description = fieldValue.ToString(),
                    });
                }
            });
            return lst;
        }
    }

    public class EnumAttr
    {
        public int Value { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public bool IsRequireInputParameter { get; set; }
    }
}
