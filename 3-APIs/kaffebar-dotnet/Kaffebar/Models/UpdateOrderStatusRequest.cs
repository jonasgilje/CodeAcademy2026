using System.ComponentModel.DataAnnotations;

namespace Kaffebar.Models;

public record UpdateOrderStatusRequest([Required] OrderStatus Status);