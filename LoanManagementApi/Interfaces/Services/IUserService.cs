using LoanManagementApi.RequestModel;
using LoanManagementApi.ResponseModel;

namespace LoanManagementApi.Interfaces.Services
{
    public interface IUserService
    {
        static abstract string HashPassword(string plainText);
        Task<BaseResponse> LoginUser(LoginUserRequestModel model);
        Task<BaseResponse> RegisterAsync(RegisterUserRequestModel model);
    }
}