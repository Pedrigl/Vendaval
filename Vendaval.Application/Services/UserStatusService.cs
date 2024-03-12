using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vendaval.Application.Services.Interfaces;

namespace Vendaval.Application.Services
{
    public class UserStatusService : IUserStatusService
    {
        private readonly Dictionary<string, string> _onlineCostumers = new Dictionary<string, string>();
        private readonly Dictionary<string, string> _onlineSellers = new Dictionary<string, string>();

        public void AddOnlineCostumer(string userId, string connectionId)
        {
            _onlineCostumers.Add(userId, connectionId);
        }

        public void RemoveOnlineCostumer(string userId)
        {
            _onlineCostumers.Remove(userId);
        }

        public void AddOnlineSeller(string userId, string connectionId)
        {
            _onlineSellers.Add(userId, connectionId);
        }

        public void RemoveOnlineSeller(string userId)
        {
            _onlineSellers.Remove(userId);
        }

        public IEnumerable<string> GetOnlineSellers()
        {
            return _onlineSellers.Values;
        }

        public IEnumerable<string> GetOnlineCostumers()
        {
            return _onlineCostumers.Values;
        }
        
    }
}
