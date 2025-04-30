using Domain.DTOs.Customers;
using Domain.DTOs.OrderItems;
using Domain.Entites;

namespace Domain.DTOs.Orders;

public class CreateOrderDto
{
    public int CustomerId { get; set; }
    public decimal Total { get; set; }
    public DateTime OrderDate { get; set; }
    public GetCustomerDto Customer { get; set; } 
    public List<ProOrderItemDto> Items{get; set; }
}
