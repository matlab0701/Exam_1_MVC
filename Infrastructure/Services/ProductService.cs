using System.Net;
using AutoMapper;
using Domain.DTOs.Products;
using Domain.Entites;
using Domain.Filters;
using Domain.Responses;
using Infrastructure.Data;
using Infrastructure.Interfaces;

namespace Infrastructure.Services;

public class ProductService(DataContext context, IMapper mapper) : IProductService
{
    public async Task<Response<GetProductDto>> CreateAsync(CreateProductDto request)
    {
        var product = mapper.Map<Product>(request);
        await context.Products.AddAsync(product);
        var result = await context.SaveChangesAsync();
        var data = mapper.Map<GetProductDto>(product);

        return result == 0 ?
        new Response<GetProductDto>(HttpStatusCode.BadRequest, "Product not added!")
        : new Response<GetProductDto>(data);
    }

    public async Task<Response<string>> DeleteAsync(int Id)
    {
        var delete = await context.Products.FindAsync(Id);
        if (delete == null)
        {
            return new Response<string>("Id is not found");
        }
        context.Remove(delete);

        var res = await context.SaveChangesAsync();

        return res == 0 ?
        new Response<string>(HttpStatusCode.BadRequest, "Product not deleted!")
        : new Response<string>("Delete Succesfuly");
    }

    public async Task<Response<List<GetProductDto>>> GetAllAsync(ProductFilter filter)
    {
        var validFilter = new ValidFilter(filter.PageNumber, filter.PageSize);
        var customer = context.Products.AsQueryable();
        if (filter.Name != null)
        {
            customer = customer.Where(c => c.Name.ToLower().Contains(filter.Name.ToLower()));
        }


        var mapped = mapper.Map<List<GetProductDto>>(customer);

        var totalRecords = mapped.Count;

        var data = mapped
        .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
        .Take(validFilter.PageSize)
        .ToList();

        return new PagedResponse<List<GetProductDto>>(data, validFilter.PageNumber, validFilter.PageSize, totalRecords);

    }

    public async Task<Response<GetProductDto>> GetProductAsync(int Id)
    {
        var Product = await context.Products.FindAsync(Id);
        if (Product == null)
        {
            return new Response<GetProductDto>(HttpStatusCode.NotFound, "Id not found");
        }

        var res = mapper.Map<GetProductDto>(Product);

        return new Response<GetProductDto>(res);
    }

    public async Task<Response<GetProductDto>> UpDateAsync(int Id, UpdateProductDto request)
    {
        var Product = await context.Products.FindAsync(Id);
        if (Product == null)
        {
            return new Response<GetProductDto>(HttpStatusCode.NotFound, "Id not found");
        }


        Product.Name = request.Name;
        Product.Price = request.Price;
        var res = await context.SaveChangesAsync();
        var maped = mapper.Map<GetProductDto>(Product);


        return res == 0 ?
        new Response<GetProductDto>(HttpStatusCode.BadRequest, "Product not updated")
        : new Response<GetProductDto>(maped);


    }
}
