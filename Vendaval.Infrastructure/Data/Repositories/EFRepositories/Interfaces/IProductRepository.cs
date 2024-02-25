using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vendaval.Domain.Entities;

namespace Vendaval.Infrastructure.Data.Repositories.EFRepositories.Interfaces
{
    public interface IProductRepository
    {
        Task<Product> GetByIdAsync(int id);
        Task<List<Product>> GetAll();
        IEnumerable<Product> GetWhere(Func<Product, bool> predicate);
        Task<Product> AddAsync(Product entity);
        void Update(int entityId, Product entity);
        void Delete(Product entity);
        void DeleteRange(IEnumerable<Product> entities);
        Task<bool> Save();
    }
}
