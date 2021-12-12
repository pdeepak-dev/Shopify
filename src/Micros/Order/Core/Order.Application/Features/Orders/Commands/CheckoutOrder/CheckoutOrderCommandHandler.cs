using System;
using MediatR;
using AutoMapper;
using System.Threading;
using Order.Domain.Entities;
using System.Threading.Tasks;
using Order.Application.Models;
using Microsoft.Extensions.Logging;
using Order.Application.Contracts.Persistence;
using Order.Application.Contracts.Infrastructure;

namespace Order.Application.Features.Orders.Commands.CheckoutOrder
{
    public class CheckoutOrderCommandHandler : IRequestHandler<CheckoutOrderCommand, int>
    {
        private readonly IOrderRepository _orderRepo;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;
        private readonly ILogger<CheckoutOrderCommandHandler> _logger;

        public CheckoutOrderCommandHandler(IOrderRepository orderRepo, IMapper mapper, IEmailService emailService, ILogger<CheckoutOrderCommandHandler> logger)
        {
            _orderRepo = orderRepo ?? throw new ArgumentNullException(nameof(orderRepo));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<int> Handle(CheckoutOrderCommand request, CancellationToken cancellationToken)
        {
            var orderEntity = _mapper.Map<OrderEntity>(request);
            var newOrder = await _orderRepo.AddAsync(orderEntity);

            _logger.LogInformation($"Order {newOrder.Id} is succesfully created.");

            await SendMail(newOrder);

            return newOrder.Id;
        }

        private async Task SendMail(OrderEntity newOrder)
        {
            var email = new Email()
            {
                To = "deepakpandey753951@gmail.com",
                Body = $"Order was created.",
                Subject = "Order was created"
            };

            try
            {
                await _emailService.SendEmail(email);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Order {newOrder.Id} failed due to an error with the mail service: " +
                    $"{ex.Message}");
            }
        }
    }
}