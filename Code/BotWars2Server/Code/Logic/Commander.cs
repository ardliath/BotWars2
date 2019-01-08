using System.Collections.Generic;
using BotWars2Server.Code.Communication;
using BotWars2Server.Code.State;
using System.Linq;
using System;

namespace BotWars2Server.Code.Logic
{
    public class Commander : ICommander
    {
        public Commander()
        {
            this.Players = new Dictionary<string, RemoteBot>();
        }

        public Dictionary<string, RemoteBot> Players { get; set; }
        public Action<RegisterData> RegistrationAction { get; set; }

        public void Register(RegisterData data)
        {
            this.RegistrationAction?.Invoke(data);
        }

        public void RegisterRegistrationAction(Action<RegisterData> action)
        {
            this.RegistrationAction = action;
        }

        public void RegisterPlayersActiveInGame(IEnumerable<RemoteBot> players)
        {
            this.Players = players.ToDictionary(x => x.Name, x => x);
        }

        public void Turn(TurnData data)
        {
            var player = this.Players.ContainsKey(data.Name) ? this.Players[data.Name] : null;
            if (player != null && player.CurrentCommand != null)
            {
                lock (player.CurrentCommand)
                {
                    player.CurrentCommand = data;
                }
            }
        }
    }
}