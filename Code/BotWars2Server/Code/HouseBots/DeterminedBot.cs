using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BotWars2Server.Code.State;

namespace BotWars2Server.Code.HouseBots
{
    public class DeterminedBot : HouseBotBase
    {
        public int Direction { get; set; }

        public Random Random { get; set; }

        public DeterminedBot() : base("Determined Bot")
        {
            this.Random = new Random(Guid.NewGuid().GetHashCode());
        }

        public override Position GetMove()
        {
            switch (this.Direction)
            {
                case 0:
                    return new Position(this.Position.X, this.Position.Y - 1);
                case 1:
                    return new Position(this.Position.X, this.Position.Y + 1);
                case 2:
                    return new Position(this.Position.X - 1, this.Position.Y);
                default:
                    return new Position(this.Position.X + 1, this.Position.Y);
            }
        }

        public override void UpdateState(Arena arena)
        {
        }

        public override void SendStartInstruction(Arena arena)
        {
        }
    }
}
