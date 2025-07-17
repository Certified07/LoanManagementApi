using LoanManagementApi.Models.Entities;

namespace LoanManagementApi.Repositories.Interfaces
{
    public interface IRepaymentScheduleRepository
    {
        Task<RepaymentSchedule> CreateAsync(RepaymentSchedule schedule);
        Task<List<RepaymentSchedule>> GetScheduleByLoanIdAsync(Guid loanId);
        Task<RepaymentSchedule> UpdateAsync(RepaymentSchedule schedule);
        Task<bool> DeleteAsync(Guid id);
    }
}
