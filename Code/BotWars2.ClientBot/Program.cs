using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BotWars2.ClientBot
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Ready to send Register Command...");
            Console.ReadKey();

            SendRegisterCommand();

            Console.Write("Ready to send Turn Command...");
            Console.ReadKey();

            SendTurnCommand();
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

        private static void SendTurnCommand()
        {
            var request = (HttpWebRequest)WebRequest.Create(string.Format("{0}/turn", "http://localhost:5999"));

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
    }
}
