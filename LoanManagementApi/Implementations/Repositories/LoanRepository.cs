using LoanManagementApi.Interfaces.Repositories;
using LoanManagementApi.Models.Entities;
using LoanManagementApi.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace LoanManagementApi.Implementations.Repositories
{
    public class LoanRepository : ILoanRepository
    {
        private readonly MyContext _context;

        public LoanRepository(MyContext context)
        {
            _context = context;
        }

        public async Task<Loan> CreateAsync(Loan loan)
        {
            _context.Loans.Add(loan);
            await _context.SaveChangesAsync();
            return loan;
        }

        public async Task<Loan> GetByIdAsync(string id)
        {
            return await _context.Loans.Include(x => x.Repayment)
                                           .ThenInclude(x => x.RepaymentSchedules)
                                           .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<Loan>> GetAllAsync()
        {
            return await _context.Loans.Include(l => l.Client).ToListAsync();
        }

        public async Task<Loan> UpdateAsync(Loan loan)
        {
            var existingLoan = await _context.Loans.FindAsync(loan.Id);
            if (existingLoan == null)
            {
                return null;
            }

            existingLoan.ClientId = loan.ClientId;
            existingLoan.InterestRate = loan.InterestRate;
            existingLoan.DurationInMonths = loan.DurationInMonths;
            existingLoan.Status = loan.Status;
            existingLoan.ApplicationDate = loan.ApplicationDate;
            existingLoan.ApprovalDate = loan.ApprovalDate;

            await _context.SaveChangesAsync();
            return existingLoan;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var loan = await _context.Loans.FindAsync(id);
            if (loan == null)
            {
                return false;
            }

            _context.Loans.Remove(loan);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Loan>> GetByClientIdAsync(string clientId)
        {
            return await _context.Loans
                .Include(l => l.Client)
                .Where(l => l.ClientId == clientId)
                .ToListAsync();
        }

        public async Task<List<Loan>> GetDefaultsAsync()
        {
            return await _context.Loans
                .Include(l => l.Client)
                .Where(l => l.Status == LoanStatus.Defaulted)
                .ToListAsync();
        }

        public async Task<List<Loan>> GetPaidAsync()
        {
            return await _context.Loans
                .Include(l => l.Client)
                .Where(l => l.Status == LoanStatus.Paid)
                .ToListAsync();
        }

        public async Task<List<Loan>> GetOutstandingAsync()
        {
            return await _context.Loans
                .Include(l => l.Client)
                .Where(l => l.Status == LoanStatus.Active || l.Status == LoanStatus.Approved)
                .ToListAsync();
        }
    }

}
