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
        private static Tetromino[] _tetrominoes = new Tetromino[] { Tetromino.I, Tetromino.J, Tetromino.L, Tetromino.O, Tetromino.S, Tetromino.T, Tetromino.Z };

        public static void Main(string[] args)
        {
            int index = 0;
            Game game = new Game();
            game.Matrix.Inject(_tetrominoes[index]);
            ShowFrame();
            
            while (true)
            {
                DisplayGameState(game);
                if (!game.Matrix.MoveOne())
                {
                    index = (index < 6 ? index + 1 : 0);
                    game.Matrix.Inject(_tetrominoes[index]);
                }
                Thread.Sleep(500);
            }
        }

        private static void ShowFrame()
        {
            Console.Clear();
            Console.SetCursorPosition(0, 0);
            Console.ForegroundColor = ConsoleColor.White;

            for (int i = 0; i < 22; i++)
            {
                Console.WriteLine("|          |");
            }
            Console.WriteLine("|__________|");
        }

        private static void DisplayGameState(Game game)
        {
            Matrix matrix = game.Matrix;
            string gridState = matrix.GridState;

            int row = 0;
            for (int i = row; i < 22; i++)
            {
                Console.SetCursorPosition(1, i);
                Console.Write("          ");
            }

            string[] gridRows = gridState.Split('|');
            foreach (string gridRow in gridRows.Where(r => r != ""))
            {
                Console.SetCursorPosition(1, row++);
                string[] cells = gridRow.Split(',');
                foreach(string cell in cells)
                {
                    int value = int.Parse(cell);
                    if (value == 0)
                    {
                        Console.Write(" ");
                    }
                    else
                    {
                        string colourName = Enum.GetName(typeof(TetrominoColour), (TetrominoColour)value).Replace("Light","");
                        Console.ForegroundColor = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), colourName);
                        Console.Write("#");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                }
            }
        }
    }
}
