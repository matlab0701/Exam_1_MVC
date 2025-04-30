using Domain.DTOs.Customers;
using Domain.Filters;
using Domain.Responses;

namespace Infrastructure.Interfaces;

public interface ICustomerService
{
    Task<Response<List<GetCustomerDto>>> GetAllAsync(CustomerFilter filter);
    Task<Response<GetCustomerDto>> CreateAsync(CreateCustomerDto request);
    Task<Response<GetCustomerDto>> GetCustomerAsync(int Id);
    Task<Response<GetCustomerDto>> UpDateAsync(int Id, UpdateCustomerDto request);
    Task<Response<string>> DeleteAsync(int Id);
}
