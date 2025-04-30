using System.Net;
using AutoMapper;
using Domain.DTOs.OrderItems;
using Domain.Entites;
using Domain.Filters;
using Domain.Responses;
using Infrastructure.Data;
using Infrastructure.Interfaces;

namespace Infrastructure.Services;

public class OrderItemService(DataContext context, IMapper mapper) : IOrderItemService
{
    public async Task<Response<GetOrderItemDto>> CreateAsync(CreateOrderItemDto request)
    {
        var orderItem = mapper.Map<OrderItem>(request);
        await context.OrderItems.AddAsync(orderItem);
        var result = await context.SaveChangesAsync();
        var data = mapper.Map<GetOrderItemDto>(orderItem);

        return result == 0 ?
        new Response<GetOrderItemDto>(HttpStatusCode.BadRequest, "OrderItem not added!")
        : new Response<GetOrderItemDto>(data);
    }

    public async Task<Response<string>> DeleteAsync(int Id)
    {
        var delete = await context.OrderItems.FindAsync(Id);
        if (delete == null)
        {
            return new Response<string>("Id is not found");
        }
        context.Remove(delete);

        var res = await context.SaveChangesAsync();

        return res == 0 ?
        new Response<string>(HttpStatusCode.BadRequest, "OrderItem not deleted!")
        : new Response<string>("Delete Succesfuly");
    }

    public async Task<Response<List<GetOrderItemDto>>> GetAllAsync(OrderItemFilter filter)
    {
        var validFilter = new ValidFilter(filter.PageNumber, filter.PageSize);
        var customer = context.OrderItems.Where(c => c.Quantity > 0).AsQueryable();
        var mapped = mapper.Map<List<GetOrderItemDto>>(customer);

        var totalRecords = mapped.Count;

        var data = mapped
        .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
        .Take(validFilter.PageSize)
        .ToList();

        return new PagedResponse<List<GetOrderItemDto>>(data, validFilter.PageNumber, validFilter.PageSize, totalRecords);

    }

    public async Task<Response<GetOrderItemDto>> GetOrderItemAsync(int Id)
    {
        var OrderItem = await context.OrderItems.FindAsync(Id);
        if (OrderItem == null)
        {
            return new Response<GetOrderItemDto>(HttpStatusCode.NotFound, "Id not found");
        }

        var res = mapper.Map<GetOrderItemDto>(OrderItem);

        return new Response<GetOrderItemDto>(res);
    }

    public async Task<Response<GetOrderItemDto>> UpDateAsync(int Id, UpdateOrderItemDto request)
    {
        var OrderItem = await context.OrderItems.FindAsync(Id);
        if (OrderItem == null)
        {
            return new Response<GetOrderItemDto>(HttpStatusCode.NotFound, "Id not found");
        }


        OrderItem.OrderId = request.OrderId;
        OrderItem.ProductId = request.ProductId;
        OrderItem.Quantity = request.Quantity;

        var res = await context.SaveChangesAsync();
        var maped = mapper.Map<GetOrderItemDto>(OrderItem);


        return res == 0 ?
        new Response<GetOrderItemDto>(HttpStatusCode.BadRequest, "OrderItem not updated")
        : new Response<GetOrderItemDto>(maped);


    }
}

