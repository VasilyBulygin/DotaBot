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
using System.Windows.Forms;

namespace bot
{
    public class DotaBotWorker : IDisposable
    {
        public DotaBotWorkerOptions Options = new DotaBotWorkerOptions();
        public string PID { get; set; }
        [System.ComponentModel.Browsable(false)]
        public int WindowWidth { get; set; }
        [System.ComponentModel.Browsable(false)]
        public int WindowHeight { get; set; }
        public BotStatus Status;


        //Событие отсылки текущего действия
        public delegate void MsgEventHandler(string PID, string msg);
        public event MsgEventHandler PostMsg = delegate { };
        public event EventHandler Exited = delegate { };

        //Указатель на главное окно
        private volatile IntPtr hwnd;
        //Текущий скриншот главного окна
        private volatile Bitmap shot;
        //Процесс(thx cap)
        private volatile Process process;

        private readonly ManualResetEvent runningEvent = new ManualResetEvent(false);
        private readonly ManualResetEvent playingEvent = new ManualResetEvent(false);
        private readonly ManualResetEvent expEvent = new ManualResetEvent(false); //нет экспы
        //Получали ли мы экспу в последее время?
        private bool IsNoExp = false;
        private readonly ManualResetEvent healthEvent = new ManualResetEvent(false); //мало хп
        //Мало ли у нас здоровья?
        private bool IsLowHP = false;

        //TODO: Вообще говоря, нужно уменьшить число потоков
        private Thread mainThread;
        private Thread statusWatchingThread;
        private Thread endMatchThread;
        private Thread heroPickThread;
        private Thread expWatchDogThread;
        private Thread healthWatchDogThread;
        private Thread feedingThread;

        private volatile int currentGold;
        private volatile int currentHealth;
        private Inventory inventory;
        private object _locker = new object();

        private readonly Dictionary<string, DotaObject> dotaObjects = new Dictionary<string, DotaObject>();
        private readonly List<DotaItem> dotaItems = new List<DotaItem>();

        private List<HeroOnScreen> heroesOnScreen = new List<HeroOnScreen>();

        private volatile bool isHelpDisabled = false;

        public event EventHandler OnLevelUp = delegate { };
        public event EventHandler OnHeroKilled = delegate { };

        private static VAMemory VAM;

        public int CurrentHealth
        {
            get
            {
                return currentHealth;
            }
        }

        public List<HeroOnScreen> HeroesOnScreen
        {
            get
            {
                return heroesOnScreen;
            }
        }

        public int CurrentGold
        {
            get
            {
                return currentGold;
            }
        }

        public Inventory Inventory
        {
            get
            {
                return inventory;
            }
        }

        //Изучение скилла
        public void LearnSkill(int skillNum)
        {
            if (dotaObjects["LevelUpButton"].Availible(shot))
            {
                dotaObjects["LevelUpButton"].Click(hwnd, "left");
                WinAPIExt.PMMouseClick(hwnd, "left", 317 + 53 * skillNum, 447, 1);
            }
        }

        //Покупка айтема по имени
        public bool BuyItemWithName(string name)
        {
            var item = dotaItems.Where(x => x.Name == name).First();
            if (currentGold >= item.Cost && dotaObjects["ShopButton"].Availible(shot))
            {
                dotaObjects["ShopButton"].Click(hwnd, "right");
                Thread.Sleep(1000);
                if (item.Type == "Simple")
                {
                    WinAPIExt.PMMouseClick(hwnd, "left", 0, 0);//клик на пункте стандартных айтемов в шопе
                }
                else
                {
                    WinAPIExt.PMMouseClick(hwnd, "left", 0, 0);//клик на пункте доп. айтемов
                }
                WinAPIExt.PMMouseClick(hwnd, "left", item.X, item.Y);
                return true;
            }
            else
                return false;
        }

        public void UseInventoryItem(DotaItem item)
        {

        }

        public void UseInventorySlotOnCoords()
        {

        }

        /// <summary>
        /// Использование скилла героя
        /// </summary>
        /// <param name="skillNum">Номер скилла на панели скиллов</param>
        public void UseHeroSkill(int skillNum)
        {
            var startOffset = 0;//первоначальный отступ
            WinAPIExt.PMMouseClick(hwnd, "left", startOffset + skillNum * 64, 1);
        }

        public void UseHeroSkillOnCoords(int skillNum, int x, int y)
        {
            var startOffset = 0;
            WinAPIExt.PMMouseClick(hwnd, "left", startOffset + skillNum * 64, 1);
            WinAPIExt.PMMouseClick(hwnd, "left", x, y);
        }

        public List<string> GetInventoryItems()
        {

            return new List<string>();
        }



