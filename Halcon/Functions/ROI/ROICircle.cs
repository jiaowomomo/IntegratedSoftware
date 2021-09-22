using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Halcon.Functions
{
    [Serializable]
    public class ROICircle : ROIBase
    {
        public double Row { get; set; } = 0;
        public double Column { get; set; } = 0;
        public double Radius { get; set; } = 0;

        public override void CreateDrawingObject()
        {
            DrawingObject = new HalconDotNet.HDrawingObject();
            DrawingObject.CreateDrawingObjectCircle(Row, Column, Radius);
        }

        public override void GenerateParameter()
        {
            Row = DrawingObject.GetDrawingObjectParams("row");
            Column = DrawingObject.GetDrawingObjectParams("column");
            Radius = DrawingObject.GetDrawingObjectParams("radius");
        }
    }
}
