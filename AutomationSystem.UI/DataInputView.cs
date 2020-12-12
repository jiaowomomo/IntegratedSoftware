using AutomationSystem.GlobalObject;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutomationSystem.UI
{
    public partial class DataInputView : Form
    {
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
                //整形
                List<string> names = GlobalObjectList.ImageListObject[nSelectIndex].ProcessList[index].GetDataManager.GetInputIntNames();
                for (int i = 0; i < names.Count; i++)
                {
                    string value = num.ToString() + "." + names[i] + ":INT:";
                    int values = GlobalObjectList.ImageListObject[nSelectIndex].ProcessList[index].GetDataManager.GetInputInt(names[i]);
                    value += values.ToString();
                    listViewData.Items.Add(value);
                    num++;
                }
                //整形数组
                names = GlobalObjectList.ImageListObject[nSelectIndex].ProcessList[index].GetDataManager.GetInputIntVectorNames();
                for (int i = 0; i < names.Count; i++)
                {
                    string value = num.ToString() + "." + names[i] + ":INT[]:";
                    List<int> values = GlobalObjectList.ImageListObject[nSelectIndex].ProcessList[index].GetDataManager.GetInputIntVector(names[i]);
                    for (int j = 0; j < values.Count; j++)
                    {
                        if (j < values.Count - 1)
                        {
                            value += values[j].ToString() + ",";
                        }
                        else
                        {
                            value += values[j].ToString();
                        }
                    }
                    listViewData.Items.Add(value);
                    num++;
                }
                //双精度
                names = GlobalObjectList.ImageListObject[nSelectIndex].ProcessList[index].GetDataManager.GetInputDoubleNames();
                for (int i = 0; i < names.Count; i++)
                {
                    string value = num.ToString() + "." + names[i] + ":DOUBLE:";
                    double values = GlobalObjectList.ImageListObject[nSelectIndex].ProcessList[index].GetDataManager.GetInputDouble(names[i]);
                    value += values.ToString();
                    listViewData.Items.Add(value);
                    num++;
                }
                //双精度数组
                names = GlobalObjectList.ImageListObject[nSelectIndex].ProcessList[index].GetDataManager.GetInputDoubleVectorNames();
                for (int i = 0; i < names.Count; i++)
                {
                    string value = num.ToString() + "." + names[i] + ":DOUBLE[]:";
                    List<double> values = GlobalObjectList.ImageListObject[nSelectIndex].ProcessList[index].GetDataManager.GetInputDoubleVector(names[i]);
                    for (int j = 0; j < values.Count; j++)
                    {
                        if (j < values.Count - 1)
                        {
                            value += values[j].ToString() + ",";
                        }
                        else
                        {
                            value += values[j].ToString();
                        }
                    }
                    listViewData.Items.Add(value);
                    num++;
                }
            }
        }

        int nSelectIndex = 0;
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            nSelectIndex = comboBox1.SelectedIndex;
            listViewObject.Items.Clear();
            for (int i = 0; i < GlobalObjectList.ImageListObject[nSelectIndex].ProcessList.Count; i++)
            {
                listViewObject.Items.Add((i + 1).ToString() + "." + GlobalObjectList.ImageListObject[nSelectIndex].ProcessList[i].ToolName());
            }
            if (listViewObject.Items.Count != 0)
            {
                listViewObject.Items[0].Selected = true;
            }
        }
    }
}
