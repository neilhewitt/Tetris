using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tetris.Core
{
    public class Overlay
    {
        private Matrix _matrix;

        public Tetromino TetrominoInPlay { get; private set; }
        public Position TetrominoPosition { get; private set; }

        public void InjectTetromino(Tetromino t)
        {
            // add to row 0, column 0
            TetrominoInPlay = t;
            TetrominoPosition = new Position(0, 0);
        }

        public bool Move()
        {
            Tetromino t = TetrominoInPlay;
            int row = TetrominoPosition.Row;
            int column = TetrominoPosition.Column;

            if (row < (23 - t.BoundingSquareSize))
            {
                t.RotateClockwise();
                lock (this)
                {
                    TetrominoInPlay = t;
                    TetrominoPosition = new Position(row + 1, column);

                }
                return true;
            }

            return false;
        }


        public Overlay(Matrix matrix)
        {
            _matrix = matrix;
        }
    }
}
