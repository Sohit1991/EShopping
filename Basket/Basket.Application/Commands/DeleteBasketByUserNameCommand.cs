using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basket.Application.Commands
{
    public class DeleteBasketByUserNameCommand : IRequest
    {
        public DeleteBasketByUserNameCommand(string userName)
        {
            UserName = userName;
        }
        public string UserName { get; set; }
    }
}
