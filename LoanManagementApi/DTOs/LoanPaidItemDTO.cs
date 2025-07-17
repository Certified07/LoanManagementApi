namespace LoanManagementApi.DTOs
{
    public class LoanPaidItemDTO
    {
        public Guid Id { get; set; }
        public Guid ClientId { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaidDate { get; set; }
    }
}
