using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tetris.Core
{
    public class Game
    {
        public Matrix Matrix { get; }

        public Game()
        {
            Matrix = new Matrix();
        }
    }
}
