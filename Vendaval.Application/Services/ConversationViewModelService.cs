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
using Vendaval.Infrastructure.Data.Repositories.EFRepositories;
using Vendaval.Infrastructure.Data.Repositories.EFRepositories.Interfaces;

namespace Vendaval.Application.Services
{
    public class ConversationViewModelService : IConversationViewModelService
    {
        private readonly IConversationRepository _conversationRepository;
        private readonly IChatUserRepository _chatUserRepository;
        private readonly IMapper _mapper;

        public ConversationViewModelService(IConversationRepository conversationRepository, IChatUserRepository chatUserRepository, IMapper mapper)
        {
            _conversationRepository = conversationRepository;
            _chatUserRepository = chatUserRepository;
            _mapper = mapper;
        }

        public async Task<ConversationViewModel> CreateConversationAsync(List<ChatUserViewModel> conversationParticipants)
        {
            var participantIds = conversationParticipants.Select(cp => cp.Id).ToList();
            var existingParticipants = _chatUserRepository.GetWhere(user => participantIds.Contains(user.Id)).ToList();
            var conversation = new Conversation
            {
                Participants = existingParticipants
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

        public async Task<IEnumerable<ConversationViewModel>> GetUserConversations(int userId)
        {
            var conversations = await _conversationRepository.GetConversationsByUserIdAsync(userId);
            return _mapper.Map<IEnumerable<Conversation>, IEnumerable<ConversationViewModel>>(conversations);
        }
    }
}
