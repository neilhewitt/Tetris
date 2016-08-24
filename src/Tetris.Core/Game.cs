using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tetris.Core
{
    public class Game
    {
        public GameBoard Board { get; }

        public void BeginPlay()
        {
        }

        public void EndPlay()
        {
        }

        public Game(int rows = 22, int columns = 10)
        {
            Board = new GameBoard(rows, columns);
        }
    }
}
