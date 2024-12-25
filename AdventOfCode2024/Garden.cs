using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2024
{
    public static class Garden
    {
        public static List<Region> Regions { get; } = new();
        public static List<GardenPlot> GardenPlots { get; } = new();

        public static void SetRegions()
        {
            while (GardenPlots.Any(x => x.Region == null))
            {
                var gardenPlot = GardenPlots.First(x => x.Region == null);
                var region = new Region(gardenPlot.Plant);
                gardenPlot.Region = region;
                region.GardenPlots.Add(gardenPlot);
                Regions.Add(region);
                SetRegionForPlotRange(gardenPlot, region);
            }

        }

        internal static int GetFencingPricePart1()
        {
            int result = 0;

            // Calculate fence price per region
            foreach (var region in Regions)
            {
                var area = region.GardenPlots.Count;
                var perimeter = PerimeterPart1(region);
                var subResult = area * perimeter;
                //Console.WriteLine($"Region {region.PlantType}: Area * perimeter:  {area} * {perimeter} = {subResult}");
                result += subResult;
            }
            return result;
        }

        public static int GetFencingPricePart2()
        {
            int result = 0;

            // Calculate fence price per region
            foreach (var region in Regions)
            {
                var area = region.GardenPlots.Count;
                var perimeter = PerimeterPart2(region);
                var subResult = area * perimeter;
                //Console.WriteLine($"Region {region.PlantType}: Area * perimeter:  {area} * {perimeter} = {subResult}");
                result += subResult;
            }
            return result;
        }

        private static int PerimeterPart2(Region region)
        {
            // Keep track of sides, if a x already exists and it's not interrupted we don't have to calculate it
            Dictionary<int, List<int>> topSides = new();
            Dictionary<int, List<int>> rightSides = new();
            Dictionary<int, List<int>> bottomSides = new();
            Dictionary<int, List<int>> leftSides = new();
            var topPerimeter = 0;
            var rightPerimeter = 0;
            var bottomPerimeter = 0;
            var leftPerimeter = 0;
            foreach (var gardenPlot in region.GardenPlots.Where(x => x.IsPartOfFence).OrderBy(gp=>gp.X).ThenBy(gp=>gp.Y))
            {
                // Check if I can find a plot above, 
                var top = region.GardenPlots.FirstOrDefault(g => g.X == gardenPlot.X && g.Y == gardenPlot.Y - 1);
                // If none is found the top side of this plot needs a fence
                if (top == null)
                {
                    // Now let's check: If this one for example is x2 and y1 and there is already another one
                    // that is x1 y1 I don't have to count this one as it is part of the same side
                    // It's important that I can reach it uninterupted, otherwise it will count as a new side
                    if (topSides.TryGetValue(gardenPlot.Y, out List<int> xValues))
                    {
                        xValues.Add(gardenPlot.X);
                        // If I can't connect to one direct next to me, i on't have to count
                        if (!xValues.Contains(gardenPlot.X - 1) && !xValues.Contains(gardenPlot.X + 1))
                        {
                            topPerimeter++;
                        }
                    }
                    // If none is found create a new side
                    else
                    {
                        topSides.Add(gardenPlot.Y, new List<int>() { gardenPlot.X });
                        topPerimeter++;
                    }
                }


                var right = region.GardenPlots.FirstOrDefault(g => g.X == gardenPlot.X + 1 && g.Y == gardenPlot.Y);
                if (right == null)
                {
                    // Now let's check: If this one for example is x2 and y3 and there is already another one
                    // that is x2 y4 I don't have to count this one as it is part of the same side
                    // It's important that I can reach it uninterupted, otherwise it will count as a new side
                    if (rightSides.TryGetValue(gardenPlot.X, out List<int> yValues))
                    {
                        yValues.Add(gardenPlot.Y);
                        // If I can't connect to one direct next to me, i on't have to count
                        if (!yValues.Contains(gardenPlot.Y - 1) && !yValues.Contains(gardenPlot.Y + 1))
                        {
                            rightPerimeter++;
                        }
                    }
                    // If none is found create a new side
                    else
                    {
                        rightSides.Add(gardenPlot.X, new List<int>() { gardenPlot.Y });
                        rightPerimeter++;
                    }
                }


                var bottom = region.GardenPlots.FirstOrDefault(g => g.X == gardenPlot.X && g.Y == gardenPlot.Y + 1);
                if (bottom == null)
                {
                    // Now let's check: If this one for example is x2 and y1 and there is already another one
                    // that is x1 y1 I don't have to count this one as it is part of the same side
                    // It's important that I can reach it uninterupted, otherwise it will count as a new side
                    if (bottomSides.TryGetValue(gardenPlot.Y, out List<int> xValues))
                    {
                        xValues.Add(gardenPlot.X);
                        // If I can't connect to one direct next to me, i on't have to count
                        if (!xValues.Contains(gardenPlot.X - 1) && !xValues.Contains(gardenPlot.X + 1))
                        {
                            bottomPerimeter++;
                        }
                    }
                    // If none is found create a new side
                    else
                    {
                        bottomSides.Add(gardenPlot.Y, new List<int>() { gardenPlot.X });
                        bottomPerimeter++;
                    }
                }

                var left = region.GardenPlots.FirstOrDefault(g => g.X == gardenPlot.X - 1 && g.Y == gardenPlot.Y);
                if (left == null)
                {
                    // Now let's check: If this one for example is x2 and y3 and there is already another one
                    // that is x2 y4 I don't have to count this one as it is part of the same side
                    // It's important that I can reach it uninterupted, otherwise it will count as a new side
                    if (leftSides.TryGetValue(gardenPlot.X, out List<int> yValues))
                    {
                        yValues.Add(gardenPlot.Y);
                        // If I can't connect to one direct next to me, i on't have to count
                        if (!yValues.Contains(gardenPlot.Y - 1) && !yValues.Contains(gardenPlot.Y + 1))
                        {
                            leftPerimeter++;
                        }
                    }
                    // If none is found create a new side
                    else
                    {
                        leftSides.Add(gardenPlot.X, new List<int>() { gardenPlot.Y });
                        leftPerimeter++;
                    }
                }
            }
            return topPerimeter + rightPerimeter + bottomPerimeter + leftPerimeter;
        }

        private static int PerimeterPart1(Region region)
        {
            var perimeter = 0;
            foreach (var gardenPlot in region.GardenPlots)
            {
                var subPerimeter = 0;
                var top = region.GardenPlots.FirstOrDefault(g => g.X == gardenPlot.X && g.Y == gardenPlot.Y - 1);
                if (top == null)
                    subPerimeter++;

                var right = region.GardenPlots.FirstOrDefault(g => g.X == gardenPlot.X + 1 && g.Y == gardenPlot.Y);
                if (right == null)
                    subPerimeter++;

                var bottom = region.GardenPlots.FirstOrDefault(g => g.X == gardenPlot.X && g.Y == gardenPlot.Y + 1);
                if (bottom == null)
                    subPerimeter++;

                var left = region.GardenPlots.FirstOrDefault(g => g.X == gardenPlot.X - 1 && g.Y == gardenPlot.Y);
                if (left == null)
                    subPerimeter++;
                if (subPerimeter > 0)
                    gardenPlot.IsPartOfFence = true;
                perimeter += subPerimeter;
            }
            return perimeter;
        }

        static void SetRegionForPlotRange(GardenPlot gardenPlot, Region region)
        {
            var result = new List<GardenPlot>();
            var top = GardenPlots.FirstOrDefault(g => g.X == gardenPlot.X && g.Y == gardenPlot.Y - 1 &&
            g.Plant == gardenPlot.Plant &&
            g.Region == null);
            if (top != null)
            {
                top.Region = region;
                region.GardenPlots.Add(top);
                SetRegionForPlotRange(top, region);
            }

            var right = GardenPlots.FirstOrDefault(g => g.X == gardenPlot.X + 1 && g.Y == gardenPlot.Y &&
            g.Plant == gardenPlot.Plant &&
            g.Region == null);
            if (right != null)
            {
                right.Region = region;
                region.GardenPlots.Add(right);
                SetRegionForPlotRange(right, region);
            }


            var bottom = GardenPlots.FirstOrDefault(g => g.X == gardenPlot.X && g.Y == gardenPlot.Y + 1 &&
            g.Plant == gardenPlot.Plant &&
            g.Region == null);
            if (bottom != null)
            {
                bottom.Region = region;
                region.GardenPlots.Add(bottom);
                SetRegionForPlotRange(bottom, region);
            }

            var left = GardenPlots.FirstOrDefault(g => g.X == gardenPlot.X - 1 && g.Y == gardenPlot.Y &&
            g.Plant == gardenPlot.Plant &&
            g.Region == null);
            if (left != null)
            {
                left.Region = region;
                region.GardenPlots.Add(left);
                SetRegionForPlotRange(left, region);
            }
        }
    }
}
