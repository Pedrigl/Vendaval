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

namespace Vendaval.Application.Services
{
    public class ChatHub : Hub
    {
        private readonly IUserStatusService _userStatusService;

        public ChatHub(IUserStatusService userStatusService)
        {
            _userStatusService = userStatusService;
        }

        public async Task SendPrivateMessage(string receiverId, MessageViewModel message)
        {
            var senderId = Context.User.FindFirst(ClaimTypes.NameIdentifier).Value;
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
            var userId = Context.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var role = Context.User.FindFirst(ClaimTypes.Role).Value;

            if (role == UserType.Costumer.ToString())
            {
                _userStatusService.AddOnlineCostumer(userId, Context.ConnectionId);
                await SendOnlineCostumers();
            }
            else if (role == UserType.Seller.ToString())
            {
                _userStatusService.AddOnlineSeller(userId, Context.ConnectionId);
                await SendOnlineSellers();
            }

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var userId = Context.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var role = Context.User.FindFirst(ClaimTypes.Role).Value;

            if (role == UserType.Costumer.ToString())
            {
                _userStatusService.RemoveOnlineCostumer(userId);
                await SendOnlineCostumers();
            }
            else if (role == UserType.Seller.ToString())
            {
                _userStatusService.RemoveOnlineSeller(userId);
                await SendOnlineSellers();
            }

            await base.OnDisconnectedAsync(exception);
        }
    }   
}
