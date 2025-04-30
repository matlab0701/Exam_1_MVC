using Domain.DTOs.Products;
using Domain.Filters;
using Domain.Responses;

namespace Infrastructure.Interfaces;

public interface IProductService
{
    Task<Response<List<GetProductDto>>> GetAllAsync(ProductFilter filter);
    Task<Response<GetProductDto>> CreateAsync(CreateProductDto request);
    Task<Response<GetProductDto>> GetProductAsync(int Id);
    Task<Response<GetProductDto>> UpDateAsync(int Id, UpdateProductDto request);
    Task<Response<string>> DeleteAsync(int Id);
}
