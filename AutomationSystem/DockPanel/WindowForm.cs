using Halcon.Functions;
using HalconDotNet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace AutomationSystem
{
    public partial class WindowForm : DockContent
    {
        public WindowForm()
        {
            InitializeComponent();
        }

        public void SetImage(HImage hImage)
        {
            hShowWindow1.SetImage(hImage);
        }

        public void SetShowObjects(List<ShowObject> objects)
        {
            hShowWindow1.SetShowObjects(objects);
        }

        public void SetTexts(List<ShowText> objects)
        {
            hShowWindow1.SetTexts(objects);
        }

        private void WindowForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            hShowWindow1.ReleaseRam();
            this.Dispose();
            GC.Collect();
        }
    }
}
