using AutoMapper;
using Order.Domain.Entities;
using Order.Application.Features.Orders.Commands.UpdateOrder;
using Order.Application.Features.Orders.Queries.GetOrdersList;
using Order.Application.Features.Orders.Commands.CheckoutOrder;

namespace Order.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<OrderEntity, OrderDto>().ReverseMap();
            CreateMap<OrderEntity, CheckoutOrderCommand>().ReverseMap();
            CreateMap<OrderEntity, UpdateOrderCommand>().ReverseMap();
        }
    }
}