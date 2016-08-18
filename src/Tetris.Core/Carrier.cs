using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tetris.Core
{
    public class Carrier
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

                int bottomRowOfTetrominoIndex = t.PopulatedCells.Select(x => x.Row).Max();
                IEnumerable<int> activeCellsInRow = t.PopulatedCells.Where(x => x.Row == bottomRowOfTetrominoIndex).Select(x => x.Column);
                int gridRowBelowTetrominoIndex = row + bottomRowOfTetrominoIndex + 1;

                if (gridRowBelowTetrominoIndex >= _matrix.Rows)
                {
                    // we're at the bottom
                    return false;
                }
                else
                {
                    foreach (int activeCell in activeCellsInRow)
                    {
                        if (_matrix.GridWithoutCarrier.GetCell(gridRowBelowTetrominoIndex, activeCell).Contents != (int)TetrominoColour.Empty)
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


        public Carrier(Matrix matrix)
        {
            _matrix = matrix;
        }
    }
}
