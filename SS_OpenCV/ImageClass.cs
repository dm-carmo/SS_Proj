using System;
using System.Collections.Generic;
using System.Text;
using Emgu.CV.Structure;
using Emgu.CV;

namespace SS_OpenCV
{
    class ImageClass
    {

        /// <summary>
        /// Image Negative
        /// </summary>
        /// <param name="img">Image</param>
        internal static void Negative(Image<Bgr, byte> img)
        {
            unsafe
            {
                // direct access to the image memory(sequencial)
                // direcion top left -> bottom right

                MIplImage m = img.MIplImage;
                byte* dataPtr = (byte*)m.imageData.ToPointer(); // Pointer to the image
                byte blue, green, red;

                int width = img.Width;
                int height = img.Height;
                int nChan = m.nChannels; // number of channels - 3
                int padding = m.widthStep - m.nChannels * m.width; // alinhament bytes (padding)
                int x, y;

                if (nChan == 3) // image in RGB
                {
                    for (y = 0; y < img.Height; y++)
                    {
                        for (x = 0; x < img.Width; x++)
                        {
                            //obtém as 3 componentes
                            blue = dataPtr[0];
                            green = dataPtr[1];
                            red = dataPtr[2];

                            // store in the image
                            dataPtr[0] = (byte)(255 - blue);
                            dataPtr[1] = (byte)(255 - green);
                            dataPtr[2] = (byte)(255 - red);

                            // advance the pointer to the next pixel
                            dataPtr += nChan;
                        }

                        //at the end of the line advance the pointer by the aligment bytes (padding)
                        dataPtr += padding;
                    }
                }
            }
        }

        internal static void BrightCont(Image<Bgr, byte> img, int brilho, float cont)
        {
            unsafe
            {
                // direct access to the image memory(sequencial)
                // direcion top left -> bottom right

                MIplImage m = img.MIplImage;
                byte* dataPtr = (byte*)m.imageData.ToPointer(); // Pointer to the image
                int blue, green, red;

                int width = img.Width;
                int height = img.Height;
                int nChan = m.nChannels; // number of channels - 3
                int padding = m.widthStep - m.nChannels * m.width; // alinhament bytes (padding)
                int x, y;

                if (nChan == 3) // image in RGB
                {
                    for (y = 0; y < img.Height; y++)
                    {
                        for (x = 0; x < img.Width; x++)
                        {
                            //obtém as 3 componentes
                            blue = (int)(dataPtr[0] * cont + brilho);
                            green = (int)(dataPtr[1] * cont + brilho);
                            red = (int)(dataPtr[2] * cont + brilho);

                            if (blue > 255) blue = 255;
                            if (green > 255) green = 255;
                            if (red > 255) red = 255;

                            if (blue < 0) blue = 0;
                            if (red < 0) red = 0;
                            if (green < 0) green = 0;

                            // store in the image
                            dataPtr[0] = (byte)(blue);
                            dataPtr[1] = (byte)(green);
                            dataPtr[2] = (byte)(red);

                            // advance the pointer to the next pixel
                            dataPtr += nChan;
                        }

                        //at the end of the line advance the pointer by the aligment bytes (padding)
                        dataPtr += padding;
                    }
                }
            }
        }


        /// <summary>
        /// Convert to gray
        /// Direct access to memory
        /// </summary>
        /// <param name="img">image</param>
        internal static void ConvertToGray(Image<Bgr, byte> img, char mode)
        {
            unsafe
            {
                // direct access to the image memory(sequencial)
                // direcion top left -> bottom right

                MIplImage m = img.MIplImage;
                byte* dataPtr = (byte*)m.imageData.ToPointer(); // Pointer to the image
                byte blue, green, red, gray;

                int width = img.Width;
                int height = img.Height;
                int nChan = m.nChannels; // number of channels - 3
                int padding = m.widthStep - m.nChannels * m.width; // alinhament bytes (padding)
                int x, y;

                if (nChan == 3) // image in RGB
                {
                    for (y = 0; y < height; y++)
                    {
                        for (x = 0; x < width; x++)
                        {
                            //obtém as 3 componentes
                            blue = dataPtr[0];
                            green = dataPtr[1];
                            red = dataPtr[2];

                            // convert to gray
                            switch (mode) {
                                case 'M': gray = (byte)(((int)blue + green + red) / 3); break;
                                case 'R': gray = (byte)red; break;
                                case 'G': gray = (byte)green; break;
                                case 'B': gray = (byte)blue; break;
                                default: gray = 0; break;
                            }

                            // store in the image
                            dataPtr[0] = gray;
                            dataPtr[1] = gray;
                            dataPtr[2] = gray;

                            // advance the pointer to the next pixel
                            dataPtr += nChan;
                        }

                        //at the end of the line advance the pointer by the aligment bytes (padding)
                        dataPtr += padding;
                    }
                }
            }
        }

        internal static void Translate(Image<Bgr, byte> img, int dx, int dy)
        {
            unsafe
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
                    for(int x = 0; x < width; x++)
                    {
                        byte* orig = dataPtrCopy + (y - dy) * widthstep + (x - dx) * nC;
                        byte* dest = dataPtr + y * widthstep + x * nC;

                        if (y - dy > 0 && y - dy < height && x - dx > 0 && x - dx < width)
                        {
                            dest[0] = orig[0];
                            dest[1] = orig[1];
                            dest[2] = orig[2];
                        }

                        else
                        {
                            dest[0] = 0;
                            dest[1] = 0;
                            dest[2] = 0;
                        }

                    }
                }
            }
        }

        internal static void Rotate(Image<Bgr, byte> img, int ang)
        {
            unsafe
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
                        int nx = (int)((x - width / 2) * cose - (height / 2 - y) * sine + width / 2);
                        int ny = (int)(height / 2 - (x - width / 2) * sine - (height / 2 - y) * cose);
                        byte* orig = dataPtrCopy + ny * widthstep + nx * nC;
                        byte* dest = dataPtr + y * widthstep + x * nC;

                        if (ny > 0 && ny < height && nx > 0 && nx < width)
                        {
                            dest[0] = orig[0];
                            dest[1] = orig[1];
                            dest[2] = orig[2];
                        }

                        else
                        {
                            dest[0] = 0;
                            dest[1] = 0;
                            dest[2] = 0;
                        }

                    }
                }
            }
        }

        internal static void Zoom(Image<Bgr, byte> img, double factor, int mouseX, int mouseY)
        {
            if (factor == 1) return;
            unsafe
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
                        int ny = (int)((mouseY + y) / factor);
                        int nx = (int)((mouseX + x) / factor);
                        byte* orig = dataPtrCopy + ny * widthstep + nx * nC;
                        byte* dest = dataPtr + y * widthstep + x * nC;

                        if (ny > 0 && ny < height && nx > 0 && nx < width)
                        {
                            dest[0] = orig[0];
                            dest[1] = orig[1];
                            dest[2] = orig[2];
                        }

                        else
                        {
                            dest[0] = 0;
                            dest[1] = 0;
                            dest[2] = 0;
                        }
                    }
                }
            }
        }
    }
}
