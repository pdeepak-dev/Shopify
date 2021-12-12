using System;
using MediatR;
using AutoMapper;
using System.Threading;
using System.Threading.Tasks;
using Order.Application.Exceptions;
using Microsoft.Extensions.Logging;
using Order.Application.Contracts.Persistence;

namespace Order.Application.Features.Orders.Commands.DeleteOrder
{
    public class DeleteOrderCommandHandler : IRequestHandler<DeleteOrderCommand>
    {
        private readonly IOrderRepository _orderRepo;
        private readonly IMapper _mapper;
        private readonly ILogger<DeleteOrderCommandHandler> _logger;

        public DeleteOrderCommandHandler(IOrderRepository orderRepo, IMapper mapper, ILogger<DeleteOrderCommandHandler> logger)
        {
            _orderRepo = orderRepo ?? throw new ArgumentNullException(nameof(orderRepo));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Unit> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
        {
            var orderToDelete = await _orderRepo.GetByIdAsync(request.Id);

            if (orderToDelete == null)
            {
                throw new NotFoundException(nameof(Order), request.Id);
            }

            await _orderRepo.DeleteAsync(orderToDelete);

            _logger.LogInformation($"Order {orderToDelete.Id} is successfully deleted.");

            return Unit.Value;
        }
    }
}