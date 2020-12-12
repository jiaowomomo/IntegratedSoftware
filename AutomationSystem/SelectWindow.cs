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
    public partial class SelectWindow : Form
    {
        public List<int> SelectIndex = new List<int>();

        public SelectWindow(List<string> names, List<int> select)
        {
            InitializeComponent();
            SelectIndex = select;
            comboBox1.Items.Clear();
            for (int i = 0; i < 20; i++)
            {
                comboBox1.Items.Add("流程" + i.ToString());
            }

            comboBox2.Items.Clear();
            for (int i = 0; i < names.Count; i++)
            {
                comboBox2.Items.Add(names[i]);
            }
            comboBox1.SelectedIndex = 0;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex != -1)
            {
                comboBox2.SelectedIndex = SelectIndex[comboBox1.SelectedIndex];
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox2.SelectedIndex != -1)
            {
                SelectIndex[comboBox1.SelectedIndex] = comboBox2.SelectedIndex;
            }
        }
    }
}
