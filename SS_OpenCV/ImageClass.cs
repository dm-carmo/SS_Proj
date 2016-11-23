using System;
using System.Linq;
using Emgu.CV.Structure;
using Emgu.CV;

namespace SS_OpenCV
{
    class ImageClass
    {

        /// <summary>
        /// Inverts an image's colours
        /// </summary>
        /// <param name="img">The mage</param>
        unsafe public static void Negative(Image<Bgr, byte> img)
        {
            // direct access to the image memory(sequencial)
            // direcion top left -> bottom right

            MIplImage m = img.MIplImage;
            byte* dataPtr = (byte*)m.imageData.ToPointer(); // Pointer to the image
            byte blue, green, red;

            int width = img.Width;
            int height = img.Height;
            int nChan = m.nChannels; // number of channels - 3
            int padding = m.widthStep - m.nChannels * m.width; // alignment bytes (padding)

            if (nChan == 3) // image in RGB
            {
                for (int y = 0; y < img.Height; y++)
                {
                    for (int x = 0; x < img.Width; x++)
                    {
                        //gets the 3 components
                        blue = dataPtr[0];
                        green = dataPtr[1];
                        red = dataPtr[2];

                        // stores the new values in the image
                        dataPtr[0] = (byte)(255 - blue);
                        dataPtr[1] = (byte)(255 - green);
                        dataPtr[2] = (byte)(255 - red);

                        // moves the pointer to the next pixel
                        dataPtr += nChan;
                    }

                    //at the end of the line moves the pointer by the alignment bytes (padding)
                    dataPtr += padding;
                }
            }
        }

        /// <summary>
        /// Changes the brightness and contrast of a picture
        /// </summary>
        /// <param name="img">The picture to modify</param>
        /// <param name="brilho">The amount of brightness to add/remove</param>
        /// <param name="cont">The contrast multiplier</param>
        unsafe public static void BrightCont(Image<Bgr, byte> img, int brilho, float cont)
        {

            // direct access to the image memory(sequencial)
            // direcion top left -> bottom right

            MIplImage m = img.MIplImage;
            byte* dataPtr = (byte*)m.imageData.ToPointer(); // Pointer to the image
            double blue, green, red;

            int width = img.Width;
            int height = img.Height;
            int nChan = m.nChannels; // number of channels - 3
            int padding = m.widthStep - m.nChannels * m.width; // alignment bytes (padding)

            if (nChan == 3) // image in RGB
            {
                for (int y = 0; y < img.Height; y++)
                {
                    for (int x = 0; x < img.Width; x++)
                    {
                        //gets the 3 components
                        blue = dataPtr[0] * cont + brilho;
                        green = dataPtr[1] * cont + brilho;
                        red = dataPtr[2] * cont + brilho;

                        if (blue > 255) blue = 255;
                        if (green > 255) green = 255;
                        if (red > 255) red = 255;

                        if (blue < 0) blue = 0;
                        if (red < 0) red = 0;
                        if (green < 0) green = 0;

                        // stores the new values in the image
                        dataPtr[0] = (byte)Math.Round(blue);
                        dataPtr[1] = (byte)Math.Round(green);
                        dataPtr[2] = (byte)Math.Round(red);

                        // moves the pointer to the next pixel
                        dataPtr += nChan;
                    }

                    //at the end of the line moves the pointer by the alignment bytes (padding)
                    dataPtr += padding;
                }
            }
        }

