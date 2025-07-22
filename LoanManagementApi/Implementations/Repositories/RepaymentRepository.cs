using LoanManagementApi.Interfaces.Repositories;
using LoanManagementApi.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace LoanManagementApi.Implementations.Repositories
{
    public class RepaymentRepository : IRepaymentRepository
    {
        private readonly MyContext _context;

        public RepaymentRepository(MyContext context)
        {
            _context = context;
        }

        public async Task<Repayment> CreateAsync(Repayment repayment)
        {
            repayment.Id = Guid.NewGuid().ToString();
            repayment.CreatedAt = DateTime.UtcNow;
            _context.Repayments.Add(repayment);
            await _context.SaveChangesAsync();
            return repayment;
        }

        public async Task<Repayment> GetByIdAsync(string id)
        {
            return await _context.Repayments
                .Include(r => r.Loan)
                .ThenInclude(l => l.Client)
                .Include(r => r.RepaymentSchedules)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<List<Repayment>> GetHistoryByLoanIdAsync(string loanId)
        {
            return await _context.Repayments
                .Include(r => r.Loan)
                .ThenInclude(l => l.Client)
                .Include(r => r.RepaymentSchedules)
                .Where(r => r.LoanId == loanId)
                .ToListAsync();
        }
        public async Task<Repayment> GetByLoanId(string loanId)
        {
            return await _context.Repayments.Include(r => r.RepaymentSchedules)
                                                      .FirstOrDefaultAsync(r => r.LoanId == loanId);

        }

        public async Task<Repayment> UpdateAsync(Repayment repayment)
        {
            var existingRepayment = await _context.Repayments.FindAsync(repayment.Id);
            if (existingRepayment == null)
            {
                return null;
            }

            existingRepayment.LoanId = repayment.LoanId;
            existingRepayment.TotalAmount = repayment.TotalAmount;
            existingRepayment.PaymentDate = repayment.PaymentDate;
            existingRepayment.Status = repayment.Status;
            existingRepayment.Penalty = repayment.Penalty;

            await _context.SaveChangesAsync();
            return existingRepayment;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var repayment = await _context.Repayments.FindAsync(id);
            if (repayment == null)
            {
                return false;
            }

            _context.Repayments.Remove(repayment);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
