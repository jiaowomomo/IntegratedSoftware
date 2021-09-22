using Halcon.Functions;
using HalconDotNet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Halcon.FindLine
{
    [Serializable]
    public class FindLine : IImageHalconObject
    {
        private string _ModelImage = "";
        [Description("模板图片"), ReadOnly(true), Category("设置")]
        public string ModelImage
        {
            get { return _ModelImage; }
            set { _ModelImage = value; }
        }

        private int _CaliperCount = 10;
        [Description("卡尺数量"), Category("卡尺设置"), Editor(typeof(CaliperCountEditor), typeof(UITypeEditor))]
        public int CaliperCount
        {
            get { return _CaliperCount; }
            set
            {
                if (value >= 1 && value <= 100)
                    _CaliperCount = value;
            }
        }

        private double _CaliperWidth = 10;
        [Description("卡尺宽度"), Category("卡尺设置"), Editor(typeof(CaliperWidthEditor), typeof(UITypeEditor))]
        public double CaliperWidth
        {
            get { return _CaliperWidth; }
            set
            {
                if (value >= 1 && value <= 100)
                    _CaliperWidth = value;
            }
        }

        private double _Sigma = 1;
        [Description("平滑系数"), Category("查找参数"), Editor(typeof(SigmaEditor), typeof(UITypeEditor))]
        public double Sigma
        {
            get { return _Sigma; }
            set
            {
                if (value >= 1 && value <= 100)
                    _Sigma = value;
            }
        }

        private double _Threshold = 30;
        [Description("边缘阈值"), Category("查找参数"), Editor(typeof(ThresholdEditor), typeof(UITypeEditor))]
        public double Threshold
        {
            get { return _Threshold; }
            set
            {
                if (value >= 1 && value <= 100)
                    _Threshold = value;
            }
        }

        private string _FindEdge = "全部";
        [Description("查找边缘"), Category("查找参数"), TypeConverter(typeof(FindEdgeConvert))]
        public string FindEdge
        {
            get { return _FindEdge; }
            set { _FindEdge = value; }
        }

        private string _FindSelect = "全部";
        [Description("选择条件"), Category("查找参数"), TypeConverter(typeof(FindSelectConvert))]
        public string FindSelect
        {
            get { return _FindSelect; }
            set { _FindSelect = value; }
        }

        public FindLine()
        {
            GetDataManager.AddInputDouble("轮廓中心X");
            GetDataManager.AddInputDouble("轮廓中心Y");
            GetDataManager.AddInputDoubleArray("匹配位置X");
            GetDataManager.AddInputDoubleArray("匹配位置Y");
            GetDataManager.AddInputDoubleArray("匹配角度");
            GetDataManager.AddOutputInt("直线数");
            GetDataManager.AddOutputDoubleArray("起点X");
            GetDataManager.AddOutputDoubleArray("起点Y");
            GetDataManager.AddOutputDoubleArray("终点X");
            GetDataManager.AddOutputDoubleArray("终点Y");
        }

        public string GetFindEdge()
        {
            if (FindEdge == "全部")
                return "all";
            else if (FindEdge == "由白到黑")
                return "negative";
            else
                return "positive";
        }

        public string GetFindSelect()
        {
            if (FindSelect == "全部")
                return "all";
            else if (FindSelect == "第一条边")
                return "first";
            else
                return "last";
        }

        public override void EditParameters()
        {
            //FindLineForm findLineForm = new FindLineForm(this);
            //findLineForm.ShowDialog();
            //if (findLineForm.Result == System.Windows.Forms.DialogResult.OK)
            //{
            //    ModelImage = findLineForm.ModelImage;
            //    CaliperCount = findLineForm.CaliperCount;
            //    CaliperWidth = findLineForm.CaliperWidth;
            //    Sigma = findLineForm.Sigma;
            //    Threshold = findLineForm.Threshold;
            //    FindEdge = findLineForm.FindEdge;
            //    FindSelect = findLineForm.FindSelect;
            //    m_listROI = findLineForm.m_listROI;
            //    IsSetupOK = true;
            //}
            //else
            //{
            //    IsSetupOK = false;
            //}
            FindLineFormNew findLineForm = new FindLineFormNew(this);
            if (findLineForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                ModelImage = findLineForm.MyFindLine.ModelImage;
                CaliperCount = findLineForm.MyFindLine.CaliperCount;
                CaliperWidth = findLineForm.MyFindLine.CaliperWidth;
                Sigma = findLineForm.MyFindLine.Sigma;
                Threshold = findLineForm.MyFindLine.Threshold;
                FindEdge = findLineForm.MyFindLine.FindEdge;
                FindSelect = findLineForm.MyFindLine.FindSelect;
                ROIList = findLineForm.MyFindLine.ROIList;
                IsSetupOK = true;
            }
            else
            {
                IsSetupOK = false;
            }
        }

        public override void Execute(ref HImage source, ref List<ShowObject> showObjects, ref List<ShowText> showTexts)
        {
            if (ROIList.Count != 0)
            {
                ROIRotateRectangle rotateRectangle = ROIList[0] as ROIRotateRectangle;
                int rowMid = CaliperCount / 2;
                double centerX = 0;
                double centerY = 0;
                HTuple width, height;
                if (source == null || source.Key == IntPtr.Zero)
                {
                    throw new RunException(RunExceptionType.NoInputImage);
                }
                source.GetImageSize(out width, out height);
                double ModelX = GetDataManager.GetInputDouble("轮廓中心X");
                double ModelY = GetDataManager.GetInputDouble("轮廓中心Y");
                List<double> posX = GetDataManager.GetInputDoubleArray("匹配位置X");
                List<double> posY = GetDataManager.GetInputDoubleArray("匹配位置Y");
                List<double> angle = GetDataManager.GetInputDoubleArray("匹配角度");
                int count = 0;
                List<double> lineStartX = new List<double>();
                List<double> lineStartY = new List<double>();
                List<double> lineEndX = new List<double>();
                List<double> lineEndY = new List<double>();
                for (int j = 0; j < angle.Count; j++)
                {
                    HHomMat2D hHomMat2D = new HHomMat2D();
                    hHomMat2D.VectorAngleToRigid(ModelY, ModelX, 0, posY[j], posX[j], angle[j]);
                    HTuple rowTrans, colTrans;
                    hHomMat2D.AffineTransPixel(rotateRectangle.Row, rotateRectangle.Column, out rowTrans, out colTrans);
                    HTuple rows = new HTuple();
                    HTuple colums = new HTuple();
                    for (int i = 0; i < CaliperCount; i++)
                    {
                        centerX = colTrans + (i - rowMid) * 2 * CaliperWidth * Math.Sin(rotateRectangle.Phi + angle[j]);//求中心点
                        centerY = rowTrans + (i - rowMid) * 2 * CaliperWidth * Math.Cos(rotateRectangle.Phi + angle[j]);
                        HMeasure hMeasure = new HMeasure();
                        hMeasure.GenMeasureRectangle2(centerY, centerX, rotateRectangle.Phi+angle[j], rotateRectangle.Length1, CaliperWidth, width, height, "nearest_neighbor");
                        HTuple rowEdge, columnEdge, amplitude, distance;
                        hMeasure.MeasurePos(source, Sigma, Threshold, GetFindEdge(), GetFindSelect(), out rowEdge, out columnEdge, out amplitude, out distance);
                        for (int k = 0; k < rowEdge.Length; k++)
                        {
                            HObject hObject;
                            rows.Append(rowEdge.TupleSelect(k));
                            colums.Append(columnEdge.TupleSelect(k));
                            HOperatorSet.GenCrossContourXld(out hObject, rowEdge.TupleSelect(k), columnEdge.TupleSelect(k), 6.0, 0.785398);
                            showObjects.Add(new ShowObject(hObject, "yellow"));
                        }
                        hMeasure.Dispose();
                    }
                    if (rows.Length == 0)
                    {
                        continue;
                    }
                    HXLDCont hXLDCont = new HXLDCont();
                    hXLDCont.GenContourPolygonXld(rows, colums);
                    HTuple rowBegin, colBegin, rowEnd, colEnd, nr, nc, dist;
                    hXLDCont.FitLineContourXld("tukey", -1, 0, 5, 2.0, out rowBegin, out colBegin, out rowEnd, out colEnd, out nr, out nc, out dist);
                    hXLDCont.Dispose();
                    HXLDCont fitLine = new HXLDCont();
                    fitLine.GenContourPolygonXld(rowBegin.TupleConcat(rowEnd), colBegin.TupleConcat(colEnd));
                    showObjects.Add(new ShowObject(fitLine, "blue"));
                    count++;
                    lineStartX.Add(colBegin.D);
                    lineStartY.Add(rowBegin.D);
                    lineEndX.Add(colEnd.D);
                    lineEndY.Add(rowEnd.D);
                }
                GetDataManager.SetOutputInt("直线数", count);
                GetDataManager.SetOutputDoubleArray("起点X", lineStartX);
                GetDataManager.SetOutputDoubleArray("起点Y", lineStartY);
                GetDataManager.SetOutputDoubleArray("终点X", lineEndX);
                GetDataManager.SetOutputDoubleArray("终点Y", lineEndY);
            }
        }

        public override void SetParameters()
        {
            //FindLineForm findLineForm = new FindLineForm();
            //findLineForm.ShowDialog();
            //if (findLineForm.Result == System.Windows.Forms.DialogResult.OK)
            //{
            //    ModelImage = findLineForm.ModelImage;
            //    CaliperCount = findLineForm.CaliperCount;
            //    CaliperWidth = findLineForm.CaliperWidth;
            //    Sigma = findLineForm.Sigma;
            //    Threshold = findLineForm.Threshold;
            //    FindEdge = findLineForm.FindEdge;
            //    FindSelect = findLineForm.FindSelect;
            //    m_listROI = findLineForm.m_listROI;
            //    IsSetupOK = true;
            //}
            //else
            //{
            //    IsSetupOK = false;
            //}
            FindLineFormNew findLineForm = new FindLineFormNew(this);
            if (findLineForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                ModelImage = findLineForm.MyFindLine.ModelImage;
                CaliperCount = findLineForm.MyFindLine.CaliperCount;
                CaliperWidth = findLineForm.MyFindLine.CaliperWidth;
                Sigma = findLineForm.MyFindLine.Sigma;
                Threshold = findLineForm.MyFindLine.Threshold;
                FindEdge = findLineForm.MyFindLine.FindEdge;
                FindSelect = findLineForm.MyFindLine.FindSelect;
                ROIList = findLineForm.MyFindLine.ROIList;
                IsSetupOK = true;
            }
            else
            {
                IsSetupOK = false;
            }
        }

        public override string ToolDescriptText()
        {
            return "查找直线";
        }

        public override string ToolName()
        {
            return "查找直线";
        }

        public override string ToolType()
        {
            return "定位工具";
        }
    }
}
