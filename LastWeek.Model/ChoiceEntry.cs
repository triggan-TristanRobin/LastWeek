using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LastWeek.Model
{
    public class ChoiceEntry : Entry
    {
        public List<string> Choices { get; set; }
        public string Selected { get; set; }
    }
}
