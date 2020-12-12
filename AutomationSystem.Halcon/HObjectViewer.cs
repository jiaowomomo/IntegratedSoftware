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

namespace AutomationSystem.Halcon
{
    public partial class HObjectViewer : UserControl
    {
        private bool _ShowToolbar = true;
        [Category("自定义"), Description("是否展示工具栏"), Browsable(true)]
        public bool ShowToolbar
        {
            get { return _ShowToolbar; }
            set
            {
                _ShowToolbar = value;
                this.tableLayoutPanel1.RowStyles[0].Height = _ShowToolbar ? 30F : 0F;
                this.tableLayoutPanel1.Height = _ShowToolbar ? 60 : 30;
            }
        }

        HTuple WindowID, ImageWidth, ImageHeight;
        private double RowDown;//鼠标按下时的行坐标
        private double ColDown;//鼠标按下时的列坐标
        private HImage source_Image;//图像变量
        public HImage Source_Image
        {
            get { return source_Image; }
            set
            {
                if (source_Image != null)
                {
                    source_Image.Dispose();
                }
                source_Image = value;
                if (source_Image != null)
                {
                    HTuple channel;
                    HOperatorSet.CountChannels(source_Image, out channel);
                    toolStripStatusLabelChannel.Text = channel.I.ToString();
                    HOperatorSet.GetImageSize(source_Image, out ImageWidth, out ImageHeight);//获取图像大小
                    CrossRow = ImageHeight / 2;
                    CrossCol = ImageWidth / 2;
                }
                ResetWndCtrl(true);
            }
        }

        private bool _IsShowCross = false;

        public bool IsShowCross
        {
            get { return _IsShowCross; }
            set
            {
                _IsShowCross = value;
                ResetWndCtrl(false);
            }
        }

        public bool IsSetCross = false;
        private double CrossRow = 0;
        private double CrossCol = 0;

        public HWindowControl ViewerInstance
        {
            get
            {
                return ViewerControl;
            }
        }

        private bool CanImageMove = false;

        //图像改变事件
        public delegate void ImageChangedEventHandler(bool isZoom, EventArgs e);
        public event ImageChangedEventHandler OnImageChanged;

