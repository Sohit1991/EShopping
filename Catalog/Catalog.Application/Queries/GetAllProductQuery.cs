using Catalog.Application.Responses;
using Catalog.Core.Specs;
using MediatR;

namespace Catalog.Application.Queries
{
    public class GetAllProductQuery : IRequest<Pagination<ProductResponse>>
    {
        public GetAllProductQuery(CatalogSpecParams catalogSpec)
        {
            CatalogSpec = catalogSpec;
        }

        public CatalogSpecParams CatalogSpec { get; set; }
    }
}
