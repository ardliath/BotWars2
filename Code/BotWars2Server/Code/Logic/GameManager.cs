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
                player.MaximumTailLength = this.Arena.ArenaOptions.StartingMaximumTailLength;
                tracks.Add(new Track(player));                
            }
            this.Arena.Tracks = tracks;
            this.SetStartPositions();
            this.CreateWalls();

            foreach (var player in this.Arena.Players)
            {
                this.SendStartInstructions(player);
            }


            while (this.Arena.Players.Count(p => p.IsAlive) > 1)
            {
                foreach(var player in this.Arena.Players.Where(p => p.IsAlive))
                {
                    var newPosition = this.GetMoveFromPlayer(player);
                    var track = this.Arena.Tracks.SingleOrDefault(t => t.Player.Equals(player));
                    if (IsPositionValidForPlayer(player, newPosition))
                    {                        
                        track?.PreviousPositions.Add(player.Position);
                        player.Position = newPosition;                        
                    }
                    if(player.MaximumTailLength.HasValue
                        && player.MaximumTailLength.Value < track.PreviousPositions.Count())
                    {
                        var numberToRemove = track.PreviousPositions.Count() - player.MaximumTailLength.Value;
                        track.PreviousPositions = track.PreviousPositions.Skip(numberToRemove).ToList();
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

        private void CreateWalls()
        {
            var walls = new List<Wall>();
            if (this.Arena.ArenaOptions.BoundaryStyle == BoundaryStyle.Walled)
            {
                var thisWall = new Wall();
                for (int i = 0; i < this.Arena.Width; i++) thisWall.Add(new Position(i, 0));
                walls.Add(thisWall);

                thisWall = new Wall();
                for (int i = 0; i < this.Arena.Width; i++) thisWall.Add(new Position(i, this.Arena.Height));
                walls.Add(thisWall);

                thisWall = new Wall();
                for (int i = 0; i < this.Arena.Height; i++) thisWall.Add(new Position(0, i));
                walls.Add(thisWall);

                thisWall = new Wall();
                for (int i = 0; i < this.Arena.Height; i++) thisWall.Add(new Position(this.Arena.Width, i));
                walls.Add(thisWall);
            }

            var random = new Random();
            for(int i = 0; i < this.Arena.ArenaOptions.InteriorWalls; i++)
            {
                var isHorizontal = random.Next(100) < 50;
                var doesMove = i < this.Arena.ArenaOptions.MovingWalls;
                var moveDirection = random.Next(100) < 50;

                var dimension = isHorizontal ? this.Arena.Width : this.Arena.Height;
                int length = random.Next(5, dimension);
                var space = dimension - length;
                var spaceInFront = random.Next(0, space);

                int xOffset = isHorizontal ? 1 : 0;
                int yOffset = isHorizontal ? 0 : 1;
                int xStart = isHorizontal ? spaceInFront : random.Next(0, this.Arena.Width);
                int yStart = isHorizontal ? random.Next(0, this.Arena.Height) : spaceInFront;

                var thisWall = new Wall();
                for (int j = 0; j < length; j++)
                {
                    thisWall.Add(new Position(xStart + (j * xOffset), yStart + (j * yOffset)));
                }
                walls.Add(thisWall);
            }

            this.Arena.Walls = walls;
        }

        /// <summary>
        /// The Size of the Internal Circle.
        /// </summary>
        /// <param name="paddingFromEdge">How close the players will be to the edge</param>
        public void SetStartPositions(int paddingFromEdge = 50)
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
                    .SelectMany(t => t.PreviousPositions)
                    .Union(arena.Walls.SelectMany(w => w));

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
