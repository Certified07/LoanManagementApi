using LoanManagementApi.Models.Entities;

namespace LoanManagementApi.Interfaces.Repositories
{
    public interface ILoanRepository
    {
        Task<Loan> CreateAsync(Loan loan);
        Task<Loan> GetByIdAsync(Guid id);
        Task<List<Loan>> GetAllAsync();
        Task<Loan> UpdateAsync(Loan loan);
        Task<bool> DeleteAsync(Guid id);
        Task<List<Loan>> GetByClientIdAsync(Guid clientId);
        Task<List<Loan>> GetDefaultsAsync();
        Task<List<Loan>> GetPaidAsync();
        Task<List<Loan>> GetOutstandingAsync();
    }
}
