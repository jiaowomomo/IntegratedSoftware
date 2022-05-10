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

namespace CommonLibrary.CodeSystem.Controls
{
    public partial class CustomReferenceForm : Form
    {
        private const string NAMESPACE_NODE = "Namespace";
        private const string FIELD_NODE = "Fields";
        private const string PROPERTY_NODE = "Properties";
        private const string METHOD_NODE = "Methods";

        public CustomReferenceForm()
        {
            InitializeComponent();
        }

        private void CustomReferenceForm_Load(object sender, EventArgs e)
        {
            InitListViewReference();
        }

        private void InitListViewReference()
        {
            listViewCustomReference.Items.Clear();
            foreach (string customReference in CodeManager.Instance.CustomReferences)
            {
                listViewCustomReference.Items.Add(customReference, customReference, null);
            }
        }

        private void listViewCustomReference_DoubleClick(object sender, EventArgs e)
        {
            ObtainDynamicLibrary();
        }

        private void ObtainDynamicLibrary()
        {
            if (listViewCustomReference.FocusedItem != null)
            {
                string strReference = listViewCustomReference.FocusedItem.Text;
                ShowDynamicLibraryInformation(strReference);
            }
        }

        private void ShowDynamicLibraryInformation(string strReference)
        {
            string strDLLName = strReference.Replace(".dll", string.Empty);
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            Assembly[] select = assemblies.Where(x => x.FullName.Contains(strDLLName)).ToArray();
            Assembly assembly = null;
            if (select.Length == 0)
            {
                try
                {
                    string strDLLPath;
                    strDLLPath = Path.Combine(Application.StartupPath, strReference);
                    if (File.Exists(strDLLPath))
                    {
                        assembly = Assembly.LoadFile(strDLLPath);
                    }
                    else
                    {
                        strDLLPath = Path.Combine(CodeManager.Instance.ThirdPartyDLLDirectory, strReference);
                        if (File.Exists(strDLLPath))
                        {
                            assembly = Assembly.LoadFile(strDLLPath);
                        }
                    }
                }
                catch
                {
                    assembly = null;
                }
            }
            else
            {
                assembly = select[0];
            }
            if (assembly != null)
            {
                treeViewReference.Nodes.Clear();
                try
                {
                    Type[] types = assembly.GetTypes();
                    treeViewReference.ImageIndex = 5;
                    treeViewReference.SelectedImageIndex = 6;
                    foreach (Type type in types)
                    {
                        treeViewReference.Nodes.Add(type.Name, type.Name);
                        treeViewReference.Nodes[type.Name].ImageIndex = 0;
                        treeViewReference.Nodes[type.Name].Nodes.Add(NAMESPACE_NODE, NAMESPACE_NODE);
                        treeViewReference.Nodes[type.Name].Nodes[NAMESPACE_NODE].ImageIndex = 1;
                        treeViewReference.Nodes[type.Name].Nodes.Add(FIELD_NODE, FIELD_NODE);
                        treeViewReference.Nodes[type.Name].Nodes[FIELD_NODE].ImageIndex = 2;
                        treeViewReference.Nodes[type.Name].Nodes.Add(PROPERTY_NODE, PROPERTY_NODE);
                        treeViewReference.Nodes[type.Name].Nodes[PROPERTY_NODE].ImageIndex = 3;
                        treeViewReference.Nodes[type.Name].Nodes.Add(METHOD_NODE, METHOD_NODE);
                        treeViewReference.Nodes[type.Name].Nodes[METHOD_NODE].ImageIndex = 4;
                        FieldInfo[] fieldInfos = type.GetFields();
                        PropertyInfo[] propertyInfos = type.GetProperties();
                        ConstructorInfo[] constructorInfos = type.GetConstructors();
                        MethodInfo[] methodInfos = type.GetMethods();
                        treeViewReference.Nodes[type.Name].Nodes[NAMESPACE_NODE].Nodes.Add(type.Namespace);
                        foreach (FieldInfo fieldInfo in fieldInfos)
                        {
                            treeViewReference.Nodes[type.Name].Nodes[FIELD_NODE].Nodes.Add($"{(fieldInfo.IsStatic ? "static " : string.Empty)}{(fieldInfo.IsLiteral ? "const " : string.Empty)}{(fieldInfo.IsInitOnly ? "readonly " : string.Empty)}{fieldInfo.FieldType.FullName} {fieldInfo.Name}");
                        }
                        foreach (PropertyInfo propertyInfo in propertyInfos)
                        {
                            MethodInfo methodInfo = propertyInfo.GetGetMethod();
                            treeViewReference.Nodes[type.Name].Nodes[PROPERTY_NODE].Nodes.Add($"{(methodInfo != null && methodInfo.IsStatic ? "static " : string.Empty)}{propertyInfo.PropertyType.FullName} {propertyInfo.Name} {(propertyInfo.CanRead ? "get;" : string.Empty)}{ (propertyInfo.CanWrite ? "set;" : string.Empty)}");
                        }
                        foreach (ConstructorInfo constructorInfo in constructorInfos)
                        {
                            treeViewReference.Nodes[type.Name].Nodes[METHOD_NODE].Nodes.Add($"{constructorInfo}");
                        }
                        foreach (MethodInfo methodInfo in methodInfos)
                        {
                            treeViewReference.Nodes[type.Name].Nodes[METHOD_NODE].Nodes.Add($"{(methodInfo.IsStatic ? "static " : string.Empty)}{methodInfo}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    string strError = $"无法查阅相关内容,原因:\r\n{ex.Message}\r\n";
                    if (ex.InnerException != null)
                    {
                        strError += $"{ex.InnerException.Message}\r\n";
                    }
                    if (ex is System.Reflection.ReflectionTypeLoadException typeLoadException)
                    {
                        foreach (Exception item in typeLoadException.LoaderExceptions)
                        {
                            strError += $"{item.Message}\r\n";
                        }
                    }
                    MessageBox.Show(strError, "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void CustomReferenceForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

        private void toolStripMenuItemObtain_Click(object sender, EventArgs e)
        {
            ObtainDynamicLibrary();
        }

        private void toolStripMenuItemRefresh_Click(object sender, EventArgs e)
        {
            InitListViewReference();
        }

        private void treeViewReference_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.C)
            {
                if (treeViewReference.SelectedNode != null)
                {
                    Clipboard.SetDataObject(treeViewReference.SelectedNode.Text);
                }
            }
        }
    }
}
