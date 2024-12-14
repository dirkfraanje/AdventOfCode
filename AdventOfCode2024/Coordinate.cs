using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2024
{
    internal class Coordinate
    {
        public Coordinate(int x, int y, char value)
        {
            X = x; Y = y; Value = value; 
        }
        public int X; public int Y;
        public char Value;
        public bool Visited;
        public bool HasAntinodeOnly1, HasAntinode;
        //public string Color;
    }
}
