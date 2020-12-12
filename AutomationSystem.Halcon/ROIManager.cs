using HalconDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomationSystem.Halcon
{
    public class ROIManager
    {
        private int MaxCount = 1;
        private List<ROIBase> m_listROIs = new List<ROIBase>();

        public void SetMaxCount(int count)
        {
            MaxCount = count;
        }

        public void ResetROIs()
        {
            for (int i = 0; i < m_listROIs.Count; i++)
            {
                m_listROIs[i].m_DrawingObject.Dispose();
            }
            m_listROIs = new List<ROIBase>();
        }

        public void SetROIs(List<ROIBase> rois)
        {
            for (int i = 0; i < m_listROIs.Count; i++)
            {
                m_listROIs[i].m_DrawingObject.Dispose();
            }
            m_listROIs = rois;
        }

        public List<ROIBase> GetROIs()
        {
            return m_listROIs;
        }

        public bool AddROI(ROIBase roi)
        {
            if (m_listROIs.Count < MaxCount)
            {
                m_listROIs.Add(roi);
                return true;
            }
            return false;
        }

        public void DeleteROI(int index)
        {
            for (int i = 0; i < m_listROIs.Count; i++)
            {
                if (m_listROIs[i].m_DrawingObject.ID == index)
                {
                    m_listROIs[i].m_DrawingObject.Dispose();
                    m_listROIs.RemoveAt(i);
                    return;
                }
            }
        }

        public HRegion GetRegions()
        {
            HRegion hRegion = null;
            HRegion temp = new HRegion();
            if (m_listROIs.Count != 0)
            {
                for (int i = 0; i < m_listROIs.Count; i++)
                {
                    if ((m_listROIs[i].m_DrawingObject == null) || (m_listROIs[i].m_DrawingObject.ID == -1))
                    {
                        m_listROIs[i].CreateDrawingObject();
                    }
                }
                hRegion = new HRegion(m_listROIs[0].m_DrawingObject.GetDrawingObjectIconic());
                for (int i = 1; i < m_listROIs.Count; i++)
                {
                    if (m_listROIs[i].m_Status == ROIStatus.UNION)
                    {
                        temp = new HRegion(m_listROIs[i].m_DrawingObject.GetDrawingObjectIconic());
                        hRegion = hRegion.Union2(temp);
                    }
                    else if (m_listROIs[i].m_Status == ROIStatus.INTERSECTION)
                    {
                        temp = new HRegion(m_listROIs[i].m_DrawingObject.GetDrawingObjectIconic());
                        hRegion = hRegion.Intersection(temp);
                    }
                    else
                    {
                        temp = new HRegion(m_listROIs[i].m_DrawingObject.GetDrawingObjectIconic());
                        hRegion = hRegion.Difference(temp);
                    }
                }
            }
            temp.Dispose();
            return hRegion;
        }
    }
}
