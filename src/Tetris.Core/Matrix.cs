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
        private const int _rows = 22;
        private const int _columns = 10;
        private int[,] _grid;

        public string State => CaptureState();
        public Overlay Overlay { get; private set; }

        public Cell CellAt(int row, int column)
        {
            if (row < 0 || row > _rows || column < 0 || column > _columns) return null;

            return new Cell(row, column, (TetrominoColour)_grid[row, column], this);
        }

        private string CaptureState()
        {
            Tetromino t = Overlay.TetrominoInPlay;
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < _rows; i++)
            {
                for (int j = 0; j < _columns; j++)
                {
                    int row = Overlay.TetrominoPosition.Row;
                    int column = Overlay.TetrominoPosition.Column;

                    if ((i >= row && j >= column) && (i < row + t.BoundingSquareSize && j < column + t.BoundingSquareSize))
                    {
                        int[,] grid = t.Grid;
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

        public Matrix()
        {
            _grid = new int[_rows, _columns];
            Overlay = new Overlay(this);
        }
    }
}
