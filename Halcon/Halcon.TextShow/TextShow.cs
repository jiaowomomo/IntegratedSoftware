using Halcon.Functions;
using HalconDotNet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Halcon.TextShow
{
    [Serializable]
    public class TextShow : IImageHalconObject
    {
        private double _dbX = 0;
        [Description("显示位置X"), Category("设置")]
        public double dbX
        {
            get { return _dbX; }
            set { _dbX = value; }
        }

        private double _dbY = 0;
        [Description("显示位置Y"), Category("设置")]
        public double dbY
        {
            get { return _dbY; }
            set { _dbY = value; }
        }

        private string _strHead = "";
        [Description("显示数据头"), Category("设置")]
        public string strHead
        {
            get { return _strHead; }
            set { _strHead = value; }
        }

        private string _strEnd = "";
        [Description("显示数据尾"), Category("设置")]
        public string strEnd
        {
            get { return _strEnd; }
            set { _strEnd = value; }
        }

        private int _nSize = 15;
        [Description("文字尺寸"), Category("设置"), Editor(typeof(SizeEditor), typeof(UITypeEditor))]
        public int nSize
        {
            get { return _nSize; }
            set
            {
                if (value >= 1 && value <= 255)
                    _nSize = value;
            }
        }

        private string _strColor = "black";
        [Description("文字颜色"), Category("设置"), TypeConverter(typeof(ColorConvert))]
        public string strColor
        {
            get { return _strColor; }
            set { _strColor = value; }
        }

        public TextShow()
        {
            GetDataManager.AddInputDoubleArray("输入数值");
        }

        public override void EditParameters()
        {
            //TextShowForm textShowForm = new TextShowForm(this);
            //textShowForm.ShowDialog();
            //if (textShowForm.Result == System.Windows.Forms.DialogResult.OK)
            //{
            //    dbX = textShowForm.dbX;
            //    dbY = textShowForm.dbY;
            //    strHead = textShowForm.strHead;
            //    strEnd = textShowForm.strEnd;
            //    nSize = textShowForm.nSize;
            //    strColor = textShowForm.strColor;
            //    IsSetupOK = true;
            //}
            //else
            //{
            //    IsSetupOK = false;
            //}
            TextShowFormNew textShowForm = new TextShowFormNew(this);
            if (textShowForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                dbX = textShowForm.MyTextShow.dbX;
                dbY = textShowForm.MyTextShow.dbY;
                strHead = textShowForm.MyTextShow.strHead;
                strEnd = textShowForm.MyTextShow.strEnd;
                nSize = textShowForm.MyTextShow.nSize;
                strColor = textShowForm.MyTextShow.strColor;
                IsSetupOK = true;
            }
            else
            {
                IsSetupOK = false;
            }
        }

        public override void Execute(ref HImage source, ref List<ShowObject> showObjects, ref List<ShowText> showTexts)
        {
            if (GetDataManager.GetInputDoubleArray("输入数值").Count != 0)
            {
                showTexts.Add(new ShowText(dbX, dbY, strHead + GetDataManager.GetInputDoubleArray("输入数值")[0].ToString() + strEnd, strColor));
            }
        }

        public override void SetParameters()
        {
            //TextShowForm textShowForm = new TextShowForm();
            //textShowForm.ShowDialog();
            //if (textShowForm.Result == System.Windows.Forms.DialogResult.OK)
            //{
            //    dbX = textShowForm.dbX;
            //    dbY = textShowForm.dbY;
            //    strHead = textShowForm.strHead;
            //    strEnd = textShowForm.strEnd;
            //    nSize = textShowForm.nSize;
            //    strColor = textShowForm.strColor;
            //    IsSetupOK = true;
            //}
            //else
            //{
            //    IsSetupOK = false;
            //}
            TextShowFormNew textShowForm = new TextShowFormNew(this);
            if (textShowForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                dbX = textShowForm.MyTextShow.dbX;
                dbY = textShowForm.MyTextShow.dbY;
                strHead = textShowForm.MyTextShow.strHead;
                strEnd = textShowForm.MyTextShow.strEnd;
                nSize = textShowForm.MyTextShow.nSize;
                strColor = textShowForm.MyTextShow.strColor;
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