        public DotaBotWorker(string pid)
        {
            this.PID = pid;
            Status = BotStatus.CanSearch;
            try
            {
                process = Process.GetProcessById(Convert.ToInt32(pid));
                process.EnableRaisingEvents = true;
                process.Exited += (o, ev) => { Exited(this, null); };

                VAM = new VAMemory("dota");

                hwnd = process.MainWindowHandle;
                shot = new Bitmap(1, 1);
                WindowWidth = 640;
                WindowHeight = 480;

                //Берем инфу об объектах
                using (var sr = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + "Xml.xml"))
                {
                    var serializer = new XmlSerializer(typeof(List<DotaObject>));
                    var list = (List<DotaObject>)serializer.Deserialize(sr);
                    foreach (var dobj in list)
                    {
                        dobj.LoadPicture();
                        dotaObjects[dobj.Name] = dobj;
                    }
                }

                mainThread = new Thread(new ParameterizedThreadStart(WindowCapturing));
                mainThread.Start(runningEvent);

                statusWatchingThread = new Thread(new ParameterizedThreadStart(StatusWatching));
                statusWatchingThread.Start(runningEvent);

                heroPickThread = new Thread(new ParameterizedThreadStart(HeroPicking));
                heroPickThread.Start(runningEvent);
                Status = BotStatus.Playing;
                expWatchDogThread = new Thread(new ParameterizedThreadStart(ExpirienceWatching));
                expWatchDogThread.Start(playingEvent);

                healthWatchDogThread = new Thread(new ParameterizedThreadStart(HealthWatching));
                healthWatchDogThread.Start(playingEvent);

                feedingThread = new Thread(new ParameterizedThreadStart(Feeding));
                feedingThread.Start(playingEvent);
                playingEvent.Set();
            }
            catch
            { }
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
            playingEvent.Reset();
            PostMsg(this.PID, "Worker stopped");
        }

        public int CurrentTime()
        {
            Int32 BaseAdr = Convert.ToInt32((Int32)Helper.getClientAdress() + 0x12F5568);
            Int32[] off = new Int32[] { 0x54 };
            IntPtr adr = Helper.getAddressByOffset(BaseAdr, off);
            return VAM.ReadInt32(adr);
        }

        private void WindowCapturing(object rev)
        {
            var ev = (ManualResetEvent)rev;
            while (ev.WaitOne())
            {
                lock (_locker)
                {
                    shot = WinAPIExt.GetPictureByWindowHandle(hwnd, 0, 0, WindowWidth, WindowHeight);
                    if (Status == BotStatus.Playing)
                    {
                        heroesOnScreen = Helper.FindHeroesOnImage(shot);
                    }
                }
                Thread.Sleep(Options.CapturingTimeout);
            }
        }

        private void StatusWatching(object rev)
        {
            //Следим за доступностью не игровых объектов и маркеров текущего статуса
            var ev = (ManualResetEvent)rev;
            while (ev.WaitOne())
            {
                bool findButtonAvailible, acceptButtonAvailible, cancelButtonAvailible;
                lock (_locker)
                {
                    findButtonAvailible = dotaObjects["FindButton"].Availible(shot);
                    acceptButtonAvailible = (dotaObjects["AcceptButton"].Availible(shot) ||
                                             dotaObjects["AcceptButtonHover"].Availible(shot) ||
                                             dotaObjects["AcceptButtonDown"].Availible(shot));
                    cancelButtonAvailible = dotaObjects["CancelButton"].Availible(shot);
                }
                if (findButtonAvailible)
                {
                    playingEvent.Reset();
                    PostMsg(this.PID, "FindButton availible");
                    lock (_locker)
                    {
                        dotaObjects["FindButton"].Click(hwnd, "left", true, 3);
                    }
                    //lua.DoString(@"dota_object_click('FindButton','left',true,3);");
                    PostMsg(this.PID, "FindButton Clicked");
                    Status = BotStatus.Searching;
                }
                else if (acceptButtonAvailible)
                {
                    playingEvent.Reset();
                    PostMsg(this.PID, "AcceptButton availible");
                    lock (_locker)
                    {
                        dotaObjects["AcceptButton"].Click(hwnd, "left", true, 3);
                    }
                    PostMsg(this.PID, "AcceptButton clicked");

                    Status = BotStatus.MatchAccepted;
                }
                else if (cancelButtonAvailible)
                {
                    playingEvent.Reset();
                    PostMsg(this.PID, "CancelButton availible");
                    lock (_locker)
                    {
                        dotaObjects["CancelButton"].Click(hwnd, "left", true, 3);
                    }
                    PostMsg(this.PID, "CancelButton clicked");

                    Status = BotStatus.CanSearch;
                }
                Thread.Sleep(Options.WatchingTimeout);//Проверка идет раз в N секунд(в зависимости от настроек)
            }
        }

