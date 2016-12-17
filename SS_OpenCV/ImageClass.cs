﻿using System;
using System.IO;
using System.Linq;
using Emgu.CV.Structure;
using Emgu.CV;
using System.Drawing;
using System.Collections.Generic;

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
                    int origY = y - dy;
                    int origX = x - dx;
                    //Gets the original position of the pixel
                    byte* orig = dataPtrCopy + origY * widthstep + origX * nC;
                    byte* dest = dataPtr + y * widthstep + x * nC;

                    if (origY >= 0 && origY < height && origX >= 0 && origX < width)
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

            for (int y = 1; y < height - 1; y++)
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
            MIplImage m = img.MIplImage;
            int npixels = m.width * m.height;
            int threshold = 0;
            double covar = 0.0;
            for (int t = 0; t < 255; t++) //calculates covariance for each threshold
            {
                double q1 = 0.0, q2 = 0.0, u1 = 0.0, u2 = 0.0;
                int i;
                for (i = 0; i <= t; i++)
                {
                    double probs = (double)intensity[i] / npixels;
                    q1 += probs;
                    u1 += probs * i;
                }
                for (; i < 256; i++)
                {
                    double probs = (double)intensity[i] / npixels;
                    q2 += probs;
                    u2 += probs * i;
                }
                u1 /= q1;
                u2 /= q2;
                double res = q1 * q2 * (u1 - u2) * (u1 - u2);
                if (res > covar)
                {
                    threshold = t;
                    covar = res;
                }
            }
            ConvertToBW(img, threshold); //binarizes the image based on the chosen threshold
        }

        /// <summary>
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

                    if (dest[0] > threshold)
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

            for (int y = 1; y < height - 1; y++)
            {
                for (int x = 1; x < width - 1; x++)
                {
                    byte* orig = dataPtrCopy + y * widthstep + x * nC;
                    byte* dest = dataPtr + y * widthstep + x * nC;

                    byte* topleft = orig - widthstep - nC;
                    byte* top = orig - widthstep;
                    byte* topright = orig - widthstep + nC;
                    byte* left = orig - nC;
                    byte* right = orig + nC;
                    byte* bttmleft = orig + widthstep - nC;
                    byte* bottom = orig + widthstep;
                    byte* bttmright = orig + widthstep + nC;

                    int b = Math.Abs(topleft[0] - bttmleft[0] + 2 * top[0] - 2 * bottom[0] + topright[0] - bttmright[0])
                        + Math.Abs(-topleft[0] - 2 * left[0] - bttmleft[0] + topright[0] + 2 * right[0] + bttmright[0]);

                    int g = Math.Abs(topleft[1] - bttmleft[1] + 2 * top[1] - 2 * bottom[1] + topright[1] - bttmright[1])
                        + Math.Abs(-topleft[1] - 2 * left[1] - bttmleft[1] + topright[1] + 2 * right[1] + bttmright[1]);

                    int r = Math.Abs(topleft[2] - bttmleft[2] + 2 * top[2] - 2 * bottom[2] + topright[2] - bttmright[2])
                        + Math.Abs(-topleft[2] - 2 * left[2] - bttmleft[2] + topright[2] + 2 * right[2] + bttmright[2]);

                    dest[0] = (byte)(b > 255 ? 255 : b);
                    dest[1] = (byte)(g > 255 ? 255 : g);
                    dest[2] = (byte)(r > 255 ? 255 : r);
                }
            }

            //top/bottom margins
            for (int x = 1; x < width - 1; x++)
            {
                byte* orig = dataPtrCopy + x * nC;
                byte* dest = dataPtr + x * nC;

                byte* left = orig - nC;
                byte* right = orig + nC;
                byte* bttmleft = orig + widthstep - nC;
                byte* bottom = orig + widthstep;
                byte* bttmright = orig + widthstep + nC;

                int b = Math.Abs(left[0] - bttmleft[0] + 2 * orig[0] - 2 * bottom[0] + right[0] - bttmright[0])
                    + Math.Abs(-3 * left[0] - bttmleft[0] + 3 * right[0] + bttmright[0]);

                int g = Math.Abs(left[1] - bttmleft[1] + 2 * orig[1] - 2 * bottom[1] + right[1] - bttmright[1])
                    + Math.Abs(-3 * left[1] - bttmleft[1] + 3 * right[1] + bttmright[1]);

                int r = Math.Abs(left[2] - bttmleft[2] + 2 * orig[2] - 2 * bottom[2] + right[2] - bttmright[2])
                    + Math.Abs(-3 * left[2] - bttmleft[2] + 3 * right[2] + bttmright[2]);

                dest[0] = (byte)(b > 255 ? 255 : b);
                dest[1] = (byte)(g > 255 ? 255 : g);
                dest[2] = (byte)(r > 255 ? 255 : r);

                orig = dataPtrCopy + (height - 1) * widthstep + x * nC;
                dest = dataPtr + (height - 1) * widthstep + x * nC;

                byte* topleft = orig - widthstep - nC;
                byte* top = orig - widthstep;
                byte* topright = orig - widthstep + nC;
                left = orig - nC;
                right = orig + nC;

                b = Math.Abs(topleft[0] - left[0] + 2 * top[0] - 2 * orig[0] + topright[0] - right[0])
                    + Math.Abs(-3 * left[0] - topleft[0] + 3 * right[0] + topright[0]);

                g = Math.Abs(topleft[1] - left[1] + 2 * top[1] - 2 * orig[1] + topright[1] - right[1])
                    + Math.Abs(-3 * left[1] - topleft[1] + 3 * right[1] + topright[1]);

                r = Math.Abs(topleft[2] - left[2] + 2 * top[2] - 2 * orig[2] + topright[2] - right[2])
                    + Math.Abs(-3 * left[2] - topleft[2] + 3 * right[2] + topright[2]);

                dest[0] = (byte)(b > 255 ? 255 : b);
                dest[1] = (byte)(g > 255 ? 255 : g);
                dest[2] = (byte)(r > 255 ? 255 : r);
            }

            //left/right margins
            for (int y = 1; y < height - 1; y++)
            {
                byte* orig = dataPtrCopy + y * widthstep;
                byte* dest = dataPtr + y * widthstep;

                byte* top = orig - widthstep;
                byte* topright = orig - widthstep + nC;
                byte* right = orig + nC;
                byte* bottom = orig + widthstep;
                byte* bttmright = orig + widthstep + nC;

                int b = Math.Abs(3 * top[0] - 3 * bottom[0] + topright[0] - bttmright[0])
                    + Math.Abs(-top[0] - 2 * orig[0] - bottom[0] + topright[0] + 2 * right[0] + bttmright[0]);

                int g = Math.Abs(3 * top[1] - 3 * bottom[1] + topright[1] - bttmright[1])
                    + Math.Abs(-top[1] - 2 * orig[1] - bottom[1] + topright[1] + 2 * right[1] + bttmright[1]);

                int r = Math.Abs(3 * top[2] - 3 * bottom[2] + topright[2] - (orig + nC + widthstep)[2])
                    + Math.Abs(-top[2] - 2 * orig[2] - bottom[2] + topright[2] + 2 * right[2] + bttmright[2]);

                dest[0] = (byte)(b > 255 ? 255 : b);
                dest[1] = (byte)(g > 255 ? 255 : g);
                dest[2] = (byte)(r > 255 ? 255 : r);

                orig = dataPtrCopy + y * widthstep + (width - 1) * nC;
                dest = dataPtr + y * widthstep + (width - 1) * nC;

                byte* topleft = orig - widthstep - nC;
                top = orig - widthstep;
                byte* left = orig - nC;
                byte* bttmleft = orig + widthstep - nC;
                bottom = orig + widthstep;

                b = Math.Abs(3 * top[0] - 3 * bottom[0] + topleft[0] - bttmleft[0])
                    + Math.Abs(-topleft[0] - 2 * left[0] - bttmleft[0] + top[0] + 2 * orig[0] + bottom[0]);

                g = Math.Abs(3 * top[1] - 3 * bottom[1] + topleft[1] - bttmleft[1])
                    + Math.Abs(-topleft[1] - 2 * left[1] - bttmleft[1] + top[1] + 2 * orig[1] + bottom[1]);

                r = Math.Abs(3 * top[2] - 3 * bottom[2] + topleft[2] - bttmleft[2])
                    + Math.Abs(-topleft[2] - 2 * left[2] - bttmleft[2] + top[2] + 2 * orig[2] + bottom[2]);

                dest[0] = (byte)(b > 255 ? 255 : b);
                dest[1] = (byte)(g > 255 ? 255 : g);
                dest[2] = (byte)(r > 255 ? 255 : r);
            }

            //topleft/bottomleft/topright/bottomright corners
            {
                byte* orig = dataPtrCopy;
                byte* dest = dataPtr;

                int b = Math.Abs(3 * orig[0] - 3 * (orig + widthstep)[0] + (orig + nC)[0] - (orig + nC + widthstep)[0])
                    + Math.Abs(-3 * orig[0] - (orig + widthstep)[0] + 3 * (orig + nC)[0] + (orig + nC + widthstep)[0]);

                int g = Math.Abs(3 * orig[1] - 3 * (orig + widthstep)[1] + (orig + nC)[1] - (orig + nC + widthstep)[1])
                    + Math.Abs(-3 * orig[1] - (orig + widthstep)[1] + 3 * (orig + nC)[1] + (orig + nC + widthstep)[1]);

                int r = Math.Abs(3 * orig[2] - 3 * (orig + widthstep)[2] + (orig + nC)[2] - (orig + nC + widthstep)[2])
                    + Math.Abs(-3 * orig[2] - (orig + widthstep)[2] + 3 * (orig + nC)[2] + (orig + nC + widthstep)[2]);

                dest[0] = (byte)(b > 255 ? 255 : b);
                dest[1] = (byte)(g > 255 ? 255 : g);
                dest[2] = (byte)(r > 255 ? 255 : r);

                orig = dataPtrCopy + (height - 1) * widthstep;
                dest = dataPtr + (height - 1) * widthstep;

                b = Math.Abs(3 * (orig - widthstep)[0] - 3 * (orig)[0] + (orig + nC - widthstep)[0] - (orig + nC)[0])
                    + Math.Abs(-3 * (orig)[0] - (orig - widthstep)[0] + 3 * (orig + nC)[0] + (orig + nC - widthstep)[0]);

                g = Math.Abs(3 * (orig - widthstep)[1] - 3 * orig[1] + (orig + nC - widthstep)[1] - (orig + nC)[1])
                    + Math.Abs(-3 * orig[1] - (orig - widthstep)[1] + 3 * (orig + nC)[1] + (orig + nC - widthstep)[1]);

                r = Math.Abs(3 * (orig - widthstep)[2] - 3 * orig[2] + (orig + nC - widthstep)[2] - (orig + nC)[2])
                    + Math.Abs(-3 * orig[2] - (orig - widthstep)[2] + 3 * (orig + nC)[2] + (orig + nC - widthstep)[2]);

                dest[0] = (byte)(b > 255 ? 255 : b);
                dest[1] = (byte)(g > 255 ? 255 : g);
                dest[2] = (byte)(r > 255 ? 255 : r);

                orig = dataPtrCopy + (width - 1) * nC;
                dest = dataPtr + (width - 1) * nC;

                b = Math.Abs(3 * orig[0] - 3 * (orig + widthstep)[0] + (orig - nC)[0] - (orig - nC + widthstep)[0])
                    + Math.Abs(-3 * (orig - nC)[0] - (orig - nC + widthstep)[0] + 3 * orig[0] + (orig + widthstep)[0]);

                g = Math.Abs(3 * orig[1] - 3 * (orig + widthstep)[1] + (orig - nC)[1] - (orig - nC + widthstep)[1])
                    + Math.Abs(-3 * (orig - nC)[1] - (orig - nC + widthstep)[1] + 3 * orig[1] + (orig + widthstep)[1]);

                r = Math.Abs(3 * orig[2] - 3 * (orig + widthstep)[2] + (orig - nC)[2] - (orig - nC + widthstep)[2])
                    + Math.Abs(-3 * (orig - nC)[2] - (orig - nC + widthstep)[2] + 3 * orig[2] + (orig + widthstep)[2]);

                dest[0] = (byte)(b > 255 ? 255 : b);
                dest[1] = (byte)(g > 255 ? 255 : g);
                dest[2] = (byte)(r > 255 ? 255 : r);

                orig = dataPtrCopy + (height - 1) * widthstep + (width - 1) * nC;
                dest = dataPtr + (height - 1) * widthstep + (width - 1) * nC;

                b = Math.Abs(3 * (orig - widthstep)[0] - 3 * orig[0] + (orig - nC - widthstep)[0] - (orig - nC)[0])
                    + Math.Abs(-(orig - nC - widthstep)[0] - 3 * (orig - nC)[0] + (orig - widthstep)[0] + 3 * orig[0]);

                g = Math.Abs(3 * (orig - widthstep)[1] - 3 * orig[1] + (orig - nC - widthstep)[1] - (orig - nC)[1])
                    + Math.Abs(-(orig - nC - widthstep)[1] - 3 * (orig - nC)[1] + (orig - widthstep)[1] + 3 * orig[1]);

                r = Math.Abs(3 * (orig - widthstep)[2] - 3 * orig[2] + (orig - nC - widthstep)[2] - (orig - nC)[2])
                    + Math.Abs(-(orig - nC - widthstep)[2] - 3 * (orig - nC)[2] + (orig - widthstep)[2] + 3 * orig[2]);

                dest[0] = (byte)(b > 255 ? 255 : b);
                dest[1] = (byte)(g > 255 ? 255 : g);
                dest[2] = (byte)(r > 255 ? 255 : r);
            }
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
                    byte* rightPt = dataPtrCopy + y * widthstep + (x + 1) * nC; //the pixel to the right
                    byte* downPt = dataPtrCopy + (y + 1) * widthstep + x * nC; //the pixel below
                    int bsum = Math.Abs(currPt[0] - rightPt[0]) + Math.Abs(currPt[0] - downPt[0]);
                    int gsum = Math.Abs(currPt[1] - rightPt[1]) + Math.Abs(currPt[1] - downPt[1]);
                    int rsum = Math.Abs(currPt[2] - rightPt[2]) + Math.Abs(currPt[2] - downPt[2]);
                    dest[0] = (byte)(bsum > 255 ? 255 : bsum);
                    dest[1] = (byte)(gsum > 255 ? 255 : gsum);
                    dest[2] = (byte)(rsum > 255 ? 255 : rsum);
                }
            }

            //calculates the filter's value for the right margin
            for (int y = 0; y < height - 1; y++)
            {
                byte* dest = dataPtr + y * widthstep + (width - 1) * nC;
                byte* currPt = dataPtrCopy + y * widthstep + (width - 1) * nC;
                //no pixel to the right (margin duplication, difference would be zero)
                byte* downPt = dataPtrCopy + (y + 1) * widthstep + (width - 1) * nC;
                int bsum = Math.Abs(currPt[0] - downPt[0]);
                int gsum = Math.Abs(currPt[1] - downPt[1]);
                int rsum = Math.Abs(currPt[2] - downPt[2]);
                dest[0] = (byte)(bsum > 255 ? 255 : bsum);
                dest[1] = (byte)(gsum > 255 ? 255 : gsum);
                dest[2] = (byte)(rsum > 255 ? 255 : rsum);
            }

            //calculates the filter's value for the bottom margin
            for (int x = 0; x < width - 1; x++)
            {
                byte* dest = dataPtr + (height - 1) * widthstep + x * nC;
                byte* currPt = dataPtrCopy + (height - 1) * widthstep + x * nC;
                //no pixel below (margin duplication, difference would be zero)
                byte* rightPt = dataPtrCopy + (height - 1) * widthstep + (x + 1) * nC;
                int bsum = Math.Abs(currPt[0] - rightPt[0]);
                int gsum = Math.Abs(currPt[1] - rightPt[1]);
                int rsum = Math.Abs(currPt[2] - rightPt[2]);
                dest[0] = (byte)(bsum > 255 ? 255 : bsum);
                dest[1] = (byte)(gsum > 255 ? 255 : gsum);
                dest[2] = (byte)(rsum > 255 ? 255 : rsum);
            }

            //bottom-right corner has no one to its right or below, so it becomes black (margin duplication, differences would be zero)
            dataPtr = dataPtr + (height - 1) * widthstep + (width - 1) * nC;
            dataPtr[0] = 0;
            dataPtr[1] = 0;
            dataPtr[2] = 0;
        }

        /// <summary>
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
            float i1 = mat[0, 0];
            float i2 = mat[1, 0];
            float i3 = mat[2, 0];
            float i4 = mat[0, 1];
            float i5 = mat[1, 1];
            float i6 = mat[2, 1];
            float i7 = mat[0, 2];
            float i8 = mat[1, 2];
            float i9 = mat[2, 2];

            float d0, d1, d2;

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
                    byte* orig = dataPtrCopy + y * widthstep + x * nC;

                    byte* i1pix = orig - widthstep - nC;
                    byte* i2pix = orig - widthstep;
                    byte* i3pix = orig - widthstep + nC;
                    byte* i4pix = orig - nC;
                    byte* i6pix = orig + nC;
                    byte* i7pix = orig + widthstep - nC;
                    byte* i8pix = orig + widthstep;
                    byte* i9pix = orig + widthstep + nC;

                    d0 = (i1 * i1pix[0] + i2 * i2pix[0] + i3 * i3pix[0] + i4 * i4pix[0] + i5 * orig[0] + i6 * i6pix[0] +
                         i7 * i7pix[0] + i8 * i8pix[0] + i9 * i9pix[0]) / weight;
                    d1 = (i1 * i1pix[1] + i2 * i2pix[1] + i3 * i3pix[1] + i4 * i4pix[1] + i5 * orig[1] + i6 * i6pix[1] +
                         i7 * i7pix[1] + i8 * i8pix[1] + i9 * i9pix[1]) / weight;
                    d2 = (i1 * i1pix[2] + i2 * i2pix[2] + i3 * i3pix[2] + i4 * i4pix[2] + i5 * orig[2] + i6 * i6pix[2] +
                         i7 * i7pix[2] + i8 * i8pix[2] + i9 * i9pix[2]) / weight;

                    dest[0] = (byte)(d0 > 255 ? 255 : Math.Round(Math.Abs(d0)));
                    dest[1] = (byte)(d1 > 255 ? 255 : Math.Round(Math.Abs(d1)));
                    dest[2] = (byte)(d2 > 255 ? 255 : Math.Round(Math.Abs(d2)));
                }
            }

            //processing corners and margins
            //top-left corner
            d0 = (((i1 + i2 + i4 + i5) * dataPtrCopy[0] + (i8 + i7) * (dataPtrCopy + widthstep)[0] + (i6 + i3) * (dataPtrCopy + nC)[0] + i9 * (dataPtrCopy + widthstep + nC)[0]) / weight);
            d1 = (((i1 + i2 + i4 + i5) * dataPtrCopy[1] + (i8 + i7) * (dataPtrCopy + widthstep)[1] + (i6 + i3) * (dataPtrCopy + nC)[1] + i9 * (dataPtrCopy + widthstep + nC)[1]) / weight);
            d2 = (((i1 + i2 + i4 + i5) * dataPtrCopy[2] + (i8 + i7) * (dataPtrCopy + widthstep)[2] + (i6 + i3) * (dataPtrCopy + nC)[2] + i9 * (dataPtrCopy + widthstep + nC)[2]) / weight);
            dataPtr[0] = (byte)(d0 > 255 ? 255 : Math.Round(Math.Abs(d0)));
            dataPtr[1] = (byte)(d1 > 255 ? 255 : Math.Round(Math.Abs(d1)));
            dataPtr[2] = (byte)(d2 > 255 ? 255 : Math.Round(Math.Abs(d2)));

            //top-right corner
            byte* corner = dataPtr + (width - 1) * nC;
            byte* cornerCopy = dataPtrCopy + (width - 1) * nC;

            d0 = (((i3 + i6 + i2 + i5) * cornerCopy[0] + (i8 + i9) * (cornerCopy + widthstep)[0] + (i4 + i1) * (cornerCopy - nC)[0] + i7 * (cornerCopy + widthstep - nC)[0]) / weight);
            d1 = (((i3 + i6 + i2 + i5) * cornerCopy[1] + (i8 + i9) * (cornerCopy + widthstep)[1] + (i4 + i1) * (cornerCopy - nC)[1] + i7 * (cornerCopy + widthstep - nC)[1]) / weight);
            d2 = (((i3 + i6 + i2 + i5) * cornerCopy[2] + (i8 + i9) * (cornerCopy + widthstep)[2] + (i4 + i1) * (cornerCopy - nC)[2] + i7 * (cornerCopy + widthstep - nC)[2]) / weight);

            corner[0] = (byte)(d0 > 255 ? 255 : Math.Round(Math.Abs(d0)));
            corner[1] = (byte)(d1 > 255 ? 255 : Math.Round(Math.Abs(d1)));
            corner[2] = (byte)(d2 > 255 ? 255 : Math.Round(Math.Abs(d2)));

            //bottom-left corner
            corner = dataPtr + (height - 1) * widthstep;
            cornerCopy = dataPtrCopy + (height - 1) * widthstep;

            d0 = (((i5 + i4 + i8 + i7) * cornerCopy[0] + (i2 + i1) * (cornerCopy - widthstep)[0] + (i6 + i9) * (cornerCopy + nC)[0] + i3 * (cornerCopy - widthstep + nC)[0]) / weight);
            d1 = (((i5 + i4 + i8 + i7) * cornerCopy[1] + (i2 + i1) * (cornerCopy - widthstep)[1] + (i6 + i9) * (cornerCopy + nC)[1] + i3 * (cornerCopy - widthstep + nC)[1]) / weight);
            d2 = (((i5 + i4 + i8 + i7) * cornerCopy[2] + (i2 + i1) * (cornerCopy - widthstep)[2] + (i6 + i9) * (cornerCopy + nC)[2] + i3 * (cornerCopy - widthstep + nC)[2]) / weight);

            corner[0] = (byte)(d0 > 255 ? 255 : Math.Round(Math.Abs(d0)));
            corner[1] = (byte)(d1 > 255 ? 255 : Math.Round(Math.Abs(d1)));
            corner[2] = (byte)(d2 > 255 ? 255 : Math.Round(Math.Abs(d2)));

            //bottom-right corner
            corner = dataPtr + (height - 1) * widthstep + (width - 1) * nC;
            cornerCopy = dataPtrCopy + (height - 1) * widthstep + (width - 1) * nC;

            d0 = (((i5 + i6 + i8 + i9) * cornerCopy[0] + (i2 + i3) * (cornerCopy - widthstep)[0] + (i4 + i7) * (cornerCopy - nC)[0] + i1 * (cornerCopy - widthstep - nC)[0]) / weight);
            d1 = (((i5 + i6 + i8 + i9) * cornerCopy[1] + (i2 + i3) * (cornerCopy - widthstep)[1] + (i4 + i7) * (cornerCopy - nC)[1] + i1 * (cornerCopy - widthstep - nC)[1]) / weight);
            d2 = (((i5 + i6 + i8 + i9) * cornerCopy[2] + (i2 + i3) * (cornerCopy - widthstep)[2] + (i4 + i7) * (cornerCopy - nC)[2] + i1 * (cornerCopy - widthstep - nC)[2]) / weight);

            corner[0] = (byte)(d0 > 255 ? 255 : Math.Round(Math.Abs(d0)));
            corner[1] = (byte)(d1 > 255 ? 255 : Math.Round(Math.Abs(d1)));
            corner[2] = (byte)(d2 > 255 ? 255 : Math.Round(Math.Abs(d2)));

            for (int i = 1; i < width - 1; i++)
            {
                //top margin
                byte* margemSup = dataPtr + i * nC;
                byte* margemSupCop = dataPtrCopy + i * nC;

                byte* mSupLeft = margemSupCop - nC;
                byte* mSupRight = margemSupCop + nC;

                byte* margemSupDown = margemSupCop + widthstep;
                byte* mSupRightDown = mSupRight + widthstep;
                byte* mLeftSupDown = mSupLeft + widthstep;

                d0 = (((i5 + i2) * margemSupCop[0] + (i4 + i1) * mSupLeft[0] + (i6 + i3) * mSupRight[0] + i8 * margemSupDown[0] + i9 * mSupRightDown[0] + i7 * mLeftSupDown[0]) / weight);
                d1 = (((i5 + i2) * margemSupCop[1] + (i4 + i1) * mSupLeft[1] + (i6 + i3) * mSupRight[1] + i8 * margemSupDown[1] + i9 * mSupRightDown[1] + i7 * mLeftSupDown[1]) / weight);
                d2 = (((i5 + i2) * margemSupCop[2] + (i4 + i1) * mSupLeft[2] + (i6 + i3) * mSupRight[2] + i8 * margemSupDown[2] + i9 * mSupRightDown[2] + i7 * mLeftSupDown[2]) / weight);

                margemSup[0] = (byte)(d0 > 255 ? 255 : Math.Round(Math.Abs(d0)));
                margemSup[1] = (byte)(d1 > 255 ? 255 : Math.Round(Math.Abs(d1)));
                margemSup[2] = (byte)(d2 > 255 ? 255 : Math.Round(Math.Abs(d2)));

                //bottom margin
                byte* margemInf = dataPtr + (height - 1) * widthstep + i * nC;
                byte* margemInfCop = dataPtrCopy + (height - 1) * widthstep + i * nC;

                byte* mInfLeft = margemInfCop - nC;
                byte* mInfRight = margemInfCop + nC;

                byte* mInfUp = margemInfCop - widthstep;
                byte* mInfRightUp = mInfRight - widthstep;
                byte* mInfLeftUp = mInfLeft - widthstep;

                d0 = (((i5 + i8) * margemInfCop[0] + (i4 + i7) * mInfLeft[0] + (i6 + i9) * mInfRight[0] + i2 * mInfUp[0] + i3 * mInfRightUp[0] + i1 * mInfLeftUp[0]) / weight);
                d1 = (((i5 + i8) * margemInfCop[1] + (i4 + i7) * mInfLeft[1] + (i6 + i9) * mInfRight[1] + i2 * mInfUp[1] + i3 * mInfRightUp[1] + i1 * mInfLeftUp[1]) / weight);
                d2 = (((i5 + i8) * margemInfCop[2] + (i4 + i7) * mInfLeft[2] + (i6 + i9) * mInfRight[2] + i2 * mInfUp[2] + i3 * mInfRightUp[2] + i1 * mInfLeftUp[2]) / weight);

                margemInf[0] = (byte)(d0 > 255 ? 255 : Math.Round(Math.Abs(d0)));
                margemInf[1] = (byte)(d1 > 255 ? 255 : Math.Round(Math.Abs(d1)));
                margemInf[2] = (byte)(d2 > 255 ? 255 : Math.Round(Math.Abs(d2)));
            }

            for (int i = 1; i < height - 1; i++)
            {
                //left margin
                byte* margemEsq = dataPtr + i * widthstep;
                byte* margemEsqCop = dataPtrCopy + i * widthstep;

                byte* mEsqUp = margemEsqCop - widthstep;
                byte* mEsqDown = margemEsqCop + widthstep;

                byte* mEsqRight = margemEsqCop + nC;
                byte* mEsqUpRight = mEsqUp + nC;
                byte* mEsqDownRight = mEsqDown + nC;

                d0 = (((i5 + i4) * margemEsqCop[0] + (i2 + i1) * mEsqUp[0] + (i8 + i7) * mEsqDown[0] + i6 * mEsqRight[0] + i9 * mEsqDownRight[0] + i3 * mEsqUpRight[0]) / weight);
                d1 = (((i5 + i4) * margemEsqCop[1] + (i2 + i1) * mEsqUp[1] + (i8 + i7) * mEsqDown[1] + i6 * mEsqRight[1] + i9 * mEsqDownRight[1] + i3 * mEsqUpRight[1]) / weight);
                d2 = (((i5 + i4) * margemEsqCop[2] + (i2 + i1) * mEsqUp[2] + (i8 + i7) * mEsqDown[2] + i6 * mEsqRight[2] + i9 * mEsqDownRight[2] + i3 * mEsqUpRight[2]) / weight);

                margemEsq[0] = (byte)(d0 > 255 ? 255 : Math.Round(Math.Abs(d0)));
                margemEsq[1] = (byte)(d1 > 255 ? 255 : Math.Round(Math.Abs(d1)));
                margemEsq[2] = (byte)(d2 > 255 ? 255 : Math.Round(Math.Abs(d2)));

                //right margin
                byte* margemDir = dataPtr + (width - 1) * nC + i * widthstep;
                byte* margemDirCop = dataPtrCopy + (width - 1) * nC + i * widthstep;

                byte* mDirUp = margemDirCop - widthstep;
                byte* mDirDown = margemDirCop + widthstep;

                byte* mDirLeft = margemDirCop - nC;
                byte* mDirUpLeft = mDirUp - nC;
                byte* mDirDownLeft = mDirDown - nC;

                d0 = (((i5 + i6) * margemDirCop[0] + (i2 + i3) * mDirUp[0] + (i8 + i9) * mDirDown[0] + i4 * mDirLeft[0] + i7 * mDirDownLeft[0] + i1 * mDirUpLeft[0]) / weight);
                d1 = (((i5 + i6) * margemDirCop[1] + (i2 + i3) * mDirUp[1] + (i8 + i9) * mDirDown[1] + i4 * mDirLeft[1] + i7 * mDirDownLeft[1] + i1 * mDirUpLeft[1]) / weight);
                d2 = (((i5 + i6) * margemDirCop[2] + (i2 + i3) * mDirUp[2] + (i8 + i9) * mDirDown[2] + i4 * mDirLeft[2] + i7 * mDirDownLeft[2] + i1 * mDirUpLeft[2]) / weight);

                margemDir[0] = (byte)(d0 > 255 ? 255 : Math.Round(Math.Abs(d0)));
                margemDir[1] = (byte)(d1 > 255 ? 255 : Math.Round(Math.Abs(d1)));
                margemDir[2] = (byte)(d2 > 255 ? 255 : Math.Round(Math.Abs(d2)));
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
                    byte* rightPt = dataPtrCopy + y * widthstep + (x + 1) * nC; //the pixel to the right
                    byte* rightdownPt = dataPtrCopy + (y + 1) * widthstep + (x + 1) * nC; //the pixel to the right and below
                    byte* downPt = dataPtrCopy + (y + 1) * widthstep + x * nC; //the pixel below
                    int bsum = Math.Abs(currPt[0] - rightdownPt[0]) + Math.Abs(rightPt[0] - downPt[0]);
                    int gsum = Math.Abs(currPt[1] - rightdownPt[1]) + Math.Abs(rightPt[1] - downPt[1]);
                    int rsum = Math.Abs(currPt[2] - rightdownPt[2]) + Math.Abs(rightPt[2] - downPt[2]);
                    dest[0] = (byte)(bsum > 255 ? 255 : bsum);
                    dest[1] = (byte)(gsum > 255 ? 255 : gsum);
                    dest[2] = (byte)(rsum > 255 ? 255 : rsum);
                }
            }

            //calculates the filter's value for the right margin
            for (int y = 0; y < height - 1; y++)
            {
                byte* dest = dataPtr + y * widthstep + (width - 1) * nC;
                byte* currPt = dataPtrCopy + y * widthstep + (width - 1) * nC;
                //no pixels to the right (margin duplication)
                byte* downPt = dataPtrCopy + (y + 1) * widthstep + (width - 1) * nC;
                int bsum = 2 * Math.Abs(currPt[0] - downPt[0]);
                int gsum = 2 * Math.Abs(currPt[1] - downPt[1]);
                int rsum = 2 * Math.Abs(currPt[2] - downPt[2]);
                dest[0] = (byte)(bsum > 255 ? 255 : bsum);
                dest[1] = (byte)(gsum > 255 ? 255 : gsum);
                dest[2] = (byte)(rsum > 255 ? 255 : rsum);
            }

            //calculates the filter's value for the bottom margin
            for (int x = 0; x < width - 1; x++)
            {
                byte* dest = dataPtr + (height - 1) * widthstep + x * nC;
                byte* currPt = dataPtrCopy + (height - 1) * widthstep + x * nC;
                //no pixels below (margin duplication)
                byte* rightPt = dataPtrCopy + (height - 1) * widthstep + (x + 1) * nC;
                int bsum = 2 * Math.Abs(currPt[0] - rightPt[0]);
                int gsum = 2 * Math.Abs(currPt[1] - rightPt[1]);
                int rsum = 2 * Math.Abs(currPt[2] - rightPt[2]);
                dest[0] = (byte)(bsum > 255 ? 255 : bsum);
                dest[1] = (byte)(gsum > 255 ? 255 : gsum);
                dest[2] = (byte)(rsum > 255 ? 255 : rsum);
            }

            //bottom-right corner has no one to its right or below, so it becomes black (margin duplication, differences would be zero)
            dataPtr = dataPtr + (height - 1) * widthstep + (width - 1) * nC;
            dataPtr[0] = 0;
            dataPtr[1] = 0;
            dataPtr[2] = 0;
        }

        /// <summary>
        /// *** NOT PERFECT ***
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

            string path = "../../Resources/characters";
            Dictionary<char, Image<Bgr, byte>> charList = new Dictionary<char, Image<Bgr, byte>>();
            foreach (string file in Directory.EnumerateFiles(path))
            {
                char c = file.ElementAt(file.Length - 5);
                Image<Bgr, byte> i = new Image<Bgr, byte>(file);
                ConvertToBW_Otsu(i);
                charList.Add(c, i);
            }

            LP_Location = Locate_License_Plate(img);

            Locate_LP_Chars(img.Copy(), LP_Location, out LP_Chr1, out LP_Chr2, out LP_Chr3, out LP_Chr4, out LP_Chr5, out LP_Chr6);

            LP_C1 = CharacterRecognition(imgCopy, LP_Chr1, charList);
            LP_C2 = CharacterRecognition(imgCopy, LP_Chr2, charList);
            LP_C3 = CharacterRecognition(imgCopy, LP_Chr3, charList);
            LP_C4 = CharacterRecognition(imgCopy, LP_Chr4, charList);
            LP_C5 = CharacterRecognition(imgCopy, LP_Chr5, charList);
            LP_C6 = CharacterRecognition(imgCopy, LP_Chr6, charList);
            LP_Country = "";
            LP_Month = "";
            LP_Year = "";

            //debug only (draw lines)
            DebugDrawLines(img, LP_Location, LP_Chr1, LP_Chr2, LP_Chr3, LP_Chr4, LP_Chr5, LP_Chr6);
        }

        unsafe private static void DebugDrawLines(Image<Bgr, byte> img, Rectangle LP_Location, Rectangle LP_C1, Rectangle LP_C2, Rectangle LP_C3, Rectangle LP_C4, Rectangle LP_C5, Rectangle LP_C6)
        {
            MIplImage m = img.MIplImage;
            byte* imgPtr = (byte*)m.imageData.ToPointer();
            int widthstep = m.widthStep;
            int nC = m.nChannels;
            int width = m.width;
            int height = m.height;

            //LP_Location
            for (int y = LP_Location.Top; y < LP_Location.Bottom; y++)
            {
                byte* pixelPtr;
                pixelPtr = imgPtr + y * widthstep + LP_Location.Left * nC;
                pixelPtr[1] = 255;
                pixelPtr = imgPtr + y * widthstep + (LP_Location.Right - 1) * nC;
                pixelPtr[1] = 255;
            }
            for (int x = LP_Location.Left; x < LP_Location.Right; x++)
            {
                byte* pixelPtr;
                pixelPtr = imgPtr + LP_Location.Top * widthstep + x * nC;
                pixelPtr[1] = 255;
                pixelPtr = imgPtr + (LP_Location.Bottom - 1) * widthstep + x * nC;
                pixelPtr[1] = 255;
            }
            //LP_C1
            for (int y = LP_C1.Top; y < LP_C1.Bottom; y++)
            {
                byte* pixelPtr;
                pixelPtr = imgPtr + y * widthstep + LP_C1.Left * nC;
                pixelPtr[1] = 255;
                pixelPtr = imgPtr + y * widthstep + (LP_C1.Right - 1) * nC;
                pixelPtr[1] = 255;
            }
            for (int x = LP_C1.Left; x < LP_C1.Right; x++)
            {
                byte* pixelPtr;
                pixelPtr = imgPtr + LP_C1.Top * widthstep + x * nC;
                pixelPtr[1] = 255;
                pixelPtr = imgPtr + (LP_C1.Bottom - 1) * widthstep + x * nC;
                pixelPtr[1] = 255;
            }
            //LP_C2
            for (int y = LP_C2.Top; y < LP_C2.Bottom; y++)
            {
                byte* pixelPtr;
                pixelPtr = imgPtr + y * widthstep + LP_C2.Left * nC;
                pixelPtr[1] = 255;
                pixelPtr = imgPtr + y * widthstep + (LP_C2.Right - 1) * nC;
                pixelPtr[1] = 255;
            }
            for (int x = LP_C2.Left; x < LP_C2.Right; x++)
            {
                byte* pixelPtr;
                pixelPtr = imgPtr + LP_C2.Top * widthstep + x * nC;
                pixelPtr[1] = 255;
                pixelPtr = imgPtr + (LP_C2.Bottom - 1) * widthstep + x * nC;
                pixelPtr[1] = 255;
            }
            //LP_C3
            for (int y = LP_C3.Top; y < LP_C3.Bottom; y++)
            {
                byte* pixelPtr;
                pixelPtr = imgPtr + y * widthstep + LP_C3.Left * nC;
                pixelPtr[1] = 255;
                pixelPtr = imgPtr + y * widthstep + (LP_C3.Right - 1) * nC;
                pixelPtr[1] = 255;
            }
            for (int x = LP_C3.Left; x < LP_C3.Right; x++)
            {
                byte* pixelPtr;
                pixelPtr = imgPtr + LP_C3.Top * widthstep + x * nC;
                pixelPtr[1] = 255;
                pixelPtr = imgPtr + (LP_C3.Bottom - 1) * widthstep + x * nC;
                pixelPtr[1] = 255;
            }
            //LP_C4
            for (int y = LP_C4.Top; y < LP_C4.Bottom; y++)
            {
                byte* pixelPtr;
                pixelPtr = imgPtr + y * widthstep + LP_C4.Left * nC;
                pixelPtr[1] = 255;
                pixelPtr = imgPtr + y * widthstep + (LP_C4.Right - 1) * nC;
                pixelPtr[1] = 255;
            }
            for (int x = LP_C4.Left; x < LP_C4.Right; x++)
            {
                byte* pixelPtr;
                pixelPtr = imgPtr + LP_C4.Top * widthstep + x * nC;
                pixelPtr[1] = 255;
                pixelPtr = imgPtr + (LP_C4.Bottom - 1) * widthstep + x * nC;
                pixelPtr[1] = 255;
            }
            //LP_C5
            for (int y = LP_C5.Top; y < LP_C5.Bottom; y++)
            {
                byte* pixelPtr;
                pixelPtr = imgPtr + y * widthstep + LP_C5.Left * nC;
                pixelPtr[1] = 255;
                pixelPtr = imgPtr + y * widthstep + (LP_C5.Right - 1) * nC;
                pixelPtr[1] = 255;
            }
            for (int x = LP_C5.Left; x < LP_C5.Right; x++)
            {
                byte* pixelPtr;
                pixelPtr = imgPtr + LP_C5.Top * widthstep + x * nC;
                pixelPtr[1] = 255;
                pixelPtr = imgPtr + (LP_C5.Bottom - 1) * widthstep + x * nC;
                pixelPtr[1] = 255;
            }
            //LP_C6
            for (int y = LP_C6.Top; y < LP_C6.Bottom; y++)
            {
                byte* pixelPtr;
                pixelPtr = imgPtr + y * widthstep + LP_C6.Left * nC;
                pixelPtr[1] = 255;
                pixelPtr = imgPtr + y * widthstep + (LP_C6.Right - 1) * nC;
                pixelPtr[1] = 255;
            }
            for (int x = LP_C6.Left; x < LP_C6.Right; x++)
            {
                byte* pixelPtr;
                pixelPtr = imgPtr + LP_C6.Top * widthstep + x * nC;
                pixelPtr[1] = 255;
                pixelPtr = imgPtr + (LP_C6.Bottom - 1) * widthstep + x * nC;
                pixelPtr[1] = 255;
            }
        }

        /// <summary>
        /// *** Not great ***
        /// Locates a license plate
        /// </summary>
        /// <param name="img">The image</param>
        /// <returns>The location of the license plate</returns>
        unsafe private static Rectangle Locate_License_Plate(Image<Bgr, byte> img)
        {
            int upperLimit, lowerLimit, leftLimit, rightLimit;
            LocateLPVertical(img.Copy(), out upperLimit, out lowerLimit); //get upper & lower limits
            LocateLPLeft(img.Copy(), upperLimit, lowerLimit, out leftLimit); //get left limit
            LocateLPRight(img.Copy(), upperLimit, lowerLimit, leftLimit, out rightLimit); //get right limit

            return new Rectangle(leftLimit, upperLimit, (rightLimit - leftLimit), (lowerLimit - upperLimit));
        }

        /// <summary>
        /// Locate upper and lower limits of the license plate
        /// </summary>
        /// <param name="img">The image</param>
        /// <param name="upperLimit">License plate upper limit</param>
        /// <param name="lowerLimit">License plate lower limit</param>
        unsafe private static void LocateLPVertical(Image<Bgr, byte> img, out int upperLimit, out int lowerLimit)
        {
            //Preprocess image
            float[,] gauss = { { 1, 2, 1 }, { 2, 4, 2 }, { 1, 2, 1 } };
            NonUniform(img, img.Copy(), gauss, 16); //noise reduction
            NonUniform(img, img.Copy(), gauss, 16); //noise reduction
            NonUniform(img, img.Copy(), gauss, 16); //noise reduction
            AvgChannel(img); //convert to grayscale
            Roberts(img, img.Copy()); //edge detection
            ConvertToBW(img, 50); //binarization

            MIplImage m = img.MIplImage;
            byte* imgPtr = (byte*)m.imageData.ToPointer();
            int widthstep = m.widthStep;
            int nC = m.nChannels;
            int width = m.width;
            int height = m.height;

            //Determine row with highest amplitude
            int[] sfr = new int[height];
            int maxRow = 0;
            for (int y = 0; y < height; y++)
            {
                sfr[y] = 0;
                for (int x = 2; x < width - 2; x++)
                {
                    byte* pixelPtr = imgPtr + y * widthstep + x * nC;
                    byte* pixelPtrPrev = imgPtr + y * widthstep + (x - 1) * nC;
                    if ((pixelPtrPrev[0] > 128 && pixelPtr[0] < 128) || (pixelPtrPrev[0] < 128 && pixelPtr[0] > 128))
                    {
                        sfr[y]++; //color switched. edge found
                    }
                }
                if (sfr[y] > sfr[maxRow])
                {
                    maxRow = y; //new max found
                }
            }

            int halfMaxValue = sfr[maxRow] / 2; //half of the maximal value
            //Search upper limit
            upperLimit = 0;
            for (int y = maxRow; y >= 0; y--)
            {
                if (sfr[y] == halfMaxValue || (sfr[y] > halfMaxValue - (halfMaxValue / 2)) && (sfr[y] < halfMaxValue))
                {
                    upperLimit = y - 3; //upper limit found
                    break;
                }
            }
            //Search lower limit
            lowerLimit = height - 1;
            for (int y = maxRow; y < height; y++)
            {
                if (sfr[y] == halfMaxValue || (sfr[y] > halfMaxValue - (halfMaxValue / 2)) && (sfr[y] < halfMaxValue))
                {
                    lowerLimit = y + 3; //lower limit found
                    break;
                }
            }
        }

        /// <summary>
        /// Locate left limit of the license plate
        /// </summary>
        /// <param name="img">The image</param>
        /// <param name="upperLimit">License plate upper limit</param>
        /// <param name="lowerLimit">License plate lower limit</param>
        /// <param name="leftLimit">License plate left limit</param>
        unsafe private static void LocateLPLeft(Image<Bgr, byte> img, int upperLimit, int lowerLimit, out int leftLimit)
        {
            //Preprocess image
            EnhanceBlue(img);

            MIplImage m = img.MIplImage;
            byte* imgPtr = (byte*)m.imageData.ToPointer();
            int widthstep = m.widthStep;
            int nC = m.nChannels;
            int width = m.width;
            int height = m.height;

            //Determine column with highest amplitude
            int[] sfc = new int[width];
            int maxColumn = 0;
            for (int x = 0; x < width; x++)
            {
                sfc[x] = 0;
                for (int y = upperLimit; y < lowerLimit; y++)
                {
                    byte* pixelPtr = imgPtr + y * widthstep + x * nC;
                    if (pixelPtr[0] == 255)
                    {
                        sfc[x]++;
                    }
                }
                if (sfc[x] > sfc[maxColumn])
                {
                    maxColumn = x; //new max found
                }
            }

            //Search left limit
            leftLimit = 0;
            for (int x = maxColumn; x < width; x++)
            {
                if (sfc[x] == 0) { leftLimit = x; break; }
            }
        }

        unsafe private static void EnhanceBlue(Image<Bgr, byte> img)
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
                    if (red + green > blue) { gray = 0; } else { gray = 255; };

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
        /// Locate right limit of the license plate
        /// </summary>
        /// <param name="img">The image</param>
        /// <param name="upperLimit">License plate upper limit</param>
        /// <param name="lowerLimit">License plate lower limit</param>
        /// <param name="leftLimit">License plate left limit</param>
        /// <param name="rightLimit">License plate right limit</param>
        unsafe private static void LocateLPRight(Image<Bgr, byte> img, int upperLimit, int lowerLimit, int leftLimit, out int rightLimit)
        {
            //Preprocess image
            ConvertToBW_Otsu(img);

            MIplImage m = img.MIplImage;
            byte* imgPtr = (byte*)m.imageData.ToPointer();
            int widthstep = m.widthStep;
            int nC = m.nChannels;
            int width = m.width;
            int height = m.height;

            //Determine column with highest amplitude
            int[] sfc = new int[width];
            for (int x = leftLimit; x < width; x++)
            {
                sfc[x] = 0;
                for (int y = upperLimit; y < lowerLimit; y++)
                {
                    byte* pixelPtr = imgPtr + y * widthstep + x * nC;
                    if (pixelPtr[0] == 255)
                    {
                        sfc[x]++;
                    }
                }
            }

            //Search right limit
            rightLimit = 0;
            for (int x = leftLimit; x < width; x++)
            {
                if (sfc[x] == 0) { rightLimit = x; break; }
            }
        }

        /// <summary>
        /// *** Not great ***
        /// Locates the license plate characters
        /// </summary>
        /// <param name="img">The image</param>
        /// <param name="LP_Location">The license plate location</param>
        /// <param name="LP_Chr1">Location of character 1</param>
        /// <param name="LP_Chr2">Location of character 2</param>
        /// <param name="LP_Chr3">Location of character 3</param>
        /// <param name="LP_Chr4">Location of character 4</param>
        /// <param name="LP_Chr5">Location of character 5</param>
        /// <param name="LP_Chr6">Location of character 6</param>
        unsafe private static void Locate_LP_Chars(Image<Bgr, byte> img, Rectangle LP_Location, out Rectangle LP_Chr1, out Rectangle LP_Chr2, out Rectangle LP_Chr3, out Rectangle LP_Chr4, out Rectangle LP_Chr5, out Rectangle LP_Chr6)
        {
            int upperLimit = LP_Location.Top;
            int lowerLimit = LP_Location.Bottom;
            int leftLimit = LP_Location.Left;
            int rightLimit = LP_Location.Right;
            int width = LP_Location.Width;
            int height = LP_Location.Height;

            //Preprocess image
            ConvertToBW_Otsu(img);

            MIplImage m = img.MIplImage;
            byte* imgPtr = (byte*)m.imageData.ToPointer();
            int widthstep = m.widthStep;
            int nC = m.nChannels;

            // Determine column with highest amplitude
            int[] sfc = new int[width];
            int min = 255;
            for (int x = 0; x < width; x++)
            {
                sfc[x] = 0;
                for (int y = 0; y < height; y++)
                {
                    byte* pixelPtr = imgPtr + (y + upperLimit) * widthstep + (x + leftLimit) * nC;
                    if (pixelPtr[0] == 0)
                    {
                        sfc[x]++;
                    }
                }
                if (sfc[x] < min && sfc[x] != 0)
                {
                    min = sfc[x]; //new max found
                }
            }

            //Crop lower levels in order to separate chars
            for (int i = 0; i < sfc.Length; i++)
            {
                if (sfc[i] < (min * 2)) { sfc[i] = 0; }
            }

            //Find objects along x axis
            int start = -1;
            int length = 0;
            int objCount = 0;
            Rectangle[] charLocations = new Rectangle[6];
            for (int i = 0; i < sfc.Length; i++)
            {
                if (start == -1 && sfc[i] != 0)
                {
                    //new object found
                    start = i;
                    length++;
                }
                else if (start != -1)
                {
                    if (sfc[i] != 0)
                    {
                        //inside object
                        length++;
                    }
                    else
                    {
                        //outside object
                        if (length > 5)
                        {
                            //object was large enough. don't discard
                            int charX, charY, charHeight;
                            charX = start + leftLimit;
                            TrimCharArea(img, upperLimit, lowerLimit, charX, out charY, length, out charHeight); //trim top and bottom to only show char
                            charLocations[objCount++] = new Rectangle(charX, charY, length, charHeight); //create char rectangle
                        }
                        //reset
                        start = -1;
                        length = 0;
                        if (objCount == 6)
                        {
                            break; //only looking for 6 chars
                        }
                    }
                }
            }
            if (start != -1 && objCount < 6)
            {
                //did not finish, now outside object
                if (length > 5)
                {
                    //object was large enough. don't discard
                    int charX, charY, charHeight;
                    charX = start + leftLimit;
                    TrimCharArea(img, upperLimit, lowerLimit, charX, out charY, length, out charHeight); //trim top and bottom to only show char
                    charLocations[objCount++] = new Rectangle(charX, charY, length, charHeight); //create char rectangle
                }
            }

            //Assign rectangles
            LP_Chr1 = charLocations[0];
            LP_Chr2 = charLocations[1];
            LP_Chr3 = charLocations[2];
            LP_Chr4 = charLocations[3];
            LP_Chr5 = charLocations[4];
            LP_Chr6 = charLocations[5];
        }

        /// <summary>
        /// Trim the top and bottom section where a character is located 
        /// </summary>
        /// <param name="img">The image</param>
        /// <param name="LP_UpperLimit">License plate upper limit</param>
        /// <param name="LP_LowerLimit">License plate lower limit</param>
        /// <param name="charX">Character top-left corner x coord</param>
        /// <param name="charY">Character top-left corner y coord</param>
        /// <param name="charWidth">Character width</param>
        /// <param name="charHeight">Character height</param>
        unsafe private static void TrimCharArea(Image<Bgr, byte> img, int LP_UpperLimit, int LP_LowerLimit, int charX, out int charY, int charWidth, out int charHeight)
        {
            MIplImage m = img.MIplImage;
            byte* imgPtr = (byte*)m.imageData.ToPointer();
            int widthstep = m.widthStep;
            int nC = m.nChannels;
            int width = m.width;
            int height = m.height;

            //Determine row with highest amplitude
            int[] sfr = new int[LP_LowerLimit - LP_UpperLimit];
            for (int y = 0; y < sfr.Length; y++)
            {
                sfr[y] = 0;
                for (int x = 0; x < charWidth; x++)
                {
                    byte* pixelPtr = imgPtr + (y + LP_UpperLimit) * widthstep + (x + charX) * nC;
                    if (pixelPtr[0] == 0)
                    {
                        sfr[y]++;
                    }
                }
            }

            //Find object along y axis
            charY = LP_UpperLimit;
            charHeight = LP_LowerLimit - LP_UpperLimit;
            int start = -1;
            int length = 0;
            bool found = false;
            for (int i = 0; i < sfr.Length; i++)
            {
                if (start == -1 && sfr[i] != 0)
                {
                    //new object found
                    start = i;
                    length++;
                }
                else if (start != -1)
                {
                    if (sfr[i] != 0)
                    {
                        //inside object
                        length++;
                    }
                    else
                    {
                        //outside object
                        if (length > 5)
                        {
                            //object was large enough. don't discard
                            charY = start + LP_UpperLimit;
                            charHeight = length;
                            found = true;
                            break; //only looking for 1 char
                        }
                        //reset
                        start = -1;
                        length = 0;
                    }
                }
            }
            if (start != -1 && !found)
            {
                //did not finish, now outside object
                if (length > 5)
                {
                    //object was large enough. don't discard
                    charY = start + LP_UpperLimit;
                    charHeight = length;
                    found = true;
                }
            }

            Console.WriteLine("Char @ (" + charX + "," + charY + ") with width (" + charWidth + ") and height (" + charHeight + ")");
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

                    int lx = (int)nx;
                    int uy = (int)ny;
                    double xdec = nx - lx;
                    double ydec = ny - uy;

                    byte* luPtr = dataPtrCopy + lx * nC + uy * widthstep;
                    byte* ruPtr = luPtr + nC;
                    byte* ldPtr = luPtr + widthstep;
                    byte* rdPtr = ruPtr + widthstep;

                    if (ny >= 0 && ny < height && nx >= 0 && nx < width)
                    {
                        dataPtr[0] = (byte)Math.Round((1 - ydec) * ((1 - xdec) * luPtr[0] + xdec * ruPtr[0]) + ydec * ((1 - xdec) * ldPtr[0] + xdec * rdPtr[0]));
                        dataPtr[1] = (byte)Math.Round((1 - ydec) * ((1 - xdec) * luPtr[1] + xdec * ruPtr[1]) + ydec * ((1 - xdec) * ldPtr[1] + xdec * rdPtr[1]));
                        dataPtr[2] = (byte)Math.Round((1 - ydec) * ((1 - xdec) * luPtr[2] + xdec * ruPtr[2]) + ydec * ((1 - xdec) * ldPtr[2] + xdec * rdPtr[2]));
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

                    int lx = (int)nx;
                    int uy = (int)ny;
                    double xdec = nx - lx;
                    double ydec = ny - uy;

                    byte* luPtr = dataPtrCopy + lx * nC + uy * widthstep;
                    byte* ruPtr = luPtr + nC;
                    byte* ldPtr = luPtr + widthstep;
                    byte* rdPtr = ruPtr + widthstep;

                    byte* dest = dataPtr + y * widthstep + x * nC;

                    if (ny >= 0 && ny < height && nx >= 0 && nx < width)
                    {
                        dest[0] = (byte)Math.Round((1 - ydec) * ((1 - xdec) * luPtr[0] + xdec * ruPtr[0]) + ydec * ((1 - xdec) * ldPtr[0] + xdec * rdPtr[0]));
                        dest[1] = (byte)Math.Round((1 - ydec) * ((1 - xdec) * luPtr[1] + xdec * ruPtr[1]) + ydec * ((1 - xdec) * ldPtr[1] + xdec * rdPtr[1]));
                        dest[2] = (byte)Math.Round((1 - ydec) * ((1 - xdec) * luPtr[2] + xdec * ruPtr[2]) + ydec * ((1 - xdec) * ldPtr[2] + xdec * rdPtr[2]));
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
        /// Work in progress
        /// </summary>
        /// <param name="img">The image</param>
        /// <param name="charLoc">A rectangle representing the character's location</param>
        /// <param name="charList">The list of characters for the recognition process</param>
        /// <returns></returns>
        unsafe private static string CharacterRecognition(Image<Bgr, byte> img, Rectangle charLoc, Dictionary<char, Image<Bgr, byte>> charList)
        {
            string s = "";
            img.ROI = charLoc;
            Image<Bgr, byte> charImg = img.Copy();
            img.ROI = Rectangle.Empty;
            byte* charPtr = (byte*)charImg.MIplImage.imageData.ToPointer();
            Dictionary<char, int> counts = new Dictionary<char, int>();
            Dictionary<char, int> assyms = new Dictionary<char, int>();
            Dictionary<char, int> assymsExternalLower = new Dictionary<char, int>();
            Dictionary<char, int> assymsInternal = new Dictionary<char, int>();
            Dictionary<char, int> assymsInternalLower = new Dictionary<char, int>();
            Dictionary<char, int> assymsVertical = new Dictionary<char, int>();
            Dictionary<char, int> assymsExternalVerticalLower = new Dictionary<char, int>();
            Dictionary<char, int> assymsInternalVertical = new Dictionary<char, int>();
            Dictionary<char, int> assymsInternalVerticalLower = new Dictionary<char, int>();

            // aqui fica em b&w
            ConvertToBW_Otsu(charImg);

            // aqui tentamos reconhecer o caracter
            foreach (Image<Bgr, byte> i in charList.Values)
            {
                char c = charList.FirstOrDefault(x => x.Value.Equals(i)).Key;

                int charCount = CountPixels(charImg);
                int iCount = CountPixels(i);

                int charAssym = ExternalAsymmetry(charImg);
                int iAssym = ExternalAsymmetry(i);

                int charAssymExternalLower = ExternalLowerAsymmetry(charImg);
                int iAssymExternalLower = ExternalLowerAsymmetry(i);

                int charAssymInternal = InternalAsymmetry(charImg);
                int iAssymInternal = InternalAsymmetry(i);

                int charAssymInternalLower = InternalLowerAsymmetry(charImg);
                int iAssymInternalLower = InternalLowerAsymmetry(i);

                int charAssymVertical = ExternalVerticalAsymmetry(charImg);
                int iAssymVertical = ExternalVerticalAsymmetry(i);

                int charAssymExternalVerticalLower = ExternalLowerVerticalAsymmetry(charImg);
                int iAssymExternalVerticalLower = ExternalLowerVerticalAsymmetry(i);

                int charAssymInternalVertical = InternalVerticalAsymmetry(charImg);
                int iAssymInternalVertical = InternalVerticalAsymmetry(i);

                int charAssymInternalLowerVertical = InternalLowerVerticalAsymmetry(charImg);
                int iAssymInternalLowerVertical = InternalLowerVerticalAsymmetry(i);

                int countDiff = Math.Abs(charCount - iCount);
                int assymDiff = Math.Abs(charAssym - iAssym);
                int assymExternalLowerDiff = Math.Abs(charAssymExternalLower - iAssymExternalLower);
                int assymInternalDiff = Math.Abs(charAssymInternal - iAssymInternal);
                int assymInternalLowerDiff = Math.Abs(charAssymInternalLower - iAssymInternalLower);
                int assymDiffVertical = Math.Abs(charAssymVertical - iAssymVertical);
                int assymExternalLowerVerticalDiff = Math.Abs(charAssymExternalVerticalLower - iAssymExternalVerticalLower);
                int assymInternalVerticalDiff = Math.Abs(charAssymInternalVertical - iAssymInternalVertical);
                int assymInternalLowerVerticalDiff = Math.Abs(charAssymInternalLowerVertical - iAssymInternalLowerVertical);

                counts.Add(c, countDiff);
                assyms.Add(c, assymDiff);
                assymsExternalLower.Add(c, assymExternalLowerDiff);
                assymsInternal.Add(c, assymInternalDiff);
                assymsInternalLower.Add(c, assymInternalLowerDiff);
                assymsVertical.Add(c, assymDiffVertical);
                assymsExternalVerticalLower.Add(c, assymExternalLowerVerticalDiff);
                assymsInternalVertical.Add(c, assymInternalVerticalDiff);
                assymsInternalVerticalLower.Add(c, assymInternalLowerVerticalDiff);
            }

            var countsOrd = counts.OrderBy(x => x.Value);
            var assymsOrd = assyms.OrderBy(x => x.Value);
            var assymsExternalLowerOrd = assymsExternalLower.OrderBy(x => x.Value);
            var assymsInternalOrd = assymsInternal.OrderBy(x => x.Value);
            var assymsInternalLowerOrd = assymsInternalLower.OrderBy(x => x.Value);
            var assymsVerticalOrd = assymsVertical.OrderBy(x => x.Value);
            var assymsExternalLowerVerticalOrd = assymsExternalVerticalLower.OrderBy(x => x.Value);
            var assymsInternalVerticalOrd = assymsInternalVertical.OrderBy(x => x.Value);
            var assymsInternalLowerVerticalOrd = assymsInternalVerticalLower.OrderBy(x => x.Value);
            SortedList<char, int> list = new SortedList<char, int>();
            foreach (char c in charList.Keys)
            {
                int i = countsOrd.TakeWhile(x => x.Key != c).Count() + assymsOrd.TakeWhile(x => x.Key != c).Count()
                    + assymsExternalLowerOrd.TakeWhile(x => x.Key != c).Count() + assymsInternalOrd.TakeWhile(x => x.Key != c).Count()
                    + assymsInternalLowerOrd.TakeWhile(x => x.Key != c).Count() + assymsVerticalOrd.TakeWhile(x => x.Key != c).Count()
                    + assymsExternalLowerVerticalOrd.TakeWhile(x => x.Key != c).Count() + assymsInternalVerticalOrd.TakeWhile(x => x.Key != c).Count()
                    + assymsInternalLowerVerticalOrd.TakeWhile(x => x.Key != c).Count();
                list.Add(c, i);
            }

            s = list.OrderBy(x => x.Value).First().Key.ToString();

            Console.WriteLine("\nthis character is a " + s);

            return s;
        }

        unsafe private static int ExternalAsymmetry(Image<Bgr, byte> img)
        {
            MIplImage m = img.MIplImage;
            int widthstep = m.widthStep;
            int nC = m.nChannels;
            byte* imgPtr = (byte*)m.imageData.ToPointer();
            int width = m.width;
            int halfwidth = (int)Math.Round(width / 2.0);
            int height = m.height;
            int halfheight = (int)Math.Round(height / 2.0);
            int extAssym = 0;


            for (int y = 0; y < halfheight; y++)
            {
                int x, assym = 0;
                for (x = 0; x < halfwidth; x++)
                {
                    byte* pixel = imgPtr + y * widthstep + x * nC;
                    if (pixel[0] == 255) assym++;
                    else break;
                }

                for (x = width - 1; x >= halfwidth; x--)
                {
                    byte* pixel = imgPtr + y * widthstep + x * nC;
                    if (pixel[0] == 255) assym--;
                    else break;
                }
                extAssym += Math.Abs(assym);
            }

            return extAssym;
        }

        unsafe private static int ExternalLowerAsymmetry(Image<Bgr, byte> img)
        {
            MIplImage m = img.MIplImage;
            int widthstep = m.widthStep;
            int nC = m.nChannels;
            byte* imgPtr = (byte*)m.imageData.ToPointer();
            int width = m.width;
            int halfwidth = (int)Math.Round(width / 2.0);
            int height = m.height;
            int halfheight = (int)Math.Round(height / 2.0);
            int extAssym = 0;


            for (int y = halfheight; y < height; y++)
            {
                int x, assym = 0;
                for (x = 0; x < halfwidth; x++)
                {
                    byte* pixel = imgPtr + y * widthstep + x * nC;
                    if (pixel[0] == 255) assym++;
                    else break;
                }

                for (x = width - 1; x >= halfwidth; x--)
                {
                    byte* pixel = imgPtr + y * widthstep + x * nC;
                    if (pixel[0] == 255) assym--;
                    else break;
                }
                extAssym += Math.Abs(assym);
            }

            return extAssym;
        }

        unsafe private static int InternalAsymmetry(Image<Bgr, byte> img)
        {
            MIplImage m = img.MIplImage;
            int widthstep = m.widthStep;
            int nC = m.nChannels;
            byte* imgPtr = (byte*)m.imageData.ToPointer();
            int width = m.width;
            int halfwidth = width / 2;
            int height = m.height;
            int halfheight = height / 2;
            int intAssym = 0;


            for (int y = 0; y < halfheight; y++)
            {
                int x;
                for (x = halfwidth; x > 0; x--)
                {
                    byte* pixel = imgPtr + y * widthstep + x * nC;
                    if (pixel[0] == 255) intAssym++;
                    else break;
                }

                for (x = halfwidth; x < width; x++)
                {
                    byte* pixel = imgPtr + y * widthstep + x * nC;
                    if (pixel[0] == 255) intAssym--;
                    else break;
                }
                if (intAssym < 0) intAssym *= -1;
            }

            return intAssym;
        }

        unsafe private static int InternalLowerAsymmetry(Image<Bgr, byte> img)
        {
            MIplImage m = img.MIplImage;
            int widthstep = m.widthStep;
            int nC = m.nChannels;
            byte* imgPtr = (byte*)m.imageData.ToPointer();
            int width = m.width;
            int halfwidth = width / 2;
            int height = m.height;
            int halfheight = height / 2;
            int intAssym = 0;


            for (int y = halfheight; y < height; y++)
            {
                int x;
                for (x = halfwidth; x > 0; x--)
                {
                    byte* pixel = imgPtr + y * widthstep + x * nC;
                    if (pixel[0] == 255) intAssym++;
                    else break;
                }

                for (x = halfwidth; x < width; x++)
                {
                    byte* pixel = imgPtr + y * widthstep + x * nC;
                    if (pixel[0] == 255) intAssym--;
                    else break;
                }
                if (intAssym < 0) intAssym *= -1;
            }

            return intAssym;
        }

        unsafe private static int ExternalVerticalAsymmetry(Image<Bgr, byte> img)
        {
            MIplImage m = img.MIplImage;
            int widthstep = m.widthStep;
            int nC = m.nChannels;
            byte* imgPtr = (byte*)m.imageData.ToPointer();
            int width = m.width;
            int halfwidth = (int)Math.Round(width / 2.0);
            int height = m.height;
            int halfheight = (int)Math.Round(height / 2.0);
            int extAssym = 0;


            for (int x = 0; x < halfwidth; x++)
            {
                int y, assym = 0;
                for (y = 0; y < halfheight; y++)
                {
                    byte* pixel = imgPtr + y * widthstep + x * nC;
                    if (pixel[0] == 255) assym++;
                    else break;
                }

                for (y = height - 1; y >= halfheight; y--)
                {
                    byte* pixel = imgPtr + y * widthstep + x * nC;
                    if (pixel[0] == 255) assym--;
                    else break;
                }
                extAssym += Math.Abs(assym);
            }

            return extAssym;
        }

        unsafe private static int ExternalLowerVerticalAsymmetry(Image<Bgr, byte> img)
        {
            MIplImage m = img.MIplImage;
            int widthstep = m.widthStep;
            int nC = m.nChannels;
            byte* imgPtr = (byte*)m.imageData.ToPointer();
            int width = m.width;
            int halfwidth = (int)Math.Round(width / 2.0);
            int height = m.height;
            int halfheight = (int)Math.Round(height / 2.0);
            int extAssym = 0;


            for (int x = halfwidth; x < width; x++)
            {
                int y, assym = 0;
                for (y = 0; y < halfheight; y++)
                {
                    byte* pixel = imgPtr + y * widthstep + x * nC;
                    if (pixel[0] == 255) assym++;
                    else break;
                }

                for (y = height - 1; y >= halfheight; y--)
                {
                    byte* pixel = imgPtr + y * widthstep + x * nC;
                    if (pixel[0] == 255) assym--;
                    else break;
                }
                extAssym += Math.Abs(assym);
            }

            return extAssym;
        }

        unsafe private static int InternalVerticalAsymmetry(Image<Bgr, byte> img)
        {
            MIplImage m = img.MIplImage;
            int widthstep = m.widthStep;
            int nC = m.nChannels;
            byte* imgPtr = (byte*)m.imageData.ToPointer();
            int width = m.width;
            int halfwidth = width / 2;
            int height = m.height;
            int halfheight = height / 2;
            int intAssym = 0;


            for (int x = 0; x < halfwidth; x++)
            {
                int y;
                for (y = halfheight; y > 0; y--)
                {
                    byte* pixel = imgPtr + y * widthstep + x * nC;
                    if (pixel[0] == 255) intAssym++;
                    else break;
                }

                for (y = halfheight; y < height; y++)
                {
                    byte* pixel = imgPtr + y * widthstep + x * nC;
                    if (pixel[0] == 255) intAssym--;
                    else break;
                }
                if (intAssym < 0) intAssym *= -1;
            }

            return intAssym;
        }

        unsafe private static int InternalLowerVerticalAsymmetry(Image<Bgr, byte> img)
        {
            MIplImage m = img.MIplImage;
            int widthstep = m.widthStep;
            int nC = m.nChannels;
            byte* imgPtr = (byte*)m.imageData.ToPointer();
            int width = m.width;
            int halfwidth = width / 2;
            int height = m.height;
            int halfheight = height / 2;
            int intAssym = 0;


            for (int x = halfwidth; x < width; x++)
            {
                int y;
                for (y = halfheight; y > 0; y--)
                {
                    byte* pixel = imgPtr + y * widthstep + x * nC;
                    if (pixel[0] == 255) intAssym++;
                    else break;
                }

                for (y = halfheight; y < height; y++)
                {
                    byte* pixel = imgPtr + y * widthstep + x * nC;
                    if (pixel[0] == 255) intAssym--;
                    else break;
                }
                if (intAssym < 0) intAssym *= -1;
            }

            return intAssym;
        }

        unsafe private static int CountPixels(Image<Bgr, byte> img)
        {
            MIplImage m = img.MIplImage;
            int widthstep = m.widthStep;
            int nC = m.nChannels;
            byte* imgPtr = (byte*)m.imageData.ToPointer();
            int width = m.width;
            int height = m.height;
            int count = 0;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    byte* pixel = imgPtr + y * widthstep + x * nC;
                    if (pixel[0] == 0) count++;
                }
            }

            return count;
        }
    }
}