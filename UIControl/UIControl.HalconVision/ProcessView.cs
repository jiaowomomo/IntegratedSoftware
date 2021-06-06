using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Global.Functions;

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
                if (GlobalObjectList.ImageListObject.Count != 0)
                {
                    if (GlobalObjectList.ImageListObject[i].OnProcessChanged == null)
                    {
                        GlobalObjectList.ImageListObject[i].OnProcessChanged += UpdateListView;
                    }
                }
            }
            comboBox1.SelectedIndex = 0;
        }

        private void UpdateListView(object sender, EventArgs args)
        {
            listView1.Items.Clear();
            if (GlobalObjectList.ImageListObject.Count != 0)
            {
                for (int i = 0; i < GlobalObjectList.ImageListObject[GlobalObjectList.SelectedImageIndex].ProcessCount; i++)
                {
                    listView1.Items.Add((i + 1).ToString() + "." + GlobalObjectList.ImageListObject[GlobalObjectList.SelectedImageIndex].GetProcessByIndex(i).ToolName());
                }
            }
        }

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (listView1.SelectedItems.Count != 0)
            {
                int index = listView1.SelectedItems[0].Index;
                GlobalObjectList.ImageListObject[GlobalObjectList.SelectedImageIndex].GetProcessByIndex(index).EditParameters();
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
                GlobalObjectList.ImageListObject[GlobalObjectList.SelectedImageIndex].DeleteProcess(index);
            }
        }

        private void toolStripMenuItemPrevious_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count != 0)
            {
                int index = listView1.SelectedItems[0].Index;
                GlobalObjectList.ImageListObject[GlobalObjectList.SelectedImageIndex].MoveToProvious(index);
            }
        }

        private void toolStripMenuItemNext_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count != 0)
            {
                int index = listView1.SelectedItems[0].Index;
                GlobalObjectList.ImageListObject[GlobalObjectList.SelectedImageIndex].MoveToNext(index);
            }
        }

        private void toolStripMenuItemEdit_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count != 0)
            {
                int index = listView1.SelectedItems[0].Index;
                GlobalObjectList.ImageListObject[GlobalObjectList.SelectedImageIndex].GetProcessByIndex(index).EditParameters();
            }
        }

        private void toolStripMenuItemClear_Click(object sender, EventArgs e)
        {
            GlobalObjectList.ImageListObject[GlobalObjectList.SelectedImageIndex].CreateNewProcess();
        }

        private void toolStripMenuItemRunToCurrent_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count != 0)
            {
                int index = listView1.SelectedItems[0].Index;
                GlobalObjectList.RunImageProcess(0, index + 1, GlobalObjectList.SelectedImageIndex);
            }
        }

        private void toolStripMenuItemRunCurrent_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count != 0)
            {
                int index = listView1.SelectedItems[0].Index;
                GlobalObjectList.RunImageProcess(index, index + 1, GlobalObjectList.SelectedImageIndex);
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            GlobalObjectList.SelectedImageIndex = comboBox1.SelectedIndex;
            UpdateListView(null, new EventArgs());
        }
    }
}
