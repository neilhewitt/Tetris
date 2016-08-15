using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tetris.Core
{
    public class Game
    {
        public Matrix Matrix { get; }

        public void BeginPlay()
        {
        }

        public void EndPlay()
        {
        }

        public Game(int rows = 22, int columns = 10)
        {
            Matrix = new Matrix(rows, columns);
        }
    }
}
