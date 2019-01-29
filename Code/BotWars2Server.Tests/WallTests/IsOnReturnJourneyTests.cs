using BotWars2Server.Code.State;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotWars2Server.Tests.WallTests
{
    public class IsOnReturnJourneyTests
    {
        [TestCase(10, 0, false)]
        [TestCase(10, 10, true)]
        [TestCase(10, 11, true)]
        [TestCase(10, 12, true)]
        [TestCase(10, 19, true)]
        [TestCase(10, 20, false)]
        [TestCase(10, 21, false)]
        public void Test(int cycle, int tick, bool expectedResult)
        {
            Assert.AreEqual(expectedResult, Wall.IsOnReturnJourney(cycle, tick));
        }
    }
}
