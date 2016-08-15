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

        //[Test]
        //public void CanInjectTetromino()
        //{
        //    Tetromino t = Tetromino.O;
        //    Matrix matrix = new Matrix();

        //    Assert.DoesNotThrow(() =>
        //    {
        //        matrix._overlay.InjectTetromino(t);
        //    });
        //    Assert.That(matrix._overlay.TetrominoInPlay.Name == 'O' && matrix._overlay.TetrominoPosition.Column == 0 && matrix._overlay.TetrominoPosition.Row == 0);
        //}
    }
}
