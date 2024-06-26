﻿using AutoMapper;
using EventBus.Messages.Events;
using Ordering.Application.Command;
using Ordering.Application.Responses;
using Ordering.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Mappers
{
    public class OrderMappingProfile : Profile
    {
        public OrderMappingProfile()
        {
            CreateMap<Order, OrderResponse>().ReverseMap();
            CreateMap<Order, CheckOutOrderCommand>().ReverseMap();
            CreateMap<Order, UpdateOrderCommand>().ReverseMap();
            CreateMap<CheckOutOrderCommand, BasketCheckOutEvent>().ReverseMap();
            //CreateMap<Order, DeleteOrderCommand>().ReverseMap();
        }
    }
}
