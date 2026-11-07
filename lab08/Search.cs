using System;

public static class Search
{
    // Завдання 1: Лінійний пошук з лічильником
    public static int LinearSearch<T>(T[] a, T target, Func<T, T, int> cmp, out int comparisons)
    {
        comparisons = 0;
        for (int i = 0; i < a.Length; i++)
        {
            comparisons++;
            if (cmp(a[i], target) == 0)
            {
                return i;
            }
        }
        return -1;
    }

    // Завдання 2: Правильний бінарний пошук
    public static int BinarySearch<T>(T[] a, T target, Func<T, T, int> cmp, out int comparisons)
    {
        comparisons = 0;
        int lo = 0;
        int hi = a.Length - 1;

        while (lo <= hi)
        {
            int mid = lo + (hi - lo) / 2;
            comparisons++;
            int res = cmp(a[mid], target);

            if (res == 0)
            {
                return mid;
            }
            else if (res < 0)
            {
                lo = mid + 1;
            }
            else
            {
                hi = mid - 1;
            }
        }
        return -1;
    }

    // Завдання 3: Пошук меж (LowerBound / UpperBound)
    public static int FindBound<T>(T[] a, Func<T, bool> condition, out int comparisons)
    {
        comparisons = 0;
        int lo = 0;
        int hi = a.Length;

        while (lo < hi)
        {
            int mid = lo + (hi - lo) / 2;
            comparisons++;
            
            if (condition(a[mid]))
            {
                hi = mid;
            }
            else
            {
                lo = mid + 1;
            }
        }
        return lo;
    }

    // Завдання 4: Потрійний пошук (Варіант c = 2)
    public static int TernarySearch<T>(T[] a, T target, Func<T, T, int> cmp, out int comparisons)
    {
        comparisons = 0;
        int lo = 0;
        int hi = a.Length - 1;

        while (lo <= hi)
        {
            int len = hi - lo;
            int mid1 = lo + len / 3;
            int mid2 = hi - len / 3;

            comparisons++;
            int res1 = cmp(a[mid1], target);
            if (res1 == 0) return mid1;

            comparisons++;
            int res2 = cmp(a[mid2], target);
            if (res2 == 0) return mid2;

            if (res1 > 0)
            {
                hi = mid1 - 1;
            }
            else if (res2 < 0)
            {
                lo = mid2 + 1;
            }
            else
            {
                lo = mid1 + 1;
                hi = mid2 - 1;
            }
        }
        return -1;
    }
}
