using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bot.Models
{
    public class ShopItem
    {
        public string Name { get; set; }
        public int Price { get; set; }
        public byte Page { get; set; }
        public byte XIndex { get; set; }
        public byte YIndex { get; set; }

        //Унести отсюда это
        private const int ShopXOffset = 430;
        private const int ShopYOffset = 230;

        private const int ShopXItem = 25;
        private const int ShopYItem = 25;

        private const int ShopPageXOffset = 390;

        public bool CanBuy(int money)
        {
            return money >= this.Price;
        }

        public void Buy(IntPtr hwnd)
        {
            //выбираем вкладку магазина
            WinAPIExt.PMMouseClick(hwnd, "left", ShopPageXOffset + Page*56, 777);
            //ждемс
            //покупаемс
            WinAPIExt.PMMouseClick(hwnd, "right", ShopXOffset + ShopXItem * XIndex, ShopYOffset + ShopYItem * YIndex);
            //пересчитываем валюту

        }

    }
}
