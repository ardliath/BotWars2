using System;
using System.Collections.Generic;

namespace BotWars2.ClientBot.Messages
{
    public class EndRound
    {
        public KeyValuePair<string, int>[] Scores { get; set; }

        public int MyScore { get; set; }

        public int NumberOfGames { get; set; }

        public int NumberOfGamesForEachPlayer { get; set; }
    }
}
