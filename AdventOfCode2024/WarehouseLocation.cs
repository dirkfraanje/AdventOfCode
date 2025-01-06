using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2024
{
    public class WarehouseLocation : CoordinateBase
    {
        public char Value { get; set; }
        public WarehouseLocation(int x, int y, char value) : base(x, y)
        {
            Value = value;
        }

        internal void FlipBoxSide()
        {
            Value = Value == '[' ? ']' : Value == ']' ?'[': throw new InvalidOperationException();
        }
    }
}
