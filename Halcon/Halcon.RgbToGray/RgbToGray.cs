using Halcon.Functions;
using HalconDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Halcon.RgbToGray
{
    [Serializable]
    public class RgbToGray : IImageHalconObject
    {
        public override void EditParameters()
        {
            RgbToGrayForm rgbToGrayForm = new RgbToGrayForm();
            rgbToGrayForm.ShowDialog();
            if (rgbToGrayForm.Result == System.Windows.Forms.DialogResult.OK)
            {
                IsSetupOK = true;
            }
            else
            {
                IsSetupOK = false;
            }
        }

        public override void Execute(ref HImage source, ref List<ShowObject> showObjects, ref List<ShowText> showTexts)
        {
            HImage gray = new HImage();
            if (source == null || source.Key == IntPtr.Zero)
            {
                if (gray != null)
                {
                    gray.Dispose();
                }
                throw new RunException(RunExceptionType.NoInputImage);
            }
            if (source.CountChannels() == 3)
            {
                gray = source.Rgb1ToGray();
                source.Dispose();
                source = gray.Clone();
            }
            else
            {
            }
            if (gray != null)
            {
                gray.Dispose();
            }
        }

        public override void SetParameters()
        {
            RgbToGrayForm rgbToGrayForm = new RgbToGrayForm();
            rgbToGrayForm.ShowDialog();
            if (rgbToGrayForm.Result == System.Windows.Forms.DialogResult.OK)
            {
                IsSetupOK = true;
            }
            else
            {
                IsSetupOK = false;
            }
        }

        public override string ToolDescriptText()
        {
            return "RGB三通道图转换为单通道灰度图，方便后续图像处理";
        }

        public override string ToolName()
        {
            return "RGB转灰度图";
        }

        public override string ToolType()
        {
            return "预处理";
        }
    }
}
