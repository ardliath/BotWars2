using BotWars2Server.Code.Logic;
using BotWars2Server.Code.State;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotWars2Server.Tests.LogicTests.GameManagerTests
{
    class CheckRadarScanner
    {
        [Test]
        public void RadarScanner_Detects_Walls_and_Players()
        {
            var arena = CreateArena(new Position(5, 50), new Position(5, 15), true);

            var scan = Radar.Scan(arena.Players.First(), arena);

            Assert.IsTrue(scan.Up == 35);
            Assert.IsTrue(scan.Right == 95);
            Assert.IsTrue(scan.Down == 50);
            Assert.IsTrue(scan.Left == 5);
        }

        [Test]
        public void RadarScannar_Detects_Tracks()
        {
            var arena = CreateArena(new Position(5, 50), new Position(5, 15), true);
            List<Track> tracks = new List<Track>();
            Track t = new Track(arena.Players.First());
            t.PreviousPositions.Add(new Position(5, 40));
            t.PreviousPositions.Add(new Position(5, 65));
            t.PreviousPositions.Add(new Position(21, 50));
            t.PreviousPositions.Add(new Position(3, 50));
            tracks.Add(t);
            arena.Tracks = tracks;

            var scan = Radar.Scan(arena.Players.First(), arena);

            Assert.IsTrue(scan.Up == 10);
            Assert.IsTrue(scan.Right == 16);
            Assert.IsTrue(scan.Down == 15);
            Assert.IsTrue(scan.Left == 2);
        }
        
        [Test]
        public void RadarScanner_Detects_Enemy()
        {
            var arena = CreateArena(new Position(5, 50), new Position(5, 15), true);
            var scan = Radar.Scan(arena.Players.First(), arena);

            Assert.AreEqual(scan.EnemyPositions[0].X, 5);
            Assert.AreEqual(scan.EnemyPositions[0].Y, 15);

            Assert.AreEqual(scan.MyPosition.X, 5);
            Assert.AreEqual(scan.MyPosition.Y, 50);

        }

        public Arena CreateArena(Position playerOnePosition, Position playerTwoPosition, bool AddWalls)
        {
            var players = new Player[]
            {
                new Mock<Player>(Guid.NewGuid().ToString()).Object,
                new Mock<Player>(Guid.NewGuid().ToString()).Object
            };
            players.ElementAt(0).Position = playerOnePosition;
            players.ElementAt(1).Position = playerTwoPosition;
            var arena = new Code.State.Arena
            {
                Height = 100,
                Width = 100,
                Players = players,
                Tracks = new Track[]
                {
                    new Track(players.First()),
                    new Track(players.Last())
                },
            };

            if (AddWalls)
            {


                List<Wall> walls = new List<Wall>();

                var thisWall = new Wall();
                for (int i = 0; i < arena.Width; i++) thisWall.Add(new Position(i, 0));
                walls.Add(thisWall);

                thisWall = new Wall();
                for (int i = 0; i < arena.Width; i++) thisWall.Add(new Position(i, arena.Height));
                walls.Add(thisWall);

                thisWall = new Wall();
                for (int i = 0; i < arena.Height; i++) thisWall.Add(new Position(0, i));
                walls.Add(thisWall);

                thisWall = new Wall();
                for (int i = 0; i < arena.Height; i++) thisWall.Add(new Position(arena.Width, i));
                walls.Add(thisWall);

                arena.Walls = walls;
            }

            return arena;
        }
    }
}
