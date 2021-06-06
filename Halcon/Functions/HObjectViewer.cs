using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using HalconDotNet;
using static HalconDotNet.HDrawingObject;
using System.IO;

namespace Halcon.Functions
{
    public partial class HObjectViewer : UserControl
    {
        private bool m_bIsShowToolbar = true;
        [Category("自定义"), Description("是否展示工具栏"), Browsable(true)]
        public bool ShowToolbar
        {
            get { return m_bIsShowToolbar; }
            set
            {
                m_bIsShowToolbar = value;
                this.tableLayoutPanel1.RowStyles[0].Height = m_bIsShowToolbar ? 30F : 0F;
                this.tableLayoutPanel1.Height = m_bIsShowToolbar ? 60 : 30;
            }
        }

        private ViewerTools m_activeTool = ViewerTools.Arrow;
        [Category("DataBindings"), Description("默认激活工具"), Browsable(true)]
        public ViewerTools ActiveTool
        {
            get { return m_activeTool; }
            set
            {
                m_activeTool = value;
                if (OnToolChanged != null)
                {
                    OnToolChanged(null, new ToolEventArgs(m_activeTool));
                }
            }
        }

        private HTuple m_windowID, m_imageWidth, m_imageHeight;
        private double m_rowMouseDown;//鼠标按下时的行坐标
        private double m_colMouseDown;//鼠标按下时的列坐标
        private HImage m_sourceImage;//图像变量
        public HImage SourceImage
        {
            get { return m_sourceImage; }
            set
            {
                if (m_sourceImage != null)
                {
                    m_sourceImage.Dispose();
                }
                m_sourceImage = value;
                if (m_sourceImage != null)
                {
                    HTuple channel;
                    HOperatorSet.CountChannels(m_sourceImage, out channel);
                    toolStripStatusLabelChannel.Text = channel.I.ToString();
                    HOperatorSet.GetImageSize(m_sourceImage, out m_imageWidth, out m_imageHeight);//获取图像大小
                    m_dbCrossRow = m_imageHeight / 2;
                    m_dbCrossCol = m_imageWidth / 2;
                }
                ResetWndCtrl(true);
            }
        }

        private bool m_bIsShowCross = false;

        public bool IsShowCross
        {
            get { return m_bIsShowCross; }
            set
            {
                m_bIsShowCross = value;
                ResetWndCtrl(false);
            }
        }

        private bool m_bCanImageMove = false;
        private double m_dbCrossRow = 0;
        private double m_dbCrossCol = 0;
        private int m_nSelectedRoi = -1;
        private bool m_bIsROISelected = false;
        private object m_showImgLock = new object();
        private List<ToolStripButton> m_listTools = new List<ToolStripButton>();//工具控件          
        private List<ShowObject> m_listShowObjects = new List<ShowObject>();//显示对象
        private List<ShowText> m_listShowTexts = new List<ShowText>();//显示文字             
        private ROIManager m_roiManager = new ROIManager();//ROI集合
        private ROIStatus m_ROIStatus = ROIStatus.UNION;
        private HDrawingObjectCallbackClass OnDragDrawingObject = null;
        private HDrawingObjectCallbackClass OnAttachDrawingObject = null;
        private HDrawingObjectCallbackClass OnResizeDrawingObject = null;
        private Action ExecuteImageHandle;

        public bool IsSetCross { get; set; } = false;
        public HWindowControl ViewerInstance { get => ViewerControl; }

        //图像改变事件
        public delegate void ImageChangedEventHandler(bool isZoom, EventArgs e);
        public event ImageChangedEventHandler OnImageChanged;

        public delegate void ToolChangedEventHandler(object sender, ToolEventArgs e);
        public event ToolChangedEventHandler OnToolChanged;

        public HDrawingObjectCallbackClass OnSelectDrawingObject = null;

