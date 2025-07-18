using LoanManagementApi.RequestModel;
using LoanManagementApi.ResponseModel;

namespace LoanManagementApi.Interfaces.Services
{
    public interface ILoanDurationRuleService
    {
        Task<BaseResponse> CreateRuleAsync(CreateRuleRequestModel model);
        Task<GetLoanDurationRuleResponseModel> FindByAmountAsync(decimal amount);
        Task<GetAllDurationRulesResponseModel> GetAllAsync();
        Task<GetLoanDurationRuleResponseModel> GetByIdAsync(string id);
    }
}