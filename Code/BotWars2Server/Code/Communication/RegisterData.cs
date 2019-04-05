using System;

namespace BotWars2Server.Code.Communication
{
    public class RegisterData
    {
        public string Name { get; set; }
        public string SecretCommandCode { get; set; }

        public Uri Uri { get; set; }
    }
}