using HalconDotNet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Halcon.LocalThreshold
{
    public partial class LocalThresholdForm : Form
    {
        public string strMethod = "adapted_std_deviation";
        public string strRegion = "dark";
        public int nOKValue = 0;
        public int nNGValue = 0;
        public string ModelImage = "";
        HImage source_Image;
        public DialogResult Result { get; set; }

        public LocalThresholdForm()
        {
            InitializeComponent();

            comboBoxRegion.SelectedIndex = 0;
        }

        public LocalThresholdForm(LocalThreshold localThreshold)
        {
            InitializeComponent();

            comboBoxRegion.SelectedIndex = localThreshold.strRegion == "light" ? 0 : 1;
            numericUpDownOK.Value = Convert.ToDecimal(localThreshold.nOKValue);
            numericUpDownNG.Value = Convert.ToDecimal(localThreshold.nNGValue);
            ModelImage = localThreshold.ModelImage;
            source_Image = new HImage(ModelImage);
            compareViewer1.SetSoureImage(source_Image);
            ImageProcessing();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            strRegion = comboBoxRegion.Text == "亮" ? "light" : "dark";
            nOKValue = Convert.ToInt32(numericUpDownOK.Value);
            nNGValue = Convert.ToInt32(numericUpDownNG.Value);
            Result = DialogResult.OK;
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Result = DialogResult.Cancel;
            this.Close();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "BMP File|*.bmp|PNG File|*.png|JPEG File|*.jpg|All|*.*";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                if (!string.IsNullOrEmpty(ofd.FileName))
                {
                    source_Image = new HImage(ofd.FileName);
                    ModelImage = ofd.FileName;
                    ImageProcessing();
                }
            }
        }

        private void ImageProcessing()
        {
            if (source_Image != null)
            {
                compareViewer1.SetSoureImage(source_Image);
                HRegion threshold;
                HImage binary;
                threshold = compareViewer1.GetSoureImage().LocalThreshold(strMethod, comboBoxRegion.Text == "亮" ? "light" : "dark", new HTuple(), new HTuple());
                HTuple width, height;
                compareViewer1.GetSoureImage().GetImageSize(out width, out height);
                binary = threshold.RegionToBin(Convert.ToInt32(numericUpDownOK.Value), Convert.ToInt32(numericUpDownNG.Value), width, height);
                compareViewer1.SetCompareImage(binary);
                threshold.Dispose();
                binary.Dispose();
            }
        }

        private void comboBoxRegion_SelectedIndexChanged(object sender, EventArgs e)
        {
            ImageProcessing();
        }

        private void numericUpDownOK_ValueChanged(object sender, EventArgs e)
        {
            ImageProcessing();
        }

        private void LocalThresholdForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (source_Image != null)
            {
                source_Image.Dispose();
            }
            compareViewer1.ReleaseRam();
            this.Dispose();
            GC.Collect();
        }
    }
}
