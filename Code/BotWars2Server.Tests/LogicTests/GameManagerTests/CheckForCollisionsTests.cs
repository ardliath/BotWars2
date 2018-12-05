using BotWars2Server.Code.Logic;
using BotWars2Server.Code.State;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotWars2Server.Tests.LogicTests.GameManagerTests
{
    public class CheckForCollisionsTests
    {
        [Test]
        public void When_second_player_colides_with_first_then_they_are_destroyed()
        {
            var arena = CreateArena(new Position(5, 50), new Position(5, 15), (a) =>
            {
                for (int i = 0; i < a.Players.First().Position.Y; i++)
                {
                    a.Tracks.ElementAt(0).PreviousPositions.Add(new Position(a.Players.First().Position.X, i));
                }
            });

            GameManager.CheckForCollisions(arena);

            Assert.IsFalse(arena.Players.Last().IsAlive);
            Assert.IsTrue(arena.Players.First().IsAlive);
        }

        [Test]
        public void When_first_player_colides_with_second_then_they_are_destroyed()
        {
            var arena = CreateArena(new Position(50, 50), new Position(75, 50), (a) =>
            {
                for (int i = 25; i < a.Players.Last().Position.X; i++)
                {
                    a.Tracks.ElementAt(0).PreviousPositions.Add(new Position(i, a.Players.Last().Position.Y));
                }
            });

            GameManager.CheckForCollisions(arena);

            Assert.IsTrue(arena.Players.Last().IsAlive);
            Assert.IsFalse(arena.Players.First().IsAlive);
        }

        public Arena CreateArena(Position playerOnePosition, Position playerTwoPosition, Action<Arena> createTracks)
        {
            var players = new Player[]
            {
                new Player(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), playerOnePosition),
                new Player(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), playerTwoPosition)
            };
            var arena = new Arena
            {
                Height = 100,
                Width = 100,
                Players = players,
                Tracks = new Track[]
                {
                    new Track(players.First()),
                    new Track(players.Last())
                }
            };
            createTracks(arena);

            return arena;
        }
    }
}
