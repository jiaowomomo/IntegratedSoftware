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
        private static readonly Lazy<MessageForm> m_instacne = new Lazy<MessageForm>(() => new MessageForm());

        public static MessageForm Instance { get => m_instacne.Value; }

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
