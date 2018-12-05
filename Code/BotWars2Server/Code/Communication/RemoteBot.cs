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

        public RemoteBot(string name, string uri, Position position) : base(name, position)
        {
            this.Uri = uri;
        }
    }
}
