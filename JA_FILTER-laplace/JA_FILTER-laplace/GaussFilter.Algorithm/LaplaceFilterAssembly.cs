using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using GaussFilter.ProgressNotifier;

namespace GaussFilter.Algorithm
{
    public class LaplaceFilterAssembly
    {
        private const int BYTES_IN_PIXEL = 4;
        private readonly Bitmap image;
        private readonly Bitmap filteredImage;
        private readonly int[] _mask;
        
        public unsafe LaplaceFilterAssembly(Bitmap image)
        {
            _mask = new int[] {
                        -1, -1, -1,
                        -1,  8, -1,
                        -1, -1, -1 };
            this.image = image;
            filteredImage = image.Clone() as Bitmap;
        }

        [DllImport("GaussFilter.Algorithm.ASM.dll", EntryPoint = "laplace")]
        private static extern unsafe int ApplyLaplaceAsm(int startingSubpixelIndex, int subpixelsToFilter, byte* original, byte* filtered, int* mask, int subpixelImageWidth);

        public unsafe Bitmap ApplyAssemblyFilter(Bitmap _bitmap)
        {
            Bitmap bitmap = (Bitmap)_bitmap.Clone();
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

            return bitmap;

        }


        private unsafe void Filter(byte* original, byte* filtered)
        {
            fixed (int* mask = _mask)
            {
                var startingSubpixelIndex = image.Width * 4 + 4;
                var imageSubpixelWidth = image.Width * 4;
                var imageSubpixelWidthEditable = imageSubpixelWidth - 8;
                for (int y = 2; y < image.Height; y++)
                {
                    ApplyLaplaceAsm(startingSubpixelIndex, imageSubpixelWidthEditable, original, filtered, mask, imageSubpixelWidth);
                    startingSubpixelIndex += imageSubpixelWidth;
                }
            }
        }

        private event EventHandler<ProgressNotifierEventArgs> ProgressChanged;

        public void OnProgressChanged(float newValue)
        {
            ProgressChanged?.Invoke(this, new ProgressNotifierEventArgs(newValue));
        }


    }
}