        private void Image_ChangeEvent(bool isZoom, EventArgs e)
        {
            try
            {
                if (SourceImage != null)
                {
                    HOperatorSet.ClearWindow(m_windowID);//清空窗体
                    toolStripStatusLabelSize.Text = m_imageWidth.I.ToString() + "*" + m_imageHeight.I.ToString();
                    HTuple row1, column1, row2, column2;
                    if (isZoom)
                    {
                        ZoomToFit(out row1, out column1, out row2, out column2);
                    }
                    else
                    {
                        HOperatorSet.GetPart(m_windowID, out row1, out column1, out row2, out column2);//得到当前的窗口坐标
                    }
                    HOperatorSet.SetPart(m_windowID, row1, column1, row2, column2);//设置显示在窗体的图像大小
                    HOperatorSet.DispObj(m_sourceImage, m_windowID);//显示图像
                    if (m_bIsShowCross)
                    {
                        HOperatorSet.SetColor(m_windowID, "blue");
                        HOperatorSet.DispLine(m_windowID, 0, m_dbCrossCol, m_imageHeight, m_dbCrossCol);
                        HOperatorSet.DispLine(m_windowID, m_dbCrossRow, 0, m_dbCrossRow, m_imageWidth);
                    }
                    RefreshShowObjects();
                    RefreshShowTexts();
                }
            }
            catch (HOperatorException he)
            { }
        }

        private void ZoomToFit(out HTuple row1, out HTuple column1, out HTuple row2, out HTuple column2)
        {
            double ratioWidth = (1.0) * m_imageWidth[0].I / ViewerControl.Width;
            double ratioHeight = (1.0) * m_imageHeight[0].I / ViewerControl.Height;

            row1 = 0;
            column1 = 0;
            row2 = 0;
            column2 = 0;
            //if ((ratioWidth >= 1) || (ratioHeight >= 1))
            //{
            if (ratioWidth >= ratioHeight)
            {
                double overSize = ((ViewerControl.Height * ratioWidth) - m_imageHeight) / 2;
                row1 = -overSize;
                column1 = 0;
                row2 = m_imageHeight + overSize;
                column2 = m_imageWidth - 1;
            }
            else
            {
                double overSize = ((ViewerControl.Width * ratioHeight) - m_imageWidth) / 2;
                row1 = 0;
                column1 = -overSize;
                row2 = m_imageHeight - 1;
                column2 = m_imageWidth + overSize;
            }
            //}
            //else
            //{
            //    if (ratioWidth >= ratioHeight)
            //    {
            //        double overSize = ((ViewerControl.Height * ratioWidth) - ImageHeight) / 2;
            //        row1 = -overSize;
            //        column1 = 0;
            //        row2 = ImageHeight + overSize;
            //        column2 = ImageWidth - 1;
            //    }
            //    else
            //    {
            //        double overSize = ((ViewerControl.Width * ratioHeight) - ImageWidth) / 2;
            //        row1 = 0;
            //        column1 = -overSize;
            //        row2 = ImageHeight - 1;
            //        column2 = ImageWidth + overSize;
            //    }
            //}
        }

        public HObjectViewer()
        {
            InitializeComponent();
            try
            {
                HOperatorSet.SetSystem("graphic_stack_size", 100000);//增加堆栈存储内存
                m_windowID = ViewerInstance.HalconWindow;
                OnImageChanged += Image_ChangeEvent;
                OnToolChanged += Tool_ChangeEvent;
                OnSelectDrawingObject += OnSelectObject;
            }
            catch
            {
            }
        }

