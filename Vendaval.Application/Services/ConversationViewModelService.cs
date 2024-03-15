using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vendaval.Application.Services.Interfaces;
using Vendaval.Application.ValueObjects;
using Vendaval.Application.ViewModels;
using Vendaval.Domain.Entities;
using Vendaval.Infrastructure.Data.Repositories.EFRepositories.Interfaces;

namespace Vendaval.Application.Services
{
    public class ConversationViewModelService : IConversationViewModelService
    {
        private readonly IConversationRepository _conversationRepository;
        private readonly IMapper _mapper;

        public ConversationViewModelService(IConversationRepository conversationRepository, IMapper mapper)
        {
            _conversationRepository = conversationRepository;
            _mapper = mapper;
        }

        public async Task<ConversationViewModel> CreateConversation(List<ChatUserViewModel> conversationParticipants)
        {
            var conversation = new Conversation
            {
                Participants = _mapper.Map<List<ChatUserViewModel>, List<ChatUser>>(conversationParticipants)
            };

            var createdConversation =await _conversationRepository.AddAsync(conversation);

            return _mapper.Map<Conversation, ConversationViewModel>(createdConversation);
        }
        public async Task AddMessageToConversationAsync(int conversationId, MessageViewModel message)
        {
            var mappedMessage = _mapper.Map<MessageViewModel, Message>(message);
            await _conversationRepository.AddMessageToConversation(conversationId, mappedMessage);
            await _conversationRepository.Save();
        }

        public async Task<ConversationViewModel> GetConversationByParticipantsAsync(int user1Id, int user2Id)
        {
            var conversation = await _conversationRepository.GetConversationByParticipantsAsync(user1Id, user2Id);
            return _mapper.Map<Conversation, ConversationViewModel>(conversation);
        }

        public IEnumerable<ConversationViewModel> GetUserConversations(int userId)
        {
            var conversations = _conversationRepository.GetConversationsByUserIdAsync(userId).Result;
            return _mapper.Map<IEnumerable<Conversation>, IEnumerable<ConversationViewModel>>(conversations);
        }
    }
}
