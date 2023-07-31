using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Errors;
using API.Extensions;
using AutoMapper;
using Core.Entities.OrderAgregate;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    public class OrdersController: BaseApiController
    {

        public IOrderService _orderService { get; }
        public IMapper _mapper { get; }

        public OrdersController(IOrderService orderService,IMapper mapper)
        {
            _mapper = mapper;
            _orderService = orderService;
            
        }
        
        [HttpPost]
        public async Task<ActionResult<Order>> CreateOeder(OrderDto orderDto)
        {
            var email = HttpContext.User.RetrieveEmailFromPrincipal();
            var address = _mapper.Map<AddressDto,Address>(orderDto.ShipToAddress);
            var order = await _orderService.CreateOrderAsync( email, orderDto.DeliveryMethodId,orderDto.BaskerId, address);
            
            if (order == null) return BadRequest(new ApiResponse(400,"Problem creating order"));

            return Ok(order);
        }
    }
}