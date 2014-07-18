using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace bot
{
    //TODO: Наверное, нужно будет это задействовать. 
    public enum BotStatus
    {
        Searching,
        MatchAccepted,
        Disconnected,
        HeroPicking
    }


    public class DotaBotWorker : IDisposable
    {

        public string PID { get; set; }
        [System.ComponentModel.Browsable(false)]
        public int WindowWidth { get; set; }
        [System.ComponentModel.Browsable(false)]
        public int WindowHeight { get; set; }

        public delegate void MsgEventHandler(string PID, string msg);

        public event MsgEventHandler PostMsg = delegate { };

        private volatile IntPtr hwnd;
        private volatile Bitmap shot;

        private ManualResetEvent runningEvent = new ManualResetEvent(false);

        private Thread mainThread;
        private Thread matchSearchThread;
        private Thread endMatchThread;
        private Thread heroPickThread;

        private Dictionary<string, DotaObject> dotaObjects = new Dictionary<string, DotaObject>();

        public DotaBotWorker(string pid)
        {
            this.PID = pid;
            var p = Process.GetProcessById(Convert.ToInt32(pid));
            p.EnableRaisingEvents = true;
            p.Exited += (o, ev) => { System.Windows.Forms.MessageBox.Show("Exited"); };
            hwnd = p.MainWindowHandle;
            shot = new Bitmap(1, 1);
            WindowWidth = 640;
            WindowHeight = 480;
            using (StreamReader sr = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + "Xml.xml"))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<DotaObject>));
                var list = (List<DotaObject>)serializer.Deserialize(sr);
                foreach (var dobj in list)
                {
                    dobj.LoadPicture();
                    dotaObjects[dobj.Name] = dobj;
                }
            }

            mainThread = new Thread(new ParameterizedThreadStart(WindowCapturing));
            mainThread.Start(runningEvent);

            matchSearchThread = new Thread(new ParameterizedThreadStart(MatchSearchWatching));
            matchSearchThread.Start(runningEvent);

            endMatchThread = new Thread(new ParameterizedThreadStart(EndMatchWatching));
            endMatchThread.Start(runningEvent);

            heroPickThread = new Thread(new ParameterizedThreadStart(HeroPicking));
            heroPickThread.Start(runningEvent);
        }

        ~DotaBotWorker()
        {
            Dispose();
        }

        public void Start()
        {
            runningEvent.Set();
            PostMsg(this.PID, "Worker started");
        }

        public void Stop()
        {
            runningEvent.Reset();
            PostMsg(this.PID, "Worker stopped");
        }

        private void WindowCapturing(object rev)
        {
            var ev = (ManualResetEvent)rev;
            while (ev.WaitOne())
            {
                shot = WinAPIExt.GetPictureByWindowHandle(hwnd, 0, 0, WindowWidth, WindowHeight);
                Thread.Sleep(200);
            }
        }

        private void MatchSearchWatching(object rev)
        {
            var ev = (ManualResetEvent)rev;
            while (ev.WaitOne())
            {
                if (dotaObjects["FindButton"].Availible(shot))
                {
                    PostMsg(this.PID, "FindButton availible");
                    dotaObjects["FindButton"].Click(hwnd, "left", true, 3);
                    PostMsg(this.PID, "FindButton Clicked");
                }
                else if (dotaObjects["AcceptButton"].Availible(shot) || dotaObjects["AcceptButtonHover"].Availible(shot) || dotaObjects["AcceptButtonDown"].Availible(shot))
                {
                    PostMsg(this.PID, "AcceptButton availible");
                    dotaObjects["AcceptButton"].Click(hwnd, "left", true, 3);
                    PostMsg(this.PID, "AcceptButton clicked");
                }
                Thread.Sleep(1000);
            }
        }

        private void ExpirienceWatching(object rev)
        {
            var ev = (ManualResetEvent)rev;
            while (ev.WaitOne())
            {
                Bitmap region;
                lock (shot)
                {
                    region = shot.GetBitmapRegion(new Rectangle(0, 0, 1, 1));
                }
                Thread.Sleep(1000);
            }
        }

        private void EndMatchWatching(object rev)
        {
            var ev = (ManualResetEvent)rev;
            while (ev.WaitOne())
            {
                //TODO: Заполнять метку конца игры
                if (dotaObjects["CancelButton"].Availible(shot))
                {
                    PostMsg(this.PID, "CancelButton availible");
                    dotaObjects["CancelButton"].Click(hwnd, "left", true, 2);
                    PostMsg(this.PID, "CancelButton clicked");
                }
                Thread.Sleep(1000);
            }
        }

        private void HeroPicking(object rev)
        {
            var ev = (ManualResetEvent)rev;
            while (ev.WaitOne())
            {
                if (dotaObjects["SmileInHeroPick"].Availible(shot))
                {
                    PostMsg(this.PID, "SmileInHeroPick availible");
                    Thread.Sleep(2000);
                    if (!dotaObjects["RepickButton"].Availible(shot))
                    {
                        PostMsg(this.PID, "RepickButton availible");
                        if (dotaObjects["RandomButton"].Availible(shot))
                        {
                            Thread.Sleep(2000);
                            PostMsg(this.PID, "RandomButton availible");
                            dotaObjects["RandomButton"].Click(hwnd, "left", true, 1);
                            PostMsg(this.PID, "RandomButton clicked");

                            Thread.Sleep(2000);
                            WinAPIExt.PMMouseClick(hwnd, "left", 8, 8, 1);
                            PostMsg(this.PID, "MainMenuButton clicked");
                        }
                        else
                        {

                        }
                    }
                    else
                    {
                        Thread.Sleep(10000);
                        //Установим флаг того, что мы пикнули героя
                    }
                }
                Thread.Sleep(1000);
            }
        }


        #region Реализация IDisposable
        private bool disposed = false;

        //Implement IDisposable.
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    runningEvent.Reset();
                    matchSearchThread.Abort();
                    endMatchThread.Abort();
                    heroPickThread.Abort();
                    mainThread.Abort();
                }
                disposed = true;
            }
        }
        #endregion
    }
}
