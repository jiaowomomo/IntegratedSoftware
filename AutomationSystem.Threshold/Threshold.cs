using AutomationSystem.Halcon;
using HalconDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomationSystem.Threshold
{
    [Serializable]
    public class Threshold : IImageHalconObject
    {
        public double dbThresholdMin = 0;
        public double dbThresholdMax = 0;
        public int nOKValue = 0;
        public int nNGValue = 0;
        public string ModelImage = "";

        public Threshold()
        {
        }

        public override void EditParameters()
        {
            ThresholdForm thresholdForm = new ThresholdForm(this);
            thresholdForm.ShowDialog();
            if (thresholdForm.Result == System.Windows.Forms.DialogResult.OK)
            {
                dbThresholdMin = thresholdForm.dbThresholdMin;
                dbThresholdMax = thresholdForm.dbThresholdMax;
                nOKValue = thresholdForm.nOKValue;
                nNGValue = thresholdForm.nNGValue;
                ModelImage = thresholdForm.ModelImage;
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
            threshold = source.Threshold(dbThresholdMin, dbThresholdMax);
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
            ThresholdForm thresholdForm = new ThresholdForm();
            thresholdForm.ShowDialog();
            if (thresholdForm.Result == System.Windows.Forms.DialogResult.OK)
            {
                dbThresholdMin = thresholdForm.dbThresholdMin;
                dbThresholdMax = thresholdForm.dbThresholdMax;
                nOKValue = thresholdForm.nOKValue;
                nNGValue = thresholdForm.nNGValue;
                ModelImage = thresholdForm.ModelImage;
                IsSetupOK = true;
            }
            else
            {
                IsSetupOK = false;
            }
        }

        public override string ToolDescriptText()
        {
            return "提取符合灰度范围内的像素值，并将图像转换为二值化图像";
        }

        public override string ToolName()
        {
            return "普通阈值分割";
        }

        public override string ToolType()
        {
            return "预处理";
        }
    }
}
