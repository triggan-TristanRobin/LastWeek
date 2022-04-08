using LastWeek.Model.Enums;
using System;
using System.Collections.Generic;
using System.Text.Json;

namespace LastWeek.Model
{
    public class Record : Entity
    {
        public int Order { get; set; }
        public string Question { get; set; }

        public Record GetTemplate()
        {
            Record template = (Record)Activator.CreateInstance(GetType());
            template.Question = Question;
            return template;
        }

        public virtual IEnumerable<string> AnswersAsTextEnumerable()
        {
            throw new NotImplementedException();
        }

        public virtual void Update(Record recordToUdpate)
        {
            Order = recordToUdpate.Order;
            Question = recordToUdpate.Question;
        }
    }
}