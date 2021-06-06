using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Halcon.ManualCalibration
{
    public partial class SetCoordinate : Form
    {
        public double XCoordinate { get; set; }
        public double YCoordinate { get; set; }

        public SetCoordinate()
        {
            InitializeComponent();
            XCoordinate = 0;
            YCoordinate = 0;
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            XCoordinate = Convert.ToDouble(numericUpDown1.Value);
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            YCoordinate = Convert.ToDouble(numericUpDown2.Value);
        }
    }
}
