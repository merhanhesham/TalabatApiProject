using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Repositories;

namespace Talabat.Repository
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDatabase _database;
        public BasketRepository(IConnectionMultiplexer Redis)//ask clr for object from class implements IConnectionMultiplexer
        {
            _database=Redis.GetDatabase();
        }
        public async Task<bool> DeleteBasketAsync(string BasketId)
        {
            //redis 3bara 3n dictionary, key & value, so param is key
            return await _database.KeyDeleteAsync(BasketId);
            
        }

        public async Task<CustomerBasket?> GetBasketAsync(string BasketId)
        {
            var Basket= await _database.StringGetAsync(BasketId);//this func returns the value of key in redis, ymkn mykonsh elvalue CustomerBasket f lazm a2kdlo
            //and i can't return basket as it is because it's of diff type than the return type of function
            /*if (Basket.IsNull) return null;
            else
                return JsonSerializer.Deserialize<CustomerBasket>(Basket);*/

            return Basket.IsNull? null : JsonSerializer.Deserialize<CustomerBasket>(Basket);
        }

        public async Task<CustomerBasket?> UpdateBasketAsync(CustomerBasket Basket)
        {
            //since the function takes param of type redis, i need to serialize Basket first
            var serializedBasket = JsonSerializer.Serialize(Basket);
            //update basket
            //if this basket exists update, if not create 
            var CreatedOrUpdated = await _database.StringSetAsync(Basket.Id, serializedBasket, TimeSpan.FromDays(1));//returns T/F
            if (!CreatedOrUpdated) return null;//means basket isn't created or updated, it's null
            else
            {
               return await GetBasketAsync(Basket.Id);//return basket after updated
            }
        }
    }
}
