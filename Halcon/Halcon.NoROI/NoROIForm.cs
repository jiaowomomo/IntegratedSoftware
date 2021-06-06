using Halcon.Functions;
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

namespace Halcon.NoROI
{
    public partial class NoROIForm : Form
    {
        HImage source_Image;
        public List<ROIBase> m_listROI = new List<ROIBase>();
        public string ModelImage = "";
        public DialogResult Result { get; set; }

        public NoROIForm()
        {
            InitializeComponent();
            compareViewer1.SetImageHandle(ImageThreshold);
            compareViewer1.SetROICount(20);
        }

        public NoROIForm(NoROI noROI)
        {
            InitializeComponent();
            compareViewer1.SetImageHandle(ImageThreshold);
            compareViewer1.SetROICount(20);
            m_listROI = noROI.ROIList;
            for (int i = 0; i < m_listROI.Count; i++)
            {
                compareViewer1.AttachDrawObj(m_listROI[i]);
            }
            ModelImage = noROI.ModelImage;
            source_Image = new HImage(ModelImage);
            compareViewer1.SetSoureImage(source_Image);
            ImageThreshold();
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
                    compareViewer1.SetSoureImage(source_Image);
                    ModelImage = ofd.FileName;
                    ImageThreshold();
                }
            }
        }

        object handleLock = new object();

        private void ImageThreshold()
        {
            lock (handleLock)
            {
                if (source_Image != null)
                {
                    HRegion hRegion = compareViewer1.GetRegions();
                    if (hRegion != null)
                    {
                        HRegion hRegionAll = source_Image.GetDomain();
                        hRegionAll = hRegionAll.Difference(hRegion);
                        HImage hImage = source_Image.ReduceDomain(hRegionAll);
                        compareViewer1.SetCompareImage(hImage);
                        hRegion.Dispose();
                        hImage.Dispose();
                        hRegionAll.Dispose();
                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            m_listROI = compareViewer1.GetROIs();
            Result = DialogResult.OK;
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Result = DialogResult.Cancel;
            this.Close();
        }

        private void NoROIForm_FormClosing(object sender, FormClosingEventArgs e)
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
