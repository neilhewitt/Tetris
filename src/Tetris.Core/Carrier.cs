using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tetris.Core
{
    public class TetrominoCarrier
    {
        private Matrix _matrix;

        public Tetromino Tetromino { get; private set; }
        public Position Position { get; private set; }

        public void InjectTetromino(Tetromino t, int column)
        {
            // add to row 0, column 0
            Tetromino = t;
            Position = new Position(0, column);
        }

        public void RemoveTetromino()
        {
            Tetromino = null;
            Position = null;
        }

        public bool Move()
        {
            lock (this)
            {
                Tetromino t = Tetromino;
                int row = Position.Row;
                int column = Position.Column;

                // we can move down if:
                // 1. the matrix cells beneath all of the active cells in our bottom row in the current rotation are empty
                // 2. the active cells in our bottom row are not already on the last row

                bool cellsAreEmpty = true;

                // because of rotation, we can't be sure which rows of the tetromino grid the actual parts of the tetromino are on
                // so we ask the tetromino to tell us which positions are occupied and select the max row

                int bottomRowOfTetromino = t.ActivePositions.Select(x => x.Row).Max();
                int[] activeCellsInRow = t.ActivePositions.Where(x => x.Row == bottomRowOfTetromino).Select(x => x.Column).OrderBy(x => x).ToArray();
                int gridRowBelowTetromino = row + bottomRowOfTetromino + 1;

                if (gridRowBelowTetromino >= _matrix.Rows)
                {
                    // we're at the bottom
                    return false;
                }
                else
                {
                    foreach (int activeCell in activeCellsInRow)
                    {
                        if (_matrix.CellAt(gridRowBelowTetromino, activeCell).Colour != TetrominoColour.Empty)
                        {
                            cellsAreEmpty = false;
                            break;
                        }
                    }

                    if (cellsAreEmpty)
                    {
                        Tetromino = t;
                        Position = new Position(row + 1, column);

                        return true;
                    }
                }
            }

            return false;
        }


        public TetrominoCarrier(Matrix matrix)
        {
            _matrix = matrix;
        }
    }
}
