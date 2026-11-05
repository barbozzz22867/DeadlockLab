namespace DeadlockLab;

public class DeadlockDemo
{
    public static void Run()
    {
        Console.WriteLine("=== DEADLOCK DEMO ===");

        object lockA = new();
        object lockB = new();

        using Barrier barrier = new(2);

        Thread threadA = new(() =>
        {
            Console.WriteLine("Поток A: ожидает ресурс A");

            lock (lockA)
            {
                Console.WriteLine("Поток A: захватил ресурс A");

                barrier.SignalAndWait();

                Console.WriteLine("Поток A: ожидает ресурс B");

                lock (lockB)
                {
                    Console.WriteLine("Поток A: получил оба ресурса");
                }
            }
        });

        Thread threadB = new(() =>
        {
            Console.WriteLine("Поток B: ожидает ресурс B");

            lock (lockB)
            {
                Console.WriteLine("Поток B: захватил ресурс B");

                barrier.SignalAndWait();

                Console.WriteLine("Поток B: ожидает ресурс A");

                lock (lockA)
                {
                    Console.WriteLine("Поток B: получил оба ресурса");
                }
            }
        });

        threadA.IsBackground = true;
        threadB.IsBackground = true;

        threadA.Start();
        threadB.Start();

        bool completedA = threadA.Join(2000);
        bool completedB = threadB.Join(2000);

        Console.WriteLine();

        if (!completedA || !completedB)
        {
            Console.WriteLine("DEADLOCK ОБНАРУЖЕН!");
            Console.WriteLine("Потоки не завершились за 2 секунды.");
        }
        else
        {
            Console.WriteLine("Оба потока завершились.");
        }
    }
}