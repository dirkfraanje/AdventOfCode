using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2024
{
    public class Region
    {
        public List<GardenPlot> GardenPlots { get; } = new();
        public char PlantType { get; }
        public Region(char plantType)
        {
            PlantType = plantType;
        }
    }
}
