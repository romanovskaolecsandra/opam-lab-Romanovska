using System;
using System.Collections.Generic;

namespace Lab07
{
    public class Student
    {
        public string Name { get; set; }
        public string Group { get; set; }
        public int Score { get; set; }

        public override string ToString()
        {
            return $"{Name} ({Group}) - Бал: {Score}";
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            // ГЕНЕРАЦІЯ ДАНИХ (Твій новий варіант: N=7, K=19)
            Random rand = new Random(7);
            List<int> numbers = new List<int>();
            for (int i = 0; i < 20; i++)
            {
                numbers.Add(rand.Next(-19, 20));
            }

            List<string> words = new List<string>
            {
                "яблуко", "ананас", "апельсин", "кіт", "дерево", 
                "онлайн-курс", "ехо", "абзац", "ідея", "сік", 
                "міні-маркет", "комп'ютер", "олівець", "екран"
            };

            List<Student> students = new List<Student>
            {
                new Student { Name = "Шевченко", Group = "КН-3/1", Score = 85 },
                new Student { Name = "Коваленко", Group = "КН-3/2", Score = 95 },
                new Student { Name = "Бондаренко", Group = "КН-3/1", Score = 58 },
                new Student { Name = "Ткаченко", Group = "КН-3/1", Score = 74 },
                new Student { Name = "Лисенко", Group = "КН-3/3", Score = 64 }
            };

            // =================================================================
            // ЗАВДАННЯ 1: ВІДБІР (a=1)
            // =================================================================
            Console.WriteLine("=== ЗАВДАННЯ 1: ВІДБІР ===");
            var filteredNumbers = Algorithms.Filter(numbers, x => x % 3 == 0);
            Console.WriteLine($"Числа кратні 3: {string.Join(", ", filteredNumbers)}");

            string vowels = "аеєиіїоуюяАЕЄИІЇОУЮЯ";
            var filteredWords = Algorithms.Filter(words, w => w.Length > 0 && vowels.Contains(w[0].ToString()));
            Console.WriteLine($"Рядки з голосної: {string.Join(", ", filteredWords)}");

            var filteredStudents = Algorithms.Filter(students, s => s.Group == "КН-3/1");
            Console.WriteLine($"Студенти КН-3/1:\n  {string.Join("\n  ", filteredStudents)}");

            // =================================================================
            // ЗАВДАННЯ 2: ПЕРЕТВОРЕННЯ (b=3 - Квадрат числа!)
            // =================================================================
            Console.WriteLine("\n=== ЗАВДАННЯ 2: ПЕРЕТВОРЕННЯ ===");
            var mappedNumbers = Algorithms.Map(numbers, x => x * x); // Піднесення до квадрату за варіантом b=3
            Console.WriteLine($"Числа -> квадрати: {string.Join(", ", mappedNumbers)}");

            var mappedWords = Algorithms.Map(words, w => w.Length > 0 ? w[0].ToString() : "");
            Console.WriteLine($"Рядки -> перші літери: {string.Join(", ", mappedWords)}");

            var mappedStudents = Algorithms.Map(students, s => s.Score);
            Console.WriteLine($"Студенти -> бали: {string.Join(", ", mappedStudents)}");

            // =================================================================
            // ЗАВДАННЯ 3: АГРЕГАЦІЯ (c=2)
            // =================================================================
            Console.WriteLine("\n=== ЗАВДАННЯ 3: АГРЕГАЦІЯ ===");
            int sum = Algorithms.Aggregate(numbers, 0, (acc, x) => acc + x);
            long product = Algorithms.Aggregate(numbers, 1L, (acc, x) => acc * x);
            int max = Algorithms.Aggregate(numbers, int.MinValue, (acc, x) => x > acc ? x : acc);
            string csvWords = Algorithms.Aggregate(words, "", (acc, w) => acc == "" ? w : acc + ", " + w);

            Console.WriteLine($"1. Сума чисел: {sum}");
            Console.WriteLine($"2. Добуток чисел: {product}");
            Console.WriteLine($"3. Максимум чисел: {max}");
            Console.WriteLine($"4. Конкатенація рядків: {csvWords}");

            // =================================================================
            // ЗАВДАННЯ 4: ГРУПУВАННЯ
            // =================================================================
            Console.WriteLine("\n=== ЗАВДАННЯ 4: ГРУПУВАННЯ ===");
            var grpByGroup = Algorithms.GroupBy(students, s => s.Group);
            Console.WriteLine("Студенти за групами:");
            foreach (var p in grpByGroup) Console.WriteLine($"  Група {p.Key}: {p.Value.Count} чол.");

            var grpWordsLen = Algorithms.GroupBy(words, w => w.Length);
            Console.WriteLine("Рядки за довжиною:");
            foreach (var p in grpWordsLen) Console.WriteLine($"  Довжина {p.Key}: {p.Value.Count} слів");

            // =================================================================
            // ЗАВДАННЯ 5: ЗАХОПЛЕННЯ ЗМІННИХ
            // =================================================================
            Console.WriteLine("\n=== ЗАВДАННЯ 5: ЗАХОПЛЕННЯ ЗМІННИХ ===");
            int threshold = 19;
            Func<int, bool> checkThreshold = x => x > threshold;
            threshold = 1000;
            Console.WriteLine($"Ситуація 1 (50 > threshold?): {checkThreshold(50)}");

            List<Func<int>> forLambdas = new List<Func<int>>();
            for (int i = 0; i < 3; i++)
            {
                forLambdas.Add(() => i);
            }
            Console.Write("Ситуація 2 (цикл for): ");
            foreach (var f in forLambdas) Console.Write(f() + " ");
            Console.WriteLine();

            List<Func<int>> foreachLambdas = new List<Func<int>>();
            foreach (var v in new int[] { 0, 1, 2 })
            {
                foreachLambdas.Add(() => v);
            }
            Console.Write("Ситуація 3 (цикл foreach): ");
            foreach (var f in foreachLambdas) Console.Write(f() + " ");
            Console.WriteLine();
        }
    }
}
