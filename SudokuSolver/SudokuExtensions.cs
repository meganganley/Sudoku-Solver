using System;
using System.Collections.Generic;
using System.Linq;

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

        // TODO: Fuck me
        // 3. Searching for Single Candidates: COLUMN
        // "is there a value in any column that is only possible in one location?"

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

                    // TODO: don't do when value already used
                    int result = grid.CheckColForValueInOptions(col, value);
                    if (result != -1)
                    {
                        grid.GetCell(result, col).Value = value;
                        grid.GetCell(result, col).Options = new List<int>();
                        grid.CompletedCells++;
                        Console.WriteLine("Solved a number: [{0},{1}], value = {2}", result, col,
                            grid.GetCell(result, col).Value);
                    }
                }
            }
        }


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

                    // TODO: don't do when value already used
                    int result = grid.CheckRowForValueInOptions(row, value);
                    if (result != -1)
                    {
                        grid.GetCell(row, result).Value = value;
                        grid.GetCell(row, result).Options = new List<int>();
                        grid.CompletedCells++;
                        Console.WriteLine("Solved a number: [{0},{1}], value = {2}", row, result,
                            grid.GetCell(row, result).Value);
                    }
                }
            }
        }


        //        public static void MissingValue(this Grid grid)
        //        {
        //            // todo: wtf???
        //            // METHOD: 5. Searching for missing numbers in rows and columns:
        //            // "what "
        //            for (int i = 0; i < grid.GridSize; i++)
        //            {
        //                for (int j = 0; j < grid.GridSize; j++)
        //                {
        //                    Cell temp = grid.GetCell(i, j);
        //                    if (temp.Options.Count > 1)
        //                    {
        //                        Console.WriteLine("Checking [{0},{1}]", i, j);
        //                        //////// TODO: Should this be here?????
        //                        temp.Options = grid.UpdateCellOptions(i, j, temp.Square);
        //                        if (temp.Options.Count == 1)
        //                        {
        //                            temp.Value = temp.Options[0];
        //                            grid.CompletedCells++;
        //                            Console.WriteLine("Solved a number: [{0},{1}], value = {2}", i, j, temp.Value);
        //                        }
        //                        else
        //                        {
        //
        //                        }
        //                    }
        //                }
        //            }
        //  }
        

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