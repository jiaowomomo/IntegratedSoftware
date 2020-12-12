using AutomationSystem.Halcon;
using HalconDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automation.HysteresisThreshold
{
    [Serializable]
    public class HysteresisThreshold : IImageHalconObject
    {
        public int nThresholdMin = 0;
        public int nThresholdMax = 0;
        public int nLength = 1;
        public int nOKValue = 0;
        public int nNGValue = 0;
        public string ModelImage = "";

        public override void EditParameters()
        {
            HysteresisThresholdForm hysteresisThresholdForm = new HysteresisThresholdForm(this);
            hysteresisThresholdForm.ShowDialog();
            if (hysteresisThresholdForm.Result == System.Windows.Forms.DialogResult.OK)
            {
                nThresholdMin = hysteresisThresholdForm.nThresholdMin;
                nThresholdMax = hysteresisThresholdForm.nThresholdMax;
                nLength = hysteresisThresholdForm.nLength;
                nOKValue = hysteresisThresholdForm.nOKValue;
                nNGValue = hysteresisThresholdForm.nNGValue;
                ModelImage = hysteresisThresholdForm.ModelImage;
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
            threshold = source.HysteresisThreshold(nThresholdMin, nThresholdMax, nLength);
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
            HysteresisThresholdForm hysteresisThresholdForm = new HysteresisThresholdForm();
            hysteresisThresholdForm.ShowDialog();
            if (hysteresisThresholdForm.Result == System.Windows.Forms.DialogResult.OK)
            {
                nThresholdMin = hysteresisThresholdForm.nThresholdMin;
                nThresholdMax = hysteresisThresholdForm.nThresholdMax;
                nLength = hysteresisThresholdForm.nLength;
                nOKValue = hysteresisThresholdForm.nOKValue;
                nNGValue = hysteresisThresholdForm.nNGValue;
                ModelImage = hysteresisThresholdForm.ModelImage;
                IsSetupOK = true;
            }
            else
            {
                IsSetupOK = false;
            }
        }

        public override string ToolDescriptText()
        {
            return "滞后性阈值";
        }

        public override string ToolName()
        {
            return "滞后性阈值";
        }

        public override string ToolType()
        {
            return "预处理";
        }
    }
}
