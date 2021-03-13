using System.Numerics;

namespace Model
{
    public class RangeEntry : Entry
    {
        public Vector<int> Boundaries { get; set; }
        public int Selected { get; set; }
    }
}
