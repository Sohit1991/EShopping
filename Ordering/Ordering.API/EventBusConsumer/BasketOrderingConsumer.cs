using AutoMapper;
using EventBus.Messages.Events;
using MassTransit;
using MediatR;
using Ordering.Application.Command;

namespace Ordering.API.EventBusConsumer
{
    // IConcumsere implementaion
    public class BasketOrderingConsumer : IConsumer<BasketCheckOutEvent>
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly ILogger<BasketOrderingConsumer> _logger;

        public BasketOrderingConsumer(IMediator mediator, IMapper mapper, ILogger<BasketOrderingConsumer> logger)
        {
            this._mediator = mediator;
            this._mapper = mapper;
            this._logger = logger;
        }
        public async Task Consume(ConsumeContext<BasketCheckOutEvent> context)
        {
            using var scope = _logger.BeginScope("Consumer Basket Checkout Event for {correlationId}",
                    context.Message.CorrelationId);
            var command = _mapper.Map<CheckOutOrderCommand>(context.Message);
            var result = await _mediator.Send(command);
            _logger.LogInformation($"Basket checkout event completed");
        }
    }
}
