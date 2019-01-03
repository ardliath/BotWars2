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
            this.Players = new Dictionary<string, RemoteBot>();
        }

        public Dictionary<string, RemoteBot> Players { get; set; }

        public void Register(RegisterData data)
        {
            throw new System.NotImplementedException();
        }

        public void RegisterPlayers(IEnumerable<RemoteBot> players)
        {
            this.Players = players.ToDictionary(x => x.Name, x => x);
        }

        public void Turn(TurnData data)
        {
            var player = this.Players.ContainsKey(data.Name) ? this.Players[data.Name] : null;
            if (player != null)
            {
                lock (player.CurrentCommand)
                {
                    player.CurrentCommand = data;
                }
            }
        }
    }
}