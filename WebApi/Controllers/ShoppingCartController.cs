using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    public class ShoppingCartController : BaseApiController
    {
        private readonly IShoppingCartRepository _shoppingCartRepository;

        public ShoppingCartController(IShoppingCartRepository shoppingCartRepository)
        {
            _shoppingCartRepository = shoppingCartRepository;
        }

        [HttpGet]
        public async Task<ActionResult<ShoppingCart>> GetShoppingCartId(string id)
        {
            var cart = await _shoppingCartRepository.GetShoppingCartAsync(id);
            return Ok(cart ?? new ShoppingCart(id));
        }

        [HttpPost]
        public async Task<ActionResult<ShoppingCart>> UpdateShoppingCart(ShoppingCart cartParemeter)
        {
            var cartUpdated = await _shoppingCartRepository.UpdateShoppingCartAsync(cartParemeter);

            return Ok(cartUpdated);
        }

        [HttpDelete]
        public async Task<ActionResult<bool>> DeleteShoppingCart(string id)
        {
             return await _shoppingCartRepository.DeleteShoppingCartAsync(id);
        }
    }
}
