using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vendaval.Application.Services.Interfaces
{
    public interface IUserStatusService
    {
        void AddOnlineCostumer(string userId, string connectionId);
        void RemoveOnlineCostumer(string userId);
        void AddOnlineSeller(string userId, string connectionId);
        void RemoveOnlineSeller(string userId);
        IEnumerable<string> GetOnlineSellers();
        IEnumerable<string> GetOnlineCostumers();
    }
}
