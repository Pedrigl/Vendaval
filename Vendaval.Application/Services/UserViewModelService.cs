using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Vendaval.Application.Services.Interfaces;
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

            result.TokenExpiration = TimeSpan.FromDays(1);

            await _redisRepository.SetValueAsync("UserLogin" + user.Id.ToString(), JsonConvert.SerializeObject(result));

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
            SecurityTokenDescriptor tokenDescriptor = GenerateStd(user, key);
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private static SecurityTokenDescriptor GenerateStd(User user, byte[] key)
        {
            return new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString()),
                    new Claim(ClaimTypes.Role, user.UserType.ToString())
                }),

                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
        }

        public async Task<LoginResult> Register(UserViewModel user)
        {
            var mappedUser = _mapper.Map<User>(user);

            var passwordCheck = CheckIfPasswordIsValid(mappedUser.Password);

            if(!passwordCheck.Success)
                return passwordCheck;

            var emailCheck = CheckIfEmailIsValid(mappedUser.Email);

            if(!emailCheck.Success)
                return emailCheck;

            
            if (CheckIfUserExists(mappedUser))
                return new LoginResult { Success = false, Message = "User already exists" };

            await _userRepository.AddAsync(mappedUser);

            await _userRepository.Save();
            
            var newUser = _userRepository.GetByEmail(mappedUser.Email);

            var result = CheckIfRegistrationIsSuccesfull(newUser);

            return result;
        }
        
        private LoginResult CheckIfEmailIsValid(string email)
        {
            var result = new LoginResult();

            if (!email.Contains("@") || !email.Contains(".com"))
            {
                result.Success = false;
                result.Message = "Invalid email";
                return result;
            }

            return result;
        }
        private LoginResult CheckIfPasswordIsValid(string password)
        {
            var result = new LoginResult();

            if (password.Length < 8)
            {
                result.Success = false;
                result.Message = "Password must be at least 8 characters long";
                return result;
            }

            var requirements = new List<(Func<char, bool> condition, string message)>
            {
                (char.IsUpper, "Password must contain at least one uppercase letter"),
                (char.IsLower, "Password must contain at least one lowercase letter"),
                (char.IsDigit, "Password must contain at least one number")
            };

            foreach (var (condition, message) in requirements)
            {
                if (!password.Any(condition))
                {
                    result.Success = false;
                    result.Message = message;
                    return result;
                }
            }

            return result;
        }

        private bool CheckIfUserExists(User user)
        {
            return _userRepository.GetWhere(u => u.Email == user.Email).Any();
        }

        private LoginResult CheckIfRegistrationIsSuccesfull(User user)
        {
            var result = new LoginResult();

            if (user == null)
            {
                result.Success = false;
                result.Message = "User not found";
                return result;
            }

            result.User = _mapper.Map<UserViewModel>(user);

            return result;
        }

        public async Task<bool> Logout(int userId)
        {
            var token = await _redisRepository.GetValueAsync("UserTokenId" + userId.ToString());

            if (token.IsNullOrEmpty)
                return false;

            return await _redisRepository.RemoveValueAsync("UserTokenId" + userId.ToString());
        }

    }
}
