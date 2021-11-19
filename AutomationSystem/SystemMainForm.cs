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
using CommonLibrary.DataHelper;
using CommonLibrary.ExtensionUtils;
using CommonLibrary.Manager;
using Halcon.Functions;
using HalconDotNet;
using UIControl.HalconVision;
using WeifenLuo.WinFormsUI.Docking;

namespace AutomationSystem
{
    public partial class SystemMainForm : Form
    {
        private const int FIXED_FORM_COUNT = 3;
        private const int SELECT_WINDOW_COUNT = 20;

        private static readonly string m_strDefaultConfigFile = Path.Combine(Application.StartupPath, "Default.jw");
        private static readonly string m_strHistoryFilePath = Path.Combine(Application.StartupPath, "History", "FilePath.txt");
        private static readonly string m_strSerialConfigPath = Path.Combine(Application.StartupPath, "CtrlCard", "Serial.cfg");
        private static readonly string m_strServerConfigPath = Path.Combine(Application.StartupPath, "CtrlCard", "Server.cfg");
        private static readonly string m_strClientConfigPath = Path.Combine(Application.StartupPath, "CtrlCard", "Client.cfg");
        private static readonly string m_strCalibrationPluginPath = Path.Combine(Application.StartupPath, "Calibration");
        private static readonly string m_strHistoryCodePath = Path.Combine(Application.StartupPath, "History", "CodePath.txt");
        private static readonly string m_strTempPath = Path.Combine(Application.StartupPath, "Temp");
        private static readonly string m_strTempWindowConfigPath = Path.Combine(m_strTempPath, "Window.config");
        private static readonly string m_strTempSelectConfigPath = Path.Combine(m_strTempPath, "Select.config");
        private static readonly string m_strTempDockDefaultConfigPath = Path.Combine(m_strTempPath, "DockManagerDefault.config");
        private static readonly string m_strTempDrawPath = Path.Combine(m_strTempPath, "Draw.route");
        private static readonly string m_strTempProcessPath = Path.Combine(m_strTempPath, "Process");

        private ProcessManager<string> m_listWindows = new ProcessManager<string>();//新建窗口管理
        private ProcessManager<int> m_listSelectWindows = new ProcessManager<int>();//展示结果窗口索引
        private int m_nWindowsCount = 0;
        private int m_nLoadedWindowsCount = 0;
        private string m_strCurrentFile = string.Empty;

        public SystemMainForm()
        {
            InitializeComponent();

            InitDirectorys();

            GlobalProcessManager.CreateMultiProcessManager<IImageHalconObject>(GlobalImageProcessControl.ImageKeyName, SELECT_WINDOW_COUNT);
            for (int i = 0; i < SELECT_WINDOW_COUNT; i++)
            {
                m_listSelectWindows.AddProcess(0);
            }
            AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(Function.CurrentDomain_AssemblyResolve);
        }

