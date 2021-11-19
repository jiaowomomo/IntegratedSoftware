using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CommonLibrary.Manager;

namespace UIControl.Common
{
    public partial class ProcessModule : UserControl
    {
        private string m_strModuleName = "工具";

        public string ModuleName
        {
            get { return m_strModuleName; }
            set
            {
                m_strModuleName = value;
                lbName.Text = m_strModuleName;
            }
        }

        public delegate void ModuleClickEventHandler(ProcessModule processModule);
        public ModuleClickEventHandler ModuleChange = null;

        public ProcessModule()
        {
            InitializeComponent();
        }

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
                case RunStatus.Exception:
                    pictureBox2.Image = Properties.Resources.Exception;
                    break;
                default:
                    pictureBox2.Image = Properties.Resources.Wait;
                    break;
            }
        }
    }
}
