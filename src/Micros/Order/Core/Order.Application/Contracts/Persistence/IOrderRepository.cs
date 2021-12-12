using Order.Domain.Entities;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Order.Application.Contracts.Persistence
{
    public interface IOrderRepository : IAsyncRepository<OrderEntity>
    {
        Task<IEnumerable<OrderEntity>> GetOrdersByUserName(string userName);
    }
}