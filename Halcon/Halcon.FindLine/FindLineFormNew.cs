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

namespace Halcon.FindLine
{
    public partial class FindLineFormNew : Form
    {
        HImage source_Image;
        public FindLine MyFindLine;

        public FindLineFormNew(FindLine _findLine)
        {
            InitializeComponent();
            MyFindLine = new FindLine()
            {
                CaliperCount = _findLine.CaliperCount,
                CaliperWidth = _findLine.CaliperWidth,
                FindEdge = _findLine.FindEdge,
                FindSelect = _findLine.FindSelect,
                ModelImage = _findLine.ModelImage,
                ROIList = _findLine.ROIList,
                Sigma = _findLine.Sigma,
                Threshold = _findLine.Threshold
            };
            hObjectViewer1.SetImageHandle(ImageHandle);
            propertiesViewer1.SetProperties(MyFindLine);
            propertiesViewer1.SetHandle(new Action(ImageHandle));
            if (!string.IsNullOrEmpty(MyFindLine.ModelImage))
            {
                source_Image = new HImage(MyFindLine.ModelImage);
                hObjectViewer1.SetImage(source_Image);
                for (int i = 0; i < MyFindLine.ROIList.Count; i++)
                {
                    MyFindLine.ROIList[i].CreateDrawingObject();
                    hObjectViewer1.AttachDrawObj(MyFindLine.ROIList[i]);
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
                    MyFindLine.ModelImage = ofd.FileName;
                }
            }
        }

        private void ImageHandle()
        {
            if (source_Image != null)
            {
                if (hObjectViewer1.GetROIs().Count != 0)
                {
                    ROIRotateRectangle rotateRectangle = hObjectViewer1.GetROIs()[0] as ROIRotateRectangle;
                    int count = MyFindLine.CaliperCount;
                    int rowMid = count / 2;
                    double caliper = MyFindLine.CaliperWidth;
                    double centerX = 0;
                    double centerY = 0;
                    HTuple width, height;
                    source_Image.GetImageSize(out width, out height);
                    hObjectViewer1.ResetShowObjects();
                    for (int i = 0; i < count; i++)
                    {
                        HXLDCont rotateRect = new HXLDCont();
                        centerX = rotateRectangle.Column + (i - rowMid) * 2 * caliper * Math.Sin(rotateRectangle.Phi);//求中心点
                        centerY = rotateRectangle.Row + (i - rowMid) * 2 * caliper * Math.Cos(rotateRectangle.Phi);
                        rotateRect.GenRectangle2ContourXld(centerY, centerX, rotateRectangle.Phi, rotateRectangle.Length1, caliper);//生成取点区域
                        hObjectViewer1.AddShowObject(new ShowObject(rotateRect));
                        HMeasure hMeasure = new HMeasure();
                        hMeasure.GenMeasureRectangle2(centerY, centerX, rotateRectangle.Phi, rotateRectangle.Length1, caliper, width, height, "nearest_neighbor");
                        HTuple rowEdge, columnEdge, amplitude, distance;
                        string transition = MyFindLine.GetFindEdge();
                        string select = MyFindLine.GetFindSelect();
                        hMeasure.MeasurePos(source_Image, MyFindLine.Sigma, MyFindLine.Threshold, transition, select, out rowEdge, out columnEdge, out amplitude, out distance);
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

        private void FindLineFormNew_FormClosing(object sender, FormClosingEventArgs e)
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
            MyFindLine.ROIList = hObjectViewer1.GetROIs();
            DialogResult = DialogResult.OK;
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}
