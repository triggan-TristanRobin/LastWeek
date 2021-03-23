using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Review
    {
        public Guid Guid { get; set; }
        public ReviewStatus Status { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<Entry> Entries { get; set; }

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
    }
}
