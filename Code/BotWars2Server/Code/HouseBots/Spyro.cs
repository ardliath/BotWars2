using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BotWars2Server.Code.Communication;
using BotWars2Server.Code.State;

namespace BotWars2Server.Code.HouseBots
{
    public class Spyro : HouseBotBase
    {
        public Direction? CurrentDirection { get; set; }

        public int StepsTaken { get; set; }

        public int StepsNeeded { get; set; }

        public override IEnumerable<int> PlayableRounds => Enumerable.Range(1, 2);

        public Spyro() : base("Spyro")
        {

        }

        public override Position GetMove(RadarScan scan)
        {
            if(!CurrentDirection.HasValue)
            {
                CurrentDirection = Direction.Up;
                this.StepsNeeded = 1;
            }
            else
            {
                if (this.GetWallInDirection(scan, this.CurrentDirection.Value) < 2)
                {
                    var desiredDirection = GetDesiredDirection(this.CurrentDirection.Value);
                    var range = GetWallInDirection(scan, desiredDirection);
                    int i = 0;
                    while (range < 3 && i < 5)
                    {
                        desiredDirection = GetDesiredDirection(this.CurrentDirection.Value);
                        range = GetWallInDirection(scan, desiredDirection);
                        i++;
                    }

                    this.CurrentDirection = desiredDirection;
                    this.StepsTaken = 0;
                    this.StepsNeeded = 1;
                }
                if (StepsNeeded == StepsTaken)
                {
                    this.CurrentDirection = GetDesiredDirection(this.CurrentDirection.Value);
                    if(this.CurrentDirection == Direction.Down || this.CurrentDirection == Direction.Up)
                    {
                        StepsNeeded++;
                    }                    
                    this.StepsTaken = 0;                    
                }
            }

            this.StepsTaken++;
            return ConvertDirectionToPosition(this.CurrentDirection.Value);
        }

        private Direction GetDesiredDirection(Direction direction)
        {
            switch (direction)
            {
                case Direction.Up:
                    return Direction.Right;

                case Direction.Right:
                    return Direction.Down;
                    break;

                case Direction.Down:
                    return Direction.Left;

                default:
                    return Direction.Up;
            }
        }

        public override void SendStartInstruction(Arena arena)
        {
            
        }

        public override void UpdateState(Arena arena)
        {
            
        }
    }
}
