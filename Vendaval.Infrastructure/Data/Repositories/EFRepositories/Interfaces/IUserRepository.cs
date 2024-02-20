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
        Task<User> GetById(int id);
        Task<List<User>> GetAll();
        User GetByEmailAndPassword(string email, string password);
        IEnumerable<User> GetWhere(Func<User, bool> predicate);
        Task AddAsync(User user);
        void Update(int entityId, User user);
        void Delete(User user);
    }
}
