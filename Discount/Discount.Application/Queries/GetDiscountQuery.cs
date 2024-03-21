using Discount.Grpc.Protos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discount.Application.Queries
{
    public class GetDiscountQuery:IRequest<CouponModel>
    {
        public GetDiscountQuery(string productName)
        {
            ProductName = productName;
        }

        public string ProductName { get; }
    }
}
