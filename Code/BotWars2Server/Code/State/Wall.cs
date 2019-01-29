using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotWars2Server.Code.State
{
    public class Wall : List<Position>
    {
        public bool DoesMove
        {
            get
            {
                return this.MovementTransform != null;
            }
        }

        public Position MovementTransform { get; set; }

        public int MovementCycle { get; set; }



        public Position TransformBrick(Position brick, int tick)
        {
            if (this.DoesMove)
            {
                var cycleTick = tick % this.MovementCycle;
                bool isOnReturnJourney = IsOnReturnJourney(this.MovementCycle, tick);

                if (isOnReturnJourney) // if we're on the way back
                {
                    var returnJourneyOffset = isOnReturnJourney ? -1 : 1;

                    int actualX = brick.X // then our position is X (the origin)
                        + (this.MovementCycle * this.MovementTransform.X) // added to a full movement cycle of the wall
                        + (cycleTick * this.MovementTransform.X * returnJourneyOffset); // subtract the tick we're on

                    int actualY = brick.Y
                        + (this.MovementCycle * this.MovementTransform.Y)
                        + (cycleTick * this.MovementTransform.Y * returnJourneyOffset);

                    return new Position(actualX, actualY);
                }
                else
                {
                    return new Position(brick.X + (cycleTick * this.MovementTransform.X),
                        brick.Y + (cycleTick * this.MovementTransform.Y));
                }
            }
            else
            {
                return brick;
            }
        }

        public static bool IsOnReturnJourney(int cycle, int tick)
        {
            var outAndBack = cycle * 2;
            var doubleCycleTick = tick % outAndBack; // ticks to go out and back            
            var onReturnJourney = doubleCycleTick >= cycle;
            return onReturnJourney;
        }

        public IEnumerable<Position> TransformBricks(Arena arena, int tick)
        {
            return this.Select(b => this.TransformBrick(b, tick));
        }
    }
}
