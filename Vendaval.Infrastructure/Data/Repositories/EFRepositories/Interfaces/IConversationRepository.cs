using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vendaval.Domain.Entities;

namespace Vendaval.Infrastructure.Data.Repositories.EFRepositories.Interfaces
{
    public interface IConversationRepository
    {
        Task<Conversation> GetConversationByParticipantsAsync(int user1Id, int user2Id);
        Task<List<Conversation>> GetConversationsByUserIdAsync(int userId);
        Task AddMessageToConversation(int id, Message message);
        Task<Conversation> GetByIdAsync(int id);
        Task<List<Conversation>> GetAllAsync();
        IEnumerable<Conversation> GetWhere(Func<Conversation, bool> predicate);
        Task<Conversation> AddAsync(Conversation entity);
        void Update(int entityId, Conversation entity);
        void Delete(Conversation entity);
        Task<bool> Save();
    }
}
