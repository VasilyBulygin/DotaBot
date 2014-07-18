using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bot
{
    public static class Helper
    {
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
    }
}
