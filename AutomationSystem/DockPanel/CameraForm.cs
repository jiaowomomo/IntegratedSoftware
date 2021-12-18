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
    public partial class CameraForm : DockContent
    {
        private static readonly Lazy<CameraForm> m_instance = new Lazy<CameraForm>(() => new CameraForm());

        public static CameraForm Instance { get => m_instance.Value; }

        private CameraForm()
        {
            InitializeComponent();
            this.FormClosing += hCameraWindow1.Form_FormClosing;
        }

        private void CameraForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            hCameraWindow1.ReleaseRam();
            this.Dispose();
            GC.Collect();
        }
    }
}
