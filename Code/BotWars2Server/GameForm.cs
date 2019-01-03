using BotWars2Server.Code.Communication;
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
    public partial class GameForm : Form
    {
        public ICommander Commander { get; }

        public GameForm(ICommander commander)
        {
            InitializeComponent();
            Commander = commander;
        }


        public void StartGame(Arena arena, params Player[] players)
        {
            this.Commander.RegisterPlayers(players);
            var gameManager = new GameManager(arena, players);

            gameManager.Play(Update);
        }

        public void Update(Arena arena)
        {
            using (var bitmap = new Bitmap(this.pictureBox1.Width, pictureBox1.Height))
            {
                using (var gfx = Graphics.FromImage(bitmap))
                {
                    gfx.FillRectangle(Brushes.Black, new Rectangle(0, 0, this.pictureBox1.Width, this.pictureBox1.Height));

                    foreach (var player in arena.Players)
                    {
                        var track = arena.Tracks.SingleOrDefault(t => t.Player.Equals(player));
                        if (track != null)
                        {
                            foreach (var position in track.PreviousPositions)
                            {
                                DrawPreviousPosition(gfx, position, arena);
                            }
                        }
                        if (player.IsAlive)
                        {
                            gfx.FillEllipse(Brushes.Aquamarine, new RectangleF(new Point(player.Position.X - arena.Zoom, player.Position.Y - arena.Zoom), new Size(arena.Zoom * 2, arena.Zoom * 2)));
                        }
                    }
                }

                using (var gfx = this.pictureBox1.CreateGraphics())
                {
                    gfx.DrawImage(bitmap, new PointF(0, 0));
                }
            }
        }

        public void DrawPreviousPosition(Graphics gfx, Position position, Arena arena)
        {
            var offset = arena.Zoom / 2;
            gfx.DrawRectangle(Pens.Red, new Rectangle(position.X - offset, position.Y - offset, arena.Zoom, arena.Zoom));
        }
    }
}
