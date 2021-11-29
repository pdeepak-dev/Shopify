﻿using System;
using Newtonsoft.Json;
using Basket.API.Entities;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;

namespace Basket.API.Repositories
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDistributedCache _redisCache;

        public BasketRepository(IDistributedCache redisCache)
            => _redisCache = redisCache ?? throw new ArgumentNullException(nameof(redisCache));

        public async Task DeleteBasketAsync(string userName)
            => await _redisCache.RemoveAsync(userName);

        public async Task<ShoppingCart> GetBasketAsync(string userName)
        {
            var basket = await _redisCache.GetStringAsync(userName);

            if (!string.IsNullOrWhiteSpace(basket))
                return null;

            return JsonConvert.DeserializeObject<ShoppingCart>(basket);
        }

        public async Task<ShoppingCart> UpdateBasketAsync(ShoppingCart basket)
        {
            await _redisCache.SetStringAsync(basket.UserName, JsonConvert.SerializeObject(basket));
            return await GetBasketAsync(basket.UserName);
        }
    }
}
