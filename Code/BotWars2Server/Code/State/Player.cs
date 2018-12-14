using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotWars2Server.Code.State
{
    public abstract class Player
    {
        public string Name { get; set; }        
        public Position Position { get; set; }

        public bool IsAlive { get; set; }

        /// <summary>
        /// The maximum number of spaces a player can have in their tail (null means no limit), this value can be adjusted as the game is played
        /// </summary>
        public int? MaximumTailLength { get; internal set; }

        public Player(string name)
        {
            this.IsAlive = true;
            Name = name;
        }

        public abstract Position GetMove();

        public abstract void UpdateState(Arena arena);
        public abstract void SendStartInstruction(Arena arena);
    }
}
