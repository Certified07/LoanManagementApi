using LoanManagementApi.ResponseModel;

namespace LoanManagementApi.Interfaces.Services
{
    public interface ILoanDurationRuleService
    {
        Task<BaseResponse> CreateRuleAsync(decimal min, decimal max, int duration);
        Task<GetLoanDurationRuleResponseModel> FindByAmountAsync(decimal amount);
        Task<GetAllDurationRulesResponseModel> GetAllAsync();
        Task<GetLoanDurationRuleResponseModel> GetByIdAsync(string id);
    }
}