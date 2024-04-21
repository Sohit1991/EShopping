using AutoMapper;
using EventBus.Messages.Events;
using MassTransit;
using MediatR;
using Ordering.Application.Command;

namespace Ordering.API.EventBusConsumer
{
    // IConcumsere implementaion
    public class BasketOrderingConsumerV2 : IConsumer<BasketCheckOutEventV2>
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly ILogger<BasketOrderingConsumerV2> _logger;

        public BasketOrderingConsumerV2(IMediator mediator, IMapper mapper, ILogger<BasketOrderingConsumerV2> logger)
        {
            this._mediator = mediator;
            this._mapper = mapper;
            this._logger = logger;
        }
        public async Task Consume(ConsumeContext<BasketCheckOutEventV2> context)
        {
            using var scope = _logger.BeginScope("Consumer Basket Checkout Event for {correlationId}",
                    context.Message.CorrelationId);
            var command = _mapper.Map<CheckOutOrderCommand>(context.Message);
            PopulateAddressDetails(command);
            var result = await _mediator.Send(command);
            _logger.LogInformation($"Basket checkout event completed");
        }
        private static void PopulateAddressDetails(CheckOutOrderCommand command)
        {
            command.FirstName = "Sohit";
            command.LastName = "Khanchi";
            command.EmailAddress = "sohitkhanchi@gmail.com";
            command.AddressLine = "Bangalore";
            command.Country = "India";
            command.State = "KA";
            command.ZipCode = "560001";
            command.PaymentMethod = 1;
            command.CardName = "Visa";
            command.CardNumber = "1234567890123456";
            command.Expiration = "12/25";
            command.Cvv= "123";
        }
    }
}
