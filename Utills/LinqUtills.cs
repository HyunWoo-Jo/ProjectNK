using System.Collections.Generic;
using System;
using UnityEngine;
using System.Linq;

namespace N.Utills
{
    public static class LinqUtills
    {
        public static T MinBy<T, TKey>(this IEnumerable<T> source, Func<T, TKey> selector)
        where TKey : IComparable<TKey> {
            return source.Aggregate((minItem, nextItem) =>
                selector(nextItem).CompareTo(selector(minItem)) < 0 ? nextItem : minItem);
        }
    }
}
