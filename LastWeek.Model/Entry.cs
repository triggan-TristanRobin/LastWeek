using LastWeek.Model.Enums;
using System;
using System.Text.Json;

namespace LastWeek.Model
{
    public class Entry : Entity
    {
        public string Question { get; set; }

        public Entry GetTemplate()
        {
            Entry template = (Entry)Activator.CreateInstance(GetType());
            template.Question = Question;
            return template;
        }
    }
}