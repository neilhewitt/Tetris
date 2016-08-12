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

        public Game()
        {
            Matrix = new Matrix();
        }
    }
}
