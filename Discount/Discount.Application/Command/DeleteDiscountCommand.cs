using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discount.Application.Command
{
    public class DeleteDiscountCommand:IRequest<bool>
    {

        public string ProductName { get; }

        public DeleteDiscountCommand(string productName)
        {
            ProductName = productName;
        }

    }
}
