using CommonLibrary.ExtensionUtils;
using CommonLibrary.Manager;
using Halcon.Functions;
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
        private static readonly string m_strExternToolPath = Path.Combine(Application.StartupPath, "ExternTool");
        private static readonly Lazy<ToolForm> m_instance = new Lazy<ToolForm>(() => new ToolForm());

        public static ToolForm Instance { get => m_instance.Value; }

        private Dictionary<string, IImageHalconObject> m_dicObject = new Dictionary<string, IImageHalconObject>();

        private ToolForm()
        {
            InitializeComponent();

            LoadFormExternTool(m_strExternToolPath);
        }

        private void TreeView1_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            TreeNode treeNode = e.Node as TreeNode;
            if (treeNode.Parent != null)
            {
                Type type = m_dicObject[treeNode.Name].GetType();
                IImageHalconObject halconObject = (IImageHalconObject)Activator.CreateInstance(type);
                halconObject.SetParameters();
                if (halconObject.IsSetupOK)
                {
                    GlobalProcessManager.AddProcess<IImageHalconObject>(GlobalImageProcessControl.ImageKeyName, halconObject, GlobalImageProcessControl.SelectedImageIndex);
                }
            }
        }

        private void LoadFormExternTool(string fileFullPath)
        {
            List<IImageHalconObject> imageHalconObjects = Function.LoadFormPlugins<IImageHalconObject>(fileFullPath);
            foreach (var imageHalconObject in imageHalconObjects)
            {
                m_dicObject.Add(imageHalconObject.ToolName(), imageHalconObject);
                //向菜单栏中动态添加一个菜单项
                TreeNode mainNode = new TreeNode();
                mainNode.Name = imageHalconObject.ToolType();
                mainNode.Text = imageHalconObject.ToolType();
                if (!treeView1.Nodes.ContainsKey(mainNode.Name))
                {
                    TreeNode subNode = new TreeNode();
                    subNode.Name = imageHalconObject.ToolName();
                    subNode.Text = imageHalconObject.ToolName();
                    mainNode.Nodes.Add(subNode);
                    treeView1.Nodes.Add(mainNode);
                }
                else
                {
                    TreeNode[] nodes = treeView1.Nodes.Find(mainNode.Name, false);
                    TreeNode subNode = new TreeNode();
                    subNode.Name = imageHalconObject.ToolName();
                    subNode.Text = imageHalconObject.ToolName();
                    nodes[0].Nodes.Add(subNode);
                }
            }
        }

        private void TreeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            TreeNode treeNode = e.Node as TreeNode;
            if (treeNode.Parent != null)
            {
                lbTip.Text = m_dicObject[treeNode.Name].ToolDescriptText();
            }
        }
    }
}
