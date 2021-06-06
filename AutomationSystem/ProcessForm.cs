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
        private static readonly Lazy<ProcessForm> m_instance = new Lazy<ProcessForm>(() => new ProcessForm());

        public static ProcessForm Instance { get => m_instance.Value; }

        private ProcessForm()
        {
            InitializeComponent();
        }
    }
}
