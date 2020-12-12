using AutomationSystem.GlobalObject;
using AutomationSystem.Halcon;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace AutomationSystem
{
    public partial class ToolForm : DockContent
    {
        private static ToolForm _instance = null;

        public static ToolForm Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ToolForm();
                }
                return _instance;
            }
        }

        private Dictionary<string, IImageHalconObject> m_dicObject = new Dictionary<string, IImageHalconObject>();

        private ToolForm()
        {
            InitializeComponent();

            LoadFormExternTool(Application.StartupPath + @"\ExternTool");
        }

        private void treeView1_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            TreeNode treeNode = e.Node as TreeNode;
            if (treeNode.Parent != null)
            {
                int A = m_dicObject[treeNode.Name].GetHashCode();
                Type type = m_dicObject[treeNode.Name].GetType();
                IImageHalconObject halconObject = (IImageHalconObject)Activator.CreateInstance(type);
                A = halconObject.GetHashCode();
                halconObject.SetParameters();
                if (halconObject.IsSetupOK)
                {
                    GlobalObjectList.ImageListObject[GlobalObjectList.nSelectIndex].AddProcess(halconObject);
                }
            }
        }

        private void LoadFormExternTool(string fileName)
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
                    Type typeIEditor = typeof(IImageHalconObject);
                    for (int i = 0; i < types.Length; i++)
                    {
                        //验证当前的类型即实现了IEditor接口并且该类型还可以被实例化
                        if (typeIEditor.IsAssignableFrom(types[i]) && !types[i].IsAbstract)
                        {
                            IImageHalconObject halconObject = (IImageHalconObject)Activator.CreateInstance(types[i]);
                            m_dicObject.Add(halconObject.ToolName(), halconObject);
                            //向菜单栏中动态添加一个菜单项
                            TreeNode mainNode = new TreeNode();
                            mainNode.Name = halconObject.ToolType();
                            mainNode.Text = halconObject.ToolType();
                            if (!treeView1.Nodes.ContainsKey(mainNode.Name))
                            {
                                TreeNode subNode = new TreeNode();
                                subNode.Name = halconObject.ToolName();
                                subNode.Text = halconObject.ToolName();
                                mainNode.Nodes.Add(subNode);
                                treeView1.Nodes.Add(mainNode);
                            }
                            else
                            {
                                TreeNode[] nodes = treeView1.Nodes.Find(mainNode.Name, false);
                                TreeNode subNode = new TreeNode();
                                subNode.Name = halconObject.ToolName();
                                subNode.Text = halconObject.ToolName();
                                nodes[0].Nodes.Add(subNode);
                            }
                        }
                    }
                }
                catch
                { }
            }
        }

        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            TreeNode treeNode = e.Node as TreeNode;
            if (treeNode.Parent != null)
            {
                lbTip.Text = m_dicObject[treeNode.Name].ToolDescriptText();
            }
        }
    }
}
