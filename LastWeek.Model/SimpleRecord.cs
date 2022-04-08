using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LastWeek.Model
{
    public class SimpleRecord : Record
    {
        public List<string> Answers { get; set; }

        public override IEnumerable<string> AnswersAsTextEnumerable()
        {
            return Answers;
        }

        public override void Update(Record recordToUdpate)
        {
            base.Update(recordToUdpate);
            if (recordToUdpate is SimpleRecord simple)
                Answers = simple.Answers;
            else
                throw new ArgumentException("Cannot update record of different type");
        }
    }
}
