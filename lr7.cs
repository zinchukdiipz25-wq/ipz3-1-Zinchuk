using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

// Лабораторна робота №7. Стек і черга
// Виконав: Зінчук Д. Ю., ІПЗ-3/1
// Вхідні дані з бланка не змінено.

record Request(string Name, int Minutes);

class Program
{
    static bool IsBalanced(string line)
    {
        Stack<char> stack = new Stack<char>();
        Dictionary<char, char> pairs = new() { [')'] = '(', [']'] = '[', ['}'] = '{' };
        foreach (char symbol in line)
        {
            if (symbol == '(' || symbol == '[' || symbol == '{')
                stack.Push(symbol);
            else if (pairs.ContainsKey(symbol))
            {
                if (stack.Count == 0) return false; // Перед Pop перевіряємо порожність.
                if (stack.Pop() != pairs[symbol]) return false;
            }
        }
        return stack.Count == 0;
    }

    static int ProcessQueue(IEnumerable<Request> requests)
    {
        Queue<Request> queue = new Queue<Request>(requests);
        if (queue.Count == 0)
        {
            Console.WriteLine("Немає заявок для обробки. Оброблено заявок: 0, загальний час: 0 хв");
            return 0;
        }
        int time = 0, number = 0;
        while (queue.Count > 0) // Перед Dequeue перевіряємо порожність.
        {
            Request request = queue.Dequeue();
            int start = time;
            time += request.Minutes;
            Console.WriteLine($"{++number}. {request.Name} — з {start} до {time} хв");
        }
        Console.WriteLine($"Оброблено заявок: {number}, загальний час: {time} хв");
        return time;
    }

    static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        string[] lines = { "", "a + b", "{[a] + (b)}", "([)]", "a)", "(a[b]", "f(x[1]) * {y + (z)}" };
        Request[] requests = {
            new("Довідка про навчання", 4),
            new("Академічна відпустка", 7),
            new("Копія наказу", 2),
            new("Виписка з оцінками", 6),
            new("Перескладання", 9),
            new("Студентський квиток", 3)
        };
        Console.WriteLine("Задача 1. Перевірка дужок");
        foreach (string line in lines)
            Console.WriteLine($"{(line.Length == 0 ? "(порожній рядок)" : line)} -> {(IsBalanced(line) ? "так" : "ні")}");
        Console.WriteLine("\nЗадача 2. Обробка заявок");
        ProcessQueue(requests);
        // Перевірка порожньої черги (окремий граничний випадок).
        Console.WriteLine("\nПеревірка порожньої черги:");
        ProcessQueue(Array.Empty<Request>());
    }
}
