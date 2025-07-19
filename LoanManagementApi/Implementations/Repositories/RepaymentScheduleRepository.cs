using LoanManagementApi.Interfaces.Repositories;
using LoanManagementApi.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace LoanManagementApi.Implementations.Repositories
{
    public class RepaymentScheduleRepository : IRepaymentScheduleRepository
    {
        private readonly MyContext _context;

        public RepaymentScheduleRepository(MyContext context)
        {
            _context = context;
        }

        public async Task<RepaymentSchedule> CreateAsync(RepaymentSchedule schedule)
        {
            _context.RepaymentSchedules.Add(schedule);
            await _context.SaveChangesAsync();
            return schedule;
        }

        public async Task<RepaymentSchedule> UpdateAsync(RepaymentSchedule schedule)
        {
            var existingSchedule = await _context.RepaymentSchedules.FindAsync(schedule.Id);
            if (existingSchedule == null)
            {
                return null;
            }
            existingSchedule.RepaymentId = schedule.RepaymentId;
            existingSchedule.DueDate = schedule.DueDate;
            existingSchedule.Amount = schedule.Amount;
            existingSchedule.Status = schedule.Status;
            existingSchedule.Penalty = schedule.Penalty;
            existingSchedule.PaymentDate = schedule.PaymentDate;

            await _context.SaveChangesAsync();
            return existingSchedule;
        }
        public async Task<RepaymentSchedule?> GetByIdAsync(string repaymentScheduleId)
        {
            return await _context.RepaymentSchedules.Include(x => x.Repayment)
                                                             .ThenInclude(r => r.Loan)
                                                             .FirstOrDefaultAsync(x => x.Id == repaymentScheduleId);

        }
        public async Task<bool> DeleteAsync(string id)
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
