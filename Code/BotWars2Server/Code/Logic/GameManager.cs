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
        public GameManager(Game game)
        {
            Arena = game.Arena;
            Players = game.Players;
        }

        public Arena Arena { get; }
        public IEnumerable<Player> Players { get; }

        public int Play(Action<Arena, int> updateAction)
        {
            this.Arena.Players = this.Players;
            var tracks = new List<Track>();
            foreach(var player in this.Players)
            {
                player.IsAlive = true;
                player.MaximumTailLength = this.Arena.ArenaOptions.StartingMaximumTailLength;
                tracks.Add(new Track(player));                
            }
            this.Arena.Tracks = tracks;
            this.SetStartPositions();
            this.CreateWalls();

            foreach (var player in this.Arena.Players)
            {
                try
                {
                    player.SendStartInstruction(this.Arena);
                }
                catch(Exception) // swallow issues sending start message to bot
                {
                }
            }

            int moves = 0;
            while (this.Arena.Players.Count(p => p.IsAlive) > 1)
            {
                foreach(var player in this.Arena.Players.Where(p => p.IsAlive))
                {
                    try
                    {
                        var scan = Radar.Scan(player, this.Arena);
                        var newPosition = player.GetMove(scan);
                        var track = this.Arena.Tracks.SingleOrDefault(t => t.Player.Equals(player));
                        if (IsPositionValidForPlayer(player, newPosition))
                        {
                            track?.PreviousPositions.Add(player.Position);
                            player.Position = newPosition;
                        }
                        if (player.MaximumTailLength.HasValue
                            && player.MaximumTailLength.Value < track.PreviousPositions.Count())
                        {
                            var numberToRemove = track.PreviousPositions.Count() - player.MaximumTailLength.Value;
                            track.PreviousPositions = track.PreviousPositions.Skip(numberToRemove).ToList();
                        }
                    }
                    catch(Exception) // if there's an error then move the bot up one
                    {
                        player.Position.Y--;
                    }
                }

                CheckForCollisions(this.Arena, moves);

                foreach (var player in this.Arena.Players)
                {
                    this.UpdatePlayersOnArena(player);
                }

                updateAction(this.Arena, moves);

                var frame = 0;
                const int tickPerSpin = 1;
                do
                {
                    Application.DoEvents();
                    Thread.Sleep(tickPerSpin);
                    frame++;
                } while (frame < this.Arena.Speed);
                
                moves++;
            }

            foreach (var player in this.Arena.Players)
            {
                try
                {
                    player.SendEndGame(this.Arena);
                }
                catch (Exception) // swallow issues sending start message to bot
                {
                }
            }

            return moves;
        }

        private void CreateWalls()
        {
            var walls = new List<Wall>();

            AddArenaBoundaries(walls);
            AddInternalWalls(walls);

            this.Arena.Walls = walls;
        }

        private void AddInternalWalls(List<Wall> walls)
        {
            var random = new Random();
            const int minWallLength = 5;

            for (int i = 0; i < this.Arena.ArenaOptions.InteriorWalls; i++)
            {
                bool wallOrientationIsHorizontal = random.Next(0,2) < 1;
                bool wallDoesMove = i < this.Arena.ArenaOptions.MovingWalls;

                int maxFreeSpace = wallOrientationIsHorizontal ? this.Arena.Width : this.Arena.Height;
                int wallLength = random.Next(minWallLength, maxFreeSpace - 20);
                //var spaceBetweenWallStartAndBoundary = random.Next(0, maxFreeSpace);
                int spaceBetweenWallStartAndBoundary = (maxFreeSpace - wallLength) / 2;
                int freeSpaceForMovement = maxFreeSpace - wallLength;

                //to make the walls appear symmetrical, I'm assuming that this only needs to be horizontal symmetry, not vertical...
                //otherwise the walls would only ever be located in the exact middle of the arena

                //also check if arena size is odd or even, if odd then make walls odd length, otherwise make even

                var thisWall = new Wall();
                var mirWall = new Wall();
                
                int xStart = random.Next(0, this.Arena.Width);
                int xOffset = wallOrientationIsHorizontal ? 1 : 0;

                int yStart = random.Next(0, this.Arena.Height);
                int yOffset = wallOrientationIsHorizontal ? 0 : 1;

                List<Position> safeWallPositions = GetPlayerSafeWallPositions(this.Players, new Position(xStart, yStart), wallLength, xOffset, yOffset);
                thisWall.AddRange(safeWallPositions);
                
                if (wallDoesMove)
                {
                    const int wallSpeed = 1; // just in case we want to make this configurable in the future
                    bool moveDirectionIsHorizontal = random.Next(0,2) < 1;

                    thisWall.MovementCycle = random.Next(5, Math.Max(5, freeSpaceForMovement - spaceBetweenWallStartAndBoundary));
                    thisWall.MovementTransform = moveDirectionIsHorizontal
                        ? new Position(wallSpeed, 0)
                        : new Position(0, wallSpeed);
                }

                walls.Add(thisWall);
            }
        }

        private List<Position> GetPlayerSafeWallPositions(IEnumerable<Player> players, Position startingPosition, int wallLength, int xOffset, int yOffset)
        {
            List<Position> playerSafeWalls = new List<Position>();
            //This value sets how much space to have around the player where the walls cannot be
            int paddingSpace = 3;

            Random random = new Random();
            bool wallOrientationIsHorizontal = random.Next(0, 2) < 1;



            List<Position> playerPositions = (from Player p in players
                                        select new Position(p.Position.X, p.Position.Y)).ToList();

            List<Position> UnsafePositions = new List<Position>();

            foreach (Position p in playerPositions) {
                UnsafePositions.Add(new Position(p.X + paddingSpace, p.Y + paddingSpace));
                UnsafePositions.Add(new Position(p.X - paddingSpace, p.Y - paddingSpace));
                    }


            List<Position> wallPositions = new List<Position>();
            bool isntSafeForPlayers = true;

            while (isntSafeForPlayers)
            {
                for (int j = 0; j < wallLength; j++)
                {
                    wallPositions.Add(new Position(startingPosition.X + (j * xOffset), startingPosition.Y + (j * yOffset)));
                }

                foreach (Position p in wallPositions)
                {
                    for (int i = 0; i < UnsafePositions.Count; i = i + 2)
                    {
                        if ((p.X < UnsafePositions[i].X && p.X > UnsafePositions[i + 1].X) && (p.Y < UnsafePositions[i].Y && p.Y > UnsafePositions[i + 1].Y))
                        {
                            isntSafeForPlayers = true;
                            break;
                        }
                        else
                        {
                            isntSafeForPlayers = false;
                        }
                    }   
                }
                if (isntSafeForPlayers)
                {
                    wallPositions.Clear();

                    int maxFreeSpace = wallOrientationIsHorizontal ? this.Arena.Width : this.Arena.Height;
                    int spaceBetweenWallStartAndBoundary = (maxFreeSpace - wallLength) / 2;

                    int xStart = random.Next(0, this.Arena.Width);
                    xOffset = wallOrientationIsHorizontal ? 1 : 0;

                    int yStart = random.Next(0, this.Arena.Height);
                    yOffset = wallOrientationIsHorizontal ? 0 : 1;
                    startingPosition = new Position(xStart, yStart);
                }
            }
            playerSafeWalls.AddRange(wallPositions);
            return playerSafeWalls;
        }

        private void AddArenaBoundaries(List<Wall> walls)
        {
            if (this.Arena.ArenaOptions.BoundaryStyle == BoundaryStyle.Walled)
            {
                var thisWall = new Wall();
                for (int i = 0; i < this.Arena.Width; i++) thisWall.Add(new Position(i, 0));
                walls.Add(thisWall);

                thisWall = new Wall();
                for (int i = 0; i < this.Arena.Width; i++) thisWall.Add(new Position(i, this.Arena.Height - 1));
                walls.Add(thisWall);

                thisWall = new Wall();
                for (int i = 0; i < this.Arena.Height; i++) thisWall.Add(new Position(0, i));
                walls.Add(thisWall);

                thisWall = new Wall();
                for (int i = 0; i < this.Arena.Height; i++) thisWall.Add(new Position(this.Arena.Width - 1, i));
                walls.Add(thisWall);
            }
        }

        /// <summary>
        /// The Size of the Internal Circle.
        /// </summary>
        /// <param name="paddingFromEdge">How close the players will be to the edge</param>
        public void SetStartPositions()
        {
            int paddingFromEdge = this.Arena.Height / 5;
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

        public static IEnumerable<Game> CreateGames(List<Player> players, Arena arena)
        {

            var games = new List<Game>();

            if (!arena.ArenaOptions.PlayAllPlayersInSingleGame)
            {
                for (int i = 0; i < players.Count; i++)
                {
                    for (int j = 0; j < players.Count; j++)
                    {
                        if (j >= i) continue;

                        games.Add(new Game(new Player[]
                        {
                        players.ElementAt(i),
                        players.ElementAt(j)
                        },
                        arena));
                    }
                }
            }
            else
            {
                games.Add(new Game(players, arena));
            }

            return games;
        }

        /// <summary>
        /// Checks if there have been any collisions and removes players from the game if there have been
        /// </summary>
        public static void CheckForCollisions(Arena arena, int tick)
        {
            foreach(var player in arena.Players.Where(p => p.IsAlive))
            {
                var otherPlayers = arena.Players.Where(p => !p.Equals(player));
                foreach(var otherPlayer in otherPlayers)
                {
                    if(otherPlayer.Position.Equals(player.Position)) // if we've collided
                    {
                        player.IsAlive = false; // then kill both bots
                        otherPlayer.IsAlive = false;
                    }
                }

                var otherTracks = arena.Tracks
                    .Where(t => otherPlayers.Contains(t.Player))
                    .SelectMany(t => t.PreviousPositions)
                    .Union(arena.Walls.SelectMany(w => w.TransformBricks(arena, tick)));

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
        /// Updates the player on the current Arena state
        /// </summary>
        private void UpdatePlayersOnArena(Player player)
        {
            player.UpdateState(this.Arena);
        }
    }
}
