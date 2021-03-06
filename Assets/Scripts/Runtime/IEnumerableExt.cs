using System;
using System.Collections.Generic;
using System.Linq;
using ComradeVanti.CSharpTools;

namespace Dev.ComradeVanti.Wurfel
{

    // ReSharper disable once InconsistentNaming
    public static class IEnumerableExt
    {

        public static void Iter<T>(this IEnumerable<T> items, Action<T> action)
        {
            foreach (var item in items) action(item);
        }

        public static T Reduce<T>(this IEnumerable<T> items, Func<T, T, T> reducer)
        {
            var array = items.ToArray();

            if (array.Length == 0)
                throw new ArgumentException("Cannot reduce empty collection", nameof(items));

            return array.Skip(1).Aggregate(array[0], reducer);
        }

        public static IEnumerable<IEnumerable<T>> Chunks<T>(this IEnumerable<T> items, int chunkSize)
        {
            var array = items.ToArray();

            IEnumerable<T> ChunkAt(int i)
            {
                for (; i < array.Length; i++)
                    yield return array[i];
            }

            for (var i = 0; i < array.Length; i += chunkSize)
                yield return ChunkAt(i);
        }

        public static T FirstBy<T>(this IEnumerable<T> items, Func<T, float> sorter) =>
            items.OrderBy(sorter).First();

        public static T LastBy<T>(this IEnumerable<T> items, Func<T, float> sorter) =>
            items.OrderByDescending(sorter).First();

        public static IEnumerable<T> Slice<T>(this IEnumerable<T> items, int start, int count) =>
            items.Skip(start).Take(count);

        public static IEnumerable<T> Collect<T>(this IEnumerable<Opt<T>> opts) =>
            opts.Where(opt => opt.IsSome())
                .Select(opt => opt.Get());

        public static bool None<T>(this IEnumerable<T> items, Func<T, bool> pred) =>
            !items.Any(pred);

        public static T FirstOr<T>(this IEnumerable<T> items, T orDefault)
        {
            try
            {
                return items.First();
            }
            catch
            {
                return orDefault;
            }
        }

        public static Opt<T> TryFirst<T>(this IEnumerable<T> items, Func<T, bool> pred) =>
            items.Where(pred).Select(Opt.Some).FirstOr(Opt.None<T>());

        public static IEnumerable<T> Without<T>(this IEnumerable<T> items, T item) =>
            items.Where(it => !Equals(it, item));

        public static T Random<T>(this IEnumerable<T> items)
        {
            var array = items.ToArray();
            var index = UnityEngine.Random.Range(0, array.Length);
            return array[index];
        }

        public static T RandomElementByWeight<T>(this IEnumerable<T> items, Func<T, float> weightSelector)
        {
            var array = items.ToArray();
            var totalWeight = array.Sum(weightSelector);
            var itemWeightIndex = UnityEngine.Random.Range(0, totalWeight);
            float currentWeightIndex = 0;

            foreach (var item in from weightedItem in array select new { Value = weightedItem, Weight = weightSelector(weightedItem) })
            {
                currentWeightIndex += item.Weight;

                if (currentWeightIndex >= itemWeightIndex)
                    return item.Value;
            }

            throw new Exception("Empty sequence");
        }

    }

}