using LoanManagementApi.Data;
using LoanManagementApi.Models.Entities;
using LoanManagementApi.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LoanManagementApi.Repositories.Implementations
{
    public class RepaymentScheduleRepository : IRepaymentScheduleRepository
    {
        private readonly LoanManagementContext _context;

        public RepaymentScheduleRepository(LoanManagementContext context)
        {
            _context = context;
        }

        public async Task<RepaymentSchedule> CreateAsync(RepaymentSchedule schedule)
        {
            schedule.Id = Guid.NewGuid();
            schedule.CreatedAt = DateTime.UtcNow;
            _context.RepaymentSchedules.Add(schedule);
            await _context.SaveChangesAsync();
            return schedule;
        }

        public async Task<List<RepaymentSchedule>> GetScheduleByLoanIdAsync(Guid loanId)
        {
            return await _context.RepaymentSchedules
                .Include(rs => rs.Loan)
                .ThenInclude(l => l.Client)
                .Include(rs => rs.Repayment)
                .Where(rs => rs.LoanId == loanId)
                .ToListAsync();
        }

        public async Task<RepaymentSchedule> UpdateAsync(RepaymentSchedule schedule)
        {
            var existingSchedule = await _context.RepaymentSchedules.FindAsync(schedule.Id);
            if (existingSchedule == null)
            {
                return null;
            }

            existingSchedule.LoanId = schedule.LoanId;
            existingSchedule.RepaymentId = schedule.RepaymentId;
            existingSchedule.InstallmentNumber = schedule.InstallmentNumber;
            existingSchedule.DueDate = schedule.DueDate;
            existingSchedule.AmountDue = schedule.AmountDue;
            existingSchedule.Status = schedule.Status;

            await _context.SaveChangesAsync();
            return existingSchedule;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var schedule = await _context.RepaymentSchedules.FindAsync(id);
            if (schedule == null)
            {
                return false;
            }

            _context.RepaymentSchedules.Remove(schedule);
            await _context.SaveChangesAsync();
            return true;
        }
    }

}
