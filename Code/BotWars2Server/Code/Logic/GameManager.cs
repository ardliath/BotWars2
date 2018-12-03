using BotWars2Server.Code.State;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotWars2Server.Code.Logic
{
    public class GameManager
    {
        public GameManager(Arena arena, params Player[] players)
        {
            Arena = arena;
            Players = players;
        }

        public Arena Arena { get; }
        public IEnumerable<Player> Players { get; }

        public void Play(Action<Arena> updateAction)
        {
            this.Arena.Players = this.Players;
            var tracks = new List<Track>();
            foreach(var player in this.Players)
            {
                tracks.Add(new Track(player));
            }
            this.Arena.Tracks = tracks;

            foreach (var player in this.Arena.Players)
            {
                this.SendStartInstructions(player);
            }


            while (this.Arena.Players.Count(p => p.IsAlive) > 1)
            {
                foreach(var player in this.Arena.Players.Where(p => p.IsAlive))
                {
                    this.GetMoveFromPlayer(player);
                }

                foreach (var player in this.Arena.Players)
                {
                    this.UpdatePlayersOnArena(player);
                }

                updateAction(this.Arena);
            }
        }

        private void SendStartInstructions(Player player)
        {
            // This is where we tell the player what the game is and what's going on
        }

        private void UpdatePlayersOnArena(Player player)
        {
            // This is where we will tell the players what is going on in the game
        }

        private void GetMoveFromPlayer(Player player)
        {
            // This is where we ask each player what we they want to do
        }
    }
}
