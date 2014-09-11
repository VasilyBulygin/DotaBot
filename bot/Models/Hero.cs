using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bot
{
    public class Hero
    {
        private int currentGold;
        public int CurrentGold
        {
            get { return currentGold; }
            set { currentGold = value; }
        }
        private Inventory inventory;
        public Inventory Inventory
        {
            get { lock (inventory) { return inventory; } }
            set { lock (inventory) { inventory = value; } }
        }
    }
}
