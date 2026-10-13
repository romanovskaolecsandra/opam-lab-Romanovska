using System;
using System.Collections;
using System.Collections.Generic;

namespace Lab05
{
    public class IntStack
    {
        private int[] _array;
        private int _count;

        public int Count => _count;
        public int Capacity => _array.Length;

        public IntStack(int initialCapacity)
        {
            _array = new int[initialCapacity];
            _count = 0;
        }

        public void Push(int x)
        {
            if (_count == _array.Length)
            {
                int[] newArray = new int[_array.Length * 2];
                for (int i = 0; i < _array.Length; i++)
                {
                    newArray[i] = _array[i];
                }
                _array = newArray;
            }
            _array[_count] = x;
            _count++;
        }

        public int Pop()
        {
            if (_count == 0)
            {
                throw new InvalidOperationException("Стек порожній.");
            }
            _count--;
            return _array[_count];
        }

        public int Peek()
        {
            if (_count == 0)
            {
                throw new InvalidOperationException("Стек порожній.");
            }
            return _array[_count - 1];
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            Console.WriteLine("=== ЗАВДАННЯ 1 ===");
            RunTask1Tests(false);

            Console.WriteLine("\n=== ЗАВДАННЯ 4 ===");
            DemoCustomStackExpansion();
            RunTask1Tests(true);

            Console.WriteLine("\n=== ЗАВДАННЯ 2 ===");
            RunTask2Tests();

            Console.WriteLine("\n=== ЗАВДАННЯ 3 ===");
            SimulateQueueService();

            Console.ReadLine();
        }

        static (bool isBalanced, int errorIndex) CheckBrackets(string input, bool useCustomStack)
        {
            if (string.IsNullOrEmpty(input))
            {
                return (true, -1);
            }

            Stack<char> stdStack = new Stack<char>();
            IntStack customStack = new IntStack(5);

            for (int i = 0; i < input.Length; i++)
            {
                char ch = input[i];

                if (ch == '(' || ch == '[' || ch == '{' || ch == '<')
                {
                    if (useCustomStack) customStack.Push(ch);
                    else stdStack.Push(ch);
                }
                else if (ch == ')' || ch == ']' || ch == '}' || ch == '>')
                {
                    bool isEmpty = useCustomStack ? (customStack.Count == 0) : (stdStack.Count == 0);
                    
                    if (isEmpty)
                    {
                        return (false, i);
                    }

                    char openCh = useCustomStack ? (char)customStack.Pop() : stdStack.Pop();

                    if ((ch == ')' && openCh != '(') ||
                        (ch == ']' && openCh != '[') ||
                        (ch == '}' && openCh != '{') ||
                        (ch == '>' && openCh != '<'))
                    {
                        return (false, i);
                    }
                }
            }

            bool finalIsEmpty = useCustomStack ? (customStack.Count == 0) : (stdStack.Count == 0);
            if (!finalIsEmpty)
            {
                return (false, input.Length);
            }

            return (true, -1);
        }

        static void RunTask1Tests(bool useCustomStack)
        {
            string[] tests = { "(a[b]{c})", "([)]", ")(", "((", "" };

            foreach (var t in tests)
            {
                var res = CheckBrackets(t, useCustomStack);
                Console.WriteLine($"Вхід: \"{t}\" -> {res.isBalanced}, Індекс помилки: {res.errorIndex}");
            }

            string kOpen = new string('(', 19);
            string kClose = new string(')', 18);
            string customTest = kOpen + kClose;

            var resCustom = CheckBrackets(customTest, useCustomStack);
            Console.WriteLine($"Тест 19 відкривальних, 18 закривальних:");
            Console.WriteLine($"Вхід довжиною {customTest.Length} -> {resCustom.isBalanced}, Індекс помилки: {resCustom.errorIndex}");
        }

