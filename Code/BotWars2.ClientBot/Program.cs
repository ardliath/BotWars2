﻿using BotWars2.ClientBot.Framework;
using BotWars2.ClientBot.Messages;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
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
        public static MyBot Bot { get; set; }

        public static string ServerAddress { get; set; }

        public static HttpListenerClass Listener { get; set; }

        public static ArenaState CurrentGame { get; set; }

   

        static void Main(string[] args)
        {
            Bot = new MyBot();

            var timer = new Timer(SendRegisterCommand, null, 30 * 1000, 10 * 1000);

            Console.WriteLine(@"                                                                            
,--------.                             ,--. ,--.        ,--.                
'--.  .--',--.--. ,---. ,--,--,  ,---. |  | |  |,--,--, `--' ,---. ,--,--,  
   |  |   |  .--'| .-. ||      \(  .-' |  | |  ||      \,--.| .-. ||      \ 
   |  |   |  |   ' '-' '|  ||  |.-'  `)'  '-'  '|  ||  ||  |' '-' '|  ||  | 
   `--'   `--'    `---' `--''--'`----'  `-----' `--''--'`--' `---' `--''--' 
                                                                            ");

            var rand = new Random();
            var bootmessages = new string[]
            {
                "Initialising the reality",
                "Booting the bootstrapper",
                "Syncing the sockets",
                "Calibrating the connection",
                "Porting processes"
            };

            foreach (var joke in bootmessages.OrderBy(m => rand.Next()))
            {
                if (rand.Next(0, 100) > 50)
                {
                    Console.WriteLine($"{joke}...");
                    Thread.Sleep(rand.Next(500, 1000));                    
                }
            }

            Console.WriteLine("Would you like to:");
            Console.WriteLine(" 1 - Connect to Game");
            Console.WriteLine(" 2 - Connect to Test Server");
            Console.WriteLine(" ");

            var key = Console.ReadKey().Key;
            Console.WriteLine();
            Console.WriteLine();
            if (key == ConsoleKey.D1)
            {
                ServerAddress = ConfigurationManager.AppSettings["BotWarsServer"];
                Console.WriteLine("Forcing bot into virtual reality construct...");
            }
            else if(key == ConsoleKey.D2)
            {                
                Console.WriteLine("Isolating bot in test environment...");

                var p = new Process();
                var rootPath = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory)
                    .Parent // bin
                    .Parent // ClientBot
                    .Parent.FullName; // Code

                var path = string.Concat(rootPath, @"\BotWars2Server\bin\debug\BotWars2Server.exe");
                p.StartInfo.FileName = path;
                p.Start();
                ServerAddress = "http://localhost";
                Thread.Sleep(1000);
            }


            SendRegisterCommand();
            Console.WriteLine("Bot deployed!");


            Console.WriteLine("Ready Command sent waiting for game to begin...");
            Listener = new HttpListenerClass(6999, Bot);
            Listener.Initialize();                        
        }

        private static void SendRegisterCommand(object state = null)
        {            
            var request = (HttpWebRequest)WebRequest.Create(string.Format("{0}/register", ServerAddress));

            var postData = new Register
            {
                Name = Bot.Name,
                SecretCommandCode = Bot.SecretCommandCode,
                Uri = new Uri("http://" + Environment.MachineName + ":6999")
            };

            var data = Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(postData));

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
            Console.WriteLine("Server communication confirmed");
        }
    }
}
