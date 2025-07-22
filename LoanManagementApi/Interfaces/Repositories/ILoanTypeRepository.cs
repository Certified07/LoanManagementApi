using LoanManagementApi.Models.Entities;

namespace LoanManagementApi.Interfaces.Repositories
{
    public interface ILoanTypeRepository
    {
        Task AddAsync(LoanType loanType);
        Task DeleteAsync(LoanType loanType);
        Task<bool> ExistsAsync(int id);
        Task<List<LoanType>> GetAllAsync();
        Task<LoanType?> GetByIdAsync(int id);
        Task<LoanType?> GetByNameAsync(string name);
        Task UpdateAsync(LoanType loanType);
    }
}