using AutoMapper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vendaval.Application.Services.Interfaces;
using Vendaval.Application.ValueObjects;
using Vendaval.Application.ViewModels;
using Vendaval.Domain.Entities;
using Vendaval.Infrastructure.Data.Repositories.EFRepositories.Interfaces;
using Vendaval.Infrastructure.Data.Repositories.RedisRepositories.Interfaces;

namespace Vendaval.Application.Services
{
    public class UserAddressViewModelService : IUserAddressViewModelService
    {
        private readonly IUserAddressRepository _userAddressRepository;
        private readonly IRedisRepository _redisRepository;
        private readonly IMapper _mapper;

        public UserAddressViewModelService(IUserAddressRepository userAddressRepository, IMapper mapper, IRedisRepository redisRepository)
        {
            _userAddressRepository = userAddressRepository;
            _mapper = mapper;
            _redisRepository = redisRepository;
        }

        private async Task<MethodResult<UserAddressViewModel>> GetUserAddressFromCache(int userId)
        {
            var userAddress = await _redisRepository.GetValueAsync($"AddressUserId{userId}");
            
            if (userAddress.IsNullOrEmpty)
                return new MethodResult<UserAddressViewModel> { Success = false, Message = "No addresses on cache"};

            var parsedUserAddress = JsonConvert.DeserializeObject<UserAddressViewModel>(userAddress);

            return new MethodResult<UserAddressViewModel> { Success = true, data = parsedUserAddress };
        }

        private async Task SaveAndClearCache(int userId)
        {
            await _redisRepository.RemoveValueAsync($"AddressUserId{userId}");
            await _userAddressRepository.Save();
        }

        public async Task<MethodResult<List<UserAddressViewModel>>> GetUserAddressByUserId(int userId)
        {
            var userAddressFromCache = await GetUserAddressFromCache(userId);

            if (userAddressFromCache.Success)
                return new MethodResult<List<UserAddressViewModel>> { Success = true, data = new List<UserAddressViewModel> { userAddressFromCache.data } };

            var userAddress = _userAddressRepository.GetWhere(a=> a.UserId == userId);
            var mappedUserAddress = _mapper.Map<List<UserAddressViewModel>>(userAddress);

            if (mappedUserAddress.Count == 0)
                return new MethodResult<List<UserAddressViewModel>> { Success = false, Message = "No Addresses were found for this user"};

            await _redisRepository.SetValueAsync($"AddressUserId{userId}", JsonConvert.SerializeObject(mappedUserAddress));

            return new MethodResult<List<UserAddressViewModel>> { Success = true, data = mappedUserAddress };
        }

        public async Task<MethodResult<UserAddressViewModel>> UpdateAddress(UserAddressViewModel userAddressViewModel)
        {
            var userAddress = _mapper.Map<UserAddress>(userAddressViewModel);

            try
            {
                _userAddressRepository.Update(userAddress.Id, userAddress);
                await SaveAndClearCache(userAddress.Id);
                return new MethodResult<UserAddressViewModel> { Success = true, data = userAddressViewModel };
            }
            catch (Exception ex)
            {
                return new MethodResult<UserAddressViewModel> { Success = false, Message = ex.Message };
            }
        }

        public async Task<MethodResult<UserAddressViewModel>> AddAddress(UserAddressViewModel userAddressViewModel)
        {
            var userAddress = _mapper.Map<UserAddress>(userAddressViewModel);

            try
            {
                var address = await _userAddressRepository.AddAsync(userAddress);

                return new MethodResult<UserAddressViewModel> { Success = true, data = _mapper.Map<UserAddress, UserAddressViewModel>(address)};
            }
            catch (Exception ex)
            {
                return new MethodResult<UserAddressViewModel> { Success = false, Message = ex.Message };
            }
        }

        public async Task<MethodResult<UserAddressViewModel>> DeleteAddress(UserAddressViewModel userAddress)
        {
            var mappedAddress = _mapper.Map<UserAddress>(userAddress);
            try
            {
                _userAddressRepository.Delete(mappedAddress);
                await SaveAndClearCache(mappedAddress.UserId);
                return new MethodResult<UserAddressViewModel> { Success = true, Message = "Address was deleeted successfully" };
            }
            catch (Exception ex)
            {
                return new MethodResult<UserAddressViewModel> { Success = false, Message = ex.Message };
            }
        }
    }
}
