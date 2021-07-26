using HalconDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Halcon.Functions
{
    public class ROIManager
    {
        private int m_maxCount = 1;
        private List<ROIBase> m_listROIs = new List<ROIBase>();

        public int ROICount { get => m_listROIs.Count; }

        public void SetMaxCount(int count)
        {
            m_maxCount = count;
        }

        public void ResetROIs()
        {
            for (int i = 0; i < m_listROIs.Count; i++)
            {
                m_listROIs[i].DrawingObject.Dispose();
            }
            m_listROIs = new List<ROIBase>();
        }

        public void SetROIs(List<ROIBase> rois)
        {
            for (int i = 0; i < m_listROIs.Count; i++)
            {
                m_listROIs[i].DrawingObject.Dispose();
            }
            m_listROIs = rois;
        }

        public List<ROIBase> GetROIs()
        {
            return m_listROIs;
        }

        public ROIBase GetROIByIndex(int index)
        {
            if (index >= 0 && index < m_listROIs.Count)
            {
                return m_listROIs[index];
            }
            return null;
        }

        public bool AddROI(ROIBase roi)
        {
            if (m_listROIs.Count < m_maxCount)
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
                if (m_listROIs[i].DrawingObject.ID == index)
                {
                    m_listROIs[i].DrawingObject.Dispose();
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
                    if ((m_listROIs[i].DrawingObject == null) || (m_listROIs[i].DrawingObject.ID == -1))
                    {
                        m_listROIs[i].CreateDrawingObject();
                    }
                }
                hRegion = new HRegion(m_listROIs[0].DrawingObject.GetDrawingObjectIconic());
                for (int i = 1; i < m_listROIs.Count; i++)
                {
                    if (m_listROIs[i].Status == ROIStatus.UNION)
                    {
                        temp = new HRegion(m_listROIs[i].DrawingObject.GetDrawingObjectIconic());
                        hRegion = hRegion.Union2(temp);
                    }
                    else if (m_listROIs[i].Status == ROIStatus.INTERSECTION)
                    {
                        temp = new HRegion(m_listROIs[i].DrawingObject.GetDrawingObjectIconic());
                        hRegion = hRegion.Intersection(temp);
                    }
                    else
                    {
                        temp = new HRegion(m_listROIs[i].DrawingObject.GetDrawingObjectIconic());
                        hRegion = hRegion.Difference(temp);
                    }
                }
            }
            temp.Dispose();
            return hRegion;
        }
    }
}
