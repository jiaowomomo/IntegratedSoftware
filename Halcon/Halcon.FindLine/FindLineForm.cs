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
    public partial class FindLineForm : Form
    {
        HImage source_Image;
        public string ModelImage = "";
        public int CaliperCount = 0;
        public double CaliperWidth = 0;
        public double Sigma = 1;
        public double Threshold = 30;
        public string FindEdge = "all";
        public string FindSelect = "all";
        public List<ROIBase> m_listROI = new List<ROIBase>();
        public DialogResult Result { get; set; }

        public FindLineForm()
        {
            InitializeComponent();
            hObjectViewer1.SetImageHandle(ImageHandle);
            comboBoxEdge.SelectedIndex = 0;
            comboBoxSelect.SelectedIndex = 0;
        }

        public FindLineForm(FindLine findLine)
        {
            InitializeComponent();
            hObjectViewer1.SetImageHandle(ImageHandle);
            numericUpDownCount.Value = Convert.ToDecimal(findLine.CaliperCount);
            numericUpDownWidth.Value = Convert.ToDecimal(findLine.CaliperWidth);
            numericUpDownSigma.Value = Convert.ToDecimal(findLine.Sigma);
            numericUpDownThreshold.Value = Convert.ToDecimal(findLine.Threshold);
            if (findLine.FindEdge == "all")
            {
                comboBoxEdge.SelectedIndex = 0;
            }
            else if (findLine.FindEdge == "negative")
            {
                comboBoxEdge.SelectedIndex = 1;
            }
            else
            {
                comboBoxEdge.SelectedIndex = 2;
            }
            if (findLine.FindSelect == "all")
            {
                comboBoxSelect.SelectedIndex = 0;
            }
            else if (findLine.FindSelect == "first")
            {
                comboBoxSelect.SelectedIndex = 1;
            }
            else
            {
                comboBoxSelect.SelectedIndex = 2;
            }
            ModelImage = findLine.ModelImage;
            source_Image = new HImage(ModelImage);
            hObjectViewer1.SetImage(source_Image);
            m_listROI = findLine.ROIList;
            for (int i = 0; i < m_listROI.Count; i++)
            {
                m_listROI[i].CreateDrawingObject();
                hObjectViewer1.AttachDrawObj(m_listROI[i]);
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
                    ModelImage = ofd.FileName;
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
                    int count = Convert.ToInt32(numericUpDownCount.Value);
                    int rowMid = count / 2;
                    double caliper = Convert.ToDouble(numericUpDownWidth.Value);
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
                        string transition = "all";
                        if (comboBoxEdge.SelectedIndex == 0)
                        {
                            transition = "all";
                        }
                        else if (comboBoxEdge.SelectedIndex == 1)
                        {
                            transition = "negative";
                        }
                        else
                        {
                            transition = "positive";
                        }
                        string select = "all";
                        if (comboBoxSelect.SelectedIndex == 0)
                        {
                            select = "all";
                        }
                        else if (comboBoxSelect.SelectedIndex == 1)
                        {
                            select = "first";
                        }
                        else
                        {
                            select = "last";
                        }
                        hMeasure.MeasurePos(source_Image, Convert.ToDouble(numericUpDownSigma.Value), Convert.ToDouble(numericUpDownThreshold.Value), transition, select, out rowEdge, out columnEdge, out amplitude, out distance);
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

        private void button2_Click(object sender, EventArgs e)
        {
            CaliperCount = Convert.ToInt32(numericUpDownCount.Value);
            CaliperWidth = Convert.ToDouble(numericUpDownWidth.Value);
            Sigma = Convert.ToDouble(numericUpDownSigma.Value);
            Threshold = Convert.ToDouble(numericUpDownThreshold.Value);
            if (comboBoxEdge.SelectedIndex == 0)
            {
                FindEdge = "all";
            }
            else if (comboBoxEdge.SelectedIndex == 1)
            {
                FindEdge = "negative";
            }
            else
            {
                FindEdge = "positive";
            }
            if (comboBoxSelect.SelectedIndex == 0)
            {
                FindSelect = "all";
            }
            else if (comboBoxSelect.SelectedIndex == 1)
            {
                FindSelect = "first";
            }
            else
            {
                FindSelect = "last";
            }
            m_listROI = hObjectViewer1.GetROIs();
            Result = DialogResult.OK;
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Result = DialogResult.Cancel;
            this.Close();
        }

        private void FindLineForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (source_Image != null)
            {
                source_Image.Dispose();
            }
            hObjectViewer1.ReleaseRam();
            this.Dispose();
            GC.Collect();
        }

        private void numericUpDown_ValueChanged(object sender, EventArgs e)
        {
            ImageHandle();
        }

        private void comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ImageHandle();
        }
    }
}
