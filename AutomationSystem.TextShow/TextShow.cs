using AutomationSystem.Halcon;
using HalconDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomationSystem.TextShow
{
    [Serializable]
    public class TextShow : IImageHalconObject
    {
        public double dbX = 0;
        public double dbY = 0;
        public string strHead = "";
        public string strEnd = "";
        public int nSize = 15;
        public string strColor = "black";

        public TextShow()
        {
            GetDataManager.AddInputDoubleVector("输入数值");
        }

        public override void EditParameters()
        {
            TextShowForm textShowForm = new TextShowForm(this);
            textShowForm.ShowDialog();
            if (textShowForm.Result == System.Windows.Forms.DialogResult.OK)
            {
                dbX = textShowForm.dbX;
                dbY = textShowForm.dbY;
                strHead = textShowForm.strHead;
                strEnd = textShowForm.strEnd;
                nSize = textShowForm.nSize;
                strColor = textShowForm.strColor;
                IsSetupOK = true;
            }
            else
            {
                IsSetupOK = false;
            }
        }

        public override void Execute(ref HImage source, ref List<ShowObject> showObjects, ref List<ShowText> showTexts)
        {
            if (GetDataManager.GetInputDoubleVector("输入数值").Count != 0)
            {
                showTexts.Add(new ShowText(dbX, dbY, strHead + GetDataManager.GetInputDoubleVector("输入数值")[0].ToString() + strEnd, strColor));
            }
        }

        public override void SetParameters()
        {
            TextShowForm textShowForm = new TextShowForm();
            textShowForm.ShowDialog();
            if (textShowForm.Result == System.Windows.Forms.DialogResult.OK)
            {
                dbX = textShowForm.dbX;
                dbY = textShowForm.dbY;
                strHead = textShowForm.strHead;
                strEnd = textShowForm.strEnd;
                nSize = textShowForm.nSize;
                strColor = textShowForm.strColor;
                IsSetupOK = true;
            }
            else
            {
                IsSetupOK = false;
            }
        }

        public override string ToolDescriptText()
        {
            return "文字显示";
        }

        public override string ToolName()
        {
            return "文字显示";
        }

        public override string ToolType()
        {
            return "系统工具";
        }
    }
}
