using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace CommonLibrary.ExtensionUtils
{
    public static class Function
    {
        public static List<T> LoadFormPlugins<T>(string fileFullPath)
        {
            List<T> plugins = new List<T>();
            //搜索plugins目录下的所有的dll文件 
            string[] dlls = Directory.GetFiles(fileFullPath, "*.dll");
            //获取IEditor接口的Type
            Type typeIEditor = typeof(T);
            //循环将每个dll文件都加载起来
            foreach (string dllPath in dlls)
            {
                try
                {
                    //动态加载当前循环的dll文件
                    Assembly assembly = Assembly.LoadFile(dllPath);
                    //获取当前dll中的所有的public类型
                    Type[] types = assembly.GetExportedTypes();
                    for (int i = 0; i < types.Length; i++)
                    {
                        //验证当前的类型即实现了IEditor接口并且该类型还可以被实例化
                        if (typeIEditor.IsAssignableFrom(types[i]) && !types[i].IsAbstract)
                        {
                            T plugin = (T)Activator.CreateInstance(types[i]);
                            plugins.Add(plugin);
                        }
                    }
                }
                catch
                { }
            }
            return plugins;
        }

        public static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs e)
        {
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            Assembly[] select = assemblies.Where(x => x.FullName == e.Name).ToArray();
            if (select.Length == 0)
            {
                return null;
            }
            else
            {
                return select[0];
            }
        }
    }
}
