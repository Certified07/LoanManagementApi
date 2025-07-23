using LoanManagementApi.Models.Entities;
using LoanManagementApi.RequestModel;
using LoanManagementApi.ResponseModel;

namespace LoanManagementApi.Interfaces.Services
{
    public interface IRepaymentService
    {
        Task GenerateFixedRepaymentScheduleAsync(string loanId, int durationInMonths);
        Task<RepaymentScheduleResponseModel> GetRepaymentSummaryAsync(string loanId);

        Task<BaseResponse> MakePaymentAsync(MakeRepaymentRequestModel model);
        Task<List<RepaymentResponseModel>> GetHistoryByLoanIdAsync(string loanId);
        Task GenerateFlexibleRepaymentSchedule(Loan loan);

    }
}