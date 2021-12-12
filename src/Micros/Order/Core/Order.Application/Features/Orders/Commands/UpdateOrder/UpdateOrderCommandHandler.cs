using System;
using MediatR;
using AutoMapper;
using System.Threading;
using Order.Domain.Entities;
using System.Threading.Tasks;
using Order.Application.Exceptions;
using Microsoft.Extensions.Logging;
using Order.Application.Contracts.Persistence;

namespace Order.Application.Features.Orders.Commands.UpdateOrder
{
    public class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand>
    {
        private readonly IOrderRepository _orderRepo;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateOrderCommandHandler> _logger;

        public UpdateOrderCommandHandler(IOrderRepository orderRepo, IMapper mapper, ILogger<UpdateOrderCommandHandler> logger)
        {
            _orderRepo = orderRepo ?? throw new ArgumentNullException(nameof(orderRepo));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Unit> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
        {
            var orderToUpdate = await _orderRepo.GetByIdAsync(request.Id);

            if (orderToUpdate == null)
            {
                throw new NotFoundException(nameof(Order), request.Id);
            }

            _mapper.Map<OrderEntity>(request);

            await _orderRepo.UpdateAsync(orderToUpdate);

            _logger.LogInformation($"Order {orderToUpdate.Id} is successfully updated.");

            return Unit.Value;
        }
    }
}