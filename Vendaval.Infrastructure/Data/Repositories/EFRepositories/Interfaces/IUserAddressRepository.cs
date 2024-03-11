using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vendaval.Domain.Entities;

namespace Vendaval.Infrastructure.Data.Repositories.EFRepositories.Interfaces
{
    public interface IUserAddressRepository
    {
        Task<UserAddress> GetWhere(Func<UserAddress, bool> predicate);
        Task<UserAddress> AddAsync(UserAddress userAddress);
        void Update(int entityId, UserAddress userAddress);
        void Delete(UserAddress userAddress);
        Task<bool> Save();
    }
}
