using System;
using System.Collections.Generic;
using System.Diagnostics;

class Program
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        const int N = 22;
        const int K = 19;

        Console.WriteLine($"Варіант: N = {N}, K = {K}, a = 1, b = 2, c = 2\n");

        Random rndMain = new Random(N);
        List<int> mainList = new List<int>(20);
        for (int i = 0; i < 20; i++)
        {
            mainList.Add(rndMain.Next(-9, 10));
        }

        Console.Write("Основний список: ");
        PrintList(mainList);
        Console.WriteLine();

        Console.WriteLine("--- Завдання 1 ---");
        List<int> task1Result = FilterMultiplesOfFive(mainList);
        Console.Write("Результат (кратні 5): ");
        PrintList(task1Result);
        Console.WriteLine($"Кількість елементів (Count): {task1Result.Count}");
        Console.WriteLine($"Ємність (Capacity): {task1Result.Capacity}");
        Console.WriteLine();

        Console.WriteLine("--- Завдання 2 ---");
        List<int> badAttemptList = new List<int>(mainList);
        List<int> skippedElements = RemoveNegativesIncorrectly(badAttemptList);
        Console.Write("1. Хибний варіант (результат): ");
        PrintList(badAttemptList);
        Console.Write("Пропущені елементи: ");
        PrintList(skippedElements);

        List<int> listAttempt2 = new List<int>(mainList);
        RemoveNegativesFromEnd(listAttempt2);
        Console.Write("2. Цикл з кінця: ");
        PrintList(listAttempt2);

        List<int> listAttempt3 = RemoveNegativesToNewList(mainList);
        Console.Write("3. Новий список: ");
        PrintList(listAttempt3);

        bool task2Equals = AreListsEqual(listAttempt2, listAttempt3);
        Console.WriteLine($"Варіанти 2 і 3 однакові: {task2Equals.ToString().ToLower()}");
        Console.WriteLine();

        Console.WriteLine("--- Завдання 3 ---");
        List<int> sorted1 = GenerateSortedList(14, N);
        List<int> sorted2 = GenerateSortedList(9, N + 100);

        Console.Write("Перший упорядкований (14 ел.): ");
        PrintList(sorted1);
        Console.Write("Другий упорядкований (9 ел.):  ");
        PrintList(sorted2);

        List<int> mergedList = MergeSortedLists(sorted1, sorted2);
        Console.Write("Результат злиття:              ");
        PrintList(mergedList);

        bool isMergedSorted = IsListSorted(mergedList);
        Console.WriteLine($"Перевірка на впорядкованість: {isMergedSorted.ToString().ToLower()}");
        Console.WriteLine();

        Console.WriteLine("--- Завдання 4 ---");
        SplitListByThreshold(mainList, 5, out List<int> lessThan5, out List<int> restList);
        
        Console.Write("Менші за 5: ");
        PrintList(lessThan5);
        Console.Write("Решта:      ");
        PrintList(restList);

        bool checkCounts = (lessThan5.Count + restList.Count) == mainList.Count;
        Console.WriteLine($"Сума Count обох списків дорівнює вихідному: {checkCounts.ToString().ToLower()}");
        Console.WriteLine();

        Console.WriteLine("--- Завдання 5 (Замір часу) ---");
        RunTimeMeasurements();
    }

    static void PrintList(List<int> list)
    {
        Console.Write("[");
        for (int i = 0; i < list.Count; i++)
        {
            Console.Write(list[i]);
            if (i < list.Count - 1) Console.Write(", ");
        }
        Console.WriteLine("]");
    }

    static bool AreListsEqual(List<int> l1, List<int> l2)
    {
        if (l1.Count != l2.Count) return false;
        for (int i = 0; i < l1.Count; i++)
        {
            if (l1[i] != l2[i]) return false;
        }
        return true;
    }

    static List<int> FilterMultiplesOfFive(List<int> source)
    {
        int count = 0;
        for (int i = 0; i < source.Count; i++)
        {
            if (source[i] % 5 == 0) count++;
        }

        List<int> result = new List<int>(count);
        for (int i = 0; i < source.Count; i++)
        {
            if (source[i] % 5 == 0)
            {
                result.Add(source[i]);
            }
        }
        return result;
    }

    static List<int> RemoveNegativesIncorrectly(List<int> list)
    {
        List<int> skipped = new List<int>();
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i] < 0)
            {
                if (i + 1 < list.Count && list[i + 1] < 0)
                {
                    skipped.Add(list[i + 1]);
                }
                list.RemoveAt(i);
            }
        }
        return skipped;
    }

    static void RemoveNegativesFromEnd(List<int> list)
    {
        for (int i = list.Count - 1; i >= 0; i--)
        {
            if (list[i] < 0)
            {
                list.RemoveAt(i);
            }
        }
    }

    static List<int> RemoveNegativesToNewList(List<int> source)
    {
        int count = 0;
        for (int i = 0; i < source.Count; i++)
        {
            if (source[i] >= 0) count++;
        }

        List<int> result = new List<int>(count);
        for (int i = 0; i < source.Count; i++)
        {
            if (source[i] >= 0)
            {
                result.Add(source[i]);
            }
        }
        return result;
    }

    static List<int> GenerateSortedList(int length, int seed)
    {
        Random rnd = new Random(seed);
        List<int> list = new List<int>(length);
        
        if (length == 0) return list;

        int current = rnd.Next(1, 10);
        list.Add(current);

        for (int i = 1; i < length; i++)
        {
            current += rnd.Next(1, 10);
            list.Add(current);
        }
        return list;
    }

    static List<int> MergeSortedLists(List<int> list1, List<int> list2)
    {
        List<int> merged = new List<int>(list1.Count + list2.Count);
        int i = 0, j = 0;

        while (i < list1.Count && j < list2.Count)
        {
            if (list1[i] <= list2[j])
            {
                merged.Add(list1[i]);
                i++;
            }
            else
            {
                merged.Add(list2[j]);
                j++;
            }
        }

        while (i < list1.Count)
        {
            merged.Add(list1[i]);
            i++;
        }

        while (j < list2.Count)
        {
            merged.Add(list2[j]);
            j++;
        }

        return merged;
    }

    static bool IsListSorted(List<int> list)
    {
        for (int i = 1; i < list.Count; i++)
        {
            if (list[i] < list[i - 1]) return false;
        }
        return true;
    }

    static void SplitListByThreshold(List<int> source, int threshold, out List<int> less, out List<int> rest)
    {
        int countLess = 0;
        for (int i = 0; i < source.Count; i++)
        {
            if (source[i] < threshold) countLess++;
        }

        less = new List<int>(countLess);
        rest = new List<int>(source.Count - countLess);

        for (int i = 0; i < source.Count; i++)
        {
            if (source[i] < threshold)
                less.Add(source[i]);
            else
                rest.Add(source[i]);
        }
    }

    static void RunTimeMeasurements()
    {
        const int WarmupSize = 1000;
        const int Size = 100000;
        const int ContainsIterations = 1000;

        List<int> warmupList = new List<int>();
        for (int i = 0; i < WarmupSize; i++) warmupList.Add(i);
        warmupList.Insert(0, -1);
        bool _ = warmupList.Contains(-999);

        Stopwatch sw = new Stopwatch();

        sw.Restart();
        List<int> list1 = new List<int>();
        for (int i = 0; i < Size; i++) list1.Add(i);
        sw.Stop();
        double timeAddUnset = sw.Elapsed.TotalMilliseconds;
        Console.WriteLine($"1. Add(x) без місткості: {timeAddUnset:F2} мс");

        sw.Restart();
        List<int> list2 = new List<int>(Size);
        for (int i = 0; i < Size; i++) list2.Add(i);
        sw.Stop();
        double timeAddSet = sw.Elapsed.TotalMilliseconds;
        Console.WriteLine($"2. Add(x) з заданою місткістю: {timeAddSet:F2} мс");

        sw.Restart();
        List<int> list3 = new List<int>();
        for (int i = 0; i < Size; i++) list3.Insert(0, i);
        sw.Stop();
        double timeInsertZero = sw.Elapsed.TotalMilliseconds;
        Console.WriteLine($"3. Insert(0, x): {timeInsertZero:F2} мс");

        sw.Restart();
        for (int i = 0; i < ContainsIterations; i++)
        {
            bool __ = list1.Contains(-1);
        }
        sw.Stop();
        double timeContains = sw.Elapsed.TotalMilliseconds;
        Console.WriteLine($"4. 1000 * Contains: {timeContains:F2} мс");

        double ratioInsertVsAdd = timeInsertZero / timeAddSet;
        Console.WriteLine($"\nInsert(0,x) повільніший за Add(x) у {ratioInsertVsAdd:F1} разів.");
    }
}
