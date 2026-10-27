using System;
using System.Collections.Generic;

namespace Lab07
{
    public static class Algorithms
    {
        // Завдання 1: Фільтрація (Відбір)
        public static List<T> Filter<T>(List<T> src, Func<T, bool> keep)
        {
            if (src == null) throw new ArgumentNullException(nameof(src));
            if (keep == null) throw new ArgumentNullException(nameof(keep));

            List<T> result = new List<T>();
            foreach (var item in src)
            {
                if (keep(item)) result.Add(item);
            }
            return result;
        }

        // Завдання 2: Проекція (Перетворення)
        public static List<TOut> Map<TIn, TOut>(List<TIn> src, Func<TIn, TOut> convert)
        {
            if (src == null) throw new ArgumentNullException(nameof(src));
            if (convert == null) throw new ArgumentNullException(nameof(convert));

            List<TOut> result = new List<TOut>();
            foreach (var item in src)
            {
                result.Add(convert(item));
            }
            return result;
        }

        // Завдання 3: Агрегація
        public static TAcc Aggregate<T, TAcc>(List<T> src, TAcc start, Func<TAcc, T, TAcc> step)
        {
            if (src == null) throw new ArgumentNullException(nameof(src));
            if (step == null) throw new ArgumentNullException(nameof(step));

            TAcc accumulator = start;
            foreach (var item in src)
            {
                accumulator = step(accumulator, item);
            }
            return accumulator;
        }

        // Завдання 4: Групування
        public static Dictionary<TKey, List<T>> GroupBy<T, TKey>(List<T> src, Func<T, TKey> keyOf)
        {
            if (src == null) throw new ArgumentNullException(nameof(src));
            if (keyOf == null) throw new ArgumentNullException(nameof(keyOf));

            var result = new Dictionary<TKey, List<T>>();
            foreach (var item in src)
            {
                TKey key = keyOf(item);
                if (!result.ContainsKey(key))
                {
                    result[key] = new List<T>();
                }
                result[key].Add(item);
            }
            return result;
        }
    }
}
