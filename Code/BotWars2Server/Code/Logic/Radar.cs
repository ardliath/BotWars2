using BotWars2Server.Code.Communication;
using BotWars2Server.Code.State;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotWars2Server.Code.Logic
{
    public class Radar
    {
        public static RadarScan Scan(Player player, Arena arena)
        {
            var enemyPlayerPositions = arena.Players.Where(plyr => player.Name != plyr.Name)
                .Select(plyr => plyr.Position);

            return new RadarScan(ScanInDirection(Direction.Up, arena, player),
                ScanInDirection(Direction.Down, arena, player),
                ScanInDirection(Direction.Left, arena, player),
                ScanInDirection(Direction.Right, arena, player),
                enemyPlayerPositions.ToArray(),
                player.Position);
        }

        private static int ScanInDirection(Direction dir, Arena arena, Player player)
        {
            switch (dir)
            {
                case Direction.Up:
                    return CalculateSpace(arena, player, Direction.Up);
                case Direction.Down:
                    return CalculateSpace(arena, player, Direction.Down);
                case Direction.Left:
                    return CalculateSpace(arena, player, Direction.Left);
                case Direction.Right:
                    return CalculateSpace(arena, player, Direction.Right);
                default:
                    return 0;
            }
        }

        private static int CalculateSpace(Arena arena, Player player, Direction dir)
        {
            List<Position> occupiedpositions = new List<Position>();
            var tracks = arena.Tracks;
            var walls = arena.Walls;
            var players = arena.Players;
            var thisPlayer = player;

            foreach (Track t in tracks)
            {
                foreach (Position pos in t.PreviousPositions)
                {
                    occupiedpositions.Add(pos);
                }
            }

            foreach (Player individualPlayer in players)
            {
                occupiedpositions.Add(individualPlayer.Position);
            }

            foreach (Wall wall in walls)
            {
                foreach (Position wallPosition in wall)
                {
                    occupiedpositions.Add(wallPosition);
                }
            }

            int space = 0;
            switch (dir)
            {
                case Direction.Up:
                    space = occupiedpositions.Where(pos => pos.X == player.Position.X)
                        .Select(obs => player.Position.Y - obs.Y)
                        .Where(x => x > 0)
                        .Min();
                    return space;
                case Direction.Down:
                    space = occupiedpositions.Where(pos => pos.X == player.Position.X)
                        .Select(obs => obs.Y - player.Position.Y)
                        .Where(x => x > 0)
                        .Min();
                    return space;
                case Direction.Left:
                    space = occupiedpositions.Where(pos => pos.Y == player.Position.Y)
                        .Select(obs => player.Position.X - obs.X)
                        .Where(x => x > 0)
                        .Min();
                    return space;
                case Direction.Right:
                    space = occupiedpositions.Where(pos => pos.Y == player.Position.Y)
                        .Select(obs => obs.X - player.Position.X)
                        .Where(x => x > 0)
                        .Min();
                    return space;
                default:
                    return 0;
            }
        }
    }
}
