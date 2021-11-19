using CommonLibrary.Manager;
using Halcon.Functions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UIControl.HalconVision
{
    public partial class DataInputView : Form
    {
        private int m_nSelectIndex = 0;

        public DataInputView()
        {
            InitializeComponent();
        }

        private void DataInputView_Load(object sender, EventArgs e)
        {
            comboBox1.Items.Clear();
            for (int i = 0; i < 20; i++)
            {
                comboBox1.Items.Add("流程" + i.ToString());
            }
            comboBox1.SelectedIndex = 0;
        }

        private void listViewObject_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listViewObject.SelectedItems.Count > 0)
            {
                listViewData.Items.Clear();
                int index = listViewObject.SelectedItems[0].Index;
                int num = 1;
                ProcessManagerResult<ProcessManager<IImageHalconObject>> managerResult = GlobalProcessManager.GetProcessManager<IImageHalconObject>(GlobalImageProcessControl.ImageKeyName, m_nSelectIndex);
                if (managerResult.OK)
                {
                    IImageHalconObject imageHalconObject = managerResult.GetProcessManager.GetProcessByIndex(index);
                    //整形
                    List<string> names = imageHalconObject.GetDataManager.GetInputIntNames();
                    for (int i = 0; i < names.Count; i++)
                    {
                        string value = num.ToString() + "." + names[i] + ":INT:";
                        int values = imageHalconObject.GetDataManager.GetInputInt(names[i]);
                        value += values.ToString("0.000");
                        listViewData.Items.Add(value);
                        num++;
                    }
                    //整形数组
                    names = imageHalconObject.GetDataManager.GetInputIntArrayNames();
                    for (int i = 0; i < names.Count; i++)
                    {
                        string value = num.ToString() + "." + names[i] + ":INT[]:";
                        List<int> values = imageHalconObject.GetDataManager.GetInputIntArray(names[i]);
                        for (int j = 0; j < values.Count; j++)
                        {
                            if (j < values.Count - 1)
                            {
                                value += values[j].ToString("0.000") + ",";
                            }
                            else
                            {
                                value += values[j].ToString("0.000");
                            }
                        }
                        listViewData.Items.Add(value);
                        num++;
                    }
                    //双精度
                    names = imageHalconObject.GetDataManager.GetInputDoubleNames();
                    for (int i = 0; i < names.Count; i++)
                    {
                        string value = num.ToString() + "." + names[i] + ":DOUBLE:";
                        double values = imageHalconObject.GetDataManager.GetInputDouble(names[i]);
                        value += values.ToString("0.000");
                        listViewData.Items.Add(value);
                        num++;
                    }
                    //双精度数组
                    names = imageHalconObject.GetDataManager.GetInputDoubleArrayNames();
                    for (int i = 0; i < names.Count; i++)
                    {
                        string value = num.ToString() + "." + names[i] + ":DOUBLE[]:";
                        List<double> values = imageHalconObject.GetDataManager.GetInputDoubleArray(names[i]);
                        for (int j = 0; j < values.Count; j++)
                        {
                            if (j < values.Count - 1)
                            {
                                value += values[j].ToString("0.000") + ",";
                            }
                            else
                            {
                                value += values[j].ToString("0.000");
                            }
                        }
                        listViewData.Items.Add(value);
                        num++;
                    }
                }
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            m_nSelectIndex = comboBox1.SelectedIndex;
            listViewObject.Items.Clear();
            ProcessManagerResult<ProcessManager<IImageHalconObject>> managerResult = GlobalProcessManager.GetProcessManager<IImageHalconObject>(GlobalImageProcessControl.ImageKeyName, m_nSelectIndex);
            if (managerResult.OK)
            {
                ProcessManager<IImageHalconObject> processManager = managerResult.GetProcessManager;
                for (int i = 0; i < processManager.ProcessCount; i++)
                {
                    listViewObject.Items.Add((i + 1).ToString() + "." + processManager.GetProcessByIndex(i).ToolName());
                }
            }
            if (listViewObject.Items.Count != 0)
            {
                listViewObject.Items[0].Selected = true;
            }
        }
    }
}
