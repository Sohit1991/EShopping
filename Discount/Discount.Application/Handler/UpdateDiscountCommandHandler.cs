﻿using AutoMapper;
using Discount.Application.Command;
using Discount.Core.Entities;
using Discount.Core.Repositories;
using Discount.Grpc.Protos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discount.Application.Handler
{
    public class UpdateDiscountCommandHandler : IRequestHandler<UpdateDiscountCommand, CouponModel>
    {
        private readonly IDiscountRepository _discountRepository;
        private readonly IMapper _mapper;

        public UpdateDiscountCommandHandler(IDiscountRepository discountRepository,IMapper mapper)
        {
            this._discountRepository = discountRepository;
            this._mapper = mapper;
        }
        public async Task<CouponModel> Handle(UpdateDiscountCommand request, CancellationToken cancellationToken)
        {
            var coupon = _mapper.Map<Coupon>(request);
            await _discountRepository.UpdateDiscount(coupon);
            var couponModel = _mapper.Map<CouponModel>(coupon);
            return couponModel;
        }
    }
}
