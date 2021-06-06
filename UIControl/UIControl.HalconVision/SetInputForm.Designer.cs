namespace UIControl.HalconVision
{
    partial class SetInputForm
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.listViewInput = new System.Windows.Forms.ListView();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.listViewOutput = new System.Windows.Forms.ListView();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.listViewParameter = new System.Windows.Forms.ListView();
            this.button3 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.listViewInput);
            this.groupBox1.Location = new System.Drawing.Point(19, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(200, 426);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "输入参数";
            // 
            // listViewInput
            // 
            this.listViewInput.BackColor = System.Drawing.Color.White;
            this.listViewInput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewInput.FullRowSelect = true;
            this.listViewInput.HideSelection = false;
            this.listViewInput.Location = new System.Drawing.Point(3, 21);
            this.listViewInput.MultiSelect = false;
            this.listViewInput.Name = "listViewInput";
            this.listViewInput.Size = new System.Drawing.Size(194, 402);
            this.listViewInput.TabIndex = 1;
            this.listViewInput.UseCompatibleStateImageBehavior = false;
            this.listViewInput.View = System.Windows.Forms.View.List;
            this.listViewInput.SelectedIndexChanged += new System.EventHandler(this.listViewInput_SelectedIndexChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.listViewOutput);
            this.groupBox2.Location = new System.Drawing.Point(225, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(200, 426);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "可选输入对象";
            // 
            // listViewOutput
            // 
            this.listViewOutput.BackColor = System.Drawing.Color.White;
            this.listViewOutput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewOutput.FullRowSelect = true;
            this.listViewOutput.HideSelection = false;
            this.listViewOutput.Location = new System.Drawing.Point(3, 21);
            this.listViewOutput.MultiSelect = false;
            this.listViewOutput.Name = "listViewOutput";
            this.listViewOutput.Size = new System.Drawing.Size(194, 402);
            this.listViewOutput.TabIndex = 1;
            this.listViewOutput.UseCompatibleStateImageBehavior = false;
            this.listViewOutput.View = System.Windows.Forms.View.List;
            this.listViewOutput.SelectedIndexChanged += new System.EventHandler(this.listViewOutput_SelectedIndexChanged);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.listViewParameter);
            this.groupBox3.Location = new System.Drawing.Point(431, 12);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(200, 426);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "对象输出参数";
            // 
            // listViewParameter
            // 
            this.listViewParameter.BackColor = System.Drawing.Color.White;
            this.listViewParameter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewParameter.FullRowSelect = true;
            this.listViewParameter.HideSelection = false;
            this.listViewParameter.Location = new System.Drawing.Point(3, 21);
            this.listViewParameter.MultiSelect = false;
            this.listViewParameter.Name = "listViewParameter";
            this.listViewParameter.Size = new System.Drawing.Size(194, 402);
            this.listViewParameter.TabIndex = 1;
            this.listViewParameter.UseCompatibleStateImageBehavior = false;
            this.listViewParameter.View = System.Windows.Forms.View.List;
            this.listViewParameter.SelectedIndexChanged += new System.EventHandler(this.listViewParameter_SelectedIndexChanged);
            // 
            // button3
            // 
            this.button3.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button3.Location = new System.Drawing.Point(638, 409);
            this.button3.Margin = new System.Windows.Forms.Padding(4);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(100, 29);
            this.button3.TabIndex = 52;
            this.button3.Text = "取消";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button2.Location = new System.Drawing.Point(638, 371);
            this.button2.Margin = new System.Windows.Forms.Padding(4);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(100, 29);
            this.button2.TabIndex = 51;
            this.button2.Text = "确定";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // SetInputForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(749, 450);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "SetInputForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "输入设置";
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ListView listViewInput;
        private System.Windows.Forms.ListView listViewOutput;
        private System.Windows.Forms.ListView listViewParameter;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button2;
    }
}