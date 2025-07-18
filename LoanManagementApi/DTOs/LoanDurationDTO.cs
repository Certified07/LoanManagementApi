namespace LoanManagementApi.DTOs
{
    public class LoanDurationDTO
    {
        public string Id { get; set; }

        public decimal MinAmount { get; set; }
        public decimal MaxAmount { get; set; }

        public int MaxDurationInMonths { get; set; }
    }
}
