using HalconDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Halcon.Functions
{
    [Serializable]
    public abstract class ROIBase
    {
        private ROIStatus m_roiStatus = ROIStatus.UNION;

        [NonSerialized]
        private HDrawingObject m_drawingObject = null;

        public HDrawingObject DrawingObject { get => m_drawingObject; set => m_drawingObject = value; }
        public ROIStatus Status { get => m_roiStatus; set => m_roiStatus = value; }

        public HRegion GetRegion()
        {
            return new HRegion(DrawingObject.GetDrawingObjectIconic());
        }

        public abstract void CreateDrawingObject();
        public abstract void GenerateParameter();
    }
}
