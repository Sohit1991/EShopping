using Catalog.Application.Mappers;
using Catalog.Application.Queries;
using Catalog.Application.Responses;
using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using MediatR;

namespace Catalog.Application.Handlers
{
    public class GetAllBrandQueryHandler : IRequestHandler<GetAllBrandQuery, IList<BrandResponse>>
    {
        private readonly IBrandRepository _brandRepository;

        public GetAllBrandQueryHandler(IBrandRepository brandRepository)
        {
            this._brandRepository = brandRepository;
        }
        public async Task<IList<BrandResponse>> Handle(GetAllBrandQuery request, CancellationToken cancellationToken)
        {
            var brandList = await _brandRepository.GetAllBrands();
            return ProductMapper.Mapper.Map<IList<ProductBrand>,IList<BrandResponse>>(brandList.ToList());
        }
    }
}
