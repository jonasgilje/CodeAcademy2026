namespace Kaffebar.Models;

public record Order(
    Guid OrderId,
    Guid CoffeeId,
    string CustomerName,
    int Quantity,
    Size Size,
    MilkType MilkType,
    bool? ExtraShot,
    OrderStatus Status);