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
            this.negativeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.brightnessContrastToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.transformsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.translationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rotationToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.rotationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bilinearToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.zoomToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.zoomToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bilinearToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.filtersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.meanNoiseReductionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.x3ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.nonUniformFiltersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.edgeDetectionFiltersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sobelFilterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.differentialToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.robertsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.medianFilterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.binarizationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.manualToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.otsuToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showHistogramToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.intensityToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rGBToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.allToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.autoZoomToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.autoresToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.evalFormToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
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
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.imageToolStripMenuItem,
            this.autoresToolStripMenuItem,
            this.evalFormToolStripMenuItem});
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
            this.showHistogramToolStripMenuItem,
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
            this.brightnessContrastToolStripMenuItem,
            this.binarizationToolStripMenuItem});
            this.colorToolStripMenuItem.Name = "colorToolStripMenuItem";
            this.colorToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.colorToolStripMenuItem.Text = "Color";
            // 
            // convertToToolStripMenuItem
            // 
            this.convertToToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.grayToolStripMenuItem,
            this.grayRedToolStripMenuItem,
            this.grayGreenToolStripMenuItem,
            this.grayBlueToolStripMenuItem});
            this.convertToToolStripMenuItem.Name = "convertToToolStripMenuItem";
            this.convertToToolStripMenuItem.Size = new System.Drawing.Size(179, 22);
            this.convertToToolStripMenuItem.Text = "Grayscale";
            // 
            // grayToolStripMenuItem
            // 
            this.grayToolStripMenuItem.Name = "grayToolStripMenuItem";
            this.grayToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.grayToolStripMenuItem.Text = "By Average";
            this.grayToolStripMenuItem.Click += new System.EventHandler(this.grayToolStripMenuItem_Click);
            // 
            // grayRedToolStripMenuItem
            // 
            this.grayRedToolStripMenuItem.Name = "grayRedToolStripMenuItem";
            this.grayRedToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.grayRedToolStripMenuItem.Text = "By Red component";
            this.grayRedToolStripMenuItem.Click += new System.EventHandler(this.grayRedToolStripMenuItem_Click);
            // 
            // grayGreenToolStripMenuItem
            // 
            this.grayGreenToolStripMenuItem.Name = "grayGreenToolStripMenuItem";
            this.grayGreenToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.grayGreenToolStripMenuItem.Text = "By Green component";
            this.grayGreenToolStripMenuItem.Click += new System.EventHandler(this.grayGreenToolStripMenuItem_Click);
            // 
            // grayBlueToolStripMenuItem
            // 
            this.grayBlueToolStripMenuItem.Name = "grayBlueToolStripMenuItem";
            this.grayBlueToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.grayBlueToolStripMenuItem.Text = "By Blue component";
            this.grayBlueToolStripMenuItem.Click += new System.EventHandler(this.grayBlueToolStripMenuItem_Click);
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
            this.rotationToolStripMenuItem1,
            this.zoomToolStripMenuItem1});
            this.transformsToolStripMenuItem.Name = "transformsToolStripMenuItem";
            this.transformsToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.transformsToolStripMenuItem.Text = "Transformations";
            // 
            // translationToolStripMenuItem
            // 
            this.translationToolStripMenuItem.Name = "translationToolStripMenuItem";
            this.translationToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.translationToolStripMenuItem.Text = "Translation";
            this.translationToolStripMenuItem.Click += new System.EventHandler(this.translationToolStripMenuItem_Click);
            // 
            // rotationToolStripMenuItem1
            // 
            this.rotationToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.rotationToolStripMenuItem,
            this.bilinearToolStripMenuItem});
            this.rotationToolStripMenuItem1.Name = "rotationToolStripMenuItem1";
            this.rotationToolStripMenuItem1.Size = new System.Drawing.Size(152, 22);
            this.rotationToolStripMenuItem1.Text = "Rotation";
            // 
            // rotationToolStripMenuItem
            // 
            this.rotationToolStripMenuItem.Name = "rotationToolStripMenuItem";
            this.rotationToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.rotationToolStripMenuItem.Text = "Nearest Neighbour";
            this.rotationToolStripMenuItem.Click += new System.EventHandler(this.rotationToolStripMenuItem_Click);
            // 
            // bilinearToolStripMenuItem
            // 
            this.bilinearToolStripMenuItem.Name = "bilinearToolStripMenuItem";
            this.bilinearToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.bilinearToolStripMenuItem.Text = "Bilinear";
            this.bilinearToolStripMenuItem.Click += new System.EventHandler(this.bilinearToolStripMenuItem_Click);
            // 
            // zoomToolStripMenuItem1
            // 
            this.zoomToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.zoomToolStripMenuItem,
            this.bilinearToolStripMenuItem1});
            this.zoomToolStripMenuItem1.Name = "zoomToolStripMenuItem1";
            this.zoomToolStripMenuItem1.Size = new System.Drawing.Size(152, 22);
            this.zoomToolStripMenuItem1.Text = "Zoom";
            // 
            // zoomToolStripMenuItem
            // 
            this.zoomToolStripMenuItem.Name = "zoomToolStripMenuItem";
            this.zoomToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.zoomToolStripMenuItem.Text = "Nearest Neighbour";
            this.zoomToolStripMenuItem.Click += new System.EventHandler(this.zoomToolStripMenuItem_Click);
            // 
            // bilinearToolStripMenuItem1
            // 
            this.bilinearToolStripMenuItem1.Name = "bilinearToolStripMenuItem1";
            this.bilinearToolStripMenuItem1.Size = new System.Drawing.Size(174, 22);
            this.bilinearToolStripMenuItem1.Text = "Bilinear";
            this.bilinearToolStripMenuItem1.Click += new System.EventHandler(this.bilinearToolStripMenuItem1_Click);
            // 
            // filtersToolStripMenuItem
            // 
            this.filtersToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.meanNoiseReductionToolStripMenuItem,
            this.nonUniformFiltersToolStripMenuItem,
            this.edgeDetectionFiltersToolStripMenuItem,
            this.medianFilterToolStripMenuItem});
            this.filtersToolStripMenuItem.Name = "filtersToolStripMenuItem";
            this.filtersToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.filtersToolStripMenuItem.Text = "Filters";
            // 
            // meanNoiseReductionToolStripMenuItem
            // 
            this.meanNoiseReductionToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.x3ToolStripMenuItem});
            this.meanNoiseReductionToolStripMenuItem.Name = "meanNoiseReductionToolStripMenuItem";
            this.meanNoiseReductionToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
            this.meanNoiseReductionToolStripMenuItem.Text = "Mean Noise Reduction";
            // 
            // x3ToolStripMenuItem
            // 
            this.x3ToolStripMenuItem.Name = "x3ToolStripMenuItem";
            this.x3ToolStripMenuItem.Size = new System.Drawing.Size(91, 22);
            this.x3ToolStripMenuItem.Text = "3x3";
            this.x3ToolStripMenuItem.Click += new System.EventHandler(this.x3ToolStripMenuItem_Click);
            // 
            // nonUniformFiltersToolStripMenuItem
            // 
            this.nonUniformFiltersToolStripMenuItem.Name = "nonUniformFiltersToolStripMenuItem";
            this.nonUniformFiltersToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
            this.nonUniformFiltersToolStripMenuItem.Text = "Non Uniform";
            this.nonUniformFiltersToolStripMenuItem.Click += new System.EventHandler(this.nonUniformFiltersToolStripMenuItem_Click);
            // 
            // edgeDetectionFiltersToolStripMenuItem
            // 
            this.edgeDetectionFiltersToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sobelFilterToolStripMenuItem,
            this.differentialToolStripMenuItem,
            this.robertsToolStripMenuItem});
            this.edgeDetectionFiltersToolStripMenuItem.Name = "edgeDetectionFiltersToolStripMenuItem";
            this.edgeDetectionFiltersToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
            this.edgeDetectionFiltersToolStripMenuItem.Text = "Edge Detection";
            // 
            // sobelFilterToolStripMenuItem
            // 
            this.sobelFilterToolStripMenuItem.Name = "sobelFilterToolStripMenuItem";
            this.sobelFilterToolStripMenuItem.Size = new System.Drawing.Size(132, 22);
            this.sobelFilterToolStripMenuItem.Text = "Sobel";
            this.sobelFilterToolStripMenuItem.Click += new System.EventHandler(this.sobelFilterToolStripMenuItem_Click);
            // 
            // differentialToolStripMenuItem
            // 
            this.differentialToolStripMenuItem.Name = "differentialToolStripMenuItem";
            this.differentialToolStripMenuItem.Size = new System.Drawing.Size(132, 22);
            this.differentialToolStripMenuItem.Text = "Differential";
            this.differentialToolStripMenuItem.Click += new System.EventHandler(this.differentialToolStripMenuItem_Click);
            // 
            // robertsToolStripMenuItem
            // 
            this.robertsToolStripMenuItem.Name = "robertsToolStripMenuItem";
            this.robertsToolStripMenuItem.Size = new System.Drawing.Size(132, 22);
            this.robertsToolStripMenuItem.Text = "Roberts";
            this.robertsToolStripMenuItem.Click += new System.EventHandler(this.robertsToolStripMenuItem_Click);
            // 
            // medianFilterToolStripMenuItem
            // 
            this.medianFilterToolStripMenuItem.Name = "medianFilterToolStripMenuItem";
            this.medianFilterToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
            this.medianFilterToolStripMenuItem.Text = "Median";
            this.medianFilterToolStripMenuItem.Click += new System.EventHandler(this.medianFilterToolStripMenuItem_Click);
            // 
            // binarizationToolStripMenuItem
            // 
            this.binarizationToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.manualToolStripMenuItem,
            this.otsuToolStripMenuItem});
            this.binarizationToolStripMenuItem.Name = "binarizationToolStripMenuItem";
            this.binarizationToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
            this.binarizationToolStripMenuItem.Text = "Binarization";
            // 
            // manualToolStripMenuItem
            // 
            this.manualToolStripMenuItem.Name = "manualToolStripMenuItem";
            this.manualToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
            this.manualToolStripMenuItem.Text = "Manual";
            this.manualToolStripMenuItem.Click += new System.EventHandler(this.manualToolStripMenuItem_Click);
            // 
            // otsuToolStripMenuItem
            // 
            this.otsuToolStripMenuItem.Name = "otsuToolStripMenuItem";
            this.otsuToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
            this.otsuToolStripMenuItem.Text = "Otsu";
            this.otsuToolStripMenuItem.Click += new System.EventHandler(this.otsuToolStripMenuItem_Click);
            // 
            // showHistogramToolStripMenuItem
            // 
            this.showHistogramToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.intensityToolStripMenuItem,
            this.rGBToolStripMenuItem,
            this.allToolStripMenuItem});
            this.showHistogramToolStripMenuItem.Name = "showHistogramToolStripMenuItem";
            this.showHistogramToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.showHistogramToolStripMenuItem.Text = "Show Histogram";
            // 
            // intensityToolStripMenuItem
            // 
            this.intensityToolStripMenuItem.Name = "intensityToolStripMenuItem";
            this.intensityToolStripMenuItem.Size = new System.Drawing.Size(119, 22);
            this.intensityToolStripMenuItem.Text = "Intensity";
            this.intensityToolStripMenuItem.Click += new System.EventHandler(this.intensityToolStripMenuItem_Click);
            // 
            // rGBToolStripMenuItem
            // 
            this.rGBToolStripMenuItem.Name = "rGBToolStripMenuItem";
            this.rGBToolStripMenuItem.Size = new System.Drawing.Size(119, 22);
            this.rGBToolStripMenuItem.Text = "RGB";
            this.rGBToolStripMenuItem.Click += new System.EventHandler(this.rGBToolStripMenuItem_Click);
            // 
            // allToolStripMenuItem
            // 
            this.allToolStripMenuItem.Name = "allToolStripMenuItem";
            this.allToolStripMenuItem.Size = new System.Drawing.Size(119, 22);
            this.allToolStripMenuItem.Text = "All";
            this.allToolStripMenuItem.Click += new System.EventHandler(this.showHistogramToolStripMenuItem_Click);
            // 
            // autoZoomToolStripMenuItem
            // 
            this.autoZoomToolStripMenuItem.CheckOnClick = true;
            this.autoZoomToolStripMenuItem.Name = "autoZoomToolStripMenuItem";
            this.autoZoomToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
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
            // evalFormToolStripMenuItem
            // 
            this.evalFormToolStripMenuItem.Name = "evalFormToolStripMenuItem";
            this.evalFormToolStripMenuItem.Size = new System.Drawing.Size(71, 20);
            this.evalFormToolStripMenuItem.Text = "Eval Form";
            this.evalFormToolStripMenuItem.Click += new System.EventHandler(this.evalFormToolStripMenuItem_Click);
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
            this.ImageViewer.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ImageViewer_MouseClick);
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
        private System.Windows.Forms.ToolStripMenuItem meanNoiseReductionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem x3ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem nonUniformFiltersToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem edgeDetectionFiltersToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sobelFilterToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem differentialToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem medianFilterToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showHistogramToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem binarizationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem manualToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem otsuToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem evalFormToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem intensityToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem rGBToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem allToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem robertsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem rotationToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem bilinearToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem zoomToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem bilinearToolStripMenuItem1;
    }
}

