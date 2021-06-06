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
using static System.Windows.Forms.ListView;

namespace Halcon.ReadImage
{
    public partial class ReadImageForm : Form
    {
        HImage source_Image;
        public DialogResult Result { get; set; }
        public List<string> StrFilePathList = new List<string>();

        public ReadImageForm()
        {
            InitializeComponent();
        }

        public ReadImageForm(ReadImage readImage)
        {
            InitializeComponent();

            listView1.Items.Clear();
            foreach (var item in readImage.StrFilePathList)
            {
                listView1.Items.Add(item);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = true;
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
                        foreach (var item in ofd.FileNames)
                        {
                            listView1.Items.Add(item);
                        }
                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (listView1.Items.Count != 0)
            {
                StrFilePathList = new List<string>();
                foreach (ListViewItem item in listView1.Items)
                {
                    StrFilePathList.Add(item.Text);
                }
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

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count != 0)
            {
                int index = listView1.SelectedItems[0].Index;
                if (index != -1)
                {
                    if (!string.IsNullOrEmpty(listView1.SelectedItems[0].Text))
                    {
                        source_Image = new HImage(listView1.SelectedItems[0].Text);
                        if (source_Image != null)
                        {
                            hObjectViewer1.SetImage(source_Image);
                        }
                    }
                }
            }
        }
    }
}
