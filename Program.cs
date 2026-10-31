using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

// Лабораторна робота №8. Швидкий пошук ключів
// Виконав: Зінчук Д. Ю., ІПЗ-3/1
// Вхідні дані не змінено. Старі варіанти залишено поруч у коментарях.
record Client(int Id, string Name);
record Order(int Number, int ClientId, int Sum);

class Program
{
    static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        Client[] clients = { new(101, "Коваленко"), new(102, "Мельник"), new(103, "Шевчук"), new(104, "Бондар"), new(105, "Ткачук"), new(106, "Гриценко") };
        Order[] orders = { new(1, 103, 450), new(2, 101, 120), new(3, 105, 980), new(4, 103, 300), new(5, 102, 75), new(6, 106, 1500), new(7, 101, 220), new(8, 103, 60) };
        int[] blocked = { 102, 104 };

        Console.WriteLine("Фрагмент 1");
        // Старий варіант:
        // foreach (var order in orders)
        //     foreach (var client in clients)
        //         if (client.Id == order.ClientId)
        //             Console.WriteLine($"Замовлення {order.Number} — {client.Name}");
        Dictionary<int, Client> byId = clients.ToDictionary(c => c.Id);
        foreach (var order in orders)
            if (byId.TryGetValue(order.ClientId, out Client? client))
                Console.WriteLine($"Замовлення {order.Number} — {client.Name}");

        Console.WriteLine("\nФрагмент 2");
        // Старий варіант:
        // int total = 0;
        // foreach (var order in orders)
        //     if (!blocked.Contains(order.ClientId)) total += order.Sum;
        HashSet<int> blockedIds = new HashSet<int>(blocked);
        int total = 0;
        foreach (var order in orders)
            if (!blockedIds.Contains(order.ClientId)) total += order.Sum;
        Console.WriteLine($"Сума прийнятих замовлень: {total}");

        Console.WriteLine("\nФрагмент 3");
        // Старий варіант:
        // List<int> counted = new();
        // foreach (var order in orders) {
        //     if (counted.Contains(order.ClientId)) continue;
        //     int count = 0;
        //     foreach (var other in orders)
        //         if (other.ClientId == order.ClientId) count++;
        //     counted.Add(order.ClientId);
        //     Console.WriteLine($"{order.ClientId}: {count}");
        // }
        Dictionary<int, int> counts = new();
        foreach (var order in orders)
        {
            counts.TryGetValue(order.ClientId, out int count);
            counts[order.ClientId] = count + 1;
        }
        HashSet<int> printed = new();
        foreach (var order in orders) // Порядок першої появи визначає orders.
            if (printed.Add(order.ClientId))
                Console.WriteLine($"{order.ClientId}: {counts[order.ClientId]}");

        Console.WriteLine("\nФрагмент 4");
        // Структура швидкого пошуку не потрібна: один прохід.
        Order best = orders[0];
        foreach (var order in orders)
            if (order.Sum > best.Sum) best = order;
        Console.WriteLine($"Найбільше замовлення: {best.Sum}");

        Console.WriteLine("\nФрагмент 5");
        // Старий варіант: словник помилково створювався всередині циклу замовлень.
        // foreach (var order in orders) {
        //     Dictionary<int, Client> inside = new();
        //     foreach (var c in clients) inside[c.Id] = c;
        //     Console.WriteLine($"Замовлення {order.Number} — {inside[order.ClientId].Name}");
        // }
        // Нової структури НЕ створюємо: використовуємо byId з фрагмента 1.
        foreach (var order in orders)
            Console.WriteLine($"Замовлення {order.Number} — {byId[order.ClientId].Name}");
    }
}
