using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LastWeek.Model
{
    public class ChoiceRecord : Record
    {
        public List<string> Choices { get; set; }
        public string Selected { get; set; }

        public override IEnumerable<string> AnswersAsTextEnumerable()
        {
            return new List<string> { Selected };
        }
    }
}
