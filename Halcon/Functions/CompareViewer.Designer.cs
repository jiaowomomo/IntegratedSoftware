namespace Halcon.Functions
{
    partial class CompareViewer
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.hObjectViewerSource = new Halcon.Functions.HObjectViewer();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.hObjectViewerCompare = new Halcon.Functions.HObjectViewer();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.hObjectViewerSource);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(432, 570);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "原图";
            // 
            // hObjectViewerSource
            // 
            this.hObjectViewerSource.ActiveTool = Halcon.Functions.ViewerTools.Arrow;
            this.hObjectViewerSource.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.hObjectViewerSource.Dock = System.Windows.Forms.DockStyle.Fill;
            this.hObjectViewerSource.IsShowCross = false;
            this.hObjectViewerSource.Location = new System.Drawing.Point(3, 21);
            this.hObjectViewerSource.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.hObjectViewerSource.Name = "hObjectViewerSource";
            this.hObjectViewerSource.ShowToolbar = false;
            this.hObjectViewerSource.Size = new System.Drawing.Size(426, 546);
            this.hObjectViewerSource.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.hObjectViewerCompare);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(441, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(433, 570);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "处理后";
            // 
            // hObjectViewerCompare
            // 
            this.hObjectViewerCompare.ActiveTool = Halcon.Functions.ViewerTools.Arrow;
            this.hObjectViewerCompare.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.hObjectViewerCompare.Dock = System.Windows.Forms.DockStyle.Fill;
            this.hObjectViewerCompare.IsShowCross = false;
            this.hObjectViewerCompare.Location = new System.Drawing.Point(3, 21);
            this.hObjectViewerCompare.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.hObjectViewerCompare.Name = "hObjectViewerCompare";
            this.hObjectViewerCompare.ShowToolbar = false;
            this.hObjectViewerCompare.Size = new System.Drawing.Size(427, 546);
            this.hObjectViewerCompare.TabIndex = 1;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.groupBox1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.groupBox2, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(877, 576);
            this.tableLayoutPanel1.TabIndex = 2;
            // 
            // CompareViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "CompareViewer";
            this.Size = new System.Drawing.Size(877, 576);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private HObjectViewer hObjectViewerSource;
        private HObjectViewer hObjectViewerCompare;
    }
}
