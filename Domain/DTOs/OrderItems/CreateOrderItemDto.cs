using Domain.Entites;

namespace Domain.DTOs.OrderItems;

public class CreateOrderItemDto
{
    public int OrderId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public Product Product { get; set; }


}
