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
    public partial class LocalThresholdFormNew : Form
    {
        HImage source_Image;
        public LocalThreshold MyLocalThreshold;

        public LocalThresholdFormNew(LocalThreshold _localThreshold)
        {
            InitializeComponent();
            MyLocalThreshold = new LocalThreshold()
            {
                nNGValue = _localThreshold.nNGValue,
                nOKValue = _localThreshold.nOKValue,
                ModelImage = _localThreshold.ModelImage,
                strMethod = _localThreshold.strMethod,
                strRegion = _localThreshold.strRegion
            };
            propertiesViewer1.SetProperties(MyLocalThreshold);
            propertiesViewer1.SetHandle(new Action(ImageProcessing));
            if (!string.IsNullOrEmpty(MyLocalThreshold.ModelImage))
            {
                source_Image = new HImage(MyLocalThreshold.ModelImage);
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
                    MyLocalThreshold.ModelImage = ofd.FileName;
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
                threshold = compareViewer1.GetSoureImage().LocalThreshold(MyLocalThreshold.strMethod, MyLocalThreshold.GetExtract(), new HTuple(), new HTuple());
                HTuple width, height;
                compareViewer1.GetSoureImage().GetImageSize(out width, out height);
                binary = threshold.RegionToBin(MyLocalThreshold.nOKValue, MyLocalThreshold.nNGValue, width, height);
                compareViewer1.SetCompareImage(binary);
                threshold.Dispose();
                binary.Dispose();
            }
        }

        private void LocalThresholdFormNew_FormClosing(object sender, FormClosingEventArgs e)
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
