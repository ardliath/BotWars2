using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BotWars2Server.Code.Communication;
using BotWars2Server.Code.State;



namespace BotWars2Server.Code.HouseBots
{
    class Hunter : HouseBotBase
    {
        public Hunter() : base("Hunter")
        {

        }

        public override IEnumerable<int> PlayableRounds => Enumerable.Range(2, 3);

        public override Position GetMove(RadarScan scan)
        {
            var myPos = scan.MyPosition;
            var enemyPos = scan.EnemyPositions;

            List<int> i = new List<int>();
            i.Add(myPos.X - enemyPos[0].X);
            i.Add(enemyPos[0].X - myPos.X);
            i.Add(myPos.Y - enemyPos[0].Y);
            i.Add(enemyPos[0].Y - myPos.Y);

            int indexMax = i.IndexOf(i.Max());

            switch (indexMax)
            {
                case 0:
                    return ConvertDirectionToPosition(Direction.Left);
                case 1:
                    return ConvertDirectionToPosition(Direction.Right);
                case 2:
                    return ConvertDirectionToPosition(Direction.Up);
                case 3:
                    return ConvertDirectionToPosition(Direction.Down);
                default:
                    return ConvertDirectionToPosition(Direction.Up);
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
