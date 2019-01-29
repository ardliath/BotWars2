using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotWars2Server.Code.State
{
    public class Arena
    {
        public int Zoom { get; set; }
        public ArenaOptions ArenaOptions { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
        public IEnumerable<Player> Players { get; set; }
        public IEnumerable<Track> Tracks { get; set; }

        public IEnumerable<Wall> Walls { get; set; }


        public Arena(int zoom = 5)
        {
            this.Zoom = zoom;
            this.ArenaOptions = new ArenaOptions();
            this.Walls = new Wall[] { };            
        }
    }
}
