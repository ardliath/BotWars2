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
    public class IsPositionValidForPlayerTests
    {
        [TestCase(5, 5, 5, 7, false)]
        [TestCase(5, 5, 5, 6, true)]
        [TestCase(5, 5, 5, 5, false)]
        [TestCase(5, 5, 5, 4, true)]
        [TestCase(5, 5, 5, 3, false)]
        [TestCase(5, 5, 7, 5, false)]
        [TestCase(5, 5, 6, 5, true)]
        [TestCase(5, 5, 4, 5, true)]
        [TestCase(5, 5, 3, 5, false)]
        [TestCase(5, 2, 5, 0, false)]
        [TestCase(5, 2, 5, 1, true)]
        [TestCase(5, 2, 5, 2, false)]
        [TestCase(5, 2, 5, 3, true)]
        [TestCase(5, 2, 5, 4, false)]
        [TestCase(5, 5, 6, 6, false)]
        [TestCase(5, 5, 4, 4, false)]
        public void IsMoveValid(int currentX, int currentY, int newX, int newY, bool expectedIsValid)
        {
            var player = new Player(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), new Position { X = currentX, Y = currentY });

            var isValid = GameManager.IsPositionValidForPlayer(player, new Position { X = newX, Y = newY });

            Assert.AreEqual(expectedIsValid, isValid);
        }
    }
}
