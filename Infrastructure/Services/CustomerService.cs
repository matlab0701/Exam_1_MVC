using System.Net;
using AutoMapper;
using Domain.DTOs.Customers;
using Domain.Entites;
using Domain.Filters;
using Domain.Responses;
using Infrastructure.Data;
using Infrastructure.Interfaces;

namespace Infrastructure.Services;

public class CustomerService(DataContext context, IMapper mapper) : ICustomerService
{

    public async Task<Response<GetCustomerDto>> CreateAsync(CreateCustomerDto request)
    {
        var customer = mapper.Map<Customer>(request);
        await context.Customers.AddAsync(customer);
        var result = await context.SaveChangesAsync();
        var data = mapper.Map<GetCustomerDto>(customer);

        return result == 0 ?
        new Response<GetCustomerDto>(HttpStatusCode.BadRequest, "Customer not added!")
        : new Response<GetCustomerDto>(data);
    }

    public async Task<Response<string>> DeleteAsync(int Id)
    {
        var delete = await context.Customers.FindAsync(Id);
        if (delete == null)
        {
            return new Response<string>("Id is not found");
        }
        context.Remove(delete);

        var res = await context.SaveChangesAsync();

        return res == 0 ?
        new Response<string>(HttpStatusCode.BadRequest, "Customer not deleted!")
        : new Response<string>("Delete Succesfuly");
    }

    public async Task<Response<List<GetCustomerDto>>> GetAllAsync(CustomerFilter filter)
    {
        var validFilter = new ValidFilter(filter.PageNumber, filter.PageSize);
        var customer = context.Customers.AsQueryable();
        if (filter.FullName != null)
        {
            customer = customer.Where(c => c.FullName.ToLower().Contains(filter.FullName.ToLower()));
        }

        if (filter.Email != null)
        {
            customer = customer.Where(c => c.Email.ToLower().Contains(filter.Email.ToLower()));
        }

        var mapped = mapper.Map<List<GetCustomerDto>>(customer);

        var totalRecords = mapped.Count;

        var data = mapped
        .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
        .Take(validFilter.PageSize)
        .ToList();

        return new PagedResponse<List<GetCustomerDto>>(data, validFilter.PageNumber, validFilter.PageSize, totalRecords);

    }

    public async Task<Response<GetCustomerDto>> GetCustomerAsync(int Id)
    {
        var customer = await context.Customers.FindAsync(Id);
        if (customer == null)
        {
            return new Response<GetCustomerDto>(HttpStatusCode.NotFound, "Id not found");
        }

        var res = mapper.Map<GetCustomerDto>(customer);

        return new Response<GetCustomerDto>(res);
    }

    public async Task<Response<GetCustomerDto>> UpDateAsync(int Id, UpdateCustomerDto request)
    {
        var customer = await context.Customers.FindAsync(Id);
        if (customer == null)
        {
            return new Response<GetCustomerDto>(HttpStatusCode.NotFound, "Id not found");
        }


        customer.PhoneNumber = request.PhoneNumber;
        customer.FullName = request.FullName;
        customer.Email = request.Email;

        var res = await context.SaveChangesAsync();
        var maped = mapper.Map<GetCustomerDto>(customer);


        return res == 0 ?
        new Response<GetCustomerDto>(HttpStatusCode.BadRequest, "Customer not updated")
        : new Response<GetCustomerDto>(maped);


    }

}
