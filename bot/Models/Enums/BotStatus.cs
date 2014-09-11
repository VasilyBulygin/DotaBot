using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bot
{
    //TODO: Наверное, нужно будет это задействовать. 
    public enum BotStatus
    {
        CanSearch,
        Searching,
        MatchAccepted,
        Disconnected,
        HeroPicking,
        HeroPicked,
        Playing
    }
}
