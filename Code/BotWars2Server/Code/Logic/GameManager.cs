using BotWars2Server.Code.State;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

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
            this.SetStartPositions();

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

                CheckForCollisions(this.Arena);

                foreach (var player in this.Arena.Players)
                {
                    this.UpdatePlayersOnArena(player);
                }

                updateAction(this.Arena);

                for (int i = 0; i < 20; i++)
                {
                    Application.DoEvents();
                    Thread.Sleep(5);
                }
            }
        }

        /// <summary>
        /// The Size of the Internal Circle.
        /// </summary>
        /// <param name="paddingFromEdge">How close the players will be to the edge</param>
        private void SetStartPositions(int paddingFromEdge = 50)
        {
            var radius = (this.Arena.Width - (paddingFromEdge * 2)) / 2;
            Position centrepoint = new Position(this.Arena.Width / 2, this.Arena.Height / 2);
            var currentAngle = 0;

            foreach (var player in this.Arena.Players)
            {
                var xcoords = (Math.Round(radius * (Math.Cos(currentAngle * (Math.PI / 180)))));
                var ycoords = (Math.Round(radius * (Math.Sin(currentAngle * (Math.PI / 180)))));
                player.Position = new Position((int)xcoords + centrepoint.X, (int)ycoords + centrepoint.Y);
                currentAngle = currentAngle + 360 / this.Players.Count();
            }
        }

        /// <summary>
        /// Checks if there have been any collisions and removes players from the game if there have been
        /// </summary>
        public static void CheckForCollisions(Arena arena)
        {
            foreach(var player in arena.Players.Where(p => p.IsAlive))
            {
                var otherTracks = arena.Tracks
                    .Where(t => !t.Player.Equals(player))
                    .SelectMany(t => t.PreviousPositions);

                var playerTrack = arena.Tracks
                    .Single(t => t.Player.Equals(player))?.PreviousPositions;

                var dangerousTracks = arena.ArenaOptions.CrossingOwnTrackCausesDestruction
                    ? otherTracks.Union(playerTrack)
                    : otherTracks;

                if (dangerousTracks.Any(p => p.Equals(player.Position)))
                {
                    player.IsAlive = false;
                }
            }
        }

        /// <summary>
        /// Returns a bool indicating whether the player can move to the position
        /// </summary>
        /// <param name="player">The player who wants to move</param>
        /// <param name="newPosition">Their current position</param>
        /// <returns>A bool indicating whether the given move is valid</returns>
        public static bool IsPositionValidForPlayer(Player player, Position newPosition)
        {            
            if(player.Position.Equals(newPosition))
            {
                return false;
            }

            var diffX = Math.Abs(player.Position.X - newPosition.X);
            var diffY = Math.Abs(player.Position.Y - newPosition.Y);

            return diffX + diffY == 1;
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
