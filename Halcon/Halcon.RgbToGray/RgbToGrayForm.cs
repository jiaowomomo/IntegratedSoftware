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

namespace Halcon.RgbToGray
{
    public partial class RgbToGrayForm : Form
    {
        HImage source_Image;
        public DialogResult Result { get; set; }

        public RgbToGrayForm()
        {
            InitializeComponent();
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
                    ImageProcessing();
                }
            }
        }

        private void ImageProcessing()
        {
            if (source_Image != null)
            {
                compareViewer1.SetSoureImage(source_Image);
                HImage gray = new HImage();
                if (compareViewer1.GetSoureImage().CountChannels() == 3)
                {
                    gray = compareViewer1.GetSoureImage().Rgb1ToGray();
                    compareViewer1.SetCompareImage(gray);
                }
                else
                {
                    compareViewer1.SetCompareImage(compareViewer1.GetSoureImage());
                }
                if (gray != null)
                {
                    gray.Dispose();
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Result = DialogResult.OK;
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Result = DialogResult.Cancel;
            this.Close();
        }

        private void RgbToGrayForm_FormClosing(object sender, FormClosingEventArgs e)
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
