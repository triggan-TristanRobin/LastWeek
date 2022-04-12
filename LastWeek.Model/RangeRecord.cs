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
        public int Start => Boundaries.Start.Value;
        [JsonIgnore]
        public int End => Boundaries.End.Value;
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

        public override void Update(Record recordToUdpate)
        {
            base.Update(recordToUdpate);
            if (recordToUdpate is RangeRecord range)
            {
                Boundaries = range.Boundaries;
                Selected = range.Selected;
            }
            else
                throw new ArgumentException("Cannot update record of different type");
        }

        public override Record GetTemplate()
        {
            Record template = new RangeRecord { Boundaries = new Range(Start, End) };
            template.Question = Question;
            return template;
        }
    }
}
