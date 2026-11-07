using System;

public static class Broken
{
    // Поломка 1: while (lo < hi) замість <=
    public static int BinarySearch_Broken1<T>(T[] a, T target, Func<T, T, int> cmp, out int comparisons)
    {
        comparisons = 0;
        int lo = 0;
        int hi = a.Length - 1;

        while (lo < hi) 
        {
            int mid = lo + (hi - lo) / 2;
            comparisons++;
            int res = cmp(a[mid], target);

            if (res == 0) return mid;
            if (res < 0) lo = mid + 1;
            else hi = mid - 1;
        }
        return -1;
    }

    // Поломка 2: lo = mid замість mid + 1
    public static int BinarySearch_Broken2<T>(T[] a, T target, Func<T, T, int> cmp, out int comparisons)
    {
        comparisons = 0;
        int lo = 0;
        int hi = a.Length - 1;
        int iterations = 0;

        while (lo <= hi)
        {
            iterations++;
            if (iterations > 1000) 
            {
                Console.WriteLine($"[ЗАВИСАННЯ]: Цикл зациклився при lo={lo}, hi={hi}, mid={lo + (hi - lo) / 2}");
                return -2; 
            }

            int mid = lo + (hi - lo) / 2;
            comparisons++;
            int res = cmp(a[mid], target);

            if (res == 0) return mid;
            if (res < 0) lo = mid; 
            else hi = mid - 1;
        }
        return -1;
    }

    // Поломка 3: mid = (lo + hi) / 2
    public static int BinarySearch_Broken3<T>(T[] a, T target, Func<T, T, int> cmp, out int comparisons)
    {
        comparisons = 0;
        int lo = 0;
        int hi = a.Length - 1;

        while (lo <= hi)
        {
            int mid = (lo + hi) / 2; 
            comparisons++;
            int res = cmp(a[mid], target);

            if (res == 0) return mid;
            if (res < 0) lo = mid + 1;
            else hi = mid - 1;
        }
        return -1;
    }
}
