using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;
using HalconDotNet;
using Halcon.Functions;

namespace UIControl.HalconVision
{
    public partial class HShowWindow : UserControl
    {
        public HShowWindow()
        {
            InitializeComponent();
        }

        private void toolStripButtonOpen_Click(object sender, EventArgs e)
        {
            Type type = hObjectViewer1.GetType();
            MethodInfo methodInfo = type.GetMethod("toolStripMenuItemOpen_Click", BindingFlags.Instance | BindingFlags.NonPublic);
            if (methodInfo != null)
            {
                object[] parameters = new object[] { null, new EventArgs() };
                methodInfo.Invoke(hObjectViewer1, parameters);
            }
        }

        private void toolStripButtonSaveImage_Click(object sender, EventArgs e)
        {
            Type type = hObjectViewer1.GetType();
            MethodInfo methodInfo = type.GetMethod("toolStripMenuItemSave_Click", BindingFlags.Instance | BindingFlags.NonPublic);
            if (methodInfo != null)
            {
                object[] parameters = new object[] { null, new EventArgs() };
                methodInfo.Invoke(hObjectViewer1, parameters);
            }
        }

        private void toolStripButtonSaveWindow_Click(object sender, EventArgs e)
        {
            Type type = hObjectViewer1.GetType();
            MethodInfo methodInfo = type.GetMethod("toolStripMenuItemSaveWindow_Click", BindingFlags.Instance | BindingFlags.NonPublic);
            if (methodInfo != null)
            {
                object[] parameters = new object[] { null, new EventArgs() };
                methodInfo.Invoke(hObjectViewer1, parameters);
            }
        }

        private void toolStripButtonZoom_Click(object sender, EventArgs e)
        {
            Type type = hObjectViewer1.GetType();
            MethodInfo methodInfo = type.GetMethod("toolStripMenuItemZoom_Click", BindingFlags.Instance | BindingFlags.NonPublic);
            if (methodInfo != null)
            {
                object[] parameters = new object[] { null, new EventArgs() };
                methodInfo.Invoke(hObjectViewer1, parameters);
            }
        }

        private void checkBoxShowCross_CheckedChanged(object sender, EventArgs e)
        {
            hObjectViewer1.IsShowCross = checkBoxShowCross.Checked;
        }

        private void checkBoxSetCross_CheckedChanged(object sender, EventArgs e)
        {
            hObjectViewer1.IsSetCross = checkBoxSetCross.Checked;
        }

        public void SetImage(HImage hImage)
        {
            hObjectViewer1.SetImage(hImage);
        }

        public void SetShowObjects(List<ShowObject> objects)
        {
            hObjectViewer1.SetShowObjects(objects);
        }

        public void SetTexts(List<ShowText> objects)
        {
            hObjectViewer1.SetTexts(objects);
        }

        public void ReleaseRam()
        {
            hObjectViewer1.ReleaseRam();
            this.Dispose();
            GC.Collect();
        }
    }
}
