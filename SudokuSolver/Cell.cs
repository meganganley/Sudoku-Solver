using System.Collections.Generic;

namespace SudokuSolver
{
    public class Cell
    {
        public Cell(int x, int y, int value)
        {
            X = x;
            Y = y;
            Value = value;
        }

        public int X { get; set; }
        public int Y { get; set; }
        public int Square { get; set; }
        public int Value { get; set; }
        public List<int> Options { get; set; }
        public bool Solved { get; set; }
      
    }
}
