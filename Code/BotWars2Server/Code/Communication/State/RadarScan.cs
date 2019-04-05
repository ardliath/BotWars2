using BotWars2Server.Code.State;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace BotWars2Server.Code.Communication
{
    public class RadarScan
    {
        public int Up { get; set; }
        public int Left { get; set; }
        public int Right { get; set; }
        public int Down { get; set; }
        public Position[] EnemyPositions { get;  set; }

        public Position MyPosition { get; set; }
        

        public RadarScan(int up, int down, int left, int right, Position[] enemyPositions, Position myPosition)
        {
            Up = up;
            Down = down;
            Left = left;
            Right = right;
            EnemyPositions = enemyPositions;
            MyPosition = myPosition;
        }
    }
}