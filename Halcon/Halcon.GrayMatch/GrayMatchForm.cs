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

namespace Halcon.GrayMatch
{
    public partial class GrayMatchForm : Form
    {
        HImage source_Image;
        public HTemplate hTemplate;
        public double AngleStart = 0;
        public double AngleEnd = 0;
        public double ScaleMin = 0.9;
        public double ScaleMax = 1.1;
        public double MinScore = 0.5;
        public int MatchCount = 1;
        public double Overlap = 0.5;
        public int Pyramid = 5;
        public double Greediness = 0.5;
        public string ModelImage = "";
        public double ModelCenterX = 0;
        public double ModelCenterY = 0;
        public List<ROIBase> m_listROI = new List<ROIBase>();
        HXLDCont hXLDCont;
        public DialogResult Result { get; set; }
        private bool isLoad = false;

        public GrayMatchForm()
        {
            InitializeComponent();

            hObjectViewer1.SetROICount(20);
            hObjectViewer1.SetImageHandle(ImageHandle);
            isLoad = true;
        }

        public GrayMatchForm(GrayMatch shapeMatch)
        {
            InitializeComponent();

            hObjectViewer1.SetROICount(20);
            hObjectViewer1.SetImageHandle(ImageHandle);
            hTemplate = shapeMatch.hTemplate.Clone();
            numericUpDownStartAngle.Value = Convert.ToDecimal(shapeMatch.AngleStart);
            numericUpDownEndAngle.Value = Convert.ToDecimal(shapeMatch.AngleEnd);
            numericUpDownScaleMin.Value = Convert.ToDecimal(shapeMatch.ScaleMin);
            numericUpDownScaleMax.Value = Convert.ToDecimal(shapeMatch.ScaleMax);
            numericUpDownMinScore.Value = Convert.ToDecimal(shapeMatch.MinScore);
            numericUpDownMatchCount.Value = Convert.ToDecimal(shapeMatch.MatchCount);
            numericUpDownOverlap.Value = Convert.ToDecimal(shapeMatch.Overlap);
            numericUpDownPyramid.Value = Convert.ToDecimal(shapeMatch.Pyramid);
            numericUpDownGreediness.Value = Convert.ToDecimal(shapeMatch.Greediness);
            ModelImage = shapeMatch.ModelImage;
            source_Image = new HImage(ModelImage);
            hObjectViewer1.SetImage(source_Image);
            m_listROI = shapeMatch.ROIList;
            for (int i = 0; i < m_listROI.Count; i++)
            {
                m_listROI[i].CreateDrawingObject();
                hObjectViewer1.AttachDrawObj(m_listROI[i]);
            }
            HRegion hRegion = hObjectViewer1.GetRegions();
            if (hXLDCont != null)
            {
                hXLDCont.Dispose();
            }
            hXLDCont = hRegion.GenContourRegionXld("border");
            hRegion.Dispose();
            isLoad = true;
        }

        private void ImageHandle()
        {
            HRegion hRegion = hObjectViewer1.GetRegions();
            if (hRegion != null)
            {
                double angleStart = 3.14 * Convert.ToDouble(numericUpDownStartAngle.Value) / 180.0;
                double angleExtent = 3.14 * Convert.ToDouble(numericUpDownEndAngle.Value) / 180.0 - angleStart;
                HImage hImage = source_Image.ReduceDomain(hRegion);
                try
                {
                    //if (checkBoxAuto.Checked)
                    //{
                    //    HTuple param, paramValue;
                    //    HOperatorSet.DetermineShapeModelParams(hImage, "auto", angleStart, angleExtent, Convert.ToDouble(numericUpDownScaleMin.Value), Convert.ToDouble(numericUpDownScaleMax.Value), "auto", "use_polarity", "auto", "auto", "all", out param, out paramValue);
                    //    SetUI(param, paramValue);
                    //}
                    //HTuple threshold = new HTuple(Convert.ToInt32(numericUpDownContrastLow.Value)).TupleConcat(Convert.ToInt32(numericUpDownContrastHigh.Value)).TupleConcat(Convert.ToInt32(numericUpDownMinSize.Value));

                    if (hTemplate != null)
                    {
                        hTemplate.Dispose();
                    }
                    hTemplate = hImage.CreateTemplateRot(4, angleStart, angleExtent, 0.0982, "sort", "original");
                    if (hXLDCont != null)
                    {
                        hXLDCont.Dispose();
                    }
                    hXLDCont = hRegion.GenContourRegionXld("border");
                    HTuple row, column;
                    hRegion.AreaCenter(out row, out column);
                    ModelCenterX = column.D;
                    ModelCenterY = row.D;
                    hObjectViewer1.ResetShowObjects();
                    hObjectViewer1.AddShowObject(new Halcon.Functions.ShowObject(hXLDCont.Clone()));
                    hObjectViewer1.ResetWndCtrl(false);
                    toolStripStatusLabel1.Text = "创建模板成功";
                }
                catch
                {
                    toolStripStatusLabel1.Text = "创建模板失败";
                }
                hRegion.Dispose();
                hImage.Dispose();
            }
        }