        /// <summary>
        /// Следим за экспой по времени ExpTimeOut
        /// </summary>
        /// <param name="rev"></param>
        private void ExpirienceWatching(object rev)
        {
            var ev = (ManualResetEvent)rev;
            Bitmap prevRegion = new Bitmap(1, 1);
            while (ev.WaitOne())
            {
              /*  Bitmap region;
                lock (shot)
                {
                    region = shot.GetBitmapRegion(new Rectangle(147, 472, 52, 4));
                }
                if (Helper.Compare2Bitmaps(region, prevRegion))
                {
                    expEvent.Set();
                    IsNoExp = true;
                }
                else
                {
                    expEvent.Reset();
                    IsNoExp = false;
                }*/
                Thread.Sleep(Options.ExpTimeout);
               
            }
        }


        private void HealthWatching(object rev)
        {
            var ev = (ManualResetEvent)rev;
            while (ev.WaitOne())
            {
                if (!dotaObjects["HealthBar"].Availible(shot))
                {
                    healthEvent.Set();
                    IsLowHP = true;
                }
                else
                {
                    healthEvent.Reset();
                    IsLowHP = false;
                }
                Thread.Sleep(200); //Тормозим
            }
        }

        private void HeroPicking(object rev)
        {
            var ev = (ManualResetEvent)rev;
            while (ev.WaitOne())
            {
                if (dotaObjects["SmileInHeroPick"].Availible(shot))//Доступен экран с выбором героя
                {
                    PostMsg(this.PID, "SmileInHeroPick availible");
                    isHelpDisabled = false;
                    Thread.Sleep(2000);

                    //Пока недоступна кнопка репика героя, т. е. герой еще не пикнут
                    if (!dotaObjects["RepickButton"].Availible(shot))
                    {
                        //Определяем, какая у нас сторона.
                        //Определение осуществляется по мини карте - если объект на миникарте доступен, то мы играем за Dire
                        Options.IsDireTeam = !dotaObjects["RadiantThrone_M_HP"].Availible(shot);

                        PostMsg(this.PID, "Picking hero");
                        if (Options.IsRandomHeroPick && dotaObjects["RandomButton"].Availible(shot))
                        {
                            Thread.Sleep(2000);
                            PostMsg(this.PID, "RandomButton availible");
                            dotaObjects["RandomButton"].Click(hwnd, "left", true, 1);
                            PostMsg(this.PID, "RandomButton clicked");

                            if (Options.IsOnlyPickHero)
                            {
                                Thread.Sleep(2000);
                                WinAPIExt.PMMouseClick(hwnd, "left", 8, 8, 1);
                                PostMsg(this.PID, "MainMenuButton clicked");
                            }
                        }
                        else
                        {
                            //Не рандомим
                            WinAPIExt.au3.ControlSend("", "", process.MainWindowTitle, Options.HeroToPick);
                        }
                        Status = BotStatus.HeroPicked;
                    }
                    else
                    {
                        if (!Options.IsOnlyPickHero)
                        {
                            Thread.Sleep(10000);
                            WinAPIExt.PMMouseClick(hwnd, "left", 325, 345, 1);
                        }
                        //кликаем на вступлении в игру и запускаем фидера, если в экране игры
                        //}
                        //Установим флаг того, что мы пикнули героя*/
                    }
                }
                else
                {
                    if (!Options.IsOnlyPickHero)
                    {
                        if (dotaObjects["HealthBar"].Availible(shot))
                        {
                            Status = BotStatus.Playing;
                            //lua["isRunning"] = true;
                            playingEvent.Set();
                            //ExecuteScript("feeder");
                        }
                    }
                }
                Thread.Sleep(1000);
            }
        }

        private void ExecuteScript(string fileName)
        {
            // lua.DoFile(String.Format("{0}{1}.lua",AppDomain.CurrentDomain.BaseDirectory,fileName));
        }

