using LastWeek.Model.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LastWeek.Model
{
    public class Review : Entity, IComparable<Review>
    {
        public ReviewStatus Status { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<Record> Records { get; set; }
        public User User { get; set; }
        public ReviewType Type { get; set; }

        public Review() : this(ReviewStatus.New)
        {
        }

        public Review(ReviewStatus status = ReviewStatus.New, List<Record> records = null)
        {
            Status = status;
            Records = records ?? new List<Record>();
        }

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
            return HashCode.Combine(Guid, Created, Updated, Deleted, Status, StartDate, EndDate, Records);
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
                Records = new()
            };
            foreach(var record in Records)
            {
                template.Records.Add(record.GetTemplate());
            }
            return template;
        }

        public void Activate()
        {
            Status = Status == ReviewStatus.New ? ReviewStatus.Active : Status;
        }

        public void Validate()
        {
            Status = Status != ReviewStatus.Archived ? ReviewStatus.Validated : Status;
        }

        public void Archive()
        {
            Status = ReviewStatus.Archived;
        }

        public int CompareTo(Review other)
        {
            return other == null ? 1 : other.StartDate.CompareTo(StartDate);
        }

        public Period GetPeriod()
        {
            var days = (EndDate - StartDate).Days;
            return Enum.IsDefined(typeof(Period), days) ? (Period)days : Period.Custom;
        }
    }
}
