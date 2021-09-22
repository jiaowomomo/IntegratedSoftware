namespace UIControl.HalconVision
{
    partial class HShowWindow
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
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonOpen = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonSaveImage = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonSaveWindow = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonZoom = new System.Windows.Forms.ToolStripButton();
            this.hObjectViewer1 = new Halcon.Functions.HObjectViewer();
            this.checkBoxSetCross = new System.Windows.Forms.CheckBox();
            this.checkBoxShowCross = new System.Windows.Forms.CheckBox();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonOpen,
            this.toolStripButtonSaveImage,
            this.toolStripButtonSaveWindow,
            this.toolStripButtonZoom});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(727, 27);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButtonOpen
            // 
            this.toolStripButtonOpen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonOpen.Image = global::UIControl.HalconVision.Properties.Resources.file;
            this.toolStripButtonOpen.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonOpen.Name = "toolStripButtonOpen";
            this.toolStripButtonOpen.Size = new System.Drawing.Size(24, 24);
            this.toolStripButtonOpen.Text = "打开图片";
            this.toolStripButtonOpen.Click += new System.EventHandler(this.toolStripButtonOpen_Click);
            // 
            // toolStripButtonSaveImage
            // 
            this.toolStripButtonSaveImage.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonSaveImage.Image = global::UIControl.HalconVision.Properties.Resources.save;
            this.toolStripButtonSaveImage.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonSaveImage.Name = "toolStripButtonSaveImage";
            this.toolStripButtonSaveImage.Size = new System.Drawing.Size(24, 24);
            this.toolStripButtonSaveImage.Text = "保存图片";
            this.toolStripButtonSaveImage.Click += new System.EventHandler(this.toolStripButtonSaveImage_Click);
            // 
            // toolStripButtonSaveWindow
            // 
            this.toolStripButtonSaveWindow.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonSaveWindow.Image = global::UIControl.HalconVision.Properties.Resources.save_as;
            this.toolStripButtonSaveWindow.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonSaveWindow.Name = "toolStripButtonSaveWindow";
            this.toolStripButtonSaveWindow.Size = new System.Drawing.Size(24, 24);
            this.toolStripButtonSaveWindow.Text = "保存窗口";
            this.toolStripButtonSaveWindow.Click += new System.EventHandler(this.toolStripButtonSaveWindow_Click);
            // 
            // toolStripButtonZoom
            // 
            this.toolStripButtonZoom.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonZoom.Image = global::UIControl.HalconVision.Properties.Resources.zoom;
            this.toolStripButtonZoom.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonZoom.Name = "toolStripButtonZoom";
            this.toolStripButtonZoom.Size = new System.Drawing.Size(24, 24);
            this.toolStripButtonZoom.Text = "适应窗口";
            this.toolStripButtonZoom.Click += new System.EventHandler(this.toolStripButtonZoom_Click);
            // 
            // hObjectViewer1
            // 
            this.hObjectViewer1.ActiveTool = Halcon.Functions.ViewerTools.Arrow;
            this.hObjectViewer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.hObjectViewer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.hObjectViewer1.IsShowCross = false;
            this.hObjectViewer1.Location = new System.Drawing.Point(0, 27);
            this.hObjectViewer1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.hObjectViewer1.Name = "hObjectViewer1";
            this.hObjectViewer1.ShowToolbar = false;
            this.hObjectViewer1.Size = new System.Drawing.Size(727, 555);
            this.hObjectViewer1.TabIndex = 1;
            // 
            // checkBoxSetCross
            // 
            this.checkBoxSetCross.AutoSize = true;
            this.checkBoxSetCross.Location = new System.Drawing.Point(219, 6);
            this.checkBoxSetCross.Margin = new System.Windows.Forms.Padding(4);
            this.checkBoxSetCross.Name = "checkBoxSetCross";
            this.checkBoxSetCross.Size = new System.Drawing.Size(74, 19);
            this.checkBoxSetCross.TabIndex = 6;
            this.checkBoxSetCross.Text = "设置十";
            this.checkBoxSetCross.UseVisualStyleBackColor = true;
            this.checkBoxSetCross.CheckedChanged += new System.EventHandler(this.checkBoxSetCross_CheckedChanged);
            // 
            // checkBoxShowCross
            // 
            this.checkBoxShowCross.AutoSize = true;
            this.checkBoxShowCross.Location = new System.Drawing.Point(133, 6);
            this.checkBoxShowCross.Margin = new System.Windows.Forms.Padding(4);
            this.checkBoxShowCross.Name = "checkBoxShowCross";
            this.checkBoxShowCross.Size = new System.Drawing.Size(74, 19);
            this.checkBoxShowCross.TabIndex = 5;
            this.checkBoxShowCross.Text = "显示十";
            this.checkBoxShowCross.UseVisualStyleBackColor = true;
            this.checkBoxShowCross.CheckedChanged += new System.EventHandler(this.checkBoxShowCross_CheckedChanged);
            // 
            // HShowWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.checkBoxSetCross);
            this.Controls.Add(this.checkBoxShowCross);
            this.Controls.Add(this.hObjectViewer1);
            this.Controls.Add(this.toolStrip1);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "HShowWindow";
            this.Size = new System.Drawing.Size(727, 582);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private Halcon.Functions.HObjectViewer hObjectViewer1;
        private System.Windows.Forms.ToolStripButton toolStripButtonSaveImage;
        private System.Windows.Forms.ToolStripButton toolStripButtonSaveWindow;
        private System.Windows.Forms.ToolStripButton toolStripButtonZoom;
        private System.Windows.Forms.ToolStripButton toolStripButtonOpen;
        private System.Windows.Forms.CheckBox checkBoxSetCross;
        private System.Windows.Forms.CheckBox checkBoxShowCross;
    }
}
