using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BotWars2Server.Code.Communication;

namespace BotWars2Server.Code.State
{
    public abstract class Player
    {
        public string Name { get; set; }        
        public Position Position { get; set; }

        public bool IsAlive { get; set; }        

        /// <summary>
        /// The maximum number of spaces a player can have in their tail (null means no limit), this value can be adjusted as the game is played
        /// </summary>
        public int? MaximumTailLength { get; internal set; }

        /// <summary>
        /// Gets the round the player can play in
        /// </summary>
        public virtual IEnumerable<int> PlayableRounds
        {
            get
            {
                return Enumerable.Range(1, 4);
            }
        }

        public Player(string name)
        {
            this.IsAlive = true;
            Name = name;

            this.BikeBrush = null;
        }

        public abstract Position GetMove(RadarScan scan);

        public Brush BikeBrush { get; set; }

        public abstract void UpdateState(Arena arena);
        public abstract void SendStartInstruction(Arena arena);
        public abstract void SendEndGame(Arena arena);
        public abstract void SendEndRound(IEnumerable<KeyValuePair<string, int>> scores, int myScore, int numberOfGames, int numberOfGamesForEachPlayer);
        
    }
}
