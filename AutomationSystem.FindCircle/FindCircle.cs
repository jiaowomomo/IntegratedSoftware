using AutomationSystem.Halcon;
using HalconDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomationSystem.FindCircle
{
    [Serializable]
    public class FindCircle : IImageHalconObject
    {
        public string ModelImage = "";
        public int CaliperCount = 0;
        public double CaliperWidth = 0;
        public double Sigma = 1;
        public double Threshold = 30;
        public string FindEdge = "all";
        public string FindSelect = "all";

        public FindCircle()
        {
            GetDataManager.AddInputDouble("轮廓中心X");
            GetDataManager.AddInputDouble("轮廓中心Y");
            GetDataManager.AddInputDoubleVector("匹配位置X");
            GetDataManager.AddInputDoubleVector("匹配位置Y");
            GetDataManager.AddInputDoubleVector("匹配角度");
            GetDataManager.AddOutputInt("圆个数");
            GetDataManager.AddOutputDoubleVector("圆心X");
            GetDataManager.AddOutputDoubleVector("圆心Y");
        }

        public override void EditParameters()
        {
            FindCircleForm findCircleForm = new FindCircleForm(this);
            findCircleForm.ShowDialog();
            if (findCircleForm.Result == System.Windows.Forms.DialogResult.OK)
            {
                ModelImage = findCircleForm.ModelImage;
                CaliperCount = findCircleForm.CaliperCount;
                CaliperWidth = findCircleForm.CaliperWidth;
                Sigma = findCircleForm.Sigma;
                Threshold = findCircleForm.Threshold;
                FindEdge = findCircleForm.FindEdge;
                FindSelect = findCircleForm.FindSelect;
                m_listROI = findCircleForm.m_listROI;
                IsSetupOK = true;
            }
            else
            {
                IsSetupOK = false;
            }
        }

        public override void Execute(ref HImage source, ref List<ShowObject> showObjects, ref List<ShowText> showTexts)
        {
            if (m_listROI.Count != 0)
            {
                ROICircle rOICircle = m_listROI[0] as ROICircle;
                double centerX = 0;
                double centerY = 0;
                HTuple width, height;
                if (source == null || source.Key == IntPtr.Zero)
                {
                    throw new RunException(1);
                }
                source.GetImageSize(out width, out height);
                double ModelX = GetDataManager.GetInputDouble("轮廓中心X");
                double ModelY = GetDataManager.GetInputDouble("轮廓中心Y");
                List<double> posX = GetDataManager.GetInputDoubleVector("匹配位置X");
                List<double> posY = GetDataManager.GetInputDoubleVector("匹配位置Y");
                List<double> angle = GetDataManager.GetInputDoubleVector("匹配角度");
                int count = 0;
                List<double> circleX = new List<double>();
                List<double> circleY = new List<double>();
                for (int j = 0; j < angle.Count; j++)
                {
                    HHomMat2D hHomMat2D = new HHomMat2D();
                    hHomMat2D.VectorAngleToRigid(ModelY, ModelX, 0, posY[j], posX[j], angle[j]);
                    HTuple rowTrans, colTrans;
                    hHomMat2D.AffineTransPixel(rOICircle.row, rOICircle.column, out rowTrans, out colTrans);
                    HTuple rows = new HTuple();
                    HTuple colums = new HTuple();
                    for (int i = 0; i < CaliperCount; i++)
                    {
                        centerX = colTrans + rOICircle.radius * Math.Cos(i * Math.PI * 2 / CaliperCount);//求中心点
                        centerY = rowTrans + rOICircle.radius * Math.Sin(i * Math.PI * 2 / CaliperCount);
                        HMeasure hMeasure = new HMeasure();
                        hMeasure.GenMeasureRectangle2(centerY, centerX, -i * Math.PI * 2 / CaliperCount, rOICircle.radius, CaliperWidth, width, height, "nearest_neighbor");
                        HTuple rowEdge, columnEdge, amplitude, distance;
                        hMeasure.MeasurePos(source, Sigma, Threshold, FindEdge, FindSelect, out rowEdge, out columnEdge, out amplitude, out distance);
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
                GetDataManager.SetOutputDoubleVector("圆心X", circleX);
                GetDataManager.SetOutputDoubleVector("圆心Y", circleY);
            }
        }

        public override void SetParameters()
        {
            FindCircleForm findCircleForm = new FindCircleForm();
            findCircleForm.ShowDialog();
            if (findCircleForm.Result == System.Windows.Forms.DialogResult.OK)
            {
                ModelImage = findCircleForm.ModelImage;
                CaliperCount = findCircleForm.CaliperCount;
                CaliperWidth = findCircleForm.CaliperWidth;
                Sigma = findCircleForm.Sigma;
                Threshold = findCircleForm.Threshold;
                FindEdge = findCircleForm.FindEdge;
                FindSelect = findCircleForm.FindSelect;
                m_listROI = findCircleForm.m_listROI;
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
