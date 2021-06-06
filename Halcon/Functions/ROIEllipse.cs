using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Halcon.Functions
{
    [Serializable]
    public class ROIEllipse : ROIBase
    {
        public double Row { get; set; } = 0;
        public double Column { get; set; } = 0;
        public double Phi { get; set; } = 0;
        public double Radius1 { get; set; } = 0;
        public double Radius2 { get; set; } = 0;

        public override void CreateDrawingObject()
        {
            DrawingObject = new HalconDotNet.HDrawingObject();
            DrawingObject.CreateDrawingObjectEllipse(Row, Column, Phi, Radius1, Radius2);
        }

        public override void GenerateParameter()
        {
            Row = DrawingObject.GetDrawingObjectParams("row");
            Column = DrawingObject.GetDrawingObjectParams("column");
            Phi = DrawingObject.GetDrawingObjectParams("phi");
            Radius1 = DrawingObject.GetDrawingObjectParams("radius1");
            Radius2 = DrawingObject.GetDrawingObjectParams("radius2");
        }
    }
}
