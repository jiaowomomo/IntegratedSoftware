using AutomationSystem.Halcon;
using AutomationSystem.Manager;
using HalconDotNet;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutomationSystem.GlobalObject
{
    public static class GlobalObjectList
    {
        public static List<ProcessManager<IImageHalconObject>> ImageListObject = new List<ProcessManager<IImageHalconObject>>();//图像处理对象

        public static int nSelectSerial = -1;

        public static int nSelectServer = -1;

        public static int nSelectClient = -1;

        public static int nSelectIndex = 0;

        public static void RunImageProcess(int start, int end, int runIndex)
        {
            if (isPause)
            {
                isPause = false;
            }
            else
            {
                ParameterizedThreadStart parameterized = new ParameterizedThreadStart(RunProcess);
                Thread thread = new Thread(parameterized);
                thread.IsBackground = true;
                int[] index = new int[3] { start, end, runIndex };
                thread.Start(index);
            }
        }

        public static void RunIndexProcess(int index)
        {
            RunImageProcess(0, ImageListObject[index].ProcessList.Count, index);
        }

        public static void PauseImageProcess()
        {
            isPause = true;
        }

        public static void StopImageProcess()
        {
            isStop = true;
        }

        public static bool GetPause()
        {
            return isPause;
        }

        public static bool GetStop()
        {
            return isStop;
        }

        private static bool isPause = false;
        private static bool isStop = false;
        private static void RunProcess(object index)
        {
            HImage curImage = new HImage();
            List<ShowObject> showObjects = new List<ShowObject>();
            List<ShowText> showTexts = new List<ShowText>();
            Stopwatch sw = new Stopwatch();
            int[] indexs = index as int[];
            int start = indexs[0];
            int end = indexs[1];
            int runIndex = indexs[2];
            string message = "";
            for (int i = start; i < end; i++)
            {
                if (SetRunStatus != null)
                {
                    SetRunStatus(i, RunStatus.Wait);
                }
            }
            sw.Start();
            for (int i = start; i < end; i++)
            {
                if (isStop)
                {
                    break;
                }
                if (isPause)
                {
                    while (isPause)
                    {
                        Thread.Sleep(2);
                        if (isStop)
                        {
                            break;
                        }
                    }
                }
                if (i == 0)
                {
                    ImageListObject[runIndex].ProcessList[i].ImageHandle(ref curImage, ref showObjects, ref showTexts);//处理
                    if (SetRunStatus != null)
                    {
                        if (ImageListObject[runIndex].ProcessList[i].IsRunOK)
                        {
                            SetRunStatus(i, RunStatus.OK);
                        }
                        else
                        {
                            SetRunStatus(i, RunStatus.NG);
                            message = "工具" + (i + 1).ToString() + "执行失败，" + ImageListObject[runIndex].ProcessList[i].ErrorMessage + "\r\n";
                            break;
                        }
                    }
                }
                else
                {
                    if (ImageListObject[runIndex].ProcessList[i - 1].IsRunOK)//处理结果
                    {
                        bool isBinding = true;
                        try
                        {
                            for (int j = 0; j < ImageListObject[runIndex].ProcessList[i].GetDataManager.GetDataBindingCount(); j++)//获取绑定数据
                            {
                                if (isStop)
                                {
                                    break;
                                }
                                if (isPause)
                                {
                                    while (isPause)
                                    {
                                        Thread.Sleep(2);
                                        if (isStop)
                                        {
                                            break;
                                        }
                                    }
                                }
                                DataBinding dataBinding = ImageListObject[runIndex].ProcessList[i].GetDataManager.GetDataBinding(j);
                                if (dataBinding != null)
                                {
                                    if (dataBinding.DataType == "INT")
                                    {
                                        ImageListObject[runIndex].ProcessList[i].GetDataManager.SetInputInt(dataBinding.DataName, ImageListObject[runIndex].ProcessList[dataBinding.DataSourceIndex].GetDataManager.GetOutputInt(dataBinding.DataSourceName));
                                    }
                                    else if (dataBinding.DataType == "INT[]")
                                    {
                                        ImageListObject[runIndex].ProcessList[i].GetDataManager.SetInputIntVector(dataBinding.DataName, ImageListObject[runIndex].ProcessList[dataBinding.DataSourceIndex].GetDataManager.GetOutputIntVector(dataBinding.DataSourceName));
                                    }
                                    else if (dataBinding.DataType == "DOUBLE")
                                    {
                                        ImageListObject[runIndex].ProcessList[i].GetDataManager.SetInputDouble(dataBinding.DataName, ImageListObject[runIndex].ProcessList[dataBinding.DataSourceIndex].GetDataManager.GetOutputDouble(dataBinding.DataSourceName));
                                    }
                                    else if (dataBinding.DataType == "DOUBLE[]")
                                    {
                                        ImageListObject[runIndex].ProcessList[i].GetDataManager.SetInputDoubleVector(dataBinding.DataName, ImageListObject[runIndex].ProcessList[dataBinding.DataSourceIndex].GetDataManager.GetOutputDoubleVector(dataBinding.DataSourceName));
                                    }
                                }
                                else
                                {
                                    message = "工具" + (i + 1).ToString() + "执行失败，没有正确配置输入设置，请检查\r\n";
                                    isBinding = false;
                                    break;
                                }
                            }
                            if (isBinding)
                            {
                                ImageListObject[runIndex].ProcessList[i].ImageHandle(ref curImage, ref showObjects, ref showTexts);//处理
                                if (SetRunStatus != null)
                                {
                                    if (ImageListObject[runIndex].ProcessList[i].IsRunOK)
                                    {
                                        SetRunStatus(i, RunStatus.OK);
                                    }
                                    else
                                    {
                                        SetRunStatus(i, RunStatus.NG);
                                        message = "工具" + (i + 1).ToString() + "执行失败，" + ImageListObject[runIndex].ProcessList[i].ErrorMessage + "\r\n";
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                break;
                            }
                        }
                        catch
                        {
                            message = "工具" + (i + 1).ToString() + "执行失败，没有正确配置输入设置，请检查\r\n";
                            isBinding = false;
                            break;
                        }
                    }
                    else
                    {
                        message = "工具" + i.ToString() + "执行失败，" + ImageListObject[runIndex].ProcessList[i - 1].ErrorMessage + "\r\n";
                        break;
                    }
                }
            }
            sw.Stop();
            message += "执行完成，耗时：" + sw.ElapsedMilliseconds.ToString() + "ms";
            OnFinish.Invoke(curImage, showObjects, showTexts, message, runIndex);
            isStop = false;
            isPause = false;
            curImage.Dispose();
        }

        public delegate void Finish(HImage hImage, List<ShowObject> showObjects, List<ShowText> showTexts, string message, int index);
        public static Finish OnFinish;

        public static Action<int, RunStatus> SetRunStatus;
    }
}
