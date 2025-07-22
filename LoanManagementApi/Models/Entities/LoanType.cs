namespace LoanManagementApi.Models.Entities
{
    public class LoanType
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public decimal MaxAmount { get; set; }

        public int MaxDurationInMonths { get; set; }

        public ICollection<Loan> Loans { get; set; } = [];
    }
}
