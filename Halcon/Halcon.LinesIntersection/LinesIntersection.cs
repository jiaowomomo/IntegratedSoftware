using Halcon.Functions;
using HalconDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Halcon.LinesIntersection
{
    [Serializable]
    public class LinesIntersection : IImageHalconObject
    {
        public LinesIntersection()
        {
            GetDataManager.AddInputDoubleArray("直线1起点X");
            GetDataManager.AddInputDoubleArray("直线1起点Y");
            GetDataManager.AddInputDoubleArray("直线1终点X");
            GetDataManager.AddInputDoubleArray("直线1终点Y");
            GetDataManager.AddInputDoubleArray("直线2起点X");
            GetDataManager.AddInputDoubleArray("直线2起点Y");
            GetDataManager.AddInputDoubleArray("直线2终点X");
            GetDataManager.AddInputDoubleArray("直线2终点Y");

            GetDataManager.AddOutputDoubleArray("交点X");
            GetDataManager.AddOutputDoubleArray("交点Y");
        }

        public override void EditParameters()
        {
            IsSetupOK = true;
        }

        public override void Execute(ref HImage source, ref List<ShowObject> showObjects, ref List<ShowText> showTexts)
        {
            List<double> line1StartX = GetDataManager.GetInputDoubleArray("直线1起点X");
            List<double> line1StartY = GetDataManager.GetInputDoubleArray("直线1起点Y");
            List<double> line1EndX = GetDataManager.GetInputDoubleArray("直线1终点X");
            List<double> line1EndY = GetDataManager.GetInputDoubleArray("直线1终点Y");
            List<double> line2StartX = GetDataManager.GetInputDoubleArray("直线2起点X");
            List<double> line2StartY = GetDataManager.GetInputDoubleArray("直线2起点Y");
            List<double> line2EndX = GetDataManager.GetInputDoubleArray("直线2终点X");
            List<double> line2EndY = GetDataManager.GetInputDoubleArray("直线2终点Y");
            List<double> pointsX = new List<double>();
            List<double> pointsY = new List<double>();
            if (line1StartX.Count == line2StartX.Count)
            {
                HTuple row, column, isOverlapping;
                for (int i = 0; i < line1StartX.Count; i++)
                {
                    HOperatorSet.IntersectionLines(line1StartY[i], line1StartX[i], line1EndY[i], line1EndX[i], line2StartY[i], line2StartX[i], line2EndY[i], line2EndX[i], out row, out column, out isOverlapping);
                    pointsX.Add(column.D);
                    pointsY.Add(row.D);
                    HRegion hRegion = new HRegion();
                    hRegion.GenCircle(row.D, column.D, 5);
                    showObjects.Add(new ShowObject(hRegion, "red", "fill"));
                }
                GetDataManager.SetOutputDoubleArray("交点X", pointsX);
                GetDataManager.SetOutputDoubleArray("交点Y", pointsY);
            }
            else
            {
                throw new RunException(RunExceptionType.TwoLineNumberNotEqual);
            }
        }

        public override void SetParameters()
        {
            IsSetupOK = true;
        }

        public override string ToolDescriptText()
        {
            return "两直线交点";
        }

        public override string ToolName()
        {
            return "两直线交点";
        }

        public override string ToolType()
        {
            return "数学工具";
        }
    }
}
