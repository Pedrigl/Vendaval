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
using Vendaval.Domain.Enums;
using Vendaval.Infrastructure.Data.Repositories.EFRepositories.Interfaces;

namespace Vendaval.Application.Services
{
    public class ChatUserViewModelService : IChatUserViewModelService
    {
        private readonly IChatUserRepository _chatUserRepository;
        private readonly IMapper _mapper;

        public ChatUserViewModelService(IChatUserRepository chatUserRepository, IMapper mapper)
        {
            _chatUserRepository = chatUserRepository;
            _mapper = mapper;
        }

        public async Task CreateChatUser(ChatUserViewModel chatUser)
        {

            var chatUserExists = (await _chatUserRepository.GetByIdAsync(chatUser.Id)) != null;
            var mappedUser = _mapper.Map<ChatUserViewModel,ChatUser>(chatUser);

            if (!chatUserExists)
                await _chatUserRepository.AddAsync(mappedUser);
        }

        public void DisconnectChatUser(ChatUserViewModel chatUser)
        {
            var mappedUser = _mapper.Map<ChatUserViewModel, ChatUser>(chatUser);
            mappedUser.IsOnline = false;
            _chatUserRepository.Update(mappedUser.Id,mappedUser);
        }

        public void ConnectChatUser(ChatUserViewModel chatUser)
        {
            var mappedUser = _mapper.Map<ChatUserViewModel, ChatUser>(chatUser);
            mappedUser.IsOnline = true;
            _chatUserRepository.Update(mappedUser.Id, mappedUser);
            _chatUserRepository.Save();
        }

        public IEnumerable<ChatUserViewModel> GetOnlineUsers()
        {
            var chatUsers = _chatUserRepository.GetWhere(u => u.IsOnline == true);
            return _mapper.Map<IEnumerable<ChatUser>, IEnumerable<ChatUserViewModel>>(chatUsers);
        }

        public IEnumerable<ChatUserViewModel> GetOnlineCustomers()
        {
            var onlineUsers = _chatUserRepository.GetWhere(u => u.IsOnline == true || u.UserType == UserType.Costumer);

            return _mapper.Map<IEnumerable<ChatUser>, IEnumerable<ChatUserViewModel>>(onlineUsers);
        }

        public IEnumerable<ChatUserViewModel> GetOnlineSellers()
        {
            var onlineUsers = _chatUserRepository.GetWhere(u => u.IsOnline == true || u.UserType == UserType.Seller);

            return _mapper.Map<IEnumerable<ChatUser>, IEnumerable<ChatUserViewModel>>(onlineUsers);
        }
    }
}
