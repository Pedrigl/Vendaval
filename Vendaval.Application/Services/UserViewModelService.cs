using AutoMapper;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
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
        private readonly List<string> _validIssuers;
        private readonly List<string> _validAudiences;

        public UserViewModelService(IUserRepository userRepository, IRedisRepository redisRepository, IMapper mapper, IConfiguration configuration)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _redisRepository = redisRepository ?? throw new ArgumentNullException(nameof(redisRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _jwtSecretKey = configuration["Jwt:Key"] ?? throw new ArgumentNullException(nameof(configuration));
            _validIssuers = configuration.GetSection("Jwt:Issuers").Get<List<string>>() ?? throw new ArgumentNullException(nameof(configuration));
            _validAudiences = configuration.GetSection("Jwt:Audience").Get<List<string>>() ?? throw new ArgumentNullException(nameof(configuration));
        }

        public async Task<LoginResult> Login(LoginDto login)
        {
            var result = new LoginResult();
            
            var loginOnCache = await GetLoginFromRedis(login);

            if (loginOnCache.Success)
                return loginOnCache;

            var user = _userRepository.GetByEmail(login.Email);

            var checkResult = CheckIfLoginIsSuccesfull(user, login);

            if (!checkResult.Success)
                return checkResult;

            result.User = _mapper.Map<UserViewModel>(user);

            result.Token= GenerateToken(user);

            result.TokenExpiration = TimeSpan.FromDays(1);

            await _redisRepository.SetValueAsync("UserEmail" + user.Email.ToString(), JsonConvert.SerializeObject(result));

            return result;
        }

        private async Task<LoginResult> GetLoginFromRedis(LoginDto login)
        {
            var result = new LoginResult();

            var loginOnCache = await _redisRepository.GetValueAsync(login.Email);

            if (loginOnCache.IsNullOrEmpty)
            {
                result.Success = false;
                result.Message = "Invalid login";
                return result;
            }
                

            result = JsonConvert.DeserializeObject<LoginResult>(loginOnCache);

            if (result.User.Email != login.Email || result.User.Password != login.Password)
                return new LoginResult { Success = false, Message = "Invalid login" };

            return result;
        }

        private LoginResult CheckIfLoginIsSuccesfull(User user, LoginDto login)
        {
            var result = new LoginResult();

            var encryptedPassword = HashPassword(login.Password);

            if (user == null)
            {
                result.Success = false;
                result.Message = "User not found";
                return result;
            }

            if (user.Password != encryptedPassword)
            {
                result.Success = false;
                result.Message = "Invalid password";
                return result;
            }

            return result;
        }

        private static string HashPassword(string password)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        private string GenerateToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSecretKey);
            SecurityTokenDescriptor tokenDescriptor = GenerateStd(user, key);
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private SecurityTokenDescriptor GenerateStd(User user, byte[] key)
        {
            return new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString()),
                    new Claim(ClaimTypes.Role, user.UserType.ToString())
                }),
                Issuer = _validIssuers.First(),
                Audience = _validAudiences.First(),
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

            mappedUser.BirthDate = mappedUser.BirthDate.ToUniversalTime();

            mappedUser.Password = HashPassword(mappedUser.Password);
            var newUser = await _userRepository.AddAsync(mappedUser);

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

        public async Task<bool> Logout(string email)
        {
            var token = await _redisRepository.GetValueAsync("UserEmail" + email);

            if (token.IsNullOrEmpty)
                return true;

            return await _redisRepository.RemoveValueAsync("UserEmail" + email);
        }

        public async Task<LoginResult> PutUser(UserViewModel user)
        {
            var mappedUser = _mapper.Map<User>(user);
            var oldUser = await _userRepository.GetByIdAsync(mappedUser.Id);

            if(oldUser == null)
                return new LoginResult { Success = false, Message = "User doesn't exist" };

            var passwordCheck = CheckIfPasswordIsValid(mappedUser.Password);

            if(!passwordCheck.Success)
                return passwordCheck;

            var emailCheck = CheckIfEmailIsValid(mappedUser.Email);

            if(!emailCheck.Success)
                return emailCheck;

            mappedUser.Password = HashPassword(mappedUser.Password);

            _userRepository.Update(mappedUser.Id, mappedUser);
            await _userRepository.Save();

            var newUser = await _userRepository.GetByIdAsync(mappedUser.Id);
            return new LoginResult { Success = true, Message = "User updated", User = _mapper.Map<UserViewModel>(newUser) };
        }

        public async Task<LoginResult> PatchUser(UserViewModel user)
        {
            var mappedUser = _mapper.Map<User>(user);
            var oldUser = await _userRepository.GetByIdAsync(mappedUser.Id);

            if(oldUser == null)
                return new LoginResult { Success = false, Message = "User doesn't exist" };

            

            if(!mappedUser.Password.IsNullOrEmpty())
            {
                var passwordCheck = CheckIfPasswordIsValid(mappedUser.Password);


                if (!passwordCheck.Success)
                    return passwordCheck;
            }

            if (!mappedUser.Email.IsNullOrEmpty())
            {
                var emailCheck = CheckIfEmailIsValid(mappedUser.Email);

                if (!emailCheck.Success)
                    return emailCheck;
            }
            
            return PatchUser(mappedUser, oldUser);
        }

        private LoginResult PatchUser(User patchedUser, User oldUser)
        {
            if (oldUser == null)
                return null;

            PatchProperties(patchedUser, oldUser);

            try
            {
                _userRepository.Update(oldUser.Id, oldUser);
                _userRepository.Save();
            }
            catch (Exception ex)
            {
                return new LoginResult { Success = false, Message = ex.Message };
            }

            return new LoginResult { Success = true, Message = "User updated", User = _mapper.Map<UserViewModel>(oldUser) };
        }

        private static void PatchProperties(User patchedUser, User oldUser)
        {
            oldUser.Name = string.IsNullOrEmpty(patchedUser.Name) ? oldUser.Name : patchedUser.Name;
            oldUser.Password = string.IsNullOrEmpty(patchedUser.Password) ? oldUser.Password : HashPassword(patchedUser.Password);
            oldUser.Email = string.IsNullOrEmpty(patchedUser.Email) ? oldUser.Email : patchedUser.Email;
            oldUser.PhoneNumber = string.IsNullOrEmpty(patchedUser.PhoneNumber) ? oldUser.PhoneNumber : patchedUser.PhoneNumber;

            if(oldUser.Address != null && oldUser.Address.Count > 0 && patchedUser.Address != null && patchedUser.Address.Count >0)
                oldUser.Address.AddRange(patchedUser.Address);
        }

        public async Task<LoginResult> DeleteLogin(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            
            if (user == null)
                return new LoginResult { Success = false, Message = "User not found" };

            try
            {
                _userRepository.Delete(user);
                await _userRepository.Save();
                return new LoginResult { Success = true, Message = "User deleted" };
            }
            catch (Exception ex)
            {
                return new LoginResult { Success = false, Message = ex.Message };
            }
        }


    }
}
