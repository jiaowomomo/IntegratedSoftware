using Camera.Common;
using Halcon.Functions;
using HalconDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Halcon.GetImageFromCamera
{
    [Serializable]
    public class GetImage : IImageHalconObject
    {
        public bool IsSetExposure = false;
        public double ExposureTime = 10000;
        public int CameraIndex = -1;

        public override void EditParameters()
        {
            GetImageForm getImageForm = new GetImageForm(this);
            getImageForm.ShowDialog();
            if (getImageForm.Result == System.Windows.Forms.DialogResult.OK)
            {
                IsSetExposure = getImageForm.IsSetExposure;
                ExposureTime = getImageForm.ExposureTime;
                CameraIndex = getImageForm.CameraIndex;
                IsSetupOK = true;
            }
            else
            {
                IsSetupOK = false;
            }
        }

        public override void Execute(ref HImage source, ref List<ShowObject> showObjects, ref List<ShowText> showTexts)
        {
            if (CameraIndex != -1)
            {
                if (CameraIndex < CameraControl.Instance.GetCameras().Count)
                {
                    if (IsSetExposure)
                    {
                        CameraControl.Instance.SetExposureTime(CameraIndex, ExposureTime);
                    }
                    CameraControl.Instance.SingleShoot(CameraIndex);
                    source.Dispose();
                    source = CameraControl.Instance.GetCurrentHImage(CameraIndex);
                }
                else
                {
                    throw new RunException(RunExceptionType.CameraNotExist);
                }
            }
            else
            {
                throw new RunException(RunExceptionType.CameraNotExist);
            }
        }

        public override void SetParameters()
        {
            GetImageForm getImageForm = new GetImageForm();
            getImageForm.ShowDialog();
            if (getImageForm.Result == System.Windows.Forms.DialogResult.OK)
            {
                IsSetExposure = getImageForm.IsSetExposure;
                ExposureTime = getImageForm.ExposureTime;
                CameraIndex = getImageForm.CameraIndex;
                IsSetupOK = true;
            }
            else
            {
                IsSetupOK = false;
            }
        }

        public override string ToolDescriptText()
        {
            return "从相机获取图像";
        }

        public override string ToolName()
        {
            return "从相机获取图像";
        }

        public override string ToolType()
        {
            return "图像输入";
        }
    }
}
