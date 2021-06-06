using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Halcon.TextShow
{
    public partial class TextShowFormNew : Form
    {
        public TextShow MyTextShow;

        public TextShowFormNew(TextShow _textShow)
        {
            InitializeComponent();
            MyTextShow = new TextShow()
            {
                dbX = _textShow.dbX,
                dbY = _textShow.dbY,
                nSize = _textShow.nSize,
                strColor = _textShow.strColor,
                strEnd = _textShow.strEnd,
                strHead = _textShow.strHead
            };
            propertiesViewer1.SetProperties(MyTextShow);
            propertiesViewer1.SetHandle(new Action(ImageHandle));
        }

        private void ImageHandle()
        {
            hObjectViewer1.ResetShowTexts();
            hObjectViewer1.AddShowText(new Halcon.Functions.ShowText(MyTextShow.dbX, MyTextShow.dbY, MyTextShow.strHead + "测试" + MyTextShow.strEnd, MyTextShow.strColor));
            hObjectViewer1.ResetWndCtrl(false);
        }

        private void TextShowFormNew_FormClosing(object sender, FormClosingEventArgs e)
        {
            hObjectViewer1.ReleaseRam();
            this.Dispose();
            GC.Collect();
        }
    }
}
