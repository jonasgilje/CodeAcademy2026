using Kaffebar.Models;

namespace Kaffebar.Storage;

public interface IOrderStore
{
    void Add(Order order);

    Order? Get(Guid orderId);

    void Update(Order order);
}