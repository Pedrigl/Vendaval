﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vendaval.Application.ValueObjects;
using Vendaval.Application.ViewModels;

namespace Vendaval.Application.Services.Interfaces
{
    public interface IChatUserViewModelService
    {
        Task<MethodResult<ChatUserViewModel>> GetChatUserById(int id);
        Task<ChatUserViewModel> CreateChatUser(ChatUserViewModel chatUser);
        Task DisconnectChatUser(ChatUserViewModel chatUser);
        Task ConnectChatUser(ChatUserViewModel chatUser);
        MethodResult<IEnumerable<ChatUserViewModel>> GetOnlineUsers();
        IEnumerable<ChatUserViewModel> GetOnlineSellers();
        IEnumerable<ChatUserViewModel> GetOnlineCustomers();
    }
}
