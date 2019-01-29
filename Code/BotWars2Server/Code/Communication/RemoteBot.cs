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
            this.CurrentCommand = new TurnData
            {
                Name = name,
                Direction = "Right"
            };
        }

        public override Position GetMove()
        {
            if (this.CurrentCommand != null)
            {
                lock (this.CurrentCommand)
                {
                    var direction = Enum.Parse(typeof(Direction), this.CurrentCommand.Direction);
                    int xChange;
                    int yChange;
                    switch(direction)
                    {
                        case Direction.Down:
                            xChange = 0;
                            yChange = 1;
                            break;

                        case Direction.Up:
                            xChange = 0;
                            yChange = -1;
                            break;

                        case Direction.Left:
                            xChange = -1;
                            yChange = 0;
                            break;

                        case Direction.Right:
                            xChange = 1;
                            yChange = 0;
                            break;

                        default:
                            xChange = 0;
                            yChange = 0;
                            break;
                    }
                    return new Position(this.Position.X + xChange, this.Position.Y + yChange);
                }
            }
            else
            {
                return this.Position; // move down by default
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
