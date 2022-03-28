using System;
using System.Collections.Generic;
using System.Numerics;

namespace LastWeek.Model
{
    public class RangeRecord : Record
    {
        public Range Boundaries { get; set; }
        public double Start => Boundaries.Start.Value;
        public double End => Boundaries.End.Value;
        public double Selected { get; set; }

        public RangeRecord()
        {
            Boundaries = 1..10;
            Selected = Start;
        }

        public RangeRecord(Range boundaries)
        {
            Boundaries = boundaries;
            Selected = Start;
        }

        public override IEnumerable<string> AnswersAsTextEnumerable()
        {
            return new List<string> { $"{Selected}, on a scale from {Start} to {End}" };
        }
    }
}
