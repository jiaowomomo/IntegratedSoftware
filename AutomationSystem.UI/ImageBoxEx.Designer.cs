namespace AutomationSystem.UI
{
    partial class ImageBoxEx
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
            this.pbCamera = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pbCamera)).BeginInit();
            this.SuspendLayout();
            // 
            // pbCamera
            // 
            this.pbCamera.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.pbCamera.Location = new System.Drawing.Point(0, 0);
            this.pbCamera.Margin = new System.Windows.Forms.Padding(0);
            this.pbCamera.Name = "pbCamera";
            this.pbCamera.Size = new System.Drawing.Size(620, 520);
            this.pbCamera.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbCamera.TabIndex = 2;
            this.pbCamera.TabStop = false;
            this.pbCamera.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pbCamera_MouseDown);
            this.pbCamera.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pbCamera_MouseMove);
            this.pbCamera.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pbCamera_MouseUp);
            // 
            // ImageBoxEx
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.Controls.Add(this.pbCamera);
            this.Name = "ImageBoxEx";
            this.Size = new System.Drawing.Size(620, 520);
            this.Resize += new System.EventHandler(this.ImageBoxEx_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.pbCamera)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.PictureBox pbCamera;
    }
}
