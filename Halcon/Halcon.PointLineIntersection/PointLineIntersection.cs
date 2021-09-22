using Halcon.Functions;
using HalconDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Halcon.PointLineIntersection
{
    [Serializable]
    public class PointLineIntersection : IImageHalconObject
    {
        public PointLineIntersection()
        {
            GetDataManager.AddInputDoubleArray("直线起点X");
            GetDataManager.AddInputDoubleArray("直线起点Y");
            GetDataManager.AddInputDoubleArray("直线终点X");
            GetDataManager.AddInputDoubleArray("直线终点Y");
            GetDataManager.AddInputDoubleArray("起点X");
            GetDataManager.AddInputDoubleArray("起点Y");

            GetDataManager.AddOutputDoubleArray("交点X");
            GetDataManager.AddOutputDoubleArray("交点Y");
        }

        public override void EditParameters()
        {
            IsSetupOK = true;
        }

        public override void Execute(ref HImage source, ref List<ShowObject> showObjects, ref List<ShowText> showTexts)
        {
            List<double> line1StartX = GetDataManager.GetInputDoubleArray("直线起点X");
            List<double> line1StartY = GetDataManager.GetInputDoubleArray("直线起点Y");
            List<double> line1EndX = GetDataManager.GetInputDoubleArray("直线终点X");
            List<double> line1EndY = GetDataManager.GetInputDoubleArray("直线终点Y");
            List<double> pointStartX = GetDataManager.GetInputDoubleArray("起点X");
            List<double> pointStartY = GetDataManager.GetInputDoubleArray("起点Y");
            List<double> pointsX = new List<double>();
            List<double> pointsY = new List<double>();
            if (line1StartX.Count == pointStartX.Count)
            {
                for (int i = 0; i < line1StartX.Count; i++)
                {
                    double dx = line1StartX[i] - line1EndX[i];
                    double dy = line1StartY[i] - line1EndY[i];
                    double u = (pointStartX[i] - line1StartX[i]) * (line1StartX[i] - line1EndX[i]) + (pointStartY[i] - line1StartY[i]) * (line1StartY[i] - line1EndY[i]);
                    u = u / (dx * dx + dy * dy);
                    pointsX.Add(line1StartX[i] + u * dx);
                    pointsY.Add(line1StartY[i] + u * dy);
                    HRegion hRegion = new HRegion();
                    hRegion.GenCircle(line1StartY[i] + u * dy, line1StartX[i] + u * dx, 5);
                    showObjects.Add(new ShowObject(hRegion, "red", "fill"));
                }
                GetDataManager.SetOutputDoubleArray("交点X", pointsX);
                GetDataManager.SetOutputDoubleArray("交点Y", pointsY);
            }
            else
            {
                throw new RunException(RunExceptionType.PointAndLineNumberNotEqual);
            }
        }

        public override void SetParameters()
        {
            IsSetupOK = true;
        }

        public override string ToolDescriptText()
        {
            return "点到直线交点";
        }

        public override string ToolName()
        {
            return "点到直线交点";
        }

        public override string ToolType()
        {
            return "数学工具";
        }
    }
}