        static void EvaluateRPN(string expression)
        {
            Console.WriteLine($"Вираз ОПЗ: \"{expression}\"");
            if (string.IsNullOrWhiteSpace(expression))
            {
                Console.WriteLine("Помилка: порожній рядок.");
                return;
            }

            string[] tokens = expression.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            Stack<double> stack = new Stack<double>();

            foreach (string token in tokens)
            {
                if (double.TryParse(token, out double number))
                {
                    stack.Push(number);
                }
                else if (token == "+" || token == "-" || token == "*" || token == "/" || token == "^")
                {
                    if (stack.Count < 2)
                    {
                        Console.WriteLine("Помилка: бракує чисел.");
                        return;
                    }

                    double b = stack.Pop();
                    double a = stack.Pop();
                    double result = 0;

                    switch (token)
                    {
                        case "+": result = a + b; break;
                        case "-": result = a - b; break;
                        case "*": result = a * b; break;
                        case "/":
                            if (Math.Abs(b) < 1e-9)
                            {
                                Console.WriteLine("Помилка: ділення на нуль.");
                                return;
                            }
                            result = a / b;
                            break;
                        case "^":
                            result = Math.Pow(a, b);
                            break;
                    }
                    stack.Push(result);
                }
                else
                {
                    Console.WriteLine($"Помилка: невідомий символ \"{token}\".");
                    return;
                }
            }

            if (stack.Count != 1)
            {
                Console.WriteLine("Помилка: зайві числа.");
                return;
            }

            Console.WriteLine($"Результат: {stack.Pop()}");
        }

        static void RunTask2Tests()
        {
            EvaluateRPN("3 4 + 2 *");
            EvaluateRPN("5 3 -");
            EvaluateRPN("8 2 /");
            EvaluateRPN("2 3 4 * +");
            EvaluateRPN("5 0 /");
            
            EvaluateRPN("");
            EvaluateROnlyNumber("5");
            EvaluateRPN("5 5 5 +");
            EvaluateRPN("+ 3 4");
            EvaluateRPN("3 4 unknown");

            Console.WriteLine("\nВласний вираз:");
            EvaluateRPN("5 4 * 2 / 9 +");
        }

        static void EvaluateROnlyNumber(string expression)
        {
            string[] tokens = expression.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (tokens.Length == 1 && double.TryParse(tokens[0], out double num))
            {
                Console.WriteLine($"Вираз ОПЗ: \"{expression}\"");
                Console.WriteLine($"Результат: {num}");
            }
            else
            {
                EvaluateRPN(expression);
            }
        }

        struct Ticket
        {
            public int Number;
            public int Weight;
            public bool IsPriority;
        }

        static void SimulateQueueService()
        {
            int totalTickets = 12;
            Random rand = new Random(22);

            Queue<Ticket> normalQueue = new Queue<Ticket>();
            Queue<Ticket> priorityQueue = new Queue<Ticket>();

            for (int i = 1; i <= totalTickets; i++)
            {
                Ticket t = new Ticket
                {
                    Number = i,
                    Weight = rand.Next(1, 10),
                    IsPriority = (i % 3 == 0)
                };

                if (t.IsPriority) priorityQueue.Enqueue(t);
                else normalQueue.Enqueue(t);
            }

            int currentTime = 0;
            int maxQueueLength = 0;

            while (priorityQueue.Count > 0 || normalQueue.Count > 0)
            {
                int currentLength = priorityQueue.Count + normalQueue.Count;
                if (currentLength > maxQueueLength)
                {
                    maxQueueLength = currentLength;
                }

                Ticket activeTicket;
                if (priorityQueue.Count > 0) activeTicket = priorityQueue.Dequeue();
                else activeTicket = normalQueue.Dequeue();

                int startTime = currentTime;
                int duration = activeTicket.Weight;
                currentTime += duration;
                int endTime = currentTime;

                string typeStr = activeTicket.IsPriority ? "Пріоритетна" : "Звичайна";
                Console.WriteLine($"Заявка №{activeTicket.Number} ({typeStr}): Вага = {activeTicket.Weight}. Старт = {startTime}, Кінець = {endTime}");
            }

            Console.WriteLine($"\nМаксимальна довжина черги: {maxQueueLength}");
        }

        static void DemoCustomStackExpansion()
        {
            IntStack stack = new IntStack(5);
            for (int i = 1; i <= 6; i++)
            {
                stack.Push(i * 10);
                Console.WriteLine($"Push {i}: Count = {stack.Count}, Capacity = {stack.Capacity}");
            }
        }
    }
}
