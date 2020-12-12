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
    public partial class ProcessForm : DockContent
    {
        private static ProcessForm _instance = null;

        public static ProcessForm Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ProcessForm();
                }
                return _instance;
            }
        }

        private ProcessForm()
        {
            InitializeComponent();
        }
    }
}
