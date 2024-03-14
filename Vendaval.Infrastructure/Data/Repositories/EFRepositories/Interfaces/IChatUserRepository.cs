using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vendaval.Domain.Entities;

namespace Vendaval.Infrastructure.Data.Repositories.EFRepositories.Interfaces
{
    public interface IChatUserRepository
    {
        Task<ChatUser> GetByIdAsync(int id);
        Task<List<ChatUser>> GetAllAsync();
        IEnumerable<ChatUser> GetWhere(Func<ChatUser, bool> predicate);
        Task<ChatUser> AddAsync(ChatUser entity);
        void Update(int entityId, ChatUser entity);
        void Delete(ChatUser entity);
        Task<bool> Save();
    }
}
