using Halcon.Functions;
using HalconDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Halcon.NoROI
{
    [Serializable]
    public class NoROI : IImageHalconObject
    {
        public string ModelImage = "";

        public override void EditParameters()
        {
            NoROIForm rOIForm = new NoROIForm(this);
            rOIForm.ShowDialog();
            if (rOIForm.Result == System.Windows.Forms.DialogResult.OK)
            {
                ROIList = rOIForm.m_listROI;
                ModelImage = rOIForm.ModelImage;
                IsSetupOK = true;
            }
            else
            {
                IsSetupOK = false;
            }
        }

        public override void Execute(ref HImage source, ref List<ShowObject> showObjects, ref List<ShowText> showTexts)
        {
            ROIManager rm = new ROIManager();
            rm.SetROIs(ROIList);
            HRegion hRegion = rm.GetRegions();
            if (hRegion != null)
            {
                if (source == null || source.Key == IntPtr.Zero)
                {
                    throw new RunException(RunExceptionType.NoInputImage);
                }
                HRegion hRegionAll = source.GetDomain();
                hRegionAll = hRegionAll.Difference(hRegion);
                source = source.ReduceDomain(hRegionAll);
                hRegion.Dispose();
                hRegionAll.Dispose();
            }
        }

        public override void SetParameters()
        {
            NoROIForm rOIForm = new NoROIForm();
            rOIForm.ShowDialog();
            if (rOIForm.Result == System.Windows.Forms.DialogResult.OK)
            {
                ROIList = rOIForm.m_listROI;
                ModelImage = rOIForm.ModelImage;
                IsSetupOK = true;
            }
            else
            {
                IsSetupOK = false;
            }
        }

        public override string ToolDescriptText()
        {
            return "非ROI设置";
        }

        public override string ToolName()
        {
            return "非ROI设置";
        }

        public override string ToolType()
        {
            return "预处理";
        }
    }
}
