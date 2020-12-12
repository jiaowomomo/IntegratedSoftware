using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomationSystem.Halcon
{
    [Serializable]
    public class ROIRotateRectangle : ROIBase
    {
        public double row = 0;
        public double column = 0;
        public double phi = 0;
        public double length1 = 0;
        public double length2 = 0;

        public override void CreateDrawingObject()
        {
            m_DrawingObject = new HalconDotNet.HDrawingObject();
            m_DrawingObject.CreateDrawingObjectRectangle2(row, column, phi, length1, length2);
        }

        public override void GenerateParameter()
        {
            row = m_DrawingObject.GetDrawingObjectParams("row");
            column = m_DrawingObject.GetDrawingObjectParams("column");
            phi = m_DrawingObject.GetDrawingObjectParams("phi");
            length1 = m_DrawingObject.GetDrawingObjectParams("length1");
            length2 = m_DrawingObject.GetDrawingObjectParams("length2");
        }
    }
}
