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

namespace Halcon.Threshold
{
    public partial class ThresholdFormNew : Form
    {
        HImage source_Image;
        public Threshold MyThreshold;

        public ThresholdFormNew(Threshold _threshold)
        {
            InitializeComponent();
            MyThreshold = new Threshold()
            {
                ModelImage = _threshold.ModelImage,
                dbThresholdMax = _threshold.dbThresholdMax,
                dbThresholdMin = _threshold.dbThresholdMin,
                nNGValue = _threshold.nNGValue,
                nOKValue = _threshold.nOKValue
            };
            propertiesViewer1.SetProperties(MyThreshold);
            propertiesViewer1.SetHandle(new Action(ImageProcessing));
            if (!string.IsNullOrEmpty(MyThreshold.ModelImage))
            {
                source_Image = new HImage(MyThreshold.ModelImage);
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
                    if (source_Image != null)
                    {
                        source_Image.Dispose();
                    }
                    source_Image = new HImage(ofd.FileName);
                    MyThreshold.ModelImage = ofd.FileName;
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
                threshold = compareViewer1.GetSoureImage().Threshold(MyThreshold.dbThresholdMin, MyThreshold.dbThresholdMax);
                HTuple width, height;
                compareViewer1.GetSoureImage().GetImageSize(out width, out height);
                binary = threshold.RegionToBin(MyThreshold.nOKValue, MyThreshold.nNGValue, width, height);
                compareViewer1.SetCompareImage(binary);
                threshold.Dispose();
                binary.Dispose();
            }
        }

        private void ThresholdFormNew_FormClosing(object sender, FormClosingEventArgs e)
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
