using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bot
{
    public class Inventory
    {
        List<DotaItem> items;
        IntPtr hwnd;

        public Inventory(IntPtr hwnd)
        {
            this.hwnd = hwnd;
        }

        public DotaItem ItemByName(string name)
        {
            return new DotaItem() { Name = name };
        }

        public void UseItem(string name)
        {
            var item = ItemByName(name);
            item.Use(this.hwnd);
        }
    }
}
