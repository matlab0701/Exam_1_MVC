using AutoMapper;
using Domain.DTOs.Customers;
using Domain.DTOs.OrderItems;
using Domain.DTOs.Orders;
using Domain.DTOs.Products;
using Domain.Entites;

namespace Infrastructure.AutoMapper;

public class InfrastructureProfile : Profile
{
    public InfrastructureProfile()
    {

        CreateMap<Customer, GetCustomerDto>();
        CreateMap<GetCustomerDto, Customer>();
        CreateMap<Product, GetProductDto>();
        CreateMap<GetProductDto, Product>();
        CreateMap<Order, GetOrderDto>();
        CreateMap<GetOrderDto, Order>();
        CreateMap<OrderItem, GetOrderItemDto>();
        CreateMap<GetOrderItemDto, OrderItem>();
        CreateMap<OrderItem, ProOrderItemDto>();
        CreateMap<ProOrderItemDto, OrderItem>();

        CreateMap<CreateProductDto, Product>();
        CreateMap<UpdateProductDto, Product>();

        CreateMap<CreateCustomerDto, Customer>();
        CreateMap<UpdateCustomerDto, Customer>();

        CreateMap<CreateOrderDto, Order>();
        CreateMap<UpdateOrderDto, Order>();

        CreateMap<CreateOrderItemDto, OrderItem>();
        CreateMap<UpdateOrderItemDto, OrderItem>();


    }
}
