using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutomationSystem
{
    public partial class SetWindowName : Form
    {
        public List<string> SetNames = new List<string>();

        public SetWindowName(List<string> names)
        {
            InitializeComponent();

            comboBox1.Items.Clear();
            for (int i = 0; i < names.Count; i++)
            {
                comboBox1.Items.Add(names[i]);
            }
            SetNames = names;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex != -1)
            {
                SetNames[comboBox1.SelectedIndex] = textBox1.Text;
            }
        }
    }
}
