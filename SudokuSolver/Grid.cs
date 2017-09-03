namespace SudokuSolver
{
    public class Grid
    {
        private readonly Cell[][] _grid;
        public int GridSize;
        public int CompletedCells;

        public Grid(Cell[][] grid, int completedCells)
        {
            _grid = grid;
            GridSize = grid.Length;
            CompletedCells = completedCells;
        }

        public Cell GetCell(int x, int y)
        {
            return _grid[x][y];
        }


        public int[] GetRow(int rowIndex)
        {
            int[] row = new int[GridSize];
            for (int i = 0; i < GridSize; i++)
            {
                row[i] = _grid[rowIndex][i].Value;
            }
            return row;
        }

        public int[] GetColumn(int columnIndex)
        {
            int[] column = new int[GridSize];
            for (int i = 0; i < GridSize; i++)
            {
                column[i] = _grid[i][columnIndex].Value;
            }
            return column;
        }

        public int[] GetSquare(int squareIndex)
        {
            int[] square = new int[GridSize];
            int count = 0;

            for (int i = 0; i < GridSize; i++)
            {
                for (int j = 0; j < GridSize; j++)
                {
                    if (_grid[i][j].Square == squareIndex)
                    {
                        square[count] = _grid[i][j].Value;
                        count++;
                    }
                }
            }

            return square;
        }


        public int CheckRowForValueInOptions(int row, int value)
        {
            int count = 0;
            int index = -1;
            for (int j = 0; j < GridSize; j++)
            {
                if (_grid[row][j].Options.Contains(value))
                {
                    index = j;
                    count++;
                }
            }

            if (count == 1)
            {
                return index;
            }

            return -1;
        }

        public int CheckColForValueInOptions(int col, int value)
        {
            int count = 0;
            int index = -1;
            for (int i = 0; i < GridSize; i++)
            {
                if (_grid[i][col].Options.Contains(value))
                {
                    index = i;
                    count++;
                }
            }

            if (count == 1)
            {
                return index;
            }

            return -1;
        }

    }
}
