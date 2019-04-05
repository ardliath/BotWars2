using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BotWars2Server.Code.Communication;
using BotWars2Server.Code.State;

namespace BotWars2Server.Code.HouseBots
{
    public class SirRound : HouseBotBase
    {
        public Direction? CurrentDirection { get; set; }

        public override IEnumerable<int> PlayableRounds => Enumerable.Range(2, 3);

        public SirRound()
            : base("Sir Round")
        {

        }

        public override Position GetMove(RadarScan scan)
        {
            var closestWall = new int[]
            {
                scan.Up,
                scan.Down,
                scan.Left,
                scan.Right
            }.OrderBy(x => x).First();

            if (this.CurrentDirection == null)
            {                

                if (scan.Up == closestWall) CurrentDirection = Direction.Up;
                else if (scan.Down == closestWall) CurrentDirection = Direction.Down;
                else if (scan.Left == closestWall) CurrentDirection = Direction.Left;
                else CurrentDirection = Direction.Right;
            }

            var rangeFromWall = GetWallInDirection(scan, this.CurrentDirection.Value);

            if(rangeFromWall <= 2)
            {
                switch(CurrentDirection)
                {
                    case Direction.Up:
                        CurrentDirection = Direction.Right;
                        break;

                    case Direction.Right:
                        CurrentDirection = Direction.Down;
                        break;

                    case Direction.Down:
                        CurrentDirection = Direction.Left;
                        break;

                    default:
                        CurrentDirection = Direction.Up;
                        break;
                }
            }

            return ConvertDirectionToPosition(CurrentDirection.Value);
        }

        public override void SendStartInstruction(Arena arena)
        {
            
        }

        public override void UpdateState(Arena arena)
        {
            
        }
    }
}
