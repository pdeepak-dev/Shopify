using Order.Domain.Entities;
using System.Threading.Tasks;
using System.Collections.Generic;
using Order.Infrastructure.Persistence;
using Order.Application.Contracts.Persistence;

namespace Order.Infrastructure.Repositories
{
    public class OrderRepository : RepositoryBase<OrderEntity>, IOrderRepository
    {
        public OrderRepository(OrderContext ctx)
            : base(ctx)
        {
        }

        public async Task<IEnumerable<OrderEntity>> GetOrdersByUserName(string userName) 
            => await GetAsync(o => o.UserName == userName);
    }
}