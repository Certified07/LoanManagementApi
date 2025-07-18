

namespace LoanManagementApi.Models.Entities
{
    public class Repayment
    {
        public string Id { get; set; }

        public string LoanId { get; set; }
        public Loan Loan { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<RepaymentSchedule> RepaymentSchedules { get; set; } = new List<RepaymentSchedule>();
    }

}
