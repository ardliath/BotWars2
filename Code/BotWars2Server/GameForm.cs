using BotWars2Server.Code.Communication;
using BotWars2Server.Code.Logic;
using BotWars2Server.Code.State;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BotWars2Server
{
    public partial class GameForm : Form
    {
        public ICommander Commander { get; }

        public Game CurrentGame { get; set; }

        public GameForm(ICommander commander)
        {
            InitializeComponent();
            Commander = commander;
        }


        public void StartGame(Game game)
        {
            this.CurrentGame = game;

            this.Height = game.Arena.Height * game.Arena.Zoom + (game.Arena.Zoom * 8);
            this.Width = game.Arena.Width * game.Arena.Zoom + (game.Arena.Zoom * 4) + 200;

            this.Commander.RegisterPlayersActiveInGame(game.Players.OfType<RemoteBot>()); // register the remote bots with the listener
            var gameManager = new GameManager(game);

            var finalTick = gameManager.Play(Update);

            PlayVictoryAnimation(finalTick);

            this.CurrentGame = null;
        }

        private void PlayVictoryAnimation(int finalTick)
        {
            var winner = this.CurrentGame.Arena.Players.SingleOrDefault(p => p.IsAlive);
            var bitmap = DrawGameScreen(this.CurrentGame.Arena, finalTick);

            string message = winner != null ? $"{winner.Name} Wins!" : "Draw!";

            for (int i = 1; i <= 12; i+=3)
            {
                using (var frameBitmap = new Bitmap(bitmap))
                {
                    using (var frameGfx = Graphics.FromImage(frameBitmap))
                    {
                        using (var gfx = this.pictureBox1.CreateGraphics())
                        {
                            frameGfx.DrawString(message,
                                new Font(Font.FontFamily, 2 * i, FontStyle.Bold),
                                Brushes.White,
                                new RectangleF((this.pictureBox1.Width / 2) - (i * 10),
                                    this.pictureBox1.Height / 2 - 50,
                                    this.pictureBox1.Width,
                                    this.pictureBox1.Height));

                            gfx.DrawImage(frameBitmap, new PointF(0, 0));
                            Thread.Sleep(1);
                        }
                    }
                }
            }

            Thread.Sleep(1000);
        }

        public Bitmap DrawGameScreen(Arena arena, int tick)
        {
            var bitmap = new Bitmap(this.pictureBox1.Width * arena.Zoom, pictureBox1.Height * arena.Zoom, PixelFormat.Format16bppRgb555);
            using (var gfx = Graphics.FromImage(bitmap))
            {
                var labelX = this.Width - 200;

                gfx.FillRectangle(Brushes.Black, new Rectangle(0, 0, this.pictureBox1.Width * arena.Zoom, this.pictureBox1.Height * arena.Zoom));

                int playerIndex = 0;
                foreach (var player in arena.Players)
                {
                    var track = arena.Tracks.SingleOrDefault(t => t.Player.Equals(player));
                    if (track != null)
                    {
                        foreach (var position in track.PreviousPositions)
                        {
                            DrawPreviousPosition(gfx, position, arena, player.BikeBrush);
                        }
                    }
                    if (player.IsAlive)
                    {
                        var offset = arena.Zoom / 2;

                        DrawPlayer(gfx,
                            player,
                            (player.Position.X * arena.Zoom) - offset,
                            (player.Position.Y * arena.Zoom) - offset,
                        arena.Zoom * 2);
                    }

                    var labelY = 30 + (25 * playerIndex);
                    DrawPlayer(gfx,
                            player,
                            labelX,
                            labelY,
                        arena.Zoom * 2);

                    var botNameBrush = player.IsAlive ? Brushes.White : Brushes.DimGray;
                    gfx.DrawString(player.Name, new Font(Font.FontFamily, 12, FontStyle.Bold), botNameBrush, labelX + 25, labelY);

                    playerIndex++;
                }

                foreach (var wall in arena.Walls)
                {
                    foreach (var brick in wall.TransformBricks(arena, tick))
                    {
                        DrawPreviousPosition(gfx, brick, arena);
                    }
                }

                gfx.DrawString($"Speed: {arena.Speed}", new Font(Font.FontFamily, 12, FontStyle.Bold), Brushes.Blue, labelX + 10, this.Height - 75 - (arena.Zoom / 2));
            }

            return bitmap;
        }

        public void Update(Arena arena, int tick)
        {            
            using (var gfx = this.pictureBox1.CreateGraphics())
            {
                var bitmap = DrawGameScreen(arena, tick);
                gfx.DrawImage(bitmap, new PointF(0, 0));
            }
        }

        private void DrawPlayer(Graphics gfx, Player player, int x, int y, int size)
        {
            gfx.FillEllipse(player.BikeBrush,
                new RectangleF(
                new Point(x, y),
                new Size(size, size)));
        }

        public void DrawPreviousPosition(Graphics gfx, Position position, Arena arena, Brush brush = null)
        {            
            if(brush != null)
            {
                gfx.FillRectangle(brush, new Rectangle(position.X * arena.Zoom, position.Y * arena.Zoom, arena.Zoom, arena.Zoom));
            }
            gfx.DrawRectangle(Pens.Red, new Rectangle(position.X * arena.Zoom, position.Y * arena.Zoom, arena.Zoom, arena.Zoom));
        }

        private void GameForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (this.CurrentGame != null)
            {
                if (e.KeyData == Keys.Up && this.CurrentGame.Arena.Speed > 1)
                {
                    this.CurrentGame.Arena.Speed--;
                }

                if (e.KeyData == Keys.Down && this.CurrentGame.Arena.Speed < 150)
                {
                    this.CurrentGame.Arena.Speed++;
                }
            }
        }
    }
}
