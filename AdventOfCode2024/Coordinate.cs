using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2024
{
    internal struct Coordinate
    {
        public Coordinate(int x, int y, char value, string color)
        {
            X = x; Y = y; Value = value; Color = color;
        }
        public int X; public int Y;
        public char Value;
        public string Color;
    }
}
