using BotWars2Server.Code.Communication;
using BotWars2Server.Code.State;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotWars2Server.Code.Logic
{
    public class Radar
    {
        public static RadarScan Scan(Player player, Arena arena)
        {
            return new RadarScan();
        }
    }
}
