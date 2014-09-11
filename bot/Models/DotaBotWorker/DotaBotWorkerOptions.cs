using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bot
{
    public class DotaBotWorkerOptions
    {
        private readonly Object locker = new object();

        //Текущая команда
        private bool _isDireTeam = false;
        public bool IsDireTeam
        {
            get
            {
                lock (locker) { return _isDireTeam; }
            }
            set
            {
                lock (locker) { _isDireTeam = value; }
            }
        }

        //Время сна потока, наблюдающего за доступностью основных dotaObject's
        private int _watchingTimeout = 2000;
        [Description("Время сна потока, наблюдающего за доступностью основных объектов")]
        public int WatchingTimeout
        {
            get
            {
                lock (locker){ return _watchingTimeout; }
            }
            set
            {
                lock (locker){ _watchingTimeout = value; }
            }
        }
        
        //Интервал съемки скриншота
        private int _capturingTimeout = 200;
        [Description("Интервал съемки скриншота")]
        public int CapturingTimeout
        {
            get
            {
                lock (locker){ return _capturingTimeout; }
            }
            set
            {
                lock (locker){ _capturingTimeout = value; }
            }
        }
        
        //Рандомим ли героя
        private bool _isRandomHeroPick = true;
        public bool IsRandomHeroPick
        {
            get
            {
                lock (locker) { return _isRandomHeroPick; }
            }
            set
            {
                lock (locker) { _isRandomHeroPick = value; }
            }
        }
        //Герой для пика
        private string _heroToPick = null;
        public string HeroToPick
        {
            get
            {
                lock (locker) { return _heroToPick; }
            }
            set
            {
                lock (locker) { _heroToPick = value; }
            }
        }
        //Только пикнуть героя, не вступая в игру?
        private bool _isOnlyPickHero = true;
        public bool IsOnlyPickHero
        {
            get
            {
                lock (locker) { return _isOnlyPickHero; }
            }
            set
            {
                lock (locker) { _isOnlyPickHero = value; }
            }
        }
        //Время, в течение которого нужно получить экспу
        private int _expTimeout = 50000;
        public int ExpTimeout
        {
            get
            {
                lock (locker) { return _expTimeout; }
            }
            set
            {
                lock (locker) { _expTimeout = value; }
            }
        }

        //Интервал атак
        private int _attackInterval = 2000;
        public int AttackInterval
        {
            get
            {
                lock (locker) { return _attackInterval; }
            }
            set
            {
                lock (locker) { _attackInterval = value; }
            }
        }


        private int _shopMenuXOffset = 777;
        public int ShopMenuXOffset
        {
            get
            {
                lock (locker) { return _shopMenuXOffset; }
            }
            set
            {
                lock (locker) { _shopMenuXOffset = value; }
            }
        }

        private int _shopMenuYOffset = 555;
        public int ShopMenuYOffset
        {
            get
            {
                lock (locker) { return _shopMenuYOffset; }
            }
            set
            {
                lock (locker) { _shopMenuYOffset = value; }
            }
        }

    }
}
