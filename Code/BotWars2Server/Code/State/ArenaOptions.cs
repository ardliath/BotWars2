namespace BotWars2Server.Code.State
{
    public class ArenaOptions
    {
        public bool CrossingOwnTrackCausesDestruction { get; set; }

        /// <summary>
        /// The default at the start of the game of how long a player's tail can grow (null is no upper limit). In game this can be overriden by updating the Player's property (if they pick up bonuses or the like)
        /// </summary>
        public int? StartingMaximumTailLength { get; set; }

        public BoundaryStyle BoundaryStyle { get; set; }
        public int InteriorWalls { get; set; }
        public int MovingWalls { get; set; }

        public ArenaOptions()
        {
            this.CrossingOwnTrackCausesDestruction = true;
            this.StartingMaximumTailLength = 500;
            this.BoundaryStyle = BoundaryStyle.Walled;
            this.InteriorWalls = 1;
            this.MovingWalls = 1;
        }
    }
}