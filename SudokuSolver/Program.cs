using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver
{
    class Program
    {
        public static Cell[][] grid;
        public static int gridSize;
        public static int completedCells;
        static void Main(string[] args)
        {
            GetInput();
            DetermineSquares();

            int[] test = GetRow(2);
            int[] test2 = GetColumn(1);
            int[] test3 = GetSquare(0);

            Console.WriteLine("Row 2: ");
            for (int i = 0; i < test.Length; i++)
            {
                Console.WriteLine(test[i]);
            }
            Console.WriteLine();

            Console.WriteLine("Col 1: ");
            for (int i = 0; i < test2.Length; i++)
            {
                Console.WriteLine(test2[i]);
            }
            Console.WriteLine();

            Console.WriteLine("Square 0: ");
            for (int i = 0; i < test3.Length; i++)
            {
                Console.WriteLine(test3[i]);
            }
            Console.WriteLine();

            //
            //            while ( completedCells < gridSize*gridSize)
            //            {
            //                
            //            }

            // todo optimise order of iteration


            while (completedCells < gridSize * gridSize)
            {
                for (int i = 0; i < gridSize; i++)
                {
                    for (int j = 0; j < gridSize; j++)
                    {
                        if (grid[i][j].Options.Count > 1)
                        {
                            grid[i][j].Options = UpdateCellOptions(i, j, grid[i][j].Square);
                            if (grid[i][j].Options.Count == 1)
                            {
                                grid[i][j].Value = grid[i][j].Options[0];
                                completedCells++;
                            }
                        }
                    }
                }
            }

            //
            //            int s = 0;
            //            for (int i = 0; i < gridSize; i++)
            //            {
            //                for (int j = 0; j < gridSize; j++)
            //                {
            //                    if (grid[i][j].Options.Count > 1)
            //                    {
            //                        s = grid[i][j].Square;
            //                        grid[i][j].Value = CalculateCellValue(i, j, s);
            //                    }
            //                }
            //            }


            for (int i = 0; i < gridSize; i++)
            {
                for (int j = 0; j < gridSize; j++)
                {
                    Console.Write(grid[i][j].Value + " ");
                }
                Console.WriteLine();
            }


            Console.ReadKey();
        }

        //        static int CalculateCellValue(int x, int y, int s)
        //        {
        //            grid[x][y].Options = UpdateCellOptions(x, y, s);
        //
        //            if (grid[x][y].Options.Count == 1)
        //            {
        //                completedCells++;
        //                return grid[x][y].Options[0];
        // 
        //            }
        //          return 0;
        //        }


        static List<int> UpdateCellOptions(int x, int y, int s)
        {

            int[] row = GetRow(x);
            int[] col = GetColumn(y);
            int[] square = GetSquare(s);

            List<int> options = grid[x][y].Options;
            List<int> newOptions = new List<int>();

            // todo very inefficient
            foreach (var item in options)
            {
                if (Array.Find(row, element => element == item) != 0)
                {
                    //Console.WriteLine("found something in row");
                    continue;
                }
                else if (Array.Find(col, element => element == item) != 0)
                {
                    // Console.WriteLine("found something in col");
                    continue;
                }
                else if (Array.Find(square, element => element == item) != 0)
                {
                    //  Console.WriteLine("found something in sq");
                    continue;
                }
                newOptions.Add(item);
            }

            return newOptions;

        }


        public static void GetInput()
        {
            Console.WriteLine("Enter grid size:");
            // todo: validate square number
            gridSize = int.Parse(Console.ReadLine());

            grid = new Cell[gridSize][];
            //    bool[][] blanks = new bool[gridSize][];

            //    int[,] numberOptions = new int[gridSize, gridSize];
            //  Populate(numberOptions, gridSize);
            completedCells = 0;

            int[] tempRow;

            Console.WriteLine("Enter grid one row at a time (space-separated, 0 for blank):");
            for (int i = 0; i < gridSize; i++)
            {
                grid[i] = new Cell[gridSize];
                // todo test for whitespace issues
                tempRow = Console.ReadLine().Split(' ').Select(int.Parse).ToArray();


                for (int j = 0; j < gridSize; j++)
                {
                    grid[i][j] = new Cell(i, j, tempRow[j]);


                    if (tempRow[j] != 0)
                    {
                        grid[i][j].Options = new List<int>() { j };
                        //       numberOptions[i, j] = 1;
                        completedCells++;
                    }
                    else
                    {
                        grid[i][j].Options = Enumerable.Range(1, gridSize).ToList();
                    }
                }

            }

        }

        public static int[] GetRow(int r)
        {
            int[] row = new int[gridSize];
            for (int i = 0; i < gridSize; i++)
            {
                row[i] = grid[r][i].Value;
            }
            return row;

        }
        public static int[] GetColumn(int c)
        {
            int[] col = new int[gridSize];
            for (int i = 0; i < gridSize; i++)
            {
                col[i] = grid[i][c].Value;
            }
            return col;
        }

        public static int[] GetSquare(int s)
        {
            int[] square = new int[gridSize];
            int count = 0;

            for (int i = 0; i < gridSize; i++)
            {
                for (int j = 0; j < gridSize; j++)
                {
                    if (grid[i][j].Square == s)
                    {
                        square[count] = grid[i][j].Value;
                        count++;
                    }
                }
            }

            return square;
        }


        public static void DetermineSquares()
        {
            int square = 0;

            int countA = 0;
            int countB = 0;

            int squareSize = (int)Math.Sqrt(gridSize);

            for (int i = 0; i < gridSize; i++)
            {
                for (int j = 0; j < gridSize; j++)
                {
                    // Console.WriteLine("Count A {0}, Count B {1}, Square {2}", countA, countB, square);

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

        public static void Populate<T>(T[,] arr, T value)
        {
            for (int i = 0; i < arr.GetLength(0); i++)
            {
                for (int j = 0; j < arr.GetLength(1); j++)
                {
                    arr[i, j] = value;
                }
            }
        }

    }
}
