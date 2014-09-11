using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bot
{
    public class DotaItem
    {
        public string Name { get; set; }
        public int Cost { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public string Type { get; set; }

        public void Use(IntPtr hwnd)
        {
            WinAPIExt.PMMouseClick(hwnd, "left", X, Y);
        }
    }
}
