using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vendaval.Domain.Entities;

namespace Vendaval.Infrastructure.Data.Repositories.EFRepositories.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetByIdAsync(int id);
        Task<List<User>> GetAll();
        User GetByEmail(string email);
        IEnumerable<User> GetWhere(Func<User, bool> predicate);
        Task<User> AddAsync(User user);
        void Update(int entityId, User user);
        void Delete(User user);
        Task<bool> Save();
    }
}
