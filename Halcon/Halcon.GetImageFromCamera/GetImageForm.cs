using Camera.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Halcon.GetImageFromCamera
{
    public partial class GetImageForm : Form
    {
        public bool IsSetExposure = false;
        public double ExposureTime = 10000;
        public int CameraIndex = -1;
        public DialogResult Result { get; set; }

        public GetImageForm()
        {
            InitializeComponent();
            comboBox1.Items.Clear();
            for (int i = 0; i < CameraControl.Instance.GetCameras().Count; i++)
            {
                comboBox1.Items.Add(i);
            }
        }

        public GetImageForm(GetImage getImage)
        {
            InitializeComponent();
            comboBox1.Items.Clear();
            for (int i = 0; i < CameraControl.Instance.GetCameras().Count; i++)
            {
                comboBox1.Items.Add(i);
            }
            checkBox1.Checked = getImage.IsSetExposure;
            numericUpDown1.Value = Convert.ToDecimal(getImage.ExposureTime);
            comboBox1.SelectedIndex = getImage.CameraIndex;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            IsSetExposure = checkBox1.Checked;
            ExposureTime = Convert.ToDouble(numericUpDown1.Value);
            CameraIndex = comboBox1.SelectedIndex;
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
