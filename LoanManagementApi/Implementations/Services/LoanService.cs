using LoanManagementApi.DTOs;
using LoanManagementApi.Implementations.Repositories;
using LoanManagementApi.Interfaces.Repositories;
using LoanManagementApi.Interfaces.Services;
using LoanManagementApi.Models.Entities;
using LoanManagementApi.Models.Enums;
using LoanManagementApi.RequestModel;
using LoanManagementApi.ResponseModel;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace LoanManagementApi.Implementations.Services
{
    public class LoanService : ILoanService
    {
        private readonly ILoanRepository _loanRepository;
        private readonly IClientRepository _clientRepository;
        private readonly IRepaymentService _repaymentService;
        private readonly IUserRepository _userRepository;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly ILoanTypeRepository _loanTypeRepository;
        public LoanService(ILoanRepository loanRepository, IClientRepository clientRepository, IRepaymentService repaymentService, IUserRepository userRepository, IHttpContextAccessor contextAccessor, ILoanTypeRepository loanTypeRepository)
        {
            _loanRepository = loanRepository;
            _clientRepository = clientRepository;
            _repaymentService = repaymentService;
            _userRepository = userRepository;
            _contextAccessor = contextAccessor;
            _loanTypeRepository = loanTypeRepository;
        }

        public async Task<ApplyLoanResponseModel> ApplyAsync(LoanRequestModel model)
        {
            var user = _contextAccessor.HttpContext?.User;
            var userId = user?.FindFirstValue(ClaimTypes.NameIdentifier);
            var userDetails = await _userRepository.GetByIdAsync(userId);
            var client =await _clientRepository.GetByEmailAsync(userDetails.Email);
            if (client == null)
                return new ApplyLoanResponseModel
                {
                    Message = "Client not found",
                    Status = false
                };
            var loantype = await _loanTypeRepository.GetByNameAsync(model.LoanType);
            if (loantype == null)
            {
                return new ApplyLoanResponseModel
                {
                    Message = "We don't have this loan type for now. Try again later",
                    Status = false
                };
            }
            var existingLoans = await _loanRepository.GetByClientIdAsync(client.Id);
            var amountLoaned = existingLoans.Sum(x => x.PrincipalAmount);
            var maximumLoan = ((client.CreditScore / 100.0M) * client.Income) - amountLoaned;

            var allowedAmount = Math.Min(maximumLoan, loantype.MaxAmount);

            if (model.Amount > allowedAmount)
            {
                return new ApplyLoanResponseModel
                {
                    Message = $"The maximum amount you can loan is {(int)allowedAmount}",
                    Status = false
                };
            }
            if (model.DurationInMonths > loantype.MaxDurationInMonths)
            {
                return new ApplyLoanResponseModel
                {
                    Message = $"You can only borrow the loan for {loantype.MaxDurationInMonths} months",
                    Status = false
                };
            }
            if (model.DurationInMonths <= 0)
            {
                return new ApplyLoanResponseModel
                {
                    Message = "You can't borrow for less than a month",
                    Status = false
                };
            }
            var loan = new Loan
            {
                Id = Guid.NewGuid().ToString(),
                PrincipalAmount = model.Amount,
                InterestRate = 5M,
                TotalAmountToRepay = model.Amount + (model.Amount * 5M / 100),
                ClientId = client.Id,
                Status = LoanStatus.Pending,
                ApplicationDate = DateTime.Now,
                DurationInMonths = model.DurationInMonths,
                LoanTypeId = loantype.Id,
                IsCompleted = false,
                RepaymentType = loantype.RepaymentType
            };


            await _loanRepository.CreateAsync(loan);
            return new ApplyLoanResponseModel
            {
                LoanId = loan.Id,
                Message = "Loan application submitted successfully",
                Status = true
            };
        }
        public async Task<BaseResponse> ApproveAsync(string loanId)
        {
            var loan = await _loanRepository.GetByIdAsync(loanId);
            if (loan == null)
                return new BaseResponse
                {
                    Message = "Loan not found",
                    Status = false
                };

            if (loan.Status != LoanStatus.Pending)
                return new BaseResponse
                {
                    Message = "Loan already processed",
                    Status = false
                };

            loan.Status = LoanStatus.Approved;
            loan.ApprovalDate = DateTime.Now;
            if (loan.RepaymentType == RepaymentType.Flexible)
            {
                await _repaymentService.GenerateFlexibleRepaymentSchedule(loan);
                loan.Status = LoanStatus.Active;
            }
            else
            {
                await _repaymentService.GenerateFixedRepaymentScheduleAsync(loan.Id, loan.DurationInMonths);
                loan.Status = LoanStatus.Active;
            }
            await _loanRepository.UpdateAsync(loan);
            await _repaymentService.GetRepaymentSummaryAsync(loanId);
            return new BaseResponse
            {
                Message = "Loan approved successfully",
                Status = true
            };
        }
        public async Task<BaseResponse> RejectAsync(string loanId)
        {
            var loan = await _loanRepository.GetByIdAsync(loanId);
            if (loan == null)
                return new BaseResponse
                {
                    Message = "Loan not found",
                    Status = false
                };

            if (loan.Status != LoanStatus.Pending)
                return new BaseResponse
                {
                    Message = "Loan already processed",
                    Status = false
                };


            loan.Status = LoanStatus.Rejected;
            await _loanRepository.UpdateAsync(loan);
            return new BaseResponse
            {
                Message = "Loan rejected successfully",
                Status = true
            };
        }
        public async Task<LoansResponseModel> GetDefaultedLoans()
        {
            var response = await _loanRepository.GetDefaultsAsync();
            if (response.Count() == 0)
            {
                return new LoansResponseModel
                {
                    Message = "No defaulted loans",
                    Status = false
                };
            }
            return new LoansResponseModel
            {
                Data = response.Select(x => new LoanDTO
                {
                    Id = x.Id,
                    Amount = x.TotalAmountToRepay,
                    Balance = x.TotalAmountToRepay - x.TotalPaid,
                    Status = x.Status.ToString(),
                    ApprovedAt = x.ApprovalDate.Value,
                    DueDate = x.ApprovalDate.Value.AddMonths(x.DurationInMonths)

                }).ToList(),
                Message = "Sucessfully returned",
                Status = true
            };
        }
        public async Task<LoansResponseModel> GetOutstandingLoans()
        {
            var response = await _loanRepository.GetOutstandingAsync();
            if (response.Count() == 0)
            {
                return new LoansResponseModel
                {
                    Message = "No outstanding loans",
                    Status = false
                };
            }
            return new LoansResponseModel
            {
                Data = response.Select(x => new LoanDTO
                {
                    Id = x.Id,
                    Amount = x.TotalAmountToRepay,
                    Balance = x.TotalAmountToRepay - x.TotalPaid,
                    Status = x.Status.ToString(),
                    ApprovedAt = x.ApprovalDate.Value,
                    DueDate = x.ApprovalDate.Value.AddMonths(x.DurationInMonths)

                }).ToList(),
                Message = "Sucessfully returned",
                Status = true
            };
        }
        public async Task<LoansResponseModel> GetPaidLoans()
        {
            var response = await _loanRepository.GetPaidAsync();
            if (response.Count() == 0)
            {
                return new LoansResponseModel
                {
                    Message = "No paid loans",
                    Status = false
                };
            }
            return new LoansResponseModel
            {
                Data = response.Select(x => new LoanDTO
                {
                    Id = x.Id,
                    Amount = x.TotalAmountToRepay,
                    Balance = x.TotalAmountToRepay - x.TotalPaid,
                    Status = x.Status.ToString(),
                    ApprovedAt = x.ApprovalDate.Value,
                    DueDate = x.ApprovalDate.Value.AddMonths(x.DurationInMonths)

                }).ToList(),
                Message = "Sucessfully returned",
                Status = true
            };
        }
        public async Task<GeneralLoanResponseModel> GetLoanDetailsForCurrentUser()
        {
            var user = _contextAccessor.HttpContext?.User;
            var userId = user?.FindFirstValue(ClaimTypes.NameIdentifier);
            string role = user?.FindFirstValue(ClaimTypes.Role);
            var loans = await _loanRepository.GetAllAsync();

            if (role != "Admin")
            {
                loans = loans.Where(l => l.Client.UserId == userId).ToList();
            }

            var result = loans.Select(l => new GeneralLoanDTO
            {
                LoanId = l.Id,
                Username = l.Client.User.UserName,
                Status = l.Status.ToString(),
                Schedules = l.Repayment?.RepaymentSchedules?.Select(s => new RepaymentScheduleDTO
                {
                    Id = s.Id,
                    RepaymentId = s.Id,
                    Amount = s.Amount,
                    DueDate = s.DueDate.ToString("yyyy-MM-dd"),
                    AmountPaid = s.AmountPaid,
                    Penalty = s.Penalty,
                    Status = s.Status.ToString(),

                }).ToList() ?? new List<RepaymentScheduleDTO>()

            }).ToList();

            return new GeneralLoanResponseModel
            {
                Data = result,
                Message = "Sucessfully returned",
                Status = true
            };
        }



    }
}
