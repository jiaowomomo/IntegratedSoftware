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

namespace Halcon.AutoCircleCalibration
{
    public partial class AutoCircleCalibrationForm : Form
    {
        private int nMaxCalibIndex = 40;
        private string FilePath = Application.StartupPath + @"\AutoCircleCalibration";
        HImage sourceImage;
        HHomMat2D pixToWorld;
        List<double> pixelCoordinatesX = new List<double>();
        List<double> pixelCoordinatesY = new List<double>();
        List<double> realWorldCoordinatesX = new List<double>();
        List<double> realWorldCoordinatesY = new List<double>();

        public AutoCircleCalibrationForm()
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
                    try
                    {
                        ReduceImage = sourceImage.ReduceDomain(hObjectViewer1.GetRegions());
                        this.Invoke((EventHandler)delegate
                        {
                            GrayImage = ReduceImage.Threshold((double)trackBarMin.Value, (double)trackBarMax.Value);
                        });
                        if (GrayImage.Key != IntPtr.Zero)
                        {
                            GrayImage = GrayImage.Connection();
                            this.Invoke((EventHandler)delegate
                            {
                                GrayImage = GrayImage.SelectShape("area", "and", (double)trackBarAreaMin.Value, (double)trackBarAreaMax.Value);
                            });
                            GrayImage = GrayImage.ShapeTrans("outer_circle");
                            int count = GrayImage.CountObj();//查找圆数量
                            this.Invoke((EventHandler)delegate
                            {
                                labelAutoCircle.Text = count.ToString();
                            });
                            findCircle = GrayImage.Clone();
                            hObjectViewer1.ResetShowObjects();
                            hObjectViewer1.AddShowObject(new ShowObject(findCircle, "red", "fill"));

                            //求映射
                            int pointRows = Convert.ToInt32(numericUpDownRow.Value);
                            int pointColumns = Convert.ToInt32(numericUpDownCol.Value);
                            int rowMid = pointRows / 2;
                            int colMid = pointColumns / 2;
                            hObjectViewer1.ResetShowTexts();
                            if (count != pointRows * pointColumns)
                            {
                                hObjectViewer1.ResetWndCtrl(false);
                                ReduceImage.Dispose();
                                GrayImage.Dispose();
                                return;
                            }
                            SortCircle(GrayImage, count, pointRows, pointColumns, rowMid);
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

        private void SortCircle(HRegion GrayImage, int count, int pointRows, int pointColumns, int rowMid)
        {
            //实际值
            //求最小外接矩形
            List<double> rows = new List<double>();
            List<double> cols = new List<double>();
            List<Circle> circles = new List<Circle>();
            for (int i = 0; i < count; i++)
            {
                rows.Add(GrayImage[i + 1].Row);
                cols.Add(GrayImage[i + 1].Column);
                circles.Add(new Circle(GrayImage[i + 1].Row, GrayImage[i + 1].Column, Math.Sqrt(GrayImage[i + 1].Area / Math.PI)));
            }
            //创建点集XLD
            HXLDCont hXLDCont = new HXLDCont(new HTuple(rows.ToArray()), new HTuple(cols.ToArray()));
            HTuple rectCenterX, rectCenterY, phi, length1, length2;//length1长边，length2短边
            hXLDCont.SmallestRectangle2Xld(out rectCenterY, out rectCenterX, out phi, out length1, out length2);//拟合最小外接矩形
            //HRegion rectangele = new HRegion();
            //rectangele.GenRectangle2(rectCenterY, rectCenterX, phi, length1, length2);
            //hObjectViewer1.ResetShowObjects();
            //ShowObject showObject = new ShowObject(rectangele.GenContourRegionXld("border"));
            //hObjectViewer1.AddShowObject(showObject);
            //实际值
            double width = 0;// = 2 * length1 / (pointColumns - 1);
            double height = 0;// 2 * length2 / (pointRows - 1);
            double angle = phi * 180 / Math.PI;
            if (angle >= 45)
            {
                angle = angle - 90;
                width = 2 * length2 / (pointColumns - 1);
                height = 2 * length1 / (pointRows - 1);
            }
            else if (angle <= -45)
            {
                angle = angle + 90;
                width = 2 * length2 / (pointColumns - 1);
                height = 2 * length1 / (pointRows - 1);
            }
            else
            {
                width = 2 * length1 / (pointColumns - 1);
                height = 2 * length2 / (pointRows - 1);
            }
            double centerX = 0;
            double centerY = 0;
            double radius = circles.Count > 0 ? circles[0].Radius : 0;
            HRegion rotateRect = new HRegion();//区域取点
            List<Circle> dealRowPoints;
            List<Circle> dealPoints = new List<Circle>();
            for (int i = 0; i < pointRows; i++)
            {
                rotateRect = new HRegion();
                dealRowPoints = new List<Circle>();
                centerX = rectCenterX + (i - rowMid) * height * Math.Sin(angle * Math.PI / 180);//求中心点
                centerY = rectCenterY + (i - rowMid) * height * Math.Cos(angle * Math.PI / 180);
                rotateRect.GenRectangle2(centerY, centerX, angle * Math.PI / 180, width * (pointColumns - 1) / 2 + radius, height / 2);//生成取点区域
                //hObjectViewer1.AddShowObject(new ShowObject(rotateRect.GenContourRegionXld("border"), "blue"));
                for (int j = 0; j < circles.Count; j++)
                {
                    HTuple index = rotateRect.GetRegionIndex((int)circles[j].Row, (int)circles[j].Column);//点在区域内
                    if ((index.Length != 0) && (index.I == 1))
                    {
                        dealRowPoints.Add(circles[j]);
                    }
                }
                dealRowPoints.Sort(ComparePoint);//排序
                for (int k = 0; k < dealRowPoints.Count; k++)
                {
                    dealPoints.Add(dealRowPoints[k]);
                }
            }
            //for (int i = 0; i < dealPoints.Count; i++)
            //{
            //    HRegion hRegion = new HRegion(dealPoints[i].Row, dealPoints[i].Column, 10);
            //    hObjectViewer1.AddShowObject(new ShowObject(hRegion, "white", "fill"));
            //}
            pixelCoordinatesX = new List<double>();
            pixelCoordinatesY = new List<double>();
            for (int i = 0; i < dealPoints.Count; i++)
            {
                pixelCoordinatesX.Add(dealPoints[i].Column);
                pixelCoordinatesY.Add(dealPoints[i].Row);
                ShowText showText = new ShowText(dealPoints[i].Column, dealPoints[i].Row, i.ToString());
                hObjectViewer1.AddShowText(showText);
            }
            hXLDCont.Dispose();
            rotateRect.Dispose();
        }

        private int ComparePoint(Circle point1, Circle point2)
        {
            if (point1.Column < point2.Column)
            {
                return -1;
            }
            else if (point1.Column == point2.Column)
            {
                return 0;
            }
            else
            {
                return 1;
            }
        }

        private void trackBarAreaMin_ValueChanged(object sender, EventArgs e)
        {
            if (trackBarAreaMin.Value >= trackBarAreaMax.Value)
            {
                trackBarAreaMax.Value = trackBarAreaMin.Value;
            }
            lbAreaMin.Text = trackBarAreaMin.Value.ToString();
            ImageThreshold();
        }

        private void trackBarAreaMax_ValueChanged(object sender, EventArgs e)
        {
            if (trackBarAreaMax.Value <= trackBarAreaMin.Value)
            {
                trackBarAreaMin.Value = trackBarAreaMax.Value;
            }
            lbAreaMax.Text = trackBarAreaMax.Value.ToString();
            ImageThreshold();
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
            if (Convert.ToInt32(labelAutoCircle.Text) != numericUpDownRow.Value * numericUpDownCol.Value)
            {
                MessageBox.Show("查找圆数量不正确");
                return;
            }

            //理论值
            int pointRows = Convert.ToInt32(numericUpDownRow.Value);
            int pointColumns = Convert.ToInt32(numericUpDownCol.Value);
            double leftTopX = Convert.ToDouble(numericUpDownLeftX.Value);
            double leftTopY = Convert.ToDouble(numericUpDownLeftY.Value);
            double distX = Convert.ToDouble(numericUpDownDisX.Value);
            double distY = Convert.ToDouble(numericUpDownDisY.Value);
            realWorldCoordinatesX = new List<double>();
            realWorldCoordinatesY = new List<double>();
            for (int i = 0; i < pointRows; i++)
            {
                for (int j = 0; j < pointColumns; j++)
                {
                    realWorldCoordinatesX.Add(leftTopX + j * distX);
                    realWorldCoordinatesY.Add(leftTopY + i * distY);
                }
            }
            if ((pixelCoordinatesX.Count != 0) && (pixelCoordinatesX.Count >= 3) && (pixelCoordinatesX.Count == realWorldCoordinatesX.Count))
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

        private void button4_Click(object sender, EventArgs e)
        {
            pixelCoordinatesX.Clear();
            pixelCoordinatesY.Clear();
            realWorldCoordinatesX.Clear();
            realWorldCoordinatesY.Clear();
        }

        private void AutoCircleCalibrationForm_FormClosing(object sender, FormClosingEventArgs e)
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