        private void SystemMainForm_Load(object sender, EventArgs e)
        {
            try
            {
                if (File.Exists(m_strHistoryFilePath))
                {
                    using (StreamReader sr = new StreamReader(m_strHistoryFilePath))
                    {
                        m_strCurrentFile = sr.ReadLine();
                    }
                    if (m_strCurrentFile != "")
                    {
                        OpenProject(m_strCurrentFile);
                    }
                    else
                    {
                        NewProject();
                    }
                }
                else if (File.Exists(m_strDefaultConfigFile))
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
            GlobalImageProcessControl.OnFinish += OnFinish;
        }

        private void InitDirectorys()
        {
            List<string> directorys = new List<string>();
            directorys.Add(Path.Combine(Application.StartupPath, "History"));
            directorys.Add(Path.Combine(Application.StartupPath, "CodeSystem"));
            directorys.Add(Path.Combine(Application.StartupPath, "CtrlCard"));
            directorys.Add(Path.Combine(Application.StartupPath, "CameraDLL"));
            directorys.Add(Path.Combine(Application.StartupPath, "ModelImage"));
            directorys.Add(Path.Combine(Application.StartupPath, "AutoCircleCalibration"));
            directorys.Add(Path.Combine(Application.StartupPath, "Calibration"));
            directorys.Add(Path.Combine(Application.StartupPath, "ManualCalibration"));
            directorys.Add(Path.Combine(Application.StartupPath, "MotionCardDLL"));
            directorys.Add(Path.Combine(Application.StartupPath, "Temp"));
            directorys.Add(Path.Combine(Application.StartupPath, "ExternTool"));

            DirectoryUtils.CreateAndReturnFailureDirectorys(directorys);
        }

        private void ResetFormLocation()
        {
            ToolForm.Instance.Show(dockPanelMain, DockState.DockRight);
            MessageForm.Instance.Show(dockPanelMain, DockState.DockBottom);
            ProcessForm.Instance.Show(dockPanelMain, DockState.DockLeft);
            NewWindowForm();
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
            List<IFormMenu> formMenus = Function.LoadFormPlugins<IFormMenu>(m_strCalibrationPluginPath);
            foreach (var menu in formMenus)
            {
                //向菜单栏中动态添加一个菜单项
                ToolStripDropDownButton tsddb = new ToolStripDropDownButton(menu.MainToolStrip);
                tsddb.Name = $"ToolStripDropDownButton{menu.MainToolStrip}";
                tsddb.DisplayStyle = ToolStripItemDisplayStyle.Text;
                tsddb.ShowDropDownArrow = false;
                if (!this.toolStripTextTool.Items.ContainsKey(tsddb.Name))
                {
                    this.toolStripTextTool.Items.AddRange(new ToolStripItem[] { tsddb });
                }
                int index = this.toolStripTextTool.Items.IndexOfKey(tsddb.Name);
                ToolStripItem toolStripItem = new ToolStripMenuItem(menu.SubToolStrip);
                ((ToolStripDropDownButton)this.toolStripTextTool.Items[index]).DropDownItems.AddRange(new ToolStripItem[] { toolStripItem });
                //为刚刚增加的菜单项注册一个单击事件
                toolStripItem.Click += Tool_Clcik;
                toolStripItem.Tag = menu;
            }
        }

        private void LoadWindowMenu()
        {
            toolStripDropDownButtonWindow.DropDownItems.Clear();
            for (int i = FIXED_FORM_COUNT; i < dockPanelMain.Contents.Count; i++)
            {
                ToolStripMenuItem menuItem = new ToolStripMenuItem(dockPanelMain.Contents[i].DockHandler.TabText);
                menuItem.Checked = !dockPanelMain.Contents[i].DockHandler.IsHidden;
                dockPanelMain.Contents[i].DockHandler.CloseButtonVisible = false;
                dockPanelMain.Contents[i].DockHandler.CloseButton = false;
                dockPanelMain.Contents[i].DockHandler.HideOnClose = true;
                ((ToolStripDropDownButton)toolStripTextTool.Items["toolStripDropDownButtonWindow"]).DropDownItems.AddRange(new ToolStripItem[] { menuItem });
                //为刚刚增加的菜单项注册一个单击事件
                menuItem.Click += ToolWindow_Click;
            }
        }

        private void LoadViewMenu()
        {
            toolStripDropDownButtonView.DropDownItems.Clear();
            for (int i = 0; i < dockPanelMain.Contents.Count - m_nWindowsCount; i++)
            {
                ToolStripMenuItem menuItem = new ToolStripMenuItem(dockPanelMain.Contents[i].DockHandler.TabText);
                menuItem.Checked = !dockPanelMain.Contents[i].DockHandler.IsHidden;
                dockPanelMain.Contents[i].DockHandler.CloseButtonVisible = false;
                dockPanelMain.Contents[i].DockHandler.CloseButton = false;
                dockPanelMain.Contents[i].DockHandler.HideOnClose = true;
                ((ToolStripDropDownButton)toolStripTextTool.Items["toolStripDropDownButtonView"]).DropDownItems.AddRange(new ToolStripItem[] { menuItem });
                //为刚刚增加的菜单项注册一个单击事件
                menuItem.Click += ToolView_Click;
            }
        }

        private void Tool_Clcik(object sender, EventArgs e)
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

        private void ToolView_Click(object sender, EventArgs e)
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

        private void ToolWindow_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem tsm = sender as ToolStripMenuItem;
            tsm.Checked = !tsm.Checked;
            int index = toolStripDropDownButtonWindow.DropDownItems.IndexOf(tsm);
            if (index != -1)
            {
                if (tsm.Checked)
                {
                    dockPanelMain.Contents[FIXED_FORM_COUNT + index].DockHandler.Show();
                }
                else
                {
                    dockPanelMain.Contents[FIXED_FORM_COUNT + index].DockHandler.Hide();
                }
            }
        }

