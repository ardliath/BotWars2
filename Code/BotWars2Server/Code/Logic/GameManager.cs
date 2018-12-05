using BotWars2Server.Code.State;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
                    var newPosition = this.GetMoveFromPlayer(player);
                    if (IsPositionValidForPlayer(player, newPosition))
                    {
                        var track = this.Arena.Tracks.SingleOrDefault(t => t.Player.Equals(player));
                        track?.PreviousPositions.Add(player.Position);
                        player.Position = newPosition;
                    }
                }

                CheckForCollisions();

                foreach (var player in this.Arena.Players)
                {
                    this.UpdatePlayersOnArena(player);
                }

                updateAction(this.Arena);

                Thread.Sleep(100);
            }
        }

        private void CheckForCollisions()
        {
            // Checks if there have been any collisions and removes players from the game if there have been
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns a bool indicating whether the player can move to the position
        /// </summary>
        /// <param name="player"></param>
        /// <param name="newPosition"></param>
        /// <returns></returns>
        private bool IsPositionValidForPlayer(Player player, Position newPosition)
        {            
            throw new NotImplementedException();
        }

        /// <summary>
        /// Sent a start instruction to tell the bots where they are
        /// </summary>
        private void SendStartInstructions(Player player)
        {
            player.SendStartInstruction(this.Arena);
        }

        /// <summary>
        /// Updates the player on the current Arena state
        /// </summary>
        private void UpdatePlayersOnArena(Player player)
        {
            player.UpdateState(this.Arena);
        }

        /// <summary>
        /// Ask the player what we they want to do
        /// </summary>
        private Position GetMoveFromPlayer(Player player)
        {
            return player.GetMove();
        }
    }
}
