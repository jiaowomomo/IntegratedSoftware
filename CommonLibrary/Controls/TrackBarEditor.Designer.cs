namespace CommonLibrary.Controls
{
    partial class TrackBarEditor
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
            this.trackBarValue = new System.Windows.Forms.TrackBar();
            this.labelValue = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarValue)).BeginInit();
            this.SuspendLayout();
            // 
            // trackBarValue
            // 
            this.trackBarValue.Location = new System.Drawing.Point(64, 3);
            this.trackBarValue.Maximum = 255;
            this.trackBarValue.Name = "trackBarValue";
            this.trackBarValue.Size = new System.Drawing.Size(156, 56);
            this.trackBarValue.TabIndex = 3;
            this.trackBarValue.ValueChanged += new System.EventHandler(this.trackBarValue_ValueChanged);
            // 
            // labelValue
            // 
            this.labelValue.AutoSize = true;
            this.labelValue.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelValue.Location = new System.Drawing.Point(3, 18);
            this.labelValue.Name = "labelValue";
            this.labelValue.Size = new System.Drawing.Size(24, 27);
            this.labelValue.TabIndex = 2;
            this.labelValue.Text = "0";
            // 
            // TrackBarEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.trackBarValue);
            this.Controls.Add(this.labelValue);
            this.Name = "TrackBarEditor";
            this.Size = new System.Drawing.Size(225, 63);
            ((System.ComponentModel.ISupportInitialize)(this.trackBarValue)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TrackBar trackBarValue;
        private System.Windows.Forms.Label labelValue;
    }
}
