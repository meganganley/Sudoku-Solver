using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver
{
    class Cell
    {
        public Cell(int X, int Y, int Value)
        {
            this.X = X;
            this.Y = Y;
          //  this.Square = CalculateSquare();
            this.Value = Value;
        }

        public int X { get; set; }
        public int Y { get; set; }
        public int Square { get; set; }
        public int Value { get; set; }
        public List<int> Options { get; set; }


        private int CalculateSquare()
        {


            return 0;
        }



    }
}
