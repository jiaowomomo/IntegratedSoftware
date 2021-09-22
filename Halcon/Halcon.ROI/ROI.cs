using Halcon.Functions;
using HalconDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Halcon.ROI
{
    [Serializable]
    public class ROI : IImageHalconObject
    {
        public string ModelImage = "";

        public override void EditParameters()
        {
            ROIForm rOIForm = new ROIForm(this);
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
                source = source.ReduceDomain(hRegion);
                hRegion.Dispose();
            }
        }

        public override void SetParameters()
        {
            ROIForm rOIForm = new ROIForm();
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
            return "ROI设置";
        }

        public override string ToolName()
        {
            return "ROI设置";
        }

        public override string ToolType()
        {
            return "预处理";
        }
    }
}
