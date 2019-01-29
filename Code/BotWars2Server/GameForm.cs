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
            this.Commander.RegisterPlayersActiveInGame(players.OfType<RemoteBot>()); // register the remote bots with the listener
            var gameManager = new GameManager(arena, players);

            gameManager.Play(Update);
        }

        public void Update(Arena arena, int tick)
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

                    foreach(var wall in arena.Walls)
                    {
                        foreach (var brick in wall)
                        {
                            var transformedBrick = this.TransformBrick(brick, wall, tick);
                            DrawPreviousPosition(gfx, transformedBrick, arena);
                        }
                    }
                }

                using (var gfx = this.pictureBox1.CreateGraphics())
                {
                    gfx.DrawImage(bitmap, new PointF(0, 0));
                }
            }
        }

        private Position TransformBrick(Position brick, Wall wall, int tick)
        {
            if(wall.DoesMove)
            {
                var cycleTick = tick % wall.MovementCycle;
                var isOnReturnJourney = ((tick - cycleTick) / 2) % 2 == 1;                

                if(isOnReturnJourney) // if we're on the way back
                {
                    var returnJourneyOffset = isOnReturnJourney ? -1 : 1;

                    int actualX = brick.X // then our position is X (the origin)
                        + (wall.MovementCycle * wall.MovementTransform.X) // added to a full movement cycle of the wall
                        + (cycleTick * wall.MovementTransform.X * returnJourneyOffset); // subtract the tick we're on

                    int actualY = brick.Y 
                        + (wall.MovementCycle * wall.MovementTransform.Y) 
                        + (cycleTick * wall.MovementTransform.Y * returnJourneyOffset);

                    return new Position(actualX, actualY);
                }
                else
                {
                    return new Position(brick.X + (cycleTick * wall.MovementTransform.X),
                        brick.Y + (cycleTick * wall.MovementTransform.Y));
                }
            }
            else
            {
                return brick;
            }
        }

        public void DrawPreviousPosition(Graphics gfx, Position position, Arena arena)
        {
            var offset = arena.Zoom / 2;
            gfx.DrawRectangle(Pens.Red, new Rectangle(position.X - offset, position.Y - offset, arena.Zoom, arena.Zoom));
        }
    }
}