        private void ViewerControl_HMouseDown(object sender, HalconDotNet.HMouseEventArgs e)
        {
            if (m_imageHeight != null)
            {
                if (e.Button == MouseButtons.Middle)
                {
                    if (!m_bCanImageMove)
                    {
                        m_bCanImageMove = true;
                    }
                    try
                    {
                        HTuple Row, Column, Button;
                        HOperatorSet.GetMposition(m_windowID, out Row, out Column, out Button);//获取当前鼠标坐标
                        m_rowMouseDown = Row;    //鼠标按下时的行坐标
                        m_colMouseDown = Column; //鼠标按下时的列坐标
                    }
                    catch
                    { }
                }
                else if (e.Button == MouseButtons.Left)
                {
                    if (ActiveTool == ViewerTools.Circle)
                    {
                        HTuple Row, Column, Button;
                        HOperatorSet.GetMposition(m_windowID, out Row, out Column, out Button);//获取当前鼠标坐标
                        HDrawingObject hDrawingObject = HDrawingObject.CreateDrawingObject(HDrawingObject.HDrawingObjectType.CIRCLE, Row.D, Column.D, 70);
                        ROIBase rOIBase = new ROICircle() { DrawingObject = hDrawingObject, Status = m_ROIStatus };
                        AttachDrawObj(rOIBase);
                        ActiveTool = ViewerTools.Arrow;
                    }
                    else if (ActiveTool == ViewerTools.Ellipse)
                    {
                        HTuple Row, Column, Button;
                        HOperatorSet.GetMposition(m_windowID, out Row, out Column, out Button);//获取当前鼠标坐标
                        HDrawingObject hDrawingObject = HDrawingObject.CreateDrawingObject(HDrawingObject.HDrawingObjectType.ELLIPSE, Row.D, Column.D, 0, 70, 70);
                        ROIBase rOIBase = new ROIEllipse() { DrawingObject = hDrawingObject, Status = m_ROIStatus };
                        AttachDrawObj(rOIBase);
                        ActiveTool = ViewerTools.Arrow;
                    }
                    else if (ActiveTool == ViewerTools.RotateRectangle)
                    {
                        HTuple Row, Column, Button;
                        HOperatorSet.GetMposition(m_windowID, out Row, out Column, out Button);//获取当前鼠标坐标
                        HDrawingObject hDrawingObject = HDrawingObject.CreateDrawingObject(HDrawingObject.HDrawingObjectType.RECTANGLE2, Row.D, Column.D, 0, 70, 70);
                        ROIBase rOIBase = new ROIRotateRectangle() { DrawingObject = hDrawingObject, Status = m_ROIStatus };
                        AttachDrawObj(rOIBase);
                        ActiveTool = ViewerTools.Arrow;
                    }
                    else if (ActiveTool == ViewerTools.Rectangle)
                    {
                        HTuple Row, Column, Button;
                        HOperatorSet.GetMposition(m_windowID, out Row, out Column, out Button);//获取当前鼠标坐标
                        HDrawingObject hDrawingObject = HDrawingObject.CreateDrawingObject(HDrawingObject.HDrawingObjectType.RECTANGLE1, Row.D - 50, Column.D - 50, Row.D + 50, Column.D + 50);
                        ROIBase rOIBase = new ROIRectangle() { DrawingObject = hDrawingObject, Status = m_ROIStatus };
                        AttachDrawObj(rOIBase);
                        ActiveTool = ViewerTools.Arrow;
                    }

                    if (IsSetCross)
                    {
                        HTuple Row, Column, Button;
                        HOperatorSet.GetMposition(m_windowID, out Row, out Column, out Button);//获取当前鼠标坐标
                        m_dbCrossRow = Row.D;
                        m_dbCrossCol = Column.D;
                        ResetWndCtrl(false);
                    }
                }
            }
        }

        private void ViewerControl_HMouseMove(object sender, HalconDotNet.HMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Middle)
            {
                if (m_bCanImageMove)
                {
                    if (m_imageHeight != null)
                    {
                        try
                        {
                            HOperatorSet.SetSystem("flush_graphic", "false");
                            HTuple row1, col1, row2, col2, Row, Column, Button;
                            HOperatorSet.GetMposition(m_windowID, out Row, out Column, out Button);
                            double RowMove = Row - m_rowMouseDown;   //鼠标移动时的行坐标减去按下时的行坐标，得到行坐标的移动值
                            double ColMove = Column - m_colMouseDown;//鼠标移动时的列坐标减去按下时的列坐标，得到列坐标的移动值
                            HOperatorSet.GetPart(m_windowID, out row1, out col1, out row2, out col2);//得到当前的窗口坐标
                            HOperatorSet.SetPart(m_windowID, row1 - RowMove, col1 - ColMove, row2 - RowMove, col2 - ColMove);
                            HOperatorSet.ClearWindow(m_windowID);
                            HOperatorSet.SetSystem("flush_graphic", "true");
                            HOperatorSet.DispObj(m_sourceImage, m_windowID);
                            if (m_bIsShowCross)
                            {
                                HOperatorSet.SetColor(m_windowID, "blue");
                                HOperatorSet.DispLine(m_windowID, 0, m_dbCrossCol, m_imageHeight, m_dbCrossCol);
                                HOperatorSet.DispLine(m_windowID, m_dbCrossRow, 0, m_dbCrossRow, m_imageWidth);
                            }
                            RefreshShowObjects();
                            RefreshShowTexts();
                        }
                        catch
                        { }
                    }
                    else
                    {
                        MessageBox.Show("请加载一张图片");
                    }
                }
            }
            if (m_imageHeight != null)
            {
                try
                {
                    HTuple Row, Column, Button, pointGray;
                    HOperatorSet.GetMposition(m_windowID, out Row, out Column, out Button);//获取当前鼠标的坐标值
                    if (m_imageHeight != null && (Row > 0 && Row < m_imageHeight) && (Column > 0 && Column < m_imageWidth))//判断鼠标在图像上
                    {
                        HOperatorSet.GetGrayval(m_sourceImage, Row, Column, out pointGray);//获取当前点的灰度值
                        toolStripStatusLabelCoordinate.Text = Column.ToString() + "," + Row.ToString();
                        toolStripStatusLabelValue.Text = pointGray.ToString();
                    }
                    else
                    {
                        pointGray = "_";
                    }
                }
                catch
                { }
            }
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            toolStripMenuItemZoom.Enabled = (m_sourceImage != null);
            toolStripMenuItemSave.Enabled = (m_sourceImage != null);
            toolStripMenuItemSaveWindow.Enabled = (m_sourceImage != null);
            toolStripMenuItemRegistered.Enabled = (m_sourceImage != null);
        }

