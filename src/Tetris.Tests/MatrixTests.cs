using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tetris.Core;

namespace Tetris.Tests
{
    [TestFixture]
    public class MatrixTests
    {
        [Test]
        public void CanCreateMatrix()
        {
            Matrix matrix = null;

            Assert.DoesNotThrow(() =>
            {
                matrix = new Matrix();
            });
            Assert.That(matrix != null);
        }

        [Test]
        public void CanInjectTetromino()
        {
            Tetromino t = Tetromino.O;
            Matrix matrix = new Matrix();

            Assert.DoesNotThrow(() =>
            {
                matrix.Overlay.InjectTetromino(t);
            });
            Assert.That(matrix.Overlay.TetrominoInPlay.Name == 'O' && matrix.Overlay.TetrominoPosition.Column == 0 && matrix.Overlay.TetrominoPosition.Row == 0);
        }
    }
}
