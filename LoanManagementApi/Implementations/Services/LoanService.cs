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
        private readonly ILoanDurationRuleRepository _loanDurationRuleRepository;
        private readonly ILoanRepository _loanRepository;
        private readonly IClientRepository _clientRepository;

        public LoanService(ILoanDurationRuleRepository loanDurationRuleRepository, ILoanRepository loanRepository, IClientRepository clientRepository)
        {
            _loanDurationRuleRepository = loanDurationRuleRepository;
            _loanRepository = loanRepository;
            _clientRepository = clientRepository;
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

            if (client.CreditScore < 500 || client.Income < model.Amount / 2)
                return new BaseResponse
                {
                    Message = "Client does not meet the credit score or income requirements",
                    Status = false
                };

            var durationRule = await _loanDurationRuleRepository.FindByAmountAsync(model.Amount);
            if (durationRule == null)
                return new BaseResponse
                {
                    Message = "You can't loan this amount",
                    Status = false
                };

            var loan = new Loan
            {
                Id = Guid.NewGuid().ToString(),
                Amount = model.Amount,
                ClientId = model.ClientId,
                Status = LoanStatus.Pending,
                ApplicationDate = DateTime.Now,
                DurationInMonths = durationRule.MaxDurationInMonths,
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

            await _loanRepository.UpdateAsync(loan);
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
        private void AdjustCreditScore(Client client, int change)
        {
            client.CreditScore += change;
            if (client.CreditScore < 0) client.CreditScore = 0;
            if (client.CreditScore > 850) client.CreditScore = 850;
        }
        public async Task<BaseResponse> UpdateCreditScoreOnRepayment(string loanId, bool isDefaulted)
        {
            var loan = await _loanRepository.GetByIdAsync(loanId);
            if (loan == null)
                return new BaseResponse
                {
                    Message = "Loan not found",
                    Status = false
                };

            var client = await _clientRepository.GetByIdAsync(loan.ClientId);
            if (client == null)
                return new BaseResponse
                {
                    Message = "Client not found",
                    Status = false
                };

            if (isDefaulted)
                AdjustCreditScore(client, -40);
            else
                AdjustCreditScore(client, 30);

            await _clientRepository.UpdateAsync(client);
            return new BaseResponse
            {
                Message = "Credit score updated successfully",
                Status = true
            };
        }
    }
}
