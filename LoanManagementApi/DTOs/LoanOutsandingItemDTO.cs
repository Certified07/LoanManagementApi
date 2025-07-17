namespace LoanManagementApi.DTOs
{
    public class LoanOutstandingItemDTO
    {
        public Guid Id { get; set; }
        public Guid ClientId { get; set; }
        public decimal Amount { get; set; }
        public decimal OutstandingBalance { get; set; }
    }
}
