using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using AutomationSystem.Base;
using AutomationSystem.GlobalObject;
using AutomationSystem.Halcon;
using AutomationSystem.Manager;
using AutomationSystem.UI;
using HalconDotNet;
using WeifenLuo.WinFormsUI.Docking;

namespace AutomationSystem
{
    public partial class SystemMainForm : Form
    {
        private static string _dockpanelConfigFile = Application.StartupPath + @"\default.jw";
        private ProcessManager<string> m_listWindows = new ProcessManager<string>();//新建窗口管理
        private ProcessManager<int> m_listSelectWindows = new ProcessManager<int>();//展示结果窗口索引
        private int nWindowsCount = 0;
        private int nLoadedWindowsCount = 0;
        private int nFixedFormCount = 3;
        private string strCurrentFile = "";

        public SystemMainForm()
        {
            InitializeComponent();

            for (int i = 0; i < 20; i++)
            {
                m_listSelectWindows.AddProcess(0);
                GlobalObjectList.ImageListObject.Add(new ProcessManager<IImageHalconObject>());
            }
            AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(Function.CurrentDomain_AssemblyResolve);
        }

        private void SystemMainForm_Load(object sender, EventArgs e)
        {
            try
            {
                if (File.Exists(Application.StartupPath + @"\history\FilePath.txt"))
                {
                    using (StreamReader sr = new StreamReader(Application.StartupPath + @"\history\FilePath.txt"))
                    {
                        strCurrentFile = sr.ReadLine();
                    }
                    if (strCurrentFile != "")
                    {
                        OpenProject(strCurrentFile);
                    }
                    else
                    {
                        NewProject();
                    }
                }
                else if (File.Exists(_dockpanelConfigFile))
                {
                    NewProject();
                }
                else
                {
                    ResetFormLocation();
                }
            }
            catch (Exception)
            {
                ResetFormLocation();
            }

            LoadToolMenu();
            GlobalObjectList.OnFinish += OnFinish;

            this.WindowState = FormWindowState.Normal;
        }

        private void ResetFormLocation()
        {
            ToolForm.Instance.Show(dockPanelMain, DockState.DockRight);
            MessageForm.Instance.Show(dockPanelMain, DockState.DockBottom);
            ProcessForm.Instance.Show(dockPanelMain, DockState.DockLeft);
        }

        private void LoadToolMenu()
        {
            //加载视图
            LoadViewMenu();

            //加载窗口
            LoadWindowMenu();

            //加载插件
            LoadPlugins();
        }

        private void LoadPlugins()
        {
            Function.LoadFormPlugins(Application.StartupPath + @"\Calibration", ref this.toolStripTextTool, tool_Clcik);
        }

        private void LoadWindowMenu()
        {
            toolStripDropDownButtonWindow.DropDownItems.Clear();
            for (int i = nFixedFormCount; i < dockPanelMain.Contents.Count; i++)
            {
                ToolStripMenuItem menuItem = new ToolStripMenuItem(dockPanelMain.Contents[i].DockHandler.TabText);
                menuItem.Checked = !dockPanelMain.Contents[i].DockHandler.IsHidden;
                dockPanelMain.Contents[i].DockHandler.CloseButtonVisible = false;
                dockPanelMain.Contents[i].DockHandler.CloseButton = false;
                dockPanelMain.Contents[i].DockHandler.HideOnClose = true;
                ((ToolStripDropDownButton)toolStripTextTool.Items["toolStripDropDownButtonWindow"]).DropDownItems.AddRange(new ToolStripItem[] { menuItem });
                //为刚刚增加的菜单项注册一个单击事件
                menuItem.Click += toolWindow_Click;
            }
        }

        private void LoadViewMenu()
        {
            toolStripDropDownButtonView.DropDownItems.Clear();
            for (int i = 0; i < dockPanelMain.Contents.Count - nWindowsCount; i++)
            {
                ToolStripMenuItem menuItem = new ToolStripMenuItem(dockPanelMain.Contents[i].DockHandler.TabText);
                menuItem.Checked = !dockPanelMain.Contents[i].DockHandler.IsHidden;
                dockPanelMain.Contents[i].DockHandler.CloseButtonVisible = false;
                dockPanelMain.Contents[i].DockHandler.CloseButton = false;
                dockPanelMain.Contents[i].DockHandler.HideOnClose = true;
                ((ToolStripDropDownButton)toolStripTextTool.Items["toolStripDropDownButtonView"]).DropDownItems.AddRange(new ToolStripItem[] { menuItem });
                //为刚刚增加的菜单项注册一个单击事件
                menuItem.Click += toolView_Click;
            }
        }

