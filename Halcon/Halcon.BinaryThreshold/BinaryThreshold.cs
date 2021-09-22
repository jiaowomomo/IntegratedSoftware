using Halcon.Functions;
using HalconDotNet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Halcon.BinaryThreshold
{
    [Serializable]
    public class BinaryThreshold : IImageHalconObject
    {
        private string _strMethod = "max_separability";
        [Description("提取方法"), Category("设置"), TypeConverter(typeof(MethodConvert))]
        public string strMethod
        {
            get { return _strMethod; }
            set { _strMethod = value; }
        }

        private string _strRegion = "暗";
        [Description("提取区域"), Category("设置"), TypeConverter(typeof(ExtractConvert))]
        public string strRegion
        {
            get { return _strRegion; }
            set { _strRegion = value; }
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

        public string GetExtract()
        {
            if (strRegion == "亮")
                return "light";
            else
                return "dark";
        }

        public override void EditParameters()
        {
            //BinaryThresholdForm binaryThresholdForm = new BinaryThresholdForm(this);
            //binaryThresholdForm.ShowDialog();
            //if (binaryThresholdForm.Result == System.Windows.Forms.DialogResult.OK)
            //{
            //    strMethod = binaryThresholdForm.strMethod;
            //    strRegion = binaryThresholdForm.strRegion;
            //    nOKValue = binaryThresholdForm.nOKValue;
            //    nNGValue = binaryThresholdForm.nNGValue;
            //    ModelImage = binaryThresholdForm.ModelImage;
            //    IsSetupOK = true;
            //}
            //else
            //{
            //    IsSetupOK = false;
            //}
            BinaryThresholdFormNew binaryThresholdForm = new BinaryThresholdFormNew(this);
            if (binaryThresholdForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                strMethod = binaryThresholdForm.MyBinaryThreshold.strMethod;
                strRegion = binaryThresholdForm.MyBinaryThreshold.strRegion;
                nOKValue = binaryThresholdForm.MyBinaryThreshold.nOKValue;
                nNGValue = binaryThresholdForm.MyBinaryThreshold.nNGValue;
                ModelImage = binaryThresholdForm.MyBinaryThreshold.ModelImage;
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
                throw new RunException(RunExceptionType.NoInputImage);
            }
            threshold = source.BinaryThreshold(strMethod, GetExtract(), out useThreshold);
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
            //BinaryThresholdForm binaryThresholdForm = new BinaryThresholdForm();
            //binaryThresholdForm.ShowDialog();
            //if (binaryThresholdForm.Result == System.Windows.Forms.DialogResult.OK)
            //{
            //    strMethod = binaryThresholdForm.strMethod;
            //    strRegion = binaryThresholdForm.strRegion;
            //    nOKValue = binaryThresholdForm.nOKValue;
            //    nNGValue = binaryThresholdForm.nNGValue;
            //    ModelImage = binaryThresholdForm.ModelImage;
            //    IsSetupOK = true;
            //}
            //else
            //{
            //    IsSetupOK = false;
            //}
            BinaryThresholdFormNew binaryThresholdForm = new BinaryThresholdFormNew(this);
            if (binaryThresholdForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                strMethod = binaryThresholdForm.MyBinaryThreshold.strMethod;
                strRegion = binaryThresholdForm.MyBinaryThreshold.strRegion;
                nOKValue = binaryThresholdForm.MyBinaryThreshold.nOKValue;
                nNGValue = binaryThresholdForm.MyBinaryThreshold.nNGValue;
                ModelImage = binaryThresholdForm.MyBinaryThreshold.ModelImage;
                IsSetupOK = true;
            }
            else
            {
                IsSetupOK = false;
            }
        }

        public override string ToolDescriptText()
        {
            return "二值化";
        }

        public override string ToolName()
        {
            return "二值化";
        }

        public override string ToolType()
        {
            return "预处理";
        }
    }
}
