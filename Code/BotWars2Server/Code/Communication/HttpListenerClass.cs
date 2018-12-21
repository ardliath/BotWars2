using BotWars2Server.Code.State;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BotWars2Server.Code.Communication
{
    public class HttpListenerClass
    {
        private readonly string[] _indexFiles =
        {
            "index.html",
            "index.htm",
            "default.html",
            "default.htm"
        };
        private readonly Player _bot;
        private IDictionary<string, string> _mimeTypeMappings =
            new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase)
            {
                #region extension to MIME type list
                {".asf", "video/x-ms-asf"},
                {".asx", "video/x-ms-asf"},
                {".avi", "video/x-msvideo"},
                {".bin", "application/octet-stream"},
                {".cco", "application/x-cocoa"},
                {".crt", "application/x-x509-ca-cert"},
                {".css", "text/css"},
                {".deb", "application/octet-stream"},
                {".der", "application/x-x509-ca-cert"},
                {".dll", "application/octet-stream"},
                {".dmg", "application/octet-stream"},
                {".ear", "application/java-archive"},
                {".eot", "application/octet-stream"},
                {".exe", "application/octet-stream"},
                {".flv", "video/x-flv"},
                {".gif", "image/gif"},
                {".hqx", "application/mac-binhex40"},
                {".htc", "text/x-component"},
                {".htm", "text/html"},
                {".html", "text/html"},
                {".ico", "image/x-icon"},
                {".img", "application/octet-stream"},
                {".iso", "application/octet-stream"},
                {".jar", "application/java-archive"},
                {".jardiff", "application/x-java-archive-diff"},
                {".jng", "image/x-jng"},
                {".jnlp", "application/x-java-jnlp-file"},
                {".jpeg", "image/jpeg"},
                {".jpg", "image/jpeg"},
                {".js", "application/x-javascript"},
                {".mml", "text/mathml"},
                {".mng", "video/x-mng"},
                {".mov", "video/quicktime"},
                {".mp3", "audio/mpeg"},
                {".mpeg", "video/mpeg"},
                {".mpg", "video/mpeg"},
                {".msi", "application/octet-stream"},
                {".msm", "application/octet-stream"},
                {".msp", "application/octet-stream"},
                {".pdb", "application/x-pilot"},
                {".pdf", "application/pdf"},
                {".pem", "application/x-x509-ca-cert"},
                {".pl", "application/x-perl"},
                {".pm", "application/x-perl"},
                {".png", "image/png"},
                {".prc", "application/x-pilot"},
                {".ra", "audio/x-realaudio"},
                {".rar", "application/x-rar-compressed"},
                {".rpm", "application/x-redhat-package-manager"},
                {".rss", "text/xml"},
                {".run", "application/x-makeself"},
                {".sea", "application/x-sea"},
                {".shtml", "text/html"},
                {".sit", "application/x-stuffit"},
                {".swf", "application/x-shockwave-flash"},
                {".tcl", "application/x-tcl"},
                {".tk", "application/x-tcl"},
                {".txt", "text/plain"},
                {".war", "application/java-archive"},
                {".wbmp", "image/vnd.wap.wbmp"},
                {".wmv", "video/x-ms-wmv"},
                {".xml", "text/xml"},
                {".xpi", "application/x-xpinstall"},
                {".zip", "application/zip"},

                #endregion
            };

        private Thread _serverThread;
        private HttpListener _listener;
        private int _port;

        public int Port
        {
            get { return _port; }
            private set { }
        }

        /// <summary>
        /// Construct server with given port.
        /// </summary>
        /// <param name="port">Port of the server.</param>
        public HttpListenerClass(int port, Player bot)
        {
            this.Initialize(port);
            _bot = bot;
        }


        /// <summary>
        /// Stop server and dispose all functions.
        /// </summary>
        public void Stop()
        {
            _serverThread.Abort();
            _listener.Stop();
        }

        private void Listen()
        {

            _listener = new HttpListener();
            _listener.Prefixes.Add("http://*:" + _port.ToString() + "/");
            _listener.Start();
            while (true)
            {
                HttpListenerContext context = _listener.GetContext();
                ProcessIncomingMessage(context);
            }
        }


        private void ProcessIncomingMessage(HttpListenerContext context)
        {
            string body = null;
            StreamReader sr = new StreamReader(context.Request.InputStream);
            using (sr)
            {
                body = sr.ReadToEnd();
            }

            var method = context.Request.Url.AbsolutePath.Replace("/", "").ToLower();
            switch (method)
            {
                case "register":
                    ProcessRegisterMessage(body);
                    context.Response.StatusCode = (int)HttpStatusCode.OK;
                    context.Response.ContentType = "text/plain";
                    using (StreamWriter sw = new StreamWriter(context.Response.OutputStream))
                    {
                        sw.WriteLine(context.Request.RawUrl);
                    }

                    break;

                case "turn":
                    ProcessTurnMessage(body);
                    context.Response.StatusCode = (int)HttpStatusCode.OK;
                    context.Response.ContentType = "text/plain";
                    using (StreamWriter sw = new StreamWriter(context.Response.OutputStream))
                    {
                        using (sw)
                        {
                            sw.WriteLine(context.Request.RawUrl);
                        }
                    }

                    break;
            }
        }

        private void ProcessRegisterMessage(string messageBody)
        {
            //int health = 0;
            //int flips = 0;
            //int flipOdds = 0;
            //int fuel = 0;
            //int arenaSize = 0;
            //char direction = 'R';
            //int startPosition = 0;

            //string opponentName = string.Empty;

            //string[] parameters = messageBody.Split(new char[] { '&' });
            //foreach (var parameter in parameters)
            //{
            //    if (parameter.Contains('='))
            //    {
            //        string paramName = parameter.Split(new char[] { '=' })[0];
            //        string paramValue = parameter.Split(new char[] { '=' })[1];
        

            //Console.WriteLine(string.Format("START Opponentname={0} Health={1} ArenaSize= {2} Flips={3} FlipOdds={4} Direction={5} Fuel={6} StartPosition={7}",
            //    opponentName,
            //    health,
            //    arenaSize,
            //    flips,
            //    flipOdds,
            //    direction,
            //    fuel,
            //    startPosition));

            //_bot.SetStartValues(opponentName, health, arenaSize, flips, flipOdds, fuel, direction, startPosition);
        }

        private void ProcessTurnMessage(string messageBody)
        {            
        }


        private void Initialize(int port)
        {
            this._port = port;
            _serverThread = new Thread(this.Listen);
            _serverThread.Start();
            Console.WriteLine(string.Format("Bot initialised on port {0}.", port));
        }
    }
}
