using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CommonLibrary.Controls
{
    public partial class TrackBarEditor : UserControl
    {
        private int m_nValue = 0;

        public int Value
        {
            get { return m_nValue; }
            set
            {
                m_nValue = value;
                trackBarValue.Value = m_nValue;
            }
        }


        public TrackBarEditor(int minValue, int maxValue)
        {
            InitializeComponent();
            trackBarValue.Minimum = minValue;
            trackBarValue.Maximum = maxValue;
        }

        private void trackBarValue_ValueChanged(object sender, EventArgs e)
        {
            labelValue.Text = trackBarValue.Value.ToString();
            Value = Convert.ToInt32(labelValue.Text);
        }
    }
}
