using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using System.Text;

namespace Tetris.Core
{
    public class Matrix
    {
        private int _rows;
        private int _columns;
        private Grid<int> _grid;
        private Grid<int> _gridWithCarrier;
        private Carrier _carrier;

        public Grid<int> GridWithoutCarrier => _grid.AsReadonly();
        public Grid<int> GridWithCarrier
        {
            get
            {
                lock (this)
                {
                    if (_gridWithCarrier == null && _carrier.Tetromino != null)
                    {
                        int size = _carrier.Tetromino.GridSize - 1;
                        Grid<int> grid = _grid.Clone();
                        grid.Insert(_carrier.Tetromino.Grid, 0, 0, size, size, _carrier.Position.Row, _carrier.Position.Column, 
                            (input) => input == 1, (output) => (int)_carrier.Tetromino.Colour);
                        _gridWithCarrier = grid;
                    }     
                }

                return _gridWithCarrier;
            }
        }

        public void InjectTetromino(Tetromino t, int column = 0)
        {
            _carrier.InjectTetromino(t, column);
        }

        public bool Move()
        {
            // clear any filled rows, move grid down
            // then move carrier down one row (if it can move)
            // or freeze the tetromino in place
            lock (this)
            {
                _gridWithCarrier = null;
            }
            return _carrier.Move();
        }

        public void FreezeTetromino()
        {
            Tetromino t = _carrier.Tetromino;
            int row = _carrier.Position.Row;
            int column = _carrier.Position.Column;
            int size = t.GridSize - 1;

            // copy the tetromino shape data to the grid - it's now 'frozen'
            // we only copy squares that have shape data in them, and we mutate to the appropriate colour value for the tetromino
            lock (this)
            {
                _grid.Insert(t.Grid, 0, 0, size, size, row, column, (input) => input == 1, (output) => (int)t.Colour);
                _carrier.RemoveTetromino();
                lock (this)
                {
                    _gridWithCarrier = null;
                }

            }
        }

        internal int Rows => _rows;
        internal int Columns => _columns;

        public Matrix(int rows = 22, int columns = 10)
        {
            _rows = rows;
            _columns = columns;
            _grid = new Grid<int>(_rows, _columns);
            _carrier = new Carrier(this);
        }
    }
}
