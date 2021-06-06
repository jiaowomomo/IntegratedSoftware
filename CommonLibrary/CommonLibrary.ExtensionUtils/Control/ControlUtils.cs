using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CommonLibrary.ExtensionUtils
{
    public static class ControlUtils
    {
        private static Dictionary<string, Task> m_flashTasks = new Dictionary<string, Task>();

        /// <summary>
        /// 在拥有此控件的基础窗口句柄的线程上执行指定的委托
        /// </summary>
        /// <param name="control"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static object Invoke(this Control control, Action action)
        {
            if (control.IsDisposed || (!control.IsHandleCreated && !control.FindForm().IsHandleCreated))
            {
                return null;
            }

            return control.Invoke((Delegate)action);
        }

        /// <summary>
        /// 获取指定类型子控件
        /// </summary>
        /// <param name="control"></param>
        /// <param name="controlType"></param>
        /// <param name="lstControl"></param>
        /// <returns></returns>
        public static List<Control> GetChildControls(this Control control, Type controlType, List<Control> lstControl = null)
        {
            if (control == null)
            {
                return null;
            }
            if (lstControl.IsNull())
            {
                lstControl = new List<Control>();
            }
            var controls = control.Controls;
            foreach (Control ctrl in controls)
            {
                if (ctrl.GetType() == controlType)
                {
                    lstControl.Add(ctrl);
                }
                lstControl = GetChildControls(ctrl, controlType, lstControl);
            }
            return lstControl;
        }

        /// <summary>
        /// 获取指定类型子控件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="control"></param>
        /// <param name="lstControl"></param>
        /// <returns></returns>
        public static List<T> GetChildControls<T>(this Control control, List<T> lstControl = null)
        {
            if (control == null)
            {
                return null;
            }
            if (lstControl.IsNull())
            {
                lstControl = new List<T>();
            }
            var controls = control.Controls;
            foreach (var ctrl in controls)
            {
                if (ctrl is T)
                {

                    lstControl.Add((T)ctrl);
                }
                lstControl = GetChildControls<T>((Control)ctrl, lstControl);
            }
            return lstControl;
        }

        /// <summary>
        /// 获取指定类型子控件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="control"></param>
        /// <returns></returns>
        public static List<T> GetChildControls<T>(this Form control)
        {
            var lst = new List<T>();
            var fieldInfo = control.GetType().GetFields(System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            for (int i = 0; i < fieldInfo.Length; i++)
            {
                //获取特定控件类型
                if (fieldInfo[i].FieldType.GetInterfaces().Contains(typeof(T)))
                {
                    lst.Add((T)fieldInfo[i].GetValue(control));
                }
            }
            return lst;
        }

        /// <summary>
        /// 控件闪烁
        /// </summary>
        /// <param name="control"></param>
        /// <param name="color"></param>
        /// <param name="flashMode"></param>
        /// <param name="interval"></param>
        public static void Flash(this Control control, Color color, FlashMode flashMode = FlashMode.BackColor, int interval = 50)
        {
            Color source;
            if (flashMode == FlashMode.ForeColor)
            {
                source = control.ForeColor;
            }
            else
            {
                source = control.BackColor;
            }
            Flash(control, color, source, flashMode, interval);
        }

        /// <summary>
        /// 控件闪烁
        /// </summary>
        /// <param name="control"></param>
        /// <param name="flashColor"></param>
        /// <param name="srcColor"></param>
        /// <param name="flashMode"></param>
        /// <param name="interval"></param>
        public static void Flash(this Control control, Color flashColor, Color srcColor, FlashMode flashMode = FlashMode.BackColor, int interval = 50)
        {
            if (m_flashTasks.ContainsKey(control.Name) && !m_flashTasks[control.Name].IsCompleted)
            {
                return;
            }
            if (flashMode == FlashMode.ForeColor)
            {
                control.ForeColor = flashColor;
            }
            else
            {
                control.BackColor = flashColor;
            }

            m_flashTasks[control.Name] = new Task(() =>
                {
                    var t = Task.Delay(interval);
                    t.Wait();
                    control.Invoke(() =>
                    {
                        if (flashMode == FlashMode.ForeColor)
                        {
                            control.ForeColor = srcColor;
                        }
                        else
                        {
                            control.BackColor = srcColor;
                        }
                    });
                });
            m_flashTasks[control.Name].Start();
        }
    }

    public enum FlashMode
    {
        ForeColor,
        BackColor,
    }
}
