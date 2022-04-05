using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text.Json.Serialization;

namespace LastWeek.Model
{
    public class RangeRecord : Record
    {
        public Range Boundaries { get; set; }
        [JsonIgnore]
        public double Start => Boundaries.Start.Value;
        public double End => Boundaries.End.Value;
        [JsonIgnore]
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
