using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Halcon.Functions
{
    [Serializable]
    public class ROIRotateRectangle : ROIBase
    {
        public double Row { get; set; } = 0;
        public double Column { get; set; } = 0;
        public double Phi { get; set; } = 0;
        public double Length1 { get; set; } = 0;
        public double Length2 { get; set; } = 0;

        public override void CreateDrawingObject()
        {
            DrawingObject = new HalconDotNet.HDrawingObject();
            DrawingObject.CreateDrawingObjectRectangle2(Row, Column, Phi, Length1, Length2);
        }

        public override void GenerateParameter()
        {
            Row = DrawingObject.GetDrawingObjectParams("row");
            Column = DrawingObject.GetDrawingObjectParams("column");
            Phi = DrawingObject.GetDrawingObjectParams("phi");
            Length1 = DrawingObject.GetDrawingObjectParams("length1");
            Length2 = DrawingObject.GetDrawingObjectParams("length2");
        }
    }
}
