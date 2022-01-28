using System;

namespace LastWeek.Model
{
    public abstract class Entity
    {
        public Guid Guid { get; set; }
        public DateTime Created { get; set; } = DateTime.MinValue;
        public DateTime Updated { get; set; } = DateTime.Now;
        public bool Deleted { get; set; }
    }
}