using LoanManagementApi.Interfaces.Repositories;
using LoanManagementApi.Models.Entities;
using System.Data.Entity;

namespace LoanManagementApi.Implementations.Repositories
{
    public class LoanTypeRepository : ILoanTypeRepository
    {
        private readonly MyContext _context;

        public LoanTypeRepository(MyContext context)
        {
            _context = context;
        }
        public async Task<List<LoanType>> GetAllAsync()
        {
            return await _context.LoanTypes.ToListAsync();
        }
        public async Task<LoanType?> GetByIdAsync(int id)
        {
            return await _context.LoanTypes.FindAsync(id);
        }
        public async Task<LoanType?> GetByNameAsync(string name)
        {
            return await _context.LoanTypes.FirstOrDefaultAsync(l => l.Name == name);
        }

        public async Task AddAsync(LoanType loanType)
        {
            await _context.LoanTypes.AddAsync(loanType);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(LoanType loanType)
        {
            _context.LoanTypes.Update(loanType);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(LoanType loanType)
        {
            _context.LoanTypes.Remove(loanType);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.LoanTypes.AnyAsync(l => l.Id == id);
        }

    }
}
