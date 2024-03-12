using Vendaval.Application.Services.Interfaces;

public class UserStatusService : IUserStatusService
{
    private readonly Dictionary<string, string> _onlineCostumers = new Dictionary<string, string>();
    private readonly Dictionary<string, string> _onlineSellers = new Dictionary<string, string>();
    private readonly object _costumersLock = new object();
    private readonly object _sellersLock = new object();

    public void AddOnlineCostumer(string userId, string connectionId)
    {
        lock (_costumersLock)
        {
            _onlineCostumers.Add(userId, connectionId);
        }
    }

    public void RemoveOnlineCostumer(string userId)
    {
        lock (_costumersLock)
        {
            _onlineCostumers.Remove(userId);
        }
    }

    public void AddOnlineSeller(string userId, string connectionId)
    {
        lock (_sellersLock)
        {
            _onlineSellers.Add(userId, connectionId);
        }
    }

    public void RemoveOnlineSeller(string userId)
    {
        lock (_sellersLock)
        {
            _onlineSellers.Remove(userId);
        }
    }

    public IEnumerable<string> GetOnlineSellers()
    {
        lock (_sellersLock)
        {
            return _onlineSellers.Values.ToList();
        }
    }

    public IEnumerable<string> GetOnlineCostumers()
    {
        lock (_costumersLock)
        {
            return _onlineCostumers.Values.ToList();
        }
    }
}