        private void toolStripMenuItemZoom_Click(object sender, EventArgs e)
        {
            ResetWndCtrl(true);
        }

        private void toolStripMenuItemOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "BMP File|*.bmp|PNG File|*.png|JPEG File|*.jpg|All|*.*";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                if (!string.IsNullOrEmpty(ofd.FileName))
                {
                    if (m_sourceImage != null)
                    {
                        m_sourceImage.Dispose();
                    }
                    //HOperatorSet.ReadImage(out source_Image, ofd.FileName);
                    m_sourceImage = new HImage(ofd.FileName);
                    if (m_sourceImage != null)
                    {
                        HTuple channel;
                        HOperatorSet.CountChannels(m_sourceImage, out channel);
                        toolStripStatusLabelChannel.Text = channel.I.ToString();
                        HOperatorSet.GetImageSize(m_sourceImage, out m_imageWidth, out m_imageHeight);//获取图像大小
                        m_dbCrossRow = m_imageHeight / 2;
                        m_dbCrossCol = m_imageWidth / 2;
                    }
                    ResetWndCtrl(true);
                }
            }
        }

        private void toolStripMenuItemSave_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "BMP File|*.bmp|PNG File|*.png|JPEG File|*.jpg|All|*.*";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                if (!string.IsNullOrEmpty(sfd.FileName))
                {
                    HOperatorSet.WriteImage(m_sourceImage, Path.GetExtension(sfd.FileName).Substring(1), 0, sfd.FileName);
                }
            }
        }

        private void ViewerControl_HMouseUp(object sender, HalconDotNet.HMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Middle)
            {
                if (m_bCanImageMove)
                {
                    m_bCanImageMove = !m_bCanImageMove;
                }
            }
            else if (e.Button == MouseButtons.Left)
            {
                if (m_bIsROISelected)
                {
                    if (ExecuteImageHandle != null)
                    {
                        ExecuteImageHandle.Invoke();
                    }
                    m_bIsROISelected = false;
                }
            }
        }

        /// <summary>
        /// 放大缩小图像
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ViewerControl_HMouseWheel(object sender, HalconDotNet.HMouseEventArgs e)
        {
            if (m_sourceImage != null)
            {
                try
                {
                    HTuple Zoom, MouseRow, MouseCol, Button;
                    HTuple ImageLeftRow, ImageLeftCol, ImageRightRow, ImageRightCol, DisplayHeight, DisplayWidth, DisplayLeftRow, DisplayLeftCol, DisplayRightRow, DisplayRightCol;
                    if (e.Delta > 0)
                    {
                        Zoom = 1.5;
                    }
                    else
                    {
                        Zoom = 0.5;
                    }
                    HOperatorSet.GetMposition(m_windowID, out MouseRow, out MouseCol, out Button);//获取当前鼠标位置
                    HOperatorSet.GetPart(m_windowID, out ImageLeftRow, out ImageLeftCol, out ImageRightRow, out ImageRightCol);//获取显示在窗口中的图像内容像素位置
                    DisplayHeight = ImageRightRow - ImageLeftRow;
                    DisplayWidth = ImageRightCol - ImageLeftCol;
                    if (DisplayHeight * DisplayWidth < 32000 * 32000 || Zoom == 1.5)//普通版halcon能处理的图像最大尺寸是32K*32K。如果无限缩小原图像，导致显示的图像超出限制，则会造成程序崩溃
                    {
                        DisplayLeftRow = (ImageLeftRow + ((1 - (1.0 / Zoom)) * (MouseRow - ImageLeftRow)));
                        DisplayLeftCol = (ImageLeftCol + ((1 - (1.0 / Zoom)) * (MouseCol - ImageLeftCol)));
                        DisplayRightRow = DisplayLeftRow + (DisplayHeight / Zoom);
                        DisplayRightCol = DisplayLeftCol + (DisplayWidth / Zoom);
                        HOperatorSet.SetPart(m_windowID, DisplayLeftRow, DisplayLeftCol, DisplayRightRow, DisplayRightCol);
                        HOperatorSet.ClearWindow(m_windowID);
                        HOperatorSet.DispObj(m_sourceImage, m_windowID);
                        if (m_bIsShowCross)
                        {
                            HOperatorSet.SetColor(m_windowID, "blue");
                            HOperatorSet.DispLine(m_windowID, 0, m_dbCrossCol, m_imageHeight, m_dbCrossCol);
                            HOperatorSet.DispLine(m_windowID, m_dbCrossRow, 0, m_dbCrossRow, m_imageWidth);
                        }
                        RefreshShowObjects();
                        RefreshShowTexts();
                    }
                }
                catch (HOperatorException he)
                { }
            }
        }

        private void tsbActiveTool_Click(object sender, EventArgs e)
        {
            ToolStripButton tsb = sender as ToolStripButton;
            if (tsb != null)
            {
                string toolName = tsb.Name.Replace("tsb", "");
                ActiveTool = (ViewerTools)Enum.Parse(typeof(ViewerTools), toolName);
            }
        }

        private void HObjectViewer_Load(object sender, EventArgs e)
        {
            m_listTools.AddRange(new List<ToolStripButton>() { tsbArrow, tsbZoomIn, tsbZoomOut, tsbHand, tsbRectangle, tsbRotateRectangle, tsbCircle, tsbEllipse });
            for (int i = 0; i < m_listTools.Count; i++)
            {
                m_listTools[i].ToolTipText = m_listTools[i].Name.Replace("tsb", "");
            }
        }

        private void ViewerControl_Resize(object sender, EventArgs e)
        {
            ResetWndCtrl(true);
        }

        private void Tool_ChangeEvent(object sender, ToolEventArgs e)
        {
            for (int i = 0; i < m_listTools.Count; i++)
            {
                m_listTools[i].BackColor = Color.Transparent;
            }
            if (toolStrip1.Items["tsb" + e.ActiveTool.ToString()] != null)
            {
                toolStrip1.Items["tsb" + e.ActiveTool.ToString()].BackColor = Color.PeachPuff;
            }
        }

        public void SetImage(HImage hObject, bool isHorizon = false, bool isVertical = false, bool isRotate = false)
        {
            try
            {
                if (m_sourceImage != null)
                {
                    m_sourceImage.Dispose();
                }
                if (isHorizon)
                {
                    //HOperatorSet.MirrorImage(hObject, out hObject, "row");
                    hObject.MirrorImage("row");
                }
                if (isVertical)
                {
                    //HOperatorSet.MirrorImage(hObject, out hObject, "column");
                    hObject.MirrorImage("column");
                }
                if (isRotate)
                {
                    //HOperatorSet.RotateImage(hObject, out hObject, 90, "constant");
                    hObject.RotateImage(new HTuple(90), "constant");
                }
                SourceImage = hObject.Clone();
                //hObject.Dispose();
            }
            catch (HOperatorException he)
            {
                //MessageBox.Show(he.Message);
            }
        }

        public void AttachDrawObj(ROIBase obj)
        {
            if (m_roiManager.AddROI(obj))
            {
                obj.DrawingObject.SetDrawingObjectParams("color", "yellow");
                if (OnDragDrawingObject != null)
                {
                    obj.DrawingObject.OnDrag(OnDragDrawingObject);
                }
                if (OnAttachDrawingObject != null)
                {
                    obj.DrawingObject.OnAttach(OnAttachDrawingObject);
                }
                if (OnResizeDrawingObject != null)
                {
                    obj.DrawingObject.OnResize(OnResizeDrawingObject);
                }
                if (OnSelectDrawingObject != null)
                {
                    obj.DrawingObject.OnSelect(OnSelectDrawingObject);
                }
                ViewerControl.HalconWindow.AttachDrawingObjectToWindow(obj.DrawingObject);
                //HOperatorSet.AttachDrawingObjectToWindow(WindowID, obj.m_DrawingObject);
                if (m_sourceImage != null)
                {
                    HOperatorSet.DispObj(m_sourceImage, m_windowID);
                }
            }
        }

        private void SetImageHandle(HDrawingObject.HDrawingObjectCallbackClass hDrawingObjectCallbackClass)
        {
            OnDragDrawingObject += hDrawingObjectCallbackClass;
            OnResizeDrawingObject += hDrawingObjectCallbackClass;
        }

        public void SetImageHandle(Action action)
        {
            ExecuteImageHandle += action;
        }

        //临时解决控件尺寸变化报错
        private void HObjectViewer_SizeChanged(object sender, EventArgs e)
        {
            try
            {
                ViewerControl.Size = new Size(this.Width, this.Height - tableLayoutPanel1.Height);
            }
            catch
            { }
        }

        private void tsbClear_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < m_roiManager.ROICount; i++)
            {
                HOperatorSet.DetachDrawingObjectFromWindow(m_windowID, m_roiManager.GetROIByIndex(i).DrawingObject);
            }
            m_roiManager.ResetROIs();
        }

        private void tsbRemove_Click(object sender, EventArgs e)
        {
            if (m_nSelectedRoi != -1)
            {
                HOperatorSet.DetachDrawingObjectFromWindow(m_windowID, m_nSelectedRoi);
                m_roiManager.DeleteROI(m_nSelectedRoi);
                m_nSelectedRoi = -1;
            }
        }

        private void toolStripMenuItemSaveWindow_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "BMP File|*.bmp|PNG File|*.png|JPEG File|*.jpg|All|*.*";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                if (!string.IsNullOrEmpty(sfd.FileName))
                {
                    HObject hObject;
                    HOperatorSet.DumpWindowImage(out hObject, m_windowID);
                    HOperatorSet.WriteImage(hObject, Path.GetExtension(sfd.FileName).Substring(1), 0, sfd.FileName);
                    hObject.Dispose();
                }
            }
        }

        private void OnSelectObject(HDrawingObject dobj, HWindow hwin, string type)
        {
            for (int i = 0; i < m_roiManager.ROICount; i++)
            {
                m_roiManager.GetROIByIndex(i).DrawingObject.SetDrawingObjectParams("color", "yellow");
            }
            dobj.SetDrawingObjectParams("color", "green");
            m_nSelectedRoi = Convert.ToInt32(dobj.ID);
            m_bIsROISelected = true;
        }

        public void ResetShowObjects()
        {
            lock (m_showImgLock)
            {
                for (int i = 0; i < m_listShowObjects.Count; i++)
                {
                    m_listShowObjects[i].ShowHObject.Dispose();
                }
                m_listShowObjects = new List<ShowObject>();
            }
        }

        public void SetShowObjects(List<ShowObject> objects)
        {
            lock (m_showImgLock)
            {
                for (int i = 0; i < m_listShowObjects.Count; i++)
                {
                    m_listShowObjects[i].ShowHObject.Dispose();
                }
                m_listShowObjects = objects;
            }
        }

        public void AddShowObject(ShowObject hObject)
        {
            lock (m_showImgLock)
            {
                m_listShowObjects.Add(hObject);
            }
        }

        public void RefreshShowObjects()
        {
            for (int i = 0; i < m_listShowObjects.Count; i++)
            {
                HOperatorSet.SetColor(m_windowID, m_listShowObjects[i].ShowColor);
                HOperatorSet.SetDraw(m_windowID, m_listShowObjects[i].DrawMode);
                HOperatorSet.DispObj(m_listShowObjects[i].ShowHObject, m_windowID);
            }
        }

        public void ResetShowTexts()
        {
            lock (m_showImgLock)
            {
                m_listShowTexts = new List<ShowText>();
            }
        }

        public void SetTexts(List<ShowText> objects)
        {
            lock (m_showImgLock)
            {
                m_listShowTexts = objects;
            }
        }

        public void AddShowText(ShowText hObject)
        {
            lock (m_showImgLock)
            {
                m_listShowTexts.Add(hObject);
            }
        }

        public void RefreshShowTexts()
        {
            for (int i = 0; i < m_listShowTexts.Count; i++)
            {
                HOperatorSet.SetColor(m_windowID, m_listShowTexts[i].ShowColor);
                HOperatorSet.SetTposition(m_windowID, m_listShowTexts[i].PositionY, m_listShowTexts[i].PositionX);
                HOperatorSet.WriteString(m_windowID, m_listShowTexts[i].ShowContent);
            }
        }

        public void SetROICount(int count)
        {
            m_roiManager.SetMaxCount(count);
        }

        public void ResetROIs()
        {
            m_roiManager.ResetROIs();
        }

        public void SetROIs(List<ROIBase> rois)
        {
            m_roiManager.SetROIs(rois);
        }

        public List<ROIBase> GetROIs()
        {
            for (int i = 0; i < m_roiManager.ROICount; i++)
            {
                m_roiManager.GetROIByIndex(i).GenerateParameter();
            }
            return m_roiManager.GetROIs();
        }

        public void AddROI(ROIBase roi)
        {
            m_roiManager.AddROI(roi);
        }

        private void toolStripMenuItemUNION_Click(object sender, EventArgs e)
        {
            toolStripDropDownButtonStatus.Image = Properties.Resources.UNION;
            m_ROIStatus = ROIStatus.UNION;
        }

        private void toolStripMenuItemINTERSECTION_Click(object sender, EventArgs e)
        {
            toolStripDropDownButtonStatus.Image = Properties.Resources.INTERSECTION;
            m_ROIStatus = ROIStatus.INTERSECTION;
        }

        private void toolStripMenuItemDIFFERENCE_Click(object sender, EventArgs e)
        {
            toolStripDropDownButtonStatus.Image = Properties.Resources.DIFFERENCE;
            m_ROIStatus = ROIStatus.DIFFERENCE;
        }

        public void DeleteROI(int index)
        {
            m_roiManager.DeleteROI(index);
        }

        public HRegion GetRegions()
        {
            return m_roiManager.GetRegions();
        }

        public void ResetWndCtrl(bool isZoom)
        {
            lock (m_showImgLock)
            {
                if (OnImageChanged != null)
                {
                    ViewerControl.BeginInvoke((MethodInvoker)delegate ()
                    {
                        OnImageChanged(isZoom, new EventArgs());
                    });
                }
            }
        }

        private void toolStripMenuItemRegistered_Click(object sender, EventArgs e)
        {
            if (!Directory.Exists(Application.StartupPath + @"\ModelImage"))
            {
                Directory.CreateDirectory(Application.StartupPath + @"\ModelImage");
            }
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.InitialDirectory = Application.StartupPath + @"\ModelImage";
            sfd.Filter = "PNG File|*.png";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                if (!string.IsNullOrEmpty(sfd.FileName))
                {
                    HOperatorSet.WriteImage(m_sourceImage, "png", 0, sfd.FileName);
                }
            }
        }

        ~HObjectViewer()
        {
            ReleaseRam();
        }

        public void ReleaseRam()
        {
            ViewerControl.Dispose();
            for (int i = 0; i < m_listShowObjects.Count; i++)
            {
                m_listShowObjects[i].ShowHObject.Dispose();
            }
            for (int i = 0; i < m_roiManager.ROICount; i++)
            {
                m_roiManager.GetROIByIndex(i).DrawingObject.Dispose();
            }
            if (m_sourceImage != null)
            {
                m_sourceImage.Dispose();
            }
            if (SourceImage != null)
            {
                SourceImage.Dispose();
            }
            this.Dispose();
            GC.Collect();
        }
    }

    public class ToolEventArgs
    {
        public ViewerTools ActiveTool = ViewerTools.Arrow;

        public ToolEventArgs(ViewerTools tool)
        {
            ActiveTool = tool;
        }
    }
}
