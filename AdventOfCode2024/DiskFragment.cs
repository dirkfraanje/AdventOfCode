using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2024
{
    internal class DiskFragment
    {
        public List<int>FileIds { get; }
        public int OriginalId { get; }  
        public DiskFragment(List<int> fileIds, int originalId)
        {
            FileIds = fileIds;
            OriginalId = originalId;
        }
    }
}
