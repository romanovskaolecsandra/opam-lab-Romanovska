using System;

class Program
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        Random r = new Random(22);

        int[] a = new int[24];
        for (int i = 0; i < 24; i++)
        {
            a[i] = r.Next(1, 5);
        }

        Console.Write("Початковий масив: ");
        for (int i = 0; i < a.Length; i++) Console.Write(a[i] + " ");
        Console.WriteLine("\n");
        Task1(a);
        Console.WriteLine();

        Console.WriteLine("--- Завдання 2 ---");
        int[] filtered = Task2(a);
        Console.Write("Масив після фільтрації (кратні 5): ");
        if (filtered.Length == 0)
        {
            Console.WriteLine("Порожній масив (немає чисел кратних 5)");
        }
        else
        {
            for (int i = 0; i < filtered.Length; i++) Console.Write(filtered[i] + " ");
            Console.WriteLine();
        }
        Console.WriteLine();

        Task3(a);
        Console.WriteLine();

        Console.WriteLine("--- Завдання 4 ---");
        Console.Write("Масив до зсуву:          ");
        for (int i = 0; i < a.Length; i++) Console.Write(a[i] + " ");
        Console.WriteLine();
        
        Task4(a);
        
        Console.Write("Масив після зсуву вправо: ");
        for (int i = 0; i < a.Length; i++) Console.Write(a[i] + " ");
        Console.WriteLine("\n");

        int[,] m = new int[3, 6];
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 6; j++)
            {
                m[i, j] = r.Next(1, 5);
            }
        }
        Task5(m);
    }

    static void Task1(int[] a)
    {
        if (a.Length == 0)
        {
            Console.WriteLine("Масив порожній!");
            return;
        }

        int sum = 0;
        int min = a[0];
        int max = a[0];
        int minIdx = 0;
        int maxIdx = 0;
        int zeros = 0;

        for (int i = 0; i < a.Length; i++)
        {
            sum += a[i];

            if (a[i] < min)
            {
                min = a[i];
                minIdx = i;
            }

            if (a[i] > max)
            {
                max = a[i];
                maxIdx = i;
            }

            if (a[i] == 0)
            {
                zeros++;
            }
        }

        double avg = (double)sum / a.Length;

        Console.WriteLine("--- Завдання 1 ---");
        Console.WriteLine("Сума: " + sum);
        Console.WriteLine("Середнє: " + avg);
        Console.WriteLine("Мінімум: " + min + " (індекс " + minIdx + ")");
        Console.WriteLine("Максимум: " + max + " (індекс " + maxIdx + ")");
        Console.WriteLine("Нулів: " + zeros);
    }

    static int[] Task2(int[] a)
    {
        if (a.Length == 0) return new int[0];

        int count = 0;
        for (int i = 0; i < a.Length; i++)
        {
            if (a[i] % 5 == 0)
            {
                count++;
            }
        }

        int[] res = new int[count];
        int idx = 0;

        for (int i = 0; i < a.Length; i++)
        {
            if (a[i] % 5 == 0)
            {
                res[idx] = a[i];
                idx++;
            }
        }

        return res;
    }

    static void Task3(int[] a)
    {
        if (a.Length == 0)
        {
            Console.WriteLine("Масив порожній!");
            return;
        }

        int maxVal = a[0];
        int maxLen = 1;
        int maxStart = 0;

        int curVal = a[0];
        int curLen = 1;
        int curStart = 0;

        for (int i = 1; i <= a.Length; i++)
        {
            if (i < a.Length && a[i] == curVal)
            {
                curLen++;
            }
            else
            {
                if (curLen > maxLen)
                {
                    maxLen = curLen;
                    maxVal = curVal;
                    maxStart = curStart;
                }

                if (i < a.Length)
                {
                    curVal = a[i];
                    curLen = 1;
                    curStart = i;
                }
            }
        }

        Console.WriteLine("--- Завдання 3 ---");
        Console.WriteLine("Значення серії: " + maxVal);
        Console.WriteLine("Довжина: " + maxLen);
        Console.WriteLine("Початок (індекс): " + maxStart);
    }

    static void Task4(int[] a)
    {
        if (a.Length <= 1) return;

        int last = a[a.Length - 1];

        for (int i = a.Length - 1; i > 0; i--)
        {
            a[i] = a[i - 1];
        }

        a[0] = last;
    }

    static void Task5(int[,] m)
    {
        int rows = m.GetLength(0);
        int cols = m.GetLength(1);

        Console.WriteLine("--- Завдання 5 ---");
        Console.WriteLine("Матриця:");
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                Console.Write(m[i, j] + "\t");
            }
            Console.WriteLine();
        }
        Console.WriteLine();

        int maxRowSum = -10000;
        int bestRow = 0;

        for (int i = 0; i < rows; i++)
        {
            int rowSum = 0;
            for (int j = 0; j < cols; j++)
            {
                rowSum += m[i, j];
            }
            Console.WriteLine("Сума рядка " + i + ": " + rowSum);

            if (rowSum > maxRowSum)
            {
                maxRowSum = rowSum;
                bestRow = i;
            }
        }
        Console.WriteLine("Рядок з наибольшою сумою: " + bestRow + "\n");

        for (int j = 0; j < cols; j++)
        {
            int maxInCol = m[0, j];
            for (int i = 1; i < rows; i++)
            {
                if (m[i, j] > maxInCol)
                {
                    maxInCol = m[i, j];
                }
            }
            Console.WriteLine("Максимум стовпця " + j + ": " + maxInCol);
        }
    }
}
