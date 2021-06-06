using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Halcon.Delay
{
    public partial class DelayForm : Form
    {
        public int nDelay = 0;
        public DialogResult Result { get; set; }

        public DelayForm()
        {
            InitializeComponent();
        }

        public DelayForm(Delay delay)
        {
            InitializeComponent();

            numericUpDown1.Value = Convert.ToDecimal(delay.nDelay);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            nDelay = Convert.ToInt32(numericUpDown1.Value);
            Result = DialogResult.OK;
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Result = DialogResult.Cancel;
            this.Close();
        }
    }
}
