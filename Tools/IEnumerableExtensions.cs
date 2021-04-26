using System;
using System.Collections.Generic;

namespace Tools
{
    public static class IEnumerableExtensions
    {
        public static IEnumerable<T> Add<T>(this IEnumerable<T> e, T value)
        {
            foreach (var cur in e)
            {
                yield return cur;
            }
            yield return value;
        }

        public static IEnumerable<T> Update<T>(this IEnumerable<T> e, int index, T value)
        {
            var i = 0;
            foreach (var cur in e)
            {
                yield return i == index ? value : cur;
                i++;
            }
        }
    }
}
