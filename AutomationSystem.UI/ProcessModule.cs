using System;
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
    public partial class ProcessModule : UserControl
    {
        private string _name = "工具";

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                lbName.Text = _name;
            }
        }

        public ProcessModule()
        {
            InitializeComponent();
        }

        public delegate void ModuleClickEventHandler(ProcessModule processModule);
        public ModuleClickEventHandler ModuleChange = null;

        private void ProcessModule_MouseDown(object sender, MouseEventArgs e)
        {
            if (ModuleChange != null)
            {
                ModuleChange(this);
            }
        }

        public void SetStatusImage(RunStatus runStatus)
        {
            switch (runStatus)
            {
                case RunStatus.Wait:
                    pictureBox2.Image = Properties.Resources.Wait;
                    break;
                case RunStatus.OK:
                    pictureBox2.Image = Properties.Resources.OK;
                    break;
                case RunStatus.NG:
                    pictureBox2.Image = Properties.Resources.NG;
                    break;
                default:
                    pictureBox2.Image = Properties.Resources.Wait;
                    break;
            }
        }
    }
}
