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
        private readonly IRepaymentScheduleRepository _repaymentScheduleRepository;

        public RepaymentService(ILoanRepository loanRepository, IRepaymentRepository repaymentRepository,  IRepaymentScheduleRepository repaymentScheduleRepository)
        {
            _loanRepository = loanRepository;
            _repaymentRepository = repaymentRepository;
            _repaymentScheduleRepository = repaymentScheduleRepository;
        }
        public async Task<BaseResponse> GenerateFixedRepaymentScheduleAsync(string loanId, int durationInMonths)
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
            decimal totalamount = loan.TotalAmountToRepay;
            decimal monthlyPayment = totalamount / loan.DurationInMonths;
            var repayment = new Repayment
            {
                Id = Guid.NewGuid().ToString(),
                LoanId = loan.Id,
                TotalAmount = monthlyPayment * durationInMonths,
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
        public async Task<RepaymentSchedule> GenerateFlexibleRepaymentSchedule(Loan loan)
        {
            var repayment = new Repayment
            {
                Id = Guid.NewGuid().ToString(),
                LoanId = loan.Id,
                TotalAmount = loan.TotalAmountToRepay, 
                AmountPaid = 0,
                PaymentDate = loan.ApprovalDate.Value.AddMonths(loan.DurationInMonths),
                Status = PaymentStatus.Pending
            };

            await _repaymentRepository.CreateAsync(repayment);

            var schedule = new RepaymentSchedule
            {
                Id = Guid.NewGuid().ToString(),
                RepaymentId = repayment.Id,
                DueDate = repayment.PaymentDate,
                Amount = repayment.TotalAmount,
                Status = PaymentStatus.Pending
            };

            await _repaymentScheduleRepository.CreateAsync(schedule);

            return schedule;
        }
        public async Task<BaseResponse> MakePaymentAsync(MakeRepaymentRequestModel model)
        {
            var loan = await _loanRepository.GetByIdAsync(model.loanId);
            if (loan == null || loan.Repayment == null)
                return new BaseResponse { Message = "Loan not found or no repayment scheduled", Status = false };

            var repayment = loan.Repayment;

            if (loan.RepaymentType == RepaymentType.Flexible)
            {
                var schedule = repayment.RepaymentSchedules.First();
                schedule.AmountPaid += model.Amount;
                var amountLeft = schedule.Amount - schedule.AmountPaid;
                if (model.Amount > amountLeft)
                {
                    return new BaseResponse
                    {
                        Message = $"You just have {amountLeft} to pay",
                        Status = false
                    };
                }
                repayment.AmountPaid += model.Amount;
                loan.TotalPaid += model.Amount;
                if (schedule.AmountPaid == schedule.Amount)
                    schedule.Status = PaymentStatus.Paid;

                if (repayment.AmountPaid == repayment.TotalAmount)
                    repayment.Status = PaymentStatus.Paid;
                await _loanRepository.UpdateAsync(loan);
                await _repaymentScheduleRepository.UpdateAsync(schedule);
                await _repaymentRepository.UpdateAsync(repayment);
            }
            else
            {
                var nextSchedule = repayment.RepaymentSchedules
                    .OrderBy(s => s.DueDate)
                    .FirstOrDefault(s => s.Status == PaymentStatus.Pending);

                if (nextSchedule == null)
                    return new BaseResponse { Message = "All schedules already paid", Status = false };
                ApplyPenalty(nextSchedule);
                if (model.Amount < nextSchedule.Amount)
                    return new BaseResponse { Message = "Payment amount is less than expected installment", Status = false };
                
                nextSchedule.Status = PaymentStatus.Paid;
                nextSchedule.AmountPaid = nextSchedule.Amount;
                repayment.AmountPaid += nextSchedule.Amount;

                if (repayment.AmountPaid >= repayment.TotalAmount)
                    repayment.Status = PaymentStatus.Paid;

                await _repaymentScheduleRepository.UpdateAsync(nextSchedule);
                await _repaymentRepository.UpdateAsync(repayment);
            }

            return new BaseResponse
            {
                Message = "Payment successful",
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
