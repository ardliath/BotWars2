using BotWars2Server.Code.State;
using System.Collections.Generic;

namespace BotWars2Server.Code.Communication
{
    public interface ICommander
    {
        void Register(RegisterData data);
        void Turn(TurnData data);
        void RegisterPlayers(IEnumerable<Player> players);
    }
}