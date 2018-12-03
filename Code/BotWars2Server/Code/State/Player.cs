using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotWars2Server.Code.State
{
    public class Player
    {
        public string Name { get; set; }
        public string Uri { get; set; }
        public Position Position { get; set; }

        public bool IsAlive { get; set; }

        public Player(string name, string uri, Position position)
        {
            this.IsAlive = true;
            Name = name;
            Uri = uri;
            Position = position;
        }
    }
}
