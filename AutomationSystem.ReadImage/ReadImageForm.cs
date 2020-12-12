using HalconDotNet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutomationSystem.ReadImage
{
    public partial class ReadImageForm : Form
    {
        HImage source_Image;
        public DialogResult Result { get; set; }
        public string StrFilePath = "";

        public ReadImageForm()
        {
            InitializeComponent();
        }

        public ReadImageForm(ReadImage readImage)
        {
            InitializeComponent();

            listView1.Items.Add(readImage.StrFilePath);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "BMP File|*.bmp|PNG File|*.png|JPEG File|*.jpg|All|*.*";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                if (!string.IsNullOrEmpty(ofd.FileName))
                {
                    source_Image = new HImage(ofd.FileName);
                    if (source_Image != null)
                    {
                        hObjectViewer1.SetImage(source_Image);
                        listView1.Items.Clear();
                        listView1.Items.Add(ofd.FileName);
                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (listView1.Items.Count != 0)
            {
                StrFilePath = listView1.Items[0].Text;
                Result = DialogResult.OK;
                this.Close();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Result = DialogResult.Cancel;
            this.Close();
        }

        private void ReadImageForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (source_Image != null)
            {
                source_Image.Dispose();
            }
            hObjectViewer1.ReleaseRam();
            this.Dispose();
            GC.Collect();
        }
    }
}
