namespace BotWars2Server.Code.State
{
    public class ArenaOptions
    {
        public bool CrossingOwnTrackCausesDestruction { get; set; }

        public ArenaOptions()
        {
            this.CrossingOwnTrackCausesDestruction = true;
        }
    }
}