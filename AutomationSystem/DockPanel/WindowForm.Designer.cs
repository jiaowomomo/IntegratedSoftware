namespace AutomationSystem
{
    partial class WindowForm
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
            this.hShowWindow1 = new UIControl.HalconVision.HShowWindow();
            this.SuspendLayout();
            // 
            // hShowWindow1
            // 
            this.hShowWindow1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.hShowWindow1.Location = new System.Drawing.Point(0, 0);
            this.hShowWindow1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.hShowWindow1.Name = "hShowWindow1";
            this.hShowWindow1.Size = new System.Drawing.Size(626, 450);
            this.hShowWindow1.TabIndex = 0;
            // 
            // WindowForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(626, 450);
            this.Controls.Add(this.hShowWindow1);
            this.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Name = "WindowForm";
            this.Text = "WindowForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.WindowForm_FormClosing);
            this.ResumeLayout(false);

        }

        #endregion

        private UIControl.HalconVision.HShowWindow hShowWindow1;
    }
}