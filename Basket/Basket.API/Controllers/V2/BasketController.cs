using Asp.Versioning;
using Basket.Application.Commands;
using Basket.Application.Mappers;
using Basket.Application.Queries;
using Basket.Core.Entities;
using Common.Logging.Correlation;
using EventBus.Messages.Events;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Basket.API.Controllers.V2
{
    [ApiVersion("2")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly ILogger<BasketController> _logger;
        private readonly ICorrelationIdGenerator _correlationIdGenerator;

        public BasketController(IMediator mediator, IPublishEndpoint publishEndpoint, ILogger<BasketController> logger,
            ICorrelationIdGenerator correlationIdGenerator)
        {
            this._mediator = mediator;
            this._publishEndpoint = publishEndpoint;
            this._logger = logger;
            this._correlationIdGenerator = correlationIdGenerator;
            _logger.LogInformation("CorrelationId {CorrelationId}", _correlationIdGenerator.Get());
        }
        [Route("[action]")]
        [HttpPost]
        //[MapToApiVersion("2")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Checkout([FromBody] BasketCheckoutV2 basketCheckout)
        {
            var query = new GetBasketByUserNameQuery(basketCheckout.UserName);
            var basket = await _mediator.Send(query);
            if (basket == null)
            {
                return BadRequest();
            }
            var eventMsg = BasketMapper.Mapper.Map<BasketCheckOutEventV2>(basketCheckout);
            eventMsg.TotalPrice = basket.TotalPrice;
            eventMsg.CorrelationId = _correlationIdGenerator.Get();
            await _publishEndpoint.Publish(eventMsg);
            // remove the basket
            var deleteQuery = new DeleteBasketByUserNameCommand(basketCheckout.UserName);
            await _mediator.Send(deleteQuery);
            return Accepted();
        }
    }
}
