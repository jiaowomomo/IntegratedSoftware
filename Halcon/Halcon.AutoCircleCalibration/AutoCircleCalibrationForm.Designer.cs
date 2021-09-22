namespace Halcon.AutoCircleCalibration
{
    partial class AutoCircleCalibrationForm
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

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AutoCircleCalibrationForm));
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripDropDownButton1 = new System.Windows.Forms.ToolStripDropDownButton();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.cbTest = new System.Windows.Forms.CheckBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label16 = new System.Windows.Forms.Label();
            this.numericUpDownLeftY = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.numericUpDownLeftX = new System.Windows.Forms.NumericUpDown();
            this.labelY = new System.Windows.Forms.Label();
            this.labelAutoCircle = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.numericUpDownCol = new System.Windows.Forms.NumericUpDown();
            this.labelX = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.numericUpDownRow = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownDisY = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.numericUpDownDisX = new System.Windows.Forms.NumericUpDown();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.cbbCalibIndex = new System.Windows.Forms.ComboBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lbAreaMax = new System.Windows.Forms.Label();
            this.trackBarAreaMax = new System.Windows.Forms.TrackBar();
            this.label9 = new System.Windows.Forms.Label();
            this.lbAreaMin = new System.Windows.Forms.Label();
            this.trackBarAreaMin = new System.Windows.Forms.TrackBar();
            this.label15 = new System.Windows.Forms.Label();
            this.lbThresholdMax = new System.Windows.Forms.Label();
            this.trackBarMax = new System.Windows.Forms.TrackBar();
            this.label8 = new System.Windows.Forms.Label();
            this.lbThresholdMin = new System.Windows.Forms.Label();
            this.trackBarMin = new System.Windows.Forms.TrackBar();
            this.label5 = new System.Windows.Forms.Label();
            this.hObjectViewer1 = new Halcon.Functions.HObjectViewer();
            this.toolStrip1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownLeftY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownLeftX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownCol)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownRow)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownDisY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownDisX)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarAreaMax)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarAreaMin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarMax)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarMin)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(174, 26);
            this.toolStripMenuItem2.Text = "加载标定信息";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(174, 26);
            this.toolStripMenuItem1.Text = "打开图像";
            this.toolStripMenuItem1.Click += new System.EventHandler(this.toolStripMenuItem1_Click);
            // 
            // toolStripDropDownButton1
            // 
            this.toolStripDropDownButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripDropDownButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.toolStripMenuItem2});
            this.toolStripDropDownButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDropDownButton1.Image")));
            this.toolStripDropDownButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButton1.Name = "toolStripDropDownButton1";
            this.toolStripDropDownButton1.Size = new System.Drawing.Size(53, 24);
            this.toolStripDropDownButton1.Text = "文件";
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
            this.toolStrip1.Size = new System.Drawing.Size(1138, 27);
            this.toolStrip1.TabIndex = 29;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // cbTest
            // 
            this.cbTest.AutoSize = true;
            this.cbTest.Location = new System.Drawing.Point(319, 215);
            this.cbTest.Margin = new System.Windows.Forms.Padding(4);
            this.cbTest.Name = "cbTest";
            this.cbTest.Size = new System.Drawing.Size(89, 19);
            this.cbTest.TabIndex = 36;
            this.cbTest.Text = "测试标定";
            this.cbTest.UseVisualStyleBackColor = true;
            this.cbTest.CheckedChanged += new System.EventHandler(this.cbTest_CheckedChanged);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label11.ForeColor = System.Drawing.Color.Red;
            this.label11.Location = new System.Drawing.Point(708, 432);
            this.label11.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(0, 24);
            this.label11.TabIndex = 35;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(714, 44);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(67, 15);
            this.label3.TabIndex = 34;
            this.label3.Text = "索引号：";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label16);
            this.groupBox3.Controls.Add(this.numericUpDownLeftY);
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Controls.Add(this.label14);
            this.groupBox3.Controls.Add(this.numericUpDownLeftX);
            this.groupBox3.Controls.Add(this.labelY);
            this.groupBox3.Controls.Add(this.labelAutoCircle);
            this.groupBox3.Controls.Add(this.label10);
            this.groupBox3.Controls.Add(this.numericUpDownCol);
            this.groupBox3.Controls.Add(this.labelX);
            this.groupBox3.Controls.Add(this.cbTest);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.label12);
            this.groupBox3.Controls.Add(this.label13);
            this.groupBox3.Controls.Add(this.numericUpDownRow);
            this.groupBox3.Controls.Add(this.numericUpDownDisY);
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Controls.Add(this.numericUpDownDisX);
            this.groupBox3.Controls.Add(this.button3);
            this.groupBox3.Controls.Add(this.button4);
            this.groupBox3.Controls.Add(this.button1);
            this.groupBox3.Location = new System.Drawing.Point(713, 403);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox3.Size = new System.Drawing.Size(415, 242);
            this.groupBox3.TabIndex = 32;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "数据处理";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label16.ForeColor = System.Drawing.Color.Red;
            this.label16.Location = new System.Drawing.Point(8, 210);
            this.label16.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(172, 24);
            this.label16.TabIndex = 45;
            this.label16.Text = "点阵行必须为奇数。";
            // 
            // numericUpDownLeftY
            // 
            this.numericUpDownLeftY.DecimalPlaces = 4;
            this.numericUpDownLeftY.Location = new System.Drawing.Point(306, 137);
            this.numericUpDownLeftY.Margin = new System.Windows.Forms.Padding(4);
            this.numericUpDownLeftY.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            -2147483648});
            this.numericUpDownLeftY.Name = "numericUpDownLeftY";
            this.numericUpDownLeftY.Size = new System.Drawing.Size(89, 25);
            this.numericUpDownLeftY.TabIndex = 44;
            this.numericUpDownLeftY.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(19, 140);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(75, 15);
            this.label7.TabIndex = 41;
            this.label7.Text = "左上点X：";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(211, 140);
            this.label14.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(75, 15);
            this.label14.TabIndex = 43;
            this.label14.Text = "左上点Y：";
            // 
            // numericUpDownLeftX
            // 
            this.numericUpDownLeftX.DecimalPlaces = 4;
            this.numericUpDownLeftX.Location = new System.Drawing.Point(114, 137);
            this.numericUpDownLeftX.Margin = new System.Windows.Forms.Padding(4);
            this.numericUpDownLeftX.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            -2147483648});
            this.numericUpDownLeftX.Name = "numericUpDownLeftX";
            this.numericUpDownLeftX.Size = new System.Drawing.Size(89, 25);
            this.numericUpDownLeftX.TabIndex = 42;
            this.numericUpDownLeftX.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // labelY
            // 
            this.labelY.AutoSize = true;
            this.labelY.ForeColor = System.Drawing.Color.Red;
            this.labelY.Location = new System.Drawing.Point(283, 24);
            this.labelY.Name = "labelY";
            this.labelY.Size = new System.Drawing.Size(0, 15);
            this.labelY.TabIndex = 40;
            // 
            // labelAutoCircle
            // 
            this.labelAutoCircle.AutoSize = true;
            this.labelAutoCircle.Location = new System.Drawing.Point(118, 46);
            this.labelAutoCircle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelAutoCircle.Name = "labelAutoCircle";
            this.labelAutoCircle.Size = new System.Drawing.Size(15, 15);
            this.labelAutoCircle.TabIndex = 26;
            this.labelAutoCircle.Text = "0";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(7, 46);
            this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(97, 15);
            this.label10.TabIndex = 25;
            this.label10.Text = "查找圆数量：";
            // 
            // numericUpDownCol
            // 
            this.numericUpDownCol.Location = new System.Drawing.Point(306, 104);
            this.numericUpDownCol.Margin = new System.Windows.Forms.Padding(4);
            this.numericUpDownCol.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.numericUpDownCol.Name = "numericUpDownCol";
            this.numericUpDownCol.Size = new System.Drawing.Size(89, 25);
            this.numericUpDownCol.TabIndex = 20;
            this.numericUpDownCol.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // labelX
            // 
            this.labelX.AutoSize = true;
            this.labelX.ForeColor = System.Drawing.Color.Red;
            this.labelX.Location = new System.Drawing.Point(104, 24);
            this.labelX.Name = "labelX";
            this.labelX.Size = new System.Drawing.Size(0, 15);
            this.labelX.TabIndex = 39;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(27, 106);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(67, 15);
            this.label4.TabIndex = 17;
            this.label4.Text = "点阵行：";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(219, 106);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(67, 15);
            this.label6.TabIndex = 19;
            this.label6.Text = "点阵列：";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(7, 24);
            this.label12.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(90, 15);
            this.label12.TabIndex = 37;
            this.label12.Text = "X世界坐标：";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(186, 24);
            this.label13.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(90, 15);
            this.label13.TabIndex = 38;
            this.label13.Text = "Y世界坐标：";
            // 
            // numericUpDownRow
            // 
            this.numericUpDownRow.Increment = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.numericUpDownRow.Location = new System.Drawing.Point(114, 104);
            this.numericUpDownRow.Margin = new System.Windows.Forms.Padding(4);
            this.numericUpDownRow.Maximum = new decimal(new int[] {
            99,
            0,
            0,
            0});
            this.numericUpDownRow.Minimum = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.numericUpDownRow.Name = "numericUpDownRow";
            this.numericUpDownRow.Size = new System.Drawing.Size(89, 25);
            this.numericUpDownRow.TabIndex = 18;
            this.numericUpDownRow.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // numericUpDownDisY
            // 
            this.numericUpDownDisY.DecimalPlaces = 4;
            this.numericUpDownDisY.Location = new System.Drawing.Point(306, 70);
            this.numericUpDownDisY.Margin = new System.Windows.Forms.Padding(4);
            this.numericUpDownDisY.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            -2147483648});
            this.numericUpDownDisY.Name = "numericUpDownDisY";
            this.numericUpDownDisY.Size = new System.Drawing.Size(89, 25);
            this.numericUpDownDisY.TabIndex = 16;
            this.numericUpDownDisY.Value = new decimal(new int[] {
            5,
            0,
            0,
            -2147483648});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(19, 73);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 15);
            this.label1.TabIndex = 13;
            this.label1.Text = "X轴间距：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(211, 73);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 15);
            this.label2.TabIndex = 15;
            this.label2.Text = "Y轴间距：";
            // 
            // numericUpDownDisX
            // 
            this.numericUpDownDisX.DecimalPlaces = 4;
            this.numericUpDownDisX.Location = new System.Drawing.Point(114, 70);
            this.numericUpDownDisX.Margin = new System.Windows.Forms.Padding(4);
            this.numericUpDownDisX.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            -2147483648});
            this.numericUpDownDisX.Name = "numericUpDownDisX";
            this.numericUpDownDisX.Size = new System.Drawing.Size(89, 25);
            this.numericUpDownDisX.TabIndex = 14;
            this.numericUpDownDisX.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(162, 173);
            this.button3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(91, 35);
            this.button3.TabIndex = 11;
            this.button3.Text = "标定";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(316, 173);
            this.button4.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(91, 35);
            this.button4.TabIndex = 12;
            this.button4.Text = "重新标定";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(8, 173);
            this.button1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(91, 35);
            this.button1.TabIndex = 6;
            this.button1.Text = "返回原图";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // cbbCalibIndex
            // 
            this.cbbCalibIndex.FormattingEnabled = true;
            this.cbbCalibIndex.Location = new System.Drawing.Point(793, 41);
            this.cbbCalibIndex.Margin = new System.Windows.Forms.Padding(4);
            this.cbbCalibIndex.Name = "cbbCalibIndex";
            this.cbbCalibIndex.Size = new System.Drawing.Size(69, 23);
            this.cbbCalibIndex.TabIndex = 33;
            this.cbbCalibIndex.Text = "0";
            this.cbbCalibIndex.SelectedIndexChanged += new System.EventHandler(this.cbbCalibIndex_SelectedIndexChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lbAreaMax);
            this.groupBox2.Controls.Add(this.trackBarAreaMax);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.lbAreaMin);
            this.groupBox2.Controls.Add(this.trackBarAreaMin);
            this.groupBox2.Controls.Add(this.label15);
            this.groupBox2.Controls.Add(this.lbThresholdMax);
            this.groupBox2.Controls.Add(this.trackBarMax);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.lbThresholdMin);
            this.groupBox2.Controls.Add(this.trackBarMin);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Location = new System.Drawing.Point(713, 72);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox2.Size = new System.Drawing.Size(415, 325);
            this.groupBox2.TabIndex = 30;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "图像处理";
            // 
            // lbAreaMax
            // 
            this.lbAreaMax.AutoSize = true;
            this.lbAreaMax.Location = new System.Drawing.Point(328, 99);
            this.lbAreaMax.Name = "lbAreaMax";
            this.lbAreaMax.Size = new System.Drawing.Size(31, 15);
            this.lbAreaMax.TabIndex = 11;
            this.lbAreaMax.Text = "800";
            // 
            // trackBarAreaMax
            // 
            this.trackBarAreaMax.Cursor = System.Windows.Forms.Cursors.Hand;
            this.trackBarAreaMax.Location = new System.Drawing.Point(228, 116);
            this.trackBarAreaMax.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.trackBarAreaMax.Maximum = 10000;
            this.trackBarAreaMax.Minimum = 1;
            this.trackBarAreaMax.Name = "trackBarAreaMax";
            this.trackBarAreaMax.Size = new System.Drawing.Size(159, 56);
            this.trackBarAreaMax.TabIndex = 10;
            this.trackBarAreaMax.Value = 800;
            this.trackBarAreaMax.ValueChanged += new System.EventHandler(this.trackBarAreaMax_ValueChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(225, 99);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(97, 15);
            this.label9.TabIndex = 9;
            this.label9.Text = "最大圆面积：";
            // 
            // lbAreaMin
            // 
            this.lbAreaMin.AutoSize = true;
            this.lbAreaMin.Location = new System.Drawing.Point(116, 99);
            this.lbAreaMin.Name = "lbAreaMin";
            this.lbAreaMin.Size = new System.Drawing.Size(31, 15);
            this.lbAreaMin.TabIndex = 8;
            this.lbAreaMin.Text = "500";
            // 
            // trackBarAreaMin
            // 
            this.trackBarAreaMin.Cursor = System.Windows.Forms.Cursors.Hand;
            this.trackBarAreaMin.Location = new System.Drawing.Point(16, 116);
            this.trackBarAreaMin.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.trackBarAreaMin.Maximum = 10000;
            this.trackBarAreaMin.Minimum = 1;
            this.trackBarAreaMin.Name = "trackBarAreaMin";
            this.trackBarAreaMin.Size = new System.Drawing.Size(159, 56);
            this.trackBarAreaMin.TabIndex = 7;
            this.trackBarAreaMin.Value = 500;
            this.trackBarAreaMin.ValueChanged += new System.EventHandler(this.trackBarAreaMin_ValueChanged);
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(13, 99);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(97, 15);
            this.label15.TabIndex = 6;
            this.label15.Text = "最小圆面积：";
            // 
            // lbThresholdMax
            // 
            this.lbThresholdMax.AutoSize = true;
            this.lbThresholdMax.Location = new System.Drawing.Point(313, 24);
            this.lbThresholdMax.Name = "lbThresholdMax";
            this.lbThresholdMax.Size = new System.Drawing.Size(31, 15);
            this.lbThresholdMax.TabIndex = 5;
            this.lbThresholdMax.Text = "125";
            // 
            // trackBarMax
            // 
            this.trackBarMax.Cursor = System.Windows.Forms.Cursors.Hand;
            this.trackBarMax.Location = new System.Drawing.Point(228, 41);
            this.trackBarMax.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.trackBarMax.Maximum = 255;
            this.trackBarMax.Name = "trackBarMax";
            this.trackBarMax.Size = new System.Drawing.Size(159, 56);
            this.trackBarMax.TabIndex = 4;
            this.trackBarMax.Value = 125;
            this.trackBarMax.ValueChanged += new System.EventHandler(this.trackBarMax_ValueChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(225, 24);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(82, 15);
            this.label8.TabIndex = 3;
            this.label8.Text = "最大阈值：";
            // 
            // lbThresholdMin
            // 
            this.lbThresholdMin.AutoSize = true;
            this.lbThresholdMin.Location = new System.Drawing.Point(101, 24);
            this.lbThresholdMin.Name = "lbThresholdMin";
            this.lbThresholdMin.Size = new System.Drawing.Size(15, 15);
            this.lbThresholdMin.TabIndex = 2;
            this.lbThresholdMin.Text = "0";
            // 
            // trackBarMin
            // 
            this.trackBarMin.Cursor = System.Windows.Forms.Cursors.Hand;
            this.trackBarMin.Location = new System.Drawing.Point(16, 41);
            this.trackBarMin.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.trackBarMin.Maximum = 255;
            this.trackBarMin.Name = "trackBarMin";
            this.trackBarMin.Size = new System.Drawing.Size(159, 56);
            this.trackBarMin.TabIndex = 1;
            this.trackBarMin.ValueChanged += new System.EventHandler(this.trackBarMin_ValueChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(13, 24);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(82, 15);
            this.label5.TabIndex = 0;
            this.label5.Text = "最小阈值：";
            // 
            // hObjectViewer1
            // 
            this.hObjectViewer1.ActiveTool = Halcon.Functions.ViewerTools.Arrow;
            this.hObjectViewer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.hObjectViewer1.IsShowCross = false;
            this.hObjectViewer1.Location = new System.Drawing.Point(12, 44);
            this.hObjectViewer1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.hObjectViewer1.Name = "hObjectViewer1";
            this.hObjectViewer1.ShowToolbar = true;
            this.hObjectViewer1.Size = new System.Drawing.Size(695, 601);
            this.hObjectViewer1.TabIndex = 28;
            // 
            // AutoCircleCalibrationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1138, 656);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.cbbCalibIndex);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.hObjectViewer1);
            this.Controls.Add(this.toolStrip1);
            this.Name = "AutoCircleCalibrationForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "自动标定";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.AutoCircleCalibrationForm_FormClosing);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownLeftY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownLeftX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownCol)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownRow)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownDisY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownDisX)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarAreaMax)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarAreaMin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarMax)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarMin)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Halcon.Functions.HObjectViewer hObjectViewer1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.CheckBox cbTest;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label labelAutoCircle;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.NumericUpDown numericUpDownCol;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown numericUpDownRow;
        private System.Windows.Forms.NumericUpDown numericUpDownDisY;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown numericUpDownDisX;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ComboBox cbbCalibIndex;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label lbThresholdMax;
        private System.Windows.Forms.TrackBar trackBarMax;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label lbThresholdMin;
        private System.Windows.Forms.TrackBar trackBarMin;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label labelY;
        private System.Windows.Forms.Label labelX;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label lbAreaMax;
        private System.Windows.Forms.TrackBar trackBarAreaMax;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label lbAreaMin;
        private System.Windows.Forms.TrackBar trackBarAreaMin;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.NumericUpDown numericUpDownLeftY;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.NumericUpDown numericUpDownLeftX;
        private System.Windows.Forms.Label label16;
    }
}