        /// <summary>
        /// Converts an image to gray
        /// Direct access to memory
        /// </summary>
        /// <param name="img">The image</param>
        /// <param name="mode">Conversion mode</param>
        unsafe public static void ConvertToGray(Image<Bgr, byte> img, char mode)
        {
            // direct access to the image memory(sequencial)
            // direcion top left -> bottom right

            MIplImage m = img.MIplImage;
            byte* dataPtr = (byte*)m.imageData.ToPointer(); // Pointer to the image
            byte blue, green, red, gray;

            int width = img.Width;
            int height = img.Height;
            int nChan = m.nChannels; // number of channels - 3
            int padding = m.widthStep - m.nChannels * m.width; // alignment bytes (padding)

            if (nChan == 3) // image in RGB
            {
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        //gets the 3 components
                        blue = dataPtr[0];
                        green = dataPtr[1];
                        red = dataPtr[2];

                        // convert to gray
                        switch (mode)
                        {
                            case 'M': gray = (byte)Math.Round(((int)blue + green + red) / 3.0); break; //uses the average of the 3 components
                            case 'R': gray = red; break; //uses the value of the red component
                            case 'G': gray = green; break; //uses the value of the green component
                            case 'B': gray = blue; break; //uses the value of the blue component
                            default: gray = 0; break;
                        }

                        // stores the new values in the image
                        dataPtr[0] = dataPtr[1] = dataPtr[2] = gray;

                        // moves the pointer to the next pixel
                        dataPtr += nChan;
                    }

                    //at the end of the line moves the pointer by the alignment bytes (padding)
                    dataPtr += padding;
                }
            }
        }

        /// <summary>
        /// Moves an image
        /// </summary>
        /// <param name="img">The image to move</param>
        /// <param name="dx">Amount of pixels to move in the X axis</param>
        /// <param name="dy">Amount of pixels to move in the Y axis</param>
        unsafe public static void Translate(Image<Bgr, byte> img, int dx, int dy)
        {
            MIplImage copy = img.Copy().MIplImage;
            MIplImage m = img.MIplImage;
            byte* dataPtr = (byte*)m.imageData.ToPointer(); // Pointer to the image
            byte* dataPtrCopy = (byte*)copy.imageData.ToPointer(); // Pointer to the image copy

            int widthstep = m.widthStep;
            int nC = m.nChannels;
            int width = img.Width;
            int height = img.Height;


            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    //Gets the original position of the pixel
                    byte* orig = dataPtrCopy + (y - dy) * widthstep + (x - dx) * nC;
                    byte* dest = dataPtr + y * widthstep + x * nC;

                    if (y - dy >= 0 && y - dy < height && x - dx >= 0 && x - dx < width)
                    {
                        dest[0] = orig[0];
                        dest[1] = orig[1];
                        dest[2] = orig[2];
                    }

                    else //if we're trying to get pixels from outside the original image
                    {
                        dest[0] = 0;
                        dest[1] = 0;
                        dest[2] = 0;
                    }

                }
            }
        }

        /// <summary>
        /// Rotates an image
        /// </summary>
        /// <param name="img">The image to rotate</param>
        /// <param name="ang">The angle of rotation (in degrees)</param>
        unsafe public static void Rotate(Image<Bgr, byte> img, int ang)
        {
            MIplImage m = img.MIplImage;
            MIplImage copy = img.Copy().MIplImage;
            byte* dataPtr = (byte*)m.imageData.ToPointer(); // Pointer to the image
            byte* dataPtrCopy = (byte*)copy.imageData.ToPointer(); // Pointer to the image copy

            int widthstep = m.widthStep;
            int nC = m.nChannels;
            int width = img.Width;
            int height = img.Height;
            double rad = ang * Math.PI / 180;
            double sine = Math.Sin(rad);
            double cose = Math.Cos(rad);

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    //inverse rotation: gets the original position of the pixel from the new pixel's coordinates
                    int nx = (int)((x - width / 2) * cose - (height / 2 - y) * sine + width / 2);
                    int ny = (int)(height / 2 - (x - width / 2) * sine - (height / 2 - y) * cose);
                    byte* orig = dataPtrCopy + ny * widthstep + nx * nC;
                    byte* dest = dataPtr + y * widthstep + x * nC;

                    if (ny >= 0 && ny < height && nx >= 0 && nx < width)
                    {
                        dest[0] = orig[0];
                        dest[1] = orig[1];
                        dest[2] = orig[2];
                    }

                    else //if we're trying to get pixels from outside the original image
                    {
                        dest[0] = 0;
                        dest[1] = 0;
                        dest[2] = 0;
                    }

                }
            }
        }

        /// <summary>
        /// Zooms in/out on an image.
        /// mouseX and mouseY determine the new top-left corner
        /// </summary>
        /// <param name="img">The image</param>
        /// <param name="factor">Zoom factor</param>
        /// <param name="mouseX">Current X position of the mouse</param>
        /// <param name="mouseY">Current Y position of the mouse</param>
        unsafe public static void Zoom(Image<Bgr, byte> img, float factor, int mouseX, int mouseY)
        {
            if (factor == 1 || factor < 0) return;
            MIplImage copy = img.Copy().MIplImage;
            MIplImage m = img.MIplImage;
            byte* dataPtr = (byte*)m.imageData.ToPointer(); // Pointer to the image
            byte* dataPtrCopy = (byte*)copy.imageData.ToPointer(); // Pointer to the image copy

            int widthstep = m.widthStep;
            int nC = m.nChannels;
            int width = img.Width;
            int height = img.Height;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    //Gets the original position of the pixel
                    int ny = (int)(y / factor + mouseY);
                    int nx = (int)(x / factor + mouseX);
                    byte* orig = dataPtrCopy + ny * widthstep + nx * nC;
                    byte* dest = dataPtr + y * widthstep + x * nC;

                    if (ny >= 0 && ny < height && nx >= 0 && nx < width)
                    {
                        dest[0] = orig[0];
                        dest[1] = orig[1];
                        dest[2] = orig[2];
                    }

                    else //if we're trying to get pixels from outside the original image
                    {
                        dest[0] = 0;
                        dest[1] = 0;
                        dest[2] = 0;
                    }
                }
            }
        }

        /// <summary>
        /// Performs the binarization of an image, based on the Otsu method
        /// </summary>
        /// <param name="img">The image to binarize</param>
        public static void OtsuBinarize(Image<Bgr, byte> img)
        {
            //covariance = q1*q2*(u1-u2)^2
            //q1 = sum(i = 0, t, P(i)) -> sum of the probabilities of a pixel being below the chosen threshold
            //q2 = sum(i = t + 1, 255, P(i)) -> sum of the probabilities of a pixel being above the chosen threshold
            //u1 = sum(i = 0, t, i * P(i))/q1 -> weighted mean (below threshold)
            //u2 = sum(i = t + 1, 255, i * P(i))/q2 -> weighted mean (above threshold)
            int[] intensity = CalculateHistogram(img)[0];
            double[] probs = new double[256]; //will save the various probabilities
            double[] vars = new double[254]; //will save all covariance values for each threshold (1-254 only)
            MIplImage m = img.MIplImage;
            int npixels = m.width * m.height;
            int i;
            for (i = 0; i < 256; i++) //calculates probabilities for each intensity
            {
                probs[i] = (double)intensity[i] / npixels;
            }
            for (int t = 1; t < 255; t++) //calculates covariance for each threshold
            {
                double q1 = 0.0, q2 = 0.0, u1 = 0.0, u2 = 0.0;
                for (i = 0; i < t; i++)
                {
                    q1 += probs[i];
                    u1 += probs[i] * i;
                }
                for (; i < 256; i++)
                {
                    q2 += probs[i];
                    u2 += probs[i] * i;
                }
                u1 /= q1;
                u2 /= q2;
                vars[t - 1] = q1 * q2 * (u1 - u2) * (u1 - u2);
            }
            int threshold = Array.IndexOf(vars, vars.Max()) + 1; //chooses the maximum covariance and respective threshold
            ManualBinarize(img, threshold); //binarizes the image based on the chosen threshold
        }

        /// <summary>
        /// Performs the binarization of an image, based on a threshold
        /// </summary>
        /// <param name="img">The image to binarize</param>
        /// <param name="threshold">The binarization threshold</param>
        unsafe public static void ManualBinarize(Image<Bgr, byte> img, int threshold)
        {
            ConvertToGray(img, 'M'); //for best results we should convert the image to grayscale first
            MIplImage m = img.MIplImage;
            byte* dataPtr = (byte*)m.imageData.ToPointer(); // Pointer to the image

            int widthstep = m.widthStep;
            int nC = m.nChannels;
            int width = img.Width;
            int height = img.Height;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    byte* dest = dataPtr + y * widthstep + x * nC;
                    int[] bgr = new int[] { dest[0], dest[1], dest[2] };

                    if (bgr.Max() >= threshold)
                    {
                        dest[0] = dest[1] = dest[2] = 255;
                    }

                    else
                    {
                        dest[0] = dest[1] = dest[2] = 0;
                    }
                }
            }
        }

        /// <summary>
        /// Calculates the RGB and intensity histograms of an image
        /// </summary>
        /// <param name="img">The image</param>
        /// <returns>An array with the histogram values (intensitr, blue, red, green)</returns>
        unsafe public static int[][] CalculateHistogram(Image<Bgr, byte> img)
        {
            int[] intensity = new int[256];
            int[] blue = new int[256];
            int[] green = new int[256];
            int[] red = new int[256];
            MIplImage m = img.MIplImage;
            Image<Bgr, byte> grayscale = img.Copy();
            ConvertToGray(grayscale, 'M'); //to calculate intensity histogram we should convert the image to grayscale
            byte* dataPtr = (byte*)m.imageData.ToPointer(); // Pointer to the image
            byte* grayscalePtr = (byte*)grayscale.MIplImage.imageData.ToPointer();

            int widthstep = m.widthStep;
            int nC = m.nChannels;
            int width = img.Width;
            int height = img.Height;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    byte* pixelPtr = dataPtr + y * widthstep + x * nC;
                    byte* grayPixelPtr = grayscalePtr + y * widthstep + x * nC;
                    int[] bgr = new int[] { pixelPtr[0], pixelPtr[1], pixelPtr[2] };
                    blue[bgr[0]]++;
                    green[bgr[1]]++;
                    red[bgr[2]]++;
                    intensity[grayPixelPtr[0]]++;
                }
            }

            return new int[][] { intensity, blue, green, red };
        }

        /// <summary>
        /// Performs a median filter on an image
        /// </summary>
        /// <param name="img">The image</param>
        unsafe public static void MedianFilter(Image<Bgr, byte> img)
        {
            MIplImage m = img.MIplImage;
            MIplImage copy = img.Copy().MIplImage;
            byte* dataPtr = (byte*)m.imageData.ToPointer(); // Pointer to the image
            byte* dataPtrCopy = (byte*)copy.imageData.ToPointer(); // Pointer to the image copy

            int widthstep = m.widthStep;
            int nC = m.nChannels;
            int width = img.Width;
            int height = img.Height;

            int x, y;

            for (y = 1; y < height - 1; y++)
            {
                for (x = 1; x < width - 1; x++)
                { //calculates the median for all central pixels
                    byte* pixelPtr = dataPtr + y * widthstep + x * nC;
                    byte* pixelPtrCopy = dataPtrCopy + (y - 1) * widthstep + (x - 1) * nC;
                    CalculateMedian(m, pixelPtrCopy, pixelPtr);
                }
            }
            //calculates the median for each corner
            CalculateMedianCorners(m, dataPtr, dataPtrCopy, 0, 0); //top-left
            CalculateMedianCorners(m, dataPtr, dataPtrCopy, 0, height - 1); //bottom-left
            CalculateMedianCorners(m, dataPtr, dataPtrCopy, width - 1, 0); //top-right
            CalculateMedianCorners(m, dataPtr, dataPtrCopy, width - 1, height - 1); //bottom-right

            for (x = 1; x < width - 1; x++)
            { //calculates the median for top and bottom margin pixels
                byte* pixelPtr = dataPtr + x * nC;
                byte* pixelPtrCopy = dataPtrCopy + (x - 1) * nC;
                CalculateMedianMargins(m, pixelPtrCopy, pixelPtr, 1, 0); //top
                CalculateMedianMargins(m, pixelPtrCopy, pixelPtr, 1, height - 1); //bottom
            }

            for (y = 1; y < height - 1; x++)
            { //calculates the median for left and right margin pixels
                byte* pixelPtr = dataPtr + y * widthstep;
                byte* pixelPtrCopy = dataPtrCopy + (y - 1) * widthstep;
                CalculateMedianMargins(m, pixelPtrCopy, pixelPtr, 0, 1); //left
                CalculateMedianMargins(m, pixelPtrCopy, pixelPtr, width - 1, 1); //right
            }
        }

        /// <summary>
        /// Calculates a pixel's median
        /// </summary>
        /// <param name="m">Information about an image</param>
        /// <param name="origPtr">Pointer to the top-left pixel in the respective neighbourhood</param>
        /// <param name="copyPtr">Pointer to the pixel where we are calculating the median (we will modify this)</param>
        unsafe public static void CalculateMedian(MIplImage m, byte* origPtr, byte* copyPtr)
        {
            int[,] distMat = new int[9, 9]; //a matrix used to store the distance between any 2 pixels

            for (int y = 0; y < 3; y++)
            {
                for (int x = 0; x < 3; x++)
                {
                    byte* startPtr = origPtr + y * m.widthStep + x * m.nChannels; //we are calculating distances between this pixel and all others
                    for (int i = 0; i < 9; i++)
                    {
                        int idx = y * 3 + x;
                        if (distMat[idx, i] != 0) continue; //we have aleady done this
                        if (idx == i)
                        { //distance between a pixel and itself is zero
                            distMat[idx, i] = 0;
                            continue;
                        }
                        byte* distPtr = origPtr + (i / 3) * m.widthStep + (i % 3) * m.nChannels;
                        int dist = Math.Abs(startPtr[0] - distPtr[0]) + Math.Abs(startPtr[1] - distPtr[1]) + Math.Abs(startPtr[2] - distPtr[2]);
                        distMat[idx, i] = distMat[i, idx] = dist; //distance between A-B is the same as B-A
                    }
                }
            }
            int[] distSum = new int[9];
            //calculates the sum of distances for every pixel
            for (int i = 0; i < 9; i++) for (int j = 0; j < 9; j++) distSum[i] += distMat[i, j];
            int newIdx = Array.IndexOf(distSum, distSum.Min()); //chooses the pixel with the smallest distance sum
            byte* newPtr = origPtr + (newIdx / 3) * m.widthStep + (newIdx % 3) * m.nChannels;
            copyPtr[0] = newPtr[0];
            copyPtr[1] = newPtr[1];
            copyPtr[2] = newPtr[2];
        }

        /// <summary>
        /// Calculates a corner pixel's median
        /// </summary>
        /// <param name="m">Information about an image</param>
        /// <param name="origPtr">Pointer to the top-left corner of the image</param>
        /// <param name="copyPtr">Pointer to the top-left corner of the image (we will modify this)</param>
        /// <param name="xpos">X position of the corner we are calculating</param>
        /// <param name="ypos">Y position of the corner we are calculating</param>
        unsafe private static void CalculateMedianCorners(MIplImage m, byte* origPtr, byte* copyPtr, int xpos, int ypos)
        {
            int[,] distMat = new int[4, 4]; //a matrix used to store the distance between any 2 pixels
            //we always start from the top left corner of the neighbourhood so we must check where is that pixel
            int xsign = (xpos == 0) ? 0 : -1; //top-right and bottom-right, we need to move 1 pixel to the left
            int ysign = (ypos == 0) ? 0 : -1; //bottom-left and bottom-right, we need to move 1 pixel up
            byte* cornerPtr = origPtr + (ysign + ypos) * m.widthStep + (xsign + xpos) * m.nChannels; //top-left corner of the neighbourhood

            for (int y = 0; y < 2; y++)
            {
                for (int x = 0; x < 2; x++)
                {
                    byte* startPtr = cornerPtr + y * m.widthStep + x * m.nChannels; //we are calculating distances between this pixel and all others
                    for (int i = 0; i < 4; i++)
                    {
                        int idx = y * 2 + x;
                        if (distMat[idx, i] != 0) continue; //we have already done this
                        if (idx == i)
                        { //distance between a pixel and itself is zero
                            distMat[idx, i] = 0;
                            continue;
                        }
                        byte* distPtr = cornerPtr + (i / 2) * m.widthStep + (i % 2) * m.nChannels;
                        int dist = Math.Abs(startPtr[0] - distPtr[0]) + Math.Abs(startPtr[1] - distPtr[1]) + Math.Abs(startPtr[2] - distPtr[2]);
                        distMat[idx, i] = distMat[i, idx] = dist; //distance between A-B is the same as B-A
                    }
                }
            }
            int[] distSum = new int[4];
            //calculates the sum of distances for every pixel
            for (int i = 0; i < 4; i++)
            { //margin duplication, some pixels are repeated therefore we would have duplicated distances
                //that is taken care of here
                int c1 = (ypos == 0 | xpos == 0 ? (xpos == ypos ? 4 : 2) : 1);
                int c2 = (ypos == 0 & xpos != 0 ? 4 : (ypos != 0 ? 1 : 2));
                int c3 = (xpos == 0 & ypos != 0 ? 4 : (xpos != 0 ? 1 : 2));
                int c4 = (ypos != 0 | xpos != 0 ? 4 : (xpos == ypos & xpos == 0 ? 1 : 2));
                distSum[i] = c1 * distMat[i, 0] + c2 * distMat[i, 1] + c3 * distMat[i, 2] + c4 * distMat[i, 3];
            }
            int newIdx = Array.IndexOf(distSum, distSum.Min()); //chooses the pixel with the smallest distance sum
            byte* newPtr = cornerPtr + (newIdx / 2) * m.widthStep + (newIdx % 2) * m.nChannels;
            copyPtr[0] = newPtr[0];
            copyPtr[1] = newPtr[1];
            copyPtr[2] = newPtr[2];
        }

        /// <summary>
        /// Calculates a pixel's median, if it is part of the image margin
        /// </summary>
        /// <param name="m">Information about an image</param>
        /// <param name="origPtr">Pointer to the top-left pixel in the neighbourhood</param>
        /// <param name="copyPtr">Pointer to the pixel where we are calculating the median (we will modify this)</param>
        /// <param name="xpos">X position of the first pixel in the margin</param>
        /// <param name="ypos">Y position of the first pixel in the margin</param>
        unsafe private static void CalculateMedianMargins(MIplImage m, byte* origPtr, byte* copyPtr, int xpos, int ypos)
        {
            int[,] distMat = new int[6, 6]; //a matrix used to store the distance between any 2 pixels
            //to check whether we are in the X axis or the Y axis
            int xmax = (ypos == 1) ? 2 : 3;
            int ymax = (xpos == 1) ? 2 : 3;

            for (int y = 0; y < ymax; y++)
            {
                for (int x = 0; x < xmax; x++)
                {
                    byte* startPtr = origPtr + y * m.widthStep + x * m.nChannels; //we are calculating distances between this pixel and all others
                    for (int i = 0; i < 6; i++)
                    {
                        int idx = y * xmax + x;
                        if (distMat[idx, i] != 0) continue; //we have already done this
                        if (idx == i)
                        { //distance between a pixel and itself is zero
                            distMat[idx, i] = 0;
                            continue;
                        }
                        byte* distPtr = startPtr + (i / xmax) * m.widthStep + (i % xmax) * m.nChannels;
                        int dist = Math.Abs(startPtr[0] - distPtr[0]) + Math.Abs(startPtr[1] - distPtr[1]) + Math.Abs(startPtr[2] - distPtr[2]);
                        distMat[idx, i] = distMat[i, idx] = dist; //distance between A-B is the same as B-A
                    }
                }
            }
            int[] distSum = new int[6];
            //calculates the sum of distances for every pixel
            for (int i = 0; i < 6; i++)
            { //margin duplication, some pixels are repeated therefore we would have duplicated distances
                //that is taken care of here
                int c1 = (ypos == 0 | xpos == 0 ? 2 : 1);
                int c2 = (ypos == 0 | xpos == 0 ? 1 : 2);
                int j;
                //X axis margins duplicate top/bottom row of pixels (0, 1, 2 / 3, 4, 5 in our matrix)
                //Y axis margins duplicate left/right row of pixels (0, 2, 4 / 1, 3, 5 in our matrix)
                //all of this depends on how many pixels belong to the original image in the X axis
                int inc = (xmax == 2) ? 2 : 1;
                int cond = (xmax == 2) ? 3 : 6;
                for (j = 0; j < cond; j += inc) distSum[i] += c1 * distMat[i, j];
                for (j = (xmax == 2) ? 1 : cond; j < cond; j += inc) distSum[i] += c2 * distMat[i, j];
            }
            int newIdx = Array.IndexOf(distSum, distSum.Min()); //chooses the pixel with the smallest distance sum
            byte* newPtr = origPtr + (newIdx / xmax) * m.widthStep + (newIdx % xmax) * m.nChannels;
            copyPtr[0] = newPtr[0];
            copyPtr[1] = newPtr[1];
            copyPtr[2] = newPtr[2];
        }

        /// <summary>
        /// Performs a differential edge detection filter on an image
        /// </summary>
        /// <param name="img">The image</param>
        unsafe public static void DifferentialFilter(Image<Bgr, byte> img)
        {
            MIplImage m = img.MIplImage;
            MIplImage copy = img.Copy().MIplImage;
            byte* dataPtr = (byte*)m.imageData.ToPointer(); // Pointer to the image
            byte* dataPtrCopy = (byte*)copy.imageData.ToPointer(); // Pointer to the image copy

            int widthstep = m.widthStep;
            int nC = m.nChannels;
            int width = img.Width;
            int height = img.Height;

            //Calculates the filter's value for all pixels except bottom and right margins
            for (int y = 0; y < height - 1; y++)
            {
                for (int x = 0; x < width - 1; x++)
                {
                    byte* dest = dataPtr + y * widthstep + x * nC;
                    byte* currPt = dataPtrCopy + y * widthstep + x * nC;
                    int[] sum = { 0, 0, 0 };
                    byte* rightPt = dataPtrCopy + y * widthstep + (x + 1) * nC; //the pixel to the right
                    byte* downPt = dataPtrCopy + (y + 1) * widthstep + x * nC; //the pixel below
                    sum[0] = Math.Abs(currPt[0] - rightPt[0]) + Math.Abs(currPt[0] - downPt[0]);
                    sum[1] = Math.Abs(currPt[1] - rightPt[1]) + Math.Abs(currPt[1] - downPt[1]);
                    sum[2] = Math.Abs(currPt[2] - rightPt[2]) + Math.Abs(currPt[2] - downPt[2]);
                    dest[0] = (byte)(sum[0] > 255 ? 255 : sum[0]);
                    dest[1] = (byte)(sum[1] > 255 ? 255 : sum[1]);
                    dest[2] = (byte)(sum[2] > 255 ? 255 : sum[2]);
                }
            }

            //calculates the filter's value for the right margin
            for (int y = 0; y < height - 1; y++)
            {
                byte* dest = dataPtr + y * widthstep + (width - 1) * nC;
                byte* currPt = dataPtrCopy + y * widthstep + (width - 1) * nC;
                int[] sum = { 0, 0, 0 };
                //no pixel to the right (margin duplication, difference would be zero)
                byte* downPt = dataPtrCopy + (y + 1) * widthstep + (width - 1) * nC;
                sum[0] = Math.Abs(currPt[0] - downPt[0]);
                sum[1] = Math.Abs(currPt[1] - downPt[1]);
                sum[2] = Math.Abs(currPt[2] - downPt[2]);
                dest[0] = (byte)(sum[0] > 255 ? 255 : sum[0]);
                dest[1] = (byte)(sum[1] > 255 ? 255 : sum[1]);
                dest[2] = (byte)(sum[2] > 255 ? 255 : sum[2]);
            }

            //calculates the filter's value for the bottom margin
            for (int x = 0; x < width - 1; x++)
            {
                byte* dest = dataPtr + (height - 1) * widthstep + x * nC;
                byte* currPt = dataPtrCopy + (height - 1) * widthstep + x * nC;
                int[] sum = { 0, 0, 0 };
                //no pixel below (margin duplication, difference would be zero)
                byte* rightPt = dataPtrCopy + (height - 1) * widthstep + (x + 1) * nC;
                sum[0] = Math.Abs(currPt[0] - rightPt[0]);
                sum[1] = Math.Abs(currPt[1] - rightPt[1]);
                sum[2] = Math.Abs(currPt[2] - rightPt[2]);
                dest[0] = (byte)(sum[0] > 255 ? 255 : sum[0]);
                dest[1] = (byte)(sum[1] > 255 ? 255 : sum[1]);
                dest[2] = (byte)(sum[2] > 255 ? 255 : sum[2]);
            }

            //bottom-right corner has no one to its right or below, so it becomes black (margin duplication, differences would be zero)
            dataPtr = dataPtr + (height - 1) * widthstep + (width - 1) * nC;
            dataPtr[0] = 0;
            dataPtr[1] = 0;
            dataPtr[2] = 0;
        }

        /// <summary>
        /// Adds the RGB values of pixels between two images.
        /// Assumes they have the same width and height
        /// </summary>
        /// <param name="img1">The first image (we will modify this one)</param>
        /// <param name="img2">The second image</param>
        unsafe public static void SumPixels(Image<Bgr, byte> img1, Image<Bgr, byte> img2)
        {
            MIplImage m1 = img1.MIplImage;
            MIplImage m2 = img2.MIplImage;
            byte* dataPtr1 = (byte*)m1.imageData.ToPointer();
            byte* dataPtr2 = (byte*)m2.imageData.ToPointer();

            int widthstep = m1.widthStep;
            int nC = m1.nChannels;
            int width = img1.Width;
            int height = img1.Height;

            for (int y = 0; y < img1.Height; y++)
            {
                for (int x = 0; x < img1.Width; x++)
                {
                    byte* orig = dataPtr1 + y * widthstep + x * nC;
                    byte* sum = dataPtr2 + y * widthstep + x * nC;

                    //gets the sum of each component
                    int blue = (int)(orig[0] + sum[0]);
                    int green = (int)(orig[1] + sum[1]);
                    int red = (int)(orig[2] + sum[2]);

                    if (blue > 255) blue = 255;
                    if (green > 255) green = 255;
                    if (red > 255) red = 255;

                    // stores the new values in the image
                    dataPtr1[0] = (byte)(blue);
                    dataPtr1[1] = (byte)(green);
                    dataPtr1[2] = (byte)(red);
                }
            }
        }

        /// <summary>
        /// Performs a non-uniform filter on an image
        /// </summary>
        /// <param name="img">The image</param>
        /// <param name="mat">The filter's matrix (multipliers)</param>
        /// <param name="weight">The weight of the final sum</param>
        unsafe public static void NonUniformFilter(Image<Bgr, byte> img, int[,] mat, int weight)
        {
            MIplImage m = img.MIplImage;
            MIplImage copy = img.Copy().MIplImage;
            byte* dataPtr = (byte*)m.imageData.ToPointer(); // Pointer to the image
            byte* dataPtrCopy = (byte*)copy.imageData.ToPointer(); // Pointer to the image copy

            int widthstep = m.widthStep;
            int nC = m.nChannels;
            int width = img.Width;
            int height = img.Height;

            //calculates the central pixels' values
            for (int y = 1; y < height - 1; y++)
            {
                for (int x = 1; x < width - 1; x++)
                {
                    byte* dest = dataPtr + y * widthstep + x * nC;
                    //sum of each pixel's component, after multiplying them by the respective factor
                    int[] sum = { 0, 0, 0 };
                    for (int j = -1; j <= 1; j++)
                    {
                        for (int i = -1; i <= 1; i++)
                        {
                            byte* orig = dataPtrCopy + (y + j) * widthstep + (x + i) * nC;
                            //gets multiplier from the matrix and applies iut to each component
                            //adding the value to the sum array
                            sum[0] += mat[i + 1, j + 1] * orig[0];
                            sum[1] += mat[i + 1, j + 1] * orig[1];
                            sum[2] += mat[i + 1, j + 1] * orig[2];
                        }
                    }
                    for (int i = 0; i < sum.Length; i++)
                    {
                        //applies the weight to the sum array
                        sum[i] /= weight;
                        if (sum[i] > 255) sum[i] = 255;
                        else if (sum[i] < 0) sum[i] = Math.Abs(sum[i]);
                    }

                    dest[0] = (byte)(sum[0]);
                    dest[1] = (byte)(sum[1]);
                    dest[2] = (byte)(sum[2]);
                }
            }

            //processing corners and margins
            //top-left corner
            int d0 = (((mat[0, 0] + mat[0, 1] + mat[1, 0] + mat[1, 1]) * dataPtrCopy[0] + (mat[2, 1] + mat[2, 0]) * (dataPtrCopy + widthstep)[0] + (mat[1, 2] + mat[0, 2]) * (dataPtrCopy + nC)[0] + mat[2, 2] * (dataPtrCopy + widthstep + nC)[0]) / weight);
            int d1 = (((mat[0, 0] + mat[0, 1] + mat[1, 0] + mat[1, 1]) * dataPtrCopy[1] + (mat[2, 1] + mat[2, 0]) * (dataPtrCopy + widthstep)[1] + (mat[1, 2] + mat[0, 2]) * (dataPtrCopy + nC)[1] + mat[2, 2] * (dataPtrCopy + widthstep + nC)[1]) / weight);
            int d2 = (((mat[0, 0] + mat[0, 1] + mat[1, 0] + mat[1, 1]) * dataPtrCopy[2] + (mat[2, 1] + mat[2, 0]) * (dataPtrCopy + widthstep)[2] + (mat[1, 2] + mat[0, 2]) * (dataPtrCopy + nC)[2] + mat[2, 2] * (dataPtrCopy + widthstep + nC)[2]) / weight);
            dataPtr[0] = (byte)(d0 > 255 ? 255 : (d0 < 0 ? Math.Abs(d0) : d0));
            dataPtr[1] = (byte)(d1 > 255 ? 255 : (d1 < 0 ? Math.Abs(d1) : d1));
            dataPtr[2] = (byte)(d2 > 255 ? 255 : (d2 < 0 ? Math.Abs(d2) : d2));

            //top-right corner
            byte* corner = dataPtr + (width - 1) * nC;
            byte* cornerCopy = dataPtrCopy + (width - 1) * nC;

            d0 = (((mat[0, 2] + mat[1, 2] + mat[0, 1] + mat[1, 1]) * cornerCopy[0] + (mat[2, 1] + mat[2, 2]) * (cornerCopy + widthstep)[0] + (mat[1, 0] + mat[0, 0]) * (cornerCopy - nC)[0] + mat[2, 0] * (cornerCopy + widthstep - nC)[0]) / weight);
            d1 = (((mat[0, 2] + mat[1, 2] + mat[0, 1] + mat[1, 1]) * cornerCopy[1] + (mat[2, 1] + mat[2, 2]) * (cornerCopy + widthstep)[1] + (mat[1, 0] + mat[0, 0]) * (cornerCopy - nC)[1] + mat[2, 0] * (cornerCopy + widthstep - nC)[1]) / weight);
            d2 = (((mat[0, 2] + mat[1, 2] + mat[0, 1] + mat[1, 1]) * cornerCopy[2] + (mat[2, 1] + mat[2, 2]) * (cornerCopy + widthstep)[2] + (mat[1, 0] + mat[0, 0]) * (cornerCopy - nC)[2] + mat[2, 0] * (cornerCopy + widthstep - nC)[2]) / weight);

            corner[0] = (byte)(d0 > 255 ? 255 : (d0 < 0 ? Math.Abs(d0) : d0));
            corner[1] = (byte)(d1 > 255 ? 255 : (d1 < 0 ? Math.Abs(d1) : d1));
            corner[2] = (byte)(d2 > 255 ? 255 : (d2 < 0 ? Math.Abs(d2) : d2));

            //bottom-left corner
            corner = dataPtr + (height - 1) * widthstep;
            cornerCopy = dataPtrCopy + (height - 1) * widthstep;

            d0 = (((mat[1, 1] + mat[1, 0] + mat[2, 1] + mat[2, 0]) * cornerCopy[0] + (mat[0, 1] + mat[0, 0]) * (cornerCopy - widthstep)[0] + (mat[1, 2] + mat[2, 2]) * (cornerCopy + nC)[0] + mat[0, 2] * (cornerCopy - widthstep + nC)[0]) / weight);
            d1 = (((mat[1, 1] + mat[1, 0] + mat[2, 1] + mat[2, 0]) * cornerCopy[1] + (mat[0, 1] + mat[0, 0]) * (cornerCopy - widthstep)[1] + (mat[1, 2] + mat[2, 2]) * (cornerCopy + nC)[1] + mat[0, 2] * (cornerCopy - widthstep + nC)[1]) / weight);
            d2 = (((mat[1, 1] + mat[1, 0] + mat[2, 1] + mat[2, 0]) * cornerCopy[2] + (mat[0, 1] + mat[0, 0]) * (cornerCopy - widthstep)[2] + (mat[1, 2] + mat[2, 2]) * (cornerCopy + nC)[2] + mat[0, 2] * (cornerCopy - widthstep + nC)[2]) / weight);

            corner[0] = (byte)(d0 > 255 ? 255 : (d0 < 0 ? Math.Abs(d0) : d0));
            corner[1] = (byte)(d1 > 255 ? 255 : (d1 < 0 ? Math.Abs(d1) : d1));
            corner[2] = (byte)(d2 > 255 ? 255 : (d2 < 0 ? Math.Abs(d2) : d2));

            //bottom-right corner
            corner = dataPtr + (height - 1) * widthstep + (width - 1) * nC;
            cornerCopy = dataPtrCopy + (height - 1) * widthstep + (width - 1) * nC;

            d0 = (((mat[1, 1] + mat[1, 2] + mat[2, 1] + mat[2, 2]) * cornerCopy[0] + (mat[0, 1] + mat[0, 2]) * (cornerCopy - widthstep)[0] + (mat[1, 0] + mat[2, 0]) * (cornerCopy - nC)[0] + mat[0, 0] * (cornerCopy - widthstep - nC)[0]) / weight);
            d1 = (((mat[1, 1] + mat[1, 2] + mat[2, 1] + mat[2, 2]) * cornerCopy[1] + (mat[0, 1] + mat[0, 2]) * (cornerCopy - widthstep)[1] + (mat[1, 0] + mat[2, 0]) * (cornerCopy - nC)[1] + mat[0, 0] * (cornerCopy - widthstep - nC)[1]) / weight);
            d2 = (((mat[1, 1] + mat[1, 2] + mat[2, 1] + mat[2, 2]) * cornerCopy[2] + (mat[0, 1] + mat[0, 2]) * (cornerCopy - widthstep)[2] + (mat[1, 0] + mat[2, 0]) * (cornerCopy - nC)[2] + mat[0, 0] * (cornerCopy - widthstep - nC)[2]) / weight);

            corner[0] = (byte)(d0 > 255 ? 255 : (d0 < 0 ? Math.Abs(d0) : d0));
            corner[1] = (byte)(d1 > 255 ? 255 : (d1 < 0 ? Math.Abs(d1) : d1));
            corner[2] = (byte)(d2 > 255 ? 255 : (d2 < 0 ? Math.Abs(d2) : d2));

            for (int i = 1; i < width - 1; i++)
            {
                //top margin
                byte* margemSup = dataPtr + i * nC;
                byte* margemSupCop = dataPtrCopy + i * nC;

                byte* mSupLeft = margemSupCop - nC;
                byte* mSupRight = margemSupCop + nC;

                d0 = (((mat[1, 1] + mat[0, 1]) * margemSupCop[0] + (mat[1, 0] + mat[0, 0]) * mSupLeft[0] + (mat[2, 1] + mat[2, 0]) * mSupRight[0] + mat[2, 1] * (margemSupCop + widthstep)[0] + mat[2, 2] * (mSupRight + widthstep)[0] + mat[2, 0] * (mSupLeft + widthstep)[0]) / weight);
                d1 = (((mat[1, 1] + mat[0, 1]) * margemSupCop[1] + (mat[1, 0] + mat[0, 0]) * mSupLeft[1] + (mat[2, 1] + mat[2, 0]) * mSupRight[1] + mat[2, 1] * (margemSupCop + widthstep)[1] + mat[2, 2] * (mSupRight + widthstep)[1] + mat[2, 0] * (mSupLeft + widthstep)[1]) / weight);
                d2 = (((mat[1, 1] + mat[0, 1]) * margemSupCop[2] + (mat[1, 0] + mat[0, 0]) * mSupLeft[2] + (mat[2, 1] + mat[2, 0]) * mSupRight[2] + mat[2, 1] * (margemSupCop + widthstep)[2] + mat[2, 2] * (mSupRight + widthstep)[2] + mat[2, 0] * (mSupLeft + widthstep)[2]) / weight);

                margemSup[0] = (byte)(d0 > 255 ? 255 : (d0 < 0 ? Math.Abs(d0) : d0));
                margemSup[1] = (byte)(d1 > 255 ? 255 : (d1 < 0 ? Math.Abs(d1) : d1));
                margemSup[2] = (byte)(d2 > 255 ? 255 : (d2 < 0 ? Math.Abs(d2) : d2));

                //bottom margin
                byte* margemInf = dataPtr + (height - 1) * widthstep + i * nC;
                byte* margemInfCop = dataPtrCopy + (height - 1) * widthstep + i * nC;

                byte* mInfLeft = margemInfCop - nC;
                byte* mInfRight = margemInfCop + nC;

                d0 = (((mat[1, 1] + mat[2, 1]) * margemInfCop[0] + (mat[1, 0] + mat[2, 0]) * mInfLeft[0] + (mat[1, 2] + mat[2, 2]) * mInfRight[0] + mat[0, 1] * (margemInfCop - widthstep)[0] + mat[0, 2] * (mInfRight - widthstep)[0] + mat[0, 0] * (mInfLeft - widthstep)[0]) / weight);
                d1 = (((mat[1, 1] + mat[2, 1]) * margemInfCop[1] + (mat[1, 0] + mat[2, 0]) * mInfLeft[1] + (mat[1, 2] + mat[2, 2]) * mInfRight[1] + mat[0, 1] * (margemInfCop - widthstep)[1] + mat[0, 2] * (mInfRight - widthstep)[1] + mat[0, 0] * (mInfLeft - widthstep)[1]) / weight);
                d2 = (((mat[1, 1] + mat[2, 1]) * margemInfCop[2] + (mat[1, 0] + mat[2, 0]) * mInfLeft[2] + (mat[1, 2] + mat[2, 2]) * mInfRight[2] + mat[0, 1] * (margemInfCop - widthstep)[2] + mat[0, 2] * (mInfRight - widthstep)[2] + mat[0, 0] * (mInfLeft - widthstep)[2]) / weight);

                margemInf[0] = (byte)(d0 > 255 ? 255 : (d0 < 0 ? Math.Abs(d0) : d0));
                margemInf[1] = (byte)(d1 > 255 ? 255 : (d1 < 0 ? Math.Abs(d1) : d1));
                margemInf[2] = (byte)(d2 > 255 ? 255 : (d2 < 0 ? Math.Abs(d2) : d2));
            }

            for (int i = 1; i < height - 1; i++)
            {
                //left margin
                byte* margemEsq = dataPtr + i * widthstep;
                byte* margemEsqCop = dataPtrCopy + i * widthstep;

                byte* mEsqUp = margemEsqCop - widthstep;
                byte* mEsqDown = margemEsqCop + widthstep;

                d0 = (((mat[1, 1] + mat[1, 0]) * margemEsqCop[0] + (mat[0, 1] + mat[0, 0]) * mEsqUp[0] + (mat[2, 1] + mat[2, 0]) * mEsqDown[0] + mat[1, 2] * (margemEsqCop + nC)[0] + mat[2, 2] * (mEsqDown + nC)[0] + mat[0, 2] * (mEsqUp + nC)[0]) / weight);
                d1 = (((mat[1, 1] + mat[1, 0]) * margemEsqCop[1] + (mat[0, 1] + mat[0, 0]) * mEsqUp[1] + (mat[2, 1] + mat[2, 0]) * mEsqDown[1] + mat[1, 2] * (margemEsqCop + nC)[1] + mat[2, 2] * (mEsqDown + nC)[1] + mat[0, 2] * (mEsqUp + nC)[1]) / weight);
                d2 = (((mat[1, 1] + mat[1, 0]) * margemEsqCop[2] + (mat[0, 1] + mat[0, 0]) * mEsqUp[2] + (mat[2, 1] + mat[2, 0]) * mEsqDown[2] + mat[1, 2] * (margemEsqCop + nC)[2] + mat[2, 2] * (mEsqDown + nC)[2] + mat[0, 2] * (mEsqUp + nC)[2]) / weight);

                margemEsq[0] = (byte)(d0 > 255 ? 255 : (d0 < 0 ? Math.Abs(d0) : d0));
                margemEsq[1] = (byte)(d1 > 255 ? 255 : (d1 < 0 ? Math.Abs(d1) : d1));
                margemEsq[2] = (byte)(d2 > 255 ? 255 : (d2 < 0 ? Math.Abs(d2) : d2));

                //right margin
                byte* margemDir = dataPtr + (width - 1) * nC + i * widthstep;
                byte* margemDirCop = dataPtrCopy + (width - 1) * nC + i * widthstep;

                byte* mDirUp = margemDirCop - widthstep;
                byte* mDirDown = margemDirCop + widthstep;

                d0 = (((mat[1, 1] + mat[1, 2]) * margemDirCop[0] + (mat[0, 1] + mat[0, 2]) * mDirUp[0] + (mat[2, 1] + mat[2, 2]) * mDirDown[0] + mat[1, 0] * (margemDirCop - nC)[0] + mat[2, 0] * (mDirDown - nC)[0] + mat[0, 0] * (mDirUp - nC)[0]) / weight);
                d1 = (((mat[1, 1] + mat[1, 2]) * margemDirCop[1] + (mat[0, 1] + mat[0, 2]) * mDirUp[1] + (mat[2, 1] + mat[2, 2]) * mDirDown[1] + mat[1, 0] * (margemDirCop - nC)[1] + mat[2, 0] * (mDirDown - nC)[1] + mat[0, 0] * (mDirUp - nC)[1]) / weight);
                d2 = (((mat[1, 1] + mat[1, 2]) * margemDirCop[2] + (mat[0, 1] + mat[0, 2]) * mDirUp[2] + (mat[2, 1] + mat[2, 2]) * mDirDown[2] + mat[1, 0] * (margemDirCop - nC)[2] + mat[2, 0] * (mDirDown - nC)[2] + mat[0, 0] * (mDirUp - nC)[2]) / weight);

                margemDir[0] = (byte)(d0 > 255 ? 255 : (d0 < 0 ? Math.Abs(d0) : d0));
                margemDir[1] = (byte)(d1 > 255 ? 255 : (d1 < 0 ? Math.Abs(d1) : d1));
                margemDir[2] = (byte)(d2 > 255 ? 255 : (d2 < 0 ? Math.Abs(d2) : d2));
            }
        }
    }
}