using Domain.DTOs.OrderItems;
using Domain.Filters;
using Domain.Responses;

namespace Infrastructure.Interfaces;

public interface IOrderItemService
{
    Task<Response<List<GetOrderItemDto>>> GetAllAsync(OrderItemFilter filter);
    Task<Response<GetOrderItemDto>> CreateAsync(CreateOrderItemDto request);
    Task<Response<GetOrderItemDto>> GetOrderItemAsync(int Id);
    Task<Response<GetOrderItemDto>> UpDateAsync(int Id, UpdateOrderItemDto request);
    Task<Response<string>> DeleteAsync(int Id);
}
