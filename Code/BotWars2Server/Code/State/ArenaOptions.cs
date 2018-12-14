namespace BotWars2Server.Code.State
{
    public class ArenaOptions
    {
        public bool CrossingOwnTrackCausesDestruction { get; set; }

        public BoundaryStyle BoundaryStyle { get; set; }

        public ArenaOptions()
        {
            this.CrossingOwnTrackCausesDestruction = true;
            this.BoundaryStyle = BoundaryStyle.NeverEnding;
        }
    }
}