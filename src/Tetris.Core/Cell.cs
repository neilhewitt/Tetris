using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tetris.Core
{
    public class Cell
    {
        private Matrix _matrix;

        public TetrominoColour Colour { get; }
        public int Y { get; }
        public int X { get; }
        public bool Occupied => Colour != TetrominoColour.Empty;

        public Cell UpOne => _matrix.CellAt(Y - 1, X);
        public Cell DownOne => _matrix.CellAt(Y + 1, X);
        public Cell LeftOne => _matrix.CellAt(Y, X - 1);
        public Cell RightOne => _matrix.CellAt(Y, X + 1);

        internal Cell(int y, int x, TetrominoColour colour, Matrix matrix)
        {
            Y = y;
            X = x;
            Colour = colour;
            _matrix = matrix;
        }
    }
}
