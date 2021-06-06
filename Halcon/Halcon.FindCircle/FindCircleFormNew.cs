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

namespace Halcon.FindCircle
{
    public partial class FindCircleFormNew : Form
    {
        HImage source_Image;
        public FindCircle MyFindCircle;

        public FindCircleFormNew(FindCircle _findCircle)
        {
            InitializeComponent();
            MyFindCircle = new FindCircle()
            {
                CaliperCount = _findCircle.CaliperCount,
                CaliperWidth = _findCircle.CaliperWidth,
                FindEdge = _findCircle.FindEdge,
                FindSelect = _findCircle.FindSelect,
                ModelImage = _findCircle.ModelImage,
                ROIList = _findCircle.ROIList,
                Sigma = _findCircle.Sigma,
                Threshold = _findCircle.Threshold
            };
            hObjectViewer1.SetImageHandle(ImageHandle);
            propertiesViewer1.SetProperties(MyFindCircle);
            propertiesViewer1.SetHandle(new Action(ImageHandle));
            if (!string.IsNullOrEmpty(MyFindCircle.ModelImage))
            {
                source_Image = new HImage(MyFindCircle.ModelImage);
                hObjectViewer1.SetImage(source_Image);
                for (int i = 0; i < MyFindCircle.ROIList.Count; i++)
                {
                    MyFindCircle.ROIList[i].CreateDrawingObject();
                    hObjectViewer1.AttachDrawObj(MyFindCircle.ROIList[i]);
                }
                ImageHandle();
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
                    hObjectViewer1.SetImage(source_Image);
                    MyFindCircle.ModelImage = ofd.FileName;
                }
            }
        }

        private void ImageHandle()
        {
            if (source_Image != null)
            {
                if (hObjectViewer1.GetROIs().Count != 0)
                {
                    ROICircle rOICircle = hObjectViewer1.GetROIs()[0] as ROICircle;
                    int count = MyFindCircle.CaliperCount;
                    double caliper = MyFindCircle.CaliperWidth;
                    double centerX = 0;
                    double centerY = 0;
                    HTuple width, height;
                    source_Image.GetImageSize(out width, out height);
                    hObjectViewer1.ResetShowObjects();
                    for (int i = 0; i < count; i++)
                    {
                        HXLDCont rotateRect = new HXLDCont();
                        centerX = rOICircle.Column + rOICircle.Radius * Math.Cos(i * Math.PI * 2 / count);//求中心点
                        centerY = rOICircle.Row + rOICircle.Radius * Math.Sin(i * Math.PI * 2 / count);
                        rotateRect.GenRectangle2ContourXld(centerY, centerX, -i * Math.PI * 2 / count, rOICircle.Radius, caliper);//生成取点区域
                        hObjectViewer1.AddShowObject(new ShowObject(rotateRect));
                        HMeasure hMeasure = new HMeasure();
                        hMeasure.GenMeasureRectangle2(centerY, centerX, -i * Math.PI * 2 / count, rOICircle.Radius, caliper, width, height, "nearest_neighbor");
                        HTuple rowEdge, columnEdge, amplitude, distance;
                        string transition = MyFindCircle.GetFindEdge();
                        string select = MyFindCircle.GetFindSelect();
                        hMeasure.MeasurePos(source_Image, MyFindCircle.Sigma, MyFindCircle.Threshold, transition, select, out rowEdge, out columnEdge, out amplitude, out distance);
                        for (int j = 0; j < rowEdge.Length; j++)
                        {
                            HObject hObject;
                            HOperatorSet.GenCrossContourXld(out hObject, rowEdge.TupleSelect(j), columnEdge.TupleSelect(j), 6.0, 0.785398);
                            hObjectViewer1.AddShowObject(new ShowObject(hObject, "yellow"));
                        }
                        hMeasure.Dispose();
                    }
                    hObjectViewer1.ResetWndCtrl(false);
                }
            }
        }

        private void FindCircleFormNew_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (source_Image != null)
            {
                source_Image.Dispose();
            }
            hObjectViewer1.ReleaseRam();
            this.Dispose();
            GC.Collect();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            MyFindCircle.ROIList = hObjectViewer1.GetROIs();
            DialogResult = DialogResult.OK;
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}
