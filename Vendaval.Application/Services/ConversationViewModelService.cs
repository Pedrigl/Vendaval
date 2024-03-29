﻿using AutoMapper;
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

        public async Task<ConversationViewModel> GetConversationAsync(int conversationId)
        {
            var conversation = await _conversationRepository.GetByIdAsync(conversationId);

            return _mapper.Map<Conversation, ConversationViewModel>(conversation);
        }

        public async Task<ConversationViewModel> CreateConversationAsync(List<ChatUserViewModel> conversationParticipants)
        {
            var participantIds = conversationParticipants.Select(cp => cp.Id).ToList();
            var existingParticipants = new List<ChatUser>();

            foreach (var participantId in participantIds)
            {
                var participant = await _chatUserRepository.GetByIdAsync(participantId);
                if (participant != null)
                    existingParticipants.Add(participant);
            }

            var conversation = new Conversation
            {
                Participants = existingParticipants,
                Messages = new List<Message>()
            };

            var createdConversation =await _conversationRepository.AddAsync(conversation);

            return _mapper.Map<Conversation, ConversationViewModel>(createdConversation);
        }

        public async Task DeleteConversationAsync(Conversation conversation)
        {
            _conversationRepository.Delete(conversation);
            await _conversationRepository.Save();
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
