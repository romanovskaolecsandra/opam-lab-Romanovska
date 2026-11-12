using System;
using System.Linq;

namespace Lab09Sorting
{
    internal class Program
    {
        static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Func<int, int, int> intCmp = (x, y) => x.CompareTo(y);

            // 0. Вивід перших 10 елементів для ручного трасування
            var traceSample = Benchmarks.GenerateDatasets(10).RandomArr;
            Console.WriteLine("Перші 10 елементів (Random(22), [0..380]): " + string.Join(", ", traceSample));
            Console.WriteLine();

            // 1. Крайові випадки
            Console.WriteLine("=== КРАЙОВІ ВИПАДКИ ===");
            var edgeCases = new (string Name, int[] Data)[]
            {
                ("порожній масив", Array.Empty<int>()),
                ("один елемент", new[] { 42 }),
                ("два елементи в порядку", new[] { 10, 20 }),
                ("два елементи навпаки", new[] { 20, 10 }),
                ("усі елементи однакові", new[] { 7, 7, 7, 7, 7, 7 }),
                ("лише два різних значення, по пів масиву кожного", new[] { 5, 5, 5, 1, 1, 1 })
            };

            foreach (var (name, data) in edgeCases)
            {
                bool ok1 = CheckSort(data, Sorts.InsertionSort, intCmp);
                bool ok2 = CheckSort(data, Sorts.QuickSortMiddle, intCmp);
                bool ok3 = CheckSort(data, (int[] a, Func<int, int, int> c, out long cmp, out long mv) => Sorts.HybridQuickSort(a, 16, c, out cmp, out mv), intCmp);
                Console.WriteLine($"{name,-50} | Ins: {ok1} | Quick: {ok2} | Hybrid: {ok3}");
            }

            // 2. Завдання 3: Порівняння порогів (1, 4, 16, 41) на n = 100_000
            Console.WriteLine("\n=== ЗАВДАННЯ 3: ПОРОГИ ГІБРИДА (n = 100 000) ===");
            int[] thresholds = { 1, 4, 16, 41 };
            var data100k = Benchmarks.GenerateDatasets(100_000).RandomArr;
            foreach (int t in thresholds)
            {
                double ms = Benchmarks.MeasureMinMs(data100k, (int[] a, Func<int, int, int> c, out long cmp, out long mv) => Sorts.HybridQuickSort(a, t, c, out cmp, out mv), intCmp);
                Console.WriteLine($"Поріг {t,2}: {ms:F3} мс");
            }

            // 3. Завдання 4: Порівняння / Переставляння
            Console.WriteLine("\n=== ЗАВДАННЯ 4: ПОРІВНЯННЯ / ПЕРЕСТАВЛЯННЯ ===");
            int[] sizes4 = { 100, 1_000, 10_000 };
            foreach (int n in sizes4)
            {
                var (rnd, srt, rev, alm) = Benchmarks.GenerateDatasets(n);
                Console.WriteLine($"--- n = {n} ---");
                PrintStats("InsertionSort", rnd, srt, rev, alm, Sorts.InsertionSort, intCmp);
                PrintStats("QuickSortMid ", rnd, srt, rev, alm, Sorts.QuickSortMiddle, intCmp);
                PrintStats("Hybrid(16)   ", rnd, srt, rev, alm, (int[] a, Func<int, int, int> c, out long cmp, out long mv) => Sorts.HybridQuickSort(a, 16, c, out cmp, out mv), intCmp);
            }

            // Окрема перевірка QuickSort на однакових елементах
            int[] allEqual = Enumerable.Repeat(5, 1000).ToArray();
            Sorts.QuickSortMiddle(allEqual, intCmp, out long eqCmp, out long eqMv);
            Console.WriteLine($"QuickSort на 1000 однакових: порівнянь = {eqCmp}, переставлянь = {eqMv}");

