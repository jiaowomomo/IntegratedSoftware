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
    public partial class TextShowForm : Form
    {
        public double dbX = 0;
        public double dbY = 0;
        public string strHead = "";
        public string strEnd = "";
        public int nSize = 15;
        public string strColor = "black";
        public DialogResult Result;

        public TextShowForm()
        {
            InitializeComponent();

            comboBoxSize.SelectedIndex = 0;
            comboBoxColor.SelectedIndex = 0;
        }

        public TextShowForm(TextShow textShow)
        {
            InitializeComponent();

            textBoxX.Text = textShow.dbX.ToString();
            textBoxY.Text = textShow.dbY.ToString();
            textBoxHead.Text = textShow.strHead;
            textBoxEnd.Text = textShow.strEnd;
            comboBoxSize.SelectedIndex = comboBoxSize.Items.IndexOf(textShow.nSize.ToString());
            comboBoxColor.SelectedIndex = comboBoxColor.Items.IndexOf(textShow.strColor);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            dbX = Convert.ToDouble(textBoxX.Text);
            dbY = Convert.ToDouble(textBoxY.Text);
            strHead = textBoxHead.Text;
            strEnd = textBoxEnd.Text;
            nSize = Convert.ToInt32(comboBoxSize.Text);
            strColor = comboBoxColor.Text;
            Result = DialogResult.OK;
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Result = DialogResult.Cancel;
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            hObjectViewer1.ResetShowTexts();
            hObjectViewer1.AddShowText(new Halcon.Functions.ShowText(Convert.ToDouble(textBoxX.Text), Convert.ToDouble(textBoxY.Text), textBoxHead.Text + "测试" + textBoxEnd.Text, comboBoxColor.Text));
            hObjectViewer1.ResetWndCtrl(false);
        }

        private void TextShowForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            hObjectViewer1.ReleaseRam();
            this.Dispose();
            GC.Collect();
        }
    }
}
