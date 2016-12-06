using System;
using System.Linq;
using Emgu.CV.Structure;
using Emgu.CV;
using System.Drawing;

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

            int width = m.width;
            int height = m.height;
            int nChan = m.nChannels; // number of channels - 3
            int padding = m.widthStep - nChan * width; // alignment bytes (padding)

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    //gets the 3 components

                    // stores the new values in the image
                    dataPtr[0] = (byte)(255 - dataPtr[0]);
                    dataPtr[1] = (byte)(255 - dataPtr[1]);
                    dataPtr[2] = (byte)(255 - dataPtr[2]);

                    // moves the pointer to the next pixel
                    dataPtr += nChan;
                }

                //at the end of the line moves the pointer by the alignment bytes (padding)
                dataPtr += padding;

            }
        }

        /// <summary>
        /// Changes the brightness and contrast of a picture
        /// </summary>
        /// <param name="img">The picture to modify</param>
        /// <param name="brilho">The amount of brightness to add/remove</param>
        /// <param name="cont">The contrast multiplier</param>
        unsafe public static void BrightContrast(Image<Bgr, byte> img, int brilho, double cont)
        {

            // direct access to the image memory(sequencial)
            // direcion top left -> bottom right

            MIplImage m = img.MIplImage;
            byte* dataPtr = (byte*)m.imageData.ToPointer(); // Pointer to the image
            double blue, green, red;

            int width = m.width;
            int height = m.height;
            int nChan = m.nChannels; // number of channels - 3
            int padding = m.widthStep - nChan * width; // alignment bytes (padding)

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    //gets the 3 components
                    blue = Math.Round(dataPtr[0] * cont + brilho);
                    green = Math.Round(dataPtr[1] * cont + brilho);
                    red = Math.Round(dataPtr[2] * cont + brilho);

                    if (blue > 255) blue = 255;
                    if (green > 255) green = 255;
                    if (red > 255) red = 255;

                    if (blue < 0) blue = 0;
                    if (red < 0) red = 0;
                    if (green < 0) green = 0;

                    // stores the new values in the image
                    dataPtr[0] = (byte)blue;
                    dataPtr[1] = (byte)green;
                    dataPtr[2] = (byte)red;

                    // moves the pointer to the next pixel
                    dataPtr += nChan;
                }

                //at the end of the line moves the pointer by the alignment bytes (padding)
                dataPtr += padding;

            }
        }

        /// <summary>
        /// Converts an image to gray
        /// Direct access to memory
        /// </summary>
        /// <param name="img">The image</param>
        /// <param name="mode">Conversion mode</param>
        unsafe private static void ConvertToGray(Image<Bgr, byte> img, char mode)
        {
            // direct access to the image memory(sequencial)
            // direction top left -> bottom right

            MIplImage m = img.MIplImage;
            byte* dataPtr = (byte*)m.imageData.ToPointer(); // Pointer to the image
            byte gray;

            int width = m.width;
            int height = m.height;
            int nChan = m.nChannels; // number of channels - 3
            int padding = m.widthStep - nChan * width; // alignment bytes (padding)

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    //gets the 3 components
                    byte blue = dataPtr[0];
                    byte green = dataPtr[1];
                    byte red = dataPtr[2];

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

        /// <summary>
        /// Converts an image to gray, copying the red component's value to the other components
        /// </summary>
        /// <param name="img">The image</param>
        public static void RedChannel(Image<Bgr, byte> img)
        {
            ConvertToGray(img, 'R');
        }

        /// <summary>
        /// Converts an image to gray, copying the green component's value to the other components
        /// </summary>
        /// <param name="img">The image</param>
        public static void GreenChannel(Image<Bgr, byte> img)
        {
            ConvertToGray(img, 'G');
        }

        /// <summary>
        /// Converts an image to gray, copying the blue component's value to the other components
        /// </summary>
        /// <param name="img">The image</param>
        public static void BlueChannel(Image<Bgr, byte> img)
        {
            ConvertToGray(img, 'B');
        }

        /// <summary>
        /// Converts an image to gray, using the average value of the 3 components
        /// </summary>
        /// <param name="img">The image</param>
        public static void AvgChannel(Image<Bgr, byte> img)
        {
            ConvertToGray(img, 'M');
        }

        /// <summary>
        /// Moves an image
        /// </summary>
        /// <param name="img">The image to move</param>
        /// <param name="imgCopy">A copy of the image</param>
        /// <param name="dx">Amount of pixels to move in the X axis</param>
        /// <param name="dy">Amount of pixels to move in the Y axis</param>
        unsafe public static void Translation(Image<Bgr, byte> img, Image<Bgr, byte> imgCopy, int dx, int dy)
        {
            MIplImage copy = imgCopy.MIplImage;
            MIplImage m = img.MIplImage;
            byte* dataPtr = (byte*)m.imageData.ToPointer(); // Pointer to the image
            byte* dataPtrCopy = (byte*)copy.imageData.ToPointer(); // Pointer to the image copy

            int widthstep = m.widthStep;
            int nC = m.nChannels;
            int width = m.width;
            int height = m.height;


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
        /// <param name="imgCopy">A copy of the image</param>
        /// <param name="ang">The angle of rotation (in radians)</param>
        unsafe public static void Rotation(Image<Bgr, byte> img, Image<Bgr, byte> imgCopy, float ang)
        {
            MIplImage m = img.MIplImage;
            byte* dataPtr = (byte*)m.imageData.ToPointer(); // Pointer to the image
            byte* dataPtrCopy = (byte*)imgCopy.MIplImage.imageData.ToPointer(); // Pointer to the image copy

            int widthstep = m.widthStep;
            int nC = m.nChannels;
            int width = img.Width;
            int height = img.Height;
            double sine = Math.Sin(ang);
            double cose = Math.Cos(ang);
            int cx = width / 2;
            int cy = height / 2;
            int padding = widthstep - nC * width; // alignment bytes (padding)

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    //inverse rotation: gets the original position of the pixel from the new pixel's coordinates
                    int nx = (int)Math.Round((x - cx) * cose - (cy - y) * sine + cx);
                    int ny = (int)Math.Round(cy - (x - cx) * sine - (cy - y) * cose);
                    byte* orig = dataPtrCopy + ny * widthstep + nx * nC;
                    //byte* dest = dataPtr + y * widthstep + x * nC;

                    if (ny >= 0 && ny < height && nx >= 0 && nx < width)
                    {
                        dataPtr[0] = orig[0];
                        dataPtr[1] = orig[1];
                        dataPtr[2] = orig[2];
                    }

                    else //if we're trying to get pixels from outside the original image
                    {
                        dataPtr[0] = 0;
                        dataPtr[1] = 0;
                        dataPtr[2] = 0;
                    }
                    dataPtr += nC;

                }
                dataPtr += padding;
            }
        }

        /// <summary>
        /// Zooms in/out on an image.
        /// top-left corner is (0,0)
        /// </summary>
        /// <param name="img">The image</param>
        /// <param name="imgCopy">A copy of the image</param>
        /// <param name="factor">Zoom factor</param>
        unsafe public static void Scale(Image<Bgr, byte> img, Image<Bgr, byte> imgCopy, float factor)
        {
            Scale_point_xy(img, imgCopy, factor, img.Width, img.Height);
        }

        /// <summary>
        /// Zooms in/out on an image.
        /// mouseX and mouseY determine the new top-left corner
        /// </summary>
        /// <param name="img">The image</param>
        /// <param name="imgCopy">A copy of the image</param>
        /// <param name="factor">Zoom factor</param>
        /// <param name="mouseX">Current X position of the mouse</param>
        /// <param name="mouseY">Current Y position of the mouse</param>
        unsafe public static void Scale_point_xy(Image<Bgr, byte> img, Image<Bgr, byte> imgCopy, float factor, int mouseX, int mouseY)
        {
            if (factor == 1 || factor < 0) return;
            MIplImage copy = imgCopy.MIplImage;
            MIplImage m = img.MIplImage;
            byte* dataPtr = (byte*)m.imageData.ToPointer(); // Pointer to the image
            byte* dataPtrCopy = (byte*)copy.imageData.ToPointer(); // Pointer to the image copy

            int widthstep = m.widthStep;
            int nC = m.nChannels;
            int width = m.width;
            int height = m.height;
            int cx = width / 2;
            int cy = height / 2;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    //Gets the original position of the pixel
                    int nx = (int)Math.Round((x - cx) / factor + mouseX);
                    int ny = (int)Math.Round((y - cy) / factor + mouseY);
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
        /// *** NEEDS MARGINS ***
        /// Calculates the mean of an image, using solution A.
        /// Each pixel is replaced by the mean of their neighborhood (3x3)
        /// </summary>
        /// <param name="img">The image</param>
        /// <param name="imgCopy">A copy of the image</param>
        unsafe public static void Mean(Image<Bgr, byte> img, Image<Bgr, byte> imgCopy)
        {
            MIplImage copy = imgCopy.MIplImage;
            MIplImage m = img.MIplImage;
            byte* dataPtr = (byte*)m.imageData.ToPointer(); // Pointer to the image
            byte* dataPtrCopy = (byte*)copy.imageData.ToPointer(); // Pointer to the image copy

            int widthstep = m.widthStep;
            int nC = m.nChannels;
            int width = m.width;
            int height = m.height;
            
            for(int y = 1; y < height - 1; y++)
            {
                for (int x = 1; x < width - 1; x++)
                {
                    byte* dest = dataPtr + y * widthstep + x * nC;
                    byte* orig = dataPtrCopy + (y - 1) * widthstep + (x - 1) * nC;
                    dest[0] = (byte)Math.Round((orig[0] + orig[nC] + orig[2 * nC] + orig[widthstep] + orig[widthstep + nC] +
                        orig[widthstep + 2 * nC] + orig[2 * widthstep] + orig[2 * widthstep + nC] + orig[2 * widthstep + 2 * nC]) / 9.0);
                    dest[1] = (byte)Math.Round((orig[1] + orig[nC + 1] + orig[2 * nC + 1] + orig[widthstep + 1] + orig[widthstep + nC + 1] +
                        orig[widthstep + 2 * nC + 1] + orig[2 * widthstep + 1] + orig[2 * widthstep + nC + 1] + orig[2 * widthstep + 2 * nC + 1]) / 9.0);
                    dest[2] = (byte)Math.Round((orig[2] + orig[nC + 2] + orig[2 * nC + 2] + orig[widthstep + 2] + orig[widthstep + nC + 2] +
                        orig[widthstep + 2 * nC + 2] + orig[2 * widthstep + 2] + orig[2 * widthstep + nC + 2] + orig[2 * widthstep + 2 * nC + 2]) / 9.0);

                }
            }

            //fazer bordas
        }

        /// <summary>
        /// Performs the binarization of an image, based on the Otsu method
        /// </summary>
        /// <param name="img">The image to binarize</param>
        public static void ConvertToBW_Otsu(Image<Bgr, byte> img)
        {
            //covariance = q1*q2*(u1-u2)^2
            //q1 = sum(i = 0, t, P(i)) -> sum of the probabilities of a pixel being below the chosen threshold
            //q2 = sum(i = t + 1, 255, P(i)) -> sum of the probabilities of a pixel being above the chosen threshold
            //u1 = sum(i = 0, t, i * P(i))/q1 -> weighted mean (below threshold)
            //u2 = sum(i = t + 1, 255, i * P(i))/q2 -> weighted mean (above threshold)
            int[] intensity = Histogram_Gray(img);
            double[] probs = new double[256]; //will save the various probabilities
            MIplImage m = img.MIplImage;
            int npixels = m.width * m.height;
            for (int i = 0; i < 256; i++) //calculates probabilities for each intensity
            {
                probs[i] = (double)intensity[i] / npixels;
            }
            int threshold = 0;
            double covar = 0.0;
            for (int t = 1; t < 255; t++) //calculates covariance for each threshold
            {
                double q1 = 0.0, q2 = 0.0, u1 = 0.0, u2 = 0.0;
                int i;
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
                double res = q1 * q2 * (u1 - u2) * (u1 - u2);
                if(res > covar)
                {
                    threshold = t;
                    covar = res;
                }
            }
            ConvertToBW(img, threshold); //binarizes the image based on the chosen threshold
        }

        /// <summary>
        /// *** NOT QUITE PERFECT YET ***
        /// Performs the binarization of an image, based on a threshold
        /// </summary>
        /// <param name="img">The image to binarize</param>
        /// <param name="threshold">The binarization threshold</param>
        unsafe public static void ConvertToBW(Image<Bgr, byte> img, int threshold)
        {
            AvgChannel(img); //for best results we should convert the image to grayscale first
            MIplImage m = img.MIplImage;
            byte* dataPtr = (byte*)m.imageData.ToPointer(); // Pointer to the image

            int widthstep = m.widthStep;
            int nC = m.nChannels;
            int width = m.width;
            int height = m.height;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    byte* dest = dataPtr + y * widthstep + x * nC;

                    if (dest[0] >= threshold)
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
        /// Calculates the intensity histrogram of an image
        /// </summary>
        /// <param name="img">The image</param>
        /// <returns>An array with the histogram values (intensity)</returns>
        unsafe public static int[] Histogram_Gray(Image<Bgr, byte> img)
        {
            int[] intensity = new int[256];
            MIplImage m = img.MIplImage;
            byte* imgPtr = (byte*)m.imageData.ToPointer();

            int widthstep = m.widthStep;
            int nC = m.nChannels;
            int width = m.width;
            int height = m.height;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    byte* pixelPtr = imgPtr + y * widthstep + x * nC;
                    intensity[(int)Math.Round((pixelPtr[0] + pixelPtr[1] + pixelPtr[2]) / 3.0)]++;
                }
            }

            return intensity;
        }

        /// <summary>
        /// Calculates the RGB histogram of an image
        /// </summary>
        /// <param name="img">The image</param>
        /// <returns>A matrix with the histogram values (blue, green and red)</returns>
        unsafe public static int[,] Histogram_RGB(Image<Bgr, byte> img)
        {
            int[,] bgrMat = new int[3, 256];
            MIplImage m = img.MIplImage;
            byte* dataPtr = (byte*)img.MIplImage.imageData.ToPointer(); // Pointer to the image

            int widthstep = m.widthStep;
            int nC = m.nChannels;
            int width = m.width;
            int height = m.height;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    byte* pixelPtr = dataPtr + y * widthstep + x * nC;
                    bgrMat[0, pixelPtr[0]]++;
                    bgrMat[1, pixelPtr[1]]++;
                    bgrMat[2, pixelPtr[2]]++;
                }
            }

            return bgrMat;
        }

        /// <summary>
        /// Calculates the RGB and intensity histograms of an image
        /// </summary>
        /// <param name="img">The image</param>
        /// <returns>A matrix with the histogram values (intensity, blue, red, green)</returns>
        public static int[,] Histogram_All(Image<Bgr, byte> img)
        {
            int[] intensity = Histogram_Gray(img);
            int[,] bgrMat = Histogram_RGB(img);
            int[,] ibgrMat = new int[4, 256];
            for (int x = 0; x < 256; x++)
            {
                ibgrMat[0, x] = intensity[x];
                for (int i = 1; i < 4; i++) ibgrMat[i, x] = bgrMat[i - 1, x];
            }

            return ibgrMat;
        }

        /// <summary>
        /// *** VERY INEFFICIENT, MARGINS NOT VERY GOOD ***
        /// Performs a median filter on an image
        /// </summary>
        /// <param name="img">The image</param>
        /// <param name="imgCopy">A copy of the image</param>
        unsafe public static void Median(Image<Bgr, byte> img, Image<Bgr, byte> imgCopy)
        {
            MIplImage m = img.MIplImage;
            MIplImage copy = imgCopy.MIplImage;
            byte* dataPtr = (byte*)m.imageData.ToPointer(); // Pointer to the image
            byte* dataPtrCopy = (byte*)copy.imageData.ToPointer(); // Pointer to the image copy

            int widthstep = m.widthStep;
            int nC = m.nChannels;
            int width = m.width;
            int height = m.height;

            for (int y = 1; y < height - 1; y++)
            {
                for (int x = 1; x < width - 1; x++)
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

            for (int x = 1; x < width - 1; x++)
            { //calculates the median for top and bottom margin pixels
                byte* pixelPtr = dataPtr + x * nC;
                byte* pixelPtrCopy = dataPtrCopy + (x - 1) * nC;
                CalculateMedianMargins(m, pixelPtrCopy, pixelPtr, 1, 0); //top
                CalculateMedianMargins(m, pixelPtrCopy, pixelPtr, 1, height - 1); //bottom
            }

            for (int y = 1; y < height - 1; y++)
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
        unsafe private static void CalculateMedian(MIplImage m, byte* origPtr, byte* copyPtr)
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
        /// *** NEEDS MARGINS ***
        /// Performs a Sobel filter on an image
        /// </summary>
        /// <param name="img">The image</param>
        /// <param name="imgCopy">A copy of the image</param>
        unsafe public static void Sobel(Image<Bgr, byte> img, Image<Bgr, byte> imgCopy)
        {
            MIplImage m = img.MIplImage;
            byte* dataPtr = (byte*)m.imageData.ToPointer();
            byte* dataPtrCopy = (byte*)imgCopy.MIplImage.imageData.ToPointer();
            int width = m.width;
            int height = m.height;
            int widthstep = m.widthStep;
            int nC = m.nChannels;

            for(int y = 1; y < height - 1; y++)
            {
                for(int x = 1; x < width - 1; x++)
                {
                    byte* orig = dataPtrCopy + y * widthstep + x * nC;
                    byte* dest = dataPtr + y * widthstep + x * nC;

                    int b = Math.Abs((orig - widthstep - nC)[0] - (orig + widthstep - nC)[0] + 2 * (orig - widthstep)[0] - 2 * (orig + widthstep)[0] +
                        (orig + nC - widthstep)[0] - (orig + nC + widthstep)[0]) + Math.Abs(-(orig - nC - widthstep)[0] - 2 * (orig - nC)[0] - 
                        (orig - nC + widthstep)[0] + (orig + nC - widthstep)[0] + 2 * (orig + nC)[0] + (orig + nC + widthstep)[0]);

                    int g = Math.Abs((orig - widthstep - nC)[1] - (orig + widthstep - nC)[1] + 2 * (orig - widthstep)[1] - 2 * (orig + widthstep)[1] +
                        (orig + nC - widthstep)[1] - (orig + nC + widthstep)[1]) + Math.Abs(-(orig - nC - widthstep)[1] - 2 * (orig - nC)[1] -
                        (orig - nC + widthstep)[1] + (orig + nC - widthstep)[1] + 2 * (orig + nC)[1] + (orig + nC + widthstep)[1]);

                    int r = Math.Abs((orig - widthstep - nC)[2] - (orig + widthstep - nC)[2] + 2 * (orig - widthstep)[2] - 2 * (orig + widthstep)[2] +
                        (orig + nC - widthstep)[2] - (orig + nC + widthstep)[2]) + Math.Abs(-(orig - nC - widthstep)[2] - 2 * (orig - nC)[2] -
                        (orig - nC + widthstep)[2] + (orig + nC - widthstep)[2] + 2 * (orig + nC)[2] + (orig + nC + widthstep)[2]);

                    dest[0] = (byte)(b > 255 ? 255 : b);
                    dest[1] = (byte)(g > 255 ? 255 : g);
                    dest[2] = (byte)(r > 255 ? 255 : r);
                }
            }

            //fazer bordas

            //float[,] mat1 = { { 1, 0, -1 }, { 2, 0, -2 }, { 1, 0, -1 } };

            //float[,] mat2 = { { -1, -2, -1 }, { 0, 0, 0 }, { 1, 2, 1 } };
        }

        /// <summary>
        /// Performs a differential edge detection filter on an image
        /// </summary>
        /// <param name="img">The image</param>
        /// <param name="imgCopy">A copy of the image</param>
        unsafe public static void Diferentiation(Image<Bgr, byte> img, Image<Bgr, byte> imgCopy)
        {
            MIplImage m = img.MIplImage;
            MIplImage copy = imgCopy.MIplImage;
            byte* dataPtr = (byte*)m.imageData.ToPointer(); // Pointer to the image
            byte* dataPtrCopy = (byte*)copy.imageData.ToPointer(); // Pointer to the image copy

            int widthstep = m.widthStep;
            int nC = m.nChannels;
            int width = m.width;
            int height = m.height;

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
        unsafe private static void SumPixels(Image<Bgr, byte> img1, Image<Bgr, byte> img2)
        {
            MIplImage m1 = img1.MIplImage;
            byte* dataPtr1 = (byte*)m1.imageData.ToPointer();
            byte* dataPtr2 = (byte*)img2.MIplImage.imageData.ToPointer();

            int widthstep = m1.widthStep;
            int nC = m1.nChannels;
            int width = m1.width;
            int height = m1.height;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
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
        /// *** ALMOST GOOD ***
        /// Performs a non-uniform filter on an image
        /// </summary>
        /// <param name="img">The image</param>
        /// <param name="mat">The filter's matrix (multipliers)</param>
        /// <param name="weight">The weight of the final sum</param>
        unsafe public static void NonUniform(Image<Bgr, byte> img, Image<Bgr, byte> imgCopy, float[,] mat, float weight)
        {
            MIplImage m = img.MIplImage;
            byte* dataPtr = (byte*)m.imageData.ToPointer(); // Pointer to the image
            byte* dataPtrCopy = (byte*)imgCopy.MIplImage.imageData.ToPointer(); // Pointer to the image copy

            int widthstep = m.widthStep;
            int nC = m.nChannels;
            int width = m.width;
            int height = m.height;

            //calculates the central pixels' values
            for (int y = 1; y < height - 1; y++)
            {
                for (int x = 1; x < width - 1; x++)
                {
                    byte* dest = dataPtr + y * widthstep + x * nC;
                    //sum of each pixel's component, after multiplying them by the respective factor
                    float[] sum = { 0, 0, 0 };
                    for (int j = -1; j <= 1; j++)
                    {
                        for (int i = -1; i <= 1; i++)
                        {
                            byte* orig = dataPtrCopy + (y + j) * widthstep + (x + i) * nC;
                            //gets multiplier from the matrix and applies it to each component
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
            float d0 = (((mat[0, 0] + mat[0, 1] + mat[1, 0] + mat[1, 1]) * dataPtrCopy[0] + (mat[2, 1] + mat[2, 0]) * (dataPtrCopy + widthstep)[0] + (mat[1, 2] + mat[0, 2]) * (dataPtrCopy + nC)[0] + mat[2, 2] * (dataPtrCopy + widthstep + nC)[0]) / weight);
            float d1 = (((mat[0, 0] + mat[0, 1] + mat[1, 0] + mat[1, 1]) * dataPtrCopy[1] + (mat[2, 1] + mat[2, 0]) * (dataPtrCopy + widthstep)[1] + (mat[1, 2] + mat[0, 2]) * (dataPtrCopy + nC)[1] + mat[2, 2] * (dataPtrCopy + widthstep + nC)[1]) / weight);
            float d2 = (((mat[0, 0] + mat[0, 1] + mat[1, 0] + mat[1, 1]) * dataPtrCopy[2] + (mat[2, 1] + mat[2, 0]) * (dataPtrCopy + widthstep)[2] + (mat[1, 2] + mat[0, 2]) * (dataPtrCopy + nC)[2] + mat[2, 2] * (dataPtrCopy + widthstep + nC)[2]) / weight);
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

        /// <summary>
        /// *** TO DO ***
        /// Calculates the mean of an image, using solution B.
        /// Each pixel is replaced by the mean of their neighborhood (3x3)
        /// </summary>
        /// <param name="img">The image</param>
        /// <param name="imgCopy">A copy of the image</param>
        public static void Mean_solutionB(Image<Bgr, byte> img, Image<Bgr, byte> imgCopy)
        {

        }

        /// <summary>
        /// *** TO DO ***
        /// Calculates the mean of an image, using solution C.
        /// Each pixel is replaced by the mean of their neighborhood (size x size)
        /// </summary>
        /// <param name="img">The image</param>
        /// <param name="imgCopy">A copy of the image</param>
        /// <param name="size">The size of the filter</param>
        public static void Mean_solutionC(Image<Bgr, byte> img, Image<Bgr, byte> imgCopy, int size)
        {

        }

        /// <summary>
        /// Performs a Roberts filter on an image
        /// </summary>
        /// <param name="img">The image</param>
        /// <param name="imgCopy">A copy of the image</param>
        unsafe public static void Roberts(Image<Bgr, byte> img, Image<Bgr, byte> imgCopy)
        {
            MIplImage m = img.MIplImage;
            byte* dataPtr = (byte*)m.imageData.ToPointer(); // Pointer to the image
            byte* dataPtrCopy = (byte*)imgCopy.MIplImage.imageData.ToPointer(); // Pointer to the image copy

            int widthstep = m.widthStep;
            int nC = m.nChannels;
            int width = m.width;
            int height = m.height;

            //Calculates the filter's value for all pixels except bottom and right margins
            for (int y = 0; y < height - 1; y++)
            {
                for (int x = 0; x < width - 1; x++)
                {
                    byte* dest = dataPtr + y * widthstep + x * nC;
                    byte* currPt = dataPtrCopy + y * widthstep + x * nC;
                    int[] sum = { 0, 0, 0 };
                    byte* rightPt = dataPtrCopy + y * widthstep + (x + 1) * nC; //the pixel to the right
                    byte* rightdownPt = dataPtrCopy + (y + 1) * widthstep + (x + 1) * nC; //the pixel to the right
                    byte* downPt = dataPtrCopy + (y + 1) * widthstep + x * nC; //the pixel below
                    sum[0] = Math.Abs(currPt[0] - rightdownPt[0]) + Math.Abs(rightPt[0] - downPt[0]);
                    sum[1] = Math.Abs(currPt[1] - rightdownPt[1]) + Math.Abs(rightPt[1] - downPt[1]);
                    sum[2] = Math.Abs(currPt[2] - rightdownPt[2]) + Math.Abs(rightPt[2] - downPt[2]);
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
                sum[0] = 2 * Math.Abs(currPt[0] - downPt[0]);
                sum[1] = 2 * Math.Abs(currPt[1] - downPt[1]);
                sum[2] = 2 * Math.Abs(currPt[2] - downPt[2]);
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
                sum[0] = 2 * Math.Abs(currPt[0] - rightPt[0]);
                sum[1] = 2 * Math.Abs(currPt[1] - rightPt[1]);
                sum[2] = 2 * Math.Abs(currPt[2] - rightPt[2]);
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
        /// *** TO DO ***
        /// Finds a license plate in an image and returns its location and respective characters
        /// </summary>
        /// <param name="img">The image</param>
        /// <param name="imgCopy">A copy of the image</param>
        /// <param name="LP_Location">The location of the license plate</param>
        /// <param name="LP_Chr1">The location of the first character</param>
        /// <param name="LP_Chr2">The location of the second character</param>
        /// <param name="LP_Chr3">The location of the third character</param>
        /// <param name="LP_Chr4">The location of the fourth character</param>
        /// <param name="LP_Chr5">The location of the fifth character</param>
        /// <param name="LP_Chr6">The location of the sixth character</param>
        /// <param name="LP_C1">The value of the first character</param>
        /// <param name="LP_C2">The value of the second character</param>
        /// <param name="LP_C3">The value of the third character</param>
        /// <param name="LP_C4">The value of the fourth character</param>
        /// <param name="LP_C5">The value of the fifth character</param>
        /// <param name="LP_C6">The value of the sixth character</param>
        /// <param name="LP_Country">The license plate's country</param>
        /// <param name="LP_Month">The license plate's month</param>
        /// <param name="LP_Year">The license plate's year</param>
        public static void LP_Recognition(Image<Bgr, byte> img, Image<Bgr, byte> imgCopy, out Rectangle LP_Location, out Rectangle LP_Chr1,
            out Rectangle LP_Chr2, out Rectangle LP_Chr3, out Rectangle LP_Chr4, out Rectangle LP_Chr5, out Rectangle LP_Chr6,
            out string LP_C1, out string LP_C2, out string LP_C3, out string LP_C4, out string LP_C5, out string LP_C6,
            out string LP_Country, out string LP_Month, out string LP_Year)
        {
            LP_Location = new Rectangle();
            LP_Chr1 = new Rectangle();
            LP_Chr2 = new Rectangle();
            LP_Chr3 = new Rectangle();
            LP_Chr4 = new Rectangle();
            LP_Chr5 = new Rectangle();
            LP_Chr6 = new Rectangle();
            LP_C1 = "";
            LP_C2 = "";
            LP_C3 = "";
            LP_C4 = "";
            LP_C5 = "";
            LP_C6 = "";
            LP_Country = "";
            LP_Month = "";
            LP_Year = "";
        }

        /// <summary>
        /// Rotates an image using bilinear interpolation
        /// </summary>
        /// <param name="img">The image to rotate</param>
        /// <param name="imgCopy">A copy of the image</param>
        /// <param name="angle">The angle of rotation (in radians)</param>
        unsafe public static void Rotation_Bilinear(Image<Bgr, byte> img, Image<Bgr, byte> imgCopy, float angle)
        {
            MIplImage m = img.MIplImage;
            byte* dataPtr = (byte*)m.imageData.ToPointer(); // Pointer to the image
            byte* dataPtrCopy = (byte*)imgCopy.MIplImage.imageData.ToPointer(); // Pointer to the image copy

            int widthstep = m.widthStep;
            int nC = m.nChannels;
            int width = img.Width;
            int height = img.Height;
            double sine = Math.Sin(angle);
            double cose = Math.Cos(angle);
            int cx = width / 2;
            int cy = height / 2;
            int padding = widthstep - nC * width; // alignment bytes (padding)

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    //inverse rotation: gets the original position of the pixel from the new pixel's coordinates
                    double nx = (x - cx) * cose - (cy - y) * sine + cx;
                    double ny = cy - (x - cx) * sine - (cy - y) * cose;

                    if (ny >= 0 && ny < height && nx >= 0 && nx < width)
                    {
                        byte[] orig = BilinearInterpol(nx, ny, nC, m.widthStep, dataPtrCopy);
                        dataPtr[0] = orig[0];
                        dataPtr[1] = orig[1];
                        dataPtr[2] = orig[2];
                    }

                    else //if we're trying to get pixels from outside the original image
                    {
                        dataPtr[0] = 0;
                        dataPtr[1] = 0;
                        dataPtr[2] = 0;
                    }
                    dataPtr += nC;

                }
                dataPtr += padding;
            }
        }

        /// <summary>
        /// Zooms in/out on an image, using bilinear interpolation.
        /// top-left corner is (0,0)
        /// </summary>
        /// <param name="img">The image</param>
        /// <param name="imgCopy">A copy of the image</param>
        /// <param name="factor">Zoom factor</param>
        public static void Scale_Bilinear(Image<Bgr, byte> img, Image<Bgr, byte> imgCopy, float factor)
        {
            Scale_point_xy_Bilinear(img, imgCopy, factor, img.Width, img.Height);
        }

        /// <summary>
        /// Zooms in/out on an image, using bilinear interpolation.
        /// mouseX and mouseY determine the new top-left corner
        /// </summary>
        /// <param name="img">The image</param>
        /// <param name="imgCopy">A copy of the image</param>
        /// <param name="factor">Zoom factor</param>
        /// <param name="mouseX">Current X position of the mouse</param>
        /// <param name="mouseY">Current Y position of the mouse</param>
        unsafe public static void Scale_point_xy_Bilinear(Image<Bgr, byte> img, Image<Bgr, byte> imgCopy, float factor, int mouseX, int mouseY)
        {
            if (factor == 1 || factor < 0) return;
            MIplImage m = img.MIplImage;
            byte* dataPtr = (byte*)m.imageData.ToPointer(); // Pointer to the image
            byte* dataPtrCopy = (byte*)imgCopy.MIplImage.imageData.ToPointer(); // Pointer to the image copy

            int widthstep = m.widthStep;
            int nC = m.nChannels;
            int width = m.width;
            int height = m.height;
            int cx = width / 2;
            int cy = height / 2;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    //Gets the original position of the pixel
                    double nx = (x - cx) / factor + mouseX;
                    double ny = (y - cy) / factor + mouseY;
                    
                    byte* dest = dataPtr + y * widthstep + x * nC;

                    if (ny >= 0 && ny < height && nx >= 0 && nx < width)
                    {
                        byte[] orig = BilinearInterpol(nx, ny, nC, widthstep, dataPtrCopy);
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
        /// Calculates bilinear interpolation based on 2 non-integer coordinates
        /// </summary>
        /// <param name="x">The X coordinate</param>
        /// <param name="y">The Y coordinate</param>
        /// <param name="nC">Number of channels of the image</param>
        /// <param name="widthstep">Width-step of the image</param>
        /// <param name="imagePtr">A pointer to the image content</param>
        /// <returns>The result of bilinear interpolation using the given X and Y coordinates</returns>
        unsafe private static byte[] BilinearInterpol(double x, double y, int nC, int widthstep, byte* imagePtr)
        {
            int lx = (int)x;
            int uy = (int)y;
            double xdec = x - lx;
            double ydec = y - uy;
            byte[] newBGR = new byte[3];
            byte* luPtr = imagePtr + lx * nC + uy * widthstep, ruPtr = luPtr + nC;
            byte* ldPtr = luPtr + widthstep, rdPtr = ruPtr + widthstep;
            newBGR[0] = (byte)Math.Round((1 - ydec) * ((1 - xdec) * luPtr[0] + xdec * ruPtr[0]) + ydec * ((1 - xdec) * ldPtr[0] + xdec * rdPtr[0]));
            newBGR[1] = (byte)Math.Round((1 - ydec) * ((1 - xdec) * luPtr[1] + xdec * ruPtr[1]) + ydec * ((1 - xdec) * ldPtr[1] + xdec * rdPtr[1]));
            newBGR[2] = (byte)Math.Round((1 - ydec) * ((1 - xdec) * luPtr[2] + xdec * ruPtr[2]) + ydec * ((1 - xdec) * ldPtr[2] + xdec * rdPtr[2]));
            return newBGR;
        }
    }
}