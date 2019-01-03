using System.Collections.Generic;
using BotWars2Server.Code.Communication;
using BotWars2Server.Code.State;
using System.Linq;

namespace BotWars2Server.Code.Logic
{
    public class Commander : ICommander
    {
        public Commander()
        {
            this.Players = new Dictionary<string, Player>();
        }

        public Dictionary<string, Player> Players { get; set; }

        public void Register(RegisterData data)
        {
            throw new System.NotImplementedException();
        }

        public void RegisterPlayers(IEnumerable<Player> players)
        {
            this.Players = players.ToDictionary(x => x.Name, x => x);
        }

        public void Turn(TurnData data)
        {
            var player = this.Players[data.Name].CurrentCommand = data;
        }
    }
}