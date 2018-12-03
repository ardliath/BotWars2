using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotWars2Server.Code.State
{
    public class Arena
    {
        public int Height { get; set; }
        public int Width { get; set; }
        public IEnumerable<Player> Players { get; set; }
        public IEnumerable<Track> Tracks { get; set; }
    }
}
