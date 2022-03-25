using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LastWeek.Model
{
    public class Review : Entity
    {
        public ReviewStatus Status { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<Entry> Entries { get; set; }
        public User User { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is Review review)
            {
                foreach (var prop in typeof(Review).GetProperties())
                {
                    var property = prop.GetValue(this);
                    var toCompareProperty = prop.GetValue(review);
                    if (property != null && !property.Equals(toCompareProperty))
                    {
                        if (prop.PropertyType != typeof(string) && typeof(IEnumerable).IsAssignableFrom(prop.PropertyType))
                        {
                            IEnumerator rightEnumerator = (property as IEnumerable).GetEnumerator();
                            rightEnumerator.Reset();
                            foreach (object leftItem in toCompareProperty as IEnumerable)
                            {
                                if (!rightEnumerator.MoveNext() || !leftItem.Equals(rightEnumerator.Current))
                                {
                                    return false;
                                }
                            }
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
                return true;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Guid, Created, Updated, Deleted, Status, StartDate, EndDate, Entries);
        }

        public bool Update(Review updateDate)
        {
            if (updateDate.Guid != this.Guid)
            {
                return false;
            }
            foreach (var prop in typeof(Review).GetProperties())
            {
                if (prop.Name == "User") continue;
                var updatedProperty = prop.GetValue(updateDate);
                prop.SetValue(this, updatedProperty);
            }
            return true;
        }

        public Review GetTemplate()
        {
            var template = new Review
            {
                Guid = new Guid(),
                Status = ReviewStatus.New,
                Entries = new()
            };
            foreach(var entry in Entries)
            {
                template.Entries.Add(entry.GetTemplate());
            }
            return template;
        }
    }
}
