using LastWeek.Model.Enums;
using System;
using System.Text.Json;

namespace LastWeek.Model
{
    public class Entry : Entity
    {
        public string Question { get; set; }
    }
}