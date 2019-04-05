using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BotWars2Server.Code.Communication;
using BotWars2Server.Code.State;

namespace BotWars2Server.Code.HouseBots
{
    /// <summary>
    /// Always moves into the biggest space
    /// </summary>
    public class ClaustroBot : HouseBotBase
    {
        public ClaustroBot()
            : base("ClaustroBot")
        {

        }

        public override IEnumerable<int> PlayableRounds => Enumerable.Range(2, 3);

        public override Position GetMove(RadarScan scan)
        {
            var mostSpace = new int[]
            {
                scan.Up,
                scan.Down,
                scan.Left,
                scan.Right
            }.OrderByDescending(x => x).First();

            Direction direction;
            if (scan.Up == mostSpace) direction = Direction.Up;
            else if (scan.Down == mostSpace) direction = Direction.Down;
            else if (scan.Left == mostSpace) direction = Direction.Left;
            else direction = Direction.Right;

            return ConvertDirectionToPosition(direction);
        }

        public override void SendStartInstruction(Arena arena)
        {
            
        }

        public override void UpdateState(Arena arena)
        {
            
        }
    }
}
