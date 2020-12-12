using AutomationSystem.GlobalObject;
using AutomationSystem.Halcon;
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
    public partial class SetInputForm : Form
    {
        int objectIndex = 0;

        public SetInputForm(int selectIndex)
        {
            InitializeComponent();

            objectIndex = selectIndex;
            SetUI(selectIndex);
        }

        private void SetUI(int selectIndex)
        {
            SetInputList(selectIndex);

            SetOutputList(selectIndex);

            //SetParameterList(selectIndex);
        }

        private void SetOutputList(int selectIndex)
        {
            listViewOutput.Items.Clear();
            int num = 1;
            for (int i = 0; i < selectIndex; i++)
            {
                string value = (i + 1).ToString() + "." + GlobalObjectList.ImageListObject[GlobalObjectList.nSelectIndex].ProcessList[i].ToolName();
                listViewOutput.Items.Add(value);
                num++;
            }
        }

        private void SetInputList(int selectIndex)
        {
            listViewInput.Items.Clear();
            int num = 1;
            //整形
            List<string> names = GlobalObjectList.ImageListObject[GlobalObjectList.nSelectIndex].ProcessList[selectIndex].GetDataManager.GetInputIntNames();
            for (int i = 0; i < names.Count; i++)
            {
                string value = num.ToString() + "." + names[i] + ":INT";
                listViewInput.Items.Add(value);
                num++;
            }
            //整形数组
            names = GlobalObjectList.ImageListObject[GlobalObjectList.nSelectIndex].ProcessList[selectIndex].GetDataManager.GetInputIntVectorNames();
            for (int i = 0; i < names.Count; i++)
            {
                string value = num.ToString() + "." + names[i] + ":INT[]";
                listViewInput.Items.Add(value);
                num++;
            }
            //双精度
            names = GlobalObjectList.ImageListObject[GlobalObjectList.nSelectIndex].ProcessList[selectIndex].GetDataManager.GetInputDoubleNames();
            for (int i = 0; i < names.Count; i++)
            {
                string value = num.ToString() + "." + names[i] + ":DOUBLE";
                listViewInput.Items.Add(value);
                num++;
            }
            //双精度数组
            names = GlobalObjectList.ImageListObject[GlobalObjectList.nSelectIndex].ProcessList[selectIndex].GetDataManager.GetInputDoubleVectorNames();
            for (int i = 0; i < names.Count; i++)
            {
                string value = num.ToString() + "." + names[i] + ":DOUBLE[]";
                listViewInput.Items.Add(value);
                num++;
            }
        }

        private void SetParameterList(int selectIndex)
        {
            if (selectIndex != -1)
            {
                listViewParameter.Items.Clear();
                int num = 1;
                List<string> names;
                if (strDataType == "INT")
                {
                    //整形
                    names = GlobalObjectList.ImageListObject[GlobalObjectList.nSelectIndex].ProcessList[selectIndex].GetDataManager.GetOutputIntNames();
                    for (int i = 0; i < names.Count; i++)
                    {
                        string value = num.ToString() + "." + names[i] + ":INT";
                        listViewParameter.Items.Add(value);
                        num++;
                    }
                }
                else if (strDataType == "INT[]")
                {
                    //整形数组
                    names = GlobalObjectList.ImageListObject[GlobalObjectList.nSelectIndex].ProcessList[selectIndex].GetDataManager.GetOutputIntVectorNames();
                    for (int i = 0; i < names.Count; i++)
                    {
                        string value = num.ToString() + "." + names[i] + ":INT[]";
                        listViewParameter.Items.Add(value);
                        num++;
                    }
                }
                else if (strDataType == "DOUBLE")
                {
                    //双精度
                    names = GlobalObjectList.ImageListObject[GlobalObjectList.nSelectIndex].ProcessList[selectIndex].GetDataManager.GetOutputDoubleNames();
                    for (int i = 0; i < names.Count; i++)
                    {
                        string value = num.ToString() + "." + names[i] + ":DOUBLE";
                        listViewParameter.Items.Add(value);
                        num++;
                    }
                }
                else if (strDataType == "DOUBLE[]")
                {
                    //双精度数组
                    names = GlobalObjectList.ImageListObject[GlobalObjectList.nSelectIndex].ProcessList[selectIndex].GetDataManager.GetOutputDoubleVectorNames();
                    for (int i = 0; i < names.Count; i++)
                    {
                        string value = num.ToString() + "." + names[i] + ":DOUBLE[]";
                        listViewParameter.Items.Add(value);
                        num++;
                    }
                }

                if (GlobalObjectList.ImageListObject[GlobalObjectList.nSelectIndex].ProcessList[objectIndex].GetDataManager.GetDataBinding(DataIndex) != null)
                {
                    for (int i = 0; i < num - 1; i++)
                    {
                        string key = (i + 1).ToString() + "." + sourceName;
                        if (strDataType == "INT")
                        {
                            key += ":INT";
                        }
                        else if (strDataType == "INT[]")
                        {
                            key += ":INT[]";
                        }
                        else if (strDataType == "DOUBLE")
                        {
                            key += ":DOUBLE";
                        }
                        else if (strDataType == "DOUBLE[]")
                        {
                            key += ":DOUBLE[]";
                        }
                        if (listViewParameter.FindItemWithText(key) != null)
                        {
                            int index = listViewParameter.FindItemWithText(key).Index;
                            if (index != -1)
                            {
                                listViewParameter.Items[index].Selected = true;
                                break;
                            }
                        }
                    }
                }
            }
        }

        string strDataType = "";
        string strDataName = "";
        int DataIndex = 0;
        private void listViewInput_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listViewInput.SelectedItems.Count != 0)
            {
                string[] splits = listViewInput.SelectedItems[0].Text.Split(':');
                strDataType = splits[1];
                strDataName = splits[0].Substring(2);
                DataIndex = listViewInput.SelectedItems[0].Index;
                try
                {
                    if (GlobalObjectList.ImageListObject[GlobalObjectList.nSelectIndex].ProcessList[objectIndex].GetDataManager.GetDataBinding(DataIndex) != null)
                    {
                        DataBinding dataBinding = GlobalObjectList.ImageListObject[GlobalObjectList.nSelectIndex].ProcessList[objectIndex].GetDataManager.GetDataBinding(DataIndex);
                        sourceIndex = dataBinding.DataSourceIndex;
                        sourceName = dataBinding.DataSourceName;
                        listViewOutput.Items[sourceIndex].Selected = true;
                    }
                    SetParameterList(sourceIndex);
                }
                catch
                { }
            }
        }

        int sourceIndex = -1;
        private void listViewOutput_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listViewOutput.SelectedItems.Count != 0)
            {
                sourceIndex = listViewOutput.SelectedItems[0].Index;
                SetParameterList(sourceIndex);
            }
        }

        string sourceName = "";
        private void listViewParameter_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listViewParameter.SelectedItems.Count != 0)
            {
                string[] splits = listViewParameter.SelectedItems[0].Text.Split(':');
                sourceName = splits[0].Substring(2);
                GlobalObjectList.ImageListObject[GlobalObjectList.nSelectIndex].ProcessList[objectIndex].GetDataManager.SetDataBinding(DataIndex, strDataName, strDataType, sourceIndex, sourceName);
            }
        }
    }
}
