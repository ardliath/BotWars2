using BotWars2Server.Code.Communication;
using BotWars2Server.Code.State;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotWars2Server.Code.HouseBots
{
    public abstract class HouseBotBase : Player
    {
        public HouseBotBase(string name) : base(name)
        {
        }

        public override void SendEndGame(Arena arena)
        {
            
        }

        public override void SendEndRound(IEnumerable<KeyValuePair<string, int>> scores, int myScore, int numberOfGames, int numberOfGamesForEachPlayer)
        {
            
        }


        protected Position ConvertDirectionToPosition(Direction direction)
        {
            switch (direction)
            {
                case Direction.Up:
                    return new Position(this.Position.X, this.Position.Y - 1);
                case Direction.Down:
                    return new Position(this.Position.X, this.Position.Y + 1);
                case Direction.Left:
                    return new Position(this.Position.X - 1, this.Position.Y);
                default:
                    return new Position(this.Position.X + 1, this.Position.Y);
            }
        }

        protected Direction GetOppositeDirection(Direction direction)
        {
            switch (direction)
            {
                case Direction.Up:
                    return Direction.Down;
                case Direction.Down:
                    return Direction.Up;
                case Direction.Left:
                    return Direction.Right;
                default:
                    return Direction.Left;
            }
        }

        protected int GetWallInDirection(RadarScan scan, Direction value)
        {
            switch (value)
            {
                case Direction.Up:
                    return scan.Up;

                case Direction.Right:
                    return scan.Right;

                case Direction.Down:
                    return scan.Down;

                default:
                    return scan.Left;
            }
        }

    }
}
