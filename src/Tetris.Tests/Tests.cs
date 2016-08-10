using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using NUnit.Framework;
using Tetris.Core;

namespace Tetris.Tests
{
    [TestFixture]
    public class Tests
    {
        [Test]
        public void NUnitWorksAsExpected()
        {
            Assert.That(1 == 1);
        }

        [Test]
        public void CanStartANewGame()
        {
            Game game = new Game();
            Assert.IsNotNull(game);
        }

        [TestCase("1111", TetrominoColour.Red)]
        [TestCase("111\n001", TetrominoColour.Magenta)]
        [TestCase("111\n100", TetrominoColour.Yellow)]
        [TestCase("11\n11", TetrominoColour.Cyan)]
        [TestCase("011\n110", TetrominoColour.Blue)]
        [TestCase("111\n010", TetrominoColour.LightGrey)]
        [TestCase("110\n011", TetrominoColour.Lime)]
        public void CanMakeTetrominoFromPattern(string pattern, TetrominoColour colour)
        {
            Tetromino t = new Tetromino(pattern, colour);
            Assert.That(t.Pattern == pattern);
        }
    }
}
