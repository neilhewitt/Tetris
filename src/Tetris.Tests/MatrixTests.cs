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
                matrix = new Matrix(null);
            });
            Assert.That(matrix != null);
        }

        [Test]
        public void CanInjectTetromino()
        {
            Tetromino t = Tetromino.O;
            Matrix matrix = new Matrix(null);

            Assert.DoesNotThrow(() =>
            {
                matrix.Inject(t);
            });
            Assert.That(matrix.TetrominoState == "O:22,0");
        }
    }
}
