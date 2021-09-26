using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace ExtensionMinder.CollectionExt
{
    public static class CollectionExtensions
    {
        [DebuggerStepThrough]
        public static IEnumerable<T> Each<T>(this IEnumerable<T> source, Action<T> action)
        {
            var items = source as T[] ?? source.ToArray();
            foreach (var item in items)
                action(item);
            return items;
        }

        public static TSource MaxBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector)
        {
            return source.MaxBy(selector, Comparer<TKey>.Default);
        }


        /// <summary>
        ///     Returns the maximal element of the given sequence, based on
        ///     the given projection and the specified comparer for projected values.
        /// </summary>
        /// <remarks>
        ///     If more than one element has the maximal projected value, the first
        ///     one encountered will be returned. This overload uses the default comparer
        ///     for the projected type. This operator uses immediate execution, but
        ///     only buffers a single result (the current maximal element).
        /// </remarks>
        /// <typeparam name="TSource">Type of the source sequence</typeparam>
        /// <typeparam name="TKey">Type of the projected element</typeparam>
        /// <param name="source">Source sequence</param>
        /// <param name="selector">Selector to use to pick the results to compare</param>
        /// <param name="comparer">Comparer to use to compare projected values</param>
        /// <returns>The maximal element, according to the projection.</returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="source" />, <paramref name="selector" />
        ///     or <paramref name="comparer" /> is null
        /// </exception>
        /// <exception cref="InvalidOperationException"><paramref name="source" /> is empty</exception>
        public static TSource MaxBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector,
            IComparer<TKey> comparer)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (selector == null) throw new ArgumentNullException(nameof(selector));
            if (comparer == null) throw new ArgumentNullException(nameof(comparer));
            using (var sourceIterator = source.GetEnumerator())
            {
                if (!sourceIterator.MoveNext()) throw new InvalidOperationException("Sequence contains no elements");
                var max = sourceIterator.Current;
                var maxKey = selector(max);
                while (sourceIterator.MoveNext())
                {
                    var candidate = sourceIterator.Current;
                    var candidateProjected = selector(candidate);
                    if (comparer.Compare(candidateProjected, maxKey) > 0)
                    {
                        max = candidate;
                        maxKey = candidateProjected;
                    }
                }

                return max;
            }
        }

        [DebuggerStepThrough]
        public static IEnumerable<T> Map<T>(this IEnumerable<T> source, Func<T, T> action)
        {
            return source.Select(action);
        }

        [DebuggerStepThrough]
        public static IEnumerable<T> GetDuplicates<T>(this IEnumerable<T> source)
        {
            return
                from item in source
                group item by item
                into g
                where g.Count() > 1
                select g.Key;
        }

        [DebuggerStepThrough]
        public static bool In<T>(this T source, params T[] list)
        {
            if (null == source) throw new ArgumentNullException(nameof(source));
            return list.Contains(source);
        }

        [DebuggerStepThrough]
        public static bool In<T>(this T source, IEnumerable<T> list)
        {
            if (null == source) throw new ArgumentNullException(nameof(source));
            return list.Contains(source);
        }

        [DebuggerStepThrough]
        public static void AddRange<T>(this ICollection<T> collection, IEnumerable<T> enumerable)
        {
            foreach (var cur in enumerable) collection.Add(cur);
        }

        public static T PickRandom<T>(this IEnumerable<T> source)
        {
            return source.PickRandom(1).Single();
        }

        public static IEnumerable<T> PickRandom<T>(this IEnumerable<T> source, int count)
        {
            return source.Shuffle().Take(count);
        }

        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source)
        {
            return source.OrderBy(x => Guid.NewGuid());
        }

        public static IEnumerable<IList<object>> AsTableWith(this IEnumerable first, params IEnumerable[] lists)
        {
            var iterator1 = first.GetEnumerator();
            var enumerators = lists.Select(x => x.GetEnumerator()).ToList();
            {
                while (iterator1.MoveNext())
                {
                    var result = new List<object> {iterator1.Current};
                    enumerators.Each(x =>
                    {
                        x.MoveNext();
                        result.Add(x.Current);
                    });
                    yield return result;
                }
            }
        }

        public static string ToCsv<T>(this IEnumerable<T> items, char separator)
        {
            var output = "";
            var delimiter = ',';
            var properties = typeof(T).GetProperties()
                .Where(n =>
                    n.PropertyType == typeof(string)
                    || n.PropertyType == typeof(bool)
                    || n.PropertyType == typeof(char)
                    || n.PropertyType == typeof(byte)
                    || n.PropertyType == typeof(decimal)
                    || n.PropertyType == typeof(int)
                    || n.PropertyType == typeof(System.DateTime)
                    || n.PropertyType == typeof(System.DateTime?));
            using (var sw = new StringWriter())
            {
                var header = properties
                    .Select(n => n.Name)
                    .Aggregate((a, b) => a + delimiter + b);
                sw.WriteLine(header);
                foreach (var item in items)
                {
                    var row = properties
                        .Select(n => n.GetValue(item, null))
                        .Select(n => n?.ToString())
                        .Aggregate((a, b) => a + delimiter + b);
                    sw.WriteLine(row);
                }

                output = sw.ToString();
            }

            return output;
        }
    }
}