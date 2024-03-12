using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vendaval.Application.Services.Interfaces
{
    internal interface IChatHubService
    {
        Task SendOnlineSellers();
        Task SendOnlineCostumers();
        Task OnConnectedAsync();
        Task OnDisconnectedAsync(Exception exception);
    }
}
