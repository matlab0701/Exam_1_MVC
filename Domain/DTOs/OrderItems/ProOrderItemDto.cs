using Domain.Entites;

namespace Domain.DTOs.OrderItems;

public class ProOrderItemDto
{
    public decimal Price { get; set; }
    public decimal totalItem { get; set; }
    public string ProductName { get; set; }
    public int Quantity { get; set; }
}
