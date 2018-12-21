using BotWars2Server.Code.Communication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BotWars2Server
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            var listener = new HttpListenerClass(5999, new Code.HouseBots.RandomBot());
            Application.Run(new MainForm());
        }
    }
}
