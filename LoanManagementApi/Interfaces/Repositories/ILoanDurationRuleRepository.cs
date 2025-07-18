using LoanManagementApi.Models.Entities;

namespace LoanManagementApi.Interfaces.Repositories
{
    public interface ILoanDurationRuleRepository
    {
        Task AddAsync(LoanDurationRule rule);
        Task<LoanDurationRule> FindByAmountAsync(decimal amount);
        Task<List<LoanDurationRule>> GetAllAsync();
        Task<LoanDurationRule> GetByIdAsync(string id);
        Task UpdateAsync(LoanDurationRule rule);
        Task<bool> IsOverlappingRuleAsync(decimal minAmount, decimal maxAmount);
    }
}