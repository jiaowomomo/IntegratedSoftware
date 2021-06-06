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
    public partial class HysteresisThresholdFormNew : Form
    {
        HImage source_Image;
        public HysteresisThreshold MyHysteresisThreshold;

        public HysteresisThresholdFormNew(HysteresisThreshold _hysteresisThreshold)
        {
            InitializeComponent();
            MyHysteresisThreshold = new HysteresisThreshold()
            {
                nNGValue = _hysteresisThreshold.nNGValue,
                nOKValue = _hysteresisThreshold.nOKValue,
                nLength = _hysteresisThreshold.nLength,
                nThresholdMax = _hysteresisThreshold.nThresholdMax,
                nThresholdMin = _hysteresisThreshold.nThresholdMin,
                ModelImage = _hysteresisThreshold.ModelImage
            };
            propertiesViewer1.SetProperties(MyHysteresisThreshold);
            propertiesViewer1.SetHandle(new Action(ImageProcessing));
            if (!string.IsNullOrEmpty(MyHysteresisThreshold.ModelImage))
            {
                source_Image = new HImage(MyHysteresisThreshold.ModelImage);
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
                    MyHysteresisThreshold.ModelImage = ofd.FileName;
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
                threshold = compareViewer1.GetSoureImage().HysteresisThreshold(MyHysteresisThreshold.nThresholdMin, MyHysteresisThreshold.nThresholdMax, MyHysteresisThreshold.nLength);
                HTuple width, height;
                compareViewer1.GetSoureImage().GetImageSize(out width, out height);
                binary = threshold.RegionToBin(MyHysteresisThreshold.nOKValue, MyHysteresisThreshold.nNGValue, width, height);
                compareViewer1.SetCompareImage(binary);
                threshold.Dispose();
                binary.Dispose();
            }
        }

        private void HysteresisThresholdFormNew_FormClosing(object sender, FormClosingEventArgs e)
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
