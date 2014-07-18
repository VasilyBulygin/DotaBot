using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace bot
{
    /// <summary>
    /// This class shall keep the GDI32 APIs used in our program.
    /// </summary>
    public class PlatformInvokeGDI32
    {
        #region Class Variables
        public const int SRCCOPY = 13369376;
        #endregion
        #region Class Functions<br>
        [DllImport("gdi32.dll", EntryPoint = "DeleteDC")]
        public static extern IntPtr DeleteDC(IntPtr hDc);

        [DllImport("gdi32.dll", EntryPoint = "DeleteObject")]
        public static extern IntPtr DeleteObject(IntPtr hDc);

        [DllImport("gdi32.dll", EntryPoint = "BitBlt")]
        public static extern bool BitBlt(IntPtr hdcDest, int xDest,
            int yDest, int wDest, int hDest, IntPtr hdcSource,
            int xSrc, int ySrc, int RasterOp);

        [DllImport("gdi32.dll", EntryPoint = "CreateCompatibleBitmap")]
        public static extern IntPtr CreateCompatibleBitmap(IntPtr hdc,
            int nWidth, int nHeight);

        [DllImport("gdi32.dll", EntryPoint = "CreateCompatibleDC")]
        public static extern IntPtr CreateCompatibleDC(IntPtr hdc);

        [DllImport("gdi32.dll", EntryPoint = "SelectObject")]
        public static extern IntPtr SelectObject(IntPtr hdc, IntPtr bmp);
        #endregion

    }

    /// <summary>
    /// This class shall keep the User32 APIs used in our program.
    /// </summary>
    public class PlatformInvokeUSER32
    {
        #region Class Variables
        public const int SM_CXSCREEN = 0;
        public const int SM_CYSCREEN = 1;
        #endregion

        #region Class Functions
        [DllImport("user32.dll", EntryPoint = "GetDesktopWindow")]
        public static extern IntPtr GetDesktopWindow();

        [DllImport("user32.dll", EntryPoint = "GetDC")]
        public static extern IntPtr GetDC(IntPtr ptr);

        [DllImport("user32.dll", EntryPoint = "GetSystemMetrics")]
        public static extern int GetSystemMetrics(int abc);

        [DllImport("user32.dll", EntryPoint = "GetWindowDC")]
        public static extern IntPtr GetWindowDC(Int32 ptr);

        [DllImport("user32.dll", EntryPoint = "ReleaseDC")]
        public static extern IntPtr ReleaseDC(IntPtr hWnd, IntPtr hDc);

        #endregion
    }

    /// <summary>
    /// This class shall keep all the functionality 
    /// for capturing the desktop.
    /// </summary>
    public class CaptureScreen
    {
        #region Class Variable Declaration
        protected static IntPtr m_HBitmap;
        #endregion

        ///
        /// This class shall keep all the functionality for capturing
        /// the desktop.
        ///
            public static Bitmap GetDesktopImage(IntPtr hwnd, int x1, int y1, int x2, int y2)
            {
                SIZE size;
                IntPtr hBitmap;
                IntPtr hDC = PlatformInvokeUSER32.GetDC(hwnd);
                IntPtr hMemDC = PlatformInvokeGDI32.CreateCompatibleDC(hDC);
                size.cx = x2;
                size.cy = y2;
                hBitmap = PlatformInvokeGDI32.CreateCompatibleBitmap
                            (hDC, size.cx, size.cy);
                if (hBitmap != IntPtr.Zero)
                {
                    IntPtr hOld = (IntPtr)PlatformInvokeGDI32.SelectObject(hMemDC, hBitmap);
                    PlatformInvokeGDI32.BitBlt(hMemDC, 0, 0, size.cx, size.cy, hDC, x1, y1, PlatformInvokeGDI32.SRCCOPY);
                    PlatformInvokeGDI32.SelectObject(hMemDC, hOld);
                    //We delete the memory device context.
                    PlatformInvokeGDI32.DeleteDC(hMemDC);
                    //We release the screen device context.
                    PlatformInvokeUSER32.ReleaseDC(hwnd, hDC);
                    //Image is created by Image bitmap handle and stored in
                    //local variable.
                    Bitmap bmp = System.Drawing.Image.FromHbitmap(hBitmap);
                    //Release the memory to avoid memory leaks.
                    PlatformInvokeGDI32.DeleteObject(hBitmap);
                    //This statement runs the garbage collector manually.
                    GC.Collect();
                    //Return the bitmap 
                    return bmp;
                }
                //If hBitmap is null, retun null.
                return null;
            }
        }

        //This structure shall be used to keep the size of the screen.
        public struct SIZE
        {
            public int cx;
            public int cy;
        }
}