using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Polly.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Vendaval.Application.Services.Interfaces;
using Vendaval.Application.ViewModels;
using Vendaval.Domain.Entities;
using Vendaval.Domain.Enums;
using Vendaval.Infrastructure.Data.Repositories.EFRepositories.Interfaces;

namespace Vendaval.Application.Services
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ChatHub : Hub
    {
        private readonly IChatUserViewModelService _chatUserViewModelService;
        private readonly IConversationViewModelService _conversationViewModelService;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        public ChatHub(IChatUserViewModelService chatUserViewModelService, IConversationViewModelService conversationViewModelService, IUserRepository userRepository, IMapper mapper)
        {
            _chatUserViewModelService = chatUserViewModelService;
            _conversationViewModelService = conversationViewModelService;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task SendUserConversations(ChatUserViewModel chatUser)
        {
            var conversations = _conversationViewModelService.GetUserConversations(chatUser.Id);

            await Clients.Client(chatUser.ConnectionId).SendAsync("ReceiveUserConversations", conversations);
        }

        public async Task CreateConversation(List<ChatUserViewModel> conversationParticipants)
        {
            var conversation = await _conversationViewModelService.GetConversationByParticipantsAsync(conversationParticipants[0].Id, conversationParticipants[1].Id);

            if (conversation == null)
            {
                conversation = await _conversationViewModelService.CreateConversationAsync(conversationParticipants);
            }

            foreach (var participant in conversationParticipants)
            {
                await SendUserConversations(participant);
            }
        }

        public async Task SendPrivateMessage(MessageViewModel message, List<ChatUserViewModel> conversationParticipants)
        {
            var conversation = await _conversationViewModelService.GetConversationByParticipantsAsync(conversationParticipants[0].Id, conversationParticipants[1].Id);

            await _conversationViewModelService.AddMessageToConversationAsync(conversation.Id, message);

            foreach (var participant in conversationParticipants)
            {
                await Clients.Client(participant.ConnectionId).SendAsync("ReceivePrivateMessage", message);
            }
            
        }
        
        public async Task SendOnlineSellers()
        {
            var onlineSellers = _chatUserViewModelService.GetOnlineSellers();
            await Clients.All.SendAsync("OnlineSellers", onlineSellers);
        }

        public async Task SendOnlineCostumers()
        {
            var onlineCostumers = _chatUserViewModelService.GetOnlineCustomers();
            await Clients.All.SendAsync("OnlineCostumers", onlineCostumers);
        }

        private async Task<User> GetCurrentUser()
        {
            var userId = Context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name).Value;
            return await _userRepository.GetByIdAsync(int.Parse(userId));
        }
        public override async Task OnConnectedAsync()
        {
            var user = await GetCurrentUser();
            var chatUser = _mapper.Map<ChatUserViewModel>(user);

            chatUser.ConnectionId = Context.ConnectionId;

            _chatUserViewModelService.ConnectChatUser(chatUser);

            if(chatUser.UserType == UserType.Costumer)
                await SendOnlineCostumers();

            if(chatUser.UserType == UserType.Seller)
                await SendOnlineSellers();


            await SendOwnUser(chatUser);
            await base.OnConnectedAsync();
        }

        public async Task SendOwnUser(ChatUserViewModel chatUser)
        {
            await Clients.Caller.SendAsync("OwnChatUser", chatUser);
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {   
            var user = await GetCurrentUser();

            var chatUser = _mapper.Map<ChatUserViewModel>(user);

            _chatUserViewModelService.DisconnectChatUser(chatUser);

            if (chatUser.UserType == UserType.Costumer)
                await SendOnlineCostumers();

            if (chatUser.UserType == UserType.Seller)
                await SendOnlineSellers();

            await base.OnDisconnectedAsync(exception);
        }
    }   
}
