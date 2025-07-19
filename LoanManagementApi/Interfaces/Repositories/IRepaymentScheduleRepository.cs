using LoanManagementApi.Models.Entities;

namespace LoanManagementApi.Interfaces.Repositories
{
    public interface IRepaymentScheduleRepository
    {
        Task<RepaymentSchedule> CreateAsync(RepaymentSchedule schedule);
        Task<RepaymentSchedule> UpdateAsync(RepaymentSchedule schedule);
        Task<RepaymentSchedule?> GetByIdAsync(string repaymentScheduleId);
        Task<bool> DeleteAsync(string id);
    }
}
