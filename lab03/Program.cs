using System;
using System.IO;
using System.Collections.Generic;

class Program
{
    static int maxRecDepth = 0;

    static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        Console.Write("Введіть шлях до папки: ");
        string path = Console.ReadLine();

        if (Directory.Exists(path))
        {
            FileInfo largestFile = FindLargestFile(path);
            if (largestFile != null)
            {
                Console.WriteLine($"Найбільший файл: {largestFile.Name}");
                Console.WriteLine($"Розмір: {largestFile.Length} байт");
                Console.WriteLine($"Повний шлях: {largestFile.FullName}");
            }
            else
            {
                Console.WriteLine("Файлів не знайдено або немає доступу.");
            }
        }
        else
        {
            Console.WriteLine("Помилка: вказана папка не існує.");
        }
        Console.WriteLine();

        int[] array = new int[16];
        Random rnd = new Random(22);
        for (int i = 0; i < array.Length; i++)
        {
            array[i] = rnd.Next(-20, 21);
        }

        Console.Write("Масив: ");
        foreach (int x in array) Console.Write(x + " ");
        Console.WriteLine("\n");

        maxRecDepth = 0;
        int countRec = Solve(array, 0, array.Length - 1, 1);
        Console.WriteLine($"Кількість елементів > 9 (рекурсивно): {countRec}");
        Console.WriteLine($"Максимальна глибина рекурсії: {maxRecDepth}");
        Console.WriteLine($"Теоретичне значення log2(16): 4");
        Console.WriteLine();

        int countIter = SolveIterative(array);
        Console.WriteLine($"Кількість елементів > 9 (циклом): {countIter}");
        Console.WriteLine();

        Console.WriteLine("Способи піднятися на 8 сходинок:");
        List<int> currentPath = new List<int>();
        int totalWays = FindStairWays(8, currentPath);
        Console.WriteLine($"Загальна кількість способів: {totalWays}");
        Console.WriteLine();

        Console.WriteLine("--- Трасування стеку (Завдання 4) ---");
        int[] smallArray = { 5, 12, -3, 14 };
        Console.Write("Масив для трасування: ");
        foreach (int x in smallArray) Console.Write(x + " ");
        Console.WriteLine("\n");
        SolveTrace(smallArray, 0, smallArray.Length - 1, 0);
    }

    static FileInfo FindLargestFile(string currentDir)
    {
        FileInfo maxFile = null;

        try
        {
            string[] files = Directory.GetFiles(currentDir);
            foreach (string file in files)
            {
                FileInfo fi = new FileInfo(file);
                if (maxFile == null || fi.Length > maxFile.Length)
                {
                    maxFile = fi;
                }
            }

            string[] subDirs = Directory.GetDirectories(currentDir);
            foreach (string dir in subDirs)
            {
                FileInfo subMax = FindLargestFile(dir);
                if (subMax != null)
                {
                    if (maxFile == null || subMax.Length > maxFile.Length)
                    {
                        maxFile = subMax;
                    }
                }
            }
        }
        catch (UnauthorizedAccessException)
        {
        }

        return maxFile;
    }

    static int Solve(int[] a, int lo, int hi, int currentDepth)
    {
        if (currentDepth > maxRecDepth)
        {
            maxRecDepth = currentDepth;
        }

        if (lo == hi)
        {
            return a[lo] > 9 ? 1 : 0;
        }

        int mid = lo + (hi - lo) / 2;
        int leftResult = Solve(a, lo, mid, currentDepth + 1);
        int rightResult = Solve(a, mid + 1, hi, currentDepth + 1);

        return leftResult + rightResult;
    }

    static int SolveIterative(int[] a)
    {
        int count = 0;
        for (int i = 0; i < a.Length; i++)
        {
            if (a[i] > 9)
            {
                count++;
            }
        }
        return count;
    }

    static int FindStairWays(int n, List<int> path)
    {
        if (n == 0)
        {
            for (int i = 0; i < path.Count; i++)
            {
                Console.Write(path[i]);
                if (i < path.Count - 1) Console.Write("->");
            }
            Console.WriteLine();
            return 1;
        }

        int ways = 0;

        if (n >= 1)
        {
            path.Add(1);
            ways += FindStairWays(n - 1, path);
            path.RemoveAt(path.Count - 1);
        }

        if (n >= 2)
        {
            path.Add(2);
            ways += FindStairWays(n - 2, path);
            path.RemoveAt(path.Count - 1);
        }

        return ways;
    }

    static int SolveTrace(int[] a, int lo, int hi, int depth)
    {
        string indent = new string(' ', depth * 2);
        Console.WriteLine($"{indent}enter Count({lo}..{hi})");

        if (lo == hi)
        {
            int res = a[lo] > 9 ? 1 : 0;
            Console.WriteLine($"{indent}exit Count({lo}..{hi}) -> {res}");
            return res;
        }

        int mid = lo + (hi - lo) / 2;
        int leftRes = SolveTrace(a, lo, mid, depth + 1);
        int rightRes = SolveTrace(a, mid + 1, hi, depth + 1);
        int totalRes = leftRes + rightRes;

        Console.WriteLine($"{indent}exit Count({lo}..{hi}) -> {totalRes}");
        return totalRes;
    }
}
