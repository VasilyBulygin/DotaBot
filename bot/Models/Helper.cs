using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AForge.Imaging;
using AForge.Imaging.Filters;
using System.Diagnostics;

namespace bot
{
    public static class Helper
    {
        private static EuclideanColorFiltering filter = new EuclideanColorFiltering();

        public static int MakeLParam(int LoWord, int HiWord)
        {
            return ((HiWord << 16) | (LoWord & 0xffff));
        }

        public static bool Compare2Bitmaps(Bitmap left, Bitmap right)
        {
            if (object.Equals(left, right))
                return true;
            if (left == null || right == null)
                return false;
            if (!left.Size.Equals(right.Size) || !left.PixelFormat.Equals(right.PixelFormat))
                return false;

            Bitmap leftBitmap = left as Bitmap;
            Bitmap rightBitmap = right as Bitmap;
            if (leftBitmap == null || rightBitmap == null)
                return true;

            #region Code taking more time for comparison

            for (int col = 0; col < left.Width; col++)
            {
                for (int row = 0; row < left.Height; row++)
                {
                    var leftColor = leftBitmap.GetPixel(col, row);
                    var rightColor = rightBitmap.GetPixel(col, row);
                    if (Math.Abs(leftColor.R - rightColor.R) > 10 || Math.Abs(leftColor.G - rightColor.G) > 10 || Math.Abs(leftColor.B - rightColor.B) > 10)
                        return false;
                }
            }

            #endregion

            return true;
        }


        public static Bitmap GetBitmapRegion(this Bitmap bmp, Rectangle rect)
        {
            Bitmap region = new Bitmap(rect.Width, rect.Height);
            using (Graphics g = Graphics.FromImage(region))
            {
                lock (bmp)
                {
                    g.DrawImage(bmp, 0, 0, rect, GraphicsUnit.Pixel);
                }
            }
            return region;
        }

        public static List<HeroOnScreen> FindHeroesOnImage(Bitmap shot)
        {
            var result = new List<HeroOnScreen>();
            var image = shot.GetBitmapRegion(new Rectangle(20, 40, 580, 300));

            filter.CenterColor = new RGB(255, 0, 0);//фильтруем по красному цвету
            filter.Radius = 100;
            filter.ApplyInPlace(image);

            var blobCounter = new BlobCounter();
            //Ищем области с высотой 4 и длиной больше 4
            blobCounter.FilterBlobs = true;
            blobCounter.MinHeight = blobCounter.MaxHeight = 4;
            blobCounter.MinWidth = 4;
            blobCounter.ProcessImage(image);

            var blobs = blobCounter.GetObjectsInformation().Where(x => x.Area > 10 && x.Fullness > 0.9f).ToArray();
            foreach (var blob in blobs)
            {
                result.Add(new HeroOnScreen(){PosX = blob.Rectangle.X + 40, PosY = blob.Rectangle.Y + 40});
            }
            return result;
        }


        #region Adresses
        public static IntPtr getClientAdress()
        {
            Process[] processes = Process.GetProcessesByName("dota");

            foreach (ProcessModule pm in processes[0].Modules)
            {
                if (pm.ModuleName == "client.dll")
                {
                    return pm.BaseAddress;
                }
            }
            return (IntPtr)0x00;
        }


        public static IntPtr getAddressByOffset(Int32 Pointer, Int32[] offsets)
        {
            var VAM = new VAMemory("dota");
            Int32 DestAddress = 0;
            IntPtr currPointer = (IntPtr)Pointer;
            for (int i = 0; i < offsets.Length; i++)
            {
                DestAddress = VAM.ReadInt32(currPointer);
                currPointer = (IntPtr)(DestAddress + offsets[i]);


            }


            return currPointer;
        }
        #endregion
    }
}
