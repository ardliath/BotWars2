using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BotWars2Server.Code.State;

namespace BotWars2Server.Code.HouseBots
{
    public class RandomBot : HouseBotBase
    {
        public int Direction { get; set; }
        public int? RemainingMoves { get; set; }
        public Random Random { get; set; }

        public RandomBot() : base("Random Bot")
        {
            this.Random = new Random(Guid.NewGuid().GetHashCode());
        }

        public override Position GetMove()
        {
            if (!this.RemainingMoves.HasValue || this.RemainingMoves.Value <= 0)
            {
                var previousDirection = this.Direction;
                this.Direction = this.Random.Next(0, 4);
                if (this.Direction == 0 && previousDirection == 1) this.Direction = 2;
                if (this.Direction == 1 && previousDirection == 0) this.Direction = 3;
                if (this.Direction == 2 && previousDirection == 3) this.Direction = 0;
                if (this.Direction == 3 && previousDirection == 2) this.Direction = 1;

                this.RemainingMoves = this.Random.Next(30);
            }

            this.RemainingMoves--;
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
