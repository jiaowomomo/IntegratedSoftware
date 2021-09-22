using Halcon.Functions;
using HalconDotNet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Halcon.HysteresisThreshold
{
    [Serializable]
    public class HysteresisThreshold : IImageHalconObject
    {
        private int _nThresholdMin = 1;
        [Description("最小灰度值"), Category("设置"), Editor(typeof(ValueEditor), typeof(UITypeEditor))]
        public int nThresholdMin
        {
            get { return _nThresholdMin; }
            set
            {
                if (value < nThresholdMax && value >= 1 && value <= 255)
                {
                    _nThresholdMin = value;
                }
            }
        }

        private int _nThresholdMax = 255;
        [Description("最大灰度值"), Category("设置"), Editor(typeof(ValueEditor), typeof(UITypeEditor))]
        public int nThresholdMax
        {
            get { return _nThresholdMax; }
            set
            {
                if (value > nThresholdMin && value >= 0 && value <= 255)
                {
                    _nThresholdMax = value;
                }
            }
        }

        private int _nLength = 1;
        [Description("最大长度"), Category("设置"), Editor(typeof(ValueEditor), typeof(UITypeEditor))]
        public int nLength
        {
            get { return _nLength; }
            set
            {
                if (value >= 1 && value <= 255)
                {
                    _nLength = value;
                }
            }
        }

        private int _nOKValue = 255;
        [Description("有效灰度值"), Category("设置"), Editor(typeof(ValueEditor), typeof(UITypeEditor))]
        public int nOKValue
        {
            get { return _nOKValue; }
            set
            {
                if (value >= 0 && value <= 255)
                    _nOKValue = value;
            }
        }

        private int _nNGValue = 0;
        [Description("无效灰度值"), Category("设置"), Editor(typeof(ValueEditor), typeof(UITypeEditor))]
        public int nNGValue
        {
            get { return _nNGValue; }
            set
            {
                if (value >= 0 && value <= 255)
                    _nNGValue = value;
            }
        }

        private string _ModelImage = "";
        [Description("模板图片"), ReadOnly(true), Category("设置")]
        public string ModelImage
        {
            get { return _ModelImage; }
            set { _ModelImage = value; }
        }

        public override void EditParameters()
        {
            //HysteresisThresholdForm hysteresisThresholdForm = new HysteresisThresholdForm(this);
            //hysteresisThresholdForm.ShowDialog();
            //if (hysteresisThresholdForm.Result == System.Windows.Forms.DialogResult.OK)
            //{
            //    nThresholdMin = hysteresisThresholdForm.nThresholdMin;
            //    nThresholdMax = hysteresisThresholdForm.nThresholdMax;
            //    nLength = hysteresisThresholdForm.nLength;
            //    nOKValue = hysteresisThresholdForm.nOKValue;
            //    nNGValue = hysteresisThresholdForm.nNGValue;
            //    ModelImage = hysteresisThresholdForm.ModelImage;
            //    IsSetupOK = true;
            //}
            //else
            //{
            //    IsSetupOK = false;
            //}
            HysteresisThresholdFormNew hysteresisThresholdForm = new HysteresisThresholdFormNew(this);
            if (hysteresisThresholdForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                nThresholdMin = hysteresisThresholdForm.MyHysteresisThreshold.nThresholdMin;
                nThresholdMax = hysteresisThresholdForm.MyHysteresisThreshold.nThresholdMax;
                nLength = hysteresisThresholdForm.MyHysteresisThreshold.nLength;
                nOKValue = hysteresisThresholdForm.MyHysteresisThreshold.nOKValue;
                nNGValue = hysteresisThresholdForm.MyHysteresisThreshold.nNGValue;
                ModelImage = hysteresisThresholdForm.MyHysteresisThreshold.ModelImage;
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
                throw new RunException(RunExceptionType.NoInputImage);
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
            //HysteresisThresholdForm hysteresisThresholdForm = new HysteresisThresholdForm();
            //hysteresisThresholdForm.ShowDialog();
            //if (hysteresisThresholdForm.Result == System.Windows.Forms.DialogResult.OK)
            //{
            //    nThresholdMin = hysteresisThresholdForm.nThresholdMin;
            //    nThresholdMax = hysteresisThresholdForm.nThresholdMax;
            //    nLength = hysteresisThresholdForm.nLength;
            //    nOKValue = hysteresisThresholdForm.nOKValue;
            //    nNGValue = hysteresisThresholdForm.nNGValue;
            //    ModelImage = hysteresisThresholdForm.ModelImage;
            //    IsSetupOK = true;
            //}
            //else
            //{
            //    IsSetupOK = false;
            //}
            HysteresisThresholdFormNew hysteresisThresholdForm = new HysteresisThresholdFormNew(this);
            if (hysteresisThresholdForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                nThresholdMin = hysteresisThresholdForm.MyHysteresisThreshold.nThresholdMin;
                nThresholdMax = hysteresisThresholdForm.MyHysteresisThreshold.nThresholdMax;
                nLength = hysteresisThresholdForm.MyHysteresisThreshold.nLength;
                nOKValue = hysteresisThresholdForm.MyHysteresisThreshold.nOKValue;
                nNGValue = hysteresisThresholdForm.MyHysteresisThreshold.nNGValue;
                ModelImage = hysteresisThresholdForm.MyHysteresisThreshold.ModelImage;
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
