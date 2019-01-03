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


        public string Uri { get; set; }

        public TurnData CurrentCommand { get; internal set; }

        public RemoteBot(string name, string uri) : base(name)
        {
            this.Uri = uri;
        }

        public override Position GetMove()
        {
            lock (this.CurrentCommand)
            {
                return this.Position; // This is called when the game asks the bot what it wants to do. The latest CurrentCommand sent by the bot is stored
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
