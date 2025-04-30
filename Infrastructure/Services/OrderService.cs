using System.Net;
using AutoMapper;
using Domain.DTOs.Customers;
using Domain.DTOs.OrderItems;
using Domain.DTOs.Orders;
using Domain.Entites;
using Domain.Filters;
using Domain.Responses;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class OrderService(DataContext context, IMapper mapper) : IOrderService
{
    public async Task<Response<GetOrderDto>> CreateAsync(CreateOrderDto request)
    {
        var order = mapper.Map<Order>(request);
        request.OrderDate = order.OrderDate = order.OrderDate.ToUniversalTime();

       var customer=await context.Customers.FindAsync(request.CustomerId);
       if (customer==null)
       {
        return new Response<GetOrderDto>(HttpStatusCode.NotFound,"Customer not Found");
       }
       
        await context.Orders.AddAsync(order);
        var result = await context.SaveChangesAsync();
        var data = mapper.Map<GetOrderDto>(order);

        return result == 0 ?
        new Response<GetOrderDto>(HttpStatusCode.BadRequest, "order not added!")
        : new Response<GetOrderDto>(data);
    }


    public async Task<Response<List<GetOrderDto>>> GetAllAsync(OrderFilter filter)
    {
        var validFilter = new ValidFilter(filter.PageNumber, filter.PageSize);
        var customer = context.Orders.AsQueryable();
        if (filter.From != null)
        {
            customer = customer.Where(c => c.OrderDate >= filter.From);
        }

        if (filter.To != null)
        {
            customer = customer.Where(c => c.OrderDate <= filter.To);
        }

        var mapped = mapper.Map<List<GetOrderDto>>(customer);

        var totalRecords = mapped.Count;

        var data = mapped
        .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
        .Take(validFilter.PageSize)
        .ToList();

        return new PagedResponse<List<GetOrderDto>>(data, validFilter.PageNumber, validFilter.PageSize, totalRecords);

    }

    public async Task<Response<GetOrderDto>> GetOrderAsync(int orderId)
    {

        var order = await context.Orders
           .Include(o => o.Customer)
           .Include(o => o.Items)
            .ThenInclude(i => i.Product)
           .FirstOrDefaultAsync(o => o.Id == orderId);

        if (order == null)
        { return new Response<GetOrderDto>(HttpStatusCode.NotFound, "id not found"); }

        var dto = new GetOrderDto
        {
            Id = order.Id,
            OrderDate = order.OrderDate,
            Customer = new GetCustomerDto
            {
                FullName = order.Customer.FullName,
                Email = order.Customer.Email,
                PhoneNumber = order.Customer.PhoneNumber
            },
            Items = order.Items.Select(i => new ProOrderItemDto
            {
                ProductName = i.Product.Name,
                Price = i.Product.Price,
                Quantity = i.Quantity,
                totalItem = i.Product.Price * i.Quantity
            }).ToList(),
            Total = order.Items.Sum(i => i.Product.Price * i.Quantity)
        };
        return new Response<GetOrderDto>(dto);
    }


}
