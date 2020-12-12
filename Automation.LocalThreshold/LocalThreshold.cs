using AutomationSystem.Halcon;
using HalconDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automation.LocalThreshold
{
    [Serializable]
    public class LocalThreshold : IImageHalconObject
    {
        public string strMethod = "adapted_std_deviation";
        public string strRegion = "dark";
        public int nOKValue = 0;
        public int nNGValue = 0;
        public string ModelImage = "";

        public override void EditParameters()
        {
            LocalThresholdForm localThresholdForm = new LocalThresholdForm(this);
            localThresholdForm.ShowDialog();
            if (localThresholdForm.Result == System.Windows.Forms.DialogResult.OK)
            {
                strMethod = localThresholdForm.strMethod;
                strRegion = localThresholdForm.strRegion;
                nOKValue = localThresholdForm.nOKValue;
                nNGValue = localThresholdForm.nNGValue;
                ModelImage = localThresholdForm.ModelImage;
                IsSetupOK = true;
            }
            else
            {
                IsSetupOK = false;
            }
        }

        public override void Execute(ref HImage source, ref List<ShowObject> showObjects, ref List<ShowText> showTexts)
        {
            HRegion threshold;
            HImage binary;
            if (source == null || source.Key == IntPtr.Zero)
            {
                throw new RunException(1);
            }
            threshold = source.LocalThreshold(strMethod, strRegion, new HTuple(), new HTuple());
            HTuple width, height;
            source.GetImageSize(out width, out height);
            binary = threshold.RegionToBin(nOKValue, nNGValue, width, height);
            source.Dispose();
            source = binary.Clone();
            threshold.Dispose();
            binary.Dispose();
        }

        public override void SetParameters()
        {
            LocalThresholdForm localThresholdForm = new LocalThresholdForm();
            localThresholdForm.ShowDialog();
            if (localThresholdForm.Result == System.Windows.Forms.DialogResult.OK)
            {
                strMethod = localThresholdForm.strMethod;
                strRegion = localThresholdForm.strRegion;
                nOKValue = localThresholdForm.nOKValue;
                nNGValue = localThresholdForm.nNGValue;
                ModelImage = localThresholdForm.ModelImage;
                IsSetupOK = true;
            }
            else
            {
                IsSetupOK = false;
            }
        }

        public override string ToolDescriptText()
        {
            return "局部阈值";
        }

        public override string ToolName()
        {
            return "局部阈值";
        }

        public override string ToolType()
        {
            return "预处理";
        }
    }
}