        private void Image_ChangeEvent(bool isZoom, EventArgs e)
        {

            try
            {
                if (Source_Image != null)
                {
                    HOperatorSet.ClearWindow(WindowID);//清空窗体
                    toolStripStatusLabelSize.Text = ImageWidth.I.ToString() + "*" + ImageHeight.I.ToString();
                    HTuple row1, column1, row2, column2;
                    if (isZoom)
                    {
                        ZoomToFit(out row1, out column1, out row2, out column2);
                    }
                    else
                    {
                        HOperatorSet.GetPart(WindowID, out row1, out column1, out row2, out column2);//得到当前的窗口坐标
                    }
                    HOperatorSet.SetPart(WindowID, row1, column1, row2, column2);//设置显示在窗体的图像大小
                    HOperatorSet.DispObj(source_Image, WindowID);//显示图像
                    if (_IsShowCross)
                    {
                        HOperatorSet.SetColor(WindowID, "blue");
                        HOperatorSet.DispLine(WindowID, 0, CrossCol, ImageHeight, CrossCol);
                        HOperatorSet.DispLine(WindowID, CrossRow, 0, CrossRow, ImageWidth);
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
            double ratioWidth = (1.0) * ImageWidth[0].I / ViewerControl.Width;
            double ratioHeight = (1.0) * ImageHeight[0].I / ViewerControl.Height;

            row1 = 0;
            column1 = 0;
            row2 = 0;
            column2 = 0;
            //if ((ratioWidth >= 1) || (ratioHeight >= 1))
            //{
            if (ratioWidth >= ratioHeight)
            {
                double overSize = ((ViewerControl.Height * ratioWidth) - ImageHeight) / 2;
                row1 = -overSize;
                column1 = 0;
                row2 = ImageHeight + overSize;
                column2 = ImageWidth - 1;
            }
            else
            {
                double overSize = ((ViewerControl.Width * ratioHeight) - ImageWidth) / 2;
                row1 = 0;
                column1 = -overSize;
                row2 = ImageHeight - 1;
                column2 = ImageWidth + overSize;
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
                WindowID = ViewerInstance.HalconWindow;
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
            if (ImageHeight != null)
            {
                if (e.Button == MouseButtons.Middle)
                {
                    if (!CanImageMove)
                    {
                        CanImageMove = true;
                    }
                    try
                    {
                        HTuple Row, Column, Button;
                        HOperatorSet.GetMposition(WindowID, out Row, out Column, out Button);//获取当前鼠标坐标
                        RowDown = Row;    //鼠标按下时的行坐标
                        ColDown = Column; //鼠标按下时的列坐标
                    }
                    catch
                    { }
                }
                else if (e.Button == MouseButtons.Left)
                {
                    if (ActiveTool == ViewerTools.Circle)
                    {
                        HTuple Row, Column, Button;
                        HOperatorSet.GetMposition(WindowID, out Row, out Column, out Button);//获取当前鼠标坐标
                        HDrawingObject hDrawingObject = HDrawingObject.CreateDrawingObject(HDrawingObject.HDrawingObjectType.CIRCLE, Row.D, Column.D, 70);
                        ROIBase rOIBase = new ROICircle() { m_DrawingObject = hDrawingObject, m_Status = m_ROIStatus };
                        AttachDrawObj(rOIBase);
                        ActiveTool = ViewerTools.Arrow;
                    }
                    else if (ActiveTool == ViewerTools.Ellipse)
                    {
                        HTuple Row, Column, Button;
                        HOperatorSet.GetMposition(WindowID, out Row, out Column, out Button);//获取当前鼠标坐标
                        HDrawingObject hDrawingObject = HDrawingObject.CreateDrawingObject(HDrawingObject.HDrawingObjectType.ELLIPSE, Row.D, Column.D, 0, 70, 70);
                        ROIBase rOIBase = new ROIEllipse() { m_DrawingObject = hDrawingObject, m_Status = m_ROIStatus };
                        AttachDrawObj(rOIBase);
                        ActiveTool = ViewerTools.Arrow;
                    }
                    else if (ActiveTool == ViewerTools.RotateRectangle)
                    {
                        HTuple Row, Column, Button;
                        HOperatorSet.GetMposition(WindowID, out Row, out Column, out Button);//获取当前鼠标坐标
                        HDrawingObject hDrawingObject = HDrawingObject.CreateDrawingObject(HDrawingObject.HDrawingObjectType.RECTANGLE2, Row.D, Column.D, 0, 70, 70);
                        ROIBase rOIBase = new ROIRotateRectangle() { m_DrawingObject = hDrawingObject, m_Status = m_ROIStatus };
                        AttachDrawObj(rOIBase);
                        ActiveTool = ViewerTools.Arrow;
                    }
                    else if (ActiveTool == ViewerTools.Rectangle)
                    {
                        HTuple Row, Column, Button;
                        HOperatorSet.GetMposition(WindowID, out Row, out Column, out Button);//获取当前鼠标坐标
                        HDrawingObject hDrawingObject = HDrawingObject.CreateDrawingObject(HDrawingObject.HDrawingObjectType.RECTANGLE1, Row.D - 50, Column.D - 50, Row.D + 50, Column.D + 50);
                        ROIBase rOIBase = new ROIRectangle() { m_DrawingObject = hDrawingObject, m_Status = m_ROIStatus };
                        AttachDrawObj(rOIBase);
                        ActiveTool = ViewerTools.Arrow;
                    }

                    if (IsSetCross)
                    {
                        HTuple Row, Column, Button;
                        HOperatorSet.GetMposition(WindowID, out Row, out Column, out Button);//获取当前鼠标坐标
                        CrossRow = Row.D;
                        CrossCol = Column.D;
                        ResetWndCtrl(false);
                    }
                }
            }
        }

        private void ViewerControl_HMouseMove(object sender, HalconDotNet.HMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Middle)
            {
                if (CanImageMove)
                {
                    if (ImageHeight != null)
                    {
                        try
                        {
                            HOperatorSet.SetSystem("flush_graphic", "false");
                            HTuple row1, col1, row2, col2, Row, Column, Button;
                            HOperatorSet.GetMposition(WindowID, out Row, out Column, out Button);
                            double RowMove = Row - RowDown;   //鼠标移动时的行坐标减去按下时的行坐标，得到行坐标的移动值
                            double ColMove = Column - ColDown;//鼠标移动时的列坐标减去按下时的列坐标，得到列坐标的移动值
                            HOperatorSet.GetPart(WindowID, out row1, out col1, out row2, out col2);//得到当前的窗口坐标
                            HOperatorSet.SetPart(WindowID, row1 - RowMove, col1 - ColMove, row2 - RowMove, col2 - ColMove);
                            HOperatorSet.ClearWindow(WindowID);
                            HOperatorSet.SetSystem("flush_graphic", "true");
                            HOperatorSet.DispObj(source_Image, WindowID);
                            if (_IsShowCross)
                            {
                                HOperatorSet.SetColor(WindowID, "blue");
                                HOperatorSet.DispLine(WindowID, 0, CrossCol, ImageHeight, CrossCol);
                                HOperatorSet.DispLine(WindowID, CrossRow, 0, CrossRow, ImageWidth);
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
            if (ImageHeight != null)
            {
                try
                {
                    HTuple Row, Column, Button, pointGray;
                    HOperatorSet.GetMposition(WindowID, out Row, out Column, out Button);//获取当前鼠标的坐标值
                    if (ImageHeight != null && (Row > 0 && Row < ImageHeight) && (Column > 0 && Column < ImageWidth))//判断鼠标在图像上
                    {
                        HOperatorSet.GetGrayval(source_Image, Row, Column, out pointGray);//获取当前点的灰度值
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
            toolStripMenuItemZoom.Enabled = (source_Image != null);
            toolStripMenuItemSave.Enabled = (source_Image != null);
            toolStripMenuItemSaveWindow.Enabled = (source_Image != null);
            toolStripMenuItemRegistered.Enabled = (source_Image != null);
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
                    if (source_Image != null)
                    {
                        source_Image.Dispose();
                    }
                    //HOperatorSet.ReadImage(out source_Image, ofd.FileName);
                    source_Image = new HImage(ofd.FileName);
                    if (source_Image != null)
                    {
                        HTuple channel;
                        HOperatorSet.CountChannels(source_Image, out channel);
                        toolStripStatusLabelChannel.Text = channel.I.ToString();
                        HOperatorSet.GetImageSize(source_Image, out ImageWidth, out ImageHeight);//获取图像大小
                        CrossRow = ImageHeight / 2;
                        CrossCol = ImageWidth / 2;
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
                    HOperatorSet.WriteImage(source_Image, Path.GetExtension(sfd.FileName).Substring(1), 0, sfd.FileName);
                }
            }
        }

        private void ViewerControl_HMouseUp(object sender, HalconDotNet.HMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Middle)
            {
                if (CanImageMove)
                {
                    CanImageMove = !CanImageMove;
                }
            }
            else if (e.Button == MouseButtons.Left)
            {
                if (IsROISelected)
                {
                    if (ExecuteImageHandle != null)
                    {
                        ExecuteImageHandle.Invoke();
                    }
                    IsROISelected = false;
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
            if (source_Image != null)
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
                    HOperatorSet.GetMposition(WindowID, out MouseRow, out MouseCol, out Button);//获取当前鼠标位置
                    HOperatorSet.GetPart(WindowID, out ImageLeftRow, out ImageLeftCol, out ImageRightRow, out ImageRightCol);//获取显示在窗口中的图像内容像素位置
                    DisplayHeight = ImageRightRow - ImageLeftRow;
                    DisplayWidth = ImageRightCol - ImageLeftCol;
                    if (DisplayHeight * DisplayWidth < 32000 * 32000 || Zoom == 1.5)//普通版halcon能处理的图像最大尺寸是32K*32K。如果无限缩小原图像，导致显示的图像超出限制，则会造成程序崩溃
                    {
                        DisplayLeftRow = (ImageLeftRow + ((1 - (1.0 / Zoom)) * (MouseRow - ImageLeftRow)));
                        DisplayLeftCol = (ImageLeftCol + ((1 - (1.0 / Zoom)) * (MouseCol - ImageLeftCol)));
                        DisplayRightRow = DisplayLeftRow + (DisplayHeight / Zoom);
                        DisplayRightCol = DisplayLeftCol + (DisplayWidth / Zoom);
                        HOperatorSet.SetPart(WindowID, DisplayLeftRow, DisplayLeftCol, DisplayRightRow, DisplayRightCol);
                        HOperatorSet.ClearWindow(WindowID);
                        HOperatorSet.DispObj(source_Image, WindowID);
                        if (_IsShowCross)
                        {
                            HOperatorSet.SetColor(WindowID, "blue");
                            HOperatorSet.DispLine(WindowID, 0, CrossCol, ImageHeight, CrossCol);
                            HOperatorSet.DispLine(WindowID, CrossRow, 0, CrossRow, ImageWidth);
                        }
                        RefreshShowObjects();
                        RefreshShowTexts();
                    }
                }
                catch (HOperatorException he)
                { }
            }
        }

        private List<ToolStripButton> m_listTools = new List<ToolStripButton>();//工具控件

        private ViewerTools _ActiveTool = ViewerTools.Arrow;
        [Category("DataBindings"), Description("默认激活工具"), Browsable(true)]
        public ViewerTools ActiveTool
        {
            get { return _ActiveTool; }
            set
            {
                _ActiveTool = value;
                if (OnToolChanged != null)
                {
                    OnToolChanged(null, new ToolEventArgs(_ActiveTool));
                }
            }
        }

        public delegate void ToolChangedEventHandler(object sender, ToolEventArgs e);

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

        public event ToolChangedEventHandler OnToolChanged;

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
                if (source_Image != null)
                {
                    source_Image.Dispose();
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
                Source_Image = hObject.Clone();
                //hObject.Dispose();
            }
            catch (HOperatorException he)
            {
                //MessageBox.Show(he.Message);
            }
        }

        public void AttachDrawObj(ROIBase obj)
        {
            if (roiManager.AddROI(obj))
            {
                obj.m_DrawingObject.SetDrawingObjectParams("color", "yellow");
                if (OnDragDrawingObject != null)
                {
                    obj.m_DrawingObject.OnDrag(OnDragDrawingObject);
                }
                if (OnAttachDrawingObject != null)
                {
                    obj.m_DrawingObject.OnAttach(OnAttachDrawingObject);
                }
                if (OnResizeDrawingObject != null)
                {
                    obj.m_DrawingObject.OnResize(OnResizeDrawingObject);
                }
                if (OnSelectDrawingObject != null)
                {
                    obj.m_DrawingObject.OnSelect(OnSelectDrawingObject);
                }
                ViewerControl.HalconWindow.AttachDrawingObjectToWindow(obj.m_DrawingObject);
                //HOperatorSet.AttachDrawingObjectToWindow(WindowID, obj.m_DrawingObject);
                if (source_Image != null)
                {
                    HOperatorSet.DispObj(source_Image, WindowID);
                }
            }
        }

        private HDrawingObjectCallbackClass OnDragDrawingObject = null;
        private HDrawingObjectCallbackClass OnAttachDrawingObject = null;
        private HDrawingObjectCallbackClass OnResizeDrawingObject = null;

        private void SetImageHandle(HDrawingObject.HDrawingObjectCallbackClass hDrawingObjectCallbackClass)
        {
            OnDragDrawingObject += hDrawingObjectCallbackClass;
            OnResizeDrawingObject += hDrawingObjectCallbackClass;
        }

        private Action ExecuteImageHandle;

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
            for (int i = 0; i < roiManager.GetROIs().Count; i++)
            {
                HOperatorSet.DetachDrawingObjectFromWindow(WindowID, roiManager.GetROIs()[i].m_DrawingObject);
            }
            roiManager.ResetROIs();
        }
        int nSelectRoi = -1;

        private void tsbRemove_Click(object sender, EventArgs e)
        {
            if (nSelectRoi != -1)
            {
                HOperatorSet.DetachDrawingObjectFromWindow(WindowID, nSelectRoi);
                roiManager.DeleteROI(nSelectRoi);
                nSelectRoi = -1;
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
                    HOperatorSet.DumpWindowImage(out hObject, WindowID);
                    HOperatorSet.WriteImage(hObject, Path.GetExtension(sfd.FileName).Substring(1), 0, sfd.FileName);
                    hObject.Dispose();
                }
            }
        }

        public HDrawingObjectCallbackClass OnSelectDrawingObject = null;
        private bool IsROISelected = false;

        private void OnSelectObject(HDrawingObject dobj, HWindow hwin, string type)
        {
            for (int i = 0; i < roiManager.GetROIs().Count; i++)
            {
                roiManager.GetROIs()[i].m_DrawingObject.SetDrawingObjectParams("color", "yellow");
            }
            dobj.SetDrawingObjectParams("color", "green");
            nSelectRoi = Convert.ToInt32(dobj.ID);
            IsROISelected = true;
        }

        //显示对象
        private List<ShowObject> showObjects = new List<ShowObject>();

        public void ResetShowObjects()
        {
            lock (showImgLock)
            {
                for (int i = 0; i < showObjects.Count; i++)
                {
                    showObjects[i].m_object.Dispose();
                }
                showObjects = new List<ShowObject>();
            }
        }

        public void SetShowObjects(List<ShowObject> objects)
        {
            lock (showImgLock)
            {
                for (int i = 0; i < showObjects.Count; i++)
                {
                    showObjects[i].m_object.Dispose();
                }
                showObjects = objects;
            }
        }

        object showImgLock = new object();

        public void AddShowObject(ShowObject hObject)
        {
            lock (showImgLock)
            {
                showObjects.Add(hObject);
            }
        }

        public void RefreshShowObjects()
        {
            for (int i = 0; i < showObjects.Count; i++)
            {
                HOperatorSet.SetColor(WindowID, showObjects[i].m_showColor);
                HOperatorSet.SetDraw(WindowID, showObjects[i].m_strDrawMode);
                HOperatorSet.DispObj(showObjects[i].m_object, WindowID);
            }
        }

        //显示文字
        private List<ShowText> showTexts = new List<ShowText>();

        public void ResetShowTexts()
        {
            lock (showImgLock)
            {
                showTexts = new List<ShowText>();
            }
        }

        public void SetTexts(List<ShowText> objects)
        {
            lock (showImgLock)
            {
                showTexts = objects;
            }
        }

        public void AddShowText(ShowText hObject)
        {
            lock (showImgLock)
            {
                showTexts.Add(hObject);
            }
        }

        public void RefreshShowTexts()
        {
            for (int i = 0; i < showTexts.Count; i++)
            {
                HOperatorSet.SetColor(WindowID, showTexts[i].m_showColor);
                HOperatorSet.SetTposition(WindowID, showTexts[i].m_dbPosY, showTexts[i].m_dbPosX);
                HOperatorSet.WriteString(WindowID, showTexts[i].m_strText);
            }
        }

        //ROI集合
        private ROIManager roiManager = new ROIManager();

        public void SetROICount(int count)
        {
            roiManager.SetMaxCount(count);
        }

        public void ResetROIs()
        {
            roiManager.ResetROIs();
        }

        public void SetROIs(List<ROIBase> rois)
        {
            roiManager.SetROIs(rois);
        }

        public List<ROIBase> GetROIs()
        {
            for (int i = 0; i < roiManager.GetROIs().Count; i++)
            {
                roiManager.GetROIs()[i].GenerateParameter();
            }
            return roiManager.GetROIs();
        }

        public void AddROI(ROIBase roi)
        {
            roiManager.AddROI(roi);
        }

        private ROIStatus m_ROIStatus = ROIStatus.UNION;

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
            roiManager.DeleteROI(index);
        }

        public HRegion GetRegions()
        {
            return roiManager.GetRegions();
        }

        public void ResetWndCtrl(bool isZoom)
        {
            lock (showImgLock)
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
                    HOperatorSet.WriteImage(source_Image, "png", 0, sfd.FileName);
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
            for (int i = 0; i < showObjects.Count; i++)
            {
                showObjects[i].m_object.Dispose();
            }
            for (int i = 0; i < roiManager.GetROIs().Count; i++)
            {
                roiManager.GetROIs()[i].m_DrawingObject.Dispose();
            }
            if (source_Image != null)
            {
                source_Image.Dispose();
            }
            if (Source_Image != null)
            {
                Source_Image.Dispose();
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
