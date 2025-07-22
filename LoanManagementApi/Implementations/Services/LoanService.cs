using LoanManagementApi.DTOs;
using LoanManagementApi.Implementations.Repositories;
using LoanManagementApi.Interfaces.Repositories;
using LoanManagementApi.Interfaces.Services;
using LoanManagementApi.Models.Entities;
using LoanManagementApi.Models.Enums;
using LoanManagementApi.RequestModel;
using LoanManagementApi.ResponseModel;

namespace LoanManagementApi.Implementations.Services
{
    public class LoanService : ILoanService
    {
        private readonly ILoanRepository _loanRepository;
        private readonly IClientRepository _clientRepository;
        private readonly IRepaymentService _repaymentService;
        private readonly ILoanTypeRepository _loanTypeRepository;
        public LoanService(ILoanRepository loanRepository, IClientRepository clientRepository, IRepaymentService repaymentService,ILoanTypeRepository loanTypeRepository)
        {
            _loanRepository = loanRepository;
            _clientRepository = clientRepository;
            _repaymentService = repaymentService;
            _loanTypeRepository = loanTypeRepository;
        }
        public async Task<BaseResponse> ApplyAsync(LoanRequestModel model)
        {
            var client = await _clientRepository.GetByIdAsync(model.ClientId);
            if (client == null)
                return new BaseResponse
                {
                    Message = "Client not found",
                    Status = false
                };
            var loantype = await _loanTypeRepository.GetByIdAsync(model.LoanId);
            
            var existingLoans = await _loanRepository.GetByClientIdAsync(model.ClientId);
            var amountLoaned = existingLoans.Sum(x => x.PrincipalAmount);
            var maximumLoan = ((client.CreditScore / 100.0M) * client.Income) - amountLoaned;

            var allowedAmount = Math.Min(maximumLoan, loantype.MaxAmount);

            if (model.Amount > allowedAmount)
            {
                return new BaseResponse
                {
                    Message = $"The maximum amount you can loan is {allowedAmount}",
                    Status = false
                };
            }

            var loan = new Loan
            {
                Id = Guid.NewGuid().ToString(),
                PrincipalAmount = model.Amount,
                InterestRate = 5M,
                TotalAmountToRepay = model.Amount + (model.Amount * 5M / 100),
                ClientId = model.ClientId,
                Status = LoanStatus.Pending,
                ApplicationDate = DateTime.Now,
                DurationInMonths = model.DurationInMonths,
                LoanTypeId = model.LoanId,
                IsCompleted = false,
                RepaymentType = loantype.RepaymentType
            };


            await _loanRepository.CreateAsync(loan);
            return new BaseResponse
            {
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
            if(response.Count() == 0)
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
    }
}
