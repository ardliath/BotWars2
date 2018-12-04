namespace BotWars2Server.Code.State
{
    public class Position
    {
        public int X { get; set; }
        public int Y { get; set; }

        public override bool Equals(object obj)
        {
            var other = obj as Position;
            if (other == null) return false;

            return this.X == other.X
                && this.Y == other.Y;
        }

        public override int GetHashCode()
        {
            return (this.X * this.Y * 27).GetHashCode();
        }
    }
}