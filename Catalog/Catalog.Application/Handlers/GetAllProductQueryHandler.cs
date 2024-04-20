
using Catalog.Application.Mappers;
using Catalog.Application.Queries;
using Catalog.Application.Responses;
using Catalog.Core.Repositories;
using Catalog.Core.Specs;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Application.Handlers
{
    public class GetAllProductQueryHandler : IRequestHandler<GetAllProductQuery, Pagination<ProductResponse>>
    {
        private readonly IProductRepository _productRepository;
        private readonly ILogger<GetAllProductQueryHandler> _logger;

        public GetAllProductQueryHandler(IProductRepository productRepository,ILogger<GetAllProductQueryHandler> logger)
        {
            this._productRepository = productRepository;
            this._logger = logger;
        }
        public async Task<Pagination<ProductResponse>> Handle(GetAllProductQuery request, CancellationToken cancellationToken)
        {
            var productList = await _productRepository.GetProducts(request.CatalogSpec);
            _logger.LogDebug("Recieved Products List.Total Count {productList}",productList.Count);
            return ProductMapper.Mapper.Map<Pagination<ProductResponse>>(productList);
        }
    }
}
