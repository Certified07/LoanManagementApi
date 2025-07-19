using LoanManagementApi.ResponseModel;

namespace LoanManagementApi.Interfaces.Services
{
    public interface ILoanStatusTrackingService
    {
        Task<LoanStatusResponseModel> GetLoanStatusAsync(string loanId);
    }
}