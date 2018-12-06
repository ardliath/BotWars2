using BotWars2Server.Code.State;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotWars2Server.Code.Communication
{
    public class RemoteBot : Player
    {
        public int Direction { get; set; }
        public int? RemainingMoves { get; set;}
        public Random Random { get; set; }

        public string Uri { get; set; }

        public RemoteBot(string name, string uri) : base(name)
        {
            this.Uri = uri;
            this.Random = new Random();
        }

        public override Position GetMove()
        {
            if (!this.RemainingMoves.HasValue || this.RemainingMoves.Value <= 0)
            {
                this.Direction = this.Random.Next(0, 5);
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
