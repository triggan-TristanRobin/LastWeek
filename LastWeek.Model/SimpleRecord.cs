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
    }
}
