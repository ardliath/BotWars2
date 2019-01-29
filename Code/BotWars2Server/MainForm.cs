using BotWars2Server.Code.Communication;
using BotWars2Server.Code.HouseBots;
using BotWars2Server.Code.Logic;
using BotWars2Server.Code.State;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BotWars2Server
{
    public partial class MainForm : Form
    {
        public List<Player> Players { get; set; }
        public GameForm GameForm { get; set; }


        public MainForm(Commander commander)
        {
            InitializeComponent();

            Commander = commander;
            this.Players = new List<Player>();
            this.Commander.RegisterRegistrationAction(this.RegisterNewPlayer);
            this.GameForm = new GameForm(this.Commander);


            this.Players.Add(new RandomBot());
        }

        public Commander Commander { get; }

        public void RegisterNewPlayer(RegisterData data)
        {
            var key = data.Name;
            var existingPlayer = this.Players.OfType<RemoteBot>()?.SingleOrDefault(p => p.Name == key);
            if (existingPlayer == null)
            {
                lock (this.Players)
                {
                    this.Players.Add(new RemoteBot(data.Name, "http://localhost:6999"));
                    this.ListPlayers();
                }
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            this.ListPlayers();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            this.GameForm.Show();
            this.GameForm.StartGame(new Arena
            {
                Height = 200,
                Width = 200,
            },
            this.Players.ToArray());            
        }

        public delegate void ListPlayersDelegate();

        public void ListPlayers()
        {
            if (InvokeRequired)
            {
                Invoke(new ListPlayersDelegate(ListPlayers));
            }
            else
            {
                this.label1.Text = string.Join(", ", this.Players.Select(p => p.Name));
            }        
        }
    }
}
