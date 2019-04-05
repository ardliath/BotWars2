using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotWars2Server.Code.State
{
    public class Game
    {
        public Arena Arena { get; set; }

        public IEnumerable<Player> Players { get; set; }

        public Game(IEnumerable<Player> players, Arena arena)
        {
            Players = players;
            Arena = arena;
        }

        public override string ToString()
        {
            return $"{string.Join(" vs ", this.Players.Select(p => p.Name).ToArray())} in {this.Arena.Width} by {this.Arena.Height} arena";
        }
    }
}
