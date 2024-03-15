using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vendaval.Application.ViewModels;
using Vendaval.Domain.Entities;

namespace Vendaval.Application.Services.Interfaces
{
    public interface IConversationViewModelService
    {
        IEnumerable<ConversationViewModel> GetUserConversations(int userId);
        Task AddMessageToConversationAsync(int id, MessageViewModel message);
        Task<ConversationViewModel> GetConversationByParticipantsAsync(int user1Id, int user2Id);
    }
}
