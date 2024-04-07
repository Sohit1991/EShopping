using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ordering.Application.Command;
using Ordering.Application.Queries;
using Ordering.Application.Responses;
using System.Net;

namespace Ordering.API.Controllers
{

    public class OrderController : ApiController
    {
        private readonly IMediator _mediator;

        public OrderController(IMediator mediator)
        {
            this._mediator = mediator;
        }

        [HttpGet("{userName}", Name = "GetOrdersByUserName")]
        [ProducesResponseType(typeof(IEnumerable<OrderResponse>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<OrderResponse>>> GetOrdersByUserName(string userName)
        {
            var query = new GetOrderListQuery(userName);
            var orders = await _mediator.Send(query);
            return Ok(orders);
        }

        // Just for testing locally as it will be processed in queue
        [HttpPost(Name = "CheckoutOrder")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult<int>> CheckoutOrder([FromBody] CheckOutOrderCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPut(Name = "UpdateOrder")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<int>> UpdateOrder([FromBody] CheckOutOrderCommand command)
        {
            var result = await _mediator.Send(command);
            return NoContent();
        }

        [HttpDelete("{id}", Name = "DeleteOrder")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<int>> DeleteOrder( int id)
        {
            var command = new DeleteOrderCommand() { Id = id };
             await _mediator.Send(command);
            return NoContent();
        }
    }
}
