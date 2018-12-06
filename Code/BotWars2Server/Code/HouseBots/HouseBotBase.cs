using BotWars2Server.Code.State;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotWars2Server.Code.HouseBots
{
    public abstract class HouseBotBase : Player
    {
        public HouseBotBase(string name) : base(name)
        {
        }
    }
}
