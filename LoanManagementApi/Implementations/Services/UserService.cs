using LoanManagementApi.Interfaces.Repositories;
using LoanManagementApi.Interfaces.Services;
using LoanManagementApi.Models.Entities;
using LoanManagementApi.Models.Enums;
using LoanManagementApi.RequestModel;
using LoanManagementApi.ResponseModel;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Identity;
using System.Security.Cryptography;
using System.Text;

namespace LoanManagementApi.Implementations.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IClientRepository _clientRepository;

        private readonly SignInManager<MyUser> _signInManager;
        public UserService(IUserRepository userRepository, IClientRepository clientRepository, SignInManager<MyUser> signInManager)
        {
            _userRepository = userRepository;
            _clientRepository = clientRepository;
            _signInManager = signInManager;
        }
        public async Task<BaseResponse> RegisterAsync(RegisterUserRequestModel model)
        {
            var userNameCheck = await _userRepository.GetByUsernameAsync(model.Username);
            var emailCheck = await _userRepository.GetByEmailAsync(model.Email);
            if (userNameCheck != null)
            {
                return new BaseResponse
                {
                    Message = "Username already exist",
                    Status = false
                };
            }
            if (emailCheck != null)
            {
                return new BaseResponse
                {
                    Message = "Email already exist",
                    Status = false
                };
            }
            if (model.Age < 18)
            {
                return new BaseResponse
                {
                    Message = "You are not elligible to register to a loan app",
                    Status = false
                };
            }
            var user = new MyUser
            {
                Id = Guid.NewGuid().ToString(),
                UserName = model.Username,
                Email = model.Email,
                PasswordHash = HashPassword(model.Password),
                Role = UserRole.Client
            };
            var client = new Client
            {
                Id = Guid.NewGuid().ToString(),
                Name = model.Username,
                Email = model.Email,
                Phone = model.Phone,
                CreditScore = 50,
                Income = model.Income
            };
            await _clientRepository.CreateAsync(client);
            await _userRepository.CreateAsync(user);
            return new BaseResponse
            {
                Message = "Client successfully registered",
                Status = true
            };
        }
        public async Task<BaseResponse> LoginUser(LoginUserRequestModel model)
        {
            var user = await _userRepository.GetByEmailAsync(model.Email);
            if (user == null)
            {
                return new BaseResponse
                {
                    Message = "User not found",
                    Status = false
                };
            }
            if (user.PasswordHash != HashPassword(model.Password))
            {
                return new BaseResponse
                {
                    Message = "Invalid password",
                    Status = false
                };
            }
            await _signInManager.SignInAsync(user, isPersistent: false);
            user.IsLoggedIn = true;
            await _userRepository.UpdateAsync(user);
            return new BaseResponse
            {
                Message = "Sucessfully logged in",
                Status = true
            };
        }
        public static string HashPassword(string plainText)
        {
            var hashedPaswordBytes = SHA512.HashData(Encoding.UTF8.GetBytes(plainText));

            StringBuilder builder = new();
            foreach (var b in hashedPaswordBytes)
            {
                builder.Append(b.ToString("x2"));
            }

            return builder.ToString();
        }
    }
}
