namespace Halcon.TextShow
{
    partial class TextShowFormNew
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
            this.propertiesViewer1 = new CommonLibrary.Controls.PropertiesViewer();
            this.hObjectViewer1 = new Halcon.Functions.HObjectViewer();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonOK = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // propertiesViewer1
            // 
            this.propertiesViewer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertiesViewer1.Location = new System.Drawing.Point(0, 0);
            this.propertiesViewer1.Name = "propertiesViewer1";
            this.propertiesViewer1.Size = new System.Drawing.Size(958, 556);
            this.propertiesViewer1.TabIndex = 0;
            // 
            // hObjectViewer1
            // 
            this.hObjectViewer1.ActiveTool = Halcon.Functions.ViewerTools.Arrow;
            this.hObjectViewer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.hObjectViewer1.IsShowCross = false;
            this.hObjectViewer1.Location = new System.Drawing.Point(0, 0);
            this.hObjectViewer1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.hObjectViewer1.Name = "hObjectViewer1";
            this.hObjectViewer1.ShowToolbar = false;
            this.hObjectViewer1.Size = new System.Drawing.Size(678, 543);
            this.hObjectViewer1.TabIndex = 43;
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(835, 514);
            this.buttonCancel.Margin = new System.Windows.Forms.Padding(4);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(100, 29);
            this.buttonCancel.TabIndex = 68;
            this.buttonCancel.Text = "取消";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // buttonOK
            // 
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Location = new System.Drawing.Point(699, 514);
            this.buttonOK.Margin = new System.Windows.Forms.Padding(4);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(100, 29);
            this.buttonOK.TabIndex = 67;
            this.buttonOK.Text = "确定";
            this.buttonOK.UseVisualStyleBackColor = true;
            // 
            // TextShowFormNew
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(958, 556);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.hObjectViewer1);
            this.Controls.Add(this.propertiesViewer1);
            this.Name = "TextShowFormNew";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "文字显示";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.TextShowFormNew_FormClosing);
            this.ResumeLayout(false);

        }

        #endregion

        private CommonLibrary.Controls.PropertiesViewer propertiesViewer1;
        private Halcon.Functions.HObjectViewer hObjectViewer1;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonOK;
    }
}