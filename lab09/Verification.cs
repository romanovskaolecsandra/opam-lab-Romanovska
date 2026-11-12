using System;
using System.Collections.Generic;

namespace Lab09Sorting
{
    public static class Verification
    {
        public static bool IsSorted<T>(T[] a, Func<T, T, int> cmp)
        {
            if (a == null || a.Length <= 1) return true;
            for (int i = 0; i < a.Length - 1; i++)
            {
                if (cmp(a[i], a[i + 1]) > 0)
                    return false;
            }
            return true;
        }

        public static bool IsPermutationOf<T>(T[] sorted, T[] original) where T : notnull
        {
            if (sorted == null || original == null) return sorted == original;
            if (sorted.Length != original.Length) return false;

            var counts = new Dictionary<T, int>(original.Length);
            for (int i = 0; i < original.Length; i++)
            {
                if (counts.TryGetValue(original[i], out int c))
                    counts[original[i]] = c + 1;
                else
                    counts[original[i]] = 1;
            }

            for (int i = 0; i < sorted.Length; i++)
            {
                if (!counts.TryGetValue(sorted[i], out int c) || c == 0)
                    return false;
                counts[sorted[i]] = c - 1;
            }

            return true;
        }
    }
}
