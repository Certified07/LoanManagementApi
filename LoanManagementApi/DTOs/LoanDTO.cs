namespace LoanManagementApi.DTOs
{
    public class LoanDTO
    {
        public string Id { get; set; }
        public decimal Amount { get; set; }
        public decimal Balance { get; set; }
        public string Status { get; set; }
        public DateTime ApprovedAt { get; set; } 
        public DateTime? DueDate { get; set; }
    }
}
