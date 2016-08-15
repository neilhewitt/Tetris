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
        public int Row { get; }
        public int Column { get; }
        public bool Occupied => Colour != TetrominoColour.Empty;

        public Cell UpOne => _matrix.CellAt(Row - 1, Column);
        public Cell DownOne => _matrix.CellAt(Row + 1, Column);
        public Cell LeftOne => _matrix.CellAt(Row, Column - 1);
        public Cell RightOne => _matrix.CellAt(Row, Column + 1);

        internal Cell(int y, int x, TetrominoColour colour, Matrix matrix)
        {
            Row = y;
            Column = x;
            Colour = colour;
            _matrix = matrix;
        }
    }
}
