using Halcon.Functions;
using HalconDotNet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Halcon.FindCircle
{
    [Serializable]
    public class FindCircle : IImageHalconObject
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

        public FindCircle()
        {
            GetDataManager.AddInputDouble("轮廓中心X");
            GetDataManager.AddInputDouble("轮廓中心Y");
            GetDataManager.AddInputDoubleArray("匹配位置X");
            GetDataManager.AddInputDoubleArray("匹配位置Y");
            GetDataManager.AddInputDoubleArray("匹配角度");
            GetDataManager.AddOutputInt("圆个数");
            GetDataManager.AddOutputDoubleArray("圆心X");
            GetDataManager.AddOutputDoubleArray("圆心Y");
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
            //FindCircleForm findCircleForm = new FindCircleForm(this);
            //findCircleForm.ShowDialog();
            //if (findCircleForm.Result == System.Windows.Forms.DialogResult.OK)
            //{
            //    ModelImage = findCircleForm.ModelImage;
            //    CaliperCount = findCircleForm.CaliperCount;
            //    CaliperWidth = findCircleForm.CaliperWidth;
            //    Sigma = findCircleForm.Sigma;
            //    Threshold = findCircleForm.Threshold;
            //    FindEdge = findCircleForm.FindEdge;
            //    FindSelect = findCircleForm.FindSelect;
            //    m_listROI = findCircleForm.m_listROI;
            //    IsSetupOK = true;
            //}
            //else
            //{
            //    IsSetupOK = false;
            //}
            FindCircleFormNew findCircleForm = new FindCircleFormNew(this);
            if (findCircleForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                ModelImage = findCircleForm.MyFindCircle.ModelImage;
                CaliperCount = findCircleForm.MyFindCircle.CaliperCount;
                CaliperWidth = findCircleForm.MyFindCircle.CaliperWidth;
                Sigma = findCircleForm.MyFindCircle.Sigma;
                Threshold = findCircleForm.MyFindCircle.Threshold;
                FindEdge = findCircleForm.MyFindCircle.FindEdge;
                FindSelect = findCircleForm.MyFindCircle.FindSelect;
                ROIList = findCircleForm.MyFindCircle.ROIList;
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
                ROICircle rOICircle = ROIList[0] as ROICircle;
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
                List<double> circleX = new List<double>();
                List<double> circleY = new List<double>();
                for (int j = 0; j < angle.Count; j++)
                {
                    HHomMat2D hHomMat2D = new HHomMat2D();
                    hHomMat2D.VectorAngleToRigid(ModelY, ModelX, 0, posY[j], posX[j], angle[j]);
                    HTuple rowTrans, colTrans;
                    hHomMat2D.AffineTransPixel(rOICircle.Row, rOICircle.Column, out rowTrans, out colTrans);
                    HTuple rows = new HTuple();
                    HTuple colums = new HTuple();
                    for (int i = 0; i < CaliperCount; i++)
                    {
                        centerX = colTrans + rOICircle.Radius * Math.Cos(i * Math.PI * 2 / CaliperCount);//求中心点
                        centerY = rowTrans + rOICircle.Radius * Math.Sin(i * Math.PI * 2 / CaliperCount);
                        HMeasure hMeasure = new HMeasure();
                        hMeasure.GenMeasureRectangle2(centerY, centerX, -i * Math.PI * 2 / CaliperCount, rOICircle.Radius, CaliperWidth, width, height, "nearest_neighbor");
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
                    HTuple row, column, radius, startPhi, endPhi, pointOrder;
                    hXLDCont.FitCircleContourXld("algebraic", -1, 0, 0, 3, 2, out row, out column, out radius, out startPhi, out endPhi, out pointOrder);
                    hXLDCont.Dispose();
                    HXLDCont fitCircle = new HXLDCont();
                    fitCircle.GenCircleContourXld(row, column, radius, new HTuple(0.0), new HTuple(6.28318), new HTuple("positive"), 1.0);
                    showObjects.Add(new ShowObject(fitCircle, "blue"));
                    count++;
                    circleX.Add(column.D);
                    circleY.Add(row.D);
                }
                GetDataManager.SetOutputInt("圆个数", count);
                GetDataManager.SetOutputDoubleArray("圆心X", circleX);
                GetDataManager.SetOutputDoubleArray("圆心Y", circleY);
            }
        }

        public override void SetParameters()
        {
            //FindCircleForm findCircleForm = new FindCircleForm();
            //findCircleForm.ShowDialog();
            //if (findCircleForm.Result == System.Windows.Forms.DialogResult.OK)
            //{
            //    ModelImage = findCircleForm.ModelImage;
            //    CaliperCount = findCircleForm.CaliperCount;
            //    CaliperWidth = findCircleForm.CaliperWidth;
            //    Sigma = findCircleForm.Sigma;
            //    Threshold = findCircleForm.Threshold;
            //    FindEdge = findCircleForm.FindEdge;
            //    FindSelect = findCircleForm.FindSelect;
            //    m_listROI = findCircleForm.m_listROI;
            //    IsSetupOK = true;
            //}
            //else
            //{
            //    IsSetupOK = false;
            //}
            FindCircleFormNew findCircleForm = new FindCircleFormNew(this);
            if (findCircleForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                ModelImage = findCircleForm.MyFindCircle.ModelImage;
                CaliperCount = findCircleForm.MyFindCircle.CaliperCount;
                CaliperWidth = findCircleForm.MyFindCircle.CaliperWidth;
                Sigma = findCircleForm.MyFindCircle.Sigma;
                Threshold = findCircleForm.MyFindCircle.Threshold;
                FindEdge = findCircleForm.MyFindCircle.FindEdge;
                FindSelect = findCircleForm.MyFindCircle.FindSelect;
                ROIList = findCircleForm.MyFindCircle.ROIList;
                IsSetupOK = true;
            }
            else
            {
                IsSetupOK = false;
            }
        }

        public override string ToolDescriptText()
        {
            return "查找圆";
        }

        public override string ToolName()
        {
            return "查找圆";
        }

        public override string ToolType()
        {
            return "定位工具";
        }
    }
}
