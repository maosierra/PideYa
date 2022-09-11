using PideYa.Shared.Entities;

namespace PideYa.Client.Services;

public class OrderNotifierService
{
    public Order? Order { get; private set; }
    public event Func<Task>? Notify;

    public async Task AddOrder(Order? order)
    {
        this.Order = order;
        if (Notify is not null)
            await Notify?.Invoke();
    }
}

