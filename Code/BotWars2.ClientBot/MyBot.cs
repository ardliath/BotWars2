using System;
using BotWars2.ClientBot.Framework;
using BotWars2.ClientBot.Messages;
using System.Linq;

namespace BotWars2.ClientBot
{
    public class MyBot
    {
        public MyBot()
        {
        }

        public string Name => "Player Bot";

        public string SecretCommandCode => "123";

        public Direction GetMove(GetMove getMoveData)
        {            
            return Direction.Right;
        }

        public void StartGame(StartGame data)
        {
            
        }

        public void EndGame(EndGame endGameData)
        {
            
        }

        public void EndRound(EndRound endGameData)
        {
            
        }
    }
}