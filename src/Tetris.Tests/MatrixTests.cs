using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tetris.Core;

namespace Tetris.Tests
{
    [TestFixture]
    public class BoardTests
    {
        [Test]
        public void CanCreateBoard()
        {
            GameBoard Board = null;

            Assert.DoesNotThrow(() =>
            {
                Board = new GameBoard();
            });
            Assert.That(Board != null);
        }

        //[Test]
        //public void CanInjectTetromino()
        //{
        //    Tetromino t = Tetromino.O;
        //    Board Board = new Board();

        //    Assert.DoesNotThrow(() =>
        //    {
        //        Board._overlay.InjectTetromino(t);
        //    });
        //    Assert.That(Board._overlay.TetrominoInPlay.Name == 'O' && Board._overlay.TetrominoPosition.Column == 0 && Board._overlay.TetrominoPosition.Row == 0);
        //}
    }
}
