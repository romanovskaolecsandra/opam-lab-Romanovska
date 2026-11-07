using System;
using System.Diagnostics;

class Program
{
    static void Main()
    {
        int N = 22;
        int K = 19;
        string X = "О-00";
        string Y = "О-99";
        string missing = "Я-99";

        // Генерація основного масиву
        string[] originalArray = GenerateArray(100000, N);
        
        // Робимо відсортовану копію
        string[] sortedArray = (string[])originalArray.Clone();
        Array.Sort(sortedArray, string.Compare);

        Console.WriteLine($"--- Завдання 1: Лінійний пошук ---");
        int comp1, comp2, comp3;
        int idxX = Search.LinearSearch(sortedArray, X, string.Compare, out comp1);
        int idxFirst = Search.LinearSearch(sortedArray, sortedArray[0], string.Compare, out comp2);
        int idxMissing = Search.LinearSearch(sortedArray, missing, string.Compare, out comp3);
        Console.WriteLine($"Запит X (\"{X}\"): індекс = {idxX}, порівнянь = {comp1}");
        Console.WriteLine($"Перший елемент: індекс = 0, порівнянь = {comp2}");
        Console.WriteLine($"Значення, якого немає (\"{missing}\"): індекс = {idxMissing}, порівнянь = {comp3}\n");

        Console.WriteLine($"--- Завдання 2: Правильний бінарний пошук ---");
        Search.BinarySearch(sortedArray, X, string.Compare, out comp1);
        Search.BinarySearch(sortedArray, sortedArray[0], string.Compare, out comp2);
        Search.BinarySearch(sortedArray, missing, string.Compare, out comp3);
        Console.WriteLine($"Запит X (\"{X}\"): порівнянь = {comp1}");
        Console.WriteLine($"Перший елемент: порівнянь = {comp2}");
        Console.WriteLine($"Значення, якого немає (\"{missing}\"): порівнянь = {comp3}\n");

        Console.WriteLine($"--- Завдання 2: Тестування поломок ---");
        int compB1;
        string lastElement = sortedArray[sortedArray.Length - 1];
        int idxB1 = Broken.BinarySearch_Broken1(sortedArray, lastElement, string.Compare, out compB1);
        Console.WriteLine($"Поломка 1 (Пошук останнього елемента): повернуто індекс = {idxB1}");

        int compB2;
        Console.WriteLine("Запуск поломки 2 для елемента, якого немає:");
        Broken.BinarySearch_Broken2(sortedArray, missing, string.Compare, out compB2);

        int failedSearches = 0;
        for (int i = 0; i < 100; i++)
        {
            string target = sortedArray[new Random(i).Next(100000)];
            int dummyComp;
            if (Search.BinarySearch(originalArray, target, string.Compare, out dummyComp) == -1)
            {
                failedSearches++;
            }
        }
        Console.WriteLine($"Поломка 4: З 100 наявних елементів на невідсортованому масиві не знайдено = {failedSearches}\n");

        Console.WriteLine($"--- Завдання 3: Пошук меж ---");
        int compLB, compUB;
        int lowerBoundX = Search.FindBound(sortedArray, item => string.Compare(item, X) >= 0, out compLB);
        int upperBoundX = Search.FindBound(sortedArray, item => string.Compare(item, X) > 0, out compUB);
        int countX = upperBoundX - lowerBoundX;
        Console.WriteLine($"Нижня межа для {X}: індекс = {lowerBoundX}, порівнянь = {compLB}");
        Console.WriteLine($"Верхня межа для {X}: індекс = {upperBoundX}, порівнянь = {compUB}");
        Console.WriteLine($"Кількість елементів, рівних {X} (без повного проходу) = {countX}\n");

        int compCriteria;
        int idxCriteria = Search.FindBound(sortedArray, item => item.Contains("7"), out compCriteria);
        Console.WriteLine($"Пошук коду з цифрою '7' на впорядкованому за алфавітом масиві: індекс = {idxCriteria}\n");

        Console.WriteLine($"--- Завдання 4: Потрійний пошук (Варіант c=2) ---");
        int compT1, compT2, compT3;
        Search.TernarySearch(sortedArray, X, string.Compare, out compT1);
        Search.TernarySearch(sortedArray, sortedArray[0], string.Compare, out compT2);
        Search.TernarySearch(sortedArray, missing, string.Compare, out compT3);
        Console.WriteLine($"Потрійний пошук (Запит X): порівнянь = {compT1}");
        Console.WriteLine($"Потрійний пошук (Перший елемент): порівнянь = {compT2}");
        Console.WriteLine($"Потрійний пошук (Значення якого немає): порівнянь = {compT3}\n");

        Console.WriteLine($"--- Завдання 5: Експерименти та заміри часу ---");
        RunTimeBenchmarks(sortedArray);
    }

