using AutoMapper;
using Core.Entities.OrderShop;
using Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Linq;
using System.Security.Claims;
using WebApi.Dtos;
using WebApi.Errors;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;

namespace WebApi.Controllers
{
    [Authorize]
    public class OrderShopController : BaseApiController
    {

        private readonly IOrderShopService _orderShopService;
        private readonly IMapper _mapper;
        public OrderShopController(IOrderShopService orderShopService, IMapper mapper)
        {
            _orderShopService = orderShopService;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult<orderShopResponseDto>> AddOrderShop(OrderShopDto orderShop)
        {
            var email = HttpContext.User?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.Email).Value;
            var direction = _mapper.Map<DirectionDto, Direction>(orderShop.ShippingDirection);
            var ordershop = await _orderShopService.AddOrderShopAsync(email, orderShop.ShippingType, orderShop.CartShopId, direction);

            if (orderShop == null) return BadRequest(new CodeErrorResponse(400, "Errores creando la orden de compra"));

            return Ok(_mapper.Map<OrderShop,orderShopResponseDto>(ordershop));

        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<orderShopResponseDto>>> GetOrderShop()
        {
            var email = HttpContext.User?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.Email).Value;
            var ordersshop = await _orderShopService.GetOrderShopByUserEmailAsync(email);
            return Ok(_mapper.Map<IReadOnlyList<OrderShop>, IReadOnlyList<orderShopResponseDto>>(ordersshop));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<orderShopResponseDto>> GetOrderShopById(int id)
        {
            var email = HttpContext.User?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.Email).Value;
            var orderShop = await _orderShopService.GetOrderShopOrderByIdAsync(id, email);
            if(orderShop == null)
            {
                return NotFound(new CodeErrorResponse(404, "No se encontro la orden de compra"));
            }
            return _mapper.Map<OrderShop, orderShopResponseDto>(orderShop);
        }

        [HttpGet("shippingtype")]
        public async Task<ActionResult<IReadOnlyList<ShippingType>>> GetShippingsType()
        {
            return Ok(await _orderShopService.GetShippingTypeAsync());
        }

    }
}
