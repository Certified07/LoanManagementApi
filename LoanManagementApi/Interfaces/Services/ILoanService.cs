using LoanManagementApi.RequestModel;
using LoanManagementApi.ResponseModel;

namespace LoanManagementApi.Interfaces.Services
{
    public interface ILoanService
    {
        Task<BaseResponse> ApplyAsync(LoanRequestModel model);
        Task<BaseResponse> ApproveAsync(string loanId);
        Task<BaseResponse> RejectAsync(string loanId);
        Task<BaseResponse> UpdateCreditScoreOnRepayment(string loanId, bool isDefaulted);
    }
}