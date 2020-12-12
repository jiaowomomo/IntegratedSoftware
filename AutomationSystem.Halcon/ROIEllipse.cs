using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomationSystem.Halcon
{
    [Serializable]
    public class ROIEllipse : ROIBase
    {
        public double row = 0;
        public double column = 0;
        public double phi = 0;
        public double radius1 = 0;
        public double radius2 = 0;

        public override void CreateDrawingObject()
        {
            m_DrawingObject = new HalconDotNet.HDrawingObject();
            m_DrawingObject.CreateDrawingObjectEllipse(row, column, phi, radius1, radius2);
        }

        public override void GenerateParameter()
        {
            row = m_DrawingObject.GetDrawingObjectParams("row");
            column = m_DrawingObject.GetDrawingObjectParams("column");
            phi = m_DrawingObject.GetDrawingObjectParams("phi");
            radius1 = m_DrawingObject.GetDrawingObjectParams("radius1");
            radius2 = m_DrawingObject.GetDrawingObjectParams("radius2");
        }
    }
}
