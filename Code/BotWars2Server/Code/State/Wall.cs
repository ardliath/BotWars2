using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotWars2Server.Code.State
{
    public class Wall : List<Position>
    {
        public bool DoesMove
        {
            get
            {
                return this.MovementTransform != null;
            }
        }

        public Position MovementTransform { get; set; }

        public int MovementCycle { get; set; }
    }
}
