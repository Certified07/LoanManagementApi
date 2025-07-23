using LoanManagementApi.Interfaces.Repositories;
using LoanManagementApi.Interfaces.Services;
using LoanManagementApi.Models.Entities;
using LoanManagementApi.Models.Enums;
using LoanManagementApi.RequestModel;
using LoanManagementApi.ResponseModel;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace LoanManagementApi.Implementations.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IClientRepository _clientRepository;
        private readonly IConfiguration _config;

        public UserService(IUserRepository userRepository, IClientRepository clientRepository, IConfiguration config)
        {
            _userRepository = userRepository;
            _clientRepository = clientRepository;
            _config = config;
        }
        public async Task<BaseResponse> RegisterAsync(RegisterUserRequestModel model)
        {
            var userNameCheck = await _userRepository.GetByUsernameAsync(model.Name);
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
            var currentDate = DateOnly.FromDateTime(DateTime.UtcNow);
            var age = currentDate.Year - model.DateOfBirth.Year;
            if (age < 18)
            {
                return new BaseResponse
                {
                    Message = "You are not elligible to register to a loan app",
                    Status = false
                };
            }
            if (model.Income < 10000)
            {
                return new BaseResponse
                {
                    Message = "You must have a minimum income of 10,000",
                    Status = false
                };
            }
            var user = new MyUser
            {
                Id = Guid.NewGuid().ToString(),
                UserName = model.Name,
                Email = model.Email,
                PasswordHash = HashPassword(model.Password),
                Role = UserRole.Client
            };
            var client = new Client
            {
                Id = Guid.NewGuid().ToString(),
                Name = model.Name,
                Email = model.Email,
                Phone = model.PhoneNumber,
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
        public async Task<LoginResponseModel> LoginUser(LoginUserRequestModel model)
        {
            var user = await _userRepository.GetByEmailAsync(model.Email);
            if (user == null)
            {
                return new LoginResponseModel
                {
                    Message = "Invalid email or password",
                    Status = false
                };
            }
            if (user.PasswordHash != HashPassword(model.Password))
            {
                return new LoginResponseModel
                {
                    Message = "Invalid email or password",
                    Status = false
                };
            }
            var token = GenerateToken(user);
            return new LoginResponseModel
            {
                Token = token,
                Message = "Sucessfully logged in",
                Status = true
            };
        }
        public string GenerateToken(MyUser user)
        {
            var claims = new[]
            {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role.ToString())
        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(2),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
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
