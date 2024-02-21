using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vendaval.Application.ValueObjects;
using Vendaval.Application.ViewModels;

namespace Vendaval.Application.Services.Interfaces
{
    public interface IUserViewModelSerivce
    {
        Task<LoginResult> Login(LoginDto login);
        Task<bool> Logout(string email);
        Task<LoginResult> UpdateLogin(UserViewModel user);
        LoginResult DeleteLogin(UserViewModel userViewModel);
        Task<LoginResult> Register(UserViewModel user);
    }
}
