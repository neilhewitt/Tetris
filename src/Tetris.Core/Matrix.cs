using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using System.Text;

namespace Tetris.Core
{
    public class Matrix
    {
        private int _rows;
        private int _columns;
        private Grid<int> _grid;
        private TetrominoCarrier _carrier;

        public string State => CaptureState();

        public void InjectTetromino(Tetromino t, int column = 0)
        {
            _carrier.InjectTetromino(t, column);
        }

        public bool Move()
        {
            return _carrier.Move();
        }

        public void FreezeTetromino()
        {
            Tetromino t = _carrier.Tetromino;
            int row = _carrier.Position.Row;
            int column = _carrier.Position.Column;
            int size = t.GridSize;

            // copy the tetromino shape data to the grid - it's now 'frozen'
            t.Grid.Copy(0, 0, t.GridSize, t.GridSize, _grid, row, column, (input) => input == 1, (output) => (int)t.Colour);
            _carrier.RemoveTetromino();
        }

        public Cell CellAt(int row, int column)
        {
            if (row < 0 || row >= _rows || column < 0 || column >= _columns) return null;

            return new Cell(row, column, (TetrominoColour)_grid[row, column], this);
        }

        internal int Rows => _rows;
        internal int Columns => _columns;

        private string CaptureState()
        {
            Tetromino t = _carrier.Tetromino;
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < _rows; i++)
            {
                for (int j = 0; j < _columns; j++)
                {
                    int row = _carrier.Position.Row;
                    int column = _carrier.Position.Column;

                    if ((i >= row && j >= column) && (i < row + t.GridSize && j < column + t.GridSize))
                    {
                        ReadonlyGrid<int> grid = t.Grid;
                        int cell = grid[i - row, j - column];
                        builder.Append((cell == 1 ? ((int)t.Colour).ToString() : "0") + ",");
                    }
                    else
                    {
                        builder.Append(_grid[i, j].ToString() + ",");
                    }
                }
                builder.Remove(builder.Length-1, 1);
                builder.Append("|");
            }
            return builder.ToString();
        }

        public Matrix(int rows = 22, int columns = 10)
        {
            _rows = rows;
            _columns = columns;
            _grid = new Grid<int>(_rows, _columns);
            _carrier = new TetrominoCarrier(this);
        }
    }
}
