using LoanManagementApi.Interfaces.Repositories;
using LoanManagementApi.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace LoanManagementApi.Implementations.Repositories
{
    public class LoanDurationRuleRepository : ILoanDurationRuleRepository
    {
        private readonly MyContext _context;

        public LoanDurationRuleRepository(MyContext context)
        {
            _context = context;
        }
        public async Task<LoanDurationRule> FindByAmountAsync(decimal amount)
        {
            return await _context.LoanDurationRules
                .FirstOrDefaultAsync(r => amount >= r.MinAmount && amount <= r.MaxAmount);
        }

        public async Task<List<LoanDurationRule>> GetAllAsync() => await _context.LoanDurationRules.ToListAsync();

        public async Task AddAsync(LoanDurationRule rule)
        {
            _context.LoanDurationRules.Add(rule);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(LoanDurationRule rule)
        {
            _context.LoanDurationRules.Update(rule);
            await _context.SaveChangesAsync();
        }

        public async Task<LoanDurationRule> GetByIdAsync(string id)
        {
            return await _context.LoanDurationRules.FindAsync(id);
        }
        public async Task<bool> IsOverlappingRuleAsync(decimal minAmount, decimal maxAmount)
        {
            return await _context.LoanDurationRules.AnyAsync(rule =>
                (minAmount <= rule.MaxAmount && maxAmount >= rule.MinAmount)
            );
        }
    }
}
