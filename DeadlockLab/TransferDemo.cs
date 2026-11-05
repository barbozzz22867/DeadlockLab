namespace DeadlockLab;

public class TransferDemo
{
    public static async Task RunSafeAsync()
    {
        Console.WriteLine("=== SAFE TRANSFER DEMO ===");

        BankAccount accountA = new(1000);
        BankAccount accountB = new(2000);

        BankTransferService service = new();

        const int operations = 1000;
        const decimal amount = 1;

        Task[] tasks = new Task[2];

        tasks[0] = Task.Run(() =>
        {
            for (int i = 0; i < operations; i++)
            {
                service.TransferSafe(
                    accountA,
                    accountB,
                    amount
                );
            }
        });

        tasks[1] = Task.Run(() =>
        {
            for (int i = 0; i < operations; i++)
            {
                service.TransferSafe(
                    accountB,
                    accountA,
                    amount
                );
            }
        });

        await Task.WhenAll(tasks);

        Console.WriteLine(
            $"Баланс A: {accountA.Balance}"
        );

        Console.WriteLine(
            $"Баланс B: {accountB.Balance}"
        );

        Console.WriteLine(
            $"Общий баланс: {accountA.Balance + accountB.Balance}"
        );
    }
}