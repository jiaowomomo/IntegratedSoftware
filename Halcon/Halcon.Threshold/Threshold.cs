using Halcon.Functions;
using HalconDotNet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Halcon.Threshold
{
    [Serializable]
    public class Threshold : IImageHalconObject
    {
        private double _dbThresholdMin = 0;
        [Description("最小灰度值"), Category("设置"), Editor(typeof(ValueEditor), typeof(UITypeEditor))]
        public double dbThresholdMin
        {
            get { return _dbThresholdMin; }
            set
            {
                if (value < dbThresholdMax && value >= 0 && value <= 255)
                {
                    _dbThresholdMin = value;
                }
            }
        }


        private double _dbThresholdMax = 255;
        [Description("最大灰度值"), Category("设置"), Editor(typeof(ValueEditor), typeof(UITypeEditor))]
        public double dbThresholdMax
        {
            get { return _dbThresholdMax; }
            set
            {
                if (value > dbThresholdMin && value >= 0 && value <= 255)
                {
                    _dbThresholdMax = value;
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

        private string _modelImage = "";
        [Description("模板图片"), ReadOnly(true), Category("设置")]
        public string ModelImage
        {
            get { return _modelImage; }
            set { _modelImage = value; }
        }

        public Threshold()
        {
        }

        public override void EditParameters()
        {
            //ThresholdForm thresholdForm = new ThresholdForm(this);
            //thresholdForm.ShowDialog();
            //if (thresholdForm.Result == System.Windows.Forms.DialogResult.OK)
            //{
            //    dbThresholdMin = thresholdForm.dbThresholdMin;
            //    dbThresholdMax = thresholdForm.dbThresholdMax;
            //    nOKValue = thresholdForm.nOKValue;
            //    nNGValue = thresholdForm.nNGValue;
            //    ModelImage = thresholdForm.ModelImage;
            //    IsSetupOK = true;
            //}
            //else
            //{
            //    IsSetupOK = false;
            //}
            ThresholdFormNew thresholdForm = new ThresholdFormNew(this);
            if (thresholdForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                dbThresholdMin = thresholdForm.MyThreshold.dbThresholdMin;
                dbThresholdMax = thresholdForm.MyThreshold.dbThresholdMax;
                nOKValue = thresholdForm.MyThreshold.nOKValue;
                nNGValue = thresholdForm.MyThreshold.nNGValue;
                ModelImage = thresholdForm.MyThreshold.ModelImage;
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
            //ThresholdForm thresholdForm = new ThresholdForm();
            //thresholdForm.ShowDialog();
            //if (thresholdForm.Result == System.Windows.Forms.DialogResult.OK)
            //{
            //    dbThresholdMin = thresholdForm.dbThresholdMin;
            //    dbThresholdMax = thresholdForm.dbThresholdMax;
            //    nOKValue = thresholdForm.nOKValue;
            //    nNGValue = thresholdForm.nNGValue;
            //    ModelImage = thresholdForm.ModelImage;
            //    IsSetupOK = true;
            //}
            //else
            //{
            //    IsSetupOK = false;
            //}
            ThresholdFormNew thresholdForm = new ThresholdFormNew(this);
            if (thresholdForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                dbThresholdMin = thresholdForm.MyThreshold.dbThresholdMin;
                dbThresholdMax = thresholdForm.MyThreshold.dbThresholdMax;
                nOKValue = thresholdForm.MyThreshold.nOKValue;
                nNGValue = thresholdForm.MyThreshold.nNGValue;
                ModelImage = thresholdForm.MyThreshold.ModelImage;
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
