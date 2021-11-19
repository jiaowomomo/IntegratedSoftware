using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CommonLibrary.Manager;
using Halcon.Functions;

namespace UIControl.HalconVision
{
    public partial class ProcessView : UserControl
    {
        public ProcessView()
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
        }

        private void UpdateListView(object sender, EventArgs args)
        {
            listView1.Items.Clear();
            ProcessManagerResult<ProcessManager<IImageHalconObject>> managerResult = GlobalProcessManager.GetProcessManager<IImageHalconObject>(GlobalImageProcessControl.ImageKeyName, GlobalImageProcessControl.SelectedImageIndex);
            if (managerResult.OK)
            {
                ProcessManager<IImageHalconObject> processManager = managerResult.GetProcessManager;
                for (int i = 0; i < processManager.ProcessCount; i++)
                {
                    listView1.Items.Add((i + 1).ToString() + "." + processManager.GetProcessByIndex(i).ToolName());
                }
            }
        }

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (listView1.SelectedItems.Count != 0)
            {
                int index = listView1.SelectedItems[0].Index;
                ProcessManagerResult<ProcessManager<IImageHalconObject>> managerResult = GlobalProcessManager.GetProcessManager<IImageHalconObject>(GlobalImageProcessControl.ImageKeyName, GlobalImageProcessControl.SelectedImageIndex);
                if (managerResult.OK)
                {
                    ProcessManager<IImageHalconObject> processManager = managerResult.GetProcessManager;
                    processManager.GetProcessByIndex(index).EditParameters();
                }
            }
        }

        private void toolStripMenuItemSetInput_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count != 0)
            {
                SetInputForm setInputForm = new SetInputForm(listView1.SelectedItems[0].Index);
                setInputForm.ShowDialog();
            }
        }

        private void toolStripMenuItemDelete_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count != 0)
            {
                int index = listView1.SelectedItems[0].Index;
                ProcessManagerResult<ProcessManager<IImageHalconObject>> managerResult = GlobalProcessManager.GetProcessManager<IImageHalconObject>(GlobalImageProcessControl.ImageKeyName, GlobalImageProcessControl.SelectedImageIndex);
                if (managerResult.OK)
                {
                    ProcessManager<IImageHalconObject> processManager = managerResult.GetProcessManager;
                    processManager.DeleteProcess(index);
                }
            }
        }

        private void toolStripMenuItemPrevious_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count != 0)
            {
                int index = listView1.SelectedItems[0].Index;
                ProcessManagerResult<ProcessManager<IImageHalconObject>> managerResult = GlobalProcessManager.GetProcessManager<IImageHalconObject>(GlobalImageProcessControl.ImageKeyName, GlobalImageProcessControl.SelectedImageIndex);
                if (managerResult.OK)
                {
                    ProcessManager<IImageHalconObject> processManager = managerResult.GetProcessManager;
                    processManager.MoveToPrevious(index);
                }
            }
        }

        private void toolStripMenuItemNext_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count != 0)
            {
                int index = listView1.SelectedItems[0].Index;
                ProcessManagerResult<ProcessManager<IImageHalconObject>> managerResult = GlobalProcessManager.GetProcessManager<IImageHalconObject>(GlobalImageProcessControl.ImageKeyName, GlobalImageProcessControl.SelectedImageIndex);
                if (managerResult.OK)
                {
                    ProcessManager<IImageHalconObject> processManager = managerResult.GetProcessManager;
                    processManager.MoveToNext(index);
                }
            }
        }

        private void toolStripMenuItemEdit_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count != 0)
            {
                int index = listView1.SelectedItems[0].Index;
                ProcessManagerResult<ProcessManager<IImageHalconObject>> managerResult = GlobalProcessManager.GetProcessManager<IImageHalconObject>(GlobalImageProcessControl.ImageKeyName, GlobalImageProcessControl.SelectedImageIndex);
                if (managerResult.OK)
                {
                    ProcessManager<IImageHalconObject> processManager = managerResult.GetProcessManager;
                    processManager.GetProcessByIndex(index).EditParameters();
                }
            }
        }

        private void toolStripMenuItemClear_Click(object sender, EventArgs e)
        {
            GlobalProcessManager.CreateNewProcessManager<IImageHalconObject>(GlobalImageProcessControl.ImageKeyName, GlobalImageProcessControl.SelectedImageIndex);
        }

        private void toolStripMenuItemRunToCurrent_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count != 0)
            {
                int index = listView1.SelectedItems[0].Index;
                GlobalImageProcessControl.RunImageProcess(0, index + 1, GlobalImageProcessControl.SelectedImageIndex);
            }
        }

        private void toolStripMenuItemRunCurrent_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count != 0)
            {
                int index = listView1.SelectedItems[0].Index;
                GlobalImageProcessControl.RunImageProcess(index, index + 1, GlobalImageProcessControl.SelectedImageIndex);
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            GlobalImageProcessControl.SelectedImageIndex = comboBox1.SelectedIndex;
            UpdateListView(null, new EventArgs());
        }
    }
}
