using LoanManagementApi.DTOs;
using LoanManagementApi.Implementations.Repositories;
using LoanManagementApi.Interfaces.Repositories;
using LoanManagementApi.Interfaces.Services;
using LoanManagementApi.Models.Entities;
using LoanManagementApi.Models.Enums;
using LoanManagementApi.RequestModel;
using LoanManagementApi.ResponseModel;
using System.Security.Claims;

namespace LoanManagementApi.Implementations.Services
{
    public class RepaymentService : IRepaymentService
    {
        private readonly ILoanRepository _loanRepository;
        private readonly IRepaymentRepository _repaymentRepository;
        private readonly IRepaymentScheduleRepository _repaymentScheduleRepository;
        private readonly IHttpContextAccessor _httpContextAccessor; 
        private readonly IClientRepository _clientRepository;
        public RepaymentService(ILoanRepository loanRepository, IRepaymentRepository repaymentRepository,  IRepaymentScheduleRepository repaymentScheduleRepository,IHttpContextAccessor httpContextAccessor,IClientRepository clientRepository)
        {
            _loanRepository = loanRepository;
            _repaymentRepository = repaymentRepository;
            _repaymentScheduleRepository = repaymentScheduleRepository;
            _httpContextAccessor = httpContextAccessor;
            _clientRepository = clientRepository;
        }
        public async Task GenerateFixedRepaymentScheduleAsync(string loanId, int durationInMonths)
        {
            var loan = await _loanRepository.GetByIdAsync(loanId);
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

        }
        public async Task GenerateFlexibleRepaymentSchedule(Loan loan)
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

        }
        public async Task<BaseResponse> MakePaymentAsync(MakeRepaymentRequestModel model)
        {
            var user = _httpContextAccessor.HttpContext?.User;
            var userId = user?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userEmail = user?.FindFirst(ClaimTypes.Email)?.Value;
            var loan = await _loanRepository.GetByIdAsync(model.loanId);
            if (loan == null || loan.Repayment == null)
                return new BaseResponse { Message = "Loan not found or no repayment scheduled", Status = false };
            if (loan.Client.Email != userEmail)
            {
                return new BaseResponse
                {
                    Message = "This loan is not for the current logged in user",
                    Status = false
                };
            }
            var repayment = loan.Repayment;

            if (loan.RepaymentType == RepaymentType.Flexible)
            {
                var schedule = repayment.RepaymentSchedules.First();
                
                var amountLeft = schedule.Amount - schedule.AmountPaid;
                if (model.Amount > amountLeft)
                {
                    return new BaseResponse
                    {
                        Message = $"You just have {(int)amountLeft} to pay",
                        Status = false
                    };
                }
                schedule.AmountPaid += model.Amount;
                repayment.AmountPaid += model.Amount;
                loan.TotalPaid += model.Amount;
                if (schedule.AmountPaid == schedule.Amount)
                    schedule.Status = PaymentStatus.Paid;

                if (repayment.AmountPaid == repayment.TotalAmount)
                    repayment.Status = PaymentStatus.Paid;
                if (loan.TotalPaid == loan.TotalAmountToRepay)
                {
                    loan.Status = LoanStatus.Paid;
                }
                await UpdateClientCreditScoreAsync(loan.Id);
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
                if (model.Amount > nextSchedule.Amount)
                    return new BaseResponse { Message = $"Amount paid is greated than expected amount. The expected amount is {(int)nextSchedule.Amount}", Status = false };

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
        public async Task UpdateClientCreditScoreAsync(string loanId)
        {
            var loan = await _loanRepository.GetByIdAsync(loanId);
            if (loan == null) return;

            var client = await _clientRepository.GetByIdAsync(loan.ClientId);
            if (client == null) return;

            if (loan.Status == LoanStatus.Paid &&(DateTime.Now > loan.ApprovalDate.Value.AddMonths(loan.DurationInMonths)))
            {
                client.CreditScore -= 30;
                loan.Status = LoanStatus.Defaulted;
            }
            else if (loan.Status == LoanStatus.Paid && DateTime.Now <= loan.ApprovalDate.Value.AddMonths(loan.DurationInMonths))
            {
                client.CreditScore += 5;
            }
            client.CreditScore = Math.Max(0, Math.Min(100, client.CreditScore));
            await _loanRepository.UpdateAsync(loan);
            await _clientRepository.UpdateAsync(client);
        }
        public async Task<List<RepaymentResponseModel>> GetHistoryByLoanIdAsync(string loanId)
        {
            var repayments = await _repaymentRepository.GetHistoryByLoanIdAsync(loanId);

            if (repayments == null || !repayments.Any())
            {
                return new List<RepaymentResponseModel>();
            }

            foreach (var repayment in repayments)
            {
                foreach (var schedule in repayment.RepaymentSchedules)
                {
                    ApplyPenalty(schedule);
                    await _repaymentScheduleRepository.UpdateAsync(schedule);
                }
            }

            return repayments.Select(r => new RepaymentResponseModel
            {
                RepaymentId = r.Id,
                LoanId = r.LoanId,
                TotalAmount = r.TotalAmount,
                AmountPaid = r.AmountPaid,
                Status = r.Status.ToString(),
                CreatedAt = r.CreatedAt,
                PaymentDate = r.PaymentDate,
                Schedules = r.RepaymentSchedules.Select(s => new RepaymentScheduleDTO
                {
                    ScheduleId = s.Id,
                    DueDate = s.DueDate.ToString("yyyy-MM-dd"),
                    Amount = s.Amount,
                    Penalty = s.Penalty,
                    AmountPaid = s.AmountPaid,
                    Status = s.Status.ToString()
                }).ToList()
            }).ToList();
        }

    }

}
