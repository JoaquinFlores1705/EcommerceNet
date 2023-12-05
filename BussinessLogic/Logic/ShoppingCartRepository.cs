using Core.Entities;
using Core.Interfaces;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BussinessLogic.Logic
{
    public class ShoppingCartRepository : IShoppingCartRepository
    {
        private readonly IDatabase _database;

        public ShoppingCartRepository(IConnectionMultiplexer redis)
        {
            _database = redis.GetDatabase();
        }

        public async Task<bool> DeleteShoppingCartAsync(string cartId)
        {
            return await _database.KeyDeleteAsync(cartId);
        }

        public async Task<ShoppingCart> GetShoppingCartAsync(string cartId)
        {
            var data = await _database.StringGetAsync(cartId);

            return data.IsNullOrEmpty ? null : JsonSerializer.Deserialize<ShoppingCart>(data);
        }

        public async Task<ShoppingCart> UpdateShoppingCartAsync(ShoppingCart shoppingCart)
        {
            var status = await _database.StringSetAsync(shoppingCart.Id, JsonSerializer.Serialize(shoppingCart), TimeSpan.FromDays(1));
            if(!status) return null;
            return await GetShoppingCartAsync(shoppingCart.Id);
        }
    }
}
