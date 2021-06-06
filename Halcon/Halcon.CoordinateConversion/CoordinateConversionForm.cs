using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Halcon.CoordinateConversion
{
    public partial class CoordinateConversionForm : Form
    {
        public string strType = "";
        public int nIndex = 0;
        public DialogResult Result { get; set; }

        public CoordinateConversionForm()
        {
            InitializeComponent();
            SetUI();
            comboBoxType.SelectedIndex = 0;
            comboBoxIndex.SelectedIndex = 0;
        }

        public CoordinateConversionForm(CoordinateConversion coordinateConversion)
        {
            InitializeComponent();
            SetUI();
            comboBoxType.SelectedIndex = comboBoxType.FindString(coordinateConversion.strType);
            comboBoxIndex.SelectedIndex = coordinateConversion.nIndex;
        }

        private void SetUI()
        {
            string[] dlls = Directory.GetFiles(Application.StartupPath + @"\Calibration", "*.dll");
            comboBoxType.Items.Clear();
            for (int i = 0; i < dlls.Length; i++)
            {
                string type = dlls[i].Split('.')[1];
                comboBoxType.Items.Add(type);
            }
            for (int i = 0; i < 40; i++)
            {
                comboBoxIndex.Items.Add(i);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            strType = comboBoxType.Text;
            nIndex = comboBoxIndex.SelectedIndex;
            this.Result = DialogResult.OK;
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Result = DialogResult.Cancel;
            this.Close();
        }
    }
}
