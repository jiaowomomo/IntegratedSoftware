using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLibrary.ExtensionUtils
{
    public static class RegistryUtils
    {
        /// <summary>
        /// 获取注册表列表
        /// </summary>
        /// <param name="rk"></param>
        /// <returns></returns>
        public static Dictionary<string,object> GetValueList(this RegistryKey rk)
        {
            var lst = new Dictionary<string, object>();
            rk.GetValueNames().ToList().ForEach(x => lst.Add(x, rk.GetValue(x, null)));            
            return lst;
        }
    }
}
