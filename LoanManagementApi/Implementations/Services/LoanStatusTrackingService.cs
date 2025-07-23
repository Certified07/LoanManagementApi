using LoanManagementApi.Interfaces.Repositories;
using LoanManagementApi.Interfaces.Services;
using LoanManagementApi.Models.Enums;
using LoanManagementApi.ResponseModel;

namespace LoanManagementApi.Implementations.Services
{
    public class LoanStatusTrackingService : ILoanStatusTrackingService
    {
        private readonly ILoanRepository _loanRepository;

        public LoanStatusTrackingService(ILoanRepository loanRepository)
        {
            _loanRepository = loanRepository;
        }
        public async Task<LoanStatusResponseModel> GetLoanStatusAsync(string loanId)
        {
            var loan = await _loanRepository.GetByIdAsync(loanId);
            if (loan == null)
            {
                return new LoanStatusResponseModel()
                {
                    Message = "Loan not found",
                    Status = false
                };

            }
            return new LoanStatusResponseModel
            {
                LoanId = loan.Id,
                ClientId = loan.ClientId,
                LoanStatus = loan.Status.ToString(),
                TotalPayment = (int)loan.TotalPaid,
                Status = true,
                Message = "Sucessfully returned"
            };

        }

    }
}
