using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace bot
{
    public class CommonBotWorker : IDotaBotWorker
    {



        #region Потоки
        private Thread searchButtonThread;
        private Thread pickThread;
        private Thread discButtonThread;
        #endregion

        #region Флаги потоков
        private volatile bool isPickNow = true;
        private volatile bool isSearching = true;
        private volatile bool isSearchingNow = false;
        private volatile bool isDrawCaptured = true;
        #endregion


        private Bitmap frame;

        private ManualResetEvent runningEvent = new ManualResetEvent(false);

        private IntPtr hwnd;


        public CommonBotWorker(IntPtr hwnd)
        {
            this.hwnd = hwnd;
        }

        public void FrameCaptured(Bitmap frame)
        {
            this.frame = frame;
        }


        private void MainWorker(object manualEvent)
        {
            ManualResetEvent ev = (ManualResetEvent)manualEvent;

            searchButtonThread = new Thread(new ParameterizedThreadStart(SearchButton));
            searchButtonThread.Start(manualEvent);

            pickThread = new Thread(new ParameterizedThreadStart(Pick));
            pickThread.Start(manualEvent);

            discButtonThread = new Thread(new ParameterizedThreadStart(DiscButton));
            discButtonThread.Start(manualEvent);

            if (hwnd != IntPtr.Zero)
            {
                while (ev.WaitOne())
                {
                    if (isDrawCaptured)
                    {
                        frame = CaptureScreen.GetDesktopImage((IntPtr)hwnd, 0, 0, 640, 480);
                    }
                    Thread.Sleep(500);
                }
            }
        }

        private void SearchButton(object manualEvent)
        {
            ManualResetEvent ev = (ManualResetEvent)manualEvent;
            while (ev.WaitOne())
            {
                //if (isSearching)//Можно искать игру
                //{
                if (IsSearchButtonAvailible(hwnd))
                {
                    //Есть кнопка поиска
                    Thread.Sleep(2000);
                    _MouseClick(hwnd, "left", 440, 205, 3);
                    AddMsg("IsSearchButtonAvailible");
                    //Отменяем поиск кнопки Pick
                    isPickNow = false;
                    //Устанавливаем флаг - "Сейчас идет поиск игры"
                    // isSearchingNow = true;
                }
                //if (isSearchingNow)
                // {
                if (IsAcceptButtonAvailible(hwnd))
                {
                    AddMsg("IsAcceptButtonAvailible");
                    _MouseClick(hwnd, "left", 372, 354 + 24, 2, 100);
                    Thread.Sleep(200);
                    _MouseClick(hwnd, "left", 372, 354 + 24, 2, 100);
                    //Сейчас может быть пик
                    isPickNow = true;
                    //Поиск пока завершен
                    //isSearchingNow = false;
                }
                // }
                //}
                Thread.Sleep(5000);
            }
        }

        private void Pick(object manualEvent)
        {
            ManualResetEvent ev = (ManualResetEvent)manualEvent;
            while (ev.WaitOne())
            {
                // AddMsg(String.Format("{0}", _GetPixelColor(hwnd, 9, 543)));
                if (isPickNow)
                {
                    if (IsRadomButtonAvailible(hwnd))
                    {
                        AddMsg("IsRadomButtonAvailible");
                        Thread.Sleep(2000);
                        _MouseClick(hwnd, "left", 9, 543, 1, 80);
                        Thread.Sleep(2000);
                        _MouseClick(hwnd, "left", 16, 13, 1, 80);
                        isPickNow = false;
                        isSearching = false;
                        isSearchingNow = false;
                    }
                    Thread.Sleep(5000);
                }
                else
                {
                    Thread.Sleep(10000);
                }
            }
        }


        private void DiscButton(object manualEvent)
        {
            ManualResetEvent ev = (ManualResetEvent)manualEvent;
            while (ev.WaitOne())
            {
                if (IsDiscButtonAvailible(hwnd))
                {
                    AddMsg("IsDiscButtonAvailible");
                    _MouseClick(hwnd, "left", 508, 350, 2, 80);
                    isSearching = true;
                }
                Thread.Sleep(10000);
            }
        }

        private void MainFunc(object manualEvent)
        {
            ManualResetEvent ev = (ManualResetEvent)manualEvent;

            searchButtonThread = new Thread(new ParameterizedThreadStart(SearchButton));
            searchButtonThread.Start(manualEvent);

            pickThread = new Thread(new ParameterizedThreadStart(Pick));
            pickThread.Start(manualEvent);

            discButtonThread = new Thread(new ParameterizedThreadStart(DiscButton));
            discButtonThread.Start(manualEvent);

            if (hwnd != IntPtr.Zero)
            {
                while (ev.WaitOne())
                {
                    if (isDrawCaptured)
                    {
                        currBmp = CaptureScreen.GetDesktopImage((IntPtr)hwnd, 0, 0, 640, 480);
                        SetPic(currBmp);
                    }
                    Thread.Sleep(500);
                }
            }
        }

        private bool IsRadomButtonAvailible(IntPtr hwnd)
        {
            if (_GetPixelColor(hwnd, 9, 543) == 4800311)
                return true;
            return false;
        }

        private bool IsAcceptButtonAvailible(IntPtr hwnd)
        {
            if (_GetPixelColor(hwnd, 372, 354) == 3025702)
                return true;
            return false;
        }

        private bool IsSearchButtonAvailible(IntPtr hwnd)
        {
            if (_GetPixelColor(hwnd, 440, 205) == 2180409)
                return true;
            return false;
        }

        private bool IsDiscButtonAvailible(IntPtr hwnd)
        {
            if (_GetPixelColor(hwnd, 506, 347) == 2499878)
                return true;
            return false;
        }

        private bool IsHeroPick(IntPtr hwnd)
        {
            if (GetSum(hwnd, 808, 743, 819, 749) == 9867413)
                return true;
            return false;
        }
        private uint GetSum(IntPtr hwnd, int x1, int y1, int x2, int y2)
        {
            long result = 0xFFFFFF;
            for (int i = x1; i < x2; i++)
            {
                for (int j = y1; j < y2; j++)
                {
                    result = result ^ _GetPixelColor(hwnd, i, j);
                }
            }
            return (uint)result;
        }

        private uint _GetPixelColor(IntPtr hwnd, int x, int y)
        {
            uint result = 0;
            IntPtr dc = GetDC(hwnd);
            result = GetPixel(dc, x, y);
            if (!ReleaseDC(hwnd, dc))
            {
                throw new Exception("No ReleaseDC");
            }
            return result;
        }

        private void _MouseClick(IntPtr hwnd, string button, int x, int y, byte times = 1, int delay = 80)
        {
            if (hwnd == IntPtr.Zero)
            {
                throw new Exception("Zero pointer");
            }
            int lParam = Helper.MakeLParam(x, y);
            if (button == "left")
            {
                for (int i = 0; i < times; i++)
                {
                    PostMessage(hwnd, 0x200, 0, lParam);
                    PostMessage(hwnd, 0x201, 1, lParam);
                    PostMessage(hwnd, 0x202, 0, lParam);
                    Thread.Sleep(delay);
                }
            }
            else if (button == "right")
            {
                for (int i = 0; i < times; i++)
                {
                    PostMessage(hwnd, 0x200, 0, lParam);
                    PostMessage(hwnd, 0x204, 2, lParam);
                    PostMessage(hwnd, 0x205, 0, lParam);
                    Thread.Sleep(delay);
                }
            }
        }

    }
}
