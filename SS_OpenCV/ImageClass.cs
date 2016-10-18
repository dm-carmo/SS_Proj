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
                            switch (mode)
                            {
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
                    for (int x = 0; x < width; x++)
                    {
                        byte* orig = dataPtrCopy + (y - dy) * widthstep + (x - dx) * nC;
                        byte* dest = dataPtr + y * widthstep + x * nC;

                        if (y - dy >= 0 && y - dy < height && x - dx >= 0 && x - dx < width)
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

                        if (ny >= 0 && ny < height && nx >= 0 && nx < width)
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

        internal static void Zoom(Image<Bgr, byte> img, float factor, int mouseX, int mouseY)
        {
            if (factor == 1 || factor < 0) return;
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

        internal static void MeanReduct3(Image<Bgr, byte> img)
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

                for (int y = 1; y < height - 1; y++)
                {
                    for (int x = 1; x < width - 1; x++)
                    {
                        byte* dest = dataPtr + y * widthstep + x * nC;
                        int[] sum = { 0, 0, 0 };
                        for (int j = -1; j <= 1; j++)
                        {
                            for (int i = -1; i <= 1; i++)
                            {
                                byte* orig = dataPtrCopy + (y + j) * widthstep + (x + i) * nC;
                                sum[0] += orig[0];
                                sum[1] += orig[1];
                                sum[2] += orig[2];
                            }
                        }
                        dest[0] = (byte)(sum[0] / 9);
                        dest[1] = (byte)(sum[1] / 9);
                        dest[2] = (byte)(sum[2] / 9);
                    }
                }

                //cantos
                //Superior esquerdo
                dataPtr[0] = (byte)((4 * dataPtrCopy[0] + 2 * (dataPtrCopy + widthstep)[0] + 2 * (dataPtrCopy + nC)[0] + (dataPtrCopy + widthstep + nC)[0]) / 9);
                dataPtr[1] = (byte)((4 * dataPtrCopy[1] + 2 * (dataPtrCopy + widthstep)[1] + 2 * (dataPtrCopy + nC)[1] + (dataPtrCopy + widthstep + nC)[1]) / 9);
                dataPtr[2] = (byte)((4 * dataPtrCopy[2] + 2 * (dataPtrCopy + widthstep)[2] + 2 * (dataPtrCopy + nC)[2] + (dataPtrCopy + widthstep + nC)[2]) / 9);

                //Superior direito
                byte* corner = dataPtr + (width - 1) * nC;
                byte* cornerCopy = dataPtrCopy + (width - 1) * nC;

                corner[0] = (byte)((4 * cornerCopy[0] + 2 * (cornerCopy + widthstep)[0] + 2 * (cornerCopy - nC)[0] + (cornerCopy + widthstep - nC)[0]) / 9);
                corner[1] = (byte)((4 * cornerCopy[1] + 2 * (cornerCopy + widthstep)[1] + 2 * (cornerCopy - nC)[1] + (cornerCopy + widthstep - nC)[1]) / 9);
                corner[2] = (byte)((4 * cornerCopy[2] + 2 * (cornerCopy + widthstep)[2] + 2 * (cornerCopy - nC)[2] + (cornerCopy + widthstep - nC)[2]) / 9);

                //Inferior esquerdo
                corner = dataPtr + (height - 1) * widthstep;
                cornerCopy = dataPtrCopy + (height - 1) * widthstep;

                corner[0] = (byte)((4 * cornerCopy[0] + 2 * (cornerCopy - widthstep)[0] + 2 * (cornerCopy + nC)[0] + (cornerCopy - widthstep + nC)[0]) / 9);
                corner[1] = (byte)((4 * cornerCopy[1] + 2 * (cornerCopy - widthstep)[1] + 2 * (cornerCopy + nC)[1] + (cornerCopy - widthstep + nC)[1]) / 9);
                corner[2] = (byte)((4 * cornerCopy[2] + 2 * (cornerCopy - widthstep)[2] + 2 * (cornerCopy + nC)[2] + (cornerCopy - widthstep + nC)[2]) / 9);

                //Inferior direito
                corner = dataPtr + (height - 1) * widthstep + (width - 1) * nC;
                cornerCopy = dataPtrCopy + (height - 1) * widthstep + (width - 1) * nC;

                corner[0] = (byte)((4 * cornerCopy[0] + 2 * (cornerCopy - widthstep)[0] + 2 * (cornerCopy - nC)[0] + (cornerCopy - widthstep - nC)[0]) / 9);
                corner[1] = (byte)((4 * cornerCopy[1] + 2 * (cornerCopy - widthstep)[1] + 2 * (cornerCopy - nC)[1] + (cornerCopy - widthstep - nC)[1]) / 9);
                corner[2] = (byte)((4 * cornerCopy[2] + 2 * (cornerCopy - widthstep)[2] + 2 * (cornerCopy - nC)[2] + (cornerCopy - widthstep - nC)[2]) / 9);

                //margens
                for (int i = 1; i < width - 1; i++)
                {
                    //Margem superior
                    byte* margemSup = dataPtr + i * nC;
                    byte* margemSupCop = dataPtrCopy + i * nC;

                    byte* mSupLeft = margemSupCop - nC;
                    byte* mSupRight = margemSupCop + nC;

                    margemSup[0] = (byte)((2 * margemSupCop[0] + 2 * mSupLeft[0] + 2 * mSupRight[0] + (margemSupCop + widthstep)[0] + (mSupRight + widthstep)[0] + (mSupLeft + widthstep)[0]) / 9);
                    margemSup[1] = (byte)((2 * margemSupCop[1] + 2 * mSupLeft[1] + 2 * mSupRight[1] + (margemSupCop + widthstep)[1] + (mSupRight + widthstep)[1] + (mSupLeft + widthstep)[1]) / 9);
                    margemSup[2] = (byte)((2 * margemSupCop[2] + 2 * mSupLeft[2] + 2 * mSupRight[2] + (margemSupCop + widthstep)[2] + (mSupRight + widthstep)[2] + (mSupLeft + widthstep)[2]) / 9);

                    //Margem inferior
                    byte* margemInf = dataPtr + (height - 1) * widthstep + i * nC;
                    byte* margemInfCop = dataPtrCopy + (height - 1) * widthstep + i * nC;

                    byte* mInfLeft = margemInfCop - nC;
                    byte* mInfRight = margemInfCop + nC;

                    margemInf[0] = (byte)((2 * margemInfCop[0] + 2 * mInfLeft[0] + 2 * mInfRight[0] + (margemInfCop - widthstep)[0] + (mInfRight - widthstep)[0] + (mInfLeft - widthstep)[0]) / 9);
                    margemInf[1] = (byte)((2 * margemInfCop[1] + 2 * mInfLeft[1] + 2 * mInfRight[1] + (margemInfCop - widthstep)[1] + (mInfRight - widthstep)[1] + (mInfLeft - widthstep)[1]) / 9);
                    margemInf[2] = (byte)((2 * margemInfCop[2] + 2 * mInfLeft[2] + 2 * mInfRight[2] + (margemInfCop - widthstep)[2] + (mInfRight - widthstep)[2] + (mInfLeft - widthstep)[2]) / 9);
                }

                for (int i = 1; i < height - 1; i++)
                {
                    //Margem esquerda
                    byte* margemEsq = dataPtr + i * widthstep;
                    byte* margemEsqCop = dataPtrCopy + i * widthstep;

                    byte* mEsqUp = margemEsqCop - widthstep;
                    byte* mEsqDown = margemEsqCop + widthstep;

                    margemEsq[0] = (byte)((2 * margemEsqCop[0] + 2 * mEsqUp[0] + 2 * mEsqDown[0] + (margemEsqCop + nC)[0] + (mEsqDown + nC)[0] + (mEsqUp + nC)[0]) / 9);
                    margemEsq[1] = (byte)((2 * margemEsqCop[1] + 2 * mEsqUp[1] + 2 * mEsqDown[1] + (margemEsqCop + nC)[1] + (mEsqDown + nC)[1] + (mEsqUp + nC)[1]) / 9);
                    margemEsq[2] = (byte)((2 * margemEsqCop[2] + 2 * mEsqUp[2] + 2 * mEsqDown[2] + (margemEsqCop + nC)[2] + (mEsqDown + nC)[2] + (mEsqUp + nC)[2]) / 9);

                    //Margem direita
                    byte* margemDir = dataPtr + (width - 1) * nC + i * widthstep;
                    byte* margemDirCop = dataPtrCopy + (width - 1) * nC + i * widthstep;

                    byte* mDirUp = margemDirCop - widthstep;
                    byte* mDirDown = margemDirCop + widthstep;

                    margemDir[0] = (byte)((2 * margemDirCop[0] + 2 * mDirUp[0] + 2 * mDirDown[0] + (margemDirCop - nC)[0] + (mDirDown - nC)[0] + (mDirUp - nC)[0]) / 9);
                    margemDir[1] = (byte)((2 * margemDirCop[1] + 2 * mDirUp[1] + 2 * mDirDown[1] + (margemDirCop - nC)[1] + (mDirDown - nC)[1] + (mDirUp - nC)[1]) / 9);
                    margemDir[2] = (byte)((2 * margemDirCop[2] + 2 * mDirUp[2] + 2 * mDirDown[2] + (margemDirCop - nC)[2] + (mDirDown - nC)[2] + (mDirUp - nC)[2]) / 9);
                }
            }
        }
    }
}