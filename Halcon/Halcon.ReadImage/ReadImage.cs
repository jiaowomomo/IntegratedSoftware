using Halcon.Functions;
using HalconDotNet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Halcon.ReadImage
{
    [Serializable]
    public class ReadImage : IImageHalconObject
    {
        public List<string> StrFilePathList = new List<string>();
        private int m_intImageIndex = 0;

        public ReadImage()
        {
        }

        public override void EditParameters()
        {
            ReadImageForm rif = new ReadImageForm(this);
            rif.ShowDialog();
            if (rif.Result == System.Windows.Forms.DialogResult.OK)
            {
                StrFilePathList = rif.StrFilePathList;
                IsSetupOK = true;
            }
            else
            {
                IsSetupOK = false;
            }
        }

        public override void Execute(ref HImage source, ref List<ShowObject> showObjects, ref List<ShowText> showTexts)
        {
            if (File.Exists(StrFilePathList[m_intImageIndex]))
            {
                source = new HImage(StrFilePathList[m_intImageIndex]);
                m_intImageIndex++;
                if (m_intImageIndex == StrFilePathList.Count)
                    m_intImageIndex = 0;
            }
            else
            {
                throw new RunException(RunExceptionType.FilePathNotExist);
            }
        }

        public override void SetParameters()
        {
            ReadImageForm rif = new ReadImageForm();
            rif.ShowDialog();
            if (rif.Result == System.Windows.Forms.DialogResult.OK)
            {
                StrFilePathList = rif.StrFilePathList;
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