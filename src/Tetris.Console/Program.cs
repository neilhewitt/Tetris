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
        private static object _padlock = new object();
        private static char[] _tetrominoes;
        private static int _rows;
        private static int _columns;
        private static Random _random = new Random();

        public static void Main(string[] args)
        {
            _tetrominoes = new char[] { 'I', 'J', 'L', 'O', 'S', 'T', 'Z' };

            int index = 0;
            int column = 0;
            _rows = 22;
            _columns = 10;
            Game game = new Game(columns: _columns);
            game.Board.BeginPlay(_tetrominoes[index]);
            ShowFrame();
            //Timer timer = new Timer(TimerTick, game, 1000, 1000);

            while (true)
            {
                DisplayGameState(game);
                ConsoleKeyInfo info = Console.ReadKey();
                if (info.Key == ConsoleKey.DownArrow)
                {
                    if (!game.Board.Move())
                    {
                        //column += _tetrominoes[index].BoundingSquareSize;
                        index = _random.Next(0, 6); 
                        game.Board.BeginPlay(_tetrominoes[index]);
                    }
                }
                else if (info.Key == ConsoleKey.OemPeriod)
                {
                    game.Board.RotateClockwise();
                }
                else if (info.Key == ConsoleKey.OemComma)
                {
                    game.Board.RotateAnticlockwise();
                }
                else if (info.Key == ConsoleKey.RightArrow)
                {
                    game.Board.MoveRight();
                }
                else if (info.Key == ConsoleKey.LeftArrow)
                {
                    game.Board.MoveLeft();
                }
            }
        }

        private static void TimerTick(object state)
        {
            Game game = (Game)state;
            if (!game.Board.Move())
            {
                //column += _tetrominoes[index].BoundingSquareSize;
                int index = _random.Next(0, 6);
                game.Board.BeginPlay(_tetrominoes[index]);
            }

            DisplayGameState(game);
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
            lock (_padlock)
            {
                GameBoard Board = game.Board;
                Grid<int> grid = Board.GridWithTetromino;

                foreach (GridRow<int> row in grid.GetRows())
                {
                    Console.SetCursorPosition(1, row.First().Row);
                    foreach (GridCell<int> cell in row)
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
}
