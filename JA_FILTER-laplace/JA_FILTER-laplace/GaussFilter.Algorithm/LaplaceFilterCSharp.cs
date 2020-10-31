using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using GaussFilter.ProgressNotifier;

namespace GaussFilter.Algorithm
{
    public class LaplaceFilterCSharp
    {
        private const int BYTES_IN_PIXEL = 4;
        private readonly int[] _laplaceMask;
        private readonly Action<float> _dispatcher;
        private int _bitmapLastIndex;

        public LaplaceFilterCSharp(int[] laplaceMask, Bitmap image, Action<float> dispatcher)
        {
            _bitmapLastIndex = image.Width * image.Height * BYTES_IN_PIXEL;
            _laplaceMask = laplaceMask;
            Image = image;
            _dispatcher = dispatcher;
            FilteredImage = image.Clone() as Bitmap;
            ProgressChanged += GaussFilter_ProgressChanged;
        }

        private void GaussFilter_ProgressChanged(object sender, ProgressNotifierEventArgs e)
        {
            _dispatcher.Invoke(e.Percentage);
        }


        public double GaussRadius { get; }
        public Bitmap Image { get; private set; }
        public Bitmap FilteredImage { get; private set; }

        public unsafe void ApplyUnsafe()
        {
            Bitmap bitmap = (Bitmap)Image.Clone();
            int dataArraySize = BYTES_IN_PIXEL * bitmap.Width * bitmap.Height;
            BitmapData data = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            byte* original = (byte*)data.Scan0;

            byte[] filtered = new byte[dataArraySize];
            fixed (byte* filteredPtr = filtered)
            {

                Filter(original, filteredPtr);
                Marshal.Copy(filtered, 0, (IntPtr)original, dataArraySize);
                bitmap.UnlockBits(data);
            }

            FilteredImage = bitmap;
        }

        private unsafe void Filter(byte* original, byte* filtered)
        {
            const int boundPixelWidth = 1;
            int arraySize = Image.Width * Image.Height;
            int boundTopBottomArraySize = Image.Width * boundPixelWidth;
            int bottomBoundStartIndex = (arraySize - boundTopBottomArraySize);

            //  Set top and bottom bound
            for (int i = 0; i < boundTopBottomArraySize; i++)
            {
                //Top bound R G B A
                filtered[i * BYTES_IN_PIXEL] = 0;
                filtered[i * BYTES_IN_PIXEL + 1] = 0;
                filtered[i * BYTES_IN_PIXEL + 2] = 0;
                filtered[i * BYTES_IN_PIXEL + 3] = 0;
                //Bot bound R G B A
                filtered[(bottomBoundStartIndex + i) * BYTES_IN_PIXEL] = 0;
                filtered[(bottomBoundStartIndex + i) * BYTES_IN_PIXEL + 1] = 0;
                filtered[(bottomBoundStartIndex + i) * BYTES_IN_PIXEL + 2] = 0;
                filtered[(bottomBoundStartIndex + i) * BYTES_IN_PIXEL + 3] = 0;
            }

            int index = boundTopBottomArraySize;
            int imageWidth = (Image.Width - 2 * boundPixelWidth);
            while (index < bottomBoundStartIndex)
            {

                //Set start of the row
                for (int i = 0; i < boundPixelWidth; i++)
                {
                    var subPixelIndex = (index + i) * BYTES_IN_PIXEL;
                    //R
                    filtered[subPixelIndex] = 0;
                    //G
                    filtered[subPixelIndex + 1] = 0;
                    //B
                    filtered[subPixelIndex + 2] = 0;
                    //A
                    filtered[subPixelIndex + 3] = 0;
                }
                index += boundPixelWidth;

                //Apply gauss filter
                for (int i = 0; i < imageWidth; i++)
                {
                    ApplyFilterOnPixelUnsafe(original, filtered, Image.Width, index++);
                }

                //Set end of the row
                for (int i = 0; i < boundPixelWidth; i++)
                {
                    var subPixelIndex = (index + i) * BYTES_IN_PIXEL;

                    //R
                    filtered[subPixelIndex] = 0;
                    //G
                    filtered[subPixelIndex + 1] = 0;
                    //B
                    filtered[subPixelIndex + 2] = 0;
                    //B
                    filtered[subPixelIndex + 3] = 0;
                }
                //Should it be here???
                index += boundPixelWidth;

                OnProgressChanged((float)index / (float)_bitmapLastIndex);

            }
        }

        private unsafe void ApplyFilterOnPixelUnsafe(byte* original, byte* filtered, int arrayWidth, int index)
        {
            const int maskRadius = 3;
            const int maskRadiusFromCenter = 1;
            double R = 0d;
            double G = 0d;
            double B = 0d;
            double A = 0d;
            int maskIndexCounter = 0;
            int pixelStartingIndex;
            var minLocationY = (index / arrayWidth) - maskRadiusFromCenter;
            var minLocationX = (index % arrayWidth) - maskRadiusFromCenter;

            for (int y = minLocationY; y < minLocationY + maskRadius; y++)
            {
                for (int x = minLocationX; x < minLocationX + maskRadius; x++)
                {
                    pixelStartingIndex = (x + y * arrayWidth) * BYTES_IN_PIXEL;
                    //R
                    R += original[pixelStartingIndex++] * _laplaceMask[maskIndexCounter];
                    //G
                    G += original[pixelStartingIndex++] * _laplaceMask[maskIndexCounter];
                    //B
                    B += original[pixelStartingIndex++] * _laplaceMask[maskIndexCounter];
                    //A
                    A += original[pixelStartingIndex] * _laplaceMask[maskIndexCounter];
                    maskIndexCounter++;
                }
            }

            R = Math.Min(255, Math.Max(0, R));
            G = Math.Min(255, Math.Max(0, G));
            B = Math.Min(255, Math.Max(0, B));
            A = Math.Min(255, Math.Max(0, A));

            //Set R
            filtered[index * BYTES_IN_PIXEL] = (byte)(R);
            //Set G
            filtered[index * BYTES_IN_PIXEL + 1] = (byte)(G);
            //Set B
            filtered[index * BYTES_IN_PIXEL + 2] = (byte)(B);
            //Set A
            filtered[index * BYTES_IN_PIXEL + 3] = (byte)(A);
        }

        private event EventHandler<ProgressNotifierEventArgs> ProgressChanged;

        public void OnProgressChanged(float newValue)
        {
            ProgressChanged?.Invoke(this, new ProgressNotifierEventArgs(newValue));
        }



    }
}
