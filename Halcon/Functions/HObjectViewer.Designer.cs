namespace Halcon.Functions
{
    partial class HObjectViewer
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HObjectViewer));
            this.ViewerControl = new HalconDotNet.HWindowControl();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItemZoom = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemOpen = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemSave = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemSaveWindow = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemRegistered = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabelSize = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel4 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabelChannel = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabelCoordinate = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel3 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabelValue = new System.Windows.Forms.ToolStripStatusLabel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tsbArrow = new System.Windows.Forms.ToolStripButton();
            this.tsbZoomIn = new System.Windows.Forms.ToolStripButton();
            this.tsbZoomOut = new System.Windows.Forms.ToolStripButton();
            this.tsbHand = new System.Windows.Forms.ToolStripButton();
            this.tsbRectangle = new System.Windows.Forms.ToolStripButton();
            this.tsbRotateRectangle = new System.Windows.Forms.ToolStripButton();
            this.tsbCircle = new System.Windows.Forms.ToolStripButton();
            this.tsbEllipse = new System.Windows.Forms.ToolStripButton();
            this.tsbRemove = new System.Windows.Forms.ToolStripButton();
            this.tsbClear = new System.Windows.Forms.ToolStripButton();
            this.toolStripDropDownButtonStatus = new System.Windows.Forms.ToolStripDropDownButton();
            this.toolStripMenuItemUNION = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemINTERSECTION = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemDIFFERENCE = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // ViewerControl
            // 
            this.ViewerControl.BackColor = System.Drawing.Color.Black;
            this.ViewerControl.BorderColor = System.Drawing.Color.Black;
            this.ViewerControl.ContextMenuStrip = this.contextMenuStrip1;
            this.ViewerControl.ImagePart = new System.Drawing.Rectangle(0, 0, 640, 480);
            this.ViewerControl.Location = new System.Drawing.Point(0, 0);
            this.ViewerControl.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ViewerControl.Name = "ViewerControl";
            this.ViewerControl.Size = new System.Drawing.Size(695, 474);
            this.ViewerControl.TabIndex = 0;
            this.ViewerControl.WindowSize = new System.Drawing.Size(695, 474);
            this.ViewerControl.HMouseMove += new HalconDotNet.HMouseEventHandler(this.ViewerControl_HMouseMove);
            this.ViewerControl.HMouseDown += new HalconDotNet.HMouseEventHandler(this.ViewerControl_HMouseDown);
            this.ViewerControl.HMouseUp += new HalconDotNet.HMouseEventHandler(this.ViewerControl_HMouseUp);
            this.ViewerControl.HMouseWheel += new HalconDotNet.HMouseEventHandler(this.ViewerControl_HMouseWheel);
            this.ViewerControl.Resize += new System.EventHandler(this.ViewerControl_Resize);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemZoom,
            this.toolStripMenuItemOpen,
            this.toolStripMenuItemSave,
            this.toolStripMenuItemSaveWindow,
            this.toolStripMenuItemRegistered});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(139, 124);
            this.contextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip1_Opening);
            // 
            // toolStripMenuItemZoom
            // 
            this.toolStripMenuItemZoom.Name = "toolStripMenuItemZoom";
            this.toolStripMenuItemZoom.Size = new System.Drawing.Size(138, 24);
            this.toolStripMenuItemZoom.Text = "适应窗体";
            this.toolStripMenuItemZoom.Click += new System.EventHandler(this.toolStripMenuItemZoom_Click);
            // 
            // toolStripMenuItemOpen
            // 
            this.toolStripMenuItemOpen.Name = "toolStripMenuItemOpen";
            this.toolStripMenuItemOpen.Size = new System.Drawing.Size(138, 24);
            this.toolStripMenuItemOpen.Text = "打开图片";
            this.toolStripMenuItemOpen.Click += new System.EventHandler(this.toolStripMenuItemOpen_Click);
            // 
            // toolStripMenuItemSave
            // 
            this.toolStripMenuItemSave.Name = "toolStripMenuItemSave";
            this.toolStripMenuItemSave.Size = new System.Drawing.Size(138, 24);
            this.toolStripMenuItemSave.Text = "保存图片";
            this.toolStripMenuItemSave.Click += new System.EventHandler(this.toolStripMenuItemSave_Click);
            // 
            // toolStripMenuItemSaveWindow
            // 
            this.toolStripMenuItemSaveWindow.Name = "toolStripMenuItemSaveWindow";
            this.toolStripMenuItemSaveWindow.Size = new System.Drawing.Size(138, 24);
            this.toolStripMenuItemSaveWindow.Text = "保存窗口";
            this.toolStripMenuItemSaveWindow.Click += new System.EventHandler(this.toolStripMenuItemSaveWindow_Click);
            // 
            // toolStripMenuItemRegistered
            // 
            this.toolStripMenuItemRegistered.Name = "toolStripMenuItemRegistered";
            this.toolStripMenuItemRegistered.Size = new System.Drawing.Size(138, 24);
            this.toolStripMenuItemRegistered.Text = "注册模板";
            this.toolStripMenuItemRegistered.Click += new System.EventHandler(this.toolStripMenuItemRegistered_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.BackColor = System.Drawing.SystemColors.Control;
            this.statusStrip1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel2,
            this.toolStripStatusLabelSize,
            this.toolStripStatusLabel4,
            this.toolStripStatusLabelChannel,
            this.toolStripStatusLabel1,
            this.toolStripStatusLabelCoordinate,
            this.toolStripStatusLabel3,
            this.toolStripStatusLabelValue});
            this.statusStrip1.Location = new System.Drawing.Point(0, 30);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 13, 0);
            this.statusStrip1.Size = new System.Drawing.Size(695, 30);
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.toolStripStatusLabel2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripStatusLabel2.Image = global::Halcon.Functions.Properties.Resources.dimensions;
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(24, 25);
            this.toolStripStatusLabel2.Text = "toolStripStatusLabel2";
            // 
            // toolStripStatusLabelSize
            // 
            this.toolStripStatusLabelSize.AutoSize = false;
            this.toolStripStatusLabelSize.Name = "toolStripStatusLabelSize";
            this.toolStripStatusLabelSize.Size = new System.Drawing.Size(80, 25);
            // 
            // toolStripStatusLabel4
            // 
            this.toolStripStatusLabel4.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripStatusLabel4.Image = global::Halcon.Functions.Properties.Resources.channel;
            this.toolStripStatusLabel4.Name = "toolStripStatusLabel4";
            this.toolStripStatusLabel4.Size = new System.Drawing.Size(20, 25);
            // 
            // toolStripStatusLabelChannel
            // 
            this.toolStripStatusLabelChannel.AutoSize = false;
            this.toolStripStatusLabelChannel.Name = "toolStripStatusLabelChannel";
            this.toolStripStatusLabelChannel.Size = new System.Drawing.Size(30, 25);
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.toolStripStatusLabel1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripStatusLabel1.Image = global::Halcon.Functions.Properties.Resources.arrow480;
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(24, 25);
            // 
            // toolStripStatusLabelCoordinate
            // 
            this.toolStripStatusLabelCoordinate.AutoSize = false;
            this.toolStripStatusLabelCoordinate.Name = "toolStripStatusLabelCoordinate";
            this.toolStripStatusLabelCoordinate.Size = new System.Drawing.Size(80, 25);
            // 
            // toolStripStatusLabel3
            // 
            this.toolStripStatusLabel3.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.toolStripStatusLabel3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripStatusLabel3.Image = global::Halcon.Functions.Properties.Resources.rgb;
            this.toolStripStatusLabel3.Name = "toolStripStatusLabel3";
            this.toolStripStatusLabel3.Size = new System.Drawing.Size(24, 25);
            // 
            // toolStripStatusLabelValue
            // 
            this.toolStripStatusLabelValue.AutoSize = false;
            this.toolStripStatusLabelValue.Name = "toolStripStatusLabelValue";
            this.toolStripStatusLabelValue.Size = new System.Drawing.Size(80, 25);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.statusStrip1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.toolStrip1, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 474);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(695, 60);
            this.tableLayoutPanel1.TabIndex = 3;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbArrow,
            this.tsbZoomIn,
            this.tsbZoomOut,
            this.tsbHand,
            this.tsbRectangle,
            this.tsbRotateRectangle,
            this.tsbCircle,
            this.tsbEllipse,
            this.tsbRemove,
            this.tsbClear,
            this.toolStripDropDownButtonStatus});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(695, 30);
            this.toolStrip1.TabIndex = 3;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tsbArrow
            // 
            this.tsbArrow.BackColor = System.Drawing.Color.Transparent;
            this.tsbArrow.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbArrow.Image = global::Halcon.Functions.Properties.Resources.Arrow;
            this.tsbArrow.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsbArrow.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbArrow.Name = "tsbArrow";
            this.tsbArrow.Size = new System.Drawing.Size(23, 27);
            this.tsbArrow.Text = "toolStripButton1";
            this.tsbArrow.Click += new System.EventHandler(this.tsbActiveTool_Click);
            // 
            // tsbZoomIn
            // 
            this.tsbZoomIn.BackColor = System.Drawing.Color.Transparent;
            this.tsbZoomIn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbZoomIn.Image = global::Halcon.Functions.Properties.Resources.ZoomIn;
            this.tsbZoomIn.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsbZoomIn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbZoomIn.Name = "tsbZoomIn";
            this.tsbZoomIn.Size = new System.Drawing.Size(23, 27);
            this.tsbZoomIn.Text = "toolStripButton2";
            this.tsbZoomIn.Click += new System.EventHandler(this.tsbActiveTool_Click);
            // 
            // tsbZoomOut
            // 
            this.tsbZoomOut.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbZoomOut.Image = global::Halcon.Functions.Properties.Resources.ZoomOut;
            this.tsbZoomOut.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsbZoomOut.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbZoomOut.Name = "tsbZoomOut";
            this.tsbZoomOut.Size = new System.Drawing.Size(23, 27);
            this.tsbZoomOut.Text = "toolStripButton1";
            this.tsbZoomOut.Click += new System.EventHandler(this.tsbActiveTool_Click);
            // 
            // tsbHand
            // 
            this.tsbHand.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbHand.Image = ((System.Drawing.Image)(resources.GetObject("tsbHand.Image")));
            this.tsbHand.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsbHand.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbHand.Name = "tsbHand";
            this.tsbHand.Size = new System.Drawing.Size(23, 27);
            this.tsbHand.Text = "toolStripButton1";
            this.tsbHand.Click += new System.EventHandler(this.tsbActiveTool_Click);
            // 
            // tsbRectangle
            // 
            this.tsbRectangle.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbRectangle.Image = global::Halcon.Functions.Properties.Resources.Rectangle;
            this.tsbRectangle.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbRectangle.Name = "tsbRectangle";
            this.tsbRectangle.Size = new System.Drawing.Size(24, 27);
            this.tsbRectangle.Text = "toolStripButton1";
            this.tsbRectangle.Click += new System.EventHandler(this.tsbActiveTool_Click);
            // 
            // tsbRotateRectangle
            // 
            this.tsbRotateRectangle.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbRotateRectangle.Image = global::Halcon.Functions.Properties.Resources.RotateRectangle;
            this.tsbRotateRectangle.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbRotateRectangle.Name = "tsbRotateRectangle";
            this.tsbRotateRectangle.Size = new System.Drawing.Size(24, 27);
            this.tsbRotateRectangle.Text = "toolStripButton1";
            this.tsbRotateRectangle.Click += new System.EventHandler(this.tsbActiveTool_Click);
            // 
            // tsbCircle
            // 
            this.tsbCircle.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbCircle.Image = global::Halcon.Functions.Properties.Resources.Circle;
            this.tsbCircle.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsbCircle.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbCircle.Name = "tsbCircle";
            this.tsbCircle.Size = new System.Drawing.Size(23, 27);
            this.tsbCircle.Text = "toolStripButton1";
            this.tsbCircle.Click += new System.EventHandler(this.tsbActiveTool_Click);
            // 
            // tsbEllipse
            // 
            this.tsbEllipse.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbEllipse.Image = global::Halcon.Functions.Properties.Resources.Ellipse;
            this.tsbEllipse.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsbEllipse.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbEllipse.Name = "tsbEllipse";
            this.tsbEllipse.Size = new System.Drawing.Size(23, 27);
            this.tsbEllipse.Text = "toolStripButton2";
            this.tsbEllipse.Click += new System.EventHandler(this.tsbActiveTool_Click);
            // 
            // tsbRemove
            // 
            this.tsbRemove.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbRemove.Image = global::Halcon.Functions.Properties.Resources.Remove;
            this.tsbRemove.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbRemove.Name = "tsbRemove";
            this.tsbRemove.Size = new System.Drawing.Size(24, 27);
            this.tsbRemove.Text = "删除选中项";
            this.tsbRemove.Click += new System.EventHandler(this.tsbRemove_Click);
            // 
            // tsbClear
            // 
            this.tsbClear.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbClear.Image = global::Halcon.Functions.Properties.Resources.Clear;
            this.tsbClear.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbClear.Name = "tsbClear";
            this.tsbClear.Size = new System.Drawing.Size(24, 27);
            this.tsbClear.Text = "删除所有";
            this.tsbClear.Click += new System.EventHandler(this.tsbClear_Click);
            // 
            // toolStripDropDownButtonStatus
            // 
            this.toolStripDropDownButtonStatus.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripDropDownButtonStatus.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemUNION,
            this.toolStripMenuItemINTERSECTION,
            this.toolStripMenuItemDIFFERENCE});
            this.toolStripDropDownButtonStatus.Image = global::Halcon.Functions.Properties.Resources.UNION;
            this.toolStripDropDownButtonStatus.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButtonStatus.Name = "toolStripDropDownButtonStatus";
            this.toolStripDropDownButtonStatus.Size = new System.Drawing.Size(34, 27);
            this.toolStripDropDownButtonStatus.Text = "集合类型";
            // 
            // toolStripMenuItemUNION
            // 
            this.toolStripMenuItemUNION.Image = global::Halcon.Functions.Properties.Resources.UNION;
            this.toolStripMenuItemUNION.Name = "toolStripMenuItemUNION";
            this.toolStripMenuItemUNION.Size = new System.Drawing.Size(114, 26);
            this.toolStripMenuItemUNION.Text = "并集";
            this.toolStripMenuItemUNION.Click += new System.EventHandler(this.toolStripMenuItemUNION_Click);
            // 
            // toolStripMenuItemINTERSECTION
            // 
            this.toolStripMenuItemINTERSECTION.Image = global::Halcon.Functions.Properties.Resources.INTERSECTION;
            this.toolStripMenuItemINTERSECTION.Name = "toolStripMenuItemINTERSECTION";
            this.toolStripMenuItemINTERSECTION.Size = new System.Drawing.Size(114, 26);
            this.toolStripMenuItemINTERSECTION.Text = "交集";
            this.toolStripMenuItemINTERSECTION.Click += new System.EventHandler(this.toolStripMenuItemINTERSECTION_Click);
            // 
            // toolStripMenuItemDIFFERENCE
            // 
            this.toolStripMenuItemDIFFERENCE.Image = global::Halcon.Functions.Properties.Resources.DIFFERENCE;
            this.toolStripMenuItemDIFFERENCE.Name = "toolStripMenuItemDIFFERENCE";
            this.toolStripMenuItemDIFFERENCE.Size = new System.Drawing.Size(114, 26);
            this.toolStripMenuItemDIFFERENCE.Text = "差集";
            this.toolStripMenuItemDIFFERENCE.Click += new System.EventHandler(this.toolStripMenuItemDIFFERENCE_Click);
            // 
            // HObjectViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.ViewerControl);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "HObjectViewer";
            this.Size = new System.Drawing.Size(695, 534);
            this.Load += new System.EventHandler(this.HObjectViewer_Load);
            this.SizeChanged += new System.EventHandler(this.HObjectViewer_SizeChanged);
            this.contextMenuStrip1.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemZoom;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemOpen;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemSave;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelCoordinate;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel3;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelValue;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private HalconDotNet.HWindowControl ViewerControl;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton tsbArrow;
        private System.Windows.Forms.ToolStripButton tsbZoomIn;
        private System.Windows.Forms.ToolStripButton tsbZoomOut;
        private System.Windows.Forms.ToolStripButton tsbHand;
        private System.Windows.Forms.ToolStripButton tsbRectangle;
        private System.Windows.Forms.ToolStripButton tsbRotateRectangle;
        private System.Windows.Forms.ToolStripButton tsbCircle;
        private System.Windows.Forms.ToolStripButton tsbEllipse;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelSize;
        private System.Windows.Forms.ToolStripButton tsbRemove;
        private System.Windows.Forms.ToolStripButton tsbClear;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel4;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelChannel;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemSaveWindow;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButtonStatus;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemUNION;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemINTERSECTION;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemDIFFERENCE;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemRegistered;
    }
}
