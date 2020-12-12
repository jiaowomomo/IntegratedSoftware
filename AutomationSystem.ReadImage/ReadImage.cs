using AutomationSystem.Halcon;
using HalconDotNet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomationSystem.ReadImage
{
    [Serializable]
    public class ReadImage : IImageHalconObject
    {
        public string StrFilePath = "";
        public ReadImage()
        {
        }

        public override void EditParameters()
        {
            ReadImageForm rif = new ReadImageForm(this);
            rif.ShowDialog();
            if (rif.Result == System.Windows.Forms.DialogResult.OK)
            {
                StrFilePath = rif.StrFilePath;
                IsSetupOK = true;
            }
            else
            {
                IsSetupOK = false;
            }
        }

        public override void Execute(ref HImage source, ref List<ShowObject> showObjects, ref List<ShowText> showTexts)
        {
            if (File.Exists(StrFilePath))
            {
                source = new HImage(StrFilePath);
            }
            else
            {
                throw new RunException(5);
            }
        }

        public override void SetParameters()
        {
            ReadImageForm rif = new ReadImageForm();
            rif.ShowDialog();
            if (rif.Result == System.Windows.Forms.DialogResult.OK)
            {
                StrFilePath = rif.StrFilePath;
                IsSetupOK = true;
            }
            else
            {
                IsSetupOK = false;
            }
        }

        public override string ToolDescriptText()
        {
            return "从指定文件读取图像";
        }

        public override string ToolName()
        {
            return "从文件读取图像";
        }

        public override string ToolType()
        {
            return "图像输入";
        }
    }
}