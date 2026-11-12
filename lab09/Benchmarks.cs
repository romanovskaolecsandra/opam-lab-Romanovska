using System;
using System.Diagnostics;

namespace Lab09Sorting
{
    public record Student(string Name, string Group, int Score);

    public static class Benchmarks
    {
        public delegate void SortAction<T>(T[] a, Func<T, T, int> cmp, out long comparisons, out long moves);

        // Генерація 4 типів входу за параметрами варіанта (N = 22, K = 19, діапазон [0; 380])
        public static (int[] RandomArr, int[] SortedArr, int[] ReversedArr, int[] AlmostSortedArr) GenerateDatasets(int size, int seed = 22, int kSwaps = 19)
        {
            var rnd = new Random(seed);
            int[] randomArr = new int[size];
            for (int i = 0; i < size; i++)
            {
                randomArr[i] = rnd.Next(0, 381); // [0; 380] включно
            }

            int[] sortedArr = (int[])randomArr.Clone();
            Array.Sort(sortedArr); // Дозволено для підготовки вхідних даних

            int[] reversedArr = (int[])sortedArr.Clone();
            Array.Reverse(reversedArr);

            int[] almostSortedArr = (int[])sortedArr.Clone();
            if (size > 1)
            {
                for (int s = 0; s < kSwaps; s++)
                {
                    int idx1 = rnd.Next(0, size);
                    int idx2 = rnd.Next(0, size);
                    (almostSortedArr[idx1], almostSortedArr[idx2]) = (almostSortedArr[idx2], almostSortedArr[idx1]);
                }
            }

            return (randomArr, sortedArr, reversedArr, almostSortedArr);
        }

        // Вимірювання часу: прогрівання + 3 заміри -> мінімум
        public static double MeasureMinMs<T>(T[] source, SortAction<T> sortMethod, Func<T, T, int> cmp)
        {
            // Прогрівочний запуск
            T[] warm = (T[])source.Clone();
            sortMethod(warm, cmp, out _, out _);

            double minMs = double.MaxValue;
            var sw = new Stopwatch();

            for (int run = 0; run < 3; run++)
            {
                T[] copy = (T[])source.Clone();
                sw.Restart();
                sortMethod(copy, cmp, out _, out _);
                sw.Stop();

                double elapsed = sw.Elapsed.TotalMilliseconds;
                if (elapsed < minMs) minMs = elapsed;
            }

            return minMs;
        }

        // Пошук межі прийнятності (> 1000 мс) для простого сортування
        public static int FindAcceptabilityLimit(SortAction<int> simpleSort, Func<int, int, int> cmp)
        {
            int n = 10_000;
            while (true)
            {
                var data = GenerateDatasets(n).RandomArr;
                double ms = MeasureMinMs(data, simpleSort, cmp);
                if (ms > 1000.0)
                    return n;
                n += 5_000;
            }
        }
    }
}