        private void tool_Clcik(object sender, EventArgs e)
        {
            ToolStripItem item = sender as ToolStripItem;
            if (item != null)
            {
                if (item.Tag != null)
                {
                    IFormMenu menu = item.Tag as IFormMenu;
                    if (menu != null)
                    {
                        // 运行该插件
                        menu.ExecuteTool();
                    }
                }
            }
        }

        private void toolView_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem tsm = sender as ToolStripMenuItem;
            tsm.Checked = !tsm.Checked;
            int index = toolStripDropDownButtonView.DropDownItems.IndexOf(tsm);
            if (index != -1)
            {
                if (tsm.Checked)
                {
                    dockPanelMain.Contents[index].DockHandler.Show();
                }
                else
                {
                    dockPanelMain.Contents[index].DockHandler.Hide();
                }
            }
        }

        private void toolWindow_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem tsm = sender as ToolStripMenuItem;
            tsm.Checked = !tsm.Checked;
            int index = toolStripDropDownButtonWindow.DropDownItems.IndexOf(tsm);
            if (index != -1)
            {
                if (tsm.Checked)
                {
                    dockPanelMain.Contents[nFixedFormCount + index].DockHandler.Show();
                }
                else
                {
                    dockPanelMain.Contents[nFixedFormCount + index].DockHandler.Hide();
                }
            }
        }

        private void SystemMainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            //dockPanelMain.SaveAsXml(_dockpanelConfigFile);

            if (!Directory.Exists(Application.StartupPath + @"\history"))
            {
                Directory.CreateDirectory(Application.StartupPath + @"\history");
            }
            using (StreamWriter sw = new StreamWriter(Application.StartupPath + @"\history\FilePath.txt"))
            {
                sw.WriteLine(strCurrentFile);
            }
        }

        private IDockContent GetDeserializeDockContent(string persistString)
        {
            if (persistString == typeof(MessageForm).ToString())
                return MessageForm.Instance;
            if (persistString == typeof(ToolForm).ToString())
                return ToolForm.Instance;
            if (persistString == typeof(WindowForm).ToString())
            {
                WindowForm windowForm = new WindowForm();
                windowForm.Text = m_listWindows.ProcessList[nLoadedWindowsCount];
                nLoadedWindowsCount++;
                return windowForm;
            }
            if (persistString == typeof(ProcessForm).ToString())
                return ProcessForm.Instance;
            return null;
        }

        private void toolStripMenuItemNewWindow_Click(object sender, EventArgs e)
        {
            WindowForm windowForm = new WindowForm();
            nWindowsCount++;
            windowForm.Text = "窗口" + nWindowsCount.ToString();
            m_listWindows.AddProcess(windowForm.Text);
            windowForm.Show(dockPanelMain, DockState.Document);
            LoadWindowMenu();
        }

        private void toolStripMenuItemNewProject_Click(object sender, EventArgs e)
        {
            try
            {
                NewProject();
                LoadViewMenu();
                LoadWindowMenu();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void NewProject()
        {
            //创建缓冲文件夹
            if (!Directory.Exists(Application.StartupPath + @"\temp"))
            {
                Directory.CreateDirectory(Application.StartupPath + @"\temp");
            }
            ZipHelper.UnZip(Application.StartupPath + @"\default.jw", Application.StartupPath + @"\temp");
            m_listWindows.Load(Application.StartupPath + @"\temp\Window.config");
            nWindowsCount = m_listWindows.ProcessList.Count;
            nLoadedWindowsCount = 0;
            m_listSelectWindows.Load(Application.StartupPath + @"\temp\Select.config");
            InitDockPanel();
            dockPanelMain.LoadFromXml(Application.StartupPath + @"\temp\DockManagerDefault.config", new DeserializeDockContent(GetDeserializeDockContent));
            for (int i = 0; i < 20; i++)
            {
                GlobalObjectList.ImageListObject[i].Load(Application.StartupPath + @"\temp\Process" + i.ToString() + ".handle");
            }
            this.Text = "AutomationSystem";
            strCurrentFile = "";
        }

        private void InitDockPanel()
        {
            for (int i = nFixedFormCount; i < this.dockPanelMain.Contents.Count; i = i)
            {
                dockPanelMain.Contents[i].DockHandler.Close();
            }
            this.panel1.Controls.Remove(this.dockPanelMain);
            this.dockPanelMain = new DockPanel();
            this.dockPanelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dockPanelMain.DocumentStyle = WeifenLuo.WinFormsUI.Docking.DocumentStyle.DockingWindow;
            this.dockPanelMain.Location = new System.Drawing.Point(0, 0);
            this.dockPanelMain.Name = "dockPanelMain";
            this.dockPanelMain.Size = new System.Drawing.Size(1166, 629);
            this.dockPanelMain.TabIndex = 5;
            this.dockPanelMain.Theme = this.vS2012LightTheme1;
            this.panel1.Controls.Add(this.dockPanelMain);
        }

        private void toolStripMenuItemOpenProject_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Filter = "流程文件|*.jw";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    OpenProject(ofd.FileName);
                    LoadViewMenu();
                    LoadWindowMenu();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void OpenProject(string filePath)
        {
            //创建缓冲文件夹
            if (!Directory.Exists(Application.StartupPath + @"\temp"))
            {
                Directory.CreateDirectory(Application.StartupPath + @"\temp");
            }
            ZipHelper.UnZip(filePath, Application.StartupPath + @"\temp");
            m_listWindows.Load(Application.StartupPath + @"\temp\Window.config");
            nWindowsCount = m_listWindows.ProcessList.Count;
            nLoadedWindowsCount = 0;
            m_listSelectWindows.Load(Application.StartupPath + @"\temp\Select.config");
            InitDockPanel();
            dockPanelMain.LoadFromXml(Application.StartupPath + @"\temp\DockManagerDefault.config", new DeserializeDockContent(GetDeserializeDockContent));
            for (int i = 0; i < 20; i++)
            {
                GlobalObjectList.ImageListObject[i].Load(Application.StartupPath + @"\temp\Process" + i.ToString() + ".handle");
            }
            this.Text = "AutomationSystem---" + filePath;
            strCurrentFile = filePath;
        }

        private void toolStripMenuItemSaveProject_Click(object sender, EventArgs e)
        {
            if (strCurrentFile == "")
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "流程文件|*.jw";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    SaveProject(sfd.FileName);
                }
            }
            else
            {
                SaveProject(strCurrentFile);
            }
        }

        private void SaveProject(string savePath)
        {
            try
            {
                //创建缓冲文件夹
                if (!Directory.Exists(Application.StartupPath + @"\temp"))
                {
                    Directory.CreateDirectory(Application.StartupPath + @"\temp");
                }
                for (int i = 0; i < 20; i++)
                {
                    GlobalObjectList.ImageListObject[i].Save(Application.StartupPath + @"\temp\Process" + i.ToString() + ".handle");
                }
                m_listWindows.Save(Application.StartupPath + @"\temp\Window.config");
                m_listSelectWindows.Save(Application.StartupPath + @"\temp\Select.config");
                dockPanelMain.SaveAsXml(Application.StartupPath + @"\temp\DockManagerDefault.config");
                ZipHelper.CreateZip(Application.StartupPath + @"\temp", savePath);
                MessageBox.Show("保存成功");

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void toolStripMenuItemDataInput_Click(object sender, EventArgs e)
        {
            DataInputView dataInputView = new DataInputView();
            dataInputView.Show();
        }

        private void toolStripMenuItemDataOutput_Click(object sender, EventArgs e)
        {
            DataOutputView dataOutputView = new DataOutputView();
            dataOutputView.Show();
        }

        private void toolStripButtonNewProject_Click(object sender, EventArgs e)
        {
            try
            {
                NewProject();
                LoadViewMenu();
                LoadWindowMenu();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void toolStripButtonOpenProject_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Filter = "流程文件|*.jw";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    OpenProject(ofd.FileName);
                    LoadViewMenu();
                    LoadWindowMenu();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void toolStripButtonSaveProject_Click(object sender, EventArgs e)
        {
            if (strCurrentFile == "")
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "流程文件|*.jw";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    SaveProject(sfd.FileName);
                }
            }
            else
            {
                SaveProject(strCurrentFile);
            }
        }

        private void toolStripButtonStart_Click(object sender, EventArgs e)
        {
            Action action = new Action(() => { toolStripButtonStart.Enabled = false; toolStripButtonPause.Enabled = true; toolStripButtonStop.Enabled = true; toolStripStatusLabel1.Text = "流程运行中"; });
            this.Invoke(action);
            GlobalObjectList.RunIndexProcess(GlobalObjectList.nSelectIndex);
        }

        private void toolStripMenuItemSaveAs_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "流程文件|*.jw";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                SaveProject(sfd.FileName);
                strCurrentFile = sfd.FileName;
                this.Text = "AutomationSystem---" + strCurrentFile;
            }
        }

        private void toolStripButtonPause_Click(object sender, EventArgs e)
        {
            Action action = new Action(() => { toolStripButtonStart.Enabled = true; toolStripButtonPause.Enabled = false; toolStripButtonStop.Enabled = true; toolStripStatusLabel1.Text = "流程暂停中"; });
            this.Invoke(action);
            GlobalObjectList.PauseImageProcess();
        }

        private void toolStripButtonStop_Click(object sender, EventArgs e)
        {
            GlobalObjectList.StopImageProcess();
        }

        private void toolStripMenuItemSetWindowName_Click(object sender, EventArgs e)
        {
            SetWindowName setWindowName = new SetWindowName(m_listWindows.ProcessList);
            if (setWindowName.ShowDialog() == DialogResult.OK)
            {
                m_listWindows.CreateNewProcess();
                for (int i = 0; i < setWindowName.setNames.Count; i++)
                {
                    m_listWindows.AddProcess(setWindowName.setNames[i]);
                    dockPanelMain.Contents[nFixedFormCount + i].DockHandler.TabText = setWindowName.setNames[i];
                }
                toolStripDropDownButtonWindow.DropDownItems.Clear();
                for (int i = nFixedFormCount; i < dockPanelMain.Contents.Count; i++)
                {
                    ToolStripMenuItem menuItem = new ToolStripMenuItem(dockPanelMain.Contents[i].DockHandler.TabText);
                    menuItem.Checked = !dockPanelMain.Contents[i].DockHandler.IsHidden;
                    dockPanelMain.Contents[i].DockHandler.CloseButtonVisible = false;
                    dockPanelMain.Contents[i].DockHandler.CloseButton = false;
                    dockPanelMain.Contents[i].DockHandler.HideOnClose = true;
                    ((ToolStripDropDownButton)toolStripTextTool.Items["toolStripDropDownButtonWindow"]).DropDownItems.AddRange(new ToolStripItem[] { menuItem });
                    //为刚刚增加的菜单项注册一个单击事件
                    menuItem.Click += toolWindow_Click;
                }
            }
        }

        private void toolStripMenuItemCodeEdit_Click(object sender, EventArgs e)
        {
        }

        private void toolStripButtonRunCode_Click(object sender, EventArgs e)
        {
        }

        private void toolStripButtonPauseCode_Click(object sender, EventArgs e)
        {
        }

        private void toolStripButtonStopCode_Click(object sender, EventArgs e)
        {
        }

        private void OnStatusChange()
        {
        }

        private void OnFinish(HImage hImage, List<ShowObject> showObjects, List<ShowText> showTexts, string message, int index)
        {
            Action action = new Action(() =>
            {
                toolStripButtonStart.Enabled = true;
                toolStripButtonPause.Enabled = false;
                toolStripButtonStop.Enabled = false;
                toolStripStatusLabel1.Text = "空闲中";
                ((WindowForm)dockPanelMain.Contents[nFixedFormCount + m_listSelectWindows.ProcessList[index]].DockHandler.Content).SetShowObjects(showObjects);
                ((WindowForm)dockPanelMain.Contents[nFixedFormCount + m_listSelectWindows.ProcessList[index]].DockHandler.Content).SetTexts(showTexts);
                ((WindowForm)dockPanelMain.Contents[nFixedFormCount + m_listSelectWindows.ProcessList[index]].DockHandler.Content).SetImage(hImage);
                MessageForm.Instance.SetMessage(message);
            });
            this.Invoke(action);
        }

        private void toolStripMenuItemSelectWindow_Click(object sender, EventArgs e)
        {
            SelectWindow selectWindow = new SelectWindow(m_listWindows.ProcessList, m_listSelectWindows.ProcessList);
            if (selectWindow.ShowDialog() == DialogResult.OK)
            {
                m_listSelectWindows.CreateNewProcess();
                for (int i = 0; i < selectWindow.SelectIndex.Count; i++)
                {
                    m_listSelectWindows.AddProcess(selectWindow.SelectIndex[i]);
                }
            }
        }

        private void toolStripMenuItemSerialPort_Click(object sender, EventArgs e)
        {
        }

        private void toolStripMenuItemServer_Click(object sender, EventArgs e)
        {
        }

        private void toolStripMenuItemClient_Click(object sender, EventArgs e)
        {
        }
    }
}
