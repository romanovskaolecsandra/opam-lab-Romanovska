using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Lab06
{
    class Student
    {
        public string Name;
        public string Group;
        public int Score;

        public Student(string name, string group, int score)
        {
            Name = name;
            Group = group;
            Score = score;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            Console.WriteLine("=== ЛАБОРАТОРНА РОБОТА 6 ===");
            Console.WriteLine("Варіант: N=22, K=19, a=1, b=2, c=2\n");

            string text = "Алгоритм описує послідовність дій. Алгоритм можна записати словами, " +
                          "а можна записати кодом. Кодом алгоритм стає тоді, коли його " +
                          "виконує machine, а не людина. Одна задача – багато алгоритмів.";

            Console.WriteLine("--- Завдання 1: Частоти літер (Словник) ---");
            Dictionary<char, int> freqDict = CountLettersDict(text);
            PrintFreqDict(freqDict);

            Console.WriteLine("\n--- Завдання 1: Частоти літер (Списки) ---");
            List<char> keysList = new List<char>();
            List<int> valuesList = new List<int>();
            CountLettersList(text, keysList, valuesList);
            PrintFreqList(keysList, valuesList);

            Console.WriteLine("\n--- Завдання 2: Групування студентів ---");
            List<Student> students = new List<Student>
            {
                new Student("Коваленко", "КН-3/1", 88), new Student("Мельник", "КН-3/2", 64),
                new Student("Іщенко", "КН-3/1", 93),    new Student("Бондар", "КН-3/1", 71),
                new Student("Савченко", "КН-3/2", 55),   new Student("Ткачук", "КН-3/2", 90),
                new Student("Кравець", "КН-3/1", 76),    new Student("Іванів", "КН-3/1", 48),
                new Student("Мороз", "КН-3/2", 82),      new Student("Шевчук", "КН-3/1", 67),
                new Student("Бойко", "КН-3/2", 95),      new Student("Кушнір", "КН-3/2", 73)
            };

            Dictionary<string, List<Student>> groupedStudents = GroupStudentsByScore(students);
            PrintGroupedStudents(groupedStudents);

            Console.WriteLine("\n--- Завдання 3: Вилучення дублікатів ---");
            List<int> numList3 = GenerateRandomNumbers(25, 15, 22);
            Console.Write("Вихідний список: ");
            PrintCollection(numList3);

            HashSet<int> unorderedSet = UniqueUnordered(numList3);
            Console.Write("Без збереження порядку: ");
            PrintCollection(unorderedSet);

            List<int> orderedUnique = UniqueOrdered(numList3);
            Console.Write("З збереженням порядку:  ");
            PrintCollection(orderedUnique);

            Console.WriteLine("\n--- Завдання 4: Порівняння наборів ---");
            List<int> setA = GenerateRandomNumbers(30, 39, 22);
            List<int> setB = GenerateRandomNumbers(30, 39, 122);
            
            Console.Write("Набір А: "); PrintCollection(setA);
            Console.Write("Набір Б: "); PrintCollection(setB);

            HashSet<int> symDiffSet = SymmetricDifferenceHashSet(setA, setB);
            Console.Write("Результат (HashSet): "); PrintCollection(symDiffSet);

            List<int> symDiffList = SymmetricDifferenceLoops(setA, setB);
            Console.Write("Результат (Цикли):   "); PrintCollection(symDiffList);

            bool match = CheckCollectionsEqual(symDiffSet, symDiffList);
            Console.WriteLine($"Результати збігаються: {match}");

            Console.WriteLine("\n--- Завдання 5: Заміри часу ---");
            RunBenchmarks();
        }

        static Dictionary<char, int> CountLettersDict(string text)
        {
            Dictionary<char, int> dict = new Dictionary<char, int>();
            foreach (char c in text)
            {
                if (char.IsLetter(c))
                {
                    char lowerC = char.ToLower(c);
                    if (dict.ContainsKey(lowerC))
                        dict[lowerC]++;
                    else
                        dict[lowerC] = 1;
                }
            }
            return dict;
        }

        static void PrintFreqDict(Dictionary<char, int> dict)
        {
            int uniqueCount = 0;
            foreach (var kvp in dict)
            {
                uniqueCount++;
                if (kvp.Value >= 3)
                {
                    Console.Write($"'{kvp.Key}': {kvp.Value} | ");
                }
            }
            Console.WriteLine($"\nЗагальна кількість різних літер у словнику: {uniqueCount}");
        }
        static void CountLettersList(string text, List<char> keys, List<int> values)
        {
            foreach (char c in text)
            {
                if (char.IsLetter(c))
                {
                    char lowerC = char.ToLower(c);
                    int index = -1;
                    for (int i = 0; i < keys.Count; i++)
                    {
                        if (keys[i] == lowerC)
                        {
                            index = i;
                            break;
                        }
                    }
                    if (index != -1)
                        values[index]++;
                    else
                    {
                        keys.Add(lowerC);
                        values.Add(1);
                    }
                }
            }
        }

        static void PrintFreqList(List<char> keys, List<int> values)
        {
            int uniqueCount = 0;
            for (int i = 0; i < keys.Count; i++)
            {
                uniqueCount++;
                if (values[i] >= 3)
                {
                    Console.Write($"'{keys[i]}': {values[i]} | ");
                }
            }
            Console.WriteLine($"\nЗагальна кількість різних літер у списках: {uniqueCount}");
        }

        static Dictionary<string, List<Student>> GroupStudentsByScore(List<Student> students)
        {
            Dictionary<string, List<Student>> groups = new Dictionary<string, List<Student>>();
            groups["0-59"] = new List<Student>();
            groups["60-74"] = new List<Student>();
            groups["75-89"] = new List<Student>();
            groups["90-100"] = new List<Student>();

            foreach (var s in students)
            {
                string key;
                if (s.Score <= 59) key = "0-59";
                else if (s.Score <= 74) key = "60-74";
                else if (s.Score <= 89) key = "75-89";
                else key = "90-100";

                groups[key].Add(s);
            }
            return groups;
        }

        static void PrintGroupedStudents(Dictionary<string, List<Student>> groups)
        {
            foreach (var kvp in groups)
            {
                string key = kvp.Key;
                List<Student> list = kvp.Value;
                int count = list.Count;
                double avg = 0;

                if (count > 0)
                {
                    int sum = 0;
                    foreach (var s in list) sum += s.Score;
                    avg = (double)sum / count;
                }

                Console.WriteLine($"Діапазон {key,-6} | Кількість: {count,-2} | Середній бал: {avg:F2}");
            }
        }

        static List<int> GenerateRandomNumbers(int count, int maxVal, int seed)
        {
            Random rand = new Random(seed);
            List<int> list = new List<int>();
            for (int i = 0; i < count; i++)
            {
                list.Add(rand.Next(1, maxVal + 1));
            }
            return list;
        }

        static HashSet<int> UniqueUnordered(List<int> source)
        {
            return new HashSet<int>(source);
        }

        static List<int> UniqueOrdered(List<int> source)
        {
            HashSet<int> seen = new HashSet<int>();
            List<int> result = new List<int>();
            foreach (int num in source)
            {
                if (seen.Add(num))
                {
                    result.Add(num);
                }
            }
            return result;
        }

        static HashSet<int> SymmetricDifferenceHashSet(List<int> a, List<int> b)
        {
            HashSet<int> setA = new HashSet<int>(a);
            HashSet<int> setB = new HashSet<int>(b);
            HashSet<int> result = new HashSet<int>(setA);
            result.SymmetricExceptWith(setB);
            return result;
        }

        static List<int> SymmetricDifferenceLoops(List<int> a, List<int> b)
        {
            List<int> result = new List<int>();

            foreach (int itemA in a)
            {
                bool foundInB = false;
                foreach (int itemB in b)
                {
                    if (itemA == itemB) { foundInB = true; break; }
                }
                if (!foundInB)
                {
                    bool alreadyInRes = false;
                    foreach (int r in result) { if (r == itemA) { alreadyInRes = true; break; } }
                    if (!alreadyInRes) result.Add(itemA);
                }
            }

            foreach (int itemB in b)
            {
                bool foundInA = false;
                foreach (int itemA in a)
                {
                    if (itemB == itemA) { foundInA = true; break; }
                }
                if (!foundInA)
                {
                    bool alreadyInRes = false;
                    foreach (int r in result) { if (r == itemB) { alreadyInRes = true; break; } }
                    if (!alreadyInRes) result.Add(itemB);
                }
            }
            return result;
        }

        static bool CheckCollectionsEqual(HashSet<int> set, List<int> list)
        {
            if (set.Count != list.Count) return false;
            foreach (int item in list)
            {
                if (!set.Contains(item)) return false;
            }
            return true;
        }

        static void RunBenchmarks()
        {
            Stopwatch sw = new Stopwatch();
            Random r = new Random(42);

            List<int> bigList = new List<int>();
            HashSet<int> bigSet = new HashSet<int>();
            for (int i = 0; i < 100000; i++)
            {
                int val = r.Next();
                bigList.Add(val);
                bigSet.Add(val);
            }

            sw.Restart();
            for (int i = 0; i < 10000; i++) { bigList.Contains(i); }
            sw.Stop();
            double t1_list = sw.Elapsed.TotalMilliseconds;

            sw.Restart();
            for (int i = 0; i < 10000; i++) { bigSet.Contains(i); }
            sw.Stop();
            double t1_set = sw.Elapsed.TotalMilliseconds;

            Console.WriteLine($"1. Пошук елементів | Списковий: {t1_list:F2} мс | Множина: {t1_set:F2} мс | Виграш у {t1_list / Math.Max(t1_set, 0.001):F1} разів");

            int wordCount = 5000; 
            List<string> words = new List<string>();
            for (int i = 0; i < wordCount; i++) words.Add("word" + r.Next(1, 1000));

            sw.Restart();
            Dictionary<string, int> dTest = new Dictionary<string, int>();
            foreach (var w in words)
            {
                if (dTest.ContainsKey(w)) dTest[w]++;
                else dTest[w] = 1;
            }
            sw.Stop();
            double t2_dict = sw.Elapsed.TotalMilliseconds;

            sw.Restart();
            List<string> kList = new List<string>();
            List<int> vList = new List<int>();
            foreach (var w in words)
            {
                int idx = -1;
                for (int i = 0; i < kList.Count; i++) { if (kList[i] == w) { idx = i; break; } }
                if (idx != -1) vList[idx]++;
                else { kList.Add(w); vList.Add(1); }
            }
            sw.Stop();
            double t2_list = sw.Elapsed.TotalMilliseconds;

            Console.WriteLine($"2. Підрахунок частот | Два списки: {t2_list:F2} мс | Словник: {t2_dict:F2} мс | Виграш у {t2_list / Math.Max(t2_dict, 0.001):F1} разів");
        }

        static void PrintCollection<T>(IEnumerable<T> collection)
        {
            foreach (var item in collection) Console.Write(item + " ");
            Console.WriteLine();
        }
    }
}
