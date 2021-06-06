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

namespace Halcon.HysteresisThreshold
{
    public partial class HysteresisThresholdForm : Form
    {
        public int nThresholdMin = 0;
        public int nThresholdMax = 0;
        public int nLength = 1;
        public int nOKValue = 0;
        public int nNGValue = 0;
        public string ModelImage = "";
        HImage source_Image;
        public DialogResult Result { get; set; }

        public HysteresisThresholdForm()
        {
            InitializeComponent();
        }

        public HysteresisThresholdForm(HysteresisThreshold hysteresisThreshold)
        {
            InitializeComponent();
            numericUpDownMin.Value = Convert.ToDecimal(hysteresisThreshold.nThresholdMin);
            numericUpDownMax.Value = Convert.ToDecimal(hysteresisThreshold.nThresholdMax);
            numericUpDownLength.Value = Convert.ToDecimal(hysteresisThreshold.nLength);
            numericUpDownOK.Value = Convert.ToDecimal(hysteresisThreshold.nOKValue);
            numericUpDownNG.Value = Convert.ToDecimal(hysteresisThreshold.nNGValue);
            ModelImage = hysteresisThreshold.ModelImage;
            source_Image = new HImage(ModelImage);
            compareViewer1.SetSoureImage(source_Image);
            ImageProcessing();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            nThresholdMin = Convert.ToInt32(numericUpDownMin.Value);
            nThresholdMax = Convert.ToInt32(numericUpDownMax.Value);
            nLength = Convert.ToInt32(numericUpDownLength.Value);
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
                threshold = compareViewer1.GetSoureImage().HysteresisThreshold(Convert.ToInt32(numericUpDownMin.Value), Convert.ToInt32(numericUpDownMax.Value), Convert.ToInt32(numericUpDownLength.Value));
                HTuple width, height;
                compareViewer1.GetSoureImage().GetImageSize(out width, out height);
                binary = threshold.RegionToBin(Convert.ToInt32(numericUpDownOK.Value), Convert.ToInt32(numericUpDownNG.Value), width, height);
                compareViewer1.SetCompareImage(binary);
                threshold.Dispose();
                binary.Dispose();
            }
        }

        private void numericUpDownMin_ValueChanged(object sender, EventArgs e)
        {
            if (numericUpDownMin.Value > numericUpDownMax.Value)
            {
                numericUpDownMax.Value = numericUpDownMin.Value;
            }
            else if (numericUpDownMax.Value < numericUpDownMin.Value)
            {
                numericUpDownMin.Value = numericUpDownMax.Value;
            }
            ImageProcessing();
        }

        private void HysteresisThresholdForm_FormClosing(object sender, FormClosingEventArgs e)
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
