using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;

namespace SudokuSolver
{
    public static class SudokuExtensions
    {
        public static List<int> UpdateCellOptions(this Grid grid, int x, int y, int s)
        {
            int[] row = grid.GetRow(x);
            int[] column = grid.GetColumn(y);
            int[] square = grid.GetSquare(s);

            List<int> possibleRowValues = Enumerable.Range(1, grid.GridSize).Except(row).ToList();
            List<int> possibleColValues = Enumerable.Range(1, grid.GridSize).Except(column).ToList();
            List<int> possibleSquareValues = Enumerable.Range(1, grid.GridSize).Except(square).ToList();

            return possibleRowValues.Intersect(possibleColValues).Intersect(possibleSquareValues).ToList();
        }

        public static void SearchForLockedCandidates()
        {
            
        }


        // 3. Searching for Single Candidates: Square
        // "is there a value in any square that is only possible in one location?"

        public static void SearchSquareForSingleCandidates(this Grid grid)
        {
            for (int s = 0; s < grid.GridSize; s++)
            {
                for (int value = 1; value <= grid.GridSize; value++)
                {
                    // that value already exists in the col
                    if (Array.Exists(grid.GetSquare(s), element => element == value))
                    {
                        continue;
                    }

                    Tuple<int, int> result = grid.CheckSquareForValueInOptions(s, value);
                    if (result.Item1 != -1 && result.Item2 != -1)
                    {
                        grid.SolveCell(result.Item1, result.Item2, value);
                        grid.UpdateNeighbours(result.Item1, result.Item2);
                    }
                }
            }
        }

        public static void CheckCellForSingleOption(this Grid grid, int x, int y)
        {
            if (grid.GetCell(x, y).Solved)
            {
                return;
            }

            if (grid.GetCell(x, y).Options.Count == 1)
            {
                grid.SolveCell(x, y, grid.GetCell(x, y).Options[0]);
                grid.UpdateNeighbours(x, y);
            }
        }

        public static void SearchColumnForSingleCandidates(this Grid grid)
        {
            for (int col = 0; col < grid.GridSize; col++)
            {
                for (int value = 1; value <= grid.GridSize; value++)
                {
                    // that value already exists in the col
                    if (Array.Exists(grid.GetColumn(col), element => element == value))
                    {
                        continue;
                    }

                    int result = grid.CheckColForValueInOptions(col, value);
                    if (result != -1)
                    {
                        grid.SolveCell(result, col, value);
                        grid.UpdateNeighbours(result, col);
                    }
                }
            }
        }
        // TODO: rename - doesnt make sense. Should be single responsibility
        public static void SearchRowForSingleCandidates(this Grid grid)
        {
            // 3. Searching for Single Candidates: ROW
            // "is there a value in any row that is only possible in one location?" 

            for (int row = 0; row < grid.GridSize; row++)
            {
                for (int value = 1; value <= grid.GridSize; value++)
                {
                    // that value already exists in the row
                    if (Array.Exists(grid.GetRow(row), element => element == value))
                    {
                        continue;
                    }

                    int result = grid.CheckRowForValueInOptions(row, value);
                    if (result != -1)
                    {
                        grid.SolveCell(row, result, value);
                        grid.UpdateNeighbours(row, result);
                    }
                }
            }
        }

        public static void UpdateNeighbours(this Grid grid, int x, int y)
        {
            grid.UpdateRowNeighbours(x);
            grid.UpdateColumnNeighbours(y);
            grid.UpdateSquareNeighbours(grid.GetCell(x, y).Square);
        }

        public static void UpdateRowNeighbours(this Grid grid, int x)
        {
            Console.WriteLine("update row  {0}", x);
            for (int i = 0; i < grid.GridSize; i++)
            {
                if (grid.GetCell(x, i).Solved)
                {
                    continue;
                }

                grid.GetCell(x, i).Options = grid.UpdateCellOptions(x, i, grid.GetCell(x, i).Square);

                grid.CheckCellForSingleOption(x, i);
            }
        }

        public static void UpdateColumnNeighbours(this Grid grid, int y)
        {
            Console.WriteLine("update column {0}", y);
            for (int i = 0; i < grid.GridSize; i++)
            {
                if (grid.GetCell(i, y).Solved)
                {
                    continue;
                }
                grid.GetCell(i, y).Options = grid.UpdateCellOptions(i, y, grid.GetCell(i, y).Square);
                grid.CheckCellForSingleOption(i, y);
            }
        }

        public static void UpdateSquareNeighbours(this Grid grid, int s)
        {
            Console.WriteLine("update square {0}", s);
            Cell[] square = grid.GetSquareCells(s);
            for (int i = 0; i < grid.GridSize; i++)
            {
                if (grid.GetCell(square[i].X, square[i].Y).Solved)
                {
                    continue;
                }
                grid.GetCell(square[i].X, square[i].Y).Options = grid.UpdateCellOptions(square[i].X, square[i].Y, s);
                grid.CheckCellForSingleOption(square[i].X, square[i].Y);
            }
        }

        public static void SolveCell(this Grid grid, int x, int y, int value)
        {
            Console.WriteLine("solved cell [{0}, {1}]", x, y);
            grid.GetCell(x, y).Solved = true;
            grid.GetCell(x, y).Value = value;
            grid.GetCell(x, y).Options.Clear();
            grid.CompletedCells++;
        }


        public static bool IsValidSolution(this Grid grid)
        {
            if (grid.CompletedCells < grid.GridSize * grid.GridSize)
            {
                return false;
            }

            for (int i = 0; i < grid.GridSize; i++)
            {
                bool validRow = new HashSet<int>(grid.GetRow(i)).SetEquals(Enumerable.Range(1, grid.GridSize));
                bool validCol = new HashSet<int>(grid.GetColumn(i)).SetEquals(Enumerable.Range(1, grid.GridSize));
                bool validSquare = new HashSet<int>(grid.GetSquare(i)).SetEquals(Enumerable.Range(1, grid.GridSize));

                if (!(validRow & validCol & validSquare))
                {
                    return false;
                }
            }

            return true;
        }

    }
}