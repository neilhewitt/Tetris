using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tetris.Core
{
    public class ReadonlyGrid<T>
    {
        protected int _rows;
        protected int _columns;
        protected T[,] _grid;

        public T this[int row, int column]
        {
            get
            {
                return GetCell(row, column).Item;
            }
        }

        public GridCell<T> GetCell(int row, int column)
        {
            if (row >= _rows || column >= _columns || row < 0 || column < 0) return null;
            return new GridCell<T>(_grid[row, column], row, column);
        }

        public void Copy(int startRow, int startColumn, int endRow, int endColumn, Grid<T> destination, int destinationRow, int destinationColumn, 
            Func<T, bool> selector = null, Func<T,T> mutator = null)
        {
            if (mutator == null) mutator = (item) => item; // returns grid content un-mutated
            if (selector == null) selector = (item) => true; // always selects

            for (int row = startRow; row <= endRow; row++)
            {
                for (int column = startColumn; column <= endColumn; column++)
                {
                    if (selector(_grid[row, column]))
                    {
                        destination.Set(destinationRow + (row - startRow), destinationColumn + (column - startColumn), mutator(_grid[row,column]), true);
                    }
                }
            }
        }

        public ReadonlyGrid(int rows, int columns)
        {
            _grid = new T[rows, columns];
        }

        public ReadonlyGrid(ReadonlyGrid<T> sourceGrid)
        {
            _grid = sourceGrid._grid;
        }
    }

    public class Grid<T> : ReadonlyGrid<T>
    {
        new public T this[int row, int column]
        {
            get
            {
                return GetCell(row, column).Item;
            }
            set
            {
                Set(row, column, value);
            }
        }

        public void Set(int row, int column, T item, bool overwrite = false)
        {
            if (row >= _rows || column >= _columns || row < 0 || column < 0)
            {
                throw new ArgumentOutOfRangeException("Row or column index was out of range.");
            }

            if (item == null)
            {
                throw new ArgumentNullException("Item may not be null.");
            }

            if (_grid[row, column] != null && overwrite == false)
            {
                throw new InvalidOperationException("An item already exists at this grid position. If T is a value type this will always be the case. Call Set() with 'overwrite' set to true to overwrite cell contents.");
            }

            _grid[row, column] = item;
        }

        public void Reset(int row, int column)
        {
            if (row >= _rows || column >= _columns || row < 0 || column < 0)
            {
                throw new ArgumentOutOfRangeException("Row or column index was out of range.");
            }

            _grid[row, column] = default(T);
        }

        public void Clear()
        {
            _grid = new T[_rows, _columns];
        }

        public Grid(int rows, int columns) : base(rows, columns)
        {
        }
    }

    public class GridCell<T>
    {
        public T Item { get; }
        public int Row { get; }
        public int Column { get; }

        public GridCell(T item, int row, int column)
        {
            Item = item;
            Row = row;
            Column = column;
        }
    }
}
