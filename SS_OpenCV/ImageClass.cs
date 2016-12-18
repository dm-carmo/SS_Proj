using System;
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

            byte* dest;
            byte* orig;

            //Core
            for (int y = 1; y < height - 1; y++)
            {
                for (int x = 1; x < width - 1; x++)
                {
                    dest = dataPtr + y * widthstep + x * nC;
                    orig = dataPtrCopy + (y - 1) * widthstep + (x - 1) * nC;
                    dest[0] = (byte)Math.Round((orig[0] + orig[nC] + orig[2 * nC] + orig[widthstep] + orig[widthstep + nC] +
                        orig[widthstep + 2 * nC] + orig[2 * widthstep] + orig[2 * widthstep + nC] + orig[2 * widthstep + 2 * nC]) / 9.0);
                    dest[1] = (byte)Math.Round((orig[1] + orig[nC + 1] + orig[2 * nC + 1] + orig[widthstep + 1] + orig[widthstep + nC + 1] +
                        orig[widthstep + 2 * nC + 1] + orig[2 * widthstep + 1] + orig[2 * widthstep + nC + 1] + orig[2 * widthstep + 2 * nC + 1]) / 9.0);
                    dest[2] = (byte)Math.Round((orig[2] + orig[nC + 2] + orig[2 * nC + 2] + orig[widthstep + 2] + orig[widthstep + nC + 2] +
                        orig[widthstep + 2 * nC + 2] + orig[2 * widthstep + 2] + orig[2 * widthstep + nC + 2] + orig[2 * widthstep + 2 * nC + 2]) / 9.0);
                }
            }

            //Margem superior
            for (int x = 1; x < width - 1; x++)
            {
                dest = dataPtr + x * nC;
                orig = dataPtrCopy + (x - 1) * nC;
                dest[0] = (byte)Math.Round((2 * orig[0] + 2 * orig[nC] + 2 * orig[2 * nC] + orig[widthstep] + orig[widthstep + nC] +
                    orig[widthstep + 2 * nC]) / 9.0);
                dest[1] = (byte)Math.Round((2 * orig[1] + 2 * orig[nC + 1] + 2 * orig[2 * nC + 1] + orig[widthstep + 1] + orig[widthstep + nC + 1] +
                    orig[widthstep + 2 * nC + 1]) / 9.0);
                dest[2] = (byte)Math.Round((2 * orig[2] + 2 * orig[nC + 2] + 2 * orig[2 * nC + 2] + orig[widthstep + 2] + orig[widthstep + nC + 2] +
                    orig[widthstep + 2 * nC + 2]) / 9.0);
            }

            //Margem inferior
            for (int x = 1; x < width - 1; x++)
            {
                dest = dataPtr + (height - 1) * widthstep + x * nC;
                orig = dataPtrCopy + (height - 2) * widthstep + (x - 1) * nC;
                dest[0] = (byte)Math.Round((orig[0] + orig[nC] + orig[2 * nC] + 2 * orig[widthstep] + 2 * orig[widthstep + nC] +
                    2 * orig[widthstep + 2 * nC]) / 9.0);
                dest[1] = (byte)Math.Round((orig[1] + orig[nC + 1] + orig[2 * nC + 1] + 2 * orig[widthstep + 1] + 2 * orig[widthstep + nC + 1] +
                    2 * orig[widthstep + 2 * nC + 1]) / 9.0);
                dest[2] = (byte)Math.Round((orig[2] + orig[nC + 2] + orig[2 * nC + 2] + 2 * orig[widthstep + 2] + 2 * orig[widthstep + nC + 2] +
                    2 * orig[widthstep + 2 * nC + 2]) / 9.0);
            }

            //Margem esquerda
            for (int y = 1; y < height - 1; y++)
            {
                dest = dataPtr + y * widthstep;
                orig = dataPtrCopy + (y - 1) * widthstep;
                dest[0] = (byte)Math.Round((2 * orig[0] + orig[nC] + 2 * orig[widthstep] + orig[widthstep + nC] +
                    2 * orig[2 * widthstep] + orig[2 * widthstep + nC]) / 9.0);
                dest[1] = (byte)Math.Round((2 * orig[1] + orig[nC + 1] + 2 * orig[widthstep + 1] + orig[widthstep + nC + 1] +
                    2 * orig[2 * widthstep + 1] + orig[2 * widthstep + nC + 1]) / 9.0);
                dest[2] = (byte)Math.Round((2 * orig[2] + orig[nC + 2] + 2 * orig[widthstep + 2] + orig[widthstep + nC + 2] +
                    2 * orig[2 * widthstep + 2] + orig[2 * widthstep + nC + 2]) / 9.0);
            }

            //Margem direita
            for (int y = 1; y < height - 1; y++)
            {
                dest = dataPtr + y * widthstep + (width - 1) * nC;
                orig = dataPtrCopy + (y - 1) * widthstep + (width - 2) * nC;
                dest[0] = (byte)Math.Round((orig[0] + 2 * orig[nC] + orig[widthstep] + 2 * orig[widthstep + nC] +
                    orig[2 * widthstep] + 2 * orig[2 * widthstep + nC]) / 9.0);
                dest[1] = (byte)Math.Round((orig[1] + 2 * orig[nC + 1] + orig[widthstep + 1] + 2 * orig[widthstep + nC + 1] +
                    orig[2 * widthstep + 1] + 2 * orig[2 * widthstep + nC + 1]) / 9.0);
                dest[2] = (byte)Math.Round((orig[2] + 2 * orig[nC + 2] + orig[widthstep + 2] + 2 * orig[widthstep + nC + 2] +
                    orig[2 * widthstep + 2] + 2 * orig[2 * widthstep + nC + 2]) / 9.0);
            }
            
            //Canto superior esquerdo
            dest = dataPtr;
            orig = dataPtrCopy;
            dest[0] = (byte)Math.Round((4 * orig[0] + 2 * orig[nC] + 2 * orig[widthstep] + orig[widthstep + nC]) / 9.0);
            dest[1] = (byte)Math.Round((4 * orig[1] + 2 * orig[nC + 1] + 2 * orig[widthstep + 1] + orig[widthstep + nC + 1]) / 9.0);
            dest[2] = (byte)Math.Round((4 * orig[2] + 2 * orig[nC + 2] + 2 * orig[widthstep + 2] + orig[widthstep + nC + 2]) / 9.0);

            //Canto superior direito
            dest = dataPtr + (width - 1) * nC;
            orig = dataPtrCopy + (width - 2) * nC;
            dest[0] = (byte)Math.Round((2 * orig[0] + 4 * orig[nC] + orig[widthstep] + 2 * orig[widthstep + nC]) / 9.0);
            dest[1] = (byte)Math.Round((2 * orig[1] + 4 * orig[nC + 1] + orig[widthstep + 1] + 2 * orig[widthstep + nC + 1]) / 9.0);
            dest[2] = (byte)Math.Round((2 * orig[2] + 4 * orig[nC + 2] + orig[widthstep + 2] + 2 * orig[widthstep + nC + 2]) / 9.0);

            //Canto inferior esquerdo
            dest = dataPtr + (height - 1) * widthstep;
            orig = dataPtrCopy + (height - 2) * widthstep;
            dest[0] = (byte)Math.Round((2 * orig[0] + orig[nC] + 4 * orig[widthstep] + 2 * orig[widthstep + nC]) / 9.0);
            dest[1] = (byte)Math.Round((2 * orig[1] + orig[nC + 1] + 4 * orig[widthstep + 1] + 2 * orig[widthstep + nC + 1]) / 9.0);
            dest[2] = (byte)Math.Round((2 * orig[2] + orig[nC + 2] + 4 * orig[widthstep + 2] + 2 * orig[widthstep + nC + 2]) / 9.0);

            //Canto inferior direito
            dest = dataPtr + (height - 1) * widthstep + (width - 1) * nC;
            orig = dataPtrCopy + (height - 2) * widthstep + (width - 2) * nC;
            dest[0] = (byte)Math.Round((orig[0] + 2 * orig[nC] + 2 * orig[widthstep] + 4 * orig[widthstep + nC]) / 9.0);
            dest[1] = (byte)Math.Round((orig[1] + 2 * orig[nC + 1] + 2 * orig[widthstep + 1] + 4 * orig[widthstep + nC + 1]) / 9.0);
            dest[2] = (byte)Math.Round((orig[2] + 2 * orig[nC + 2] + 2 * orig[widthstep + 2] + 4 * orig[widthstep + nC + 2]) / 9.0);

        }

        /// <summary>
        /// *** NEEDS MARGINS ***
        /// Calculates the mean of an image, using solution B.
        /// Each pixel is replaced by the mean of their neighborhood (3x3)
        /// </summary>
        /// <param name="img">The image</param>
        /// <param name="imgCopy">A copy of the image</param>
        unsafe public static void Mean_solutionB(Image<Bgr, byte> img, Image<Bgr, byte> imgCopy)
        {
            MIplImage copy = imgCopy.MIplImage;
            MIplImage m = img.MIplImage;
            byte* dataPtr = (byte*)m.imageData.ToPointer(); // Pointer to the image
            byte* dataPtrCopy = (byte*)copy.imageData.ToPointer(); // Pointer to the image copy

            int widthstep = m.widthStep;
            int nC = m.nChannels;
            int width = m.width;
            int height = m.height;

            byte* orig = dataPtrCopy;

            for (int y = 1; y < height - 1; y++)
            { //calculates the first sum in the row
                int firstBlue = orig[0] + orig[nC] + orig[2 * nC] + orig[widthstep] + orig[widthstep + nC] +
                    orig[widthstep + 2 * nC] + orig[2 * widthstep] + orig[2 * widthstep + nC] + orig[2 * widthstep + 2 * nC];
                int firstGreen = orig[1] + orig[nC + 1] + orig[2 * nC + 1] + orig[widthstep + 1] + orig[widthstep + nC + 1] +
                orig[widthstep + 2 * nC + 1] + orig[2 * widthstep + 1] + orig[2 * widthstep + nC + 1] + orig[2 * widthstep + 2 * nC + 1];
                int firstRed = orig[2] + orig[nC + 2] + orig[2 * nC + 2] + orig[widthstep + 2] + orig[widthstep + nC + 2] +
                orig[widthstep + 2 * nC + 2] + orig[2 * widthstep + 2] + orig[2 * widthstep + nC + 2] + orig[2 * widthstep + 2 * nC + 2];

                //used later
                byte* previous = orig;

                for (int x = 1; x < width - 1; x++)
                {
                    byte* dest = dataPtr + y * widthstep + x * nC;
                    dest[0] = (byte)Math.Round(firstBlue / 9.0);
                    dest[1] = (byte)Math.Round(firstGreen / 9.0);
                    dest[2] = (byte)Math.Round(firstRed / 9.0);
                    //remove the bytes that are no longer needed
                    firstBlue -= previous[0] + previous[widthstep] + previous[2 * widthstep];
                    firstGreen -= previous[1] + previous[widthstep + 1] + previous[2 * widthstep + 1];
                    firstRed -= previous[2] + previous[widthstep + 2] + previous[2 * widthstep + 2];
                    //move pointer
                    previous += nC;
                    //add the new bytes
                    firstBlue += previous[2 * nC] + previous[widthstep + 2 * nC] + previous[2 * widthstep + 2 * nC];
                    firstGreen += previous[2 * nC + 1] + previous[widthstep + 2 * nC + 1] + previous[2 * widthstep + 2 * nC + 1];
                    firstRed += previous[2 * nC + 2] + previous[widthstep + 2 * nC + 2] + previous[2 * widthstep + 2 * nC + 2];
                }
                //move to the next row
                orig += widthstep;
            }
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
            byte* dataPtr = (byte*)m.imageData.ToPointer(); // Pointer to the image
            byte* dataPtrCopy = (byte*)imgCopy.MIplImage.imageData.ToPointer(); // Pointer to the image copy

            int nC = m.nChannels;
            int width = m.width;
            int widthstep = m.widthStep;
            int height = m.height;

            int[,] matSum = new int[9, 9];

            // Core
            // Percorre a vizinhanca e seleciona o pixel com menor distancia
            for (int y = 1; y < height - 1; y++)
            {
                for (int x = 1; x < width - 1; x++)
                {
                    byte* dest = dataPtr + y * widthstep + x * nC;
                    byte* orig = dataPtrCopy + y * widthstep + x * nC;

                    byte* a = (orig - widthstep - nC);
                    byte* b = (orig - widthstep);
                    byte* c = (orig - widthstep + nC);
                    byte* d = (orig - nC);
                    byte* e = orig;
                    byte* f = (orig + nC);
                    byte* g = (orig + widthstep - nC);
                    byte* h = (orig + widthstep);
                    byte* i = (orig + widthstep + nC);

                    //Sum (0,0)
                    int sum = matSum[0, 1] = Math.Abs(a[0] - b[0]) + Math.Abs(a[1] - b[1]) + Math.Abs(a[2] - b[2]);
                    sum += matSum[0, 2] = Math.Abs(a[0] - c[0]) + Math.Abs(a[1] - c[1]) + Math.Abs(a[2] - c[2]);
                    sum += matSum[0, 3] = Math.Abs(a[0] - d[0]) + Math.Abs(a[1] - d[1]) + Math.Abs(a[2] - d[2]);
                    sum += matSum[0, 4] = Math.Abs(a[0] - e[0]) + Math.Abs(a[1] - e[1]) + Math.Abs(a[2] - e[2]);
                    sum += matSum[0, 5] = Math.Abs(a[0] - f[0]) + Math.Abs(a[1] - f[1]) + Math.Abs(a[2] - f[2]);
                    sum += matSum[0, 6] = Math.Abs(a[0] - g[0]) + Math.Abs(a[1] - g[1]) + Math.Abs(a[2] - g[2]);
                    sum += matSum[0, 7] = Math.Abs(a[0] - h[0]) + Math.Abs(a[1] - h[1]) + Math.Abs(a[2] - h[2]);
                    sum += matSum[0, 8] = Math.Abs(a[0] - i[0]) + Math.Abs(a[1] - i[1]) + Math.Abs(a[2] - i[2]);
                    byte* pixel = a;
                    int min = sum;

                    //Sum (0,1)
                    sum = matSum[0, 1];
                    sum += matSum[1, 2] = Math.Abs(b[0] - c[0]) + Math.Abs(b[1] - c[1]) + Math.Abs(b[2] - c[2]);
                    sum += matSum[1, 3] = Math.Abs(b[0] - d[0]) + Math.Abs(b[1] - d[1]) + Math.Abs(b[2] - d[2]);
                    sum += matSum[1, 4] = Math.Abs(b[0] - e[0]) + Math.Abs(b[1] - e[1]) + Math.Abs(b[2] - e[2]);
                    sum += matSum[1, 5] = Math.Abs(b[0] - f[0]) + Math.Abs(b[1] - f[1]) + Math.Abs(b[2] - f[2]);
                    sum += matSum[1, 6] = Math.Abs(b[0] - g[0]) + Math.Abs(b[1] - g[1]) + Math.Abs(b[2] - g[2]);
                    sum += matSum[1, 7] = Math.Abs(b[0] - h[0]) + Math.Abs(b[1] - h[1]) + Math.Abs(b[2] - h[2]);
                    sum += matSum[1, 8] = Math.Abs(b[0] - i[0]) + Math.Abs(b[1] - i[1]) + Math.Abs(b[2] - i[2]);
                    if (sum < min)
                    {
                        pixel = b;
                        min = sum;
                    }

                    //Sum (0,2)
                    sum = matSum[0, 2];
                    sum += matSum[1, 2];
                    sum += matSum[2, 3] = Math.Abs(c[0] - d[0]) + Math.Abs(c[1] - d[1]) + Math.Abs(c[2] - d[2]);
                    sum += matSum[2, 4] = Math.Abs(c[0] - e[0]) + Math.Abs(c[1] - e[1]) + Math.Abs(c[2] - e[2]);
                    sum += matSum[2, 5] = Math.Abs(c[0] - f[0]) + Math.Abs(c[1] - f[1]) + Math.Abs(c[2] - f[2]);
                    sum += matSum[2, 6] = Math.Abs(c[0] - g[0]) + Math.Abs(c[1] - g[1]) + Math.Abs(c[2] - g[2]);
                    sum += matSum[2, 7] = Math.Abs(c[0] - h[0]) + Math.Abs(c[1] - h[1]) + Math.Abs(c[2] - h[2]);
                    sum += matSum[2, 8] = Math.Abs(c[0] - i[0]) + Math.Abs(c[1] - i[1]) + Math.Abs(c[2] - i[2]);
                    if (sum < min)
                    {
                        pixel = c;
                        min = sum;
                    }

                    //Sum (1,0)
                    sum = matSum[0, 3];
                    sum += matSum[1, 3];
                    sum += matSum[2, 3];
                    sum += matSum[3, 4] = Math.Abs(d[0] - e[0]) + Math.Abs(d[1] - e[1]) + Math.Abs(d[2] - e[2]);
                    sum += matSum[3, 5] = Math.Abs(d[0] - f[0]) + Math.Abs(d[1] - f[1]) + Math.Abs(d[2] - f[2]);
                    sum += matSum[3, 6] = Math.Abs(d[0] - g[0]) + Math.Abs(d[1] - g[1]) + Math.Abs(d[2] - g[2]);
                    sum += matSum[3, 7] = Math.Abs(d[0] - h[0]) + Math.Abs(d[1] - h[1]) + Math.Abs(d[2] - h[2]);
                    sum += matSum[3, 8] = Math.Abs(d[0] - i[0]) + Math.Abs(d[1] - i[1]) + Math.Abs(d[2] - i[2]);
                    if (sum < min)
                    {
                        pixel = d;
                        min = sum;
                    }

                    //Sum (1,1)
                    sum = matSum[0, 4];
                    sum += matSum[1, 4];
                    sum += matSum[2, 4];
                    sum += matSum[3, 4];
                    sum += matSum[4, 5] = Math.Abs(e[0] - f[0]) + Math.Abs(e[1] - f[1]) + Math.Abs(e[2] - f[2]);
                    sum += matSum[4, 6] = Math.Abs(e[0] - g[0]) + Math.Abs(e[1] - g[1]) + Math.Abs(e[2] - g[2]);
                    sum += matSum[4, 7] = Math.Abs(e[0] - h[0]) + Math.Abs(e[1] - h[1]) + Math.Abs(e[2] - h[2]);
                    sum += matSum[4, 8] = Math.Abs(e[0] - i[0]) + Math.Abs(e[1] - i[1]) + Math.Abs(e[2] - i[2]);
                    if (sum < min)
                    {
                        pixel = e;
                        min = sum;
                    }

                    //Sum (1,2)
                    sum = matSum[0, 5];
                    sum += matSum[1, 5];
                    sum += matSum[2, 5];
                    sum += matSum[3, 5];
                    sum += matSum[4, 5];
                    sum += matSum[5, 6] = Math.Abs(f[0] - g[0]) + Math.Abs(f[1] - g[1]) + Math.Abs(f[2] - g[2]);
                    sum += matSum[5, 7] = Math.Abs(f[0] - h[0]) + Math.Abs(f[1] - h[1]) + Math.Abs(f[2] - h[2]);
                    sum += matSum[5, 8] = Math.Abs(f[0] - i[0]) + Math.Abs(f[1] - i[1]) + Math.Abs(f[2] - i[2]);
                    if (sum < min)
                    {
                        pixel = f;
                        min = sum;
                    }

                    //Sum (2,0)
                    sum = matSum[0, 6];
                    sum += matSum[1, 6];
                    sum += matSum[2, 6];
                    sum += matSum[3, 6];
                    sum += matSum[4, 6];
                    sum += matSum[5, 6];
                    sum += matSum[6, 7] = Math.Abs(g[0] - h[0]) + Math.Abs(g[1] - h[1]) + Math.Abs(g[2] - h[2]);
                    sum += matSum[6, 8] = Math.Abs(g[0] - i[0]) + Math.Abs(g[1] - i[1]) + Math.Abs(g[2] - i[2]);
                    if (sum < min)
                    {
                        pixel = g;
                        min = sum;
                    }

                    //Sum (2,1)
                    sum = matSum[0, 7];
                    sum += matSum[1, 7];
                    sum += matSum[2, 7];
                    sum += matSum[3, 7];
                    sum += matSum[4, 7];
                    sum += matSum[5, 7];
                    sum += matSum[6, 7];
                    sum += matSum[7, 8] = Math.Abs(h[0] - i[0]) + Math.Abs(h[1] - i[1]) + Math.Abs(h[2] - i[2]);
                    if (sum < min)
                    {
                        pixel = h;
                        min = sum;
                    }

                    //Sum (2,2)
                    sum = matSum[0, 8];
                    sum += matSum[1, 8];
                    sum += matSum[2, 8];
                    sum += matSum[3, 8];
                    sum += matSum[4, 8];
                    sum += matSum[5, 8];
                    sum += matSum[6, 8];
                    sum += matSum[7, 8];
                    if (sum < min)
                    {
                        dest[0] = i[0];
                        dest[1] = i[1];
                        dest[2] = i[2];
                    }
                    else
                    {
                        dest[0] = pixel[0];
                        dest[1] = pixel[1];
                        dest[2] = pixel[2];
                    }
                }
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

            CharacterRecognition(imgCopy, LP_Chr1, LP_Chr2, LP_Chr3, LP_Chr4, LP_Chr5, LP_Chr6, charList,
                out LP_C1, out LP_C2, out LP_C3, out LP_C4, out LP_C5, out LP_C6);
            LP_Country = "";
            LP_Month = "";
            LP_Year = "";

            //debug only (draw lines)
            //DebugDrawLines(img, LP_Location, LP_Chr1, LP_Chr2, LP_Chr3, LP_Chr4, LP_Chr5, LP_Chr6);
        }

        /// <summary>
        /// *** DEBUG ***
        /// Draws lines indicating each of the rectangles' limits
        /// </summary>
        /// <param name="img">The image</param>
        /// <param name="LP_Location">License plate Location</param>
        /// <param name="LP_C1">License plate character 1</param>
        /// <param name="LP_C2">License plate character 2</param>
        /// <param name="LP_C3">License plate character 3</param>
        /// <param name="LP_C4">License plate character 4</param>
        /// <param name="LP_C5">License plate character 5</param>
        /// <param name="LP_C6">License plate character 6</param>
        unsafe private static void DebugDrawLines(Image<Bgr, byte> img, Rectangle LP_Location, Rectangle LP_C1,
            Rectangle LP_C2, Rectangle LP_C3, Rectangle LP_C4, Rectangle LP_C5, Rectangle LP_C6)
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
                byte* pixelPtr = imgPtr + y * widthstep + LP_Location.Left * nC;
                pixelPtr[1] = 255;
                pixelPtr = imgPtr + y * widthstep + (LP_Location.Right - 1) * nC;
                pixelPtr[1] = 255;
            }
            for (int x = LP_Location.Left; x < LP_Location.Right; x++)
            {
                byte* pixelPtr = imgPtr + LP_Location.Top * widthstep + x * nC;
                pixelPtr[1] = 255;
                pixelPtr = imgPtr + (LP_Location.Bottom - 1) * widthstep + x * nC;
                pixelPtr[1] = 255;
            }
            //LP_C1
            for (int y = LP_C1.Top; y < LP_C1.Bottom; y++)
            {
                byte* pixelPtr = imgPtr + y * widthstep + LP_C1.Left * nC;
                pixelPtr[1] = 255;
                pixelPtr = imgPtr + y * widthstep + (LP_C1.Right - 1) * nC;
                pixelPtr[1] = 255;
            }
            for (int x = LP_C1.Left; x < LP_C1.Right; x++)
            {
                byte* pixelPtr = imgPtr + LP_C1.Top * widthstep + x * nC;
                pixelPtr[1] = 255;
                pixelPtr = imgPtr + (LP_C1.Bottom - 1) * widthstep + x * nC;
                pixelPtr[1] = 255;
            }
            //LP_C2
            for (int y = LP_C2.Top; y < LP_C2.Bottom; y++)
            {
                byte* pixelPtr = imgPtr + y * widthstep + LP_C2.Left * nC;
                pixelPtr[1] = 255;
                pixelPtr = imgPtr + y * widthstep + (LP_C2.Right - 1) * nC;
                pixelPtr[1] = 255;
            }
            for (int x = LP_C2.Left; x < LP_C2.Right; x++)
            {
                byte* pixelPtr = imgPtr + LP_C2.Top * widthstep + x * nC;
                pixelPtr[1] = 255;
                pixelPtr = imgPtr + (LP_C2.Bottom - 1) * widthstep + x * nC;
                pixelPtr[1] = 255;
            }
            //LP_C3
            for (int y = LP_C3.Top; y < LP_C3.Bottom; y++)
            {
                byte* pixelPtr = imgPtr + y * widthstep + LP_C3.Left * nC;
                pixelPtr[1] = 255;
                pixelPtr = imgPtr + y * widthstep + (LP_C3.Right - 1) * nC;
                pixelPtr[1] = 255;
            }
            for (int x = LP_C3.Left; x < LP_C3.Right; x++)
            {
                byte* pixelPtr = imgPtr + LP_C3.Top * widthstep + x * nC;
                pixelPtr[1] = 255;
                pixelPtr = imgPtr + (LP_C3.Bottom - 1) * widthstep + x * nC;
                pixelPtr[1] = 255;
            }
            //LP_C4
            for (int y = LP_C4.Top; y < LP_C4.Bottom; y++)
            {
                byte* pixelPtr = imgPtr + y * widthstep + LP_C4.Left * nC;
                pixelPtr[1] = 255;
                pixelPtr = imgPtr + y * widthstep + (LP_C4.Right - 1) * nC;
                pixelPtr[1] = 255;
            }
            for (int x = LP_C4.Left; x < LP_C4.Right; x++)
            {
                byte* pixelPtr = imgPtr + LP_C4.Top * widthstep + x * nC;
                pixelPtr[1] = 255;
                pixelPtr = imgPtr + (LP_C4.Bottom - 1) * widthstep + x * nC;
                pixelPtr[1] = 255;
            }
            //LP_C5
            for (int y = LP_C5.Top; y < LP_C5.Bottom; y++)
            {
                byte* pixelPtr = imgPtr + y * widthstep + LP_C5.Left * nC;
                pixelPtr[1] = 255;
                pixelPtr = imgPtr + y * widthstep + (LP_C5.Right - 1) * nC;
                pixelPtr[1] = 255;
            }
            for (int x = LP_C5.Left; x < LP_C5.Right; x++)
            {
                byte* pixelPtr = imgPtr + LP_C5.Top * widthstep + x * nC;
                pixelPtr[1] = 255;
                pixelPtr = imgPtr + (LP_C5.Bottom - 1) * widthstep + x * nC;
                pixelPtr[1] = 255;
            }
            //LP_C6
            for (int y = LP_C6.Top; y < LP_C6.Bottom; y++)
            {
                byte* pixelPtr = imgPtr + y * widthstep + LP_C6.Left * nC;
                pixelPtr[1] = 255;
                pixelPtr = imgPtr + y * widthstep + (LP_C6.Right - 1) * nC;
                pixelPtr[1] = 255;
            }
            for (int x = LP_C6.Left; x < LP_C6.Right; x++)
            {
                byte* pixelPtr = imgPtr + LP_C6.Top * widthstep + x * nC;
                pixelPtr[1] = 255;
                pixelPtr = imgPtr + (LP_C6.Bottom - 1) * widthstep + x * nC;
                pixelPtr[1] = 255;
            }
        }

        /// <summary>
        /// Locates a license plate on an image
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
        /// Locates the upper and lower limits of the license plate
        /// </summary>
        /// <param name="img">The image</param>
        /// <param name="upperLimit">License plate upper limit</param>
        /// <param name="lowerLimit">License plate lower limit</param>
        unsafe private static void LocateLPVertical(Image<Bgr, byte> img, out int upperLimit, out int lowerLimit)
        {
            //Preprocess image
            float[,] gauss = { { 1, 2, 1 }, { 2, 4, 2 }, { 1, 2, 1 } };
            NonUniform(img, img.Copy(), gauss, 16); //noise reduction
            ConvertToBW_Otsu(img); //binarization
            Sobel(img, img.Copy()); //edge detection

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
                    if ((pixelPtrPrev[0] > 128 && pixelPtr[0] < 128) || (pixelPtrPrev[0] < 128 && pixelPtr[0] > 128)) sfr[y]++; //color switched. edge found
                }
                if (sfr[y] > sfr[maxRow]) maxRow = y; //new max found
            }

            int halfMaxValue = sfr[maxRow] / 2; //half of the maximal value
            int halfMaxWithSlack = halfMaxValue - (halfMaxValue / 2); //in case there isnt a row with exact value as halfMaxValue
            //Search upper limit
            upperLimit = 0;
            for (int y = maxRow; y >= 0; y--) if (sfr[y] == halfMaxValue || ((sfr[y] > halfMaxWithSlack) && (sfr[y] < halfMaxValue)))
                {
                    upperLimit = y - 1; //upper limit found
                    break;
                }

            //Search lower limit
            lowerLimit = height - 1;
            for (int y = maxRow; y < height; y++) if (sfr[y] == halfMaxValue || ((sfr[y] > halfMaxWithSlack) && (sfr[y] < halfMaxValue)))
                {
                    lowerLimit = y + 1; //lower limit found
                    break;
                }

        }

        /// <summary>
        /// Locates the left limit of the license plate
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
                    if (pixelPtr[0] == 255) sfc[x]++;
                }
                if (sfc[x] > sfc[maxColumn]) maxColumn = x; //new max found
            }

            //Search left limit
            leftLimit = 0;
            for (int x = maxColumn; x < width; x++) if (sfc[x] == 0) { leftLimit = x; break; }

        }

        /// <summary>
        /// Binarizes an image based on its blue levels
        /// </summary>
        /// <param name="img">The image</param>
        unsafe private static void EnhanceBlue(Image<Bgr, byte> img)
        {
            // direct access to the image memory(sequencial)
            // direction top left -> bottom right

            MIplImage m = img.MIplImage;
            byte* dataPtr = (byte*)m.imageData.ToPointer(); // Pointer to the image
            byte bin;

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
                    if (red + green > blue) bin = 0;
                    else bin = 255;

                    // stores the new values in the image
                    dataPtr[0] = dataPtr[1] = dataPtr[2] = bin;

                    // moves the pointer to the next pixel
                    dataPtr += nChan;
                }

                //at the end of the line moves the pointer by the alignment bytes (padding)
                dataPtr += padding;
            }

        }

        /// <summary>
        /// Locates the right limit of the license plate
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
                    if (pixelPtr[0] == 255) sfc[x]++;
                }
            }

            //Search right limit
            rightLimit = 0;
            for (int x = leftLimit; x < width; x++)
                if (sfc[x] == 0)
                {
                    rightLimit = x;
                    break;
                }
        }

        /// <summary>
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
        unsafe private static void Locate_LP_Chars(Image<Bgr, byte> img, Rectangle LP_Location, out Rectangle LP_Chr1,
            out Rectangle LP_Chr2, out Rectangle LP_Chr3, out Rectangle LP_Chr4, out Rectangle LP_Chr5, out Rectangle LP_Chr6)
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
                if (sfc[x] < min && sfc[x] != 0) min = sfc[x]; //new min found

            }

            //Crop lower levels in order to separate chars
            for (int i = 0; i < sfc.Length; i++) if (sfc[i] <= (min * 2)) sfc[i] = 0;


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
                    if (sfc[i] != 0) /*inside object*/ length++;

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
                        if (objCount == 6) break; //only looking for 6 chars

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
        /// Trims the top and bottom sections where a character is located
        /// </summary>
        /// <param name="img">The image</param>
        /// <param name="LP_UpperLimit">License plate upper limit</param>
        /// <param name="LP_LowerLimit">License plate lower limit</param>
        /// <param name="charX">Character top-left corner x coord</param>
        /// <param name="charY">Character top-left corner y coord</param>
        /// <param name="charWidth">Character width</param>
        /// <param name="charHeight">Character height</param>
        unsafe private static void TrimCharArea(Image<Bgr, byte> img, int LP_UpperLimit, int LP_LowerLimit, int charX,
            out int charY, int charWidth, out int charHeight)
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
        /// Recognizes the characters in a license plate
        /// </summary>
        /// <param name="img">The image</param>
        /// <param name="LP_Chr1">The location of the first character</param>
        /// <param name="LP_Chr2">The location of the second character</param>
        /// <param name="LP_Chr3">The location of the third character</param>
        /// <param name="LP_Chr4">The location of the fourth character</param>
        /// <param name="LP_Chr5">The location of the fifth character</param>
        /// <param name="LP_Chr6">The location of the sixth character</param>
        /// <param name="charList">The character database for the recognition</param>
        /// <param name="LP_C1">The value of the first character</param>
        /// <param name="LP_C2">The value of the second character</param>
        /// <param name="LP_C3">The value of the third character</param>
        /// <param name="LP_C4">The value of the fourth character</param>
        /// <param name="LP_C5">The value of the fifth character</param>
        /// <param name="LP_C6">The value of the sixth character</param>
        private static void CharacterRecognition(Image<Bgr, byte> img, Rectangle LP_Chr1, Rectangle LP_Chr2, Rectangle LP_Chr3, Rectangle LP_Chr4,
            Rectangle LP_Chr5, Rectangle LP_Chr6, Dictionary<char, Image<Bgr, byte>> charList, out string LP_C1, out string LP_C2, out string LP_C3,
            out string LP_C4, out string LP_C5, out string LP_C6)
        {
            img.ROI = LP_Chr1;
            Image<Bgr, byte> char1Img = img.Copy();
            img.ROI = LP_Chr2;
            Image<Bgr, byte> char2Img = img.Copy();
            img.ROI = LP_Chr3;
            Image<Bgr, byte> char3Img = img.Copy();
            img.ROI = LP_Chr4;
            Image<Bgr, byte> char4Img = img.Copy();
            img.ROI = LP_Chr5;
            Image<Bgr, byte> char5Img = img.Copy();
            img.ROI = LP_Chr6;
            Image<Bgr, byte> char6Img = img.Copy();
            img.ROI = Rectangle.Empty;

            //binarizes each character
            ConvertToBW_Otsu(char1Img);
            ConvertToBW_Otsu(char2Img);
            ConvertToBW_Otsu(char3Img);
            ConvertToBW_Otsu(char4Img);
            ConvertToBW_Otsu(char5Img);
            ConvertToBW_Otsu(char6Img);

            //finds the best letter and number match for each character
            //first index is the letter, second index is the number
            KeyValuePair<char, int>[] k1 = FindBestMatches(charList, char1Img);
            KeyValuePair<char, int>[] k2 = FindBestMatches(charList, char2Img);
            KeyValuePair<char, int>[] k3 = FindBestMatches(charList, char3Img);
            KeyValuePair<char, int>[] k4 = FindBestMatches(charList, char4Img);
            KeyValuePair<char, int>[] k5 = FindBestMatches(charList, char5Img);
            KeyValuePair<char, int>[] k6 = FindBestMatches(charList, char6Img);

            //calculates the difference ratings for each pair
            int pair1l = k1[0].Value + k2[0].Value;
            int pair1n = k1[1].Value + k2[1].Value;
            int pair2l = k3[0].Value + k4[0].Value;
            int pair2n = k3[1].Value + k4[1].Value;
            int pair3l = k5[0].Value + k6[0].Value;
            int pair3n = k5[1].Value + k6[1].Value;

            //for each pair, checks if they look more like 2 letters or 2 numbers
            //goes from right to left
            if(pair3l < pair3n)
            { //can only have 1 pair of letters, rest are numbers
                LP_C5 = k5[0].Key.ToString();
                LP_C6 = k6[0].Key.ToString();
                LP_C3 = k3[1].Key.ToString();
                LP_C4 = k4[1].Key.ToString();
                LP_C1 = k1[1].Key.ToString();
                LP_C2 = k2[1].Key.ToString();
            }
            else
            {
                LP_C5 = k5[1].Key.ToString();
                LP_C6 = k6[1].Key.ToString();
                if (pair2l < pair2n)
                { //can only have 1 pair of letters, rest are numbers
                    LP_C3 = k3[0].Key.ToString();
                    LP_C4 = k4[0].Key.ToString();
                    LP_C1 = k1[1].Key.ToString();
                    LP_C2 = k2[1].Key.ToString();
                }
                else
                { //must have 1 pair of letters
                    LP_C3 = k3[1].Key.ToString();
                    LP_C4 = k4[1].Key.ToString();
                    LP_C1 = k1[0].Key.ToString();
                    LP_C2 = k2[0].Key.ToString();
                }
            }

            Console.WriteLine($"License plate: {LP_C1}{LP_C2}-{LP_C3}{LP_C4}-{LP_C5}{LP_C6}");
        }

        /// <summary>
        /// Finds the best letter and number match for a character
        /// </summary>
        /// <param name="charList">The character database</param>
        /// <param name="charImg">The character to recognize</param>
        /// <returns>An array with the best letter match in the first position and the best number match in the second position</returns>
        private static KeyValuePair<char, int>[] FindBestMatches(Dictionary<char, Image<Bgr, byte>> charList, Image<Bgr, byte> charImg)
        {
            Dictionary<char, int> counts = new Dictionary<char, int>();
            Dictionary<char, int> assymsExtUp = new Dictionary<char, int>();
            Dictionary<char, int> assymsExtLow = new Dictionary<char, int>();
            Dictionary<char, int> assymsIntUp = new Dictionary<char, int>();
            Dictionary<char, int> assymsIntLow = new Dictionary<char, int>();
            Dictionary<char, int> assymsExtLeft = new Dictionary<char, int>();
            Dictionary<char, int> assymsExtRight = new Dictionary<char, int>();
            Dictionary<char, int> assymsIntLeft = new Dictionary<char, int>();
            Dictionary<char, int> assymsIntRight = new Dictionary<char, int>();

            //calculates the differences between our character and each character of the database
            foreach (Image<Bgr, byte> i in charList.Values)
            {
                char c = charList.First(x => x.Value.Equals(i)).Key;

                int charCount = CountPixels(charImg);
                int iCount = CountPixels(i);

                int charAssymExtUp = ExternalUpperAsymmetry(charImg);
                int iAssymExtUp = ExternalUpperAsymmetry(i);

                int charAssymExtLow = ExternalLowerAsymmetry(charImg);
                int iAssymExtLow = ExternalLowerAsymmetry(i);

                int charAssymIntUp = InternalUpperAsymmetry(charImg);
                int iAssymIntUp = InternalUpperAsymmetry(i);

                int charAssymIntLow = InternalLowerAsymmetry(charImg);
                int iAssymIntLow = InternalLowerAsymmetry(i);

                int charAssymExtLeft = ExternalLeftAsymmetry(charImg);
                int iAssymExtLeft = ExternalLeftAsymmetry(i);

                int charAssymExtRight = ExternalRightAsymmetry(charImg);
                int iAssymExtRight = ExternalRightAsymmetry(i);

                int charAssymIntLeft = InternalLeftAsymmetry(charImg);
                int iAssymIntLeft = InternalLeftAsymmetry(i);

                int charAssymIntRight = InternalRightAsymmetry(charImg);
                int iAssymIntRight = InternalRightAsymmetry(i);

                int countDiff = Math.Abs(charCount - iCount);
                int assymExtUpDiff = Math.Abs(charAssymExtUp - iAssymExtUp);
                int assymExtLowDiff = Math.Abs(charAssymExtLow - iAssymExtLow);
                int assymIntUpDiff = Math.Abs(charAssymIntUp - iAssymIntUp);
                int assymIntLowDiff = Math.Abs(charAssymIntLow - iAssymIntLow);
                int assymDiffExtLeft = Math.Abs(charAssymExtLeft - iAssymExtLeft);
                int assymExtRightDiff = Math.Abs(charAssymExtRight - iAssymExtRight);
                int assymIntLeftDiff = Math.Abs(charAssymIntLeft - iAssymIntLeft);
                int assymIntRightDiff = Math.Abs(charAssymIntRight - iAssymIntRight);

                counts.Add(c, countDiff);
                assymsExtUp.Add(c, assymExtUpDiff);
                assymsExtLow.Add(c, assymExtLowDiff);
                assymsIntUp.Add(c, assymIntUpDiff);
                assymsIntLow.Add(c, assymIntLowDiff);
                assymsExtLeft.Add(c, assymDiffExtLeft);
                assymsExtRight.Add(c, assymExtRightDiff);
                assymsIntLeft.Add(c, assymIntLeftDiff);
                assymsIntRight.Add(c, assymIntRightDiff);
            }

            //sorts each difference list in descending order
            var countsOrd = counts.OrderBy(x => x.Value);
            var assymsExtUpOrd = assymsExtUp.OrderBy(x => x.Value);
            var assymsExtLowOrd = assymsExtLow.OrderBy(x => x.Value);
            var assymsIntUpOrd = assymsIntUp.OrderBy(x => x.Value);
            var assymsIntLowOrd = assymsIntLow.OrderBy(x => x.Value);
            var assymsExtLeftOrd = assymsExtLeft.OrderBy(x => x.Value);
            var assymsExtRightOrd = assymsExtRight.OrderBy(x => x.Value);
            var assymsIntLeftOrd = assymsIntLeft.OrderBy(x => x.Value);
            var assymsIntRightOrd = assymsIntRight.OrderBy(x => x.Value);

            SortedList<char, int> list = new SortedList<char, int>();
            //gets the index of each database character in each list and adds them
            foreach (char c in charList.Keys)
            {
                int i = countsOrd.TakeWhile(x => x.Key != c).Count() + assymsExtUpOrd.TakeWhile(x => x.Key != c).Count()
                + assymsExtLowOrd.TakeWhile(x => x.Key != c).Count() + assymsIntUpOrd.TakeWhile(x => x.Key != c).Count()
                + assymsIntLowOrd.TakeWhile(x => x.Key != c).Count() + assymsExtLeftOrd.TakeWhile(x => x.Key != c).Count()
                + assymsExtRightOrd.TakeWhile(x => x.Key != c).Count() + assymsIntLeftOrd.TakeWhile(x => x.Key != c).Count()
                + assymsIntRightOrd.TakeWhile(x => x.Key != c).Count();
                list.Add(c, i);
            }

            //picks the letter and number with the smallest index sum
            KeyValuePair<char, int> kvpl = list.OrderBy(x => x.Value).First(x => x.Key > 58);
            KeyValuePair<char, int> kvpn = list.OrderBy(x => x.Value).First(x => x.Key < 58);

            return new KeyValuePair<char, int>[] { kvpl, kvpn };
        }

        /// <summary>
        /// Calculates the external assymetry for the upper half of a character
        /// </summary>
        /// <param name="img">The character</param>
        /// <returns>The value of the upper half external assymetry</returns>
        unsafe private static int ExternalUpperAsymmetry(Image<Bgr, byte> img)
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

        /// <summary>
        /// Calculates the external assymetry for the lower half of a character
        /// </summary>
        /// <param name="img">The character</param>
        /// <returns>The value of the lower half external assymetry</returns>
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

        /// <summary>
        /// Calculates the internal assymetry for the upper half of a character
        /// </summary>
        /// <param name="img">The character</param>
        /// <returns>The value of the upper half internal assymetry</returns>
        unsafe private static int InternalUpperAsymmetry(Image<Bgr, byte> img)
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

        /// <summary>
        /// Calculates the internal assymetry for the lower half of a character
        /// </summary>
        /// <param name="img">The character</param>
        /// <returns>The value of the lower half internal assymetry</returns>
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

        /// <summary>
        /// Calculates the external assymetry for the left half of a character
        /// </summary>
        /// <param name="img">The character</param>
        /// <returns>The value of the left half external assymetry</returns>
        unsafe private static int ExternalLeftAsymmetry(Image<Bgr, byte> img)
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

        /// <summary>
        /// Calculates the external assymetry for the right half of a character
        /// </summary>
        /// <param name="img">The character</param>
        /// <returns>The value of the right half external assymetry</returns>
        unsafe private static int ExternalRightAsymmetry(Image<Bgr, byte> img)
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

        /// <summary>
        /// Calculates the internal assymetry for the left half of a character
        /// </summary>
        /// <param name="img">The character</param>
        /// <returns>The value of the left half internal assymetry</returns>
        unsafe private static int InternalLeftAsymmetry(Image<Bgr, byte> img)
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

        /// <summary>
        /// Calculates the internal assymetry for the right half of a character
        /// </summary>
        /// <param name="img">The character</param>
        /// <returns>The value of the right half internal assymetry</returns>
        unsafe private static int InternalRightAsymmetry(Image<Bgr, byte> img)
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

        /// <summary>
        /// Counts the number of black pixels in an image
        /// </summary>
        /// <param name="img">The image</param>
        /// <returns>The number of black pixels in the image</returns>
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