using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace bot
{
    static class WinAPIExt
    {
        public const uint WM_MOUSEMOVE = 0x0200;
        public const uint MK_LBUTTON = 0x0001;
        public const uint WM_LBUTTONDOWN = 0x0201;
        public const uint WM_LBUTTONUP = 0x0202;
        public const uint MK_RBUTTON = 0x0002;
        public const uint WM_RBUTTONDOWN = 0x0204;
        public const uint WM_RBUTTONUP = 0x0205;


        [DllImport("User32.DLL")]
        static extern bool PostMessage(IntPtr hWnd, uint Msg, uint wParam, int lParam);

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, uint Msg, uint wParam, int lParam);

        [DllImport("user32.dll")]
        static extern IntPtr GetDC(IntPtr hWnd);

        [DllImport("gdi32.dll")]
        static extern uint GetPixel(IntPtr hdc, int nXPos, int nYPos);

        [DllImport("user32.dll")]
        static extern bool ReleaseDC(IntPtr hWnd, IntPtr hDC);

        [DllImport("gdi32.dll", EntryPoint = "SelectObject")]
        public static extern System.IntPtr SelectObject(
            [In()] System.IntPtr hdc,
            [In()] System.IntPtr h);

        [DllImport("gdi32.dll", EntryPoint = "DeleteObject")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool DeleteObject(
            [In()] System.IntPtr ho);

        [DllImport("gdi32.dll", EntryPoint = "BitBlt")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool BitBlt(
            [In()] System.IntPtr hdc, int x, int y, int cx, int cy,
            [In()] System.IntPtr hdcSrc, int x1, int y1, uint rop);


        //Клик мышью с помощью PostMessage
        public static void PMMouseClick(IntPtr hwnd, string button, int x, int y, byte times = 1, int delay = 80)
        {
            uint Button = button == "left" ? MK_LBUTTON : MK_RBUTTON;
            uint ButtonUp = button == "left" ? WM_LBUTTONUP : WM_RBUTTONUP;
            uint ButtonDown = button == "left" ? WM_LBUTTONDOWN : WM_RBUTTONDOWN;

            if (hwnd == IntPtr.Zero)
            {
                throw new Exception("Zero pointer");
            }
            int lParam = Helper.MakeLParam(x, y);
            for (int i = 0; i < times; i++)
            {
                PostMessage(hwnd, WM_MOUSEMOVE, 0, lParam);
                PostMessage(hwnd, ButtonDown, Button, lParam);
                PostMessage(hwnd, ButtonUp, 0, lParam);
                Thread.Sleep(delay);
            }
        }

        //Клик мышью с помощью SendMessage
        public static void SMMouseClick(IntPtr hwnd, string button, int x, int y, byte times = 1, int delay = 80)
        {
            uint Button = button == "left" ? MK_LBUTTON : MK_RBUTTON;
            uint ButtonUp = button == "left" ? WM_LBUTTONUP : WM_RBUTTONUP;
            uint ButtonDown = button == "left" ? WM_LBUTTONDOWN : WM_RBUTTONDOWN;
            if (hwnd == IntPtr.Zero)
            {
                throw new Exception("Zero pointer");
            }
            int lParam = Helper.MakeLParam(x, y);
            for (int i = 0; i < times; i++)
            {
                SendMessage(hwnd, WM_MOUSEMOVE, 0, lParam);
                SendMessage(hwnd, ButtonDown, Button, lParam);
                SendMessage(hwnd, ButtonUp, 0, lParam);
                Thread.Sleep(delay);
            }
        }


        public static Bitmap GetPictureByWindowHandle(IntPtr hwnd, int x1, int y1, int x2, int y2)
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

        //This structure shall be used to keep the size of the screen.
        public struct SIZE
        {
            public int cx;
            public int cy;
        }

    }
}
