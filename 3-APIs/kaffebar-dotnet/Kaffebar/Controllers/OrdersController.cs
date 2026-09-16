using Kaffebar.Models;
using Kaffebar.Storage;
using Microsoft.AspNetCore.Mvc;

namespace Kaffebar.Controllers;

[ApiController]
[Route("orders")]
public class OrdersController : ControllerBase
{
    private readonly IOrderStore _orderStore;

    public OrdersController(IOrderStore orderStore)
    {
        _orderStore = orderStore;
    }

    [HttpPost]
    public ActionResult<OrderResponse> PlaceOrder(OrderRequest orderRequest)
    {
        var order = new Order(
            Guid.NewGuid(),
            orderRequest.CoffeeId,
            orderRequest.CustomerName,
            orderRequest.Quantity,
            orderRequest.Size,
            orderRequest.MilkType,
            orderRequest.ExtraShot,
            OrderStatus.PENDING);

        _orderStore.Add(order);

        return Created($"/orders/{order.OrderId}", ToResponse(order));
    }

    [HttpGet("{orderId:guid}")]
    public ActionResult<OrderResponse> GetOrder(Guid orderId)
    {
        var order = _orderStore.Get(orderId);
        if (order is null)
        {
            return NotFound();
        }

        return Ok(ToResponse(order));
    }

    [HttpPatch("{orderId:guid}/status")]
    public ActionResult<OrderResponse> UpdateStatus(Guid orderId, UpdateOrderStatusRequest request)
    {
        var order = _orderStore.Get(orderId);
        if (order is null)
        {
            return NotFound();
        }

        if (!IsValidStatusTransition(order.Status, request.Status))
        {
            return Conflict(new ProblemDetails
            {
                Title = "Invalid status transition",
                Detail = $"Cannot change order status from {order.Status} to {request.Status}.",
                Status = StatusCodes.Status409Conflict
            });
        }

        var updatedOrder = order with { Status = request.Status };
        _orderStore.Update(updatedOrder);

        return Ok(ToResponse(updatedOrder));
    }

    private static OrderResponse ToResponse(Order order)
    {
        return new OrderResponse(
            order.OrderId,
            order.CoffeeId,
            order.CustomerName,
            order.Quantity,
            order.Size,
            order.MilkType,
            order.ExtraShot,
            order.Status);
    }

    private static bool IsValidStatusTransition(OrderStatus currentStatus, OrderStatus nextStatus)
    {
        return currentStatus switch
        {
            OrderStatus.PENDING => nextStatus == OrderStatus.BREWING,
            OrderStatus.BREWING => nextStatus == OrderStatus.READY,
            OrderStatus.READY => false,
            _ => false
        };
    }
}