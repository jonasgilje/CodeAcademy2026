using Kaffebar.Models;
using System.Collections.Concurrent;

namespace Kaffebar.Storage;

public class InMemoryOrderStore : IOrderStore
{
    private readonly ConcurrentDictionary<Guid, Order> _orders = new();

    public void Add(Order order)
    {
        _orders[order.OrderId] = order;
    }

    public Order? Get(Guid orderId)
    {
        return _orders.GetValueOrDefault(orderId);
    }

    public void Update(Order order)
    {
        _orders[order.OrderId] = order;
    }
}