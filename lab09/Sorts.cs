using System;

namespace Lab09Sorting
{
    public static class Sorts
    {
        // Завдання 1 (a = 1): Сортування вставками
        public static void InsertionSort<T>(T[] a, Func<T, T, int> cmp, out long comparisons, out long moves)
        {
            comparisons = 0;
            moves = 0;
            if (a == null || a.Length <= 1) return;

            for (int i = 1; i < a.Length; i++)
            {
                T key = a[i];
                int j = i - 1;

                while (j >= 0)
                {
                    comparisons++;
                    if (cmp(a[j], key) > 0)
                    {
                        a[j + 1] = a[j];
                        moves++;
                        j--;
                    }
                    else
                    {
                        break;
                    }
                }

                if (j + 1 != i)
                {
                    a[j + 1] = key;
                    moves++;
                }
            }
        }

        // Завдання 2 (b = 2): Швидке сортування (опорний — середній елемент ділянки)
        public static void QuickSortMiddle<T>(T[] a, Func<T, T, int> cmp, out long comparisons, out long moves)
        {
            comparisons = 0;
            moves = 0;
            if (a == null || a.Length <= 1) return;

            QuickSortRecursive(a, 0, a.Length - 1, cmp, ref comparisons, ref moves);
        }

        private static void QuickSortRecursive<T>(T[] a, int left, int right, Func<T, T, int> cmp, ref long comparisons, ref long moves)
        {
            if (left >= right) return;

            int i = left;
            int j = right;
            T pivot = a[left + (right - left) / 2];

            while (i <= j)
            {
                while (true)
                {
                    comparisons++;
                    if (cmp(a[i], pivot) < 0) i++;
                    else break;
                }

                while (true)
                {
                    comparisons++;
                    if (cmp(a[j], pivot) > 0) j--;
                    else break;
                }

                if (i <= j)
                {
                    if (i != j)
                    {
                        T temp = a[i];
                        a[i] = a[j];
                        a[j] = temp;
                        moves += 2;
                    }
                    i++;
                    j--;
                }
            }

            if (left < j) QuickSortRecursive(a, left, j, cmp, ref comparisons, ref moves);
            if (i < right) QuickSortRecursive(a, i, right, cmp, ref comparisons, ref moves);
        }

        // Завдання 3 (c = 2): Гібрид QuickSort + InsertionSort із заданим порогом
        public static void HybridQuickSort<T>(T[] a, int threshold, Func<T, T, int> cmp, out long comparisons, out long moves)
        {
            comparisons = 0;
            moves = 0;
            if (a == null || a.Length <= 1) return;

            HybridRecursive(a, 0, a.Length - 1, threshold, cmp, ref comparisons, ref moves);
        }

        private static void HybridRecursive<T>(T[] a, int left, int right, int threshold, Func<T, T, int> cmp, ref long comparisons, ref long moves)
        {
            if (left >= right) return;

            if (right - left + 1 <= threshold)
            {
                InsertionSortRange(a, left, right, cmp, ref comparisons, ref moves);
                return;
            }

            int i = left;
            int j = right;
            T pivot = a[left + (right - left) / 2];

            while (i <= j)
            {
                while (true)
                {
                    comparisons++;
                    if (cmp(a[i], pivot) < 0) i++;
                    else break;
                }

                while (true)
                {
                    comparisons++;
                    if (cmp(a[j], pivot) > 0) j--;
                    else break;
                }

                if (i <= j)
                {
                    if (i != j)
                    {
                        T temp = a[i];
                        a[i] = a[j];
                        a[j] = temp;
                        moves += 2;
                    }
                    i++;
                    j--;
                }
            }

            if (left < j) HybridRecursive(a, left, j, threshold, cmp, ref comparisons, ref moves);
            if (i < right) HybridRecursive(a, i, right, threshold, cmp, ref comparisons, ref moves);
        }

        private static void InsertionSortRange<T>(T[] a, int left, int right, Func<T, T, int> cmp, ref long comparisons, ref long moves)
        {
            for (int i = left + 1; i <= right; i++)
            {
                T key = a[i];
                int j = i - 1;

                while (j >= left)
                {
                    comparisons++;
                    if (cmp(a[j], key) > 0)
                    {
                        a[j + 1] = a[j];
                        moves++;
                        j--;
                    }
                    else
                    {
                        break;
                    }
                }

                if (j + 1 != i)
                {
                    a[j + 1] = key;
                    moves++;
                }
            }
        }
    }
}
