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

        public async Task<MethodResult<ChatUserViewModel>> GetChatUserById(int id)
        {
            var chatUser = await _chatUserRepository.GetByIdAsync(id);

            if (chatUser == null)
            {
                return new MethodResult<ChatUserViewModel> { Success = false, Message = "ChatUser was not found" };
            }

            var chatUserViewModel = _mapper.Map<ChatUser, ChatUserViewModel>(chatUser);
            return new MethodResult<ChatUserViewModel> { Success = true, data = chatUserViewModel };
        }
        public async Task<ChatUserViewModel> CreateChatUser(ChatUserViewModel chatUser)
        {
            var mappedUser = _mapper.Map<ChatUserViewModel,ChatUser>(chatUser);

            await _chatUserRepository.AddAsync(mappedUser);

            return _mapper.Map<ChatUser, ChatUserViewModel>(mappedUser);
        }

        public async Task DisconnectChatUser(ChatUserViewModel chatUser)
        {
            var mappedUser = _mapper.Map<ChatUserViewModel, ChatUser>(chatUser);
            mappedUser.IsOnline = false;
            _chatUserRepository.Update(mappedUser.Id,mappedUser);
            await _chatUserRepository.Save();
        }

        public async Task ConnectChatUser(ChatUserViewModel chatUser)
        {
            var mappedUser = _mapper.Map<ChatUserViewModel, ChatUser>(chatUser);
            mappedUser.IsOnline = true;
            _chatUserRepository.Update(mappedUser.Id, mappedUser);
            await _chatUserRepository.Save();
        }

        public MethodResult<IEnumerable<ChatUserViewModel>> GetOnlineUsers()
        {
            var chatUsers = _chatUserRepository.GetWhere(u => u.IsOnline == true).ToList();

            if (chatUsers == null)
            {
                return new MethodResult<IEnumerable<ChatUserViewModel>> { Success = false, Message = "No online users found" };
            }
            var onlineUsersMapped = _mapper.Map<IEnumerable<ChatUser>, IEnumerable<ChatUserViewModel>>(chatUsers);
            return new MethodResult<IEnumerable<ChatUserViewModel>> { Success = true, data = onlineUsersMapped };
        }

        public IEnumerable<ChatUserViewModel> GetOnlineCustomers()
        {
            var onlineUsers = _chatUserRepository.GetWhere(u => u.IsOnline == true || u.UserType == UserType.Costumer).ToList();

            return _mapper.Map<IEnumerable<ChatUser>, IEnumerable<ChatUserViewModel>>(onlineUsers);
        }

        public IEnumerable<ChatUserViewModel> GetOnlineSellers()
        {
            var onlineUsers = _chatUserRepository.GetWhere(u => u.IsOnline == true || u.UserType == UserType.Seller).ToList();

            return _mapper.Map<IEnumerable<ChatUser>, IEnumerable<ChatUserViewModel>>(onlineUsers);
        }
    }
}
