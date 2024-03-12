using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Polly.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Vendaval.Application.Services.Interfaces;
using Vendaval.Application.ViewModels;
using Vendaval.Domain.Enums;
using Vendaval.Infrastructure.Data.Repositories.EFRepositories.Interfaces;

namespace Vendaval.Application.Services
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ChatHub : Hub
    {
        private readonly IUserStatusService _userStatusService;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        public ChatHub(IUserStatusService userStatusService, IUserRepository userRepository, IMapper mapper)
        {
            _userStatusService = userStatusService;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task SendPrivateMessage(string receiverId, MessageViewModel message)
        {
            var senderId = Context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name).Value;
            var senderName = Context.User.Identity.Name;

            await Clients.Client(receiverId).SendAsync("ReceivePrivateMessage", senderId, senderName, message);
        }

        public async Task SendOnlineSellers()
        {
            var onlineSellers = _userStatusService.GetOnlineSellers();
            await Clients.Caller.SendAsync("OnlineSellers", onlineSellers);
        }

        public async Task SendOnlineCostumers()
        {
            var onlineCostumers = _userStatusService.GetOnlineCostumers();
            await Clients.Caller.SendAsync("OnlineCostumers", onlineCostumers);
        }

        public override async Task OnConnectedAsync()
        {

            var userId = Context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name).Value;
            
            var user = await _userRepository.GetByIdAsync(int.Parse(userId));
            var chatUser = _mapper.Map<ChatUserViewModel>(user);

            chatUser.ConnectionId = Context.ConnectionId;

            if (chatUser.UserType == UserType.Costumer)
            {
                if(!_userStatusService.GetOnlineCostumers().Any(c => c.Id == chatUser.Id))
                    _userStatusService.AddOnlineCostumer(chatUser);

                await SendOnlineCostumers();
            }

            if (chatUser.UserType == UserType.Seller)
            {
                if(!_userStatusService.GetOnlineSellers().Any(c => c.Id == chatUser.Id))
                    _userStatusService.AddOnlineSeller(chatUser);

                await SendOnlineSellers();
            }

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var userId = Context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name).Value;
            
            var user = await _userRepository.GetByIdAsync(int.Parse(userId));

            var chatUser = _mapper.Map<ChatUserViewModel>(user);

            if (chatUser.UserType == UserType.Costumer)
            {
                _userStatusService.RemoveOnlineCostumer(chatUser);
                await SendOnlineCostumers();
            }

            if (chatUser.UserType == UserType.Seller)
            {
                _userStatusService.RemoveOnlineSeller(chatUser);
                await SendOnlineSellers();
            }

            await base.OnDisconnectedAsync(exception);
        }
    }   
}
