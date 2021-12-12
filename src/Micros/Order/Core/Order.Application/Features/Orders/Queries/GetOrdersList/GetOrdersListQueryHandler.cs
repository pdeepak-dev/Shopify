using MediatR;
using AutoMapper;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Order.Application.Contracts.Persistence;

namespace Order.Application.Features.Orders.Queries.GetOrdersList
{
    public class GetOrdersListQueryHandler : IRequestHandler<GetOrdersListQuery, List<OrderDto>>
    {
        private readonly IOrderRepository _orderRepo;
        private readonly IMapper _mapper;

        public GetOrdersListQueryHandler(IOrderRepository orderRepo, IMapper mapper)
        {
            _orderRepo = orderRepo;
            _mapper = mapper;
        }

        public async Task<List<OrderDto>> Handle(GetOrdersListQuery request, CancellationToken cancellationToken)
        {
            var orderList = await _orderRepo.GetOrdersByUserName(request?.UserName);
            return _mapper.Map<List<OrderDto>>(orderList);
        }
    }
}