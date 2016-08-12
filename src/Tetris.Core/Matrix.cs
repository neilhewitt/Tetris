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
        private Tuple<Tetromino, int, int> _tetrominoInPlay;

        public string GridState => CaptureState();
        public string TetrominoState => (_tetrominoInPlay.Item1.Name + ":" + _tetrominoInPlay.Item2.ToString() + "," 
            + _tetrominoInPlay.Item3.ToString() + ":" + _tetrominoInPlay.Item1.Pattern).Replace("\n", "\\n");

        public void Inject(Tetromino t)
        {
            // add to row 0, column 0
            _tetrominoInPlay = new Tuple<Tetromino, int, int>(t, 0, 3);
        }

        public bool MoveOne()
        {
            Tetromino t = _tetrominoInPlay.Item1;
            int y = _tetrominoInPlay.Item2;
            int x = _tetrominoInPlay.Item3;

            if (y < (23 - t.BoundingSquareSize))
            {
                t.RotateClockwise();
                _tetrominoInPlay = new Tuple<Tetromino, int, int>(t, y + 1, x);
                return true;
            }

            return false;
        }

        private string CaptureState()
        {
            Tetromino t = _tetrominoInPlay.Item1;
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < _rows; i++)
            {
                for (int j = 0; j < _columns; j++)
                {
                    int y = _tetrominoInPlay.Item2;
                    int x = _tetrominoInPlay.Item3;

                    if ((i >= y && j >= x) && (i < y + t.BoundingSquareSize && j < x + t.BoundingSquareSize))
                    {
                        int[,] grid = t.Grid;
                        int cell = grid[i - y, j - x];
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
        }
    }
}
