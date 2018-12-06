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
        public RandomBot() : base("Random Bot")
        {
        }

        public Position CurrentPosition { get; set; }

        public override Position GetMove()
        {
            throw new NotImplementedException();
        }

        public override void SendStartInstruction(Arena arena)
        {
            //this.CurrentPosition = 
        }

        public override void UpdateState(Arena arena)
        {
            
        }
    }
}