    static string[] GenerateArray(int size, int seed)
    {
        string[] arr = new string[size];
        Random rand = new Random(seed);
        string alphabet = "АБВГДЕЄЖЗИІЇЙКЛМНОПРСТУФХЦЧШЩЬЮЯ";
        for (int i = 0; i < size; i++)
        {
            char letter = alphabet[rand.Next(alphabet.Length)];
            int number = rand.Next(0, 100);
            arr[i] = $"{letter}-{number:D2}";
        }
        return arr;
    }

    static void RunTimeBenchmarks(string[] sortedArray)
    {
        int[] sizes = { 1000, 10000, 100000 };
        Random rand = new Random(42);

        foreach (int size in sizes)
        {
            string[] subArray = new string[size];
            Array.Copy(sortedArray, subArray, size);

            string[] queries = new string[100];
            for (int i = 0; i < 100; i++) queries[i] = subArray[rand.Next(size)];

            long totalLinearComp = 0;
            long totalBinaryComp = 0;

            for (int i = 0; i < 100; i++)
            {
                int c;
                Search.LinearSearch(subArray, queries[i], string.Compare, out c);
                totalLinearComp += c;

                Search.BinarySearch(subArray, queries[i], string.Compare, out c);
                totalBinaryComp += c;
            }

            Console.WriteLine($"Розмір {size}: Лінійний (сер. порівнянь) = {totalLinearComp / 100.0}, Бінарний (сер. порівнянь) = {totalBinaryComp / 100.0}");
        }

        Console.WriteLine("\nЗаміри для визначення точки окупності (масив 100 000):");
        int[] queryCounts = { 1, 5, 10, 20, 30, 50, 100 };
        
        foreach (int q in queryCounts)
        {
            string[] testQueries = new string[q];
            for (int i = 0; i < q; i++) testQueries[i] = sortedArray[rand.Next(100000)];

            string[] un有序 = GenerateArray(100000, 22);

            Stopwatch sw = Stopwatch.StartNew();
            for (int i = 0; i < 3; i++) 
            {
                foreach (var qry in testQueries) Search.LinearSearch(un有序, qry, string.Compare, out _);
            }
            sw.Restart();
            foreach (var qry in testQueries) Search.LinearSearch(un有序, qry, string.Compare, out _);
            long timeLinear = sw.ElapsedTicks;

            sw.Restart();
            for (int i = 0; i < 3; i++) 
            {
                string[] temp = (string[])un有序.Clone();
                Array.Sort(temp, string.Compare);
                foreach (var qry in testQueries) Search.BinarySearch(temp, qry, string.Compare, out _);
            }
            sw.Restart();
            string[] arrayToSort = (string[])un有序.Clone();
            Array.Sort(arrayToSort, string.Compare);
            foreach (var qry in testQueries) Search.BinarySearch(arrayToSort, qry, string.Compare, out _);
            long timeSortAndBinary = sw.ElapsedTicks;

            Console.WriteLine($"Запитів q = {q}: Лінійний = {timeLinear} ticks, Сортування+Бінарний = {timeSortAndBinary} ticks");
        }
    }
}
