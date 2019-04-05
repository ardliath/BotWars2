using BotWars2Server.Code.Logic;
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

        public string SecretCommandCode { get; set; }
        public DateTime LastPinged { get; set; }
        public Uri Uri { get; set; }

        public RemoteBot(string name, Uri uri, string secretCommandCode, DateTime lastPinged) : base(name)
        {
            this.Uri = uri;
            this.SecretCommandCode = secretCommandCode;
            LastPinged = lastPinged;
        }

        public override Position GetMove(RadarScan scan)
        {
            var direction = this.SendGetMoveRequest(scan);
            int xChange;
            int yChange;
            switch (direction)
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

        public override void UpdateState(Arena arena)
        {
        }

        public override void SendStartInstruction(Arena arena)
        {            
            var radar = Radar.Scan(this, arena);
            var data = new CurrentGameState
            {
                Arena = new ArenaState
                {
                    ArenaHeight = arena.Height,
                    ArenaWidth = arena.Width,
                },
                CurrentPosition = this.Position,
                Radar = radar
            };

            SendMessage("startgame", data);
        }

        private Direction SendGetMoveRequest(RadarScan scan)
        {
            var data = new GetMove
            {
                RadarScan = scan
            };
            var response = SendMessage("getmove", data);
            var responseObject = JsonConvert.DeserializeObject<GetMoveResponse>(response);
            return responseObject.Direction;
        }

        private string SendMessage(string message, object data)
        {
            var request = (HttpWebRequest)WebRequest.Create(string.Format("{0}/{1}", this.Uri, message));

            var postData = JsonConvert.SerializeObject(data);
            var byteData = Encoding.ASCII.GetBytes(postData);

            request.Method = "POST";
            request.Timeout = 30000;
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = byteData.Length;

            using (var stream = request.GetRequestStream())
            {
                stream.Write(byteData, 0, byteData.Length);
            }

            var response = (HttpWebResponse)request.GetResponse();

            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
            return responseString;
        }

        public override void SendEndGame(Arena arena)
        {
            var winner = arena.Players.SingleOrDefault(p => p.IsAlive);
            var data = new EndGame
            {
                DidIWin = winner != null && winner.Equals(this)
            };

            SendMessage("endgame", data);
        }

        public override void SendEndRound(IEnumerable<KeyValuePair<string, int>> scores, int myScore, int numberOfGames, int numberOfGamesForEachPlayer)
        {
            var data = new EndRound
            {
                Scores = scores.ToArray(),
                MyScore = myScore,
                NumberOfGames = numberOfGames,
                NumberOfGamesForEachPlayer = numberOfGamesForEachPlayer
            };

            SendMessage("endround", data);
        }
    }
}