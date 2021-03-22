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
                return this.Guid == review.Guid;
                //foreach (var prop in typeof(Review).GetProperties())
                //{
                //    if (prop.PropertyType != typeof(string) && typeof(IEnumerable).IsAssignableFrom(prop.PropertyType))
                //    {
                //        IEnumerator rightEnumerator = (prop.GetValue(this) as IEnumerable).GetEnumerator();
                //        rightEnumerator.Reset();
                //        foreach (object leftItem in prop.GetValue(review) as IEnumerable)
                //        {
                //            // unequal amount of items
                //            if (!rightEnumerator.MoveNext())
                //                return false;
                //            else
                //            {
                //                if (!leftItem.Equals(rightEnumerator.Current))
                //                    return false;
                //            }
                //        }
                //    }
                //    else if (!prop.GetValue(this).Equals(prop.GetValue(review)))
                //    {
                //        return false;
                //    }
                //}
            }
            return false;
        }
    }
}
