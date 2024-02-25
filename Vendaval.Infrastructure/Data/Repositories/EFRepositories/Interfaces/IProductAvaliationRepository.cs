using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vendaval.Domain.Entities;

namespace Vendaval.Infrastructure.Data.Repositories.EFRepositories.Interfaces
{
    public interface IProductAvaliationRepository
    {
        Task<ProductAvaliation> GetByIdAsync(int id);
        Task<List<ProductAvaliation>> GetAll();
        IEnumerable<ProductAvaliation> GetWhere(Func<ProductAvaliation, bool> predicate);
        Task<ProductAvaliation> AddAsync(ProductAvaliation entity);
        void Update(int entityId, ProductAvaliation entity);
        void Delete(ProductAvaliation entity);
        void DeleteRange(IEnumerable<ProductAvaliation> entities);
        Task<bool> Save();
    }
}
