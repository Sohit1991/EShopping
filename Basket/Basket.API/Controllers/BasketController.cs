using Basket.Application.Commands;
using Basket.Application.GrpcService;
using Basket.Application.Queries;
using Basket.Application.Responses;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Basket.API.Controllers
{
    
    public class BasketController : ApiController
    {
        private readonly IMediator _mediator;

        public BasketController(IMediator mediator)
        {
            this._mediator = mediator;
        }

        [HttpGet]
        [Route("[action]/{userName}",Name ="GetBasketByUserName")]
        [ProducesResponseType(typeof(ShoppingCartResponse),(int)HttpStatusCode.OK)]
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
            
            
            var basket = await _mediator.Send(shoppingCartCommand);

            return Ok(basket);

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
    }
}
