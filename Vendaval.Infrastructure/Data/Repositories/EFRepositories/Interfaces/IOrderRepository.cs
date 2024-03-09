using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vendaval.Domain.Entities;

namespace Vendaval.Infrastructure.Data.Repositories.EFRepositories.Interfaces
{
    public interface IOrderRepository
    {
        Task<Order> GetByIdAsync(int id);
        Task<List<Order>> GetAllAsync();
        IEnumerable<Order> GetWhere(Func<Order, bool> predicate);
        Task<Order> AddAsync(Order entity);
        void Update(int entityId, Order entity);
        void Delete(Order entity);
        void DeleteRange(IEnumerable<Order> entities);
        Task<bool> Save();
    }
}
