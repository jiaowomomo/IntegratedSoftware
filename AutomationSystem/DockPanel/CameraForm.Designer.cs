namespace AutomationSystem
{
    partial class CameraForm
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
            this.hCameraWindow1 = new UIControl.HalconVision.HCameraWindow();
            this.SuspendLayout();
            // 
            // hCameraWindow1
            // 
            this.hCameraWindow1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.hCameraWindow1.Location = new System.Drawing.Point(0, 0);
            this.hCameraWindow1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.hCameraWindow1.Name = "hCameraWindow1";
            this.hCameraWindow1.Size = new System.Drawing.Size(691, 532);
            this.hCameraWindow1.TabIndex = 0;
            // 
            // CameraForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(691, 532);
            this.Controls.Add(this.hCameraWindow1);
            this.DockAreas = WeifenLuo.WinFormsUI.Docking.DockAreas.Document;
            this.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Name = "CameraForm";
            this.Text = "相机";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CameraForm_FormClosing);
            this.ResumeLayout(false);

        }

        #endregion

        private UIControl.HalconVision.HCameraWindow hCameraWindow1;
    }
}