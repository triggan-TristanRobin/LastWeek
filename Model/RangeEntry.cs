using System;
using System.Numerics;

namespace Model
{
    public class RangeEntry : Entry
    {
        public Range Boundaries { get; set; }
        public Double Selected { get; set; }

        public RangeEntry()
        {
        }

        public RangeEntry(Range boundaries)
        {
            Boundaries = boundaries;
        }
    }
}
