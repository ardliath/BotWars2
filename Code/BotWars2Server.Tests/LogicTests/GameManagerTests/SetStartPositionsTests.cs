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
    public class SetStartPositionsTests
    {
        [TestCase(250, 250, 200, 125, 50, 125, TestName = "In a 250*250 arena the two players are played at 200,125 and 50,125")]
        [TestCase(500, 500, 450, 250, 50, 250, TestName = "In a 500*500 arena the two players are played at 450,250 and 50,250")]
        [TestCase(250, 500, 450, 125, 50, 125, TestName = "In a 250*500 arena the two players are played at 450,125 and 50,125")]
        public void TwoPlayers(int arenaX, int arenaY, int p1x, int p1y, int p2x, int p2y)
        {
            var arena = CreateArena(arenaX, arenaY);
            var manager = new GameManager(arena, arena.Players.First(), arena.Players.Last());

            manager.SetStartPositions();

            Assert.AreEqual(new Position(p1x, p1y), arena.Players.First().Position);
            Assert.AreEqual(new Position(p2x, p2y), arena.Players.Last().Position);
        }


        public Arena CreateArena(int arenaX, int arenaY)
        {
            var players = new Player[]
            {
                new Mock<Player>(Guid.NewGuid().ToString()).Object,
                new Mock<Player>(Guid.NewGuid().ToString()).Object
            };

            var arena = new Arena
            {
                Height = arenaX,
                Width = arenaY,
                Players = players,
                Tracks = new Track[]
                {
                    new Track(players.First()),
                    new Track(players.Last())
                },
            };
            return arena;
        }
    }
}
