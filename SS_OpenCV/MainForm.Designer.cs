namespace SS_OpenCV
{
    partial class MainForm
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
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.undoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.imageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.colorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.convertToToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.grayToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.grayRedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.grayGreenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.grayBlueToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bWToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.negativeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.brightnessContrastToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.transformsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.translationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rotationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.zoomToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.filtersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.autoZoomToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.autoresToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1 = new System.Windows.Forms.Panel();
            this.ImageViewer = new System.Windows.Forms.PictureBox();
            this.menuStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ImageViewer)).BeginInit();
            this.SuspendLayout();
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.Filter = "Images (*.png, *.bmp, *.jpg)|*.png;*.bmp;*.jpg";
            this.openFileDialog1.FileOk += new System.ComponentModel.CancelEventHandler(this.openFileDialog1_FileOk);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.imageToolStripMenuItem,
            this.autoresToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(681, 24);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.toolStripMenuItem1,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(123, 22);
            this.openToolStripMenuItem.Text = "Open...";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(123, 22);
            this.saveToolStripMenuItem.Text = "Save As...";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(120, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(123, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.undoToolStripMenuItem});
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.editToolStripMenuItem.Text = "Edit";
            // 
            // undoToolStripMenuItem
            // 
            this.undoToolStripMenuItem.Name = "undoToolStripMenuItem";
            this.undoToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.undoToolStripMenuItem.Text = "Undo";
            this.undoToolStripMenuItem.Click += new System.EventHandler(this.undoToolStripMenuItem_Click);
            // 
            // imageToolStripMenuItem
            // 
            this.imageToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.colorToolStripMenuItem,
            this.transformsToolStripMenuItem,
            this.filtersToolStripMenuItem,
            this.autoZoomToolStripMenuItem});
            this.imageToolStripMenuItem.Name = "imageToolStripMenuItem";
            this.imageToolStripMenuItem.Size = new System.Drawing.Size(52, 20);
            this.imageToolStripMenuItem.Text = "Image";
            // 
            // colorToolStripMenuItem
            // 
            this.colorToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.convertToToolStripMenuItem,
            this.negativeToolStripMenuItem,
            this.brightnessContrastToolStripMenuItem});
            this.colorToolStripMenuItem.Name = "colorToolStripMenuItem";
            this.colorToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.colorToolStripMenuItem.Text = "Color";
            // 
            // convertToToolStripMenuItem
            // 
            this.convertToToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.grayToolStripMenuItem,
            this.grayRedToolStripMenuItem,
            this.grayGreenToolStripMenuItem,
            this.grayBlueToolStripMenuItem,
            this.bWToolStripMenuItem});
            this.convertToToolStripMenuItem.Name = "convertToToolStripMenuItem";
            this.convertToToolStripMenuItem.Size = new System.Drawing.Size(179, 22);
            this.convertToToolStripMenuItem.Text = "Convert To";
            // 
            // grayToolStripMenuItem
            // 
            this.grayToolStripMenuItem.Name = "grayToolStripMenuItem";
            this.grayToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.grayToolStripMenuItem.Text = "Gray (Average)";
            this.grayToolStripMenuItem.Click += new System.EventHandler(this.grayToolStripMenuItem_Click);
            // 
            // grayRedToolStripMenuItem
            // 
            this.grayRedToolStripMenuItem.Name = "grayRedToolStripMenuItem";
            this.grayRedToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.grayRedToolStripMenuItem.Text = "Gray (Red)";
            this.grayRedToolStripMenuItem.Click += new System.EventHandler(this.grayRedToolStripMenuItem_Click);
            // 
            // grayGreenToolStripMenuItem
            // 
            this.grayGreenToolStripMenuItem.Name = "grayGreenToolStripMenuItem";
            this.grayGreenToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.grayGreenToolStripMenuItem.Text = "Gray (Green)";
            this.grayGreenToolStripMenuItem.Click += new System.EventHandler(this.grayGreenToolStripMenuItem_Click);
            // 
            // grayBlueToolStripMenuItem
            // 
            this.grayBlueToolStripMenuItem.Name = "grayBlueToolStripMenuItem";
            this.grayBlueToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.grayBlueToolStripMenuItem.Text = "Gray (Blue)";
            this.grayBlueToolStripMenuItem.Click += new System.EventHandler(this.grayBlueToolStripMenuItem_Click);
            // 
            // bWToolStripMenuItem
            // 
            this.bWToolStripMenuItem.Name = "bWToolStripMenuItem";
            this.bWToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.bWToolStripMenuItem.Text = "B&W";
            // 
            // negativeToolStripMenuItem
            // 
            this.negativeToolStripMenuItem.Name = "negativeToolStripMenuItem";
            this.negativeToolStripMenuItem.Size = new System.Drawing.Size(179, 22);
            this.negativeToolStripMenuItem.Text = "Negative";
            this.negativeToolStripMenuItem.Click += new System.EventHandler(this.negativeToolStripMenuItem_Click);
            // 
            // brightnessContrastToolStripMenuItem
            // 
            this.brightnessContrastToolStripMenuItem.Name = "brightnessContrastToolStripMenuItem";
            this.brightnessContrastToolStripMenuItem.Size = new System.Drawing.Size(179, 22);
            this.brightnessContrastToolStripMenuItem.Text = "Brightness/Contrast";
            this.brightnessContrastToolStripMenuItem.Click += new System.EventHandler(this.brightnessContrastToolStripMenuItem_Click);
            // 
            // transformsToolStripMenuItem
            // 
            this.transformsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.translationToolStripMenuItem,
            this.rotationToolStripMenuItem,
            this.zoomToolStripMenuItem});
            this.transformsToolStripMenuItem.Name = "transformsToolStripMenuItem";
            this.transformsToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.transformsToolStripMenuItem.Text = "Transforms";
            // 
            // translationToolStripMenuItem
            // 
            this.translationToolStripMenuItem.Name = "translationToolStripMenuItem";
            this.translationToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.translationToolStripMenuItem.Text = "Translation";
            this.translationToolStripMenuItem.Click += new System.EventHandler(this.translationToolStripMenuItem_Click);
            // 
            // rotationToolStripMenuItem
            // 
            this.rotationToolStripMenuItem.Name = "rotationToolStripMenuItem";
            this.rotationToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.rotationToolStripMenuItem.Text = "Rotation";
            this.rotationToolStripMenuItem.Click += new System.EventHandler(this.rotationToolStripMenuItem_Click);
            // 
            // zoomToolStripMenuItem
            // 
            this.zoomToolStripMenuItem.Name = "zoomToolStripMenuItem";
            this.zoomToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.zoomToolStripMenuItem.Text = "Zoom";
            // 
            // filtersToolStripMenuItem
            // 
            this.filtersToolStripMenuItem.Name = "filtersToolStripMenuItem";
            this.filtersToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.filtersToolStripMenuItem.Text = "Filters";
            // 
            // autoZoomToolStripMenuItem
            // 
            this.autoZoomToolStripMenuItem.CheckOnClick = true;
            this.autoZoomToolStripMenuItem.Name = "autoZoomToolStripMenuItem";
            this.autoZoomToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.autoZoomToolStripMenuItem.Text = "Auto Zoom";
            this.autoZoomToolStripMenuItem.Click += new System.EventHandler(this.autoZoomToolStripMenuItem_Click);
            // 
            // autoresToolStripMenuItem
            // 
            this.autoresToolStripMenuItem.Name = "autoresToolStripMenuItem";
            this.autoresToolStripMenuItem.Size = new System.Drawing.Size(69, 20);
            this.autoresToolStripMenuItem.Text = "Autores...";
            this.autoresToolStripMenuItem.Click += new System.EventHandler(this.autoresToolStripMenuItem_Click);
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.ImageViewer);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 24);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(681, 361);
            this.panel1.TabIndex = 6;
            // 
            // ImageViewer
            // 
            this.ImageViewer.Location = new System.Drawing.Point(0, 0);
            this.ImageViewer.Name = "ImageViewer";
            this.ImageViewer.Size = new System.Drawing.Size(576, 427);
            this.ImageViewer.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.ImageViewer.TabIndex = 6;
            this.ImageViewer.TabStop = false;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(681, 385);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.Text = "Sistemas Sensoriais 2016/2017 - Image processing";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ImageViewer)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem imageToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem colorToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem convertToToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem grayToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem bWToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem negativeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem transformsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem translationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem rotationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem zoomToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem filtersToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem autoresToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem undoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem autoZoomToolStripMenuItem;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox ImageViewer;
        private System.Windows.Forms.ToolStripMenuItem grayRedToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem grayGreenToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem grayBlueToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem brightnessContrastToolStripMenuItem;
    }
}

