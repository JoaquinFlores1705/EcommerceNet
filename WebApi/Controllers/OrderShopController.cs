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
        public async Task<ActionResult<OrderShop>> AddOrderShop(OrderShopDto orderShop)
        {
            var email = HttpContext.User?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.Email).Value;
            var direction = _mapper.Map<DirectionDto, Direction>(orderShop.ShippingDirection);
            var ordershop = await _orderShopService.AddOrderShopAsync(email, orderShop.ShippingType, orderShop.CartShopId, direction);

            if (orderShop == null) return BadRequest(new CodeErrorResponse(400, "Errores creando la orden de compra"));

            return Ok(ordershop);

        }
    }
}
