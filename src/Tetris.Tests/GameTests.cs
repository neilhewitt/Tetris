using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tetris.Core;

namespace Tetris.Tests
{
    [TestFixture]
    public class GameTests
    {
        [Test]
        public void CanStartANewGame()
        {
            Game game = null;
            Assert.DoesNotThrow(() =>
            {
                game = new Game();
            });
            Assert.That(game != null);
        }
    }
}
