using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Tetris.Core
{
    public enum TetrominoColour
    {
        Empty, Red, Magenta, Yellow, Cyan, Blue, DarkGray, DarkBlue
    }

    public class Tetromino
    {
        public static Tetromino I(GameBoard board) => new Tetromino('I', "0000\n1111", TetrominoColour.Red, board);
        public static Tetromino J(GameBoard board) => new Tetromino('J', "111\n001", TetrominoColour.Magenta, board);
        public static Tetromino L(GameBoard board) => new Tetromino('L', "111\n100", TetrominoColour.Yellow, board);
        public static Tetromino O(GameBoard board) => new Tetromino('O', "11\n11", TetrominoColour.Cyan, board);
        public static Tetromino S(GameBoard board) => new Tetromino('S', "011\n110", TetrominoColour.Blue, board);
        public static Tetromino T(GameBoard board) => new Tetromino('T', "111\n010", TetrominoColour.DarkGray, board);
        public static Tetromino Z(GameBoard board) => new Tetromino('Z', "110\n011", TetrominoColour.DarkBlue, board);
        public static Tetromino New(char name, GameBoard board)
        {
            MethodInfo method = typeof(Tetromino).GetRuntimeMethod(new string(name, 1), new Type[] { typeof(GameBoard) });
            if (method != null)
            {
                Tetromino t = (Tetromino)method.Invoke(null, new object[] { board });
                return t;
            }

            return null;
        }

        private Grid<int> _grid;
        private IList<GridCell<int>> _activeCells;
        private GameBoard _board;

        public int GridSize => _grid.Rows;
        public Grid<int> Grid => _grid.AsReadonly();
        public string Pattern { get; private set; }
        public TetrominoColour Colour { get; private set; }
        public char Name { get; private set; }
        public GameBoard Board => _board;
        public int RowOnBoard { get; private set; }
        public int ColumnOnBoard { get; private set; }

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

        public void MoveDown()
        {
            if (RowOnBoard + PopulatedCells.Last().Row < _board.Rows)
            {
                RowOnBoard++;
            }
        }

        public void MoveLeft()
        {
            if (ColumnOnBoard + PopulatedCells.Min(x => x.Column) > 0)
            {
                ColumnOnBoard--;
            }
        }

        public void MoveRight()
        {
            if (ColumnOnBoard + PopulatedCells.Max(x => x.Column) < _board.Columns)
            {
                ColumnOnBoard++;
            }
        }

        public bool RotateClockwise()
        {
            return Rotate(RotationDirection.Clockwise);
        }

        public bool RotateAnticlockwise()
        {
            return Rotate(RotationDirection.Anticlockwise);
        }

        private bool Rotate(RotationDirection direction)
        {
            Grid<int> rotated = _grid.Rotate(direction);
            // check collision
            if (!Collides())
            {
                _grid = rotated;
                _activeCells = null;
                return true;
            }

            return false;
        }

        private bool Collides()
        {
            Grid<int> gameGrid = _board.Grid.Subgrid(_board.Tetromino.RowOnBoard, _board.Tetromino.ColumnOnBoard, GridSize, GridSize);
            foreach(GridCell<int> cell in Grid)
            {
                if (cell.Contents > 0 && gameGrid.GetCell(cell.Row, cell.Column).Contents > 0)
                {
                    // CLASH!
                    return true;
                }
            }
            return false;
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

        public Tetromino(char name, string pattern, TetrominoColour colour, GameBoard board)
        {
            Pattern = pattern;
            Colour = colour;
            Name = name;
            _board = board;
            RowOnBoard = 0;
            ColumnOnBoard = 0;

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
