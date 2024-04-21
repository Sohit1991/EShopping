using Asp.Versioning;
using Basket.Application.Commands;
using Basket.Application.Mappers;
using Basket.Application.Queries;
using Basket.Application.Responses;
using Basket.Core.Entities;
using Common.Logging.Correlation;
using EventBus.Messages.Events;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Basket.API.Controllers
{

    public class BasketController : ApiController
    {
        private readonly IMediator _mediator;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly ILogger<BasketController> _logger;
        private readonly ICorrelationIdGenerator _correlationIdGenerator;

        public BasketController(IMediator mediator, IPublishEndpoint publishEndpoint,ILogger<BasketController> logger,
            ICorrelationIdGenerator correlationIdGenerator)
        {
            this._mediator = mediator;
            this._publishEndpoint = publishEndpoint;
            this._logger = logger;
            this._correlationIdGenerator = correlationIdGenerator;
            _logger.LogInformation("CorrelationId {CorrelationId}", _correlationIdGenerator.Get());
        }

        [HttpGet]
        [Route("[action]/{userName}", Name = "GetBasketByUserName")]
        [ProducesResponseType(typeof(ShoppingCartResponse), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShoppingCartResponse>> GetBasket(string userName)
        {
            var query = new GetBasketByUserNameQuery(userName);
            var basket = await _mediator.Send(query);
            return Ok(basket);
        }

        [HttpPost("CreateBasket")]
        [ProducesResponseType(typeof(ShoppingCartResponse), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShoppingCartResponse>> UpdateBasket([FromBody] CreateShoppingCartCommand shoppingCartCommand)
        {

            try
            {
                var basket = await _mediator.Send(shoppingCartCommand);

                return Ok(basket);

            }
            catch (Exception ex)
            {

                throw ex;
            }
            //var basket = await _mediator.Send(shoppingCartCommand);

            //return Ok(basket);

        }

        [HttpDelete]
        [Route("[action]/{userName}", Name = "DeletBasketByUserName")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShoppingCartResponse>> DeletBasket(string userName)
        {
            var query = new DeleteBasketByUserNameCommand(userName);
            await _mediator.Send(query);
            return Ok(query);
        }

        [Route("[action]")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Checkout([FromBody] BasketCheckout basketCheckout)
        {
            var query = new GetBasketByUserNameQuery(basketCheckout.UserName);
            var basket = await _mediator.Send(query);
            if (basket == null)
            {
                return BadRequest();
            }
            var eventMsg = BasketMapper.Mapper.Map<BasketCheckOutEvent>(basketCheckout);
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
