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

namespace Halcon.BinaryThreshold
{
    public partial class BinaryThresholdFormNew : Form
    {
        HImage source_Image;
        public BinaryThreshold MyBinaryThreshold;

        public BinaryThresholdFormNew(BinaryThreshold _binaryThreshold)
        {
            InitializeComponent();
            MyBinaryThreshold = new BinaryThreshold()
            {
                nNGValue = _binaryThreshold.nNGValue,
                nOKValue = _binaryThreshold.nOKValue,
                ModelImage = _binaryThreshold.ModelImage,
                strMethod = _binaryThreshold.strMethod,
                strRegion = _binaryThreshold.strRegion
            };
            propertiesViewer1.SetProperties(MyBinaryThreshold);
            propertiesViewer1.SetHandle(new Action(ImageProcessing));
            if (!string.IsNullOrEmpty(MyBinaryThreshold.ModelImage))
            {
                source_Image = new HImage(MyBinaryThreshold.ModelImage);
                ImageProcessing();
            }
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
                    MyBinaryThreshold.ModelImage = ofd.FileName;
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
                HTuple useThreshold;
                threshold = compareViewer1.GetSoureImage().BinaryThreshold(MyBinaryThreshold.strMethod, MyBinaryThreshold.GetExtract(), out useThreshold);
                HTuple width, height;
                compareViewer1.GetSoureImage().GetImageSize(out width, out height);
                binary = threshold.RegionToBin(MyBinaryThreshold.nOKValue, MyBinaryThreshold.nNGValue, width, height);
                compareViewer1.SetCompareImage(binary);
                threshold.Dispose();
                binary.Dispose();
            }
        }

        private void BinaryThresholdFormNew_FormClosing(object sender, FormClosingEventArgs e)
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
