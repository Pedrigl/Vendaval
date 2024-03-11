using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vendaval.Application.ValueObjects;
using Vendaval.Application.ViewModels;

namespace Vendaval.Application.Services.Interfaces
{
    public interface IUserAddressViewModelService
    {
        Task<MethodResult<List<UserAddressViewModel>>> GetUserAddressByUserId(int userId);
        Task<MethodResult<UserAddressViewModel>> UpdateAddress(UserAddressViewModel userAddressViewModel);
        Task<MethodResult<UserAddressViewModel>> AddAddress(UserAddressViewModel userAddressViewModel);
        Task<MethodResult<UserAddressViewModel>> DeleteAddress(UserAddressViewModel userAddressViewModel);
    }
}
