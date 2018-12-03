using System.Collections.Generic;

namespace BotWars2Server.Code.State
{
    public class Track
    {
        public Player Player;

        public IList<Position> PreviousPositions { get; protected set; }

        public Track(Player player)
        {
            this.Player = player;
            this.PreviousPositions = new List<Position>();
        }
    }
}