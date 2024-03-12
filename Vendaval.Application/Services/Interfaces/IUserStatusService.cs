using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vendaval.Application.ViewModels;

namespace Vendaval.Application.Services.Interfaces
{
    public interface IUserStatusService
    {
        void AddOnlineCostumer(ChatUserViewModel chatUser);
        void RemoveOnlineCostumer(ChatUserViewModel chatUser);
        void AddOnlineSeller(ChatUserViewModel chatUser);
        void RemoveOnlineSeller(ChatUserViewModel chatUser);
        IEnumerable<ChatUserViewModel> GetOnlineSellers();
        IEnumerable<ChatUserViewModel> GetOnlineCostumers();
    }
}
