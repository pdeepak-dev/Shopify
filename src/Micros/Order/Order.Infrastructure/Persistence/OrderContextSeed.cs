using System.Linq;
using Order.Domain.Entities;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace Order.Infrastructure.Persistence
{
    public class OrderContextSeed
    {
        public static async Task SeedAsync(OrderContext ctx, ILogger<OrderContextSeed> logger)
        {
            if (!ctx.Orders.Any())
            {
                ctx.Orders.AddRange(GetPreconfiguredOrders());
                await ctx.SaveChangesAsync();
                logger.LogInformation("Seed database associated with context {DbContextName}", typeof(OrderContext).Name);
            }
        }

        private static IEnumerable<OrderEntity> GetPreconfiguredOrders()
        {
            return new List<OrderEntity>
            {
                new OrderEntity()
                {
                    UserName = "swn",
                    FirstName = "Mehmet",
                    LastName = "Ozkaya",
                    Email = "ezozkme@gmail.com",
                    AddressLine = "Bahcelievler",
                    Country = "Turkey",
                    Price = 350
                }
            };
        }
    }
}