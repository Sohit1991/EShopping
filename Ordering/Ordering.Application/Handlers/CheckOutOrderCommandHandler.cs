using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Command;
using Ordering.Core.Entities;
using Ordering.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Handlers
{
    public class CheckOutOrderCommandHandler : IRequestHandler<CheckOutOrderCommand, int>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CheckOutOrderCommandHandler> _logger;

        public CheckOutOrderCommandHandler(IOrderRepository orderRepository,IMapper mapper,ILogger<CheckOutOrderCommandHandler> logger)
        {
            this._orderRepository = orderRepository;
            this._mapper = mapper;
            this._logger = logger;
        }
        public async Task<int> Handle(CheckOutOrderCommand request, CancellationToken cancellationToken)
        {
            var orderEntity = _mapper.Map<Order>(request);
            var generatedOrder = await _orderRepository.AddAsync(orderEntity);
            _logger.LogInformation($"Order {generatedOrder} successfully created");
            return generatedOrder.Id;
        }
    }
}
