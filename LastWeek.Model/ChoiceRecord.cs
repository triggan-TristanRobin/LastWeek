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

        public override void Update(Record recordToUdpate)
        {
            base.Update(recordToUdpate);
            if (recordToUdpate is ChoiceRecord choice)
            {
                Choices = choice.Choices;
                Selected = choice.Selected;
            }
            else
                throw new ArgumentException("Cannot update record of different type");
        }
    }
}
