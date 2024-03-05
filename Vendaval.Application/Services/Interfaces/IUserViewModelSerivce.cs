using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vendaval.Application.ValueObjects;
using Vendaval.Application.ViewModels;
using Vendaval.Domain.Entities;

namespace Vendaval.Application.Services.Interfaces
{
    public interface IUserViewModelSerivce
    {
        Task<LoginResult> Login(LoginDto login);
        Task<bool> Logout(string email);
        Task<LoginResult> PutUser(UserViewModel user);
        Task<LoginResult> PatchUser(UserViewModel userViewModel);
        Task<LoginResult> DeleteLogin(int id);
        Task<LoginResult> Register(UserViewModel user);
        Task<MethodResult<List<User>>> GetAllUsersAsync();
    }
}
