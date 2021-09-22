using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Halcon.Functions
{
    [Serializable]
    public class ROIRectangle : ROIBase
    {
        public double Row1 { get; set; } = 0;
        public double Column1 { get; set; } = 0;
        public double Row2 { get; set; } = 0;
        public double Column2 { get; set; } = 0;

        public override void CreateDrawingObject()
        {
            DrawingObject = new HalconDotNet.HDrawingObject();
            DrawingObject.CreateDrawingObjectRectangle1(Row1, Column1, Row2, Column2);
        }

        public override void GenerateParameter()
        {
            Row1 = DrawingObject.GetDrawingObjectParams("row1");
            Column1 = DrawingObject.GetDrawingObjectParams("column1");
            Row2 = DrawingObject.GetDrawingObjectParams("row2");
            Column2 = DrawingObject.GetDrawingObjectParams("column2");
        }
    }
}
