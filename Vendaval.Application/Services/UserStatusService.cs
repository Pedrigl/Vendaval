using Vendaval.Application.Services.Interfaces;
using Vendaval.Application.ViewModels;

public class UserStatusService : IUserStatusService
{
    private readonly List<ChatUserViewModel> _onlineCostumers = new List<ChatUserViewModel>();
    private readonly List<ChatUserViewModel> _onlineSellers = new List<ChatUserViewModel>();
    private readonly object _costumersLock = new object();
    private readonly object _sellersLock = new object();

    public void AddOnlineCostumer(ChatUserViewModel chatUser)
    {
        lock (_costumersLock)
        {
            _onlineCostumers.Add(chatUser);
        }
    }

    public void RemoveOnlineCostumer(ChatUserViewModel chatUser)
    {
        lock (_costumersLock)
        {
            _onlineCostumers.Remove(chatUser);
        }
    }

    public void AddOnlineSeller(ChatUserViewModel chatUser)
    {
        lock (_sellersLock)
        {
            _onlineSellers.Add(chatUser);
        }
    }

    public void RemoveOnlineSeller(ChatUserViewModel chatUser)
    {
        lock (_sellersLock)
        {
            _onlineSellers.Remove(chatUser);
        }
    }

    public IEnumerable<ChatUserViewModel> GetOnlineSellers()
    {
        lock (_sellersLock)
        {
            return _onlineSellers.ToList();
        }
    }

    public IEnumerable<ChatUserViewModel> GetOnlineCostumers()
    {
        lock (_costumersLock)
        {
            return _onlineCostumers.ToList();
        }
    }
}
