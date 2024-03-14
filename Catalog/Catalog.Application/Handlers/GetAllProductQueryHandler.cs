using Catalog.Application.Mappers;
using Catalog.Application.Queries;
using Catalog.Application.Responses;
using Catalog.Core.Repositories;
using Catalog.Core.Specs;
using MediatR;
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

        public GetAllProductQueryHandler(IProductRepository productRepository)
        {
            this._productRepository = productRepository;
        }
        public async Task<Pagination<ProductResponse>> Handle(GetAllProductQuery request, CancellationToken cancellationToken)
        {
            var productList = await _productRepository.GetProducts(request.CatalogSpec);
            return ProductMapper.Mapper.Map<Pagination<ProductResponse>>(productList);
        }
    }
}
