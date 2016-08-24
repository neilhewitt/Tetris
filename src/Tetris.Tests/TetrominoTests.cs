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
    public class TetrominoTests
    {
        [Test]
        public void CanMakeTetromino()
        {
            Tetromino t = null;
            Assert.DoesNotThrow(() =>
            {
                t = Tetromino.I(null);
            });
            Assert.That(t != null);
        }

        [Test]
        public void CanRotateTetrominoClockwise()
        {
            Tetromino t = Tetromino.I(null);
            t.RotateClockwise();
            Assert.That(t.Pattern == "0010\n0010\n0010\n0010");
        }

        [Test]
        public void CanRotateTetrominoAnticlockwise()
        { 
            Tetromino t = Tetromino.I(null);
            t.RotateAnticlockwise();
            Assert.That(t.Pattern == "0100\n0100\n0100\n0100");
        }
    }
}
