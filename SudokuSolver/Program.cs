using System;
using System.Collections.Generic;
using System.Linq;

namespace SudokuSolver
{
    class Program
    {
        public static Grid grid;
        static void Main()
        {
            grid = new InputService().GetInput();
            // TODO: optimise order of iteration --> maintain record of 'areas' that are well-populated somehow??


            while (grid.CompletedCells < (grid.GridSize * grid.GridSize))
            {
                // TODO::: UTMOST IMPORTANCE
                // important --> should be called in other locations also? After every update, on every cell --> or only on every dependent cell 
                for (int i = 0; i < grid.GridSize; i++)
                {
                    for (int j = 0; j < grid.GridSize; j++)
                    {
                        Cell c = grid.GetCell(i, j);
                        if (c.Value != 0)
                        {
                            continue;
                        }

                        c.Options = grid.UpdateCellOptions(i, j, c.Square);
                        if (c.Options.Count <= 1)
                        {
                            grid.GetCell(i, j).Value = c.Options[0];
                            grid.GetCell(i, j).Options = new List<int>();
                            grid.CompletedCells++;
                            Console.WriteLine("Solved a number: [{0},{1}], value = {2}", i, j, c.Value);
                        }
                    }
                }
                
                //NEXT STEPS: implement http://www.conceptispuzzles.com/index.aspx?uri=puzzle/sudoku/techniques

                grid.SearchRowForSingleCandidates();
                grid.SearchColumnForSingleCandidates();
                
            }

            Console.WriteLine(grid.IsValidSolution());
            
            // The completed grid!! 
            for (int i = 0; i < grid.GridSize; i++)
            {
                for (int j = 0; j < grid.GridSize; j++)
                {
                    Console.Write(grid.GetCell(i,j).Value + " ");
                }
                Console.WriteLine();
            }

            Console.WriteLine("DONE");

            Console.ReadKey();
        }
    }
}
