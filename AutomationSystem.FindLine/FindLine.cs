using AutomationSystem.Halcon;
using HalconDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomationSystem.FindLine
{
    [Serializable]
    public class FindLine : IImageHalconObject
    {
        public string ModelImage = "";
        public int CaliperCount = 0;
        public double CaliperWidth = 0;
        public double Sigma = 1;
        public double Threshold = 30;
        public string FindEdge = "all";
        public string FindSelect = "all";

        public FindLine()
        {
            GetDataManager.AddInputDouble("轮廓中心X");
            GetDataManager.AddInputDouble("轮廓中心Y");
            GetDataManager.AddInputDoubleVector("匹配位置X");
            GetDataManager.AddInputDoubleVector("匹配位置Y");
            GetDataManager.AddInputDoubleVector("匹配角度");
            GetDataManager.AddOutputInt("直线数");
            GetDataManager.AddOutputDoubleVector("起点X");
            GetDataManager.AddOutputDoubleVector("起点Y");
            GetDataManager.AddOutputDoubleVector("终点X");
            GetDataManager.AddOutputDoubleVector("终点Y");
        }

        public override void EditParameters()
        {
            FindLineForm findLineForm = new FindLineForm(this);
            findLineForm.ShowDialog();
            if (findLineForm.Result == System.Windows.Forms.DialogResult.OK)
            {
                ModelImage = findLineForm.ModelImage;
                CaliperCount = findLineForm.CaliperCount;
                CaliperWidth = findLineForm.CaliperWidth;
                Sigma = findLineForm.Sigma;
                Threshold = findLineForm.Threshold;
                FindEdge = findLineForm.FindEdge;
                FindSelect = findLineForm.FindSelect;
                m_listROI = findLineForm.m_listROI;
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
                ROIRotateRectangle rotateRectangle = m_listROI[0] as ROIRotateRectangle;
                int rowMid = CaliperCount / 2;
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
                List<double> lineStartX = new List<double>();
                List<double> lineStartY = new List<double>();
                List<double> lineEndX = new List<double>();
                List<double> lineEndY = new List<double>();
                for (int j = 0; j < angle.Count; j++)
                {
                    HHomMat2D hHomMat2D = new HHomMat2D();
                    hHomMat2D.VectorAngleToRigid(ModelY, ModelX, 0, posY[j], posX[j], angle[j]);
                    HTuple rowTrans, colTrans;
                    hHomMat2D.AffineTransPixel(rotateRectangle.row, rotateRectangle.column, out rowTrans, out colTrans);
                    HTuple rows = new HTuple();
                    HTuple colums = new HTuple();
                    for (int i = 0; i < CaliperCount; i++)
                    {
                        centerX = colTrans + (i - rowMid) * 2 * CaliperWidth * Math.Sin(rotateRectangle.phi + angle[j]);//求中心点
                        centerY = rowTrans + (i - rowMid) * 2 * CaliperWidth * Math.Cos(rotateRectangle.phi + angle[j]);
                        HMeasure hMeasure = new HMeasure();
                        hMeasure.GenMeasureRectangle2(centerY, centerX, rotateRectangle.phi+angle[j], rotateRectangle.length1, CaliperWidth, width, height, "nearest_neighbor");
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
                GetDataManager.SetOutputDoubleVector("起点X", lineStartX);
                GetDataManager.SetOutputDoubleVector("起点Y", lineStartY);
                GetDataManager.SetOutputDoubleVector("终点X", lineEndX);
                GetDataManager.SetOutputDoubleVector("终点Y", lineEndY);
            }
        }

        public override void SetParameters()
        {
            FindLineForm findLineForm = new FindLineForm();
            findLineForm.ShowDialog();
            if (findLineForm.Result == System.Windows.Forms.DialogResult.OK)
            {
                ModelImage = findLineForm.ModelImage;
                CaliperCount = findLineForm.CaliperCount;
                CaliperWidth = findLineForm.CaliperWidth;
                Sigma = findLineForm.Sigma;
                Threshold = findLineForm.Threshold;
                FindEdge = findLineForm.FindEdge;
                FindSelect = findLineForm.FindSelect;
                m_listROI = findLineForm.m_listROI;
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
