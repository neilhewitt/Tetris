using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tetris.Core
{
    public class Grid<T>
    {
        protected int _rows;
        protected int _columns;
        protected T[,] _grid;

        public int Rows => _rows;
        public int Columns => _columns;
        
        public bool ReadOnly { get; protected set; }

        public T this[int row, int column]
        {
            get
            {
                return GetCell(row, column).Contents;
            }
            set
            {
                CheckReadOnly();
                Set(row, column, value);
            }
        }

        public GridCell<T> GetCell(int row, int column)
        {
            if (row >= _rows || column >= _columns || row < 0 || column < 0) return null;
            return new GridCell<T>(_grid[row, column], row, column);
        }

        public GridRow<T> GetRow(int row)
        {
            if (row > _rows) throw new ArgumentOutOfRangeException();

            List<GridCell<T>> cells = new List<GridCell<T>>();
            for (int i = 0; i < _columns; i++)
            {
                cells.Add(GetCell(row, i));
            }

            return new GridRow<T>(cells);
        }

        public IEnumerable<IEnumerable<GridCell<T>>> GetRows()
        {
            for (int i = 0; i < _rows; i++) yield return GetRow(i);
        }

        public IEnumerable<GridCell<T>> GetColumn(int column)
        {
            if (column > _columns) throw new ArgumentOutOfRangeException();

            List<GridCell<T>> cells = new List<GridCell<T>>();
            for (int i = 0; i < _rows; i++)
            {
                cells.Add(GetCell(i, column));
            }

            return new GridColumn<T>(cells);
        }

        public IEnumerable<IEnumerable<GridCell<T>>> GetColumns()
        {
            for (int i = 0; i < _columns; i++) yield return GetColumn(i);
        }

        public void Set(int row, int column, T item)
        {
            CheckReadOnly();

            if (row >= _rows || column >= _columns || row < 0 || column < 0)
            {
                throw new ArgumentOutOfRangeException("Row or column index was out of range.");
            }

            if (item == null)
            {
                throw new ArgumentNullException("Item may not be null.");
            }

            _grid[row, column] = item;
        }

        public void Reset(int row, int column)
        {
            CheckReadOnly();

            if (row >= _rows || column >= _columns || row < 0 || column < 0)
            {
                throw new ArgumentOutOfRangeException("Row or column index was out of range.");
            }

            _grid[row, column] = default(T);
        }

        public void Clear()
        {
            CheckReadOnly();

            _grid = new T[_rows, _columns];
        }

        public Grid<T> Clone()
        {
            Grid<T> newGrid = new Grid<T>(_rows, _columns);
            newGrid.Insert(this, 0, 0, _rows - 1, _columns - 1, 0, 0);
            return newGrid;
        }

        public void Insert(Grid<T> source, int startRow, int startColumn, int endRow, int endColumn, int destinationRow, int destinationColumn, 
            Func<T, bool> selector = null, Func<T,T> mutator = null)
        {
            if (mutator == null) mutator = (item) => item; // returns grid content un-mutated
            if (selector == null) selector = (item) => true; // always selects

            for (int row = startRow; row <= endRow; row++)
            {
                for (int column = startColumn; column <= endColumn; column++)
                {
                    if (selector(source._grid[row, column]))
                    {
                        int thisRow = destinationRow + (row - startRow);
                        int thisColumn = destinationColumn + (column - startColumn);
                        if (thisRow < _rows && thisColumn < _columns)
                        {
                            Set(thisRow, thisColumn, mutator(source._grid[row, column]));
                        }
                    }
                }
            }
        }

        public Grid<T> AsReadonly()
        {
            Grid<T> readonlyGrid = new Grid<T>(this);
            readonlyGrid.ReadOnly = true;
            return readonlyGrid;
        }

        private void CheckReadOnly()
        {
            if (ReadOnly)
            {
                throw new InvalidOperationException("This grid is ReadOnly. You cannot change its contents.");
            }
        }

        public Grid(int rows, int columns)
        {
            _rows = rows;
            _columns = columns;
            _grid = new T[rows, columns];
        }

        private Grid(Grid<T> contents)
        {
            _rows = contents._rows;
            _columns = contents._columns;
            _grid = contents._grid;
        }
    }

    public class GridCell<T>
    {
        public T Contents { get; }
        public int Row { get; }
        public int Column { get; }

        public GridCell(T item, int row, int column)
        {
            Contents = item;
            Row = row;
            Column = column;
        }
    }

    public class GridCellCollection<T> : IEnumerable<GridCell<T>>
    {
        private IEnumerable<GridCell<T>> _collection;

        public IEnumerator<GridCell<T>> GetEnumerator()
        {
            return _collection.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_collection).GetEnumerator();
        }

        public GridCellCollection(IEnumerable<GridCell<T>> collection)
        {
            _collection = collection;
        }
    }

    public class GridRow<T> : GridCellCollection<T>
    {
        public GridRow(IEnumerable<GridCell<T>> collection) : base(collection)
        {
        }
    }

    public class GridColumn<T> : GridCellCollection<T>
    {
        public GridColumn(IEnumerable<GridCell<T>> collection) : base(collection)
        {
        }
    }
}
