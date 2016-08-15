using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tetris.Core
{
    public enum TetrominoColour
    {
        Empty, Red, Magenta, Yellow, Cyan, Blue, LightBlue, LightGrey, Lime
    }

    public enum Direction
    {
        Clockwise, Anticlockwise
    }

    public class Tetromino
    {
        public static Tetromino I => new Tetromino('I', "0000\n1111", TetrominoColour.Red);
        public static Tetromino J => new Tetromino('J', "111\n001", TetrominoColour.Magenta);
        public static Tetromino L => new Tetromino('L', "111\n100", TetrominoColour.Yellow);
        public static Tetromino O => new Tetromino('O', "11\n11", TetrominoColour.Cyan);
        public static Tetromino S => new Tetromino('S', "011\n110", TetrominoColour.Blue);
        public static Tetromino T => new Tetromino('T', "111\n010", TetrominoColour.LightGrey);
        public static Tetromino Z => new Tetromino('Z', "110\n011", TetrominoColour.Lime);

        private int _bounds;
        private Grid<int> _grid;
        private IList<Position> _activeCells;

        public int GridSize => _bounds;
        public ReadonlyGrid<int> Grid => new ReadonlyGrid<int>(_grid);
        public string Pattern { get; private set; }
        public TetrominoColour Colour { get; private set; }
        public char Name { get; private set; }

        public IList<Position> ActivePositions
        {
            get
            {
                lock (this)
                {
                    if (_activeCells == null)
                    {
                        List<Position> positions = new List<Position>();
                        for (int i = 0; i < GridSize; i++)
                        {
                            for (int j = 0; j < GridSize; j++)
                            {
                                if (_grid[i, j] == 1)
                                {
                                    positions.Add(new Position(i, j));
                                }
                            }
                        }

                        _activeCells = positions;
                    }

                    return _activeCells;
                }
            }
        }

        public void RotateClockwise()
        {
            Rotate(Direction.Clockwise);
        }

        public void RotateAnticlockwise()
        {
            Rotate(Direction.Anticlockwise);
        }

        private void Rotate(Direction direction)
        {
            Grid<int> transform = new Grid<int>(_bounds, _bounds);
            for (int y = 0; y < _bounds; y++)
            {
                for (int x = 0; x < _bounds; x++)
                {
                    int x2 = (direction == Direction.Clockwise) ? (_bounds-1) - y : y;
                    int y2 = (direction == Direction.Clockwise) ? x : (_bounds-1) - x;
                    transform[y2, x2] = _grid[y, x];
                }
            }

            _grid = transform;
            lock (this)
            {
                UpdatePattern();
                _activeCells = null;
            }
        }

        private void UpdatePattern()
        {
            List<string> rows = new List<string>();
            for (int i = 0; i < _bounds; i++)
            {
                string row = "";
                for (int j = 0; j < _bounds; j++)
                {
                    row += _grid[i, j] == 1 ? "1" : "0";
                }
                rows.Add(row);
            }
            Pattern = String.Join("\n", rows);
        }

        public Tetromino(char name, string pattern, TetrominoColour colour)
        {
            Pattern = pattern;
            Colour = colour;
            Name = name;

            string[] parts = pattern.Split('\n');
            _bounds = parts.Max(p => p.Length);
            _grid = new Grid<int>(_bounds, _bounds);
            for (int y = 0; y < _bounds; y++)
            {
                for (int x = 0; x < _bounds; x++)
                {
                    _grid[y, x] = y > parts.Length-1 ? 0 : parts[y][x] == '1' ? 1 : 0;
                }
            }
        }
    }
}