        private void SetUI(HTuple param, HTuple paramValue)
        {
            for (int i = 0; i < param.Length; i++)
            {
                if (param[i] == "num_levels")
                {
                    numericUpDownPyramid.Value = paramValue[i];
                }
                else if (param[i] == "angle_step")
                {
                    double num = paramValue[i];
                    numericUpDownAngleStep.Value = Convert.ToDecimal(num);
                }
                else if (param[i] == "scale_step")
                {
                    numericUpDownScaleStep.Value = Convert.ToDecimal(paramValue[i].D);
                }
                else if (param[i] == "contrast_low")
                {
                    numericUpDownContrastLow.Value = paramValue[i];
                }
                else if (param[i] == "contrast_high")
                {
                    numericUpDownContrastHigh.Value = paramValue[i];
                }
                else if (param[i] == "min_size")
                {
                    numericUpDownMinSize.Value = paramValue[i];
                }
                else if (param[i] == "min_contrast")
                {
                    numericUpDownContrastMin.Value = paramValue[i];
                }
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

        private void ShapeMatchForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (source_Image != null)
            {
                source_Image.Dispose();
            }
            if (hXLDCont != null)
            {
                hXLDCont.Dispose();
            }
            hObjectViewer1.ReleaseRam();
            this.Dispose();
            GC.Collect();
        }

        private void checkBoxAuto_CheckedChanged(object sender, EventArgs e)
        {
            numericUpDownPyramid.Enabled = !checkBoxAuto.Checked;
            numericUpDownAngleStep.Enabled = !checkBoxAuto.Checked;
            numericUpDownScaleStep.Enabled = !checkBoxAuto.Checked;
            numericUpDownContrastLow.Enabled = !checkBoxAuto.Checked;
            numericUpDownContrastHigh.Enabled = !checkBoxAuto.Checked;
            numericUpDownMinSize.Enabled = !checkBoxAuto.Checked;
            numericUpDownContrastMin.Enabled = !checkBoxAuto.Checked;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            double angleStart = 3.14 * Convert.ToDouble(numericUpDownStartAngle.Value) / 180.0;
            double angleExtent = 3.14 * Convert.ToDouble(numericUpDownEndAngle.Value) / 180.0 - angleStart;
            HTuple row, column, angle, error;
            source_Image.BestMatchRotMg(hTemplate, angleStart, angleExtent, 30, "false", 4, out row, out column, out angle, out error);
            hObjectViewer1.ResetShowObjects();
            textBox1.Text = "";
            for (int i = 0; i < angle.Length; i++)
            {
                //创建二维矩阵
                HHomMat2D hv_HomMat2D = new HHomMat2D();
                hv_HomMat2D.VectorAngleToRigid(new HTuple(ModelCenterY), new HTuple(ModelCenterX), 0, row.TupleSelect(i), column.TupleSelect(i), angle.TupleSelect(i));
                //识别到的角度
                HHomMat2D transMat;
                transMat = hv_HomMat2D.HomMat2dTranslate(row, column);
                transMat = hv_HomMat2D.HomMat2dRotate(angle.TupleSelect(i), row, column);
                //识别到的行列坐标
                transMat = transMat.HomMat2dTranslate(row.TupleSelect(i), column.TupleSelect(i));
                //变换
                HXLDCont findCont;
                findCont = hXLDCont.AffineTransContourXld(hv_HomMat2D);
                hObjectViewer1.AddShowObject(new Halcon.Functions.ShowObject(findCont.Clone(), "green"));
                findCont.Dispose();
                textBox1.Text += "X:" + column.TupleSelect(i).D.ToString("0.000") + "  Y:" + row.TupleSelect(i).D.ToString("0.000") + "  A:" + angle.TupleSelect(i).D.ToString("0.000") + "  Error:" + error.TupleSelect(i).D.ToString("0.000") + "\r\n";
            }
            hObjectViewer1.ResetWndCtrl(false);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            AngleStart = Convert.ToDouble(numericUpDownStartAngle.Value);
            AngleEnd = Convert.ToDouble(numericUpDownEndAngle.Value);
            ScaleMin = Convert.ToDouble(numericUpDownScaleMin.Value);
            ScaleMax = Convert.ToDouble(numericUpDownScaleMax.Value);
            MinScore = Convert.ToDouble(numericUpDownMinScore.Value);
            MatchCount = Convert.ToInt32(numericUpDownMatchCount.Value);
            Overlap = Convert.ToDouble(numericUpDownOverlap.Value);
            Pyramid = Convert.ToInt32(numericUpDownPyramid.Value);
            Greediness = Convert.ToDouble(numericUpDownGreediness.Value);
            m_listROI = hObjectViewer1.GetROIs();
            Result = DialogResult.OK;
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Result = DialogResult.Cancel;
            this.Close();
        }

        private void numericUpDownCreate_ValueChanged(object sender, EventArgs e)
        {
            if (isLoad)
            {
                ImageHandle();
            }
        }

        private void numericUpDownFind_ValueChanged(object sender, EventArgs e)
        {
            if (isLoad)
            {
                button1_Click(null, new EventArgs());
            }
        }
    }
}
