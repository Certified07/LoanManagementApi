using LoanManagementApi.Interfaces.Repositories;
using LoanManagementApi.Models.Entities;

namespace LoanManagementApi.Implementations.Services
{
    public class PenaltyService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public PenaltyService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using var scope = _scopeFactory.CreateScope();
                var _scheduleRepository = scope.ServiceProvider.GetRequiredService<IRepaymentScheduleRepository>();

                var overdueSchedules = await _scheduleRepository.GetOverdueSchedulesAsync();

                foreach (var schedule in overdueSchedules)
                {
                    var lastDate = schedule.LastPenaltyCalculationDate?.Date ?? schedule.DueDate.Date;
                    var today = DateTime.UtcNow.Date;

                    int overdueDays = (today - lastDate).Days;
                    if (overdueDays <= 0) continue;

                    decimal penaltyToAdd = schedule.Amount * 0.05M * overdueDays;
                    schedule.Penalty += decimal.Round(penaltyToAdd, 2);
                    schedule.LastPenaltyCalculationDate = today;

                    await _scheduleRepository.UpdateAsync(schedule);
                }

                await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
            }
        }

    }
}
