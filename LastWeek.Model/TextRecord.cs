using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LastWeek.Model
{
    public class TextRecord : Record
    {
        public string Answer { get; set; }

        public override IEnumerable<string> AnswersAsTextEnumerable()
        {
            return new List<string> { Answer };
        }

        public override void Update(Record recordToUdpate)
        {
            base.Update(recordToUdpate);
            if (recordToUdpate is TextRecord text)
                Answer = text.Answer;
            else
                throw new ArgumentException("Cannot update record of different type");
        }
    }
}
