using Halcon.Functions;
using HalconDotNet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Halcon.ManualCalibration
{
    public partial class ManualCalibrationForm : Form
    {
        private int nMaxCalibIndex = 40;
        private string FilePath = Application.StartupPath + @"\ManualCalibration";
        HImage sourceImage;
        HHomMat2D pixToWorld;
        List<double> pixelCoordinatesX = new List<double>();
        List<double> pixelCoordinatesY = new List<double>();
        List<double> realWorldCoordinatesX = new List<double>();
        List<double> realWorldCoordinatesY = new List<double>();

        public ManualCalibrationForm()
        {
            InitializeComponent();

            for (int i = 0; i < nMaxCalibIndex; i++)
            {
                cbbCalibIndex.Items.Add(i);
            }
            cbbCalibIndex.SelectedIndex = 0;

            hObjectViewer1.SetImageHandle(ImageThreshold);
            //hObjectViewer1.OnResizeDrawingObject += ImageThreshold;
            //hObjectViewer1.OnAttachDrawingObject += ImageThreshold;
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Type type = hObjectViewer1.GetType();
            MethodInfo methodInfo = type.GetMethod("toolStripMenuItemOpen_Click", BindingFlags.Instance | BindingFlags.NonPublic);
            if (methodInfo != null)
            {
                object[] parameters = new object[] { null, new EventArgs() };
                methodInfo.Invoke(hObjectViewer1, parameters);
                sourceImage = hObjectViewer1.SourceImage;
            }
        }

        private void trackBarMin_ValueChanged(object sender, EventArgs e)
        {
            if (trackBarMin.Value >= trackBarMax.Value)
            {
                trackBarMax.Value = trackBarMin.Value;
            }
            lbThresholdMin.Text = trackBarMin.Value.ToString();
            ImageThreshold();
        }

        private void trackBarMax_ValueChanged(object sender, EventArgs e)
        {
            if (trackBarMax.Value <= trackBarMin.Value)
            {
                trackBarMin.Value = trackBarMax.Value;
            }
            lbThresholdMax.Text = trackBarMax.Value.ToString();
            ImageThreshold();
        }

        HRegion findCircle;
        object handleLock = new object();

        private void ImageThreshold()
        {
            lock (handleLock)
            {
                if (sourceImage != null)
                {
                    HImage ReduceImage = new HImage();
                    HRegion GrayImage = new HRegion();
                    //HXLDCont hXLDCont;
                    //HTuple Row, Column, Radius, StartPhi, EndPhi, PointOrder;
                    try
                    {
                        ReduceImage = sourceImage.ReduceDomain(hObjectViewer1.GetRegions());
                        this.Invoke((EventHandler)delegate
                        {
                            GrayImage = ReduceImage.Threshold((double)trackBarMin.Value, (double)trackBarMax.Value);
                        });
                        if (GrayImage.Key != IntPtr.Zero)
                        {
                            GrayImage.ClosingCircle(10.5);
                            GrayImage = GrayImage.ShapeTrans("outer_circle");
                            //hXLDCont = GrayImage.GenContourRegionXld("border");
                            //hXLDCont.FitCircleContourXld("atukey", -1, 2, 0, 10, 1, out Row, out Column, out Radius, out StartPhi, out EndPhi, out PointOrder);
                            //hXLDCont.GenCircleContourXld(Row, Column, Radius, StartPhi, EndPhi, PointOrder, 1);
                            findCircle = GrayImage.Clone();
                            hObjectViewer1.ResetShowObjects();
                            ShowObject showObject = new ShowObject(findCircle, "red", "fill");
                            hObjectViewer1.AddShowObject(showObject);
                            hObjectViewer1.ResetWndCtrl(false);
                        }
                    }
                    catch
                    { }
                    ReduceImage.Dispose();
                    GrayImage.Dispose();
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (findCircle.Key != IntPtr.Zero)
            {
                SetCoordinate sc = new SetCoordinate();
                if (sc.ShowDialog() == DialogResult.OK)
                {
                    int index = dataGridView1.Rows.Add();
                    dataGridView1.Rows[index].Cells[0].Value = index.ToString();
                    dataGridView1.Rows[index].Cells[1].Value = findCircle.Row.ToString();
                    dataGridView1.Rows[index].Cells[2].Value = findCircle.Column.ToString();
                    dataGridView1.Rows[index].Cells[3].Value = sc.XCoordinate.ToString("F3");
                    dataGridView1.Rows[index].Cells[4].Value = sc.YCoordinate.ToString("F3");
                    pixelCoordinatesX.Add(findCircle.Column.D);
                    pixelCoordinatesY.Add(findCircle.Row.D);
                    realWorldCoordinatesX.Add(sc.XCoordinate);
                    realWorldCoordinatesY.Add(sc.YCoordinate);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (sourceImage != null)
            {
                hObjectViewer1.SetImage(sourceImage);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (pixelCoordinatesX.Count != 0 && pixelCoordinatesX.Count >= 3)
            {
                try
                {
                    HHomMat2D mat2D = new HHomMat2D();
                    mat2D.VectorToHomMat2d(new HTuple(pixelCoordinatesX.ToArray()), new HTuple(pixelCoordinatesY.ToArray()), new HTuple(realWorldCoordinatesX.ToArray()), new HTuple(realWorldCoordinatesY.ToArray()));
                    if (!Directory.Exists(FilePath))
                    {
                        Directory.CreateDirectory(FilePath);
                    }
                    string path = FilePath + @"\calib" + cbbCalibIndex.Text + @".data";
                    HOperatorSet.WriteTuple(mat2D, path);
                    MessageBox.Show("标定成功");
                    pixToWorld = mat2D;
                }
                catch (HOperatorException he)
                {
                    MessageBox.Show(he.GetErrorMessage());
                }
            }
        }

        public void HMouseMove(object sender, HalconDotNet.HMouseEventArgs e)
        {
            if (sourceImage != null)
            {
                try
                {
                    HTuple Row, Column, Button, qx, qy;
                    HOperatorSet.GetMposition(hObjectViewer1.ViewerInstance.HalconWindow, out Row, out Column, out Button);//获取当前鼠标的坐标值
                    HOperatorSet.AffineTransPoint2d(pixToWorld, Column, Row, out qx, out qy);
                    labelX.Text = qx.D.ToString("f4");
                    labelY.Text = qy.D.ToString("f4");
                }
                catch (HOperatorException he)
                {
                    //MessageBox.Show(he.GetErrorMessage());
                }
            }
        }

        private void cbTest_CheckedChanged(object sender, EventArgs e)
        {
            if (cbTest.Checked)
            {
                hObjectViewer1.ViewerInstance.HMouseMove += HMouseMove;
            }
            else
            {
                hObjectViewer1.ViewerInstance.HMouseMove -= HMouseMove;
            }
        }

        private void cbbCalibIndex_SelectedIndexChanged(object sender, EventArgs e)
        {
            string path = FilePath + @"\calib" + cbbCalibIndex.Text + @".data";
            HTuple tuple;
            if (File.Exists(path))
            {
                HOperatorSet.ReadTuple(path, out tuple);
                pixToWorld = new HHomMat2D(tuple);
            }
            else
            {
                pixToWorld = new HHomMat2D();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            pixelCoordinatesX.Clear();
            pixelCoordinatesY.Clear();
            realWorldCoordinatesX.Clear();
            realWorldCoordinatesY.Clear();
        }

        private void ManualCalibrationForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (sourceImage != null)
            {
                sourceImage.Dispose();
            }
            if (findCircle != null)
            {
                findCircle.Dispose();
            }
            hObjectViewer1.ReleaseRam();
            this.Dispose();
            GC.Collect();
        }
    }
}
