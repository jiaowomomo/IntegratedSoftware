using HalconDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomationSystem.Halcon
{
    [Serializable]
    public abstract class ROIBase
    {
        [NonSerialized]
        public HDrawingObject m_DrawingObject;
        public ROIStatus m_Status = ROIStatus.UNION;

        //public ROIBase(HDrawingObject hDrawingObject,ROIStatus status)
        //{
        //    m_DrawingObject = hDrawingObject;
        //    m_Status = status;
        //}

        public HRegion GetRegion()
        {
            return new HRegion(m_DrawingObject.GetDrawingObjectIconic());
        }

        public abstract void CreateDrawingObject();
        public abstract void GenerateParameter();
    }
}
