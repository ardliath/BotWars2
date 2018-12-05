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
        public GameForm()
        {
            InitializeComponent();
        }


        public void StartGame(Arena arena, params Player[] players)
        {
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
                        if (player.IsAlive)
                        {
                            gfx.FillEllipse(Brushes.Aquamarine, new RectangleF(new Point(player.Position.X - 1, player.Position.Y - 1), new Size(3, 3)));
                        }
                        var track = arena.Tracks.SingleOrDefault(t => t.Player.Equals(player));
                        if (track != null)
                        {
                            foreach (var position in track.PreviousPositions)
                            {
                                DrawPixel(gfx, position);
                            }
                        }
                    }
                }

                using (var gfx = this.pictureBox1.CreateGraphics())
                {
                    gfx.DrawImage(bitmap, new PointF(0, 0));
                }
            }
        }

        public void DrawPixel(Graphics gfx, Position position)
        {
            gfx.DrawRectangle(Pens.Red, new Rectangle(position.X, position.Y, 1, 1));
        }
    }
}
