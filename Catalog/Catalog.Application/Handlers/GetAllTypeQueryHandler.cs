using Catalog.Application.Mappers;
using Catalog.Application.Queries;
using Catalog.Application.Responses;
using Catalog.Core.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Application.Handlers
{
    public class GetAllTypeQueryHandler : IRequestHandler<GetAllTypeQuery, IList<TypeResponse>>
    {
        private readonly ITypeRepository _typeRepository;

        public GetAllTypeQueryHandler(ITypeRepository typeRepository)
        {
            this._typeRepository = typeRepository;
        }

        public async Task<IList<TypeResponse>> Handle(GetAllTypeQuery request, CancellationToken cancellationToken)
        {
            var typeList = await _typeRepository.GetAllTypes();
            return ProductMapper.Mapper.Map<IList<TypeResponse>>(typeList);
        }
    }
}
