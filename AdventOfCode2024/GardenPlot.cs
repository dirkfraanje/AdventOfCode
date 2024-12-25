using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2024
{
    public class GardenPlot : CoordinateBase
    {
        public char Plant { get; }
        public Region Region { get; set; }
        public bool IsPartOfFence { get; internal set; }

        public GardenPlot(int x, int y, char plant) : base(x, y)
        {
            Plant = plant;
        }
    }
}
