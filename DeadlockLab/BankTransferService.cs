namespace DeadlockLab;

public class BankTransferService
{
    // TODO 5: защита от перевода на тот же счёт.
    public void TransferSafe(
        BankAccount from,
        BankAccount to,
        decimal amount)
    {
        if (ReferenceEquals(from, to))
            throw new ArgumentException(
                "Счета должны отличаться.",
                nameof(to));

        if (amount <= 0)
            throw new ArgumentOutOfRangeException(
                nameof(amount));

        // Единый порядок захвата: сначала счёт с меньшим Id.
        BankAccount first =
            from.Id < to.Id ? from : to;

        BankAccount second =
            from.Id < to.Id ? to : from;

        lock (first.SyncRoot)
        {
            lock (second.SyncRoot)
            {
                if (from.GetBalanceUnsafe() < amount)
                    throw new InvalidOperationException(
                        "Недостаточно средств.");

                from.SetBalanceUnsafe(
                    from.GetBalanceUnsafe() - amount);

                to.SetBalanceUnsafe(
                    to.GetBalanceUnsafe() + amount);
            }
        }
    }

    // Неправильная версия — для TODO 1 из TransferDemo.
    public void TransferUnsafe(
        BankAccount from,
        BankAccount to,
        decimal amount)
    {
        lock (from.SyncRoot)
        {
            lock (to.SyncRoot)
            {
                if (from.GetBalanceUnsafe() < amount)
                    throw new InvalidOperationException(
                        "Недостаточно средств.");

                from.SetBalanceUnsafe(
                    from.GetBalanceUnsafe() - amount);

                to.SetBalanceUnsafe(
                    to.GetBalanceUnsafe() + amount);
            }
        }
    }
}