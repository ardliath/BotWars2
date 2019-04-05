using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotWars2.ClientBot.Messages
{
    public class Register
    {
        public string Name { get; set; }
        public string SecretCommandCode { get; set; }     
        public Uri Uri { get; set; }
    }
}
