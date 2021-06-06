using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonLibrary.ExtensionUtils
{
    public static class BooleanUtils
    {
        /// <summary>
        /// 布尔转整形
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static int ToInt32(this bool instance)
        {
            return instance ? 1 : 0;
        }

        /// <summary>
        /// 布尔转十六进制
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static string ToHex(this bool instance)
        {
            return instance.ToInt32().ToHex();
        }

        public static string ToHex(this bool instance, int paddingWidth)
        {
            return instance.ToInt32().ToHex(paddingWidth);
        }

        /// <summary>
        /// 布尔转中文
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static string ToChineseName(this bool instance)
        {
            return instance ? "是" : "否";
        }

        /// <summary>
        /// 布尔转别名
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="trueAlias"></param>
        /// <param name="falseAlias"></param>
        /// <returns></returns>
        public static string ToAlias(this bool instance, string trueAlias, string falseAlias)
        {
            return instance ? trueAlias : falseAlias;
        }
    }
}
