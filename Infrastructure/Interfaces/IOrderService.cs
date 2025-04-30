using Domain.DTOs.Orders;
using Domain.Filters;
using Domain.Responses;

namespace Infrastructure.Interfaces;

public interface IOrderService
{
  Task<Response<List<GetOrderDto>>> GetAllAsync(OrderFilter filter);
  Task<Response<GetOrderDto>> CreateAsync(CreateOrderDto request);
  Task<Response<GetOrderDto>> GetOrderAsync(int OrderItemId);

}
