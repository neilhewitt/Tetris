using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using System.Text;

namespace Tetris.Core
{
    public class GameBoard
    {
        private int _rows;
        private int _columns;
        private Grid<int> _grid;
        private Grid<int> _gridWithTetromino;
        private Tetromino _tetromino;

        public Tetromino Tetromino => _tetromino;
        public int Rows => _rows;
        public int Columns => _columns;
        public Grid<int> Grid => _grid.AsReadonly();
        public Grid<int> GridWithTetromino
        {
            get
            {
                lock (this)
                {
                    if (_tetromino != null &&_gridWithTetromino == null && _tetromino != null)
                    {
                        int size = _tetromino.GridSize - 1;
                        Grid<int> grid = _grid.Clone();
                        grid.Insert(_tetromino.Grid, 0, 0, size, size, _tetromino.RowOnBoard, _tetromino.ColumnOnBoard, 
                            (input) => input == 1, (output) => (int)_tetromino.Colour);
                        _gridWithTetromino = grid;
                    }     
                }

                return _gridWithTetromino;
            }
        }

        public void BeginPlay()
        {
            _tetromino = Tetromino.New(name, this);
        }

        public bool Move()
        {
            // clear any filled rows, move grid down
            // then move carrier down one row (if it can move)
            // or freeze the tetromino in place
            lock (this)
            {
                Tetromino t = _tetromino;
                int row = t.RowOnBoard;
                int column = t.ColumnOnBoard;

                // we can move down if:
                // 1. the Board cells beneath all of the active cells in our bottom row in the current rotation are empty
                // 2. the active cells in our bottom row are not already on the last row

                bool cellsAreEmpty = true;

                // because of rotation, we can't be sure which rows of the tetromino grid the actual parts of the tetromino are on
                // so we ask the tetromino to tell us which positions are occupied and select the max row

                int bottomRowOfTetrominoIndex = t.PopulatedCells.Select(x => x.Row).Max();
                int gridRowIndex = row + bottomRowOfTetrominoIndex + 1;

                if (gridRowIndex >= _rows)
                {
                    // we're at the bottom
                    FreezeToBoard();
                    return false;
                }
                else
                {
                    for (int i = 0; i < t.GridSize; i++)
                    {
                        if (t.Grid.GetColumn(i).Any(x => x.Contents > 0))
                        {
                            bottomRowOfTetrominoIndex = t.PopulatedCells.Where(x => x.Column == i).Select(x => x.Row).Max();
                            gridRowIndex = row + bottomRowOfTetrominoIndex + 1;
                            if (_grid.GetCell(gridRowIndex, _tetromino.ColumnOnBoard + i).Contents != (int)TetrominoColour.Empty)
                            {
                                cellsAreEmpty = false;
                                break;
                            }
                        }
                    }

                    if (cellsAreEmpty)
                    {
                        t.MoveDown();
                        _gridWithTetromino = null;
                        return true;
                    }
                    else
                    {
                        FreezeToBoard();
                        return false;
                    }
                }
            }
        }

        public void MoveRight()
        {
            lock (this)
            {
                _gridWithTetromino = null;
                _tetromino.MoveRight();
            }
        }

        public void MoveLeft()
        {
            lock (this)
            {
                _gridWithTetromino = null;
                _tetromino.MoveLeft();
            }
        }

        public bool RotateClockwise()
        {
            lock (this)
            {
                _gridWithTetromino = null;
                return _tetromino.RotateClockwise();
            }
        }

        public bool RotateAnticlockwise()
        {
            lock (this)
            {
                _gridWithTetromino = null;
                return _tetromino.RotateAnticlockwise();
            }
        }

        private void FreezeToBoard()
        {
            Tetromino t = _tetromino;
            int row = _tetromino.RowOnBoard;
            int column = _tetromino.ColumnOnBoard;
            int size = t.GridSize - 1;

            // copy the tetromino shape data to the grid - it's now 'frozen'
            // we only copy squares that have shape data in them, and we mutate to the appropriate colour value for the tetromino
            lock (this)
            {
                _grid.Insert(t.Grid, 0, 0, size, size, row, column, (input) => input == 1, (output) => (int)t.Colour);
                _tetromino = null;
                lock (this)
                {
                    _gridWithTetromino = null;
                }

            }
        }

        public GameBoard(int rows = 22, int columns = 10)
        {
            _rows = rows;
            _columns = columns;
            _grid = new Grid<int>(_rows, _columns);
            _tetromino = null;
        }
    }
}
