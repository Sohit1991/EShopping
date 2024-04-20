using Catalog.Application.Commands;
using Catalog.Application.Queries;
using Catalog.Application.Responses;
using Catalog.Core.Specs;
using Common.Logging.Correlation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Catalog.API.Controllers
{

    public class CatalogController : ApiController
    {
        private readonly IMediator _mediator;
        private readonly ILogger<CatalogController> _logger;
        private readonly ICorrelationIdGenerator _correlationIdGenerator;

        public CatalogController(IMediator mediator, ILogger<CatalogController> logger, ICorrelationIdGenerator correlationIdGenerator)
        {
            this._mediator = mediator;
            this._logger = logger;
            this._correlationIdGenerator = correlationIdGenerator;
            _logger.LogInformation("CorrelationId {CorrelationId}", _correlationIdGenerator.Get());
        }


        [HttpGet]
        [Route("[action]/{id}", Name = "GetProductById")]
        [ProducesResponseType(typeof(ProductResponse), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ProductResponse>> GetProductById(string id)
        {
            var query = new GetProductByIdQuery(id);
            var product = await _mediator.Send(query);
            return Ok(product);
        }

        [HttpGet]
        [Route("[action]/{productName}", Name = "GetProductByName")]
        [ProducesResponseType(typeof(IList<ProductResponse>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IList<ProductResponse>>> GetProductByName(string productName)
        {
            var query = new GetProductByNameQuery(productName);
            var product = await _mediator.Send(query);
            return Ok(product);
        }

        [HttpGet]
        [Route("GetAllProducts")]
        [ProducesResponseType(typeof(IList<ProductResponse>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IList<ProductResponse>>> GetAllProducts([FromQuery] CatalogSpecParams catalogSpecParams)
        {
            var query = new GetAllProductQuery(catalogSpecParams);
            var product = await _mediator.Send(query);
            _logger.LogInformation("Successfully all products retriedved ");
            return Ok(product);
        }

        [HttpGet]
        [Route("GetAllBrands")]
        [ProducesResponseType(typeof(IList<BrandResponse>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IList<BrandResponse>>> GetAllBrands()
        {
            var query = new GetAllBrandQuery();
            var product = await _mediator.Send(query);
            return Ok(product);
        }

        [HttpGet]
        [Route("GetAllTypes")]
        [ProducesResponseType(typeof(IList<TypeResponse>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IList<TypeResponse>>> GetAllTypes()
        {
            var query = new GetAllTypeQuery();
            var product = await _mediator.Send(query);
            return Ok(product);
        }

        [HttpGet]
        [Route("[action]/{brandName}", Name = "GetProductByBrandName")]
        [ProducesResponseType(typeof(IList<ProductResponse>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IList<ProductResponse>>> GetProductByBrandName(string brandName)
        {
            var query = new GetProductByBrandQuery(brandName);
            var product = await _mediator.Send(query);
            return Ok(product);
        }

        [HttpPost]
        [Route("CreateProduct")]
        [ProducesResponseType(typeof(ProductResponse), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ProductResponse>> CreateProduct([FromBody] CreateProductCommand product)
        {

            var result = await _mediator.Send(product);
            return Ok(result);
        }

        [HttpPut]
        [Route("UpdateProduct")]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateProduct([FromBody] UpdateProductCommand product)
        {

            var result = await _mediator.Send(product);
            return Ok(result);
        }

        [HttpDelete]
        [Route("[action]/{id}", Name = "DeleteProduct")]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteProduct(string id)
        {
            var command = new DeleteProductByIdCommand(id);
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}
