namespace AutomationSystem
{
    partial class ProcessForm
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
            this.processViewNew1 = new UIControl.HalconVision.ProcessViewNew();
            this.SuspendLayout();
            // 
            // processViewNew1
            // 
            this.processViewNew1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.processViewNew1.Location = new System.Drawing.Point(0, 0);
            this.processViewNew1.Name = "processViewNew1";
            this.processViewNew1.Size = new System.Drawing.Size(236, 585);
            this.processViewNew1.TabIndex = 0;
            // 
            // ProcessForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(236, 585);
            this.Controls.Add(this.processViewNew1);
            this.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Name = "ProcessForm";
            this.Text = "流程列表";
            this.ResumeLayout(false);

        }

        #endregion

        private UIControl.HalconVision.ProcessViewNew processViewNew1;
    }
}