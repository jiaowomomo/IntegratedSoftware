
namespace CommonLibrary.CodeSystem.Controls
{
    partial class CustomReferenceForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CustomReferenceForm));
            this.listViewCustomReference = new System.Windows.Forms.ListView();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItemObtain = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemRefresh = new System.Windows.Forms.ToolStripMenuItem();
            this.treeViewReference = new System.Windows.Forms.TreeView();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // listViewCustomReference
            // 
            this.listViewCustomReference.ContextMenuStrip = this.contextMenuStrip1;
            this.listViewCustomReference.Dock = System.Windows.Forms.DockStyle.Left;
            this.listViewCustomReference.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listViewCustomReference.HideSelection = false;
            this.listViewCustomReference.Location = new System.Drawing.Point(0, 0);
            this.listViewCustomReference.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.listViewCustomReference.Name = "listViewCustomReference";
            this.listViewCustomReference.Size = new System.Drawing.Size(229, 540);
            this.listViewCustomReference.TabIndex = 0;
            this.listViewCustomReference.UseCompatibleStateImageBehavior = false;
            this.listViewCustomReference.View = System.Windows.Forms.View.List;
            this.listViewCustomReference.DoubleClick += new System.EventHandler(this.listViewCustomReference_DoubleClick);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemObtain,
            this.toolStripMenuItemRefresh});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(109, 52);
            // 
            // toolStripMenuItemObtain
            // 
            this.toolStripMenuItemObtain.Name = "toolStripMenuItemObtain";
            this.toolStripMenuItemObtain.Size = new System.Drawing.Size(108, 24);
            this.toolStripMenuItemObtain.Text = "获取";
            this.toolStripMenuItemObtain.Click += new System.EventHandler(this.toolStripMenuItemObtain_Click);
            // 
            // toolStripMenuItemRefresh
            // 
            this.toolStripMenuItemRefresh.Name = "toolStripMenuItemRefresh";
            this.toolStripMenuItemRefresh.Size = new System.Drawing.Size(108, 24);
            this.toolStripMenuItemRefresh.Text = "刷新";
            this.toolStripMenuItemRefresh.Click += new System.EventHandler(this.toolStripMenuItemRefresh_Click);
            // 
            // treeViewReference
            // 
            this.treeViewReference.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeViewReference.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.treeViewReference.ImageIndex = 0;
            this.treeViewReference.ImageList = this.imageList1;
            this.treeViewReference.Location = new System.Drawing.Point(229, 0);
            this.treeViewReference.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.treeViewReference.Name = "treeViewReference";
            this.treeViewReference.SelectedImageIndex = 0;
            this.treeViewReference.Size = new System.Drawing.Size(571, 540);
            this.treeViewReference.TabIndex = 1;
            this.treeViewReference.KeyDown += new System.Windows.Forms.KeyEventHandler(this.treeViewReference_KeyDown);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "Type.png");
            this.imageList1.Images.SetKeyName(1, "Namespace.png");
            this.imageList1.Images.SetKeyName(2, "Field.png");
            this.imageList1.Images.SetKeyName(3, "Property.png");
            this.imageList1.Images.SetKeyName(4, "Method.png");
            this.imageList1.Images.SetKeyName(5, "Content.png");
            this.imageList1.Images.SetKeyName(6, "Select.png");
            // 
            // splitter1
            // 
            this.splitter1.Location = new System.Drawing.Point(229, 0);
            this.splitter1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(3, 540);
            this.splitter1.TabIndex = 2;
            this.splitter1.TabStop = false;
            // 
            // CustomReferenceForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 540);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.treeViewReference);
            this.Controls.Add(this.listViewCustomReference);
            this.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "CustomReferenceForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "CustomReferenceForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CustomReferenceForm_FormClosing);
            this.Load += new System.EventHandler(this.CustomReferenceForm_Load);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView listViewCustomReference;
        private System.Windows.Forms.TreeView treeViewReference;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemObtain;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemRefresh;
        private System.Windows.Forms.ImageList imageList1;
    }
}