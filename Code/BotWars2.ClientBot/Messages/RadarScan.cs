namespace BotWars2.ClientBot.Messages
{
    public class RadarScan
    {
        public int Up { get; set; }
        public int Left { get; set; }
        public int Right { get; set; }
        public int Down { get; set; }

        public Position[] EnemyPositions { get; set; }

        public Position MyPosition { get; set; }
    }    
}