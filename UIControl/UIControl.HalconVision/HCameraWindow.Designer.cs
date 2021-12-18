namespace UIControl.HalconVision
{
    partial class HCameraWindow
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
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripComboBox1 = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton3 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton4 = new System.Windows.Forms.ToolStripButton();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.numericUpDownExposure = new System.Windows.Forms.NumericUpDown();
            this.checkBoxShowCross = new System.Windows.Forms.CheckBox();
            this.checkBoxSetCross = new System.Windows.Forms.CheckBox();
            this.hObjectViewer1 = new Halcon.Functions.HObjectViewer();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownExposure)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel2,
            this.toolStripComboBox1,
            this.toolStripButton1,
            this.toolStripButton2,
            this.toolStripButton3,
            this.toolStripButton4,
            this.toolStripLabel1});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(727, 28);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripLabel2
            // 
            this.toolStripLabel2.Name = "toolStripLabel2";
            this.toolStripLabel2.Size = new System.Drawing.Size(69, 25);
            this.toolStripLabel2.Text = "选择相机";
            // 
            // toolStripComboBox1
            // 
            this.toolStripComboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.toolStripComboBox1.Name = "toolStripComboBox1";
            this.toolStripComboBox1.Size = new System.Drawing.Size(121, 28);
            this.toolStripComboBox1.SelectedIndexChanged += new System.EventHandler(this.toolStripComboBox1_SelectedIndexChanged);
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton1.Image = global::UIControl.HalconVision.Properties.Resources.start_enable;
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(24, 25);
            this.toolStripButton1.Text = "开启采集";
            this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton2.Image = global::UIControl.HalconVision.Properties.Resources.pause_unenable;
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(24, 25);
            this.toolStripButton2.Text = "停止采集";
            this.toolStripButton2.Click += new System.EventHandler(this.toolStripButton2_Click);
            // 
            // toolStripButton3
            // 
            this.toolStripButton3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton3.Image = global::UIControl.HalconVision.Properties.Resources.horizon_unenable;
            this.toolStripButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton3.Name = "toolStripButton3";
            this.toolStripButton3.Size = new System.Drawing.Size(24, 25);
            this.toolStripButton3.Text = "水平镜像";
            this.toolStripButton3.Click += new System.EventHandler(this.toolStripButton3_Click);
            // 
            // toolStripButton4
            // 
            this.toolStripButton4.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton4.Image = global::UIControl.HalconVision.Properties.Resources.vertical_unenable;
            this.toolStripButton4.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton4.Name = "toolStripButton4";
            this.toolStripButton4.Size = new System.Drawing.Size(24, 25);
            this.toolStripButton4.Text = "垂直镜像";
            this.toolStripButton4.Click += new System.EventHandler(this.toolStripButton4_Click);
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(54, 25);
            this.toolStripLabel1.Text = "曝光值";
            // 
            // numericUpDownExposure
            // 
            this.numericUpDownExposure.Location = new System.Drawing.Point(389, 4);
            this.numericUpDownExposure.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.numericUpDownExposure.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.numericUpDownExposure.Name = "numericUpDownExposure";
            this.numericUpDownExposure.Size = new System.Drawing.Size(77, 25);
            this.numericUpDownExposure.TabIndex = 2;
            this.numericUpDownExposure.Value = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numericUpDownExposure.ValueChanged += new System.EventHandler(this.numericUpDownExposure_ValueChanged);
            // 
            // checkBoxShowCross
            // 
            this.checkBoxShowCross.AutoSize = true;
            this.checkBoxShowCross.Location = new System.Drawing.Point(473, 5);
            this.checkBoxShowCross.Margin = new System.Windows.Forms.Padding(4);
            this.checkBoxShowCross.Name = "checkBoxShowCross";
            this.checkBoxShowCross.Size = new System.Drawing.Size(74, 19);
            this.checkBoxShowCross.TabIndex = 3;
            this.checkBoxShowCross.Text = "显示十";
            this.checkBoxShowCross.UseVisualStyleBackColor = true;
            this.checkBoxShowCross.CheckedChanged += new System.EventHandler(this.checkBoxShowCross_CheckedChanged);
            // 
            // checkBoxSetCross
            // 
            this.checkBoxSetCross.AutoSize = true;
            this.checkBoxSetCross.Location = new System.Drawing.Point(559, 5);
            this.checkBoxSetCross.Margin = new System.Windows.Forms.Padding(4);
            this.checkBoxSetCross.Name = "checkBoxSetCross";
            this.checkBoxSetCross.Size = new System.Drawing.Size(74, 19);
            this.checkBoxSetCross.TabIndex = 4;
            this.checkBoxSetCross.Text = "设置十";
            this.checkBoxSetCross.UseVisualStyleBackColor = true;
            this.checkBoxSetCross.CheckedChanged += new System.EventHandler(this.checkBoxSetCross_CheckedChanged);
            // 
            // hObjectViewer1
            // 
            this.hObjectViewer1.ActiveTool = Halcon.Functions.ViewerTools.Arrow;
            this.hObjectViewer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.hObjectViewer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.hObjectViewer1.IsShowCross = false;
            this.hObjectViewer1.Location = new System.Drawing.Point(0, 28);
            this.hObjectViewer1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.hObjectViewer1.Name = "hObjectViewer1";
            this.hObjectViewer1.ShowToolbar = false;
            this.hObjectViewer1.Size = new System.Drawing.Size(727, 554);
            this.hObjectViewer1.TabIndex = 1;
            // 
            // HCameraWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.checkBoxSetCross);
            this.Controls.Add(this.checkBoxShowCross);
            this.Controls.Add(this.numericUpDownExposure);
            this.Controls.Add(this.hObjectViewer1);
            this.Controls.Add(this.toolStrip1);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "HCameraWindow";
            this.Size = new System.Drawing.Size(727, 582);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownExposure)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private Halcon.Functions.HObjectViewer hObjectViewer1;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
        private System.Windows.Forms.ToolStripButton toolStripButton3;
        private System.Windows.Forms.ToolStripButton toolStripButton4;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.NumericUpDown numericUpDownExposure;
        private System.Windows.Forms.CheckBox checkBoxShowCross;
        private System.Windows.Forms.CheckBox checkBoxSetCross;
        private System.Windows.Forms.ToolStripLabel toolStripLabel2;
        private System.Windows.Forms.ToolStripComboBox toolStripComboBox1;
    }
}
