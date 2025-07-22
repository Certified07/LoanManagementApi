using LoanManagementApi.RequestModel;
using LoanManagementApi.ResponseModel;

namespace LoanManagementApi.Interfaces.Services
{
    public interface IRepaymentService
    {
        Task<BaseResponse> GenerateFixedRepaymentScheduleAsync(string loanId, int durationInMonths);
        Task<RepaymentScheduleResponseModel> GetRepaymentSummaryAsync(string loanId);
        Task<BaseResponse> MakeRepaymentAsync(MakeRepaymentRequestModel model);
    }
}