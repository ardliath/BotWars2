using BotWars2Server.Code.State;
using System;
using System.Collections.Generic;

namespace BotWars2Server.Code.Communication
{
    public interface ICommander
    {
        void Register(RegisterData data);

        void RegisterPlayersActiveInGame(IEnumerable<RemoteBot> players);

        void RegisterRegistrationAction(Action<RegisterData> action);
    }
}