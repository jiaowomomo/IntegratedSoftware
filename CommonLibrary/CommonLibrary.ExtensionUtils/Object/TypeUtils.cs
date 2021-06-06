using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace CommonLibrary.ExtensionUtils
{
    public static class TypeUtils
    {
        public static bool IsBetween<T>(this T t, T lowerBound, T upperBound, bool includeLowerBound = true, bool includeUpperBound = true)
        where T : IComparable<T>
        {
            if (t == null) throw new ArgumentNullException("t");

            if (lowerBound.CompareTo(upperBound) > 0)
            {
                var l = lowerBound;
                lowerBound = upperBound;
                upperBound = l;
            }

            var lowerCompareResult = t.CompareTo(lowerBound);
            var upperCompareResult = t.CompareTo(upperBound);

            return (includeLowerBound && lowerCompareResult == 0) || (includeUpperBound && upperCompareResult == 0) || (lowerCompareResult > 0 && upperCompareResult < 0);
        }

        public static PropertyInfo GetPropByDescription(this Type type, string des)
        {
            var fields = type.GetProperties();

            var descs = new Dictionary<int, string>();
            for (int i = 0; i < fields.Length - 1; i++)
            {
                var attr = fields[i].GetCustomAttributes(true).FirstOrDefault<object>(x =>
                {
                    return typeof(ExtendAttribute).IsAssignableFrom(x.GetType()) && ((ExtendAttribute)x).Description == des;
                });
                if (attr.IsNotNull())
                {
                    return fields[i];
                }
            }

            return null;
        }

        public static bool IsTimeSpan(this PropertyInfo prop)
        {
            var attr = prop.GetCustomAttributes(true).FirstOrDefault<object>(x =>
            {
                return typeof(ExtendAttribute).IsAssignableFrom(x.GetType()) && ((ExtendAttribute)x).IsTimeSpan;
            });

            return attr.IsNotNull();
        }

        public static bool IsData(this PropertyInfo prop)
        {
            var attr = prop.GetCustomAttributes(true).FirstOrDefault<object>(x =>
            {
                return typeof(ExtendAttribute).IsAssignableFrom(x.GetType()) && ((ExtendAttribute)x).IsData;
            });

            return attr.IsNotNull();
        }
    }
}
