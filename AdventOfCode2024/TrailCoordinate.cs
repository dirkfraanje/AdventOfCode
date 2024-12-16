using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2024
{
    internal class TrailCoordinate : CoordinateBase
    {
        public int Height;
        public List<TrailCoordinate> _map;
        public HashSet<TrailCoordinate> ReachedBy { get; } = new();
        public int ReachedByTotal { get; private set; }
        public TrailCoordinate(int x, int y, int height, List<TrailCoordinate> map) : base(x, y)
        {
            Height = height;
            _map = map;
        }

       

        public void EndsItCanReach(TrailCoordinate comingFrom)
        {
            // Prevent looping 
            if (Height == 9)
            {
                ReachedBy.Add(comingFrom);
                ReachedByTotal++;
                return;
            }
            // Only go to higher numbers
            List<TrailCoordinate> connectedLocations = GetConnectedLocations(1);
            var result = 0;
            foreach (var h in connectedLocations)
            {
                h.EndsItCanReach(comingFrom);
            }
        }

        private List<TrailCoordinate> GetConnectedLocations(int height)
        {
            var result = new List<TrailCoordinate>();
            var top = _map.FirstOrDefault(location => location.X == X && location.Y == Y - 1 && location.Height == Height + height);
            if (top != null) result.Add(top);
            var bottom = _map.FirstOrDefault(location => location.X == X && location.Y == Y + 1 && location.Height == Height + height);
            if (bottom != null) result.Add(bottom);
            var lef = _map.FirstOrDefault(location => location.X == X - 1 && location.Y == Y && location.Height == Height + height);
            if (lef != null) result.Add(lef);
            var right = _map.FirstOrDefault(location => location.X == X + 1 && location.Y == Y && location.Height == Height + height);
            if (right != null) result.Add(right);
            return result;
        }

        
    }
}
