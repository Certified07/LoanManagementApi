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
            var schedules = loan.Repayment?.RepaymentSchedules.ToList() ?? new List<Models.Entities.RepaymentSchedule>();
            int missed = schedules.Count(s => s.DueDate < DateTime.UtcNow && s.Status != PaymentStatus.Paid);
            var lastPayment = schedules
                .Where(s => s.Status == PaymentStatus.Paid)
                .OrderByDescending(s => s.PaymentDate)
                .FirstOrDefault()?.PaymentDate;
            decimal totalDue = schedules.Sum(s => s.Amount + s.Penalty);
            decimal totalPaid = schedules.Sum(s => s.AmountPaid);
            decimal outstanding = totalDue - totalPaid;
            string status = "OnTrack";
            if (missed > 0) status = "Overdue";
            if (missed >= 3) status = "Defaulted";
            return new LoanStatusResponseModel
            {
                LoanId = loan.Id,
                ClientId = loan.ClientId,
                MissedPayments = missed,
                LastPaymentDate = lastPayment,
                TotalOutstanding = outstanding,
                LoanStatus = status
            };

        }

    }
}
