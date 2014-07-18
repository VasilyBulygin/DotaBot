using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace bot
{
    public class DotaObject
    {
        public string Name { get; set; }
        public int Top { get; set; }
        public int Left { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public Point HitPoint { get; set; }
        public string PicturePath { get; set; }

        public bool Availible(Bitmap scr)
        {
            if (scr != null)
                return Helper.Compare2Bitmaps(Picture, scr.GetBitmapRegion(new Rectangle(this.Top, this.Left, this.Width, this.Height)));
            else
                return false;
        }

        public void Click(IntPtr hwnd, string button, bool post = true, byte times = 1)
        {
            if (post)
                WinAPIExt.PMMouseClick(hwnd, button, HitPoint.X, HitPoint.Y, times);
            else
                WinAPIExt.SMMouseClick(hwnd, button, HitPoint.X, HitPoint.Y, times);
        }


        [XmlIgnore] 
        public Bitmap Picture { get; set; }

        public void LoadPicture()
        {
            try
            {
                var tmp = (Bitmap)Bitmap.FromFile(AppDomain.CurrentDomain.BaseDirectory + "Img/" + PicturePath);
                Picture = tmp.Clone(new Rectangle(0, 0, tmp.Width, tmp.Height), PixelFormat.Format32bppArgb);
            }
            catch
            {
                Picture = new Bitmap(0, 0);
            }
        }
    }
}
