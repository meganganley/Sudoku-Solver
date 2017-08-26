using System;
using System.Collections.Generic;
using System.Diagnostics;
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


            while (completedCells < (gridSize * gridSize))
            {

                for (int i = 0; i < gridSize; i++)
                {
                    for (int j = 0; j < gridSize; j++)
                    {
                        grid[i][j].Options = UpdateCellOptions(i, j, grid[i][j].Square);
                    }
                }



                // 3. Searching for Single Candidates: ROW

                for (int row = 0; row < gridSize; row++)
                {
                   
                    for (int value = 1; value <= gridSize; value++)
                    {
                        if (Array.Exists(GetRow(row), element => element == value))
                        {
                            continue;
                        }

                        // TODO: don't do when value already used
                        int result = CheckRowForValueInOptions(row, value);
                        if (result != -1)
                        {
                            grid[row][result].Value = value;
                            grid[row][result].Options = new List<int>() { value };
                            completedCells++;
                            Console.WriteLine("Solved a number: [{0},{1}], value = {2}", row, result, grid[row][result].Value);
                        }
                    }
                }


                // 3. Searching for Single Candidates: COLUMN

                for (int col = 0; col < gridSize; col++)
                {
                    for (int value = 1; value <= gridSize; value++)
                    {
                        if (Array.Exists(GetColumn(col), element => element == value))
                        {
                            continue;
                        }


                        int result = CheckColForValueInOptions(col, value);
                        if (result != -1)
                        {
                            grid[result][col].Value = value;
                            grid[result][col].Options = new List<int>() { value };
                            completedCells++;
                            Console.WriteLine("Solved a number: [{0},{1}], value = {2}", result, col, grid[result][col].Value);
                        }
                    }
                }




                // METHOD: 5. Searching for missing numbers in rows and columns:
//
                for (int i = 0; i < gridSize; i++)
                {
                    for (int j = 0; j < gridSize; j++)
                    {
                        if (grid[i][j].Options.Count > 1)
                        {
                            Console.WriteLine("Checking [{0},{1}]", i, j);
                            grid[i][j].Options = UpdateCellOptions(i, j, grid[i][j].Square);
                            if (grid[i][j].Options.Count == 1)
                            {
                                grid[i][j].Value = grid[i][j].Options[0];
                                completedCells++;
                                Console.WriteLine("Solved a number: [{0},{1}], value = {2}", i, j, grid[i][j].Value);
                            }
                            else
                            {

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
             
            Console.WriteLine("DONE");

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


        static List<int> GetAvailableRowValues()
        {
            return null;
        }


        static int CheckRowForValueInOptions(int row, int value)
        {
            int count = 0;
            int index = -1;
            for (int j = 0; j < gridSize; j++)
            {

                if (grid[row][j].Options.Contains(value))
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

        static int CheckColForValueInOptions(int col, int value)
        {
            int count = 0;
            int index = -1;
            for (int i = 0; i < gridSize; i++)
            {

                if (grid[i][col].Options.Contains(value))
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


        static List<int> UpdateCellOptions(int x, int y, int s)
        {

            int[] row = GetRow(x);
            int[] col = GetColumn(y);
            int[] square = GetSquare(s);

            //            if (OnlyOneZero(row))
            //            {
            //                return 
            //            }

            //            Stopwatch stopWatch = new Stopwatch();
            //            stopWatch.Start();

            List<int> possibleRowValues = Enumerable.Range(1, 9).Except(row).ToList();
            List<int> possibleColValues = Enumerable.Range(1, 9).Except(col).ToList();
            List<int> possibleSquareValues = Enumerable.Range(1, 9).Except(square).ToList();

            var test = possibleRowValues.Intersect(possibleColValues).Intersect(possibleSquareValues).ToList();


            //  return test;


            //            TimeSpan ts = stopWatch.Elapsed;
            //
            //            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
            //            ts.Hours, ts.Minutes, ts.Seconds,
            //            ts.Milliseconds / 10);
            //            Console.WriteLine("RunTime " + elapsedTime);

            //            stopWatch = new Stopwatch();
            //            stopWatch.Start();



            // searching for single candidates

            List<int> options = grid[x][y].Options;
            List<int> newOptions = new List<int>();

            // todo very inefficient?
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


            //            ts = stopWatch.Elapsed;
            //
            //            elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
            //            ts.Hours, ts.Minutes, ts.Seconds,
            //            ts.Milliseconds / 10);
            //            Console.WriteLine("RunTime " + elapsedTime);


            return newOptions;

        }

        public static bool OnlyOneZero(int[] arr)
        {
            int count = 0;
            for (int i = 0; i < arr.Length; i++)
            {
                if (arr[i] == 0)
                {
                    count++;
                }
                else
                {

                }
            }

            if (count == 1)
            {
                return true;
            }
            return false;
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

                    Console.WriteLine("test, {0}", j);

                    grid[i][j] = new Cell(i, j, tempRow[j]);


                    if (tempRow[j] != 0)
                    {
                        grid[i][j].Options = new List<int>() { tempRow[j] };
                        //       numberOptions[i, j] = 1;
                        completedCells++;
                    }
                    else
                    {
                        grid[i][j].Options = Enumerable.Range(1, gridSize).ToList();
                    }
                }

                Console.WriteLine("test, finished row {0}", i);

            }
            Console.WriteLine("test, finished all rows");


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
