using BotWars2Server.Code.Communication;
using BotWars2Server.Code.HouseBots;
using BotWars2Server.Code.Logic;
using BotWars2Server.Code.State;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BotWars2Server
{
    public partial class MainForm : Form
    {
        public List<Player> Players { get; set; }
        public GameForm GameForm { get; set; }

        public Timer RefreshTimer { get; set; }

        public Dictionary<string, int> Scoreboard { get; set; }

        public bool StartSelected { get; set; }
        public int RoundSelected { get; set; }

        public Commander Commander { get; }


        public MainForm(Commander commander)
        {
            InitializeComponent();

            Commander = commander;
            this.Players = new List<Player>();
            this.Commander.RegisterRegistrationAction(this.RegisterNewPlayer);
            this.GameForm = new GameForm(this.Commander);
            this.Scoreboard = new Dictionary<string, int>();

            this.Players.Add(new ClaustroBot());
            this.Players.Add(new SirRound());
            this.Players.Add(new Spyro());
            this.Players.Add(new Hunter());
            foreach (var player in Players)
            {
                if (!this.Scoreboard.ContainsKey(player.Name))
                {
                    this.Scoreboard[player.Name] = 0;
                }
            }

            this.RoundSelected = 1;
            this.StartSelected = true;
        }

        public void RegisterNewPlayer(RegisterData data)
        {
            var key = data.SecretCommandCode;
            var existingPlayer = this.Players.OfType<RemoteBot>()?.SingleOrDefault(p => p.SecretCommandCode == key);
            if (existingPlayer == null)
            {
                lock (this.Players)
                {
                    var newPlayer = new RemoteBot(data.Name, data.Uri, data.SecretCommandCode, DateTime.Now);
                    newPlayer.BikeBrush = GetBrush(this.Players.Count);
                    this.Players.Add(newPlayer);
                    if (!this.Scoreboard.ContainsKey(newPlayer.Name))
                    {
                        this.Scoreboard[newPlayer.Name] = 0;
                    }
                    this.UpdateScreen();
                }
            }
            else
            {
                existingPlayer.LastPinged = DateTime.Now;
            }
        }

        public Brush GetBrush(int index)
        {
            var commonColours = new Brush[]
            {
                Brushes.Red,
                Brushes.Blue,
                Brushes.Orange,
                Brushes.Purple,
                Brushes.Green,
                Brushes.Pink,
                Brushes.PowderBlue,
                Brushes.PaleTurquoise,
                Brushes.Orchid,
                Brushes.Olive,
                Brushes.AliceBlue,
                Brushes.Azure,
                Brushes.BlueViolet,
                Brushes.Brown,
                Brushes.CadetBlue
            };

            if (index < commonColours.Count())
            {
                return commonColours.ElementAt(index);
            }

            Type brushesType = typeof(Brushes);

            PropertyInfo[] properties = brushesType.GetProperties();

            var rnd = new Random(Guid.NewGuid().GetHashCode());
            int random = rnd.Next(properties.Length);
            return (Brush)properties[random].GetValue(null, null);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            this.UpdateScreen();

            this.RefreshTimer = new Timer();
            this.RefreshTimer.Interval = 250;
            this.RefreshTimer.Tick += RefreshTimer_Tick;
            this.RefreshTimer.Start();
            this.RestoreScores();

        }

        private void RestoreScores()
        {
            if (File.Exists("scores.json"))
            {
                try
                {
                    var data = JsonConvert.DeserializeObject<KeyValuePair<string, int>[]>(File.ReadAllText("scores.json"));
                    foreach(var datum in data)
                    {
                        if(!this.Scoreboard.ContainsKey(datum.Key))
                        {
                            this.Scoreboard.Add(datum.Key, datum.Value);
                        }
                        else
                        {
                            this.Scoreboard[datum.Key] = datum.Value;
                        }
                    }
                }
                catch (Exception) // if we can't then they'll all reset to zero
                {

                }
            }
        }

        private void SaveScores()
        {
            var data = this.Scoreboard.Select(x => new
            {
                x.Key,
                x.Value
            }).ToArray();
            File.WriteAllText("scores.json", JsonConvert.SerializeObject(data));
        }

        private void RefreshTimer_Tick(object sender, EventArgs e)
        {
            this.UpdateScreen();
        }

        public delegate void ListPlayersDelegate();

        public void UpdateScreen()
        {
            if (InvokeRequired)
            {
                Invoke(new ListPlayersDelegate(UpdateScreen));
            }
            else
            {
                var bitmap = new Bitmap(this.pictureBox1.Width, this.pictureBox1.Height);
                using (var gfx = Graphics.FromImage(bitmap))
                {
                    gfx.FillRectangle(Brushes.Black, 0, 0, bitmap.Width, bitmap.Height);

                    const int bannerHeight = 150;
                    const int padding = 5;

                    gfx.DrawString("TronsUnion", new Font(Font.FontFamily, 64, FontStyle.Bold), Brushes.White, 125, 20);
                    gfx.DrawString($"Round {this.RoundSelected}", new Font(Font.FontFamily, 24, FontStyle.Bold), Brushes.White, 300, 115);
                    gfx.DrawLine(Pens.White, padding, bannerHeight + padding, bitmap.Width - padding, bannerHeight + padding);

                    int startPosition = bannerHeight + 10;
                    for (int i = 0; i < this.Players.Count; i++)
                    {
                        var player = this.Players.OrderByDescending(p => Scoreboard[p.Name]).ElementAt(i);
                        var remote = player as RemoteBot;
                        var isActive = true;
                        if (remote != null && DateTime.Now.Subtract(remote.LastPinged) > new TimeSpan(0, 1, 0))
                        {
                            isActive = false;
                        }

                        var brush = isActive ? Brushes.White : Brushes.DimGray;

                        var score = this.Scoreboard[player.Name];
                        var pointSuffix = score == 1 ? "" : "s";
                        gfx.DrawString(player.Name, new Font(Font.FontFamily, 12, FontStyle.Bold), brush, 20, startPosition + (i * 25));
                        gfx.DrawString($"{score} point{pointSuffix}", new Font(Font.FontFamily, 12, FontStyle.Bold), brush, 300, startPosition + (i * 25));
                        if (remote != null)
                        {
                            var diff = DateTime.Now.Subtract(remote.LastPinged);
                            var lastSeen = $"Last Seen {(int)diff.TotalSeconds} seconds ago";
                            gfx.DrawString(lastSeen, new Font(Font.FontFamily, 12, FontStyle.Bold), brush, 500, startPosition + (i * 25));
                        }
                    }


                    DrawButton(gfx, bitmap, "Start", 0, StartSelected, Brushes.Green);
                    DrawButton(gfx, bitmap, "Cancel", 1, !StartSelected, Brushes.Red);
                }

                this.pictureBox1.Image = bitmap;
            }
        }

        private void DrawButton(Graphics gfx, Bitmap bitmap, string text, int positionFromRight, bool selected, Brush colour)
        {
            const int buttonWidth = 100;
            const int buttonHeight = 25;
            const int buttonSpacing = 5;
            int startX = bitmap.Width - buttonSpacing - buttonWidth - ((buttonWidth + buttonSpacing) * positionFromRight);
            int startY = bitmap.Height - 30;

            gfx.FillRectangle(colour, startX, startY, buttonWidth, buttonHeight);
            gfx.DrawString(text,
                new Font(Font.FontFamily, 12, FontStyle.Bold),
                selected ? Brushes.White : Brushes.DimGray,
                startX + buttonSpacing + buttonSpacing,
                startY + buttonSpacing);
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyData)
            {
                case Keys.Enter:
                    if (this.StartSelected)
                    {
                        this.StartGame();
                    }
                    else
                    {
                        this.Close();
                    }
                    break;

                case Keys.Left:
                case Keys.Right:
                    this.StartSelected = !this.StartSelected;
                    break;
                case Keys.Up:
                    if (this.RoundSelected < 4)
                    {
                        this.RoundSelected++;
                    }
                    break;
                case Keys.Down:
                    if (RoundSelected > 1)
                    {
                        this.RoundSelected--;
                    }
                    break;
            }
        }

        public void StartGame()
        {
            foreach (var player in Players)
            {
                if (player.BikeBrush == null)
                {
                    var index = this.Players.IndexOf(player);
                    player.BikeBrush = GetBrush(index);
                }
            }

            var arena = this.GetArena();
            var thisRoundPlayers = this.Players.Where(p => p.PlayableRounds.Contains(this.RoundSelected));
            var games = GameManager.CreateGames(thisRoundPlayers.ToList(), arena);

            if (games.Any())
            {
                this.GameForm.Show();
                Player winner;
                Dictionary<string, int> scores = new Dictionary<string, int>();
                var currentSpeed = games.First().Arena.Speed;
                foreach (var game in games)
                {
                    game.Arena.Speed = currentSpeed;
                    this.GameForm.StartGame(game);

                    currentSpeed = game.Arena.Speed; // copy the speed from the last game over to the new one
                    winner = game.Players.SingleOrDefault(p => p.IsAlive);
                    if (winner != null)
                    {
                        var points = game.Arena.ArenaOptions.PlayAllPlayersInSingleGame
                            ? game.Players.Count() - 1
                            : 1;

                        Scoreboard[winner.Name] += points;

                        if (!scores.Any(p => p.Key == winner.Name))
                        {
                            scores.Add(winner.Name, 0);
                        }
                        scores[winner.Name] += points;

                        this.SaveScores();
                    }
                }
                this.GameForm.Hide();



                foreach (var player in this.Players)
                {
                    try
                    {
                        player.SendEndRound(scores,
                            scores.ContainsKey(player.Name) ? scores[player.Name] : 0,
                            games.Count(),
                            games.Count(g => g.Players.Contains(player)));
                    }
                    catch (Exception) // swallow issues sending start message to bot
                    {
                    }
                }
            }
        }

        private Arena GetArena()
        {
            Arena arena;
            switch (this.RoundSelected)
            {
                case 1:
                    arena = new Arena
                    {
                        Height = 20,
                        Width = 30
                    };
                    arena.ArenaOptions.InteriorWalls = 0;
                    arena.ArenaOptions.MovingWalls = 0;
                    arena.Zoom = 10;
                    return arena;

                case 2:
                    arena = new Arena
                    {
                        Height = 30,
                        Width = 30
                    };
                    arena.ArenaOptions.InteriorWalls = 2;
                    arena.ArenaOptions.MovingWalls = 0;
                    arena.Zoom = 10;
                    return arena;

                case 3:
                    arena = new Arena
                    {
                        Height = 30,
                        Width = 30
                    };
                    arena.ArenaOptions.InteriorWalls = 3;
                    arena.ArenaOptions.MovingWalls = 1;
                    arena.Zoom = 10;
                    return arena;

                case 4:
                    arena = new Arena
                    {
                        Height = 40,
                        Width = 40
                    };
                    arena.ArenaOptions.InteriorWalls = 3;
                    arena.ArenaOptions.MovingWalls = 0;
                    arena.Zoom = 10;
                    arena.ArenaOptions.PlayAllPlayersInSingleGame = true;
                    return arena;

                default:
                    return new Arena
                    {
                        Height = 100,
                        Width = 100,
                    };
            }
        }
    }
}
