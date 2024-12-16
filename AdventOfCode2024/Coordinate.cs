using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2024
{
    internal class Coordinate : CoordinateBase
    {
        
        public char Value;
        public bool Visited;
        public bool HasAntinodeOnly1, HasAntinode;

        public Coordinate(int x, int y, char value) : base(x, y)
        {
            Value = value;
        }
        //public string Color;
    }
}
