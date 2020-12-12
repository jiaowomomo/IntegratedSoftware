using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace AutomationSystem.Base
{
    public static class Function
    {
        public static void LoadFormPlugins(string fileName, ref ToolStrip toolStrip, EventHandler executeMethod)
        {
            //搜索plugins目录下的所有的dll文件 
            string[] dlls = Directory.GetFiles(fileName, "*.dll");
            //循环将每个dll文件都加载起来
            foreach (string dllPath in dlls)
            {
                try
                {
                    //动态加载当前循环的dll文件
                    Assembly assembly = Assembly.LoadFile(dllPath);
                    //获取当前dll中的所有的public类型
                    Type[] types = assembly.GetExportedTypes();
                    //获取IEditor接口的Type
                    Type typeIEditor = typeof(IFormMenu);

                    for (int i = 0; i < types.Length; i++)
                    {
                        //验证当前的类型即实现了IEditor接口并且该类型还可以被实例化
                        if (typeIEditor.IsAssignableFrom(types[i]) && !types[i].IsAbstract)
                        {
                            IFormMenu menu = (IFormMenu)Activator.CreateInstance(types[i]);
                            //向菜单栏中动态添加一个菜单项
                            ToolStripDropDownButton tsddb = new ToolStripDropDownButton(menu.MainToolStrip);
                            tsddb.Name = "ToolStripDropDownButton" + menu.MainToolStrip;
                            tsddb.DisplayStyle = ToolStripItemDisplayStyle.Text;
                            tsddb.ShowDropDownArrow = false;
                            if (!toolStrip.Items.ContainsKey(tsddb.Name))
                            {
                                toolStrip.Items.AddRange(new ToolStripItem[] { tsddb });
                            }
                            int index = toolStrip.Items.IndexOfKey(tsddb.Name);
                            ToolStripItem toolStripItem = new ToolStripMenuItem(menu.SubToolStrip);
                            ((ToolStripDropDownButton)toolStrip.Items[index]).DropDownItems.AddRange(new ToolStripItem[] { toolStripItem });
                            //为刚刚增加的菜单项注册一个单击事件
                            toolStripItem.Click += executeMethod;
                            toolStripItem.Tag = menu;
                        }
                    }
                }
                catch
                { }
            }
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
