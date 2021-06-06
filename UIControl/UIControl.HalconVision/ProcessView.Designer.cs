namespace UIControl.HalconVision
{
    partial class ProcessView
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
            this.components = new System.ComponentModel.Container();
            this.listView1 = new System.Windows.Forms.ListView();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItemSetInput = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemRunToCurrent = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemRunCurrent = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemEdit = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemPrevious = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemNext = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemClear = new System.Windows.Forms.ToolStripMenuItem();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.contextMenuStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // listView1
            // 
            this.listView1.ContextMenuStrip = this.contextMenuStrip1;
            this.listView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView1.HideSelection = false;
            this.listView1.Location = new System.Drawing.Point(0, 0);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(248, 529);
            this.listView1.TabIndex = 0;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.List;
            this.listView1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listView1_MouseDoubleClick);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemSetInput,
            this.toolStripMenuItemRunToCurrent,
            this.toolStripMenuItemRunCurrent,
            this.toolStripMenuItemEdit,
            this.toolStripMenuItemDelete,
            this.toolStripMenuItemPrevious,
            this.toolStripMenuItemNext,
            this.toolStripMenuItemClear});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(184, 196);
            // 
            // toolStripMenuItemSetInput
            // 
            this.toolStripMenuItemSetInput.Name = "toolStripMenuItemSetInput";
            this.toolStripMenuItemSetInput.Size = new System.Drawing.Size(183, 24);
            this.toolStripMenuItemSetInput.Text = "输入设置";
            this.toolStripMenuItemSetInput.Click += new System.EventHandler(this.toolStripMenuItemSetInput_Click);
            // 
            // toolStripMenuItemRunToCurrent
            // 
            this.toolStripMenuItemRunToCurrent.Name = "toolStripMenuItemRunToCurrent";
            this.toolStripMenuItemRunToCurrent.Size = new System.Drawing.Size(183, 24);
            this.toolStripMenuItemRunToCurrent.Text = "运行到当前对象";
            this.toolStripMenuItemRunToCurrent.Click += new System.EventHandler(this.toolStripMenuItemRunToCurrent_Click);
            // 
            // toolStripMenuItemRunCurrent
            // 
            this.toolStripMenuItemRunCurrent.Name = "toolStripMenuItemRunCurrent";
            this.toolStripMenuItemRunCurrent.Size = new System.Drawing.Size(183, 24);
            this.toolStripMenuItemRunCurrent.Text = "运行当前对象";
            this.toolStripMenuItemRunCurrent.Click += new System.EventHandler(this.toolStripMenuItemRunCurrent_Click);
            // 
            // toolStripMenuItemEdit
            // 
            this.toolStripMenuItemEdit.Name = "toolStripMenuItemEdit";
            this.toolStripMenuItemEdit.Size = new System.Drawing.Size(183, 24);
            this.toolStripMenuItemEdit.Text = "编辑";
            this.toolStripMenuItemEdit.Click += new System.EventHandler(this.toolStripMenuItemEdit_Click);
            // 
            // toolStripMenuItemDelete
            // 
            this.toolStripMenuItemDelete.Name = "toolStripMenuItemDelete";
            this.toolStripMenuItemDelete.Size = new System.Drawing.Size(183, 24);
            this.toolStripMenuItemDelete.Text = "删除";
            this.toolStripMenuItemDelete.Click += new System.EventHandler(this.toolStripMenuItemDelete_Click);
            // 
            // toolStripMenuItemPrevious
            // 
            this.toolStripMenuItemPrevious.Name = "toolStripMenuItemPrevious";
            this.toolStripMenuItemPrevious.Size = new System.Drawing.Size(183, 24);
            this.toolStripMenuItemPrevious.Text = "上移";
            this.toolStripMenuItemPrevious.Click += new System.EventHandler(this.toolStripMenuItemPrevious_Click);
            // 
            // toolStripMenuItemNext
            // 
            this.toolStripMenuItemNext.Name = "toolStripMenuItemNext";
            this.toolStripMenuItemNext.Size = new System.Drawing.Size(183, 24);
            this.toolStripMenuItemNext.Text = "下移";
            this.toolStripMenuItemNext.Click += new System.EventHandler(this.toolStripMenuItemNext_Click);
            // 
            // toolStripMenuItemClear
            // 
            this.toolStripMenuItemClear.Name = "toolStripMenuItemClear";
            this.toolStripMenuItemClear.Size = new System.Drawing.Size(183, 24);
            this.toolStripMenuItemClear.Text = "清空";
            this.toolStripMenuItemClear.Click += new System.EventHandler(this.toolStripMenuItemClear_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(35, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(52, 15);
            this.label1.TabIndex = 1;
            this.label1.Text = "流程号";
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(93, 9);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 23);
            this.comboBox1.TabIndex = 2;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.LightSkyBlue;
            this.panel1.Controls.Add(this.comboBox1);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(248, 40);
            this.panel1.TabIndex = 3;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.listView1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 40);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(248, 529);
            this.panel2.TabIndex = 4;
            // 
            // ProcessView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "ProcessView";
            this.Size = new System.Drawing.Size(248, 569);
            this.contextMenuStrip1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemSetInput;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemDelete;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemPrevious;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemNext;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemEdit;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemClear;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemRunToCurrent;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemRunCurrent;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
    }
}
