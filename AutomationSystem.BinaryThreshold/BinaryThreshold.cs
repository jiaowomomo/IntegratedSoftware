using AutomationSystem.Halcon;
using HalconDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomationSystem.BinaryThreshold
{
    [Serializable]
    public class BinaryThreshold : IImageHalconObject
    {
        public string strMethod = "max_separability";
        public string strRegion = "dark";
        public int nOKValue = 0;
        public int nNGValue = 0;
        public string ModelImage = "";

        public override void EditParameters()
        {
            BinaryThresholdForm binaryThresholdForm = new BinaryThresholdForm(this);
            binaryThresholdForm.ShowDialog();
            if (binaryThresholdForm.Result == System.Windows.Forms.DialogResult.OK)
            {
                strMethod = binaryThresholdForm.strMethod;
                strRegion = binaryThresholdForm.strRegion;
                nOKValue = binaryThresholdForm.nOKValue;
                nNGValue = binaryThresholdForm.nNGValue;
                ModelImage = binaryThresholdForm.ModelImage;
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
            HTuple useThreshold;
            if (source == null || source.Key == IntPtr.Zero)
            {
                throw new RunException(1);
            }
            threshold = source.BinaryThreshold(strMethod, strRegion, out useThreshold);
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
            BinaryThresholdForm binaryThresholdForm = new BinaryThresholdForm();
            binaryThresholdForm.ShowDialog();
            if (binaryThresholdForm.Result == System.Windows.Forms.DialogResult.OK)
            {
                strMethod = binaryThresholdForm.strMethod;
                strRegion = binaryThresholdForm.strRegion;
                nOKValue = binaryThresholdForm.nOKValue;
                nNGValue = binaryThresholdForm.nNGValue;
                ModelImage = binaryThresholdForm.ModelImage;
                IsSetupOK = true;
            }
            else
            {
                IsSetupOK = false;
            }
        }

        public override string ToolDescriptText()
        {
            return "自适应阈值";
        }

        public override string ToolName()
        {
            return "自适应阈值";
        }

        public override string ToolType()
        {
            return "预处理";
        }
    }
}
