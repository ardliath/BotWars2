using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BotWars2Server.Code.Communication;

namespace BotWars2Server.Code.State
{
    public abstract class Player
    {
        public string Name { get; set; }        
        public Position Position { get; set; }

        public bool IsAlive { get; set; }
        public TurnData CurrentCommand { get; internal set; }

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
