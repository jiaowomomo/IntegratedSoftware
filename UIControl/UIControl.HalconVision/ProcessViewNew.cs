using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UIControl.Common;
using CommonLibrary.Manager;
using Halcon.Functions;

namespace UIControl.HalconVision
{
    public partial class ProcessViewNew : UserControl
    {
        private int m_nSelectedModule = -1;

        public ProcessViewNew()
        {
            InitializeComponent();

            comboBox1.Items.Clear();
            for (int i = 0; i < 20; i++)
            {
                comboBox1.Items.Add("流程" + i.ToString());
                ProcessManagerResult<ProcessManager<IImageHalconObject>> managerResult = GlobalProcessManager.GetProcessManager<IImageHalconObject>(GlobalImageProcessControl.ImageKeyName, i);
                if (managerResult.OK)
                {
                    ProcessManager<IImageHalconObject> processManager = managerResult.GetProcessManager;
                    if (processManager.OnProcessChanged == null)
                    {
                        processManager.OnProcessChanged += UpdateListView;
                    }
                }
            }
            comboBox1.SelectedIndex = 0;

            GlobalImageProcessControl.SetRunStatus = new Action<int, RunStatus>(this.SetRunStatus);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            GlobalImageProcessControl.SelectedImageIndex = comboBox1.SelectedIndex;
            UpdateListView(null, new EventArgs());
        }

        private void UpdateListView(object sender, EventArgs args)
        {
            flowLayoutPanel1.SuspendLayout();
            flowLayoutPanel1.Controls.Clear();
            ProcessManagerResult<ProcessManager<IImageHalconObject>> managerResult = GlobalProcessManager.GetProcessManager<IImageHalconObject>(GlobalImageProcessControl.ImageKeyName, GlobalImageProcessControl.SelectedImageIndex);
            if (managerResult.OK)
            {
                ProcessManager<IImageHalconObject> processManager = managerResult.GetProcessManager;
                for (int i = 0; i < processManager.ProcessCount; i++)
                {
                    ProcessModule processModule = new ProcessModule();
                    processModule.Width = flowLayoutPanel1.Width - 5;
                    processModule.ModuleChange += this.ModuleChange;
                    processModule.ModuleName = ((i + 1).ToString() + "." + processManager.GetProcessByIndex(i).ToolName());
                    flowLayoutPanel1.Controls.Add(processModule);
                }
            }
            flowLayoutPanel1.ResumeLayout();
        }

        private int nSelectedModule = -1;

        private void toolStripMenuItemSetInput_Click(object sender, EventArgs e)
        {
            if (m_nSelectedModule != -1)
            {
                SetInputForm setInputForm = new SetInputForm(m_nSelectedModule);
                setInputForm.ShowDialog();
                m_nSelectedModule = -1;
            }
        }

        private void toolStripMenuItemRunToCurrent_Click(object sender, EventArgs e)
        {
            if (m_nSelectedModule != -1)
            {
                GlobalImageProcessControl.RunImageProcess(0, m_nSelectedModule + 1, GlobalImageProcessControl.SelectedImageIndex);
                m_nSelectedModule = -1;
            }
        }

        private void toolStripMenuItemRunCurrent_Click(object sender, EventArgs e)
        {
            if (m_nSelectedModule != -1)
            {
                GlobalImageProcessControl.RunImageProcess(m_nSelectedModule, m_nSelectedModule + 1, GlobalImageProcessControl.SelectedImageIndex);
                m_nSelectedModule = -1;
            }
        }

        private void toolStripMenuItemEdit_Click(object sender, EventArgs e)
        {
            if (m_nSelectedModule != -1)
            {
                ProcessManagerResult<ProcessManager<IImageHalconObject>> managerResult = GlobalProcessManager.GetProcessManager<IImageHalconObject>(GlobalImageProcessControl.ImageKeyName, GlobalImageProcessControl.SelectedImageIndex);
                if (managerResult.OK)
                {
                    ProcessManager<IImageHalconObject> processManager = managerResult.GetProcessManager;
                    processManager.GetProcessByIndex(m_nSelectedModule).EditParameters();
                }
                m_nSelectedModule = -1;
            }
        }

        private void toolStripMenuItemDelete_Click(object sender, EventArgs e)
        {
            if (m_nSelectedModule != -1)
            {
                ProcessManagerResult<ProcessManager<IImageHalconObject>> managerResult = GlobalProcessManager.GetProcessManager<IImageHalconObject>(GlobalImageProcessControl.ImageKeyName, GlobalImageProcessControl.SelectedImageIndex);
                if (managerResult.OK)
                {
                    ProcessManager<IImageHalconObject> processManager = managerResult.GetProcessManager;
                    processManager.DeleteProcess(m_nSelectedModule);
                }
                m_nSelectedModule = -1;
            }
        }

        private void toolStripMenuItemPrevious_Click(object sender, EventArgs e)
        {
            if (m_nSelectedModule != -1)
            {
                ProcessManagerResult<ProcessManager<IImageHalconObject>> managerResult = GlobalProcessManager.GetProcessManager<IImageHalconObject>(GlobalImageProcessControl.ImageKeyName, GlobalImageProcessControl.SelectedImageIndex);
                if (managerResult.OK)
                {
                    ProcessManager<IImageHalconObject> processManager = managerResult.GetProcessManager;
                    processManager.MoveToPrevious(m_nSelectedModule);
                }
                m_nSelectedModule = -1;
            }
        }

        private void toolStripMenuItemNext_Click(object sender, EventArgs e)
        {
            if (m_nSelectedModule != -1)
            {
                ProcessManagerResult<ProcessManager<IImageHalconObject>> managerResult = GlobalProcessManager.GetProcessManager<IImageHalconObject>(GlobalImageProcessControl.ImageKeyName, GlobalImageProcessControl.SelectedImageIndex);
                if (managerResult.OK)
                {
                    ProcessManager<IImageHalconObject> processManager = managerResult.GetProcessManager;
                    processManager.MoveToNext(m_nSelectedModule);
                }
                m_nSelectedModule = -1;
            }
        }

        private void toolStripMenuItemClear_Click(object sender, EventArgs e)
        {
            GlobalProcessManager.CreateNewProcessManager<IImageHalconObject>(GlobalImageProcessControl.ImageKeyName, GlobalImageProcessControl.SelectedImageIndex);
        }

        private void ModuleChange(ProcessModule processModule)
        {
            for (int i = 0; i < flowLayoutPanel1.Controls.Count; i++)
            {
                flowLayoutPanel1.Controls[i].BackColor = Color.White;
            }
            processModule.BackColor = Color.Cornsilk;
            m_nSelectedModule = flowLayoutPanel1.Controls.IndexOf(processModule);
        }

        private void SetRunStatus(int index, RunStatus runStatus)
        {
            ((ProcessModule)flowLayoutPanel1.Controls[index]).SetStatusImage(runStatus);
        }

        private void ProcessViewNew_Resize(object sender, EventArgs e)
        {
            UpdateListView(null, new EventArgs());
        }
    }
}
