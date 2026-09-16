using System.ComponentModel.DataAnnotations;

namespace Kaffebar.Models;

public record OrderRequest(
    [Required]
    Guid CoffeeId,
    [Required, StringLength(50, MinimumLength = 2)]
    string CustomerName,
    [Required]
    [Range(1, 10)]
    int Quantity,
    [Required]
    Size Size, 
    [Required]
    MilkType MilkType, 
    bool? ExtraShot = null
);
public enum Size
{
    SMALL,
    MEDIUM,
    LARGE
}

public enum MilkType
{
    WHOLE,
    SKIMMED,
    OAT,
    SOY
}