namespace AutomationSystem.FindLine
{
    partial class FindLineForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FindLineForm));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripDropDownButton1 = new System.Windows.Forms.ToolStripDropDownButton();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.button3 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.hObjectViewer1 = new AutomationSystem.Halcon.HObjectViewer();
            this.label1 = new System.Windows.Forms.Label();
            this.numericUpDownSigma = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownThreshold = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.comboBoxEdge = new System.Windows.Forms.ComboBox();
            this.comboBoxSelect = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.numericUpDownWidth = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.numericUpDownCount = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownSigma)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownThreshold)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownCount)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.BackColor = System.Drawing.Color.LightYellow;
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripDropDownButton1});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(994, 27);
            this.toolStrip1.TabIndex = 42;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripDropDownButton1
            // 
            this.toolStripDropDownButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripDropDownButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1});
            this.toolStripDropDownButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDropDownButton1.Image")));
            this.toolStripDropDownButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButton1.Name = "toolStripDropDownButton1";
            this.toolStripDropDownButton1.ShowDropDownArrow = false;
            this.toolStripDropDownButton1.Size = new System.Drawing.Size(43, 24);
            this.toolStripDropDownButton1.Text = "文件";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(144, 26);
            this.toolStripMenuItem1.Text = "打开文件";
            this.toolStripMenuItem1.Click += new System.EventHandler(this.toolStripMenuItem1_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(881, 534);
            this.button3.Margin = new System.Windows.Forms.Padding(4);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(100, 29);
            this.button3.TabIndex = 47;
            this.button3.Text = "取消";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(745, 534);
            this.button2.Margin = new System.Windows.Forms.Padding(4);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(100, 29);
            this.button2.TabIndex = 46;
            this.button2.Text = "确定";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // hObjectViewer1
            // 
            this.hObjectViewer1.ActiveTool = AutomationSystem.Halcon.ViewerTools.Arrow;
            this.hObjectViewer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.hObjectViewer1.IsShowCross = false;
            this.hObjectViewer1.Location = new System.Drawing.Point(0, 29);
            this.hObjectViewer1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.hObjectViewer1.Name = "hObjectViewer1";
            this.hObjectViewer1.ShowToolbar = true;
            this.hObjectViewer1.Size = new System.Drawing.Size(695, 534);
            this.hObjectViewer1.Source_Image = null;
            this.hObjectViewer1.TabIndex = 45;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 15);
            this.label1.TabIndex = 48;
            this.label1.Text = "平滑系数：";
            // 
            // numericUpDownSigma
            // 
            this.numericUpDownSigma.DecimalPlaces = 2;
            this.numericUpDownSigma.Location = new System.Drawing.Point(101, 28);
            this.numericUpDownSigma.Minimum = new decimal(new int[] {
            4,
            0,
            0,
            65536});
            this.numericUpDownSigma.Name = "numericUpDownSigma";
            this.numericUpDownSigma.Size = new System.Drawing.Size(120, 25);
            this.numericUpDownSigma.TabIndex = 49;
            this.numericUpDownSigma.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownSigma.ValueChanged += new System.EventHandler(this.numericUpDown_ValueChanged);
            // 
            // numericUpDownThreshold
            // 
            this.numericUpDownThreshold.Location = new System.Drawing.Point(101, 58);
            this.numericUpDownThreshold.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numericUpDownThreshold.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownThreshold.Name = "numericUpDownThreshold";
            this.numericUpDownThreshold.Size = new System.Drawing.Size(120, 25);
            this.numericUpDownThreshold.TabIndex = 51;
            this.numericUpDownThreshold.Value = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this.numericUpDownThreshold.ValueChanged += new System.EventHandler(this.numericUpDown_ValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 60);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(82, 15);
            this.label2.TabIndex = 50;
            this.label2.Text = "边缘阈值：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 90);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(82, 15);
            this.label3.TabIndex = 52;
            this.label3.Text = "查找边缘：";
            // 
            // comboBoxEdge
            // 
            this.comboBoxEdge.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxEdge.FormattingEnabled = true;
            this.comboBoxEdge.Items.AddRange(new object[] {
            "全部",
            "由白到黑",
            "由黑到白"});
            this.comboBoxEdge.Location = new System.Drawing.Point(101, 87);
            this.comboBoxEdge.Name = "comboBoxEdge";
            this.comboBoxEdge.Size = new System.Drawing.Size(121, 23);
            this.comboBoxEdge.TabIndex = 53;
            this.comboBoxEdge.SelectedIndexChanged += new System.EventHandler(this.comboBox_SelectedIndexChanged);
            // 
            // comboBoxSelect
            // 
            this.comboBoxSelect.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxSelect.FormattingEnabled = true;
            this.comboBoxSelect.Items.AddRange(new object[] {
            "全部",
            "第一条边",
            "最后一边"});
            this.comboBoxSelect.Location = new System.Drawing.Point(101, 117);
            this.comboBoxSelect.Name = "comboBoxSelect";
            this.comboBoxSelect.Size = new System.Drawing.Size(121, 23);
            this.comboBoxSelect.TabIndex = 55;
            this.comboBoxSelect.SelectedIndexChanged += new System.EventHandler(this.comboBox_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 120);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(82, 15);
            this.label4.TabIndex = 54;
            this.label4.Text = "选择条件：";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.numericUpDownSigma);
            this.groupBox1.Controls.Add(this.comboBoxSelect);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.comboBoxEdge);
            this.groupBox1.Controls.Add(this.numericUpDownThreshold);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Location = new System.Drawing.Point(745, 170);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(235, 151);
            this.groupBox1.TabIndex = 56;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "查找参数";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.numericUpDownWidth);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.numericUpDownCount);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Location = new System.Drawing.Point(745, 53);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(235, 100);
            this.groupBox2.TabIndex = 57;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "卡尺设置";
            // 
            // numericUpDownWidth
            // 
            this.numericUpDownWidth.Location = new System.Drawing.Point(102, 55);
            this.numericUpDownWidth.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownWidth.Name = "numericUpDownWidth";
            this.numericUpDownWidth.Size = new System.Drawing.Size(120, 25);
            this.numericUpDownWidth.TabIndex = 53;
            this.numericUpDownWidth.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numericUpDownWidth.ValueChanged += new System.EventHandler(this.numericUpDown_ValueChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(13, 57);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(82, 15);
            this.label6.TabIndex = 52;
            this.label6.Text = "卡尺宽度：";
            // 
            // numericUpDownCount
            // 
            this.numericUpDownCount.Location = new System.Drawing.Point(102, 24);
            this.numericUpDownCount.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownCount.Name = "numericUpDownCount";
            this.numericUpDownCount.Size = new System.Drawing.Size(120, 25);
            this.numericUpDownCount.TabIndex = 51;
            this.numericUpDownCount.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numericUpDownCount.ValueChanged += new System.EventHandler(this.numericUpDown_ValueChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(13, 26);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(82, 15);
            this.label5.TabIndex = 50;
            this.label5.Text = "卡尺数量：";
            // 
            // FindLineForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(994, 571);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.hObjectViewer1);
            this.Controls.Add(this.toolStrip1);
            this.Name = "FindLineForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FindLineForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FindLineForm_FormClosing);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownSigma)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownThreshold)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownCount)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button2;
        private Halcon.HObjectViewer hObjectViewer1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown numericUpDownSigma;
        private System.Windows.Forms.NumericUpDown numericUpDownThreshold;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox comboBoxEdge;
        private System.Windows.Forms.ComboBox comboBoxSelect;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.NumericUpDown numericUpDownWidth;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown numericUpDownCount;
        private System.Windows.Forms.Label label5;
    }
}