        private void SystemMainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            using (StreamWriter sw = new StreamWriter(m_strHistoryFilePath))
            {
                sw.WriteLine(m_strCurrentFile);
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
                windowForm.Text = m_listWindows.GetProcessByIndex(m_nLoadedWindowsCount);
                m_nLoadedWindowsCount++;
                return windowForm;
            }
            if (persistString == typeof(ProcessForm).ToString())
                return ProcessForm.Instance;
            return null;
        }

        private void ToolStripMenuItemNewWindow_Click(object sender, EventArgs e)
        {
            NewWindowForm();
        }

        private void NewWindowForm()
        {
            WindowForm windowForm = new WindowForm();
            m_nWindowsCount++;
            windowForm.Text = $"窗口{m_nWindowsCount}";
            m_listWindows.AddProcess(windowForm.Text);
            windowForm.Show(dockPanelMain, DockState.Document);
            LoadWindowMenu();
        }

        private void ToolStripMenuItemNewProject_Click(object sender, EventArgs e)
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
            if (File.Exists(m_strDefaultConfigFile))
            {
                ZipHelper.UnZip(m_strDefaultConfigFile, m_strTempPath);
                m_listWindows.Load(m_strTempWindowConfigPath);
                m_nWindowsCount = m_listWindows.ProcessCount;
                m_nLoadedWindowsCount = 0;
                m_listSelectWindows.Load(m_strTempSelectConfigPath);
                InitDockPanel();
                dockPanelMain.LoadFromXml(m_strTempDockDefaultConfigPath, new DeserializeDockContent(GetDeserializeDockContent));
                for (int i = 0; i < SELECT_WINDOW_COUNT; i++)
                {
                    GlobalProcessManager.Load<IImageHalconObject>(GlobalImageProcessControl.ImageKeyName, $"{m_strTempProcessPath}{i}.handle", i);
                }
                this.Text = "AutomationSystem";
                m_strCurrentFile = string.Empty;
            }
            else
            {
                m_listWindows.CreateNewProcessManager();
                m_nWindowsCount = 0;
                m_nLoadedWindowsCount = 0;
                InitDockPanel();
                m_listSelectWindows.CreateNewProcessManager();
                for (int i = 0; i < SELECT_WINDOW_COUNT; i++)
                {
                    m_listSelectWindows.AddProcess(0);
                    GlobalProcessManager.CreateNewProcessManager<IImageHalconObject>(GlobalImageProcessControl.ImageKeyName, i);
                }
                this.Text = "AutomationSystem";
                m_strCurrentFile = string.Empty;
                ResetFormLocation();
            }
        }

        private void InitDockPanel()
        {
            for (int i = this.dockPanelMain.Contents.Count; i > FIXED_FORM_COUNT; i--)
            {
                dockPanelMain.Contents[i - 1].DockHandler.Close();
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

        private void ToolStripMenuItemOpenProject_Click(object sender, EventArgs e)
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
            ZipHelper.UnZip(filePath, m_strTempPath);
            m_listWindows.Load(m_strTempWindowConfigPath);
            m_nWindowsCount = m_listWindows.ProcessCount;
            m_nLoadedWindowsCount = 0;
            m_listSelectWindows.Load(m_strTempSelectConfigPath);
            InitDockPanel();
            dockPanelMain.LoadFromXml(m_strTempDockDefaultConfigPath, new DeserializeDockContent(GetDeserializeDockContent));
            for (int i = 0; i < SELECT_WINDOW_COUNT; i++)
            {
                GlobalProcessManager.Load<IImageHalconObject>(GlobalImageProcessControl.ImageKeyName, $"{m_strTempProcessPath}{i}.handle", i);
            }
            ProcessManagerResult<ProcessManager<IImageHalconObject>> managerResult = GlobalProcessManager.GetProcessManager<IImageHalconObject>(GlobalImageProcessControl.ImageKeyName, 0);
            if (managerResult.OK)
            {
                managerResult.GetProcessManager.OnProcessChanged(null, null);
            }
            this.Text = "AutomationSystem---" + filePath;
            m_strCurrentFile = filePath;
        }

        private void ToolStripMenuItemSaveProject_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(m_strCurrentFile))
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
                SaveProject(m_strCurrentFile);
            }
        }

        private void SaveProject(string savePath)
        {
            try
            {
                for (int i = 0; i < SELECT_WINDOW_COUNT; i++)
                {
                    GlobalProcessManager.Save<IImageHalconObject>(GlobalImageProcessControl.ImageKeyName, $"{m_strTempProcessPath}{i}.handle", i);
                }
                m_listWindows.Save(m_strTempWindowConfigPath);
                m_listSelectWindows.Save(m_strTempSelectConfigPath);
                dockPanelMain.SaveAsXml(m_strTempDockDefaultConfigPath);
                ZipHelper.CreateZip(m_strTempPath, savePath);
                MessageBox.Show("保存成功");

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ToolStripMenuItemDataInput_Click(object sender, EventArgs e)
        {
            DataInputView dataInputView = new DataInputView();
            dataInputView.Show();
        }

        private void ToolStripMenuItemDataOutput_Click(object sender, EventArgs e)
        {
            DataOutputView dataOutputView = new DataOutputView();
            dataOutputView.Show();
        }

        private void ToolStripButtonNewProject_Click(object sender, EventArgs e)
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

        private void ToolStripButtonOpenProject_Click(object sender, EventArgs e)
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

        private void ToolStripButtonSaveProject_Click(object sender, EventArgs e)
        {
            if (m_strCurrentFile == "")
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
                SaveProject(m_strCurrentFile);
            }
        }

        private void ToolStripButtonStart_Click(object sender, EventArgs e)
        {
            Action action = new Action(() => { toolStripButtonStart.Enabled = false; toolStripButtonPause.Enabled = true; toolStripButtonStop.Enabled = true; toolStripStatusLabel1.Text = "流程运行中"; });
            this.Invoke(action);
            GlobalImageProcessControl.RunIndexProcess(GlobalImageProcessControl.SelectedImageIndex);
        }

        private void ToolStripMenuItemSaveAs_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "流程文件|*.jw";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                SaveProject(sfd.FileName);
                m_strCurrentFile = sfd.FileName;
                this.Text = "AutomationSystem---" + m_strCurrentFile;
            }
        }

        private void ToolStripButtonPause_Click(object sender, EventArgs e)
        {
            Action action = new Action(() => { toolStripButtonStart.Enabled = true; toolStripButtonPause.Enabled = false; toolStripButtonStop.Enabled = true; toolStripStatusLabel1.Text = "流程暂停中"; });
            this.Invoke(action);
            GlobalImageProcessControl.PauseImageProcess();
        }

        private void ToolStripButtonStop_Click(object sender, EventArgs e)
        {
            GlobalImageProcessControl.StopImageProcess();
        }

        private void ToolStripMenuItemSetWindowName_Click(object sender, EventArgs e)
        {
            List<string> names = new List<string>();
            for (int i = 0; i < m_listWindows.ProcessCount; i++)
            {
                names.Add(m_listWindows.GetProcessByIndex(i));
            }
            SetWindowName setWindowName = new SetWindowName(names);
            if (setWindowName.ShowDialog() == DialogResult.OK)
            {
                m_listWindows.CreateNewProcessManager();
                for (int i = 0; i < setWindowName.SetNames.Count; i++)
                {
                    m_listWindows.AddProcess(setWindowName.SetNames[i]);
                    dockPanelMain.Contents[FIXED_FORM_COUNT + i].DockHandler.TabText = setWindowName.SetNames[i];
                }
                toolStripDropDownButtonWindow.DropDownItems.Clear();
                for (int i = FIXED_FORM_COUNT; i < dockPanelMain.Contents.Count; i++)
                {
                    ToolStripMenuItem menuItem = new ToolStripMenuItem(dockPanelMain.Contents[i].DockHandler.TabText);
                    menuItem.Checked = !dockPanelMain.Contents[i].DockHandler.IsHidden;
                    dockPanelMain.Contents[i].DockHandler.CloseButtonVisible = false;
                    dockPanelMain.Contents[i].DockHandler.CloseButton = false;
                    dockPanelMain.Contents[i].DockHandler.HideOnClose = true;
                    ((ToolStripDropDownButton)toolStripTextTool.Items["toolStripDropDownButtonWindow"]).DropDownItems.AddRange(new ToolStripItem[] { menuItem });
                    //为刚刚增加的菜单项注册一个单击事件
                    menuItem.Click += ToolWindow_Click;
                }
            }
        }

        private void ToolStripMenuItemCodeEdit_Click(object sender, EventArgs e)
        {
        }

        private void ToolStripButtonRunCode_Click(object sender, EventArgs e)
        {

        }

        private void ToolStripButtonPauseCode_Click(object sender, EventArgs e)
        {
      
        }

        private void ToolStripButtonStopCode_Click(object sender, EventArgs e)
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
                int selectedWindowsIndex = m_listSelectWindows.GetProcessByIndex(index);
                if (selectedWindowsIndex == -1)
                    return;
                WindowForm windowForm = ((WindowForm)dockPanelMain.Contents[FIXED_FORM_COUNT + selectedWindowsIndex].DockHandler.Content);
                if (windowForm != null)
                {
                    windowForm.SetShowObjects(showObjects);
                    windowForm.SetTexts(showTexts);
                    windowForm.SetImage(hImage);
                }
                MessageForm.Instance.SetMessage(message);
            });
            this.Invoke(action);
        }

        private void ToolStripMenuItemSelectWindow_Click(object sender, EventArgs e)
        {
            List<string> names = new List<string>();
            List<int> selectWindows = new List<int>();
            for (int i = 0; i < m_listWindows.ProcessCount; i++)
            {
                names.Add(m_listWindows.GetProcessByIndex(i));
            }
            for (int i = 0; i < m_listSelectWindows.ProcessCount; i++)
            {
                selectWindows.Add(m_listSelectWindows.GetProcessByIndex(i));
            }
            SelectWindow selectWindow = new SelectWindow(names, selectWindows);
            if (selectWindow.ShowDialog() == DialogResult.OK)
            {
                m_listSelectWindows.CreateNewProcessManager();
                for (int i = 0; i < selectWindow.SelectIndex.Count; i++)
                {
                    m_listSelectWindows.AddProcess(selectWindow.SelectIndex[i]);
                }
            }
        }

        private void ToolStripMenuItemSerialPort_Click(object sender, EventArgs e)
        {

        }

        private void ToolStripMenuItemServer_Click(object sender, EventArgs e)
        {

        }

        private void ToolStripMenuItemClient_Click(object sender, EventArgs e)
        {

        }
    }
}
