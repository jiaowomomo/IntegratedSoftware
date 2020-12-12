using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomationSystem.Halcon
{
    [Serializable]
    public class ROIRectangle : ROIBase
    {
        public double row1 = 0;
        public double column1 = 0;
        public double row2 = 0;
        public double column2 = 0;

        public override void CreateDrawingObject()
        {
            m_DrawingObject = new HalconDotNet.HDrawingObject();
            m_DrawingObject.CreateDrawingObjectRectangle1(row1, column1, row2, column2);
        }

        public override void GenerateParameter()
        {
            row1 = m_DrawingObject.GetDrawingObjectParams("row1");
            column1 = m_DrawingObject.GetDrawingObjectParams("column1");
            row2 = m_DrawingObject.GetDrawingObjectParams("row2");
            column2 = m_DrawingObject.GetDrawingObjectParams("column2");
        }
    }
}
