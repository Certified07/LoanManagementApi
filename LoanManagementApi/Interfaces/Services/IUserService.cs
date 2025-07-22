using LoanManagementApi.RequestModel;
using LoanManagementApi.ResponseModel;

namespace LoanManagementApi.Interfaces.Services
{
    public interface IUserService
    {
        Task<LoginResponseModel> LoginUser(LoginUserRequestModel model);
        Task<BaseResponse> RegisterAsync(RegisterUserRequestModel model);
    }
}