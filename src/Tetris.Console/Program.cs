using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Tetris.Core;

namespace Tetris.Command
{
    public class Program
    {
        private static Tetromino[] _tetrominoes;
        private static int _rows;
        private static int _columns;

        public static void Main(string[] args)
        {
            _tetrominoes = new Tetromino[] { Tetromino.I, Tetromino.J, Tetromino.L, Tetromino.O, Tetromino.S, Tetromino.T, Tetromino.Z };

            int index = 0;
            int column = 0;
            _rows = 22;
            _columns = 50;
            Game game = new Game(columns: _columns);
            game.Matrix.InjectTetromino(_tetrominoes[index]);
            ShowFrame();
            
            while (true)
            {
                DisplayGameState(game);
                if (!game.Matrix.Move())
                {
                    game.Matrix.FreezeTetromino();
                    //column += _tetrominoes[index].BoundingSquareSize;
                    index = (index < 6 ? index + 1 : 0);
                    game.Matrix.InjectTetromino(_tetrominoes[index], column);
                }
                Thread.Sleep(100);
            }
        }

        private static void ShowFrame()
        {
            Console.Clear();
            Console.SetCursorPosition(0, 0);
            Console.ForegroundColor = ConsoleColor.White;

            for (int i = 0; i < _rows ; i++)
            {
                Console.WriteLine("|" + new string(' ', _columns) + "|");
            }
            Console.WriteLine(new string('^', _columns + 2));
        }

        private static void DisplayGameState(Game game)
        {
            Matrix matrix = game.Matrix;
            Grid<int> grid = matrix.GridWithCarrier;

            foreach (GridRow<int> row in grid.GetRows())
            {
                Console.SetCursorPosition(1, row.First().Row);
                foreach(GridCell<int> cell in row)
                {
                    if (cell.Contents == 0)
                    {
                        Console.Write(" ");
                    }
                    else
                    {
                        string colourName = Enum.GetName(typeof(TetrominoColour), (TetrominoColour)cell.Contents);
                        Console.ForegroundColor = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), colourName);
                        Console.Write("0");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                }
            }
        }
    }
}
