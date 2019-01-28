using BotWars2Server.Code.State;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BotWars2Server.Code.Communication
{
    public class RemoteBot : Player
    {


        public string Uri { get; set; }

        public TurnData CurrentCommand { get; internal set; }

        public RemoteBot(string name, string uri) : base(name)
        {
            this.Uri = uri;
        }

        public override Position GetMove()
        {
            if (this.CurrentCommand != null)
            {
                lock (this.CurrentCommand)
                {
                    return this.Position; // This is called when the game asks the bot what it wants to do. The latest CurrentCommand sent by the bot is stored
                }
            }
            else
            {
                return this.Position;
            }
        }

        public override void UpdateState(Arena arena)
        {
        }

        public override void SendStartInstruction(Arena arena)
        {
            var request = (HttpWebRequest)WebRequest.Create(string.Format("{0}/startgame", this.Uri));

            var postData = JsonConvert.SerializeObject(arena);

            var data = Encoding.ASCII.GetBytes(postData);

            request.Method = "POST";
            request.Timeout = 30000;
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = data.Length;

            using (var stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            var response = (HttpWebResponse)request.GetResponse();

            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
        }
    }
}
