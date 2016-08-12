using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tetris.Core
{
    public class Matrix
    {
        private const int _rows = 22;
        private const int _columns = 10;
        private Random _random = new Random();
        private IList<Tuple<int, int, Tetromino>> _tetrominoes = new List<Tuple<int, int, Tetromino>>();

        public void Add(Tetromino t)
        {
            // add to row 22, random column
            int column = _random.Next(0, 9) - t.BoundingSquareSize;
            column = column < 0 ? 0 : column;
            _tetrominoes.Add(new Tuple<int, int, Tetromino>(22, column, t));
        }

        public void Descend()
        {
            for (int i = 0; i < _tetrominoes.Count(); i++)
            {
                var t = _tetrominoes[i];
                t = new Tuple<int, int, Tetromino>(t.Item1 + 1, t.Item2, t.Item3);
                _tetrominoes[i] = t;
            }
        }

        public Matrix()
        {
        }
    }
}
