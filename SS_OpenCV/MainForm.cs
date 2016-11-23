using System;
using System.ComponentModel;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;

namespace SS_OpenCV
{
    public partial class MainForm : Form
    {
        Image<Bgr, Byte> img = null; // working image
        Image<Bgr, Byte> imgUndo = null; // undo backup image - UNDO
        string title_bak = "";

        public MainForm()
        {
            InitializeComponent();
            title_bak = Text;
        }

        /// <summary>
        /// Opens a new image
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                img = new Image<Bgr, byte>(openFileDialog1.FileName);
                Text = title_bak + " [" +
                        openFileDialog1.FileName.Substring(openFileDialog1.FileName.LastIndexOf("\\") + 1) +
                        "]";
                imgUndo = img.Copy();
                ImageViewer.Image = img.Bitmap;
                ImageViewer.Refresh();
            }
        }

        /// <summary>
        /// Saves an image with a new name
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                ImageViewer.Image.Save(saveFileDialog1.FileName);
            }
        }

        /// <summary>
        /// Closes the application
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// restore last undo copy of the working image
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (imgUndo == null) // verify if the image is already opened
                return;
            Cursor = Cursors.WaitCursor;
            img = imgUndo.Copy();

            ImageViewer.Image = img.Bitmap;
            ImageViewer.Refresh(); // refresh image on the screen

            Cursor = Cursors.Default; // normal cursor 
        }

        /// <summary>
        /// Chaneg visualization mode
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void autoZoomToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // zoom
            if (autoZoomToolStripMenuItem.Checked)
            {
                ImageViewer.SizeMode = PictureBoxSizeMode.Zoom;
                ImageViewer.Dock = DockStyle.Fill;
            }
            else // with scroll bars
            {
                ImageViewer.Dock = DockStyle.None;
                ImageViewer.SizeMode = PictureBoxSizeMode.AutoSize;
            }
        }

        /// <summary>
        /// Show authors form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void autoresToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AuthorsForm form = new AuthorsForm();
            form.ShowDialog();
        }


        /// <summary>
        /// Convert the working image to grayscale
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grayToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (img == null) // verify if the image is already opened
                return;
            Cursor = Cursors.WaitCursor; // clock cursor 

            //copy Undo Image
            imgUndo = img.Copy();

            ImageClass.ConvertToGray(img, 'M');

            ImageViewer.Image = img.Bitmap;
            ImageViewer.Refresh(); // refresh image on the screen

            Cursor = Cursors.Default; // normal cursor 
        }

        /// <summary>
        /// Calculate the image negative
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void negativeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (img == null) // verify if the image is already opened
                return;
            Cursor = Cursors.WaitCursor; // clock cursor 

            //copy Undo Image
            imgUndo = img.Copy();

            ImageClass.Negative(img);

            ImageViewer.Image = img.Bitmap;
            ImageViewer.Refresh(); // refresh image on the screen

            Cursor = Cursors.Default; // normal cursor 
        }

        private void grayRedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (img == null) // verify if the image is already opened
                return;
            Cursor = Cursors.WaitCursor; // clock cursor 

            //copy Undo Image
            imgUndo = img.Copy();

            ImageClass.ConvertToGray(img, 'R');

            ImageViewer.Image = img.Bitmap;
            ImageViewer.Refresh(); // refresh image on the screen

            Cursor = Cursors.Default; // normal cursor 

        }

        private void grayGreenToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (img == null) // verify if the image is already opened
                return;
            Cursor = Cursors.WaitCursor; // clock cursor 

            //copy Undo Image
            imgUndo = img.Copy();

            ImageClass.ConvertToGray(img, 'G');

            ImageViewer.Image = img.Bitmap;
            ImageViewer.Refresh(); // refresh image on the screen

            Cursor = Cursors.Default; // normal cursor 
        }

        private void grayBlueToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (img == null) // verify if the image is already opened
                return;
            Cursor = Cursors.WaitCursor; // clock cursor 

            //copy Undo Image
            imgUndo = img.Copy();

            ImageClass.ConvertToGray(img, 'B');

            ImageViewer.Image = img.Bitmap;
            ImageViewer.Refresh(); // refresh image on the screen

            Cursor = Cursors.Default; // normal cursor 
        }

        private void brightnessContrastToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (img == null) // verify if the image is already opened
                return;

            //copy Undo Image
            imgUndo = img.Copy();

            int brilho;
            float cont;
            DoubleInputBox dib = new DoubleInputBox("Brightness:", "Contrast:", "Brightness/Contrast", numberTextBox, decimalTextBox);
            dib.ShowDialog();

            do
            {
                if (dib.DialogResult == DialogResult.Cancel) return;
                brilho = Convert.ToInt32(dib.textBox1.Text);
                cont = Convert.ToSingle(dib.textBox2.Text);
            } while (Math.Abs(brilho) > 255 && (cont < 0 || cont > 3));

            Cursor = Cursors.WaitCursor; // clock cursor 

            if (dib.DialogResult == DialogResult.OK)
            {
                ImageClass.BrightCont(img, brilho, cont);

                ImageViewer.Image = img.Bitmap;
                ImageViewer.Refresh(); // refresh image on the screen
            }

            Cursor = Cursors.Default; // normal cursor 
        }

        private void translationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (img == null) // verify if the image is already opened
                return;
            Cursor = Cursors.WaitCursor; // clock cursor 

            //copy Undo Image
            imgUndo = img.Copy();

            DoubleInputBox dib = new DoubleInputBox("X Translation:", "Y Translation:", "Translation", numberTextBox, numberTextBox);
            dib.ShowDialog();

            int dx, dy;
            dx = Convert.ToInt32(dib.textBox1.Text);
            dy = Convert.ToInt32(dib.textBox2.Text);

            if (dib.DialogResult == DialogResult.OK)
            {
                ImageClass.Translate(img, dx, dy);

                ImageViewer.Image = img.Bitmap;
                ImageViewer.Refresh(); // refresh image on the screen
            }

            Cursor = Cursors.Default; // normal cursor 
        }

        private void rotationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (img == null) // verify if the image is already opened
                return;
            Cursor = Cursors.WaitCursor; // clock cursor 

            //copy Undo Image
            imgUndo = img.Copy();

            InputBox rot = new InputBox("Rotation Angle", numberTextBox);

            rot.ShowDialog();
            if (rot.DialogResult == DialogResult.OK)
            {

                int ang = Convert.ToInt32(rot.ValueTextBox.Text);

                ImageClass.Rotate(img, ang);

                ImageViewer.Image = img.Bitmap;
                ImageViewer.Refresh(); // refresh image on the screen
            }

            Cursor = Cursors.Default; // normal cursor 
        }

        int mouseX, mouseY;
        bool mouseFlag = false;

        private void ImageViewer_MouseClick(object sender, MouseEventArgs e)
        {
            if (mouseFlag)
            {
                mouseX = e.X;
                mouseY = e.Y;

                mouseFlag = false;
            }
        }

        private void x3ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (img == null) // verify if the image is already opened
                return;
            Cursor = Cursors.WaitCursor; // clock cursor 

            //copy Undo Image
            imgUndo = img.Copy();

            int[,] mat = { { 1, 1, 1 }, { 1, 1, 1 }, { 1, 1, 1 } };

            ImageClass.NonUniformFilter(img, mat, 9);

            ImageViewer.Image = img.Bitmap;
            ImageViewer.Refresh(); // refresh image on the screen

            Cursor = Cursors.Default; // normal cursor 
        }

        private void nonUniformFiltersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (img == null) // verify if the image is already opened
                return;

            //copy Undo Image
            imgUndo = img.Copy();

            WeightMatrix wm = new WeightMatrix();
            if (wm.ShowDialog() == DialogResult.OK)
            {
                int[,] mat = {{Convert.ToInt32(wm.topleft.Text),
                                Convert.ToInt32(wm.top.Text),
                                Convert.ToInt32(wm.topright.Text) },
                                { Convert.ToInt32(wm.left.Text),
                                Convert.ToInt32(wm.middle.Text),
                                Convert.ToInt32(wm.right.Text) },
                                    { Convert.ToInt32(wm.bttmleft.Text),
                                    Convert.ToInt32(wm.bottom.Text),
                                    Convert.ToInt32(wm.bttmright.Text) }
                };
                ImageClass.NonUniformFilter(img, mat, Convert.ToInt32(wm.weight.Text));
            }
            else
            {
                return;
            }
            ImageViewer.Image = img.Bitmap;
            ImageViewer.Refresh(); // refresh image on the screen


        }

        private void sobelFilterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (img == null) // verify if the image is already opened
                return;
            Cursor = Cursors.WaitCursor; // clock cursor 

            //copy Undo Image
            imgUndo = img.Copy();

            Image<Bgr, Byte> img2 = img.Copy();

            int[,] mat1 = { { 1, 0, -1 }, { 2, 0, -2 }, { 1, 0, -1 } };

            int[,] mat2 = { { -1, -2, -1 }, { 0, 0, 0 }, { 1, 2, 1 } };

            ImageClass.NonUniformFilter(img, mat1, 1);

            ImageClass.NonUniformFilter(img2, mat2, 1);

            ImageClass.SumPixels(img, img2);

            ImageViewer.Image = img.Bitmap;
            ImageViewer.Refresh(); // refresh image on the screen

            Cursor = Cursors.Default; // normal cursor 
        }

        private void differentialToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (img == null) // verify if the image is already opened
                return;
            Cursor = Cursors.WaitCursor; // clock cursor 

            //copy Undo Image
            imgUndo = img.Copy();

            ImageClass.DifferentialFilter(img);

            ImageViewer.Image = img.Bitmap;
            ImageViewer.Refresh(); // refresh image on the screen

            Cursor = Cursors.Default; // normal cursor 
        }

        private void medianFilterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (img == null) // verify if the image is already opened
                return;
            Cursor = Cursors.WaitCursor; // clock cursor 

            //copy Undo Image
            imgUndo = img.Copy();

            ImageClass.MedianFilter(img);

            ImageViewer.Image = img.Bitmap;
            ImageViewer.Refresh(); // refresh image on the screen

            Cursor = Cursors.Default; // normal cursor 
        }

        private void showHistogramToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (img == null) // verify if the image is already opened
                return;

            int[][] hists = ImageClass.CalculateHistogram(img);

            new HistogramForm(hists[0], hists[1], hists[2], hists[3]).ShowDialog();

        }

        private void manualToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (img == null) // verify if the image is already opened
                return;
            Cursor = Cursors.WaitCursor; // clock cursor 

            //copy Undo Image
            imgUndo = img.Copy();

            InputBox thres = new InputBox("Threshold", numberTextBox);

            thres.ShowDialog();

            int threshold = Convert.ToInt32(thres.ValueTextBox.Text);

            ImageClass.ManualBinarize(img, threshold);

            ImageViewer.Image = img.Bitmap;
            ImageViewer.Refresh(); // refresh image on the screen

            Cursor = Cursors.Default; // normal cursor 
        }

        private void otsuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (img == null) // verify if the image is already opened
                return;
            Cursor = Cursors.WaitCursor; // clock cursor 

            //copy Undo Image
            imgUndo = img.Copy();

            ImageClass.OtsuBinarize(img);

            ImageViewer.Image = img.Bitmap;
            ImageViewer.Refresh(); // refresh image on the screen

            Cursor = Cursors.Default; // normal cursor 
        }

        private void zoomToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (img == null) // verify if the image is already opened
                return;
            Cursor = Cursors.Cross; // cross cursor 

            //copy Undo Image
            imgUndo = img.Copy();

            InputBox zoom = new InputBox("Zoom Level", decimalTextBox);

            mouseFlag = true;
            while (mouseFlag) Application.DoEvents();

            zoom.ShowDialog();

            float factor = Convert.ToSingle(zoom.ValueTextBox.Text);

            Cursor = Cursors.WaitCursor; // clock cursor 

            ImageClass.Zoom(img, factor, mouseX, mouseY);

            ImageViewer.Image = img.Bitmap;
            ImageViewer.Refresh(); // refresh image on the screen

            Cursor = Cursors.Default; // normal cursor 
        }

        private void numberTextBox(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void decimalTextBox(object sender, KeyPressEventArgs e)
        {
            // allows 0-9, backspace, and decimal
            if (((e.KeyChar < 48 || e.KeyChar > 57) && e.KeyChar != 8 && e.KeyChar != 44))
            {
                e.Handled = true;
                return;
            }

            // checks to make sure only 1 decimal is allowed
            if (e.KeyChar == 44)
            {
                if ((sender as TextBox).Text.IndexOf(e.KeyChar) != -1)
                    e.Handled = true;
            }
        }
    }
}