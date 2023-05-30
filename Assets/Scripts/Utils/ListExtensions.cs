using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Utils
{
    public static class ListExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T MinBy<T, TKey>(this IReadOnlyList<T> source, Func<T, TKey> selector) where TKey : IComparable
        {
            if (source.Count == 0)
                throw new ArgumentException("List must not be empty");

            if (source.Count == 1)
                return source[0];

            var minElement = source[0];
            var minValue = selector(minElement);
            foreach (var item in source)
            {
                var value = selector(item);
                if (value.CompareTo(minValue) < 0)
                {
                    minElement = item;
                    minValue = value;
                }
            }

            return minElement;
        }
    }
}