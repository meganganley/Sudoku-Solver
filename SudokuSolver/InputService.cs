using System;
using System.Collections.Generic;
using System.Linq;

namespace SudokuSolver
{
    class InputService
    {
        public Grid GetInput()
        {
            Console.WriteLine("Enter grid size:");
            // todo validate input
            // todo: validate square number
            int gridSize = int.Parse(Console.ReadLine());

            Cell[][] grid = new Cell[gridSize][];


            // todo fix
            int completedCells = 0;

            int[] tempRow;

            Console.WriteLine("Enter grid one row at a time (space-separated, 0 for blank):");
            for (int i = 0; i < gridSize; i++)
            {
                grid[i] = new Cell[gridSize];
                
                // empty line, null 
                tempRow = Console.ReadLine().Split(new char[] {' '}, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();
                
                for (int j = 0; j < gridSize; j++)
                {
                    grid[i][j] = new Cell(i, j, tempRow[j]);
                    
                    if (tempRow[j] != 0)
                    {
                        grid[i][j].Options = new List<int>();
                        completedCells++;
                    }
                    else
                    {
                        grid[i][j].Options = Enumerable.Range(1, gridSize).ToList();
                    }
                }

            }

            DetermineSquares(ref grid);

            return new Grid(grid, completedCells);
        }


        // MAGIC CODE : categorise & number each "sqrt(gridSize) x sqrt(gridSize)"-sized square (row-by-row, starting from left)
        private static void DetermineSquares(ref Cell[][] grid)
        {
            int square = 0;

            int countA = 0;
            int countB = 0;

            int gridSize = grid.Length;

            int squareSize = (int)Math.Sqrt(gridSize);

            for (int i = 0; i < gridSize; i++)
            {
                for (int j = 0; j < gridSize; j++)
                {
                    grid[i][j].Square = square;

                    countA++;

                    if (countA % squareSize == 0 && countA % gridSize != 0)
                    {
                        square++;
                    }

                }
                countB++;

                if (countB % squareSize != 0)
                {
                    square = square - squareSize + 1;
                }
                else
                {
                    square++;
                }
            }
        }
    }
}