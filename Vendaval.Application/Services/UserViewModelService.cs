using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Vendaval.Application.ValueObjects;
using Vendaval.Application.ViewModels;
using Vendaval.Domain.Entities;
using Vendaval.Infrastructure.Data.Repositories.EFRepositories.Interfaces;
using Vendaval.Infrastructure.Data.Repositories.RedisRepositories.Interfaces;

namespace Vendaval.Application.Services
{
    public class UserViewModelService : IUserViewModelSerivce
    {
        private readonly IUserRepository _userRepository;
        private readonly IRedisRepository _redisRepository;
        private readonly IMapper _mapper;
        private readonly string _jwtSecretKey;

        public UserViewModelService(IUserRepository userRepository, IRedisRepository redisRepository, IMapper mapper)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _redisRepository = redisRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<LoginResult> Login(LoginViewModel loginViewModel)
        {
            var result = new LoginResult();

            var user = _userRepository.GetByEmail(loginViewModel.Email);

            var checkResult = CheckIfLoginIsSuccesfull(user, loginViewModel);

            if (!checkResult.Success)
                return result;

            result.User = _mapper.Map<UserViewModel>(user);
            result.Token= GenerateToken(user);

            await _redisRepository.SetValueAsync("UserTokenId" + user.Id.ToString(), result.Token);

            return result;
        }

        private LoginResult CheckIfLoginIsSuccesfull(User user, LoginViewModel loginViewModel)
        {
            var result = new LoginResult();

            if (user == null)
            {
                result.Success = false;
                result.Message = "User not found";
                return result;
            }

            if (user.Password != loginViewModel.Password)
            {
                result.Success = false;
                result.Message = "Invalid password";
                return result;
            }

            return result;
        }
        private string GenerateToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSecretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString()),
                    new Claim(ClaimTypes.Role, user.UserType.ToString())
                }),

                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        


    }
}
