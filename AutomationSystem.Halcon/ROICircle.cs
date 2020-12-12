using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomationSystem.Halcon
{
    [Serializable]
    public class ROICircle : ROIBase
    {
        public double row = 0;
        public double column = 0;
        public double radius = 0;

        public override void CreateDrawingObject()
        {
            m_DrawingObject = new HalconDotNet.HDrawingObject();
            m_DrawingObject.CreateDrawingObjectCircle(row, column, radius);
        }

        public override void GenerateParameter()
        {
            row = m_DrawingObject.GetDrawingObjectParams("row");
            column = m_DrawingObject.GetDrawingObjectParams("column");
            radius = m_DrawingObject.GetDrawingObjectParams("radius");
        }
    }
}
