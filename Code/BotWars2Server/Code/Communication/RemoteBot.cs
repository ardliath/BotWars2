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
            return this.Position;
        }

        public override void UpdateState(Arena arena)
        {
        }

        public override void SendStartInstruction(Arena arena)
        {
        }
    }
}
