using System;

namespace BotWars2.ClientBot.Messages
{
    public class StartGame : EventArgs
    {
        public ArenaState Arena { get; set; }

        public Position CurrentPosition { get; set; }

        public RadarScan Radar { get; set; }
    }
}