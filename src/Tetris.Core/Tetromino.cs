using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tetris.Core
{
    public enum TetrominoColour
    {
        Empty, Red, Magenta, Yellow, Cyan, Blue, DarkGray, DarkBlue
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
        public static Tetromino T => new Tetromino('T', "111\n010", TetrominoColour.DarkGray);
        public static Tetromino Z => new Tetromino('Z', "110\n011", TetrominoColour.DarkBlue);

        private Grid<int> _grid;
        private IList<GridCell<int>> _activeCells;

        public int GridSize => _grid.Rows;
        public Grid<int> Grid => _grid.AsReadonly();
        public string Pattern { get; private set; }
        public TetrominoColour Colour { get; private set; }
        public char Name { get; private set; }

        public IList<GridCell<int>> PopulatedCells
        {
            get
            {
                lock (this)
                {
                    if (_activeCells == null)
                    {
                        List<GridCell<int>> cells = new List<GridCell<int>>();
                        for (int i = 0; i < GridSize; i++)
                        {
                            for (int j = 0; j < GridSize; j++)
                            {
                                if (_grid[i, j] == 1)
                                {
                                    cells.Add(_grid.GetCell(i, j));
                                }
                            }
                        }

                        _activeCells = cells;
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
            Grid<int> transform = new Grid<int>(GridSize, GridSize);
            for (int y = 0; y < GridSize; y++)
            {
                for (int x = 0; x < GridSize; x++)
                {
                    int x2 = (direction == Direction.Clockwise) ? (GridSize-1) - y : y;
                    int y2 = (direction == Direction.Clockwise) ? x : (GridSize-1) - x;
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
            for (int i = 0; i < GridSize; i++)
            {
                string row = "";
                for (int j = 0; j < GridSize; j++)
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
            int bounds = parts.Max(p => p.Length);
            _grid = new Grid<int>(bounds, bounds);
            for (int y = 0; y < bounds; y++)
            {
                for (int x = 0; x < bounds; x++)
                {
                    _grid[y, x] = y > parts.Length-1 ? 0 : parts[y][x] == '1' ? 1 : 0;
                }
            }
        }
    }
}
