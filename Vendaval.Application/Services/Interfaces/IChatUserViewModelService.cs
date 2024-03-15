using System;
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
        Task CreateChatUser(ChatUserViewModel chatUser);
        void DisconnectChatUser(ChatUserViewModel chatUser);
        void ConnectChatUser(ChatUserViewModel chatUser);
        IEnumerable<ChatUserViewModel> GetOnlineUsers();
        IEnumerable<ChatUserViewModel> GetOnlineSellers();
        IEnumerable<ChatUserViewModel> GetOnlineCustomers();
    }
}
