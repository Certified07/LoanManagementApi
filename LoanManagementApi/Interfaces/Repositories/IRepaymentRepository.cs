using LoanManagementApi.Models.Entities;

namespace LoanManagementApi.Interfaces.Repositories
{
    public interface IRepaymentRepository
    {
        Task<Repayment> CreateAsync(Repayment repayment);
        Task<Repayment> GetByIdAsync(string id);
        Task<List<Repayment>> GetHistoryByLoanIdAsync(string loanId);
        Task<Repayment> UpdateAsync(Repayment repayment);
        Task<bool> DeleteAsync(string id);
        Task<Repayment> GetByLoanId(string loanId);
    }
}