            // 4. Завдання 5: Час проти Array.Sort + Межа прийнятності
            Console.WriteLine("\n=== ЗАВДАННЯ 5: ЧАС (мс) ===");
            int[] sizes5 = { 1_000, 10_000, 100_000, 1_000_000 };
            foreach (int n in sizes5)
            {
                var rnd = Benchmarks.GenerateDatasets(n).RandomArr;
                string tIns = n <= 100_000 ? $"{Benchmarks.MeasureMinMs(rnd, Sorts.InsertionSort, intCmp):F3}" : "-";
                double tQuick = Benchmarks.MeasureMinMs(rnd, Sorts.QuickSortMiddle, intCmp);
                double tHyb = Benchmarks.MeasureMinMs(rnd, (int[] a, Func<int, int, int> c, out long cmp, out long mv) => Sorts.HybridQuickSort(a, 16, c, out cmp, out mv), intCmp);
                double tStd = Benchmarks.MeasureMinMs(rnd, (int[] a, Func<int, int, int> c, out long cmp, out long mv) => { cmp = 0; mv = 0; Array.Sort(a, (x, y) => c(x, y)); }, intCmp);

                Console.WriteLine($"n = {n,9} | Ins: {tIns,9} | Quick: {tQuick,8:F3} | Hybrid: {tHyb,8:F3} | Array.Sort: {tStd,8:F3}");
            }

            int limit = Benchmarks.FindAcceptabilityLimit(Sorts.InsertionSort, intCmp);
            Console.WriteLine($"Межа прийнятності InsertionSort (> 1 сек): n = {limit}");

            // 5. Перевірка стабільності
            Console.WriteLine("\n=== ПЕРЕВІРКА СТАБІЛЬНОСТІ ===");
            TestStability();
        }

        static bool CheckSort(int[] original, Benchmarks.SortAction<int> sort, Func<int, int, int> cmp)
        {
            int[] copy = (int[])original.Clone();
            sort(copy, cmp, out _, out _);
            return Verification.IsSorted(copy, cmp) && Verification.IsPermutationOf(copy, original);
        }

        static void PrintStats(string label, int[] rnd, int[] srt, int[] rev, int[] alm, Benchmarks.SortAction<int> sort, Func<int, int, int> cmp)
        {
            string RunOne(int[] src)
            {
                int[] c = (int[])src.Clone();
                sort(c, cmp, out long comparisons, out long moves);
                if (!Verification.IsSorted(c, cmp) || !Verification.IsPermutationOf(c, src))
                    throw new Exception($"Помилка перевірки в {label}!");
                return $"{comparisons} / {moves}";
            }

            Console.WriteLine($"{label}: Випадк = {RunOne(rnd)} | Відсорт = {RunOne(srt)} | Навпаки = {RunOne(rev)} | Майже = {RunOne(alm)}");
        }

        static void TestStability()
        {
            Student[] baseList =
            {
                new("Бондаренко", "КН-31", 85),
                new("Іваненко",   "КН-31", 90),
                new("Коваленко",  "КН-32", 85),
                new("Мельник",    "КН-31", 75),
                new("Петренко",   "КН-32", 90),
                new("Сидоренко",  "КН-31", 85),
                new("Ткаченко",   "КН-32", 75),
                new("Шевченко",   "КН-31", 90)
            };

            Func<Student, Student, int> byScore = (s1, s2) => s1.Score.CompareTo(s2.Score);

            void Check(string name, Action<Student[]> sorter)
            {
                var arr = (Student[])baseList.Clone();
                sorter(arr);
                string counterExample = "немає (стабільний)";
                for (int i = 0; i < arr.Length - 1; i++)
                {
                    if (arr[i].Score == arr[i + 1].Score && string.Compare(arr[i].Name, arr[i + 1].Name, StringComparison.Ordinal) > 0)
                    {
                        counterExample = $"{arr[i].Name} ({arr[i].Score}) перед {arr[i + 1].Name} ({arr[i + 1].Score})";
                        break;
                    }
                }
                Console.WriteLine($"{name,-15} -> Контрприклад: {counterExample}");
            }

            Check("InsertionSort", a => Sorts.InsertionSort(a, byScore, out _, out _));
            Check("QuickSortMid",  a => Sorts.QuickSortMiddle(a, byScore, out _, out _));
            Check("Array.Sort",    a => Array.Sort(a, (x, y) => byScore(x, y)));
            Check("OrderBy",       a => { var res = a.OrderBy(s => s.Score).ToArray(); Array.Copy(res, a, a.Length); });
        }
    }
}
