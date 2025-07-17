using LoanManagementApi.Models.Entities;

namespace LoanManagementApi.Repositories.Interfaces
{
    public interface IRepaymentRepository
    {
        Task<Repayment> CreateAsync(Repayment repayment);
        Task<Repayment> GetByIdAsync(Guid id);
        Task<List<Repayment>> GetHistoryByLoanIdAsync(Guid loanId);
        Task<Repayment> UpdateAsync(Repayment repayment);
        Task<bool> DeleteAsync(Guid id);
    }
}
