using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tetris.Core
{
    public enum TetrominoColour
    {
        Red, Magenta, Yellow, Cyan, Blue, LightBlue, LightGrey, Lime
    }

    public class Tetromino
    {
        public static readonly Tetromino I = new Tetromino("1111", TetrominoColour.Red);
        public static readonly Tetromino J = new Tetromino("111\n001", TetrominoColour.Magenta);
        public static readonly Tetromino L = new Tetromino("111\n100", TetrominoColour.Yellow);
        public static readonly Tetromino O = new Tetromino("11\n11", TetrominoColour.Cyan);
        public static readonly Tetromino S = new Tetromino("011\n110", TetrominoColour.Blue);
        public static readonly Tetromino T = new Tetromino("111\n010", TetrominoColour.LightGrey);
        public static readonly Tetromino Z = new Tetromino("110\n011", TetrominoColour.Lime);

        public string Pattern { get; private set; }
        public TetrominoColour Colour { get; private set; }

        internal Tetromino(string shape, TetrominoColour colour)
        {
            Pattern = shape;
            Colour = colour;
        }
    }
}
