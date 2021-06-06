using Halcon.Functions;
using HalconDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Halcon.Delay
{
    [Serializable]
    public class Delay : IImageHalconObject
    {
        public int nDelay = 0;

        public override void EditParameters()
        {
            DelayForm delayForm = new DelayForm(this);
            delayForm.ShowDialog();
            if (delayForm.Result == System.Windows.Forms.DialogResult.OK)
            {
                nDelay = delayForm.nDelay;
                IsSetupOK = true;
            }
            else
            {
                IsSetupOK = false;
            }
        }

        public override void Execute(ref HImage source, ref List<ShowObject> showObjects, ref List<ShowText> showTexts)
        {
            System.Threading.Thread.Sleep(nDelay);
        }

        public override void SetParameters()
        {
            DelayForm delayForm = new DelayForm();
            delayForm.ShowDialog();
            if (delayForm.Result == System.Windows.Forms.DialogResult.OK)
            {
                nDelay = delayForm.nDelay;
                IsSetupOK = true;
            }
            else
            {
                IsSetupOK = false;
            }
        }

        public override string ToolDescriptText()
        {
            return "延时";
        }

        public override string ToolName()
        {
            return "延时";
        }

        public override string ToolType()
        {
            return "系统工具";
        }
    }
}
