using LoanManagementApi.DTOs;
using LoanManagementApi.Interfaces.Repositories;
using LoanManagementApi.Interfaces.Services;
using LoanManagementApi.Models.Entities;
using LoanManagementApi.Models.Enums;
using LoanManagementApi.RequestModel;
using LoanManagementApi.ResponseModel;

namespace LoanManagementApi.Implementations.Services
{
    public class RepaymentService : IRepaymentService
    {
        private readonly ILoanRepository _loanRepository;
        private readonly IRepaymentRepository _repaymentRepository;
        private readonly ILoanDurationRuleRepository _loanDurationRuleRepository;
        private readonly IRepaymentScheduleRepository _repaymentScheduleRepository;

        public RepaymentService(ILoanRepository loanRepository, IRepaymentRepository repaymentRepository, ILoanDurationRuleRepository loanDurationRuleRepository, IRepaymentScheduleRepository repaymentScheduleRepository)
        {
            _loanRepository = loanRepository;
            _repaymentRepository = repaymentRepository;
            _loanDurationRuleRepository = loanDurationRuleRepository;
            _repaymentScheduleRepository = repaymentScheduleRepository;
        }
        public async Task<BaseResponse> GenerateRepaymentScheduleAsync(string loanId, int durationInMonths)
        {
            var loan = await _loanRepository.GetByIdAsync(loanId);
            if (loan == null)
            {
                return new BaseResponse
                {
                    Message = "Loan not found",
                    Status = false
                };
            }
            if (loan.Repayment != null && loan.Repayment.RepaymentSchedules.Count != 0)
                return new BaseResponse
                {
                    Message = "Schedule already exist",
                    Status = false
                };
            decimal monthlyInterestRate = loan.InterestRate / 100m / 12m;
            decimal monthlyPayment = loan.PrincipalAmount * (monthlyInterestRate / (1 - (decimal)Math.Pow(1 + (double)monthlyInterestRate, -durationInMonths)));
            var repayment = new Repayment
            {
                Id = Guid.NewGuid().ToString(),
                LoanId = loan.Id,
                Amount = monthlyPayment * durationInMonths,
                CreatedAt = DateTime.UtcNow,
                Status = PaymentStatus.Pending
            };
            var schedules = new List<RepaymentSchedule>();
            var dueDate = loan.ApprovalDate.Value.AddMonths(1);

            for (int i = 0; i < durationInMonths; i++)
            {
                schedules.Add(new RepaymentSchedule
                {
                    Id = Guid.NewGuid().ToString(),
                    RepaymentId = repayment.Id,
                    DueDate = dueDate,
                    Amount = decimal.Round(monthlyPayment, 2),
                    Status = PaymentStatus.Pending
                });

                dueDate = dueDate.AddMonths(1);
            }
            repayment.RepaymentSchedules = schedules;
            await _repaymentRepository.CreateAsync(repayment);
            return new BaseResponse
            {
                Message = "Repayment schedule generated successfully",
                Status = true
            };

        }
        public async Task<BaseResponse> MakeRepaymentAsync(MakeRepaymentRequestModel model)
        {
            var schedule = await _repaymentScheduleRepository.GetByIdAsync(model.ScheduleId);
            if (schedule == null)
            {
                return new BaseResponse
                {
                    Message = "Repayment schedule not found",
                    Status = false
                };
            }
            if (schedule.IsPaid)
                return new BaseResponse
                {
                    Message = "Repayment schedule already paid",
                    Status = false
                };
            ApplyPenalty(schedule);
            decimal totalDue = schedule.Amount + schedule.Penalty;
            schedule.AmountPaid += model.Amount;

            if (schedule.AmountPaid >= totalDue && schedule.Penalty == 0)
            {
                schedule.Status = PaymentStatus.OnTime;
                schedule.PaymentDate = DateTime.UtcNow;
            }
            else if (schedule.AmountPaid >= totalDue && schedule.Penalty > 0)
            {
                schedule.Status = PaymentStatus.Overdue;
                schedule.PaymentDate = DateTime.UtcNow;
            }
            else
            {
                schedule.Status = PaymentStatus.PartiallyPaid;
            }
            await _repaymentScheduleRepository.UpdateAsync(schedule);
            return new BaseResponse
            {
                Message = "Payment sucessfully made",
                Status = true
            };
        }

        private void ApplyPenalty(RepaymentSchedule schedule)
        {
            if (schedule.IsPaid || schedule.DueDate >= DateTime.UtcNow.Date)
                return;
            var lastDate = schedule.LastPenaltyCalculationDate?.Date ?? schedule.DueDate.Date;
            var today = DateTime.UtcNow.Date;

            int overdueDays = (today - lastDate).Days;
            if (overdueDays <= 0) return;
            decimal penaltyToAdd = schedule.Amount * 0.05M * overdueDays;
            schedule.Penalty += decimal.Round(penaltyToAdd, 2);
            schedule.LastPenaltyCalculationDate = today;
        }
        public async Task<RepaymentScheduleResponseModel> GetRepaymentSummaryAsync(string loanId)
        {
            var repayment = await _repaymentRepository.GetByLoanId(loanId);
            if (repayment == null) return null;
            foreach (var item in repayment.RepaymentSchedules)
            {
                ApplyPenalty(item);
                await _repaymentScheduleRepository.UpdateAsync(item);
            }
            var totalAmount = repayment.RepaymentSchedules.Sum(s => s.Amount);
            var totalPenalty = repayment.RepaymentSchedules.Sum(s => s.Penalty);
            var totalPaid = repayment.RepaymentSchedules.Sum(s => s.AmountPaid);
            return new RepaymentScheduleResponseModel
            {
                LoanId = loanId,
                TotalAmount = totalAmount,
                TotalPenalty = totalPenalty,
                TotalPaid = totalPaid,
                Outstanding = (totalAmount + totalPenalty) - totalPaid,
                Schedules = repayment.RepaymentSchedules.Select(s => new RepaymentScheduleDTO
                {
                    ScheduleId = s.Id,
                    DueDate = s.DueDate.ToString("yyyy-MM-dd"),
                    Amount = s.Amount,
                    Penalty = s.Penalty,
                    AmountPaid = s.AmountPaid,
                    Status = s.Status.ToString()
                }).ToList()
            };

        }

    }
}