        //Вынести в отдельные сборки и подгружать в зависимости от того, что выбрано и какой герой пикнут
        //Либо пересесть на lua
        private void Feeding(object rev)
        {
            var ev = (ManualResetEvent)rev;
            Random rand = new Random((int)DateTime.Now.Ticks);
            Stopwatch sw = new Stopwatch();
            sw.Start();
            int attack = 0;
            int attackInterval = Options.AttackInterval/200;
            bool boot = false;
            while (ev.WaitOne())
            {
                if(!isHelpDisabled)
                {
                    WinAPIExt.PMMouseClick(hwnd, "left", 69, 9, 1);
                    for(var i = 0; i < 4; i++)
                    {
                        WinAPIExt.PMMouseClick(hwnd, "left", 232, 62 + i * 20, 1);
                    }
                    WinAPIExt.PMMouseClick(hwnd, "left", 69, 9, 1);
                    isHelpDisabled = true;
                }
                    if (IsLowHP)
                    {
                        while (IsLowHP)
                        {
                            if (Options.IsDireTeam)
                                WinAPIExt.PMMouseClick(hwnd, "right", 120, 368, 1);
                            else
                                WinAPIExt.PMMouseClick(hwnd, "right", 10, 471, 1);
                            Thread.Sleep(Options.AttackInterval);
                        }
                    }
                    /*else if (IsNoExp)
                    {
                        //feed
                        WinAPIExt.au3.ControlSend(process.MainWindowTitle, "", "", "a");
                        Thread.Sleep(100);
                        if(Options.IsDireTeam) WinAPIExt.PMMouseClick(hwnd, "left", 14, 467, 1);
                        else WinAPIExt.PMMouseClick(hwnd, "left", 120, 367, 1);
                    }*/
                    if (dotaObjects["LevelUpButton"].Availible(shot))
                    {
                        dotaObjects["LevelUpButton"].Click(hwnd, "left");
                        Thread.Sleep(100);
                        WinAPIExt.PMMouseClick(hwnd, "left", 317, 447, 1);
                        Thread.Sleep(100);
                        WinAPIExt.PMMouseClick(hwnd, "left", 317 + 53, 447, 1);
                        Thread.Sleep(100);
                        WinAPIExt.PMMouseClick(hwnd, "left", 317 + 53 + 53, 447, 1);
                        Thread.Sleep(100);
                    }
                    if (!boot && dotaObjects["ShopButton"].Availible(shot))
                    {
                        dotaObjects["ShopButton"].Click(hwnd, "left");
                        Thread.Sleep(1000);
                        WinAPIExt.PMMouseClick(hwnd, "right", 618, 163, 1);
                        dotaObjects["ShopButton"].Click(hwnd, "left");
                        boot = true;
                    }

                    if (heroesOnScreen.Count > 0)
                    {
                        if (CurrentTime() > 600)
                        {
                            try
                            {
                                var hos = heroesOnScreen.First();
                                if (rand.Next(0, 20) == 15)
                                {
                                    WinAPIExt.PMMouseClick(hwnd, "left", 317, 447 + rand.Next(0, 6) * 40, 1);
                                    WinAPIExt.PMMouseClick(hwnd, "left", hos.PosX, hos.PosY, 1);
                                    PostMsg(PID, "Skill");
                                }
                                else
                                {
                                    WinAPIExt.PMMouseClick(hwnd, "right", hos.PosX, hos.PosY, 1);
                                }
                            }
                            catch { }
                        }
                        else
                        {
                            if (Options.IsDireTeam)
                                WinAPIExt.PMMouseClick(hwnd, "right", 526, 38, 1);
                            else
                                WinAPIExt.PMMouseClick(hwnd, "right", 53, 336, 1);
                        }
                    }
                    /*if (rand.Next(0, 20) == 15)
                    {
                        WinAPIExt.PMMouseClick(hwnd, "left", 317, 447, 1);
                        Thread.Sleep(100);
                        WinAPIExt.PMMouseClick(hwnd, "left", 380, 160, 1);
                        PostMsg(PID, "Skill");
                    }*/
                    if (CurrentTime() > 80)
                    {
                        if (attack >= attackInterval)
                        {
                            WinAPIExt.au3.ControlSend(process.MainWindowTitle, "", "", "a");
                            Thread.Sleep(100);
                            if (Options.IsDireTeam) WinAPIExt.PMMouseClick(hwnd, "left", 7, 471, 1);
                            else WinAPIExt.PMMouseClick(hwnd, "left", 120, 367, 1);
                            attack = 0;
                        }
                    }
                    else
                    {
                        Thread.Sleep(10000);
                    }
                    Thread.Sleep(200);
                    attack++;
            }
        }

        //следим, не нажата ли пауза
        private void PauseWatching()
        {
            while (true)
            {
                if (dotaObjects["PauseMarker"].Availible(shot))
                {
                    //стопаем "таймер"
                }
            }
        }

        private void Timing()
        {
            //надо бы как нибудь распознавать времечеко   
        }

        //нужно распознавать, сколько у нас сейчас голды

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
                    playingEvent.Reset();
                    statusWatchingThread.Abort();
                    heroPickThread.Abort();
                    mainThread.Abort();
                    feedingThread.Abort();
                }
                disposed = true;
            }
        }
        #endregion
    }
}
