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
    public partial class MessageForm : DockContent
    {
        private static MessageForm _instance = null;

        public static MessageForm Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new MessageForm();
                }
                return _instance;
            }
        }

        private MessageForm()
        {
            InitializeComponent();
        }

        public void SetMessage(string context)
        {
            textBox1.Text = context;
        }
    }
}
