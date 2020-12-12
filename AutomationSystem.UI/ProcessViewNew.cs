﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AutomationSystem.GlobalObject;

namespace AutomationSystem.UI
{
    public partial class ProcessViewNew : UserControl
    {
        public ProcessViewNew()
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

            GlobalObjectList.SetRunStatus = new Action<int, RunStatus>(this.SetRunStatus);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            GlobalObjectList.nSelectIndex = comboBox1.SelectedIndex;
            UpdateListView(null, new EventArgs());
        }

        private void UpdateListView(object sender, EventArgs args)
        {
            flowLayoutPanel1.SuspendLayout();
            flowLayoutPanel1.Controls.Clear();
            if (GlobalObjectList.ImageListObject.Count != 0)
            {
                for (int i = 0; i < GlobalObjectList.ImageListObject[GlobalObjectList.nSelectIndex].ProcessList.Count; i++)
                {
                    ProcessModule processModule = new ProcessModule();
                    processModule.ModuleChange += this.ModuleChange;
                    processModule.Name = ((i + 1).ToString() + "." + GlobalObjectList.ImageListObject[GlobalObjectList.nSelectIndex].ProcessList[i].ToolName());
                    flowLayoutPanel1.Controls.Add(processModule);
                }
            }
            flowLayoutPanel1.ResumeLayout();
        }

        private int nSelectedModule = -1;

        private void toolStripMenuItemSetInput_Click(object sender, EventArgs e)
        {
            if (nSelectedModule != -1)
            {
                SetInputForm setInputForm = new SetInputForm(nSelectedModule);
                setInputForm.ShowDialog();
                nSelectedModule = -1;
            }
        }

        private void toolStripMenuItemRunToCurrent_Click(object sender, EventArgs e)
        {
            if (nSelectedModule!=-1)
            {
                GlobalObjectList.RunImageProcess(0, nSelectedModule + 1, GlobalObjectList.nSelectIndex);
                nSelectedModule = -1;
            }
        }

        private void toolStripMenuItemRunCurrent_Click(object sender, EventArgs e)
        {
            if (nSelectedModule!=-1)
            {
                GlobalObjectList.RunImageProcess(nSelectedModule, nSelectedModule + 1, GlobalObjectList.nSelectIndex);
                nSelectedModule = -1;
            }
        }

        private void toolStripMenuItemEdit_Click(object sender, EventArgs e)
        {
            if (nSelectedModule != -1)
            {
                GlobalObjectList.ImageListObject[GlobalObjectList.nSelectIndex].ProcessList[nSelectedModule].EditParameters();
                nSelectedModule = -1;
            }
        }

        private void toolStripMenuItemDelete_Click(object sender, EventArgs e)
        {
            if (nSelectedModule != -1)
            {
                GlobalObjectList.ImageListObject[GlobalObjectList.nSelectIndex].DeleteProcess(nSelectedModule);
                nSelectedModule = -1;
            }
        }

        private void toolStripMenuItemPrevious_Click(object sender, EventArgs e)
        {
            if (nSelectedModule != -1)
            {
                GlobalObjectList.ImageListObject[GlobalObjectList.nSelectIndex].MoveToProvious(nSelectedModule);
                nSelectedModule = -1;
            }
        }

        private void toolStripMenuItemNext_Click(object sender, EventArgs e)
        {
            if (nSelectedModule != -1)
            {
                GlobalObjectList.ImageListObject[GlobalObjectList.nSelectIndex].MoveToNext(nSelectedModule);
                nSelectedModule = -1;
            }
        }

        private void toolStripMenuItemClear_Click(object sender, EventArgs e)
        {
            GlobalObjectList.ImageListObject[GlobalObjectList.nSelectIndex].CreateNewProcess();
        }

        private void ModuleChange(ProcessModule processModule)
        {
            for (int i = 0; i < flowLayoutPanel1.Controls.Count; i++)
            {
                flowLayoutPanel1.Controls[i].BackColor = Color.White;
            }
            processModule.BackColor = Color.Cornsilk;
            nSelectedModule = flowLayoutPanel1.Controls.IndexOf(processModule);
        }

        private void SetRunStatus(int index, RunStatus runStatus)
        {
            ((ProcessModule)flowLayoutPanel1.Controls[index]).SetStatusImage(runStatus);
        }
    }
}