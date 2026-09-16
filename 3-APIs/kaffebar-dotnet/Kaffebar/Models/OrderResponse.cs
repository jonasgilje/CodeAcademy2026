namespace Kaffebar.Models;

public record OrderResponse(
	Guid OrderId,
	Guid CoffeeId,
	string CustomerName,
	int Quantity,
	Size Size,
	MilkType MilkType,
	bool? ExtraShot,
	OrderStatus Status);