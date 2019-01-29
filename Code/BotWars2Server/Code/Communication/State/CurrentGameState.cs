using BotWars2Server.Code.State;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotWars2Server.Code.Communication
{
    public class CurrentGameState
    {
        public ArenaState Arena { get; set; }        

        public Position CurrentPosition { get; set; }

        public RadarScan Radar { get; set; }
    }
}
