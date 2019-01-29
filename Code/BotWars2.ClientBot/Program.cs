using BotWars2.ClientBot.Messages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BotWars2.ClientBot
{
    class Program
    {

        static void Main(string[] args)
        {
            Console.WriteLine("Ready to send Register Command...");
            Console.ReadKey();

            bool gameHasStarted = false;

            SendRegisterCommand();

            Console.WriteLine("Ready Command sent waiting for game to begin...");
            new HttpListenerClass(6999, data =>
            {
                gameHasStarted = true;

            }).Listen();

            do
            {
                Thread.Sleep(10);
            } while (!gameHasStarted);


            do
            {
                Console.WriteLine("Start Instruction recieved - we're playing");
                Console.WriteLine("Ready to send Turn Command...");
                var move = Direction.Up;
                switch (Console.ReadKey().Key)
                {
                    case ConsoleKey.UpArrow:
                        move = Direction.Up;
                        break;

                    case ConsoleKey.DownArrow:
                        move = Direction.Down;
                        break;

                    case ConsoleKey.LeftArrow:
                        move = Direction.Left;
                        break;

                    case ConsoleKey.RightArrow:
                        move = Direction.Right;
                        break;                        

                    default:
                        return;
                }

                SendTurnCommand(move);
            } while (true);
        }

        private static void SendRegisterCommand()
        {
            var request = (HttpWebRequest)WebRequest.Create(string.Format("{0}/register", "http://localhost:5999"));

            var postData = "{Name:'Remote Bot'}";

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

        private static void SendTurnCommand(Direction move)
        {
            var request = (HttpWebRequest)WebRequest.Create(string.Format("{0}/turn", "http://localhost:5999"));
            var moveStr = Enum.GetName(typeof(Direction), move);
            Console.WriteLine(string.Format("Sending message to the server to move - {0}", moveStr));
            var postData = string.Concat("{Name:'Remote Bot', Direction: '", moveStr, "'}");

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